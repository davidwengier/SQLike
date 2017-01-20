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

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return new string(Character, End - Start);
		}
	}
}