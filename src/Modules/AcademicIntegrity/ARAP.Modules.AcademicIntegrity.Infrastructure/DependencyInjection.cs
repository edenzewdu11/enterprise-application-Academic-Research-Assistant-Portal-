using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ARAP.Modules.AcademicIntegrity.Application;
using ARAP.Modules.AcademicIntegrity.Domain.Repositories;
using ARAP.Modules.AcademicIntegrity.Infrastructure.Persistence;
using ARAP.Modules.AcademicIntegrity.Infrastructure.Persistence.Repositories;

namespace ARAP.Modules.AcademicIntegrity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAcademicIntegrityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add application services
        services.AddAcademicIntegrityApplication();

        // Configure database
        var useInMemoryValue = configuration["UseInMemoryDatabase"];
        var useInMemory = useInMemoryValue == "true" || useInMemoryValue == "True";
        
        if (useInMemory)
        {
            services.AddDbContext<AcademicIntegrityDbContext>(options =>
                options.UseInMemoryDatabase("AcademicIntegrityDb"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("AcademicIntegrityDb");
            services.AddDbContext<AcademicIntegrityDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        // Register repositories
        services.AddScoped<IPlagiarismCheckRepository, PlagiarismCheckRepository>();

        return services;
    }
}
