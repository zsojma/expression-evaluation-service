using System;

namespace ExpressionEvaluation.Core.Infrastructure
{
    internal class Evaluator : IEvaluator
    {
        public decimal Compute(string expr)
        {
            throw new EvaluatorException();
        }
    }
}
