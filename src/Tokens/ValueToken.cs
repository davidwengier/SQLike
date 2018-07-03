using System;

namespace SQLike.Tokens
{
    /// <summary>
    /// A token
    /// </summary>
    public abstract class ValueToken : Token
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value { get; set; }

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