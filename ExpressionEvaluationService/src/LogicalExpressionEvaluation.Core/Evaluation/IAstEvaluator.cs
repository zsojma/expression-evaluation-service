using LogicalExpressionEvaluation.Core.Nodes.Binary;

namespace LogicalExpressionEvaluation.Core.Evaluation
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
        /// <param name="input">Input on which the evaluation should be performed</param>
        /// <returns>Result of the evaluation</returns>
        bool Evaluate(BinaryNode root, string input);
    }
}