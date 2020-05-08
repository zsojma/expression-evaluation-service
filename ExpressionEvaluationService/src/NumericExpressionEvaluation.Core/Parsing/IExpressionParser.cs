using NumericExpressionEvaluation.Core.Nodes.Binary;

namespace NumericExpressionEvaluation.Core.Parsing
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
