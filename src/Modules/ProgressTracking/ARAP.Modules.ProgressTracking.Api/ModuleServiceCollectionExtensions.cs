using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.ProgressTracking.Infrastructure;

namespace ARAP.Modules.ProgressTracking.Api;

public static class ModuleServiceCollectionExtensions
{
    public static IServiceCollection AddProgressTrackingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddProgressTrackingInfrastructure(configuration);
        
        return services;
    }
}
