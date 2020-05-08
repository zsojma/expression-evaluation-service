using NumericExpressionEvaluation.Core.Nodes.Binary;

namespace NumericExpressionEvaluation.Core.Evaluation
{
    /// <summary>
    /// Evaluates expression defined by AST tree
    /// </summary>
    public interface IAstEvaluator
    {
        /// <summary>
        /// Evaluates expression defined by AST tree
        /// </summary>
        /// <param name="root">Root of input AST tree</param>
        /// <returns>Result of the evaluation</returns>
        double Evaluate(BinaryNode root);
    }
}
