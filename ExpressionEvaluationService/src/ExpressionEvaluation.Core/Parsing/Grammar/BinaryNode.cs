using System.Collections.Generic;

namespace ExpressionEvaluation.Core.Parsing.Grammar
{
    public class BinaryNode
    {
        public BinaryNode(UnaryNode p)
        {
            P = p;
        }

        public UnaryNode P { get; }
        public List<(BinaryOperatorType, UnaryNode)> BP { get; } = new List<(BinaryOperatorType, UnaryNode)>();

        public override string ToString()
        {
            var output = P.ToString();
            foreach (var (b, innerP) in BP)
            {
                output += $"{b.ToDisplayString()}{innerP}";
            }

            return output;
        }
    }
}
