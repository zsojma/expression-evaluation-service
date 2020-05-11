using LogicalExpressionEvaluation.Core.Parsing;
using Xunit;

namespace LogicalExpressionEvaluation.Core.Tests
{
    public class ExpressionParserTests
    {
        [Theory]
        [InlineData("2")]
        [InlineData("not 2")]
        [InlineData("not not 2")]
        [InlineData("not (not 2)")]
        [InlineData("not 2 and not (not 2)")]
        [InlineData("1 and (2 or 3)")]
        [InlineData("1 and (2 or not 3)")]
        [InlineData("(1 and (2 or 3))")]
        [InlineData("((1 or 2) and 43) and 3.14 or 2 and 3")]
        [InlineData("not 22 and not (not 22.23 and not (44.2 or 33.2))")]
        public void Parse_CorrectInputSimpleWord_ReturnsCorrectData(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(input, parsed.ToString());
        }

        [Theory]
        [InlineData("\"a\"")]
        [InlineData("\"a b\"")]
        [InlineData("(\"f 1\" and (\"f 2\" or 3))")]
        public void Parse_CorrectInputComplexWord_ReturnsCorrectData(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(input, parsed.ToString());
        }

        [Theory]
        [InlineData(" 222    or   \" a  \"    ", "222 or \" a  \"")]
        [InlineData("\"   a and     b    \"", "\"   a and     b    \"")]
        [InlineData("    (    \"   f  1  \"     and  (  \"f 2\"   or  3   )  )", "(\"   f  1  \" and (\"f 2\" or 3))")]
        public void Parse_NotSanitized_ReturnsCorrectData(string input, string expectedResult)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(expectedResult, parsed.ToString());
        }

        [Theory]
        [InlineData("1 And 2")]
        [InlineData("NOT 2")]
        [InlineData("(\"f 1\" anD (\"f 2\" Or 3))")]
        public void Parse_CorrectInputCaseInsensitive_ReturnsCorrectData(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(input.ToLowerInvariant(), parsed.ToString());
        }

        [Theory]
        [InlineData("\"or\" and \"and\"")]
        [InlineData("not \"not\"")]
        public void Parse_CorrectInputValuesAsOperators_ReturnsCorrectData(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var parsed = parser.Parse(input);

            // Assert
            Assert.Equal(input.ToLowerInvariant(), parsed.ToString());
        }

        [Theory]
        [InlineData("not not")]
        [InlineData("not Not")]
        [InlineData("a b")]
        [InlineData("(1 + a)")]
        [InlineData("1 not")]
        [InlineData("and 1")]
        public void Parse_InvalidInput_ThrowException(string input)
        {
            // Arrange
            var parser = new ExpressionParser();

            // Act
            var exception = Record.Exception(() => parser.Parse(input));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ExpressionParserException>(exception);
        }
    }
}
