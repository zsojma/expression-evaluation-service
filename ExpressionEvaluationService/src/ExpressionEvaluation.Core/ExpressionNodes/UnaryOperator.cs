namespace ExpressionEvaluation.Core.ExpressionNodes
{
    public enum UnaryOperatorType
    {
        Unknown,
        Plus,
        Minus
    }

    public static class UnaryOperator
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

        public static UnaryOperatorType CharToOperator(char input)
        {
            return input switch
            {
                '+' => UnaryOperatorType.Plus,
                '-' => UnaryOperatorType.Minus,
                _ => UnaryOperatorType.Unknown
            };
        }
    }
}
