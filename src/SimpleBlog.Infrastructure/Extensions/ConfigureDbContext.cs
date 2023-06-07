using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlog.Domain.Constants;
using SimpleBlog.Infrastructure.Data;
using System.Reflection;

namespace SimpleBlog.Infrastructure.Extensions;
public static class ConfigureDbContext
{
    public static IServiceCollection AddSimleDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var migrationsAssembly = typeof(InfrastructureAssembly).GetTypeInfo().Assembly.GetName().Name;

        services.AddDbContext<SimpleBlogDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString(ConnectionStringKeys.SimpleBlogConfigurationKey),
                sql => sql.MigrationsAssembly(migrationsAssembly)));

        services.AddIdentityCore<UserIdentity>()
                .AddEntityFrameworkStores<SimpleBlogDbContext>();

        return services;
    }
}
