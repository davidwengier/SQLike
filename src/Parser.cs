using System;
using System.Collections.Generic;
using System.Linq;
using SQLike.Nodes;
using SQLike.Tokens;

namespace SQLike
{
	/// <summary>
	/// Parses a StarQL statement from a list of tokens that come from the Lexer
	/// </summary>
	public class Parser
	{
		private Token m_current;
		private int m_position;
		private IEnumerator<Token> m_enumerator;

		/// <summary>
		/// Gets the error message if the parsing is unsuccessful
		/// </summary>
		public string Error { get; private set; } = string.Empty;

		#region Parser Helpers

		private Token GetNext()
		{
			if (m_enumerator.MoveNext())
			{
				Token current = m_enumerator.Current;
				m_position = current.Start;
				return current;
			}
			return null;
		}

		private void SetError(string message)
		{
			this.Error = "Error at position " + m_position + ": " + message;
		}

		private T Expect<T>() where T : Token
		{
			return Expect<T>(null);
		}

		private T Expect<T>(Func<T, bool> predicate) where T : Token
		{
			T token = Accept<T>(predicate);
			if (token != null)
			{
				return token;
			}
			UnexpectedToken<T>();
			return null;
		}

		private T Accept<T>() where T : Token
		{
			return Accept<T>(null);
		}

		private T Accept<T>(Func<T, bool> predicate) where T : Token
		{
			T token = m_current as T;
			if (token != null && (predicate == null || predicate(token)))
			{
				GetNext();
				return token;
			}
			return null;
		}

		private void UnexpectedToken<T>()
		{
			SetError("Expected " + typeof(T).Name + ", found " + m_current.GetType().Name + " '" + m_current + "'");
		}

		#endregion Parser Helpers

		/// <summary>
		/// Parses the specified tokens.
		/// </summary>
		/// <param name="tokens">The tokens.</param>
		/// <returns></returns>
		public Statement Parse(IEnumerable<Token> tokens)
		{
			m_enumerator = tokens.GetEnumerator();
			m_current = GetNext();

			return Statement();
		}

		private Statement Statement()
		{
			Keyword select = Expect<Keyword>(k => k.Kind == KeywordKind.Select);
			if (select == null) return null;

			Statement statement = new Statement(select);
			if (this.Error.Length > 0) return null;

			do
			{
				Field field = Field();
				if (this.Error.Length > 0) return null;

				statement.Fields.Add(field);
			} while (Accept<Comma>() != null);

			Keyword from = Expect<Keyword>(k => k.Kind == KeywordKind.From);
			if (from == null) return null;
			statement.FromToken = from;

			statement.Table = Table();
			if (this.Error.Length > 0) return null;

			return statement;
		}

		private Field Field()
		{
			Token token;
			if ((token = Accept<NumericLiteral>()) != null)
			{
				return Numeric((NumericLiteral)token);
			}
			else if ((token = Accept<StringLiteral>()) != null)
			{
				return String((StringLiteral)token);
			}
			UnexpectedToken<Field>();
			return null;
		}

		private Field Numeric(NumericLiteral number)
		{
			return new Field()
			{
				Value = number.Number
			};
		}

		private Field String(StringLiteral token)
		{
			return new Field()
			{
				Value = token.Value
			};
		}

		private ITable Table()
		{
			Token token;
			if ((token = Accept<Identifier>()) != null)
			{
				return new Table(((Identifier)token).Value);
			}
			//else if (Accept<Bracket>())
			//{
			//	return Statement();
			//}
			UnexpectedToken<Table>();
			return null;
		}
	}
}