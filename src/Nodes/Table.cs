namespace SQLike.Nodes
{
    /// <summary>
    /// A table
    /// </summary>
    /// <seealso cref="SQLike.Nodes.ITable" />
    public class Table : ITable
    {
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Table(string value)
        {
            _value = value;
        }
    }
}