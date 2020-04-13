namespace ExpressionEvaluation.Core.Infrastructure
{
    /// <summary>
    /// Evaluates expressions in string format
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Computes a number from string expression
        /// </summary>
        /// <param name="expr">The input expression</param>
        /// <returns>Result number</returns>
        /// <exception cref="EvaluatorException">Thrown when input expression cannot be parsed.</exception>
        decimal Compute(string expr);
    }
}
