namespace ExpressionEvaluation.Core.Nodes.Unary
{
    /// <summary>
    /// Value unary node, contains numeric result
    /// </summary>
    public class UnaryValueNode : IUnaryNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value of the node</param>
        public UnaryValueNode(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Value of the node
        /// </summary>
        public decimal Value { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value.ToString("0.##");
        }
    }
}
