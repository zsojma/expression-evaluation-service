using ExpressionEvaluation.Core;
using ExpressionEvaluation.Core.Evaluation;
using ExpressionEvaluation.Core.Parsing;

namespace ExpressionEvaluation.Api.Infrastructure
{
    /// <summary>
    /// Evaluates expressions
    /// </summary>
    public class ExpressionEvaluationFacade
    {
        private readonly IExpressionParser _astParser;
        private readonly IAstEvaluator _evaluator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="astParser">Parser </param>
        /// <param name="evaluator"></param>
        public ExpressionEvaluationFacade(IExpressionParser astParser, IAstEvaluator evaluator)
        {
            _astParser = astParser;
            _evaluator = evaluator;
        }

        /// <summary>
        /// Evaluates expression in input expression
        /// </summary>
        /// <param name="expr">Input expression to evaluate</param>
        /// <returns>Evaluated value</returns>
        public double Evaluate(string expr)
        {
            try
            {
                var ast = _astParser.Parse(expr);
                var result = _evaluator.Evaluate(ast);
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
