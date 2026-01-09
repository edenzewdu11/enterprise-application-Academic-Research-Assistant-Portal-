namespace ARAP.Modules.DocumentReview.Application.DTOs;

public sealed record DocumentDto
{
    public Guid Id { get; init; }
    public Guid ProposalId { get; init; }
    public Guid StudentId { get; init; }
    public string Type { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public int Version { get; init; }
    public DateTime SubmittedAt { get; init; }
    public DateTime? ReviewedAt { get; init; }
    public Guid? ReviewerId { get; init; }
    public string? Feedback { get; init; }
}
