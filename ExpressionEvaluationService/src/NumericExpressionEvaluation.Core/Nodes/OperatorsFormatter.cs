using NumericExpressionEvaluation.Core.Nodes.Binary;
using NumericExpressionEvaluation.Core.Nodes.Unary;

namespace NumericExpressionEvaluation.Core.Nodes
{
    internal static class OperatorsFormatter
    {
        public static string ToDisplayString(this UnaryOperatorType op)
        {
            return op switch
            {
                UnaryOperatorType.Plus => "+",
                UnaryOperatorType.Minus => "-",
                _ => ""
            };
        }

        public static string ToDisplayString(this BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Result => "=",
                BinaryOperatorType.Add => "+",
                BinaryOperatorType.Subtract => "-",
                BinaryOperatorType.Multiply => "*",
                BinaryOperatorType.Divide => "/",
                BinaryOperatorType.Power => "^",
                _ => ""
            };
        }

        public static UnaryOperatorType CharToUnaryOperator(char input)
        {
            return input switch
            {
                '+' => UnaryOperatorType.Plus,
                '-' => UnaryOperatorType.Minus,
                _ => UnaryOperatorType.Unknown
            };
        }

        public static BinaryOperatorType CharToBinaryOperator(char input)
        {
            return input switch
            {
                '+' => BinaryOperatorType.Add,
                '-' => BinaryOperatorType.Subtract,
                '*' => BinaryOperatorType.Multiply,
                '/' => BinaryOperatorType.Divide,
                '^' => BinaryOperatorType.Power,
                _ => BinaryOperatorType.Unknown
            };
        }
    }
}
