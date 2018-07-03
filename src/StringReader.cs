using System;
using System.Diagnostics;
using System.Text;

namespace SQLike
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class StringReader
    {
        internal const char EndOfString = (char)0;

        private readonly string _query;
        private readonly int _length;
        private int _position;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private string DebuggerDisplay
        {
            get
            {
                if (this.AtEnd)
                {
                    return "At End";
                }
                else
                {
                    return "Pos " + _position + ": [" + Peek() + "]" + _query.Substring(_position + 1);
                }
            }
        }

        internal int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        internal StringReader(string query)
        {
            _query = query;
            _length = query.Length;
        }

        internal char Peek()
        {
            if (_length > _position)
            {
                return _query[_position];
            }
            else
            {
                return EndOfString;
            }
        }

        internal string Peek(int length)
        {
            if (_length > _position)
            {
                if (_length > _position + length)
                {
                    return _query.Substring(_position, length);
                }
                else
                {
                    return _query.Substring(_position);
                }
            }
            else
            {
                return null;
            }
        }

        internal char Read()
        {
            if (_length > _position)
            {
                return _query[_position++];
            }
            else
            {
                return EndOfString;
            }
        }

        internal void BackOne()
        {
            _position--;
        }

        internal string PeekWord()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = _position; i < _length; i++)
            {
                if (IsWordBreak(_query[i]))
                {
                    break;
                }
                else
                {
                    sb.Append(_query[i]);
                }
            }
            return sb.ToString();
        }

        private static bool IsWordBreak(char character)
        {
            return char.IsWhiteSpace(character) || character == '[';
        }

        internal string ReadWord()
        {
            return ReadUntil(IsWordBreak);
        }

        internal bool AtEnd
        {
            get
            {
                return _position >= _length;
            }
        }

        internal string ReadUntil(Func<char, bool> predicate)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = _position; i < _length; i++)
            {
                char curr = _query[i];
                if (predicate(curr))
                {
                    return sb.ToString();
                }
                sb.Append(curr);
                _position++;
            }
            return sb.ToString();
        }

        internal string ReadUntilNot(string terminator)
        {
            return ReadUntil(terminator, true);
        }

        internal string ReadUntil(string terminator, bool not)
        {
            if (terminator == null || terminator.Length == 0)
            {
                if (not)
                {
                    return Read().ToString();
                }
                return _query.Substring(_position);
            }

            // micro-optimizations!
            int termLength = terminator.Length;
            bool singleCharTerminator = (termLength == 1);
            char single = terminator[0];

            StringBuilder sb = new StringBuilder();
            for (int i = _position; i < _length; i++)
            {
                bool stop = false;
                if (single == _query[i])
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
                    else if (i + termLength <= _length && _query.Substring(i, termLength).Equals(terminator, StringComparison.OrdinalIgnoreCase))
                    {
                        return sb.ToString();
                    }
                }
                sb.Append(_query[i]);
                _position++;
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
                    _position += startLength;
                    nesting++;
                }
                else if (cur == endChar && (endIsSingleChar || Peek(endLength).Equals(end, StringComparison.OrdinalIgnoreCase)))
                {
                    if (nesting == 0)
                    {
                        _position += endLength;
                        return sb.ToString();
                    }
                    else
                    {
                        sb.Append(cur + Peek(endLength));
                        nesting--;
                        _position += endLength;
                    }
                }
                else
                {
                    sb.Append(cur);
                }
            }

            return sb.ToString();
        }
    }
}