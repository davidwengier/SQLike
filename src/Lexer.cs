using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				else if (char.IsDigit(currentChar) || currentChar == '-')
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
				else
				{
					tokens.Add(new Error()
					{
						Start = reader.Position,
						End = reader.Position + 1,
						Value = reader.Read().ToString(),
						Message = "Unknown identifier"
					});
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
			int start = reader.Position;
			int position = start;

			bool isDate = false;
			bool seenASlash = false;
			int potentialLastSpot = 0;
			StringBuilder sb = new StringBuilder();
			while (!reader.AtEnd)
			{
				char c = reader.Read();
				if (c == '/')
				{
					sb.Append(c);
					if (seenASlash)
					{
						isDate = true;
					}
					else
					{
						seenASlash = true;
						potentialLastSpot = reader.Position - start - 1;
					}
				}
				else if (c == '-' && reader.Position - start == 0)
				{
					sb.Append(c);
				}
				else if (!char.IsDigit(c) && c != '.')
				{
					break;
				}
				else
				{
					sb.Append(c);
				}
			}
			string value = sb.ToString();
			if (isDate)
			{
				DateTime dateValue;
				if (DateTime.TryParse(value, out dateValue))
				{
					return new DateLiteral()
					{
						Start = start,
						End = reader.Position,
						Value = dateValue
					};
				}
				else
				{
					return new Error()
					{
						Start = start,
						End = reader.Position,
						Value = value,
						Message = "Could not parse value as a date"
					};
				}
			}
			else if (potentialLastSpot > 0)
			{
				reader.Position = potentialLastSpot;
				value = value.Substring(0, potentialLastSpot);
			}
			if (value.IndexOf('.') > -1)
			{
				decimal decValue;
				if (decimal.TryParse(value, out decValue))
				{
					return new NumericLiteral()
					{
						Start = start,
						End = reader.Position,
						Value = decValue
					};
				}
				else
				{
					return new Error()
					{
						Start = start,
						End = reader.Position,
						Value = value,
						Message = "Found dot in identifier but could not parse as a decimal"
					};
				}
			}
			else
			{
				int intValue;
				if (int.TryParse(value, out intValue))
				{
					return new NumericLiteral()
					{
						Start = start,
						End = reader.Position,
						Value = intValue
					};
				}
				else
				{
					return new Error()
					{
						Start = start,
						End = reader.Position,
						Value = value,
						Message = "Could not parse as a integer"
					};
				}
			}
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