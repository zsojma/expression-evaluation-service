using System;

namespace ExpressionEvaluation.Core.Parsing
{
    /// <summary>
    /// Exception thrown when parsing of string to AST tree was not successful
    /// </summary>
    public class ExpressionParserException : CoreException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public ExpressionParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public ExpressionParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
