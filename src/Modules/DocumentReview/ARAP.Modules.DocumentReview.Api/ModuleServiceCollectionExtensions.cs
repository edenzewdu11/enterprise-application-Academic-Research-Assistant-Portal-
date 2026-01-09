using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.DocumentReview.Application;
using ARAP.Modules.DocumentReview.Infrastructure;

namespace ARAP.Modules.DocumentReview.Api;

public static class ModuleServiceCollectionExtensions
{
    public static IServiceCollection AddDocumentReviewModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDocumentReviewApplication();
        services.AddDocumentReviewInfrastructure(configuration);

        return services;
    }
}
