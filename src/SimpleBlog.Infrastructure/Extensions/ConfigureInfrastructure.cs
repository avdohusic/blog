using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlog.Infrastructure.Repositories;

namespace SimpleBlog.Infrastructure.Extensions;

public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructureConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSimleDbContext(configuration);

        services.AddScoped<IBlogRepository, BlogRepository>();

        return services;
    }
}