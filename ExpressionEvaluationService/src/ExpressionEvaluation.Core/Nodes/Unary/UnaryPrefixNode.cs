namespace ExpressionEvaluation.Core.Nodes.Unary
{
    /// <summary>
    /// Unary node with unary operator
    /// </summary>
    public class UnaryPrefixNode : IUnaryNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="op">Operator used as a prefix</param>
        /// <param name="value">Value of the node</param>
        public UnaryPrefixNode(UnaryOperatorType op, IUnaryNode value)
        {
            Operator = op;
            Value = value;
        }

        /// <summary>
        /// Operator used as a prefix of the <see cref="Value"/>
        /// </summary>
        public UnaryOperatorType Operator { get; }

        /// <summary>
        /// Value of the node
        /// </summary>
        public IUnaryNode Value { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Operator.ToDisplayString()}{Value}";
        }
    }
}
