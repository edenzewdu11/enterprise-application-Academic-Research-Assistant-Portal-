namespace ARAP.Modules.ResearchProposal.Application.DTOs;

public sealed record ProposalDto
{
    public Guid Id { get; init; }
    public Guid StudentId { get; init; }
    public Guid AdvisorId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Abstract { get; init; } = string.Empty;
    public string ResearchQuestion { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string? ReviewComments { get; init; }
    public DateTime? SubmittedAt { get; init; }
    public DateTime? ReviewedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
    public List<MilestoneDto> Milestones { get; init; } = new();
}

public sealed record MilestoneDto
{
    public Guid Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime Deadline { get; init; }
    public DateTime? CompletionDate { get; init; }
    public string Status { get; init; } = string.Empty;
}
