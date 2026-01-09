using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.ResearchProposal.Domain.Repositories;
using ARAP.Modules.ResearchProposal.Infrastructure.Persistence;
using ARAP.Modules.ResearchProposal.Infrastructure.Persistence.Repositories;

namespace ARAP.Modules.ResearchProposal.Infrastructure;

/// <summary>
/// Extension methods for configuring ResearchProposal infrastructure services
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddResearchProposalInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Check if we should use in-memory database for testing
        var useInMemoryValue = configuration["UseInMemoryDatabase"];
        var useInMemory = useInMemoryValue == "true" || useInMemoryValue == "True";
        
        Console.WriteLine($"[Infrastructure] UseInMemoryDatabase config value: '{useInMemoryValue}'");
        Console.WriteLine($"[Infrastructure] Using in-memory database: {useInMemory}");

        if (useInMemory)
        {
            // Use in-memory database for testing
            services.AddDbContext<ResearchProposalDbContext>(options =>
            {
                options.UseInMemoryDatabase("ResearchProposalTestDb");
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
        }
        else
        {
            // Register DbContext with PostgreSQL
            services.AddDbContext<ResearchProposalDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ResearchProposalDb")
                    ?? throw new InvalidOperationException("ResearchProposalDb connection string not found");

                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", "research_proposal");
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null);
                    });

                // Enable sensitive data logging in development
                var loggingSection = configuration.GetSection("Logging");
                var enableSensitiveData = loggingSection["EnableSensitiveDataLogging"] == "true";
                var enableDetailedErrors = loggingSection["EnableDetailedErrors"] == "true";
                
                options.EnableSensitiveDataLogging(enableSensitiveData);
                options.EnableDetailedErrors(enableDetailedErrors);
            });
        }

        // Register repositories
        services.AddScoped<IResearchProposalRepository, ResearchProposalRepository>();

        return services;
    }
}
