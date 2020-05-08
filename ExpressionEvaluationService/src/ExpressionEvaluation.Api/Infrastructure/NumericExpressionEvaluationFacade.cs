using NumericExpressionEvaluation.Core;
using NumericExpressionEvaluation.Core.Evaluation;
using NumericExpressionEvaluation.Core.Parsing;

namespace ExpressionEvaluation.Api.Infrastructure
{
    /// <summary>
    /// Evaluates expressions
    /// </summary>
    public class NumericExpressionEvaluationFacade
    {
        private readonly IExpressionParser _astParser;
        private readonly IAstEvaluator _evaluator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="astParser">Parser </param>
        /// <param name="evaluator"></param>
        public NumericExpressionEvaluationFacade(IExpressionParser astParser, IAstEvaluator evaluator)
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
            catch (NumericEvaluationException ex)
            {
                // format error message from Core
                throw new ExpressionEvaluationFacadeException(ex);
            }
        }
    }
}
