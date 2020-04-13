using ExpressionEvaluation.Core.Expression.Abstract;
using ExpressionEvaluation.Core.Expression.Concrete;

namespace ExpressionEvaluation.Core.Expression
{
    internal class ExpressionNodeFactory
    {
        public ExpressionNode CreateConstant(decimal value)
        {
            return new ConstantNode(value);
        }

        public ExpressionNode CreateMinus(ExpressionNode value)
        {
            return new UnaryNode(ExpressionNodeType.Minus, value);
        }

        public ExpressionNode CreatePercentage(ExpressionNode value)
        {
            return new UnaryNode(ExpressionNodeType.Percentage, value);
        }

        public ExpressionNode CreateAdd(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Add, left, right);
        }

        public ExpressionNode CreateSubtract(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Subtract, left, right);
        }

        public ExpressionNode CreateMultiply(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Multiply, left, right);
        }

        public ExpressionNode CreateDivide(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Divide, left, right);
        }

        public ExpressionNode CreatePower(ExpressionNode left, ExpressionNode right)
        {
            return new BinaryNode(ExpressionNodeType.Power, left, right);
        }
    }
}
