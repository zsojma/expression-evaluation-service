using System.Runtime.CompilerServices;
using ExpressionEvaluation.Api.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("ExpressionEvaluation.Api.Tests")]
namespace ExpressionEvaluation.Api
{
    /// <summary>
    /// Registration of Core project services
    /// </summary>
    public static class ApiRegistration
    {
        /// <summary>
        /// Registers services for API project
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <returns>Services collection</returns>
        public static IServiceCollection RegisterApiServices(this IServiceCollection services)
        {
            services.AddTransient<NumericExpressionEvaluationFacade>();

            return services;
        }
    }
}