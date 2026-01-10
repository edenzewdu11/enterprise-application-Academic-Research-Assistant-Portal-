using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.ProgressTracking.Application;
using ARAP.Modules.ProgressTracking.Domain.Repositories;
using ARAP.Modules.ProgressTracking.Infrastructure.Persistence;
using ARAP.Modules.ProgressTracking.Infrastructure.Persistence.Repositories;

namespace ARAP.Modules.ProgressTracking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProgressTrackingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add application services
        services.AddProgressTrackingApplication();

        // Configure database
        var useInMemoryValue = configuration["UseInMemoryDatabase"];
        var useInMemory = useInMemoryValue == "true" || useInMemoryValue == "True";
        
        if (useInMemory)
        {
            services.AddDbContext<ProgressTrackingDbContext>(options =>
                options.UseInMemoryDatabase("ProgressTrackingDb"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("ProgressTrackingDb");
            services.AddDbContext<ProgressTrackingDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        // Register repositories
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();

        return services;
    }
}
