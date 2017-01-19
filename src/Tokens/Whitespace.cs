using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// Whitespace
	/// </summary>
	public class Whitespace : Token
	{
		/// <summary>
		/// Which whitespace character this is
		/// </summary>
		public char Character { get; internal set; }
	}
}