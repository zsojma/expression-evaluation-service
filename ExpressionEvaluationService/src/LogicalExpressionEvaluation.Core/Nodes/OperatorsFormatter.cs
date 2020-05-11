using LogicalExpressionEvaluation.Core.Nodes.Binary;
using LogicalExpressionEvaluation.Core.Nodes.Unary;

namespace LogicalExpressionEvaluation.Core.Nodes
{
    internal static class OperatorsFormatter
    {
        public static string ToDisplayString(this UnaryOperatorType op)
        {
            return op switch
            {
                UnaryOperatorType.Not => "not",
                _ => ""
            };
        }

        public static string ToDisplayString(this BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Result => "=",
                BinaryOperatorType.And => "and",
                BinaryOperatorType.Or => "or",
                _ => ""
            };
        }

        public static UnaryOperatorType StringToUnaryOperator(string input)
        {
            return input.ToLowerInvariant() switch
            {
                "not" => UnaryOperatorType.Not,
                "!" => UnaryOperatorType.Not,
                _ => UnaryOperatorType.Unknown
            };
        }

        public static BinaryOperatorType StringToBinaryOperator(string input)
        {
            return input.ToLowerInvariant() switch
            {
                "and" => BinaryOperatorType.And,
                "&&" => BinaryOperatorType.And,
                "&" => BinaryOperatorType.And,
                "or" => BinaryOperatorType.Or,
                "||" => BinaryOperatorType.Or,
                "|" => BinaryOperatorType.Or,
                _ => BinaryOperatorType.Unknown
            };
        }
    }
}
