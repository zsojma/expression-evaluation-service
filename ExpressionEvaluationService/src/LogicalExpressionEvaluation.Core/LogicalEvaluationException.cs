using System;

namespace LogicalExpressionEvaluation.Core
{
    /// <summary>
    /// Exception thrown from Core module
    /// </summary>
    public abstract class LogicalEvaluationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        protected internal LogicalEvaluationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        protected internal LogicalEvaluationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
