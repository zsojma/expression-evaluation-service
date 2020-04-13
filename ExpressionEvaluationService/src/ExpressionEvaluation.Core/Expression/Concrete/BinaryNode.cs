using ExpressionEvaluation.Core.Expression.Abstract;

namespace ExpressionEvaluation.Core.Expression.Concrete
{
    public class BinaryNode : ExpressionNode
    {
        public BinaryNode(ExpressionNodeType nodeType, ExpressionNode left, ExpressionNode right)
            : base(nodeType)
        {
            Left = left;
            Right = right;
        }

        public ExpressionNode Left { get; }
        public ExpressionNode Right { get; }
    }
}
