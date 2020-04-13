using System;

namespace ExpressionEvaluation.Core.Parsing
{
    /// <summary>
    /// Exception thrown when parsing of string to AST tree was not successful
    /// </summary>
    public class AstParserException : CoreException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AstParserException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
