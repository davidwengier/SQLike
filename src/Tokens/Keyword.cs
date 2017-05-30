using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// An identifier
	/// </summary>
	public class Keyword : Token
	{
		/// <summary>
		/// Gets the value.
		/// </summary>
		public string Value { get; internal set; }

		/// <summary>
		/// Gets the kind.
		/// </summary>
		public KeywordKind Kind { get; internal set; }

		internal Keyword()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyword"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="kind">The kind.</param>
		public Keyword(string value, KeywordKind kind)
		{
			this.Value = value;
			this.Kind = kind;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Value;
		}
	}
}