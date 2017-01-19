using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// A string literal
	/// </summary>
	public class StringLiteral : Token
	{
		/// <summary>
		/// Gets the delimeter.
		/// </summary>
		public char Delimeter { get; internal set; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		public string Value { get; internal set; }
	}
}