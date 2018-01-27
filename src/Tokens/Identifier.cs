using System;
using System.Linq;

namespace SQLike.Tokens
{
	/// <summary>
	/// An identifier
	/// </summary>
	public class Identifier : ValueToken
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="Identifier"/> is qualified.
		/// </summary>
		public bool Qualified { get; set; }

		internal Identifier()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Identifier"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Identifier(string value)
		{
			this.Value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Identifier"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="qualified">if set to <c>true</c> [qualified].</param>
		public Identifier(string value, bool qualified)
			: this(value)
		{
			this.Qualified = qualified;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (this.Qualified)
			{
				return "[" + this.Value + "]";
			}
			return this.Value;
		}
	}
}