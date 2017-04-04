 param (
	[string]$ReferencesPath = $null,
	[string]$ApiKey = $null,
	[string]$NugetServer = "https://nuget.starrez.com",
	[string]$CodeSigningCertificate = $null,
	[string]$CodeSigningCertificatePassword = $null,
	[string]$PublishBranchToNuget = $null,
	[string]$BranchName = $null
 )

$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
Write-Host "Script root is $PSScriptRoot"
$ToolPath = Join-Path $PSScriptRoot "build"
if (!(Test-Path $ToolPath)) {
	Write-Host "Creating tools directory..."
	New-Item -Path $ToolPath -Type directory | out-null
}
Write-Host "Tools directory is $ToolPath"

$NugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
# Make sure nuget.exe exists.
$NugetPath = Join-Path $ToolPath "nuget.exe"
if (!(Test-Path $NugetPath)) {
	Write-Host "Downloading NuGet.exe..."
	(New-Object System.Net.WebClient).DownloadFile($NugetUrl, $NugetPath);
}

Write-Host "Nuget is at $NugetPath"

## Install/Update Git Versioning
Write-Host "Getting Nerdbank.GitVersioning"

Invoke-Expression "&`"$NugetPath`" install NerdBank.GitVersioning -Source https://api.nuget.org/v3/index.json -OutputDirectory `"$ToolPath`""
## find the most recent version of get-version.ps1 from NerdBank.GitVersioning
$versionPs1 = Get-ChildItem -Path $ToolPath -Recurse -Filter "Get-Version.ps1" | Sort-Object -Property "FullName" -Descending | Select-Object -Property "FullName" -First 1
Write-Host "Git Versioning is in $versionPs1"

## Install/Update Xunit Console Runner
Write-Host "Getting xunit.runner.console"

Invoke-Expression "&`"$NugetPath`" install xunit.runner.console -Source https://api.nuget.org/v3/index.json -OutputDirectory `"$ToolPath`""
## find the most recent version of xunit.console.exe from xunit.runner.console
$XUnitExe = Get-ChildItem -Path $ToolPath -Recurse -Filter "xunit.console.exe" | Sort-Object -Property "FullName" -Descending | Select-Object -Property "FullName" -First 1 | foreach {$_.FullName}
Write-Host "Xunit console runner is in $XUnitExe"

if ($CodeSigningCertificate) {
	## Install/Update Code Signer
	Write-Host "Getting CodeSigner"
	Invoke-Expression "&`"$NugetPath`" install CodeSigner -Source $NugetServer/nuget -OutputDirectory `"$ToolPath`""
	## find the most recent version of code Signer
	$codeSigner = (Get-ChildItem -Path $ToolPath -Recurse -Filter "CodeSigner.exe" | Sort-Object -Property "FullName" -Descending | Select-Object -Property "FullName" -First 1).FullName
}

## Call git version to get the version
$vers = (Invoke-Expression -Command $versionPs1).FullName
Write-Host "Got version info:"
$vers

## Get vars
$RegistryInfo = $(Get-ChildItem -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions\" | 
    Where { $_.Name -match '\\\d+.\d+$' } | 
    Sort-Object -property  @{Expression={[System.Convert]::ToDecimal($_.Name.Substring($_.Name.LastIndexOf("\") + 1).Replace(".",$_decSep).Replace(",",$_decSep))}} -Descending |
    Select-Object -First 1)
$MSBuildToolsVersion = $RegistryInfo.PSChildName
$MSBuildPath = $RegistryInfo.GetValue("MSBuildToolsPath")
$MSBuildPath = "$MSBuildPath\msbuild.exe"
Write-Host "Using msbuild in $MSBuildPath"
$SolutionName = (Get-ChildItem -Path $PSScriptRoot -Filter "*.sln" | Select-Object -First 1).BaseName
$Version = $vers.SimpleVersion.ToString()
$MajorMinorVersion = $vers.MajorMinorVersion.ToString()

Write-Host "Building $SolutionName"

Write-Host "Running nuget restore"
Invoke-Expression "&`"$NugetPath`" restore $SolutionName.sln  -Source $NugetServer/nuget -Source https://api.nuget.org/v3/index.json"

$MSBuildArguments = "$SolutionName.sln /p:Optimize=true /p:Configuration=Release /p:DebugSymbols=true /p:DebugType=full /tv:$MSBuildToolsVersion"
Write-Host "Running msbuild: $MSBuildArguments"
Invoke-Expression "&`"$MSBuildPath`" $MSBuildArguments"

if (-not (Test-Path $XUnitExe)) {
	Write-Host "Could not find XUnit executable at $XunitExe"
	exit 1
}
if (Test-Path "$PSScriptRoot\tests") {
	if (Test-Path "TestResult.xml") {
		Remove-Item "TestResult.xml"
	}
	Write-Host "Running XUnit tests"
	$process = (Start-Process $XUnitExe -ArgumentList "$PSScriptRoot\tests\bin\Release\$SolutionName.Tests.dll -nunit TestResult.xml" -Wait -NoNewWindow -PassThru)
	if ($process.ExitCode -ne 0) {
		Write-Host "Tests failed. See TestResult.xml for the answers"
		exit 1
	}
}

if ($CodeSigningCertificate) {
	Write-Host "Signing your code with $CodeSigningCertificate"
	Invoke-Expression "&`"$codeSigner`" /wp=$PSScriptRoot\src\bin\Release /fn=$SolutionName.dll /cp=$CodeSigningCertificate /p=$CodeSigningCertificatePassword"
}

# Only do nuget on master, when we have an api key, or when we're explicitly told to
if ($PublishBranchToNuget -eq "true" -or $BranchName -eq "master") {
	Write-Host "Nugety goodness"

	Write-Host "Deleting old nupkgs"
	Get-ChildItem -Filter "*.nupkg" | Remove-Item

	$NugetSuffix = ""
	if ($BranchName -ne "master")
	{
		$NugetSuffix = "-Suffix $($BranchName -replace '[^a-z0-9]','-')"
	}
	Write-Host "Running $NugetPath pack $SolutionName.nuspec -Version $version $NugetSuffix"
	Invoke-Expression "&`"$NugetPath`" pack $SolutionName.nuspec -Version $version $NugetSuffix"

	if ($ApiKey) {
		Write-Host "Running nuget push to $NugetServer"
		Invoke-Expression "&`"$NugetPath`" push *.nupkg -ApiKey $ApiKey -Source $NugetServer/"
	}
}

#only copy to internal references if we're on master
if ($ReferencesPath -and $BranchName -eq "master") {
	Write-Host "Publishing to internal references for NANT"
	$DeploymentFolder = "$ReferencesPath\$SolutionName\$MajorMinorVersion"
	if (Test-Path $DeploymentFolder) {
		Write-Host "Removing old files from $DeploymentFolder"
		Remove-Item $DeploymentFolder -Recurse -Force
	}
	Write-Host "Creating $DeploymentFolder"
	New-Item $DeploymentFolder -Force -ItemType Directory

	Write-Host "Deploying $PSScriptRoot\src\bin\Release\* for internal use to $DeploymentFolder"
	Get-ChildItem -Path "$PSScriptRoot\src\bin\Release" -Filter "$SolutionName.dll" | Copy-Item -Destination $DeploymentFolder -Force
	Get-ChildItem -Path "$PSScriptRoot\src\bin\Release" -Filter "$SolutionName.exe" | Copy-Item -Destination $DeploymentFolder -Force
	Get-ChildItem -Path "$PSScriptRoot\src\bin\Release" -Filter "$SolutionName.pdb" | Copy-Item -Destination $DeploymentFolder -Force
	Get-ChildItem -Path "$PSScriptRoot\src\bin\Release" -Filter "$SolutionName.xml" | Copy-Item -Destination $DeploymentFolder -Force
}