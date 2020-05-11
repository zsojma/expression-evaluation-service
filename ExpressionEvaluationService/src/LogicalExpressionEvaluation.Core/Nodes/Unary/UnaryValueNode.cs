namespace LogicalExpressionEvaluation.Core.Nodes.Unary
{
    /// <summary>
    /// Value unary node, contains value to evaluate
    /// </summary>
    public class UnaryValueNode : IUnaryNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value of the node</param>
        public UnaryValueNode(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Value of the node
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }
}
