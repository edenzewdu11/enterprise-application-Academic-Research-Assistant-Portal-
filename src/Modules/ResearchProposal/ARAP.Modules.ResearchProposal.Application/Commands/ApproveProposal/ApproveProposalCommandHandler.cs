using MediatR;
using ARAP.Modules.ResearchProposal.Domain.Repositories;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Application.Commands.ApproveProposal;

public sealed class ApproveProposalCommandHandler : IRequestHandler<ApproveProposalCommand, Unit>
{
    private readonly IResearchProposalRepository _repository;

    public ApproveProposalCommandHandler(IResearchProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ApproveProposalCommand request, CancellationToken cancellationToken)
    {
        var proposalId = ResearchProposalId.Create(request.ProposalId);
        var proposal = await _repository.GetByIdAsync(proposalId, cancellationToken);

        if (proposal is null)
            throw new InvalidOperationException("Proposal not found");

        var result = proposal.Approve(request.AdvisorId, request.Comments);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        await _repository.UpdateAsync(proposal, cancellationToken);

        return Unit.Value;
    }
}
