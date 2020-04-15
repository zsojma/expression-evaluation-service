using ExpressionEvaluation.Core.Evaluation;
using ExpressionEvaluation.Core.Nodes;
using ExpressionEvaluation.Core.Nodes.Binary;
using ExpressionEvaluation.Core.Nodes.Unary;
using Xunit;

namespace ExpressionEvaluation.Core.Tests
{
    public class AstEvaluatorTests
    {
        [Fact]
        public void Evaluate_SimpleOneLevelAst_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Add, new UnaryValueNode(3)),
                    new BinaryNodeItem(BinaryOperatorType.Multiply, new UnaryValueNode(3))
                });

            var parser = new AstEvaluator();

            // Act
            var result = parser.Evaluate(input);

            // Assert
            Assert.Equal(11, result);
        }

        [Fact]
        public void Evaluate_SameOperatorLeftOrder_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Multiply, new UnaryValueNode(3)),
                    new BinaryNodeItem(BinaryOperatorType.Multiply, new UnaryValueNode(4))
                });

            var parser = new AstEvaluator();

            // Act
            var result = parser.Evaluate(input);

            // Assert
            Assert.Equal(24, result);
        }

        [Fact]
        public void Evaluate_SameOperatorRightOrder_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Power, new UnaryValueNode(3)),
                    new BinaryNodeItem(BinaryOperatorType.Power, new UnaryValueNode(2))
                });

            var parser = new AstEvaluator();

            // Act
            var result = parser.Evaluate(input);

            // Assert
            Assert.Equal(512, result);
        }

        [Fact]
        public void Evaluate_ComplexOneLevelAst_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Multiply, new UnaryValueNode(2)),
                    new BinaryNodeItem(BinaryOperatorType.Subtract, new UnaryValueNode(3)),
                    new BinaryNodeItem(BinaryOperatorType.Power, new UnaryValueNode(3)),
                    new BinaryNodeItem(BinaryOperatorType.Subtract, new UnaryValueNode(4)),
                    new BinaryNodeItem(BinaryOperatorType.Multiply, new UnaryValueNode(4)),
                });

            var parser = new AstEvaluator();

            // Act
            var result = parser.Evaluate(input);

            // Assert
            Assert.Equal(-39, result);
        }

        [Fact]
        public void Evaluate_PowerOfNegative_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryPrefixNode(UnaryOperatorType.Minus, new UnaryValueNode(2)),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Power, new UnaryValueNode(2))
                });

            var parser = new AstEvaluator();

            // Act
            var result = parser.Evaluate(input);

            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public void Evaluate_NegativePowerOfPositive_ReturnsCorrectResult()
        {
            // Arrange
            var power = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Power, new UnaryValueNode(2))
                });

            var input = new BinaryNode(new UnaryPrefixNode(UnaryOperatorType.Minus, new UnaryExpressionNode(power)));

            var parser = new AstEvaluator();

            // Act
            var result = parser.Evaluate(input);

            // Assert
            Assert.Equal(-4, result);
        }
    }
}