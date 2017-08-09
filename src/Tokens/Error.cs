using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// An error token
	/// </summary>
	/// <seealso cref="StarNet.StarQL.Tokens.Token" />
	public class Error : ValueToken
	{
		/// <summary>
		/// Gets the error.
		/// </summary>
		public string Message { get; set; }
	}
}