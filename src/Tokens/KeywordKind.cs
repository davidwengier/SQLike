using System;
using System.Linq;

namespace SQLike.Tokens
{
	/// <summary>
	/// The kind of keyword
	/// </summary>
	public enum KeywordKind
	{
		/// <summary>
		/// The select
		/// </summary>
		Select,

		/// <summary>
		/// From
		/// </summary>
		From,

		/// <summary>
		/// The where
		/// </summary>
		Where
	}
}