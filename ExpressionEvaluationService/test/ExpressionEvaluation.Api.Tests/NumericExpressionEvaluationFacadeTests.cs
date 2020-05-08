using ExpressionEvaluation.Api.Infrastructure;
using Moq;
using NumericExpressionEvaluation.Core.Evaluation;
using NumericExpressionEvaluation.Core.Nodes.Binary;
using NumericExpressionEvaluation.Core.Parsing;
using Xunit;

namespace ExpressionEvaluation.Api.Tests
{
    public class NumericExpressionEvaluationFacadeTests
    {
        [Fact]
        public void Compute_CallsBothObjects()
        {
            // Arrange
            var astParserMock = new Mock<IExpressionParser>();
            var evaluationMock = new Mock<IAstEvaluator>();

            var facade = new NumericExpressionEvaluationFacade(astParserMock.Object, evaluationMock.Object);

            // Act
            facade.Evaluate("test");

            // Assert
            astParserMock.Verify(m => m.Parse(It.IsAny<string>()), Times.Once);
            evaluationMock.Verify(m => m.Evaluate(It.IsAny<BinaryNode>()), Times.Once);
        }

        [Fact]
        public void Compute_HandlesCoreException_ThrowsApiException()
        {
            // Arrange
            var astParserMock = new Mock<IExpressionParser>();
            astParserMock.Setup(m => m.Parse(It.IsAny<string>())).Throws(new ExpressionParserException("test"));
            var evaluationMock = new Mock<IAstEvaluator>();

            var facade = new NumericExpressionEvaluationFacade(astParserMock.Object, evaluationMock.Object);

            // Act
            var exception = Record.Exception(() => facade.Evaluate("test"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ExpressionEvaluationFacadeException>(exception);
        }
    }
}
