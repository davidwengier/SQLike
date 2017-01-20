using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// A date literal
	/// </summary>
	public class DateLiteral : Token
	{
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public DateTime Value { get; set; }
	}
}