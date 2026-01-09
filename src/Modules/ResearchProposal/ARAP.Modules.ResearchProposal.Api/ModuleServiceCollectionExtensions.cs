using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.ResearchProposal.Application;
using ARAP.Modules.ResearchProposal.Infrastructure;

namespace ARAP.Modules.ResearchProposal.Api;


public static class ModuleServiceCollectionExtensions
{
    
    public static IServiceCollection AddResearchProposalModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
       
        services.AddResearchProposalApplication();

       
        services.AddResearchProposalInfrastructure(configuration);

        return services;
    }
}
