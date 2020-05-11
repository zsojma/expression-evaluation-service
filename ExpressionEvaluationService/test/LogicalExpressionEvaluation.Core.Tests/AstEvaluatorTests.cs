using LogicalExpressionEvaluation.Core.Evaluation;
using LogicalExpressionEvaluation.Core.Nodes.Binary;
using LogicalExpressionEvaluation.Core.Nodes.Unary;
using Xunit;

namespace LogicalExpressionEvaluation.Core.Tests
{
    public class AstEvaluatorTests
    {
        [Fact]
        public void Evaluate_SimpleOneLevelAst_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode("w"),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("t")),
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("f"))
                });

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input, "wtf");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Evaluate_ComplexOneLevelAst_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode("w"),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("t")),
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("f")),
                    new BinaryNodeItem(BinaryOperatorType.Or, new UnaryValueNode("om")),
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("g"))
                });

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input, "omg");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Evaluate_SimpleTwoLevelsAst_ReturnsCorrectResult()
        {
            // Arrange
            var input = new BinaryNode(
                new UnaryValueNode("w"),
                new[]
                {
                    new BinaryNodeItem(
                        BinaryOperatorType.And,
                        new UnaryExpressionNode(
                            new BinaryNode(
                                new UnaryValueNode("t"),
                                new[]
                                {
                                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("f"))
                                }))),
                });

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input, "wtf");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Evaluate_ComplexTwoLevelsAst_ReturnsCorrectResult()
        {
            // Arrange
            var first = new BinaryNode(
                new UnaryValueNode("w"),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("t")),
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("f"))
                });
            var second = new BinaryNode(
                new UnaryValueNode("om"),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("g"))
                });
            var input = new BinaryNode(
                new UnaryExpressionNode(first),
                new[] { new BinaryNodeItem(BinaryOperatorType.Or, new UnaryExpressionNode(second)) });

            var evaluator = new AstEvaluator();

            // Act
            var result = evaluator.Evaluate(input, "omg");

            // Assert
            Assert.True(result);
        }
    }
}