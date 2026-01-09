using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ARAP.Modules.ResearchProposal.Application;


public static class DependencyInjection
{
 
    public static IServiceCollection AddResearchProposalApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
