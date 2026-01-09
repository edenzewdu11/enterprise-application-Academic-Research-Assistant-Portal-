using MediatR;
using ARAP.Modules.ResearchProposal.Domain.Aggregates;
using ARAP.Modules.ResearchProposal.Domain.Entities;
using ARAP.Modules.ResearchProposal.Domain.Repositories;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Application.Commands.CreateProposal;

public sealed class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, Guid>
{
    private readonly IResearchProposalRepository _repository;

    public CreateProposalCommandHandler(IResearchProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        // Create value objects with validation
        var titleResult = ProposalTitle.Create(request.Title);
        if (titleResult.IsFailure)
            throw new InvalidOperationException(titleResult.Error);

        var abstractResult = Abstract.Create(request.Abstract);
        if (abstractResult.IsFailure)
            throw new InvalidOperationException(abstractResult.Error);

        var questionResult = ResearchQuestion.Create(request.ResearchQuestion);
        if (questionResult.IsFailure)
            throw new InvalidOperationException(questionResult.Error);

        // Create proposal aggregate
        var proposalResult = Domain.Aggregates.ResearchProposal.Create(
            request.StudentId,
            request.AdvisorId,
            titleResult.Value,
            abstractResult.Value,
            questionResult.Value);

        if (proposalResult.IsFailure)
            throw new InvalidOperationException(proposalResult.Error);

        var proposal = proposalResult.Value;

        // Add milestones
        foreach (var milestoneDto in request.Milestones)
        {
            var milestoneType = MilestoneType.FromString(milestoneDto.Type);
            var milestoneResult = Milestone.Create(
                milestoneType,
                milestoneDto.Description,
                milestoneDto.Deadline);

            if (milestoneResult.IsFailure)
                throw new InvalidOperationException(milestoneResult.Error);

            var addResult = proposal?.AddMilestone(milestoneResult.Value);
            if (addResult.IsFailure)
                throw new InvalidOperationException(addResult.Error);
        }

        // Save to repository
        await _repository.AddAsync(proposal, cancellationToken);

        return proposal.Id.Value;
    }
}
