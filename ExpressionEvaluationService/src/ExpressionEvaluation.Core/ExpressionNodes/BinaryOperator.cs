namespace ExpressionEvaluation.Core.ExpressionNodes
{
    public enum BinaryOperatorType
    {
        Unknown,
        Add,
        Subtract,
        Multiply,
        Divide,
        Power
    }

    public static class BinaryOperator
    {
        public static string ToDisplayString(this BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Add => "+",
                BinaryOperatorType.Subtract => "-",
                BinaryOperatorType.Multiply => "*",
                BinaryOperatorType.Divide => "/",
                BinaryOperatorType.Power => "^",
                _ => ""
            };
        }

        public static BinaryOperatorType CharToOperator(char input)
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
