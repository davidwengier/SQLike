using System;
using System.Collections.Generic;
using System.Linq;
using StarNet.StarQL.Tokens;
using Xunit;

namespace StarNet.StarQL.Tests
{
	public class LexerTests
	{
		[Fact]
		public void Lex_Null_Throws_Exception()
		{
			Assert.Throws<ArgumentNullException>(() => Lexer.Lex(null));
		}

		[Fact]
		public void Lex_BlankString_Returns_EmptyList()
		{
			List<Token> tokens = Lexer.Lex("");

			Assert.NotNull(tokens);
			Assert.Equal(0, tokens.Count);
		}

		[Theory]
		[InlineData(' ')]
		[InlineData('\t')]
		[InlineData('\n')]
		[InlineData('\r')]
		public void Whitespace(char c)
		{
			string input = new string(c, 4);

			List<Token> tokens = Lexer.Lex(input);

			Assert.NotNull(tokens);
			Assert.Equal(1, tokens.Count);
			Whitespace token = Assert.IsType<Whitespace>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(3, token.End);
		}

		[Theory]
		[InlineData(' ', '\t')]
		[InlineData('\t', ' ')]
		[InlineData(' ', '\n')]
		public void Two_bits_of_whitespace(char c, char other)
		{
			string input = new string(c, 4) + new string(other, 4);

			List<Token> tokens = Lexer.Lex(input);

			Assert.NotNull(tokens);
			Assert.Equal(2, tokens.Count);
			Whitespace token = Assert.IsType<Whitespace>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(3, token.End);

			token = Assert.IsType<Whitespace>(tokens[1]);
			Assert.Equal(4, token.Start);
			Assert.Equal(7, token.End);
		}

		[Theory]
		[InlineData(",", typeof(Comma))]
		[InlineData(".", typeof(Period))]
		public void Comma(string c, Type tokenType)
		{
			List<Token> tokens = Lexer.Lex(c);

			Assert.NotNull(tokens);
			Assert.Equal(1, tokens.Count);
			Assert.IsType(tokenType, tokens[0]);
			Assert.Equal(0, tokens[0].Start);
			Assert.Equal(1, tokens[0].End);
		}

		[Theory]
		[InlineData('"', "fred is a boss")]
		[InlineData('\'', "fred is cool")]
		[InlineData('"', "fred's crazy")]
		[InlineData('\'', "fred is \"silly\"")]
		[InlineData('"', "")]
		[InlineData('\'', "")]
		[InlineData('"', "fred is\na boss")]
		public void StringLiteral(char delim, string middle)
		{
			string input = delim + middle + delim;

			List<Token> tokens = Lexer.Lex(input);
			Assert.NotNull(tokens);
			Assert.Equal(1, tokens.Count);
			StringLiteral token = Assert.IsType<StringLiteral>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(middle.Length + 1, token.End);
			Assert.Equal(delim, token.Delimeter);
			Assert.Equal(middle, token.Value);
		}

		[Theory]
		[InlineData("1", 1, 1)]
		[InlineData("-1", -1, 2)]
		[InlineData("1/3", 1, 1)]
		[InlineData("1 / 3", 1, 1)]
		[InlineData("1/1.3", 1, 1)]
		[InlineData("1 / 1.3", 1, 1)]
		public void Integers(string input, int result, int length)
		{
			List<Token> tokens = Lexer.Lex(input);

			Assert.NotNull(tokens);
			NumericLiteral token = Assert.IsType<NumericLiteral>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(length, token.End);
			Assert.Equal(result, token.Value);
		}

		[Theory]
		[InlineData("1.2", 1.2, 3)]
		[InlineData("-1.2", -1.2, 4)]
		[InlineData("1.2/3", 1.2, 3)]
		[InlineData("-1.2/3", -1.2, 4)]
		[InlineData("1.2 / 3.1", 1.2, 3)]
		[InlineData("-1.2 / 3.1", -1.2, 4)]
		[InlineData("1.2 - 3.1", 1.2, 3)]
		[InlineData("-1.2 - 3.1", -1.2, 4)]
		public void Decimals(string input, decimal result, int length)
		{
			List<Token> tokens = Lexer.Lex(input);

			Assert.NotNull(tokens);
			NumericLiteral token = Assert.IsType<NumericLiteral>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(length, token.End);
			Assert.Equal(result, token.Value);
		}

		[Theory]
		[InlineData("1-3-2017", 2017, 3, 1, 8)]
		[InlineData("1/3/2017", 2017, 3, 1, 8)]
		[InlineData("31-12-2017", 2017, 12, 31, 10)]
		[InlineData("31/12/2017", 2017, 12, 31, 10)]
		public void DateLiteral(string input, int year, int month, int day, int length)
		{
			List<Token> tokens = Lexer.Lex(input);

			Assert.NotNull(tokens);
			DateLiteral token = Assert.IsType<DateLiteral>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(length, token.End);
			Assert.Equal(new DateTime(year, month, day), token.Value);
		}

		[Theory]
		[InlineData("15/15/2017")]
		[InlineData("1..2")]
		public void Bad_Input_Return_Error(string input)
		{
			List<Token> tokens = Lexer.Lex(input);
			Error token = Assert.IsType<Error>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(input.Length, token.End);
			Assert.Equal(input, token.Value);
		}

		[Theory]
		[InlineData("[hi there]", true, "hi there", 10)]
		[InlineData("[hi]", true, "hi", 4)]
		[InlineData("hi", false, "hi", 2)]
		[InlineData("hi there", false, "hi", 2)]
		public void Indentifier(string input, bool qualified, string value, int length)
		{
			List<Token> tokens = Lexer.Lex(input);

			Assert.NotNull(tokens);
			Identifier token = Assert.IsType<Identifier>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(length, token.End);
			Assert.Equal(qualified, token.Qualified);
			Assert.Equal(value, token.Value);
		}

		[Theory]
		[MemberData("GetKeywords")]
		public void Keyword(object input)
		{
			Random rand = new Random();
			string keyword = new string(input.ToString().Select(c => rand.Next(0, 2) == 1 ? char.ToUpper(c) : char.ToLower(c)).ToArray());
			KeywordKind kind = (KeywordKind)input;

			List<Token> tokens = Lexer.Lex(keyword);

			Assert.NotNull(tokens);
			Keyword token = Assert.IsType<Keyword>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(keyword.Length, token.End);
			Assert.Equal(keyword, token.Value);
		}

		public static IEnumerable<object[]> GetKeywords()
		{
			return Enum.GetValues(typeof(KeywordKind)).Cast<object>().Select(c => new object[] { c });
		}
	}
}