using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("LogicalExpressionEvaluation.Core.Tests")]
namespace LogicalExpressionEvaluation.Core
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
            return services;
        }
    }
}