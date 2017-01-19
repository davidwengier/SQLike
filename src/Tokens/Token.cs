using System;
using System.Linq;

namespace StarNet.StarQL.Tokens
{
	/// <summary>
	/// A token
	/// </summary>
	public abstract class Token
	{
		/// <summary>
		/// The position of the start of the token
		/// </summary>
		public int Start { get; internal set; }

		/// <summary>
		/// The end of the token
		/// </summary>
		public int End { get; internal set; }
	}
}