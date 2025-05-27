using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces;   

namespace Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ITextAnalyzer>(provider => new DeepSeekAnalyzer(
                config["TogetherAI:ApiKey"]
            ));

            services.AddSingleton<ITextAnalyzer>(provider => new GeminiApiClient(
                config["Google:ProjectId"]
            ));

            return services;
        }
    }
}
