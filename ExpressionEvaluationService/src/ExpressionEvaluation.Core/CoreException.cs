using System;

namespace ExpressionEvaluation.Core
{
    /// <summary>
    /// Exception thrown from Core module
    /// </summary>
    public abstract class CoreException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        protected internal CoreException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        protected internal CoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
