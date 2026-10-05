using Agrimitra.Infrastructure.Persistence;
using Agrimitra.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Agrimitra.Infrastructure.DependencyInjection;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddAgrimitraPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddDbContext<AgrimitraDbContext>((sp, options) => options
            .UseNpgsql(connectionString, npgsql =>
            {
                npgsql.UseNetTopologySuite();
                npgsql.UseVector();
                npgsql.EnableRetryOnFailure(3);
                npgsql.MigrationsAssembly(typeof(AgrimitraDbContext).Assembly.FullName);
            })
            .AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>()));
        return services;
    }
}
