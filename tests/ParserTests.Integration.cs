using System.Collections.Generic;
using SQLike.Nodes;
using SQLike.Tokens;
using Xunit;

namespace SQLike.Tests
{
	public class ParserTests_Integration
	{
        [Theory]
        [MemberData(nameof(GetSelectStatements))]
        public void SelectStatements(string input, List<Token> expected)
        {
			var parser = new Parser();
			var tokens = Lexer.Lex(input);
			var result = parser.Parse(tokens);

			Assert.NotNull(result);
		}

		public static SerializedTheoryData<string, Statement> GetSelectStatements()
		{
			return new SerializedTheoryData<string, Statement> {
				{
					"SELECT TableOfDataID FROM TableOfData",
					new Statement(new Keyword("SELECT", KeywordKind.Select))
				}
			};
		}
	}
}