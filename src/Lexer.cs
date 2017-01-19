using System;
using System.Collections.Generic;
using System.Linq;
using StarNet.StarQL.Tokens;

namespace StarNet.StarQL
{
	/// <summary>
	/// Converts strings to tokens
	/// </summary>
	public class Lexer
	{
		/// <summary>
		/// Lexes the specified query.
		/// </summary>
		public static List<Token> Lex(string query)
		{
			Lexer lexer = new Lexer();
			return lexer.Tokenize(query);
		}

		/// <summary>
		/// Tokenizes the specified query.
		/// </summary>
		public List<Token> Tokenize(string query)
		{
			List<Token> tokens = new List<Token>();
			StringReader reader = new StringReader(query);

			while (!reader.AtEnd)
			{
				char currentChar = reader.Peek();
				if (char.IsWhiteSpace(currentChar))
				{
					tokens.Add(GetWhitespaceToken(reader, currentChar));
				}
				else if (char.IsDigit(currentChar))
				{
					tokens.Add(GetNumericOrDateLiteral(reader, currentChar));
				}
				else if (currentChar == '"' || currentChar == '\'')
				{
					tokens.Add(GetStringToken(reader, currentChar));
				}
				else if (currentChar == ',')
				{
					tokens.Add(CreateToken<Comma>(reader));
					reader.Read();
				}
			}

			return tokens;
		}

		private Token GetStringToken(StringReader reader, char currentChar)
		{
			StringLiteral token = CreateToken<StringLiteral>(reader);
			token.Delimeter = currentChar;
			token.Value = reader.ReadUntilMatching(currentChar.ToString(), currentChar.ToString());
			token.Value = token.Value.Substring(0, token.Value.Length - 1);
			token.End = reader.Position - 1;
			return token;
		}

		private Token GetNumericOrDateLiteral(StringReader reader, char currentChar)
		{
			throw new NotImplementedException();
		}

		private static Whitespace GetWhitespaceToken(StringReader reader, char currentChar)
		{
			Whitespace token = CreateToken<Whitespace>(reader);
			token.Character = currentChar;
			reader.ReadUntilNot(currentChar.ToString());
			token.End = reader.Position - 1;
			return token;
		}

		private static T CreateToken<T>(StringReader reader) where T : Token, new()
		{
			T token = new T()
			{
				Start = reader.Position,
				// Assume a single char token
				End = reader.Position
			};
			return token;
		}
	}
}