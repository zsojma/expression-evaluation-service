namespace ExpressionEvaluation.Core.ExpressionNodes
{
    public abstract class UnaryNode
    {
    }

    internal class UnaryValueNode : UnaryNode
    {
        public UnaryValueNode(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }

        public override string ToString()
        {
            return Value.ToString("0.##");
        }
    }

    internal class UnaryExpressionNode : UnaryNode
    {
        public UnaryExpressionNode(BinaryNode expression)
        {
            Expression = expression;
        }

        public BinaryNode Expression { get; }

        public override string ToString()
        {
            return $"({Expression})";
        }
    }

    internal class UnaryPrefixNode : UnaryNode
    {
        public UnaryPrefixNode(UnaryOperatorType op, UnaryNode value)
        {
            Operator = op;
            Value = value;
        }

        public UnaryOperatorType Operator { get; }
        public UnaryNode Value { get; }

        public override string ToString()
        {
            return $"{Operator.ToDisplayString()}{Value}";
        }
    }
}
