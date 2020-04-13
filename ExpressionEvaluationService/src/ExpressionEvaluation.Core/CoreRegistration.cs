using ExpressionEvaluation.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<IEvaluator, Evaluator>();
            
            return services;
        }
    }
}
