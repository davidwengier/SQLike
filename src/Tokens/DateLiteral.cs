using System;
using System.Linq;

namespace SQLike.Tokens
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