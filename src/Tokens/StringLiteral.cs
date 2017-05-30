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