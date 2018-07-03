using System;
using System.Collections.Generic;
using SQLike.Tokens;

namespace SQLike.Tests
{
    public partial class TokenComparer : IEqualityComparer<Token>
    {
        public bool Equals(Token x, Token y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            if (x.GetType() != y.GetType()) return false;

            return x.ToString().Equals(y.ToString(), StringComparison.Ordinal);
        }

        public int GetHashCode(Token obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}