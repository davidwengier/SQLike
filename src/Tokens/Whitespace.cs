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

		internal Whitespace()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Whitespace"/> class.
		/// </summary>
		/// <param name="character">The character.</param>
		public Whitespace(char character)
		{
			this.Character = character;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return new string(this.Character, this.End - this.Start + 1);
		}
	}
}