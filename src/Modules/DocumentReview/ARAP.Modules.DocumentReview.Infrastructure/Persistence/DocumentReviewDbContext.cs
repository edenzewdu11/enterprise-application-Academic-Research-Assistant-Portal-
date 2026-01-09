using Microsoft.EntityFrameworkCore;
using ARAP.Modules.DocumentReview.Domain.Aggregates;

namespace ARAP.Modules.DocumentReview.Infrastructure.Persistence;

public sealed class DocumentReviewDbContext : DbContext
{
    public DbSet<Document> Documents => Set<Document>();

    public DocumentReviewDbContext(DbContextOptions<DocumentReviewDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("document_review");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentReviewDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
