using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.ResearchProposal.Application;
using ARAP.Modules.ResearchProposal.Infrastructure;

namespace ARAP.Modules.ResearchProposal.Api;

/// <summary>
/// Extension methods for registering ResearchProposal module services
/// </summary>
public static class ModuleServiceCollectionExtensions
{
    /// <summary>
    /// Registers all ResearchProposal module services (Application + Infrastructure layers)
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Application configuration</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddResearchProposalModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register Application layer (MediatR handlers, validators)
        services.AddResearchProposalApplication();

        // Register Infrastructure layer (DbContext, repositories)
        services.AddResearchProposalInfrastructure(configuration);

        return services;
    }
}
