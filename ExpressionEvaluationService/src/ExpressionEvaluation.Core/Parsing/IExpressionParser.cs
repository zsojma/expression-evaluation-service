using ExpressionEvaluation.Core.Nodes;
using ExpressionEvaluation.Core.Nodes.Binary;

namespace ExpressionEvaluation.Core.Parsing
{
    /// <summary>
    /// Parses expression in input string to AST tree
    /// </summary>
    public interface IExpressionParser
    {
        /// <summary>
        /// Parses expression in input string to AST tree
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>Parsed AST tree</returns>
        /// <exception cref="ExpressionParserException">Thrown when input cannot be evaluated.</exception>
        BinaryNode Parse(string input);
    }
}
