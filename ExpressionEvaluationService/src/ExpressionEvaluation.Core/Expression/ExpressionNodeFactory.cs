using ExpressionEvaluation.Core.Expression.Abstract;
using ExpressionEvaluation.Core.Expression.Concrete;

namespace ExpressionEvaluation.Core.Expression
{
    internal class ExpressionNodeFactory
    {
        public ExpressionNode Plus(ExpressionNode value)
        {
            return new UnaryNode(ExpressionNodeType.Plus, value);
        }

        public ExpressionNode Minus(ExpressionNode value)
        {
            return new UnaryNode(ExpressionNodeType.Minus, value);
        }

        public ExpressionNode Percentage(ExpressionNode value)
        {
            return new UnaryNode(ExpressionNodeType.Percentage, value);
        }

        public ExpressionNode Add(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Add, left, right);
        }

        public ExpressionNode Subtract(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Subtract, left, right);
        }

        public ExpressionNode Multiply(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Multiply, left, right);
        }

        public ExpressionNode Divide(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Divide, left, right);
        }

        public ExpressionNode Power(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Power, left, right);
        }
    }
}
