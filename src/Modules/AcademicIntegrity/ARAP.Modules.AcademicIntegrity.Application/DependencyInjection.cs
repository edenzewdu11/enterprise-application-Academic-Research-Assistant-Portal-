using Microsoft.Extensions.DependencyInjection;

namespace ARAP.Modules.AcademicIntegrity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAcademicIntegrityApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
