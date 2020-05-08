using System.Collections.Generic;
using System.Linq;
using NumericExpressionEvaluation.Core.Nodes.Unary;

namespace NumericExpressionEvaluation.Core.Nodes.Binary
{
    /// <summary>
    /// Binary node of AST tree. Consist of left part and multiple right parts
    /// where each of them is on the same evaluation level.
    /// </summary>
    public class BinaryNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="left">Left part of the node</param>
        /// <param name="rights">Right parts of the node, each with operator</param>
        public BinaryNode(IUnaryNode left, IEnumerable<BinaryNodeItem>? rights = null)
        {
            Left = new BinaryNodeItem(BinaryOperatorType.Result, left);
            Rights = rights != null ? rights.ToList() : new List<BinaryNodeItem>();
        }

        /// <summary>
        /// Left part of the node
        /// </summary>
        public BinaryNodeItem Left { get; }

        /// <summary>
        /// Right parts of the node
        /// </summary>
        public IReadOnlyList<BinaryNodeItem> Rights { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Rights.Aggregate(Left.Item.ToString(), (current, item) => current + item) ?? string.Empty;
        }
    }
}
