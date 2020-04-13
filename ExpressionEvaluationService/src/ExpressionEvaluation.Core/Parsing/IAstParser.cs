using ExpressionEvaluation.Core.Parsing.Grammar;

namespace ExpressionEvaluation.Core.Parsing
{
    public interface IAstParser
    {
        // <exception cref="AstParserException">Thrown when input cannot be evaluated.</exception>
        BinaryNode Parse(string input);
    }
}
