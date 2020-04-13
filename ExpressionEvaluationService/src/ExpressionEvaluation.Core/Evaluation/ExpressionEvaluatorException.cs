using System;

namespace ExpressionEvaluation.Core.Evaluation
{
    /// <summary>
    /// Exception thrown when evaluation of expression is not successful
    /// </summary>
    public class ExpressionEvaluatorException : CoreException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExpressionEvaluatorException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
