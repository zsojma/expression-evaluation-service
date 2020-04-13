using System.Collections.Generic;

namespace ExpressionEvaluation.Core.ExpressionNodes
{
    public class BinaryNode
    {
        public BinaryNode(UnaryNode left)
        {
            Left = left;
        }

        public UnaryNode Left { get; }
        public List<(BinaryOperatorType, UnaryNode)> Rights { get; } = new List<(BinaryOperatorType, UnaryNode)>();

        public override string ToString()
        {
            var output = Left.ToString();
            foreach (var (op, right) in Rights)
            {
                output += $"{op.ToDisplayString()}{right}";
            }

            return output;
        }
    }
}
