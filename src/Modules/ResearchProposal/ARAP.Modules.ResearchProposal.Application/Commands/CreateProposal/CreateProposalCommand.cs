using MediatR;
using ARAP.Modules.ResearchProposal.Application.DTOs;

namespace ARAP.Modules.ResearchProposal.Application.Commands.CreateProposal;

/// <summary>
/// Command to create a new research proposal in draft state
/// </summary>
public sealed record CreateProposalCommand : IRequest<Guid>
{
    public Guid StudentId { get; init; }
    public Guid AdvisorId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Abstract { get; init; } = string.Empty;
    public string ResearchQuestion { get; init; } = string.Empty;
    public List<CreateMilestoneDto> Milestones { get; init; } = new();
}

/// <summary>
/// DTO for creating a new milestone (simplified version without ID and status)
/// </summary>
public sealed record CreateMilestoneDto
{
    public string Type { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime Deadline { get; init; }
}
