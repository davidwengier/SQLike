using System.Collections.Generic;
using StarNet.StarQL.Tokens;

namespace StarNet.StarQL.Nodes
{
	/// <summary>
	/// A statement
	/// </summary>
	/// <seealso cref="StarNet.StarQL.Nodes.ITable" />
	public class Statement : ITable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Statement"/> class.
		/// </summary>
		/// <param name="selectToken">The select token.</param>
		public Statement(Keyword selectToken)
		{
			this.SelectToken = selectToken;
		}

		/// <summary>
		/// Gets the fields.
		/// </summary>
		/// <value>
		/// The fields.
		/// </value>
		public List<Field> Fields { get; } = new List<Field>();

		/// <summary>
		/// Gets the table.
		/// </summary>
		/// <value>
		/// The table.
		/// </value>
		public ITable Table { get; internal set; }

		/// <summary>
		/// Gets the select token.
		/// </summary>
		/// <value>
		/// The select token.
		/// </value>
		public Keyword SelectToken { get; private set; }

		/// <summary>
		/// Gets from token.
		/// </summary>
		/// <value>
		/// From token.
		/// </value>
		public Keyword FromToken { get; internal set; }
	}
}