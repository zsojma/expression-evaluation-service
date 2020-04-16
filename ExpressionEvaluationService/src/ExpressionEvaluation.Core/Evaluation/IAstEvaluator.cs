using ExpressionEvaluation.Core.Nodes.Binary;

namespace ExpressionEvaluation.Core.Evaluation
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
