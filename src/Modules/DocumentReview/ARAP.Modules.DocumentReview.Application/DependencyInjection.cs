using Microsoft.Extensions.DependencyInjection;

namespace ARAP.Modules.DocumentReview.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDocumentReviewApplication(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}
