using Microsoft.EntityFrameworkCore;
using ARAP.Modules.AcademicIntegrity.Domain.Aggregates;

namespace ARAP.Modules.AcademicIntegrity.Infrastructure.Persistence;

public class AcademicIntegrityDbContext : DbContext
{
    public DbSet<PlagiarismCheck> PlagiarismChecks => Set<PlagiarismCheck>();

    public AcademicIntegrityDbContext(DbContextOptions<AcademicIntegrityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("academic_integrity");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AcademicIntegrityDbContext).Assembly);
    }
}
