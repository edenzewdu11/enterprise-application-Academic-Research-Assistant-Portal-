using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ARAP.Modules.ResearchProposal.Application;

/// <summary>
/// Extension methods for registering ResearchProposal Application layer services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MediatR handlers, validators, and other application services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddResearchProposalApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR for CQRS pattern
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
