using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.Notifications.Application;
using ARAP.Modules.Notifications.Domain.Repositories;
using ARAP.Modules.Notifications.Infrastructure.Persistence;
using ARAP.Modules.Notifications.Infrastructure.Persistence.Repositories;

namespace ARAP.Modules.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddNotificationsApplication();

        var useInMemoryDatabase = configuration["UseInMemoryDatabase"];
        var useInMemory = !string.IsNullOrEmpty(useInMemoryDatabase) &&
                         useInMemoryDatabase.Equals("true", StringComparison.OrdinalIgnoreCase);

        Console.WriteLine($"[Infrastructure] UseInMemoryDatabase config value: '{useInMemoryDatabase}'");
        Console.WriteLine($"[Infrastructure] Using in-memory database: {useInMemory}");

        if (useInMemory)
        {
            services.AddDbContext<NotificationsDbContext>(options =>
                options.UseInMemoryDatabase("NotificationsDb"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("NotificationsDatabase");
            services.AddDbContext<NotificationsDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}
