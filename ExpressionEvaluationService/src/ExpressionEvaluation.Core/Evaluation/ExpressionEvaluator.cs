using ExpressionEvaluation.Core.Expression.Abstract;

namespace ExpressionEvaluation.Core.Evaluation
{
    internal class ExpressionEvaluator : IExpressionEvaluator
    {
        public decimal Evaluate(ExpressionNode input)
        {
            throw new ExpressionEvaluatorException("test");
        }
    }
}
