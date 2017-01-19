using System;
using System.Collections.Generic;
using System.Linq;
using StarNet.StarQL.Tokens;
using Xunit;

namespace StarNet.StarQL.Tests
{
	public class LexerTests
	{
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
			var token = Assert.IsType<Whitespace>(tokens[0]);
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
			var token = Assert.IsType<Whitespace>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(3, token.End);

			token = Assert.IsType<Whitespace>(tokens[1]);
			Assert.Equal(4, token.Start);
			Assert.Equal(7, token.End);
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
			var token = Assert.IsType<StringLiteral>(tokens[0]);
			Assert.Equal(0, token.Start);
			Assert.Equal(middle.Length + 1, token.End);
			Assert.Equal(delim, token.Delimeter);
			Assert.Equal(middle, token.Value);
		}
	}
}