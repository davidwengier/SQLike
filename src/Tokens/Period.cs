using System;

namespace SQLike.Tokens
{
    /// <summary>
    /// Literally a Period
    /// </summary>
    public class Period : Token
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ".";
        }
    }
}