using System;

namespace ExpressionEvaluation.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when evaluation of expression is not successful
    /// </summary>
    public class EvaluatorException : InvalidOperationException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public EvaluatorException()
            : base("Expression error.")
        {
        }
    }
}
