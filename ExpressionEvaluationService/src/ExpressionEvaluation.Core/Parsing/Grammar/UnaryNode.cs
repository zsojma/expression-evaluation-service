namespace ExpressionEvaluation.Core.Parsing.Grammar
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
        public UnaryExpressionNode(BinaryNode e)
        {
            E = e;
        }

        public BinaryNode E { get; }

        public override string ToString()
        {
            return $"({E})";
        }
    }

    internal class UnaryNegativeNode : UnaryNode
    {
        public UnaryNegativeNode(UnaryNode p)
        {
            P = p;
        }

        public UnaryNode P { get; }

        public override string ToString()
        {
            return $"-{P}";
        }
    }
}
