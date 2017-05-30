using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// Literally a comma
	/// </summary>
	public class Comma : Token
	{
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return ",";
		}
	}
}