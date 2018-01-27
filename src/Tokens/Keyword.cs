using System;
using System.Linq;

namespace SQLike.Tokens
{
	/// <summary>
	/// An identifier
	/// </summary>
	public class Keyword : ValueToken
	{
		/// <summary>
		/// Gets the kind.
		/// </summary>
		public KeywordKind Kind { get; set; }

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
	}
}