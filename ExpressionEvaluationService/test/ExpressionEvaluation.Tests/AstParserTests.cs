using ExpressionEvaluation.Core.Expression;
using ExpressionEvaluation.Core.Parsing;
using Xunit;

namespace ExpressionEvaluation.Tests
{
    public class AstParserTests
    {
        [Theory]
        [InlineData("(1)")]
        [InlineData("(1 + 1)")]
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

        [Theory]
        [InlineData("a", 'a')]
        [InlineData("(1 + a)", 'a')]
        [InlineData("(1 + a) * 22 - xy + 33", 'a', 'x', 'y')]
        public void Parse_InvalidCharacter_ThrowException(string input, params char[] expectedInvalidCharacters)
        {
            // Arrange
            var nodeFactory = new ExpressionNodeFactory();
            var parser = new AstParser(nodeFactory);

            // Act
            var exception = Record.Exception(() => parser.Parse(input));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<AstParserException>(exception);

            var astException = exception as AstParserException;
            foreach (var expectedInvalidCharacter in expectedInvalidCharacters)
            {
                Assert.Contains($"'{expectedInvalidCharacter}'", astException.Message);
            }
        }

        [Fact]
        public void SplitToBlocks_InvalidBlock_ThrowsException()
        {
            // Arrange
            var nodeFactory = new ExpressionNodeFactory();
            var parser = new AstParser(nodeFactory);

            // Act
            var exception = Record.Exception(() => parser.Parse("((1)"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<AstParserException>(exception);
        }
    }
}
