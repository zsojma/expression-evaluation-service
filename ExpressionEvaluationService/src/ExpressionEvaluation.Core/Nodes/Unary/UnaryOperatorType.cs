namespace ExpressionEvaluation.Core.Nodes.Unary
{
    /// <summary>
    /// Types of unary operator
    /// </summary>
    public enum UnaryOperatorType
    {
        /// <summary>
        /// Used when the operator cannot be determined
        /// </summary>
        Unknown,

        /// <summary>
        /// Plus sign before unary operator
        /// </summary>
        Plus,

        /// <summary>
        /// Minus sign before unary operator
        /// </summary>
        Minus
    }
}
