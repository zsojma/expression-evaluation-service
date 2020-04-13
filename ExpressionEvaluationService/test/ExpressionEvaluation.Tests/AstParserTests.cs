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
            var parser = new AstParser();

            // Act
            var result = parser.Parse(input);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("(1 + a)", "a")]
        [InlineData("(1 + a) * 22 - xy + 33", "a", "xy")]
        public void Parse_InvalidCharacter_ThrowException(string input, params string[] expectedInvalidCharacters)
        {
            // Arrange
            var parser = new AstParser();

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

        [Theory]
        [InlineData("2")]
        [InlineData("1 * (2 - 3)")]
        [InlineData("(1 * (2 - 3))")]
        [InlineData("((1 + 2) * 43) / 3.14 + 2 ^ 3")]
        public void Parse_CorrectInputWithoutPercentage_ReturnsCorrectData(string input)
        {
            // Arrange
            var parser = new AstParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(input.Replace(" ", ""), parsed.ToString());
        }

        [Theory]
        [InlineData("2%", "0.02")]
        [InlineData("(1 * (2% - 3))", "(1 * (0.02 - 3))")]
        public void Parse_CorrectInputWithPercentage_ReturnsCorrectData(string input, string expectedResult)
        {
            // Arrange
            var parser = new AstParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(expectedResult.Replace(" ", ""), parsed.ToString());
        }

        [Theory]
        [InlineData("(1 * (2 - 3)%)")]
        public void Parse_InvalidInputWithPercentage_ThrowsException(string input)
        {
            // Arrange
            var parser = new AstParser();

            // Act
            var exception = Record.Exception(() => parser.Parse(input));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<AstParserException>(exception);
        }
    }
}
