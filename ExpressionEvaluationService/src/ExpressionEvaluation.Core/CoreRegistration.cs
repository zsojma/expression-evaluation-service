using System.Runtime.CompilerServices;
using ExpressionEvaluation.Core.Evaluation;
using ExpressionEvaluation.Core.Expression;
using ExpressionEvaluation.Core.Parsing;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("ExpressionEvaluation.Tests")]
namespace ExpressionEvaluation.Core
{
    /// <summary>
    /// Registration of Core project services
    /// </summary>
    public static class CoreRegistration
    {
        /// <summary>
        /// Registers services for Core project
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <returns>Services collection</returns>
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            services.AddTransient<ExpressionNodeFactory>();

            services.AddTransient<IAstParser, AstParser>();
            services.AddTransient<IExpressionEvaluator, ExpressionEvaluator>();

            return services;
        }
    }
}
