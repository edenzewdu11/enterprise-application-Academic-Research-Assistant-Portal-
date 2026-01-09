using Microsoft.Extensions.DependencyInjection;

namespace ARAP.Modules.ProgressTracking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProgressTrackingApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        return services;
    }
}
