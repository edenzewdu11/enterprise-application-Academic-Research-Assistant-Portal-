using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.Notifications.Infrastructure;

namespace ARAP.Modules.Notifications.Api;

public static class ModuleServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddNotificationsInfrastructure(configuration);

        return services;
    }
}
