using ExpressionEvaluation.Core.Expression.Abstract;

namespace ExpressionEvaluation.Core.Expression.Concrete
{
    public class UnaryNode : ExpressionNode
    {
        public UnaryNode(ExpressionNodeType nodeType, ExpressionNode operand)
            : base(nodeType)
        {
            Operand = operand;
        }

        public ExpressionNode Operand { get; }
    }
}
