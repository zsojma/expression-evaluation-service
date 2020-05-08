namespace NumericExpressionEvaluation.Core.Nodes.Binary
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
        /// Add operator
        /// </summary>
        Add,

        /// <summary>
        /// Subtract operator
        /// </summary>
        Subtract,

        /// <summary>
        /// Multiply operator
        /// </summary>
        Multiply,

        /// <summary>
        /// Division operator
        /// </summary>
        Divide,

        /// <summary>
        /// Power operator
        /// </summary>
        Power
    }
}
