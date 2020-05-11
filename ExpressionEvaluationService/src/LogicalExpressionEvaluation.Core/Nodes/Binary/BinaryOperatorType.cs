namespace LogicalExpressionEvaluation.Core.Nodes.Binary
{
    /// <summary>
    /// Types of binary operator
    /// </summary>
    public enum BinaryOperatorType
    {
        /// <summary>
        /// Used when binary operator cannot be determined
        /// </summary>
        Unknown,

        /// <summary>
        /// Result sign used with left part of binary nodes
        /// </summary>
        Result,

        /// <summary>
        /// And operator
        /// </summary>
        And,

        /// <summary>
        /// Or operator
        /// </summary>
        Or
    }
}
