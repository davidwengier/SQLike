using System;

namespace SQLike.Tokens
{
    /// <summary>
    /// A token
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// The position of the start of the token
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// The end of the token
        /// </summary>
        public int End { get; set; }
    }
}