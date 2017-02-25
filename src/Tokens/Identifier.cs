using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// An identifier
	/// </summary>
	public class Identifier : Token
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="Identifier"/> is qualified.
		/// </summary>
		public bool Qualified { get; internal set; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		public string Value { get; internal set; }
	}
}