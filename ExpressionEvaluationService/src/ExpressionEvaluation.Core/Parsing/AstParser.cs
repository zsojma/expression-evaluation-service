using ExpressionEvaluation.Core.Expression;
using ExpressionEvaluation.Core.Expression.Abstract;

namespace ExpressionEvaluation.Core.Parsing
{
    internal class AstParser : IAstParser
    {
        private readonly ExpressionNodeFactory _nodeFactory;

        public AstParser(ExpressionNodeFactory nodeFactory)
        {
            _nodeFactory = nodeFactory;
        }

        public ExpressionNode Parse(string input)
        {
            return null;
        }
    }
}
