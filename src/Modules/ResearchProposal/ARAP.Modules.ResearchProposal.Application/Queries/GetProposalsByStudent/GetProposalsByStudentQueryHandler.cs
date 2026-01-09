using MediatR;
using ARAP.Modules.ResearchProposal.Application.DTOs;
using ARAP.Modules.ResearchProposal.Domain.Repositories;

namespace ARAP.Modules.ResearchProposal.Application.Queries.GetProposalsByStudent;

public sealed class GetProposalsByStudentQueryHandler : IRequestHandler<GetProposalsByStudentQuery, List<ProposalDto>>
{
    private readonly IResearchProposalRepository _repository;

    public GetProposalsByStudentQueryHandler(IResearchProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProposalDto>> Handle(GetProposalsByStudentQuery request, CancellationToken cancellationToken)
    {
        var proposal = await _repository.GetByStudentIdAsync(request.StudentId, cancellationToken);

        if (proposal is null)
            return new List<ProposalDto>();

        return new List<ProposalDto>
        {
            new()
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
            }
        };
    }
}
