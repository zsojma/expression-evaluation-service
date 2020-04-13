using ExpressionEvaluation.Core.ExpressionNodes;

namespace ExpressionEvaluation.Core.Evaluation
{
    internal class ExpressionEvaluator : IExpressionEvaluator
    {
        public decimal Evaluate(BinaryNode input)
        {
            throw new ExpressionEvaluatorException("test");
        }
    }
}
