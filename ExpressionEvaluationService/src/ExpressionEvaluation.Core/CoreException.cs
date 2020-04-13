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
        protected internal CoreException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
