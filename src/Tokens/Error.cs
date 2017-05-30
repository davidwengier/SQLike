using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// An error token
	/// </summary>
	/// <seealso cref="StarNet.StarQL.Tokens.Token" />
	public class Error : Token
	{
		/// <summary>
		/// Gets the error.
		/// </summary>
		public string Message { get; internal set; }

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
			return this.Value;
		}
	}
}