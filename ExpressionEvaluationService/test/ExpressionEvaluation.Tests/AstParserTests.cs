using ExpressionEvaluation.Core.Expression;
using ExpressionEvaluation.Core.Parsing;
using Xunit;

namespace ExpressionEvaluation.Tests
{
    public class AstParserTests
    {
        [Theory]
        [InlineData("1 + 1")]
        public void AstParser_Parse_ReturnsCorrectResult(string input)
        {
            // Arrange
            var nodeFactory = new ExpressionNodeFactory();
            var parser = new AstParser(nodeFactory);

            // Act
            var result = parser.Parse(input);

            // Assert
            Assert.NotNull(result);
        }
    }
}
