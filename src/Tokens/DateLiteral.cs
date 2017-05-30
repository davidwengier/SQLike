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
		public string Value { get; set; }

		/// <summary>
		/// Gets the date time.
		/// </summary>
		/// <value>
		/// The date time.
		/// </value>
		public DateTime DateTime { get; internal set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}