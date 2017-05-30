using System;
using System.Collections.Generic;
using System.Linq;
using StarNet.StarQL.Tokens;
using Xunit;

namespace StarNet.StarQL.Tests
{
	public class LexerTests_Integration
	{
		[Theory]
		[MemberData("GetSelectStatements")]
		public void SelectStatements(string input, List<Token> expected)
		{
			List<Token> actual = Lexer.Lex(input);

			Assert.Equal(expected, actual, new TokenComparer());
		}

		public static IEnumerable<object[]> GetSelectStatements()
		{
			yield return new object[] {
				"SELECT EntryID FROM Entry",
				new List<Token>()
				{
					new Keyword("SELECT", KeywordKind.Select),
					new Whitespace(' '),
					new Identifier("EntryID"),
					new Whitespace(' '),
					new Keyword("FROM", KeywordKind.From),
					new Whitespace(' '),
					new Identifier("Entry")
				}
			};

			yield return new object[] {
				"select [EntryID] from Entry",
				new List<Token>()
				{
					new Keyword("select", KeywordKind.Select),
					new Whitespace(' '),
					new Identifier("EntryID", true),
					new Whitespace(' '),
					new Keyword("from", KeywordKind.From),
					new Whitespace(' '),
					new Identifier("Entry")
				}
			};
		}
	}
}