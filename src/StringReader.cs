using System;
using System.Diagnostics;
using System.Text;

namespace StarNet.StarQL
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	internal class StringReader
	{
		internal const char EndOfString = (char)0;

		private string m_query;
		private int m_length;
		private int m_position;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private string DebuggerDisplay
		{
			get
			{
				if (AtEnd)
				{
					return "At End";
				}
				else
				{
					return "Pos " + m_position + ": [" + Peek() + "]" + m_query.Substring(m_position + 1);
				}
			}
		}

		internal int Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_position = value;
			}
		}

		internal StringReader(string query)
		{
			m_query = query;
			m_length = query.Length;
		}

		internal char Peek()
		{
			if (m_length > m_position)
			{
				return m_query[m_position];
			}
			else
			{
				return EndOfString;
			}
		}

		internal string Peek(int length)
		{
			if (m_length > m_position)
			{
				if (m_length > m_position + length)
				{
					return m_query.Substring(m_position, length);
				}
				else
				{
					return m_query.Substring(m_position);
				}
			}
			else
			{
				return null;
			}
		}

		internal char Read()
		{
			if (m_length > m_position)
			{
				return m_query[m_position++];
			}
			else
			{
				return EndOfString;
			}
		}

		internal void BackOne()
		{
			m_position--;
		}

		internal string PeekWord()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = m_position; i < m_length; i++)
			{
				if (char.IsWhiteSpace(m_query, i))
				{
					break;
				}
				else
				{
					sb.Append(m_query[i]);
				}
			}
			return sb.ToString();
		}

		internal void SkipWhitespace()
		{
			while (char.IsWhiteSpace(Peek()))
			{
				Read();
			}
		}

		internal bool AtEnd
		{
			get
			{
				return m_position >= m_length;
			}
		}

		internal bool SkipIf(char character)
		{
			if (Peek() == character)
			{
				Read();
				return true;
			}
			return false;
		}

		internal string ReadUntil(Func<char, bool> predicate)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = m_position; i < m_length; i++)
			{
				char curr = m_query[i];
				if (predicate(curr))
				{
					return sb.ToString();
				}
				sb.Append(curr);
				m_position++;
			}
			return sb.ToString();
		}

		internal string ReadUntilNot(string terminator)
		{
			return ReadUntil(terminator, true);
		}

		internal string ReadUntil(string terminator)
		{
			return ReadUntil(terminator, false);
		}

		internal string ReadUntil(string terminator, bool not)
		{
			if (terminator == null || terminator.Length == 0)
			{
				if (not)
				{
					return Read().ToString();
				}
				return m_query.Substring(m_position);
			}

			// micro-optimizations!
			int termLength = terminator.Length;
			bool singleCharTerminator = (termLength == 1);
			char single = terminator[0];

			StringBuilder sb = new StringBuilder();
			for (int i = m_position; i < m_length; i++)
			{
				bool stop = false;
				if (single == m_query[i])
				{
					stop = !not;
				}
				else
				{
					stop = not;
				}

				if (stop)
				{
					if (singleCharTerminator)
					{
						return sb.ToString();
					}
					else if (i + termLength <= m_length && m_query.Substring(i, termLength).Equals(terminator, StringComparison.OrdinalIgnoreCase))
					{
						return sb.ToString();
					}
				}
				sb.Append(m_query[i]);
				m_position++;
			}
			return sb.ToString();
		}

		internal string ReadUntilMatching(string start, string end)
		{
			StringBuilder sb = new StringBuilder();

			int startLength = start.Length - 1;
			int endLength = end.Length - 1;

			// optimizations to only peek if necessary
			bool startIsSingleChar = (startLength == 0);
			bool endIsSingleChar = (endLength == 0);
			char startChar = start[0];
			char endChar = end[0];

			// save the substring if the tag wont be needed later
			if (!startIsSingleChar)
			{
				start = start.Substring(1);
			}
			if (!endIsSingleChar)
			{
				end = end.Substring(1);
			}

			int nesting = 0;
			// if we're not passed the start tag then we need to un-nest because the first thing in the loop will be to nest
			if (Peek() == startChar && (startIsSingleChar || Peek(startLength + 1).Equals(startChar + start, StringComparison.OrdinalIgnoreCase)))
			{
				nesting = -1;
			}
			char cur;
			while ((cur = Read()) != StringReader.EndOfString)
			{
				if (cur == startChar && (startIsSingleChar || Peek(startLength).Equals(start, StringComparison.OrdinalIgnoreCase)))
				{
					// if this isn't the first one then add it to the result
					if (nesting > -1)
					{
						sb.Append(cur + Peek(startLength));
					}
					m_position += startLength;
					nesting++;
				}
				else if (cur == endChar && (endIsSingleChar || Peek(endLength).Equals(end, StringComparison.OrdinalIgnoreCase)))
				{
					if (nesting == 0)
					{
						m_position += endLength;
						return sb.ToString();
					}
					else
					{
						sb.Append(cur + Peek(endLength));
						nesting--;
						m_position += endLength;
					}
				}
				else
				{
					sb.Append(cur);
				}
			}

			return sb.ToString();
		}

		internal string ReadToEnd()
		{
			string result = m_query.Substring(m_position);
			m_position = m_length;
			return result;
		}

		internal void Skip(int count)
		{
			m_position += count;
		}
	}
}