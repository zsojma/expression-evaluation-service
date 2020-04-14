using System.Collections.Generic;
using System.Linq;

namespace ExpressionEvaluation.Core.ExpressionNodes
{
    public class BinaryNodeItem
    {
        public BinaryNodeItem(BinaryOperatorType op, UnaryNode right)
        {
            Operator = op;
            Item = right;
        }

        public BinaryOperatorType Operator { get; }
        public UnaryNode Item { get; }

        public override string ToString()
        {
            return $"{Operator.ToDisplayString()}{Item}";
        }
    }

    public class BinaryNode
    {
        public BinaryNode(UnaryNode left, IEnumerable<BinaryNodeItem> rights = null)
        {
            Left = new BinaryNodeItem(BinaryOperatorType.Result, left);
            Rights = rights != null ? rights.ToList() : new List<BinaryNodeItem>();
        }

        public BinaryNodeItem Left { get; }
        public List<BinaryNodeItem> Rights { get; }

        public override string ToString()
        {
            return Rights.Aggregate(Left.Item.ToString(), (current, item) => current + item);
        }
    }
}
