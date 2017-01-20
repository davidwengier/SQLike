using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}