using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.AcademicIntegrity.Infrastructure;

namespace ARAP.Modules.AcademicIntegrity.Api;

public static class ModuleServiceCollectionExtensions
{
    public static IServiceCollection AddAcademicIntegrityModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAcademicIntegrityInfrastructure(configuration);
        
        return services;
    }
}
