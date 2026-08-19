using GitReview.Core.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace GitReview.Cli.Composition
{
    internal static class OutputStrategyCollectionExtensions
    {
        public static IServiceCollection AddOutputStrategies(this IServiceCollection services)
        {
            services.AddTransient<IOutputStrategy, PromptOutputStrategy>();
            services.AddTransient<IOutputStrategy, RawDiffOutputStrategy>();
            services.AddTransient<IOutputStrategy, AiReviewOutputStrategy>();

            return services;
        }
    }
}
