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

		[Theory]
		[MemberData("GetSelectStatements")]
		public void SelectStatements_RoundTrip(string input, List<Token> expected)
		{
			List<Token> actual = Lexer.Lex(input);

			string roundTrip = string.Join("", actual);
			Assert.Equal(input, roundTrip);
		}

		[Theory]
		[MemberData("GetSelectStatements")]
		public void SelectStatements_ToUpper(string input, List<Token> expected)
		{
			List<Token> actual = Lexer.Lex(input);

			foreach (ValueToken token in actual.OfType<ValueToken>())
			{
				token.Value = token.Value.ToUpper();
			}

			string roundTrip = string.Join("", actual);
			Assert.Equal(input.ToUpper(), roundTrip);
		}

		[Theory]
		[MemberData("GetSelectStatements")]
		public void SelectStatements_ToLower(string input, List<Token> expected)
		{
			List<Token> actual = Lexer.Lex(input);

			foreach (ValueToken token in actual.OfType<ValueToken>())
			{
				token.Value = token.Value.ToLower();
			}

			string roundTrip = string.Join("", actual);
			Assert.Equal(input.ToLower(), roundTrip);
		}

		public static SerializedTheoryData<string, List<Token>> GetSelectStatements()
		{
			return new SerializedTheoryData<string, List<Token>> {
				{
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
				},
				{
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
				}
			};
		}
	}
}