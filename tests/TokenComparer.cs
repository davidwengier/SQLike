using System;
using System.Collections.Generic;
using StarNet.StarQL.Tokens;

namespace StarNet.StarQL.Tests
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