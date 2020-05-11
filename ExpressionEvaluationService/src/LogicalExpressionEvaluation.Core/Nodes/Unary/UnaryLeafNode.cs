namespace LogicalExpressionEvaluation.Core.Nodes.Unary
{
    /// <summary>
    /// Leaf unary node, contains result
    /// </summary>
    public class UnaryLeafNode : IUnaryNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value of the node</param>
        public UnaryLeafNode(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// Value of the node
        /// </summary>
        public bool Value { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
