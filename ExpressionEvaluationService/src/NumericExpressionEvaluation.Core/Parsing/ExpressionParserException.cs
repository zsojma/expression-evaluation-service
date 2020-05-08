﻿using System;

namespace NumericExpressionEvaluation.Core.Parsing
{
    /// <summary>
    /// Exception thrown when parsing of string to AST tree was not successful
    /// </summary>
    public class ExpressionParserException : NumericEvaluationException
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