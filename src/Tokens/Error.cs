using System;
using System.Linq;

namespace SQLike.Tokens
{
	/// <summary>
	/// An error token
	/// </summary>
	/// <seealso cref="SQLike.Tokens.Token" />
	public class Error : ValueToken
	{
		/// <summary>
		/// Gets the error.
		/// </summary>
		public string Message { get; set; }
	}
}