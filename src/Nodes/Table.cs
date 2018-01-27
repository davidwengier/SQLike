namespace SQLike.Nodes
{
	/// <summary>
	/// A table
	/// </summary>
	/// <seealso cref="SQLike.Nodes.ITable" />
	public class Table : ITable
	{
		private string m_value;

		/// <summary>
		/// Initializes a new instance of the <see cref="Table"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Table(string value)
		{
			m_value = value;
		}
	}
}