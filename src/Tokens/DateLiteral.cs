using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// A date literal
	/// </summary>
	public class DateLiteral : ValueToken
	{
		/// <summary>
		/// Gets the date time.
		/// </summary>
		/// <value>
		/// The date time.
		/// </value>
		public DateTime DateTime { get; set; }
	}
}