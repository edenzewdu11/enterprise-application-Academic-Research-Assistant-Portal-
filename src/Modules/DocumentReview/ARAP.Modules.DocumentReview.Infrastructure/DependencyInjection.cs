using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.DocumentReview.Domain.Repositories;
using ARAP.Modules.DocumentReview.Infrastructure.Persistence;
using ARAP.Modules.DocumentReview.Infrastructure.Persistence.Repositories;

namespace ARAP.Modules.DocumentReview.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDocumentReviewInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var useInMemoryValue = configuration["UseInMemoryDatabase"];
        var useInMemory = useInMemoryValue == "true" || useInMemoryValue == "True";

        if (useInMemory)
        {
            services.AddDbContext<DocumentReviewDbContext>(options =>
            {
                options.UseInMemoryDatabase("DocumentReviewTestDb");
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
        }
        else
        {
            services.AddDbContext<DocumentReviewDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DocumentReviewDb")
                    ?? throw new InvalidOperationException("DocumentReviewDb connection string not found");

                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", "document_review");
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null);
                    });

                var enableSensitiveData = configuration.GetSection("Logging")["EnableSensitiveDataLogging"] == "true";
                var enableDetailedErrors = configuration.GetSection("Logging")["EnableDetailedErrors"] == "true";
                
                options.EnableSensitiveDataLogging(enableSensitiveData);
                options.EnableDetailedErrors(enableDetailedErrors);
            });
        }

        services.AddScoped<IDocumentRepository, DocumentRepository>();

        return services;
    }
}
