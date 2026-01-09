namespace ARAP.Modules.AcademicIntegrity.Application.DTOs;

public sealed record PlagiarismCheckDto(
    Guid Id,
    Guid DocumentId,
    Guid ProposalId,
    Guid InitiatedBy,
    string Status,
    decimal? SimilarityScore,
    string? ExternalCheckId,
    string? Notes,
    DateTime InitiatedAt,
    DateTime? CompletedAt);
