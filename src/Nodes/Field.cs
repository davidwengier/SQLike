using System;
using System.Linq;

namespace SQLike.Nodes
{
	/// <summary>
	/// A field
	/// </summary>
	public class Field
	{
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public object Value { get; internal set; }
	}
}