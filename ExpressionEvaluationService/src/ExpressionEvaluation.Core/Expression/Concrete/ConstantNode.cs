using ExpressionEvaluation.Core.Expression.Abstract;

namespace ExpressionEvaluation.Core.Expression.Concrete
{
    public class ConstantNode : ExpressionNode
    {
        public ConstantNode(decimal value)
            : base(ExpressionNodeType.Constant)
        {
            Value = value;
        }

        public decimal Value { get; }
    }
}
