using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using NumericExpressionEvaluation.Core.Evaluation;
using NumericExpressionEvaluation.Core.Parsing;

[assembly:InternalsVisibleTo("NumericExpressionEvaluation.Core.Tests")]
namespace NumericExpressionEvaluation.Core
{
    /// <summary>
    /// Registration of Core project services
    /// </summary>
    public static class NumericEvaluationRegistration
    {
        /// <summary>
        /// Registers services for Core project
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <returns>Services collection</returns>
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IExpressionParser, ExpressionParser>();
            services.AddTransient<IAstEvaluator, AstEvaluator>();

            return services;
        }
    }
}
