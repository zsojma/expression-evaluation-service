using NumericExpressionEvaluation.Core.Nodes.Unary;

namespace NumericExpressionEvaluation.Core.Nodes.Binary
{
    /// <summary>
    /// Binary node item used as right part of an expression
    /// Contains operator and unary node
    /// </summary>
    public class BinaryNodeItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="op">Operator</param>
        /// <param name="item">Unary node used as a right part of a binary node</param>
        public BinaryNodeItem(BinaryOperatorType op, IUnaryNode item)
        {
            Operator = op;
            Item = item;
        }

        /// <summary>
        /// Operator
        /// </summary>
        public BinaryOperatorType Operator { get; }

        /// <summary>
        /// Unary node used as a right part of a binary node
        /// </summary>
        public IUnaryNode Item { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Operator.ToDisplayString()}{Item}";
        }
    }
}
