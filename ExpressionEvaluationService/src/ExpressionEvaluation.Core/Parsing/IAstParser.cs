using ExpressionEvaluation.Core.Expression.Abstract;

namespace ExpressionEvaluation.Core.Parsing
{
    public interface IAstParser
    {
        // <exception cref="AstParserException">Thrown when input cannot be evaluated.</exception>
        ExpressionNode Parse(string input);
    }
}
