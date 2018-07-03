using System;
using System.Collections.Generic;
using System.Linq;
using SQLike.Tokens;
using Xunit;

namespace SQLike.Tests
{
    public class LexerTests_Integration
    {
        [Theory]
        [MemberData(nameof(GetSelectStatements))]
        public void SelectStatements(string input, List<Token> expected)
        {
            List<Token> actual = Lexer.Lex(input);

            Assert.Equal(expected, actual, new TokenComparer());
        }

        [Theory]
        [MemberData(nameof(GetSelectStatements))]
        public void SelectStatements_RoundTrip(string input, List<Token> expected)
        {
            if (expected == null) throw new ArgumentNullException(nameof(expected));

            List<Token> actual = Lexer.Lex(input);

            string roundTrip = string.Join("", actual);
            Assert.Equal(input, roundTrip);
        }

        [Theory]
        [MemberData(nameof(GetSelectStatements))]
        public void SelectStatements_ToUpper(string input, List<Token> expected)
        {
            if (expected == null) throw new ArgumentNullException(nameof(expected));

            List<Token> actual = Lexer.Lex(input);

            foreach (ValueToken token in actual.OfType<ValueToken>())
            {
                token.Value = token.Value.ToUpper();
            }

            string roundTrip = string.Join("", actual);
            Assert.Equal(input.ToUpper(), roundTrip);
        }

        [Theory]
        [MemberData(nameof(GetSelectStatements))]
        public void SelectStatements_ToLower(string input, List<Token> expected)
        {
            if (expected == null) throw new ArgumentNullException(nameof(expected));

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
                },
                {
                    "select [EntryID] from [Entry]",
                    new List<Token>()
                    {
                        new Keyword("select", KeywordKind.Select),
                        new Whitespace(' '),
                        new Identifier("EntryID", true),
                        new Whitespace(' '),
                        new Keyword("from", KeywordKind.From),
                        new Whitespace(' '),
                        new Identifier("Entry", true)
                    }
                },
                {
                    "select[EntryID]from[Entry]",
                    new List<Token>()
                    {
                        new Keyword("select", KeywordKind.Select),
                        new Identifier("EntryID", true),
                        new Keyword("from", KeywordKind.From),
                        new Identifier("Entry", true)
                    }
                },
                {
                    "select\n    EntryID\nfrom\n    Entry",
                    new List<Token>()
                    {
                        new Keyword("select", KeywordKind.Select),
                        new Whitespace('\n'),
                        new Whitespace(' ') { End = 3 },
                        new Identifier("EntryID"),
                        new Whitespace('\n'),
                        new Keyword("from", KeywordKind.From),
                        new Whitespace('\n'),
                        new Whitespace(' ') { End = 3 },
                        new Identifier("Entry")
                    }
                }
            };
        }
    }
}