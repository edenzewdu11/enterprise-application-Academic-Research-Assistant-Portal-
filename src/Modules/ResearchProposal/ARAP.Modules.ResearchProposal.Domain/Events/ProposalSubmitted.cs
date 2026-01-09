using ARAP.SharedKernel;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Domain.Events;


public sealed record ProposalSubmitted : DomainEvent
{
    public Guid ProposalId { get; init; }
    public Guid StudentId { get; init; }
    public string Title { get; init; }
    public Guid AdvisorId { get; init; }
    public DateTime SubmittedAt { get; init; }

    public ProposalSubmitted(
        Guid proposalId,
        Guid studentId,
        string title,
        Guid advisorId,
        DateTime submittedAt)
    {
        ProposalId = proposalId;
        StudentId = studentId;
        Title = title;
        AdvisorId = advisorId;
        SubmittedAt = submittedAt;
    }
}
