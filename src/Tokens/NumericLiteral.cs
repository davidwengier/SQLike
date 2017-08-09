using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// A numeric literal
	/// </summary>
	public class NumericLiteral : ValueToken
	{
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public object Number { get; set; }
	}
}