using System.Text.RegularExpressions;
using NumericExpressionEvaluation.Core.Parsing;
using Xunit;

namespace NumericExpressionEvaluation.Core.Tests
{
    public class ExpressionParserTests
    {
        [Theory]
        [InlineData("2")]
        [InlineData("-2")]
        [InlineData("+2")]
        [InlineData("--2")]
        [InlineData("-(-2)")]
        [InlineData("-2 * -(-2)")]
        [InlineData("22.2")]
        [InlineData("1 * (2 - 3)")]
        [InlineData("1 * (2 - -3)")]
        [InlineData("(1 * (2 - 3))")]
        [InlineData("((1 + 2) * 43) / 3.14 + 2 ^ 3")]
        [InlineData("-22 * +(-22.23 * -(44.2 + +33.2))")]
        public void Parse_CorrectInputWithoutPercentage_ReturnsCorrectData(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(FormatExpectedResult(input), parsed.ToString());
        }

        [Theory]
        [InlineData("2%", "0.02")]
        [InlineData("(1 * (2% - 3))", "(1 * (0.02 - 3))")]
        public void Parse_CorrectInputWithPercentage_ReturnsCorrectData(string input, string expectedResult)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(FormatExpectedResult(expectedResult), parsed.ToString());
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("(1 + a)", "a")]
        [InlineData("(1 + a) * 22 - xy + 33", "a", "xy")]
        public void Parse_InvalidCharacter_ThrowException(string input, params string[] expectedInvalidCharacters)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var exception = Record.Exception(() => parser.Parse(input));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ExpressionParserException>(exception);

            var astException = exception as ExpressionParserException;
            foreach (var expectedInvalidCharacter in expectedInvalidCharacters)
            {
                Assert.Contains($"'{expectedInvalidCharacter}'", astException.Message);
            }
        }

        [Theory]
        [InlineData("--")]
        [InlineData("1-")]
        [InlineData("*1")]
        [InlineData("1^-")]
        [InlineData("/-2")]
        public void Parse_InvalidInput_ThrowsException(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var exception = Record.Exception(() => parser.Parse(input));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ExpressionParserException>(exception);
        }

        [Theory]
        [InlineData("(1 * (2 - 3)%)")]
        [InlineData("(1 * (2 - 3))%")]
        public void Parse_InvalidInputWithPercentage_ThrowsException(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var exception = Record.Exception(() => parser.Parse(input));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ExpressionParserException>(exception);
        }

        [Fact]
        public void Parse_PowerIsRightAssociative_SameResult()
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed1 = parser.Parse("-2^-2^2");
            var parsed2 = parser.Parse("-(2^-(2^2))");

            // Assert
            Assert.Equal(parsed1.ToString(), parsed2.ToString());
        }

        private string FormatExpectedResult(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
    }
}
