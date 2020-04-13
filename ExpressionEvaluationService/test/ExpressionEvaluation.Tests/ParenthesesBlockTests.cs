using ExpressionEvaluation.Core.Parsing;
using Xunit;

namespace ExpressionEvaluation.Tests
{
    public class ParenthesesBlockTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("(1)")]
        [InlineData("(1) + 1")]
        [InlineData("1 + (1)")]
        [InlineData("(1) + (1)")]
        [InlineData("(1 + 1)")]
        [InlineData("((1) + (1))")]
        [InlineData("1 + 1 + 1")]
        [InlineData("(1 + 1) + 1")]
        [InlineData("1 + (1 + 1)")]
        [InlineData("(1 + (1 + 1))")]
        [InlineData("((1 + 1) + 1)")]
        [InlineData("((1 + 1 + 1))")]
        [InlineData("1 + 1 + 1 + 1")]
        [InlineData("(1 + 1) + (1 + 1)")]
        [InlineData("(1 + 1 + 1) + 1")]
        [InlineData("1 + (1 + 1 + 1)")]
        [InlineData("1 + (1 + 1) + 1")]
        [InlineData("(1 + ((1 + 1) + 1))")]
        [InlineData("(1 + (1 + ((1) + 1)))")]
        public void Constructor_ValidBlocks_IsValid(string input)
        {
            // Arrange & Act
            var result = new ParenthesesBlockOld(input);

            // Assert
            Assert.True(result.IsValid());
            Assert.Equal(input, result.ToString());
        }


        [Theory]
        [InlineData("")]
        [InlineData("()")]
        [InlineData("( )")]
        [InlineData("((1)")]
        [InlineData("(1 + 1) - 2)")]
        [InlineData("(1 + (2) + (() - 2))")]
        public void Constructor_InvalidBlocks_IsNotValid(string input)
        {
            // Arrange & Act
            var result = new ParenthesesBlockOld(input);

            // Assert
            Assert.False(result.IsValid());
        }
    }
}
