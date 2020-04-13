using ExpressionEvaluation.Api.Infrastructure;
using ExpressionEvaluation.Core.Evaluation;
using ExpressionEvaluation.Core.Expression.Abstract;
using ExpressionEvaluation.Core.Parsing;
using Moq;
using Xunit;

namespace ExpressionEvaluation.Api.Tests
{
    public class ExpressionEvaluationFacadeTests
    {
        [Fact]
        public void Compute_HandlesAstParserException_ThrowsException()
        {
            // Arrange
            var astParserMock = new Mock<IAstParser>();
            astParserMock.Setup(m => m.Parse(It.IsAny<string>())).Throws(new AstParserException("test"));
            var evaluationMock = new Mock<IExpressionEvaluator>();

            var facade = new ExpressionEvaluationFacade(astParserMock.Object, evaluationMock.Object);

            // Act
            var exception = Record.Exception(() => facade.Compute("test"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ExpressionEvaluationFacadeException>(exception);
        }

        [Fact]
        public void Compute_HandlesExpressionEvalutorException_ThrowsException()
        {
            // Arrange
            var astParserMock = new Mock<IAstParser>();
            var evaluationMock = new Mock<IExpressionEvaluator>();
            evaluationMock.Setup(m => m.Evaluate(It.IsAny<ExpressionNode>())).Throws(new ExpressionEvaluatorException("test"));

            var facade = new ExpressionEvaluationFacade(astParserMock.Object, evaluationMock.Object);

            // Act
            var exception = Record.Exception(() => facade.Compute("test"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ExpressionEvaluationFacadeException>(exception);
        }
    }
}
