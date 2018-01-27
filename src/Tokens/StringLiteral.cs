using System;
using System.Linq;

namespace SQLike.Tokens
{
	/// <summary>
	/// A string literal
	/// </summary>
	public class StringLiteral : ValueToken
	{
		/// <summary>
		/// Gets the delimeter.
		/// </summary>
		public char Delimeter { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (this.Delimeter > 0)
			{
				return this.Delimeter + this.Value + this.Delimeter;
			}
			return this.Value;
		}
	}
}