using LogicalExpressionEvaluation.Core.Nodes.Binary;

namespace LogicalExpressionEvaluation.Core.Nodes.Unary
{
    /// <summary>
    /// Expression unary node, contains inner binary expression
    /// It is used like parenthesis
    /// </summary>
    public class UnaryExpressionNode : IUnaryNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expression">Inner binary node</param>
        public UnaryExpressionNode(BinaryNode expression)
        {
            Expression = expression;
        }

        /// <summary>
        /// Inner binary node
        /// </summary>
        public BinaryNode Expression { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({Expression})";
        }
    }
}
