using ExpressionEvaluation.Core.ExpressionNodes;

namespace ExpressionEvaluation.Core.Evaluation
{
    public interface IExpressionEvaluator
    {
        // <exception cref="ExpressionEvaluatorException">Thrown when input cannot be evaluated.</exception>
        decimal Evaluate(BinaryNode root);
    }
}
