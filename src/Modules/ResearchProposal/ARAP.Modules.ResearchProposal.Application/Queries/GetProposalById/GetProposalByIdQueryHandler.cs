using MediatR;
using ARAP.Modules.ResearchProposal.Application.DTOs;
using ARAP.Modules.ResearchProposal.Domain.Repositories;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Application.Queries.GetProposalById;

public sealed class GetProposalByIdQueryHandler : IRequestHandler<GetProposalByIdQuery, ProposalDto?>
{
    private readonly IResearchProposalRepository _repository;

    public GetProposalByIdQueryHandler(IResearchProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProposalDto?> Handle(GetProposalByIdQuery request, CancellationToken cancellationToken)
    {
        var proposalId = ResearchProposalId.Create(request.ProposalId);
        var proposal = await _repository.GetByIdAsync(proposalId, cancellationToken);

        if (proposal is null)
            return null;

        return new ProposalDto
        {
            Id = proposal.Id.Value,
            StudentId = proposal.StudentId,
            AdvisorId = proposal.AdvisorId,
            Title = proposal.Title.Value,
            Abstract = proposal.Abstract.Value,
            ResearchQuestion = proposal.ResearchQuestion.Value,
            State = proposal.State.Value,
            ReviewComments = proposal.ReviewComments,
            SubmittedAt = proposal.SubmittedAt,
            ReviewedAt = proposal.ReviewedAt,
            CreatedAt = proposal.CreatedAt,
            LastModifiedAt = proposal.LastModifiedAt,
            Milestones = proposal.Milestones.Select(m => new DTOs.MilestoneDto
            {
                Id = m.Id.Value,
                Type = m.Type.Value,
                Description = m.Description,
                Deadline = m.Deadline,
                CompletionDate = m.CompletionDate,
                Status = m.Status.Value
            }).ToList()
        };
    }
}
