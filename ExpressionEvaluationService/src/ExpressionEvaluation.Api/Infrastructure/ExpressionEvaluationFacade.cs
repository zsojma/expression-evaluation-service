using ExpressionEvaluation.Core;
using ExpressionEvaluation.Core.Evaluation;
using ExpressionEvaluation.Core.Parsing;

namespace ExpressionEvaluation.Api.Infrastructure
{
    public class ExpressionEvaluationFacade
    {
        private readonly IAstParser _astParser;
        private readonly IExpressionEvaluator _evaluator;

        public ExpressionEvaluationFacade(IAstParser astParser, IExpressionEvaluator evaluator)
        {
            _astParser = astParser;
            _evaluator = evaluator;
        }

        public decimal Compute(string expr)
        {
            try
            {
                var ast = _astParser.Parse(expr);
                var result = _evaluator.Evaluate(null);
                return result;
            }
            catch (CoreException ex)
            {
                // format error message from Core
                throw new ExpressionEvaluationFacadeException(ex);
            }
        }
    }
}
