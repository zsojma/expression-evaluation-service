using System;

namespace NumericExpressionEvaluation.Core
{
    /// <summary>
    /// Exception thrown from Core module
    /// </summary>
    public abstract class NumericEvaluationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        protected internal NumericEvaluationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        protected internal NumericEvaluationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
