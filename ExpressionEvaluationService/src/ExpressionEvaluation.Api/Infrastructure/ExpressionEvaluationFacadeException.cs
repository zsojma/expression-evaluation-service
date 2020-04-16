using System;

namespace ExpressionEvaluation.Api.Infrastructure
{
    /// <summary>
    /// Exception thrown when evaluation of expression is not successful
    /// </summary>
    public class ExpressionEvaluationFacadeException : InvalidOperationException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ExpressionEvaluationFacadeException(Exception innerException)
            : base($"Expression error: {innerException.Message}", innerException)
        {
        }
    }
}
