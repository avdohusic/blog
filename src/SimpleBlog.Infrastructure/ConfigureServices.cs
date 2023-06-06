using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleBlog.Domain.Constants;
using SimpleBlog.Infrastructure.Repositories;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var migrationsAssembly = typeof(InfrastructureAssembly).GetTypeInfo().Assembly.GetName().Name;

        services.AddDbContext<SimpleBlogDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString(ConnectionStringKeys.SimpleBlogConfigurationKey),
                sql => sql.MigrationsAssembly(migrationsAssembly)));

        services.AddIdentityCore<UserIdentity>()
                .AddEntityFrameworkStores<SimpleBlogDbContext>();

        services.AddScoped<IBlogRepository, BlogRepository>();

        return services;
    }
}