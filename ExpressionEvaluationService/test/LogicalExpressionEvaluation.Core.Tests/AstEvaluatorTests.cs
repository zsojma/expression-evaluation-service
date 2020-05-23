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

        [Fact]
        public void Evaluate_ComplexTwoLevelAstWithParenthesis_ReturnsCorrectResult()
        {
            // Arrange
            var inner = new UnaryExpressionNode(
                new BinaryNode(
                    new UnaryValueNode("Zdenek"),
                    new[]
                    {
                        new BinaryNodeItem(BinaryOperatorType.Or, new UnaryValueNode("Franta"))
                    }));

            var input = new BinaryNode(
                new UnaryValueNode("Hello"),
                new[]
                {
                    new BinaryNodeItem(BinaryOperatorType.And, new UnaryValueNode("\"I am\"")),
                    new BinaryNodeItem(BinaryOperatorType.And, inner)
                });

            var evaluator = new AstEvaluator();

            // Act
            var result1 = evaluator.Evaluate(input, "Hello, I am Franta");
            var result2 = evaluator.Evaluate(input, "Hello, I am Zdenek");

            // Assert
            Assert.True(result1);
            Assert.True(result2);
        }
    }
}