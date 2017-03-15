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
	}
}