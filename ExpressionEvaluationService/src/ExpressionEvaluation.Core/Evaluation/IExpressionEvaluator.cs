using ExpressionEvaluation.Core.Parsing.Grammar;

namespace ExpressionEvaluation.Core.Evaluation
{
    public interface IExpressionEvaluator
    {
        // <exception cref="ExpressionEvaluatorException">Thrown when input cannot be evaluated.</exception>
        decimal Evaluate(BinaryNode input);
    }
}
