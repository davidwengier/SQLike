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
	public static class Lexer
	{
		/// <summary>
		/// Lexes the specified query.
		/// </summary>
		public static List<Token> Lex(string query)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));
			if (query.Length == 0) return new List<Token>();

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
					tokens.Add(GetNumericOrDateLiteral(reader));
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
				else if (currentChar == '.')
				{
					tokens.Add(CreateToken<Period>(reader));
					reader.Read();
				}
				else if (currentChar.Equals('['))
				{
					tokens.Add(GetIdentifierToken(reader, currentChar));
				}
				else
				{
					Token token = TryLexKeyword(reader);
					if (token != null)
					{
						tokens.Add(token);
					}
					else
					{
						tokens.Add(GetIdentifierToken(reader, currentChar));
					}
				}
			}

			return tokens;
		}

		private static Token TryLexKeyword(StringReader reader)
		{
			foreach (KeywordKind kind in Enum.GetValues(typeof(KeywordKind)))
			{
				if (reader.PeekWord().Equals(kind.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					return new Keyword
					{
						Start = reader.Position,
						Value = reader.ReadUntil(char.IsWhiteSpace),
						End = reader.Position,
						Kind = kind
					};
				}
			}
			return null;
		}

		private static Token GetIdentifierToken(StringReader reader, char currentChar)
		{
			Identifier token = CreateToken<Identifier>(reader);
			if (currentChar == '[')
			{
				token.Qualified = true;
				token.Value = reader.ReadUntilMatching("[", "]");
			}
			else
			{
				token.Value = reader.ReadUntil(c => char.IsWhiteSpace(c) || c == '.' || c == ',');
			}
			token.End = reader.Position;
			return token;
		}

		private static Token GetStringToken(StringReader reader, char currentChar)
		{
			StringLiteral token = CreateToken<StringLiteral>(reader);
			token.Delimeter = currentChar;
			string currentCharAsString = currentChar.ToString();
			string value = reader.ReadUntilMatching(currentCharAsString, currentCharAsString);
			if (reader.AtEnd && (value.Length == 0 || value[value.Length - 1] != currentChar))
			{
				return new Error()
				{
					Start = token.Start,
					Value = currentCharAsString + value,
					End = reader.Position,
					Message = "Unterminated string literal - Could not find matching " + currentCharAsString
				};
			}
			token.Value = value.Substring(0, value.Length - 1);
			token.End = reader.Position - 1;
			return token;
		}

		private static Token GetNumericOrDateLiteral(StringReader reader)
		{
			int start = reader.Position;

			bool isDate = false;
			bool seenASlash = false;
			bool seenAMinus = false;
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
				else if (c == '-' && reader.Position - start - 1 == 0)
				{
					sb.Append(c);
				}
				else if (c == '-')
				{
					sb.Append(c);
					if (seenAMinus)
					{
						isDate = true;
					}
					else
					{
						seenAMinus = true;
						potentialLastSpot = reader.Position - start - 1;
					}
				}
				else if (!char.IsDigit(c) && c != '.')
				{
					reader.BackOne();
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
				if (DateTime.TryParse(value, out DateTime dateValue))
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
				if (decimal.TryParse(value, out decimal decValue))
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
				if (int.TryParse(value, out int intValue))
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
				End = reader.Position + 1
			};
			return token;
		}
	}
}