using System;
using NumericExpressionEvaluation.Core.Evaluation;
using NumericExpressionEvaluation.Core.Nodes.Binary;
using NumericExpressionEvaluation.Core.Nodes.Unary;
using Xunit;

namespace NumericExpressionEvaluation.Core.Tests
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

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

            // Assert
            Assert.Equal(11, result);
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

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

            // Assert
            Assert.Equal(-39, result);
        }

        [Fact]
        public void Evaluate_SimpleTwoLevelsAst_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(
                        BinaryOperatorType.Multiply,
                        new UnaryExpressionNode(
                            new BinaryNode(
                                new UnaryValueNode(2),
                                new[]
                                {
                                    new BinaryNodeItem(BinaryOperatorType.Subtract, new UnaryValueNode(3))
                                }))),
                });

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

            // Assert
            Assert.Equal(-2, result);
        }

        [Fact]
        public void Evaluate_ComplexTwoLevelsAst_ReturnsCorrectResult()
        {
            // Arrange
            var first = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Add, new UnaryValueNode(3)),
                    new BinaryNodeItem(BinaryOperatorType.Multiply, new UnaryValueNode(3))
                });
            var second = new BinaryNode(
                new UnaryValueNode(2),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Subtract, new UnaryValueNode(3))
                });
            var input = new BinaryNode(
                new UnaryExpressionNode(first),
                new[] { new BinaryNodeItem(BinaryOperatorType.Multiply, new UnaryExpressionNode(second)) });

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

            // Assert
            Assert.Equal(-11, result);
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

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

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

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

            // Assert
            Assert.Equal(512, result);
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

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

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

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

            // Assert
            Assert.Equal(-Math.Pow(2, 2), result);
        }

        [Fact]
        public void Evaluate_PowerOfNegativeExpression_ReturnsCorrectResult()
        {
            // Arrange
            var subtract = new BinaryNode(
                new UnaryValueNode(1),
                new[] { new BinaryNodeItem(BinaryOperatorType.Subtract, new UnaryValueNode(2)) });

            var input = new BinaryNode(
                new UnaryExpressionNode(subtract),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.Power, new UnaryValueNode(3))
                });

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input);

            // Assert
            Assert.Equal(Math.Pow(1-2, 3), result);
        }
    }
}