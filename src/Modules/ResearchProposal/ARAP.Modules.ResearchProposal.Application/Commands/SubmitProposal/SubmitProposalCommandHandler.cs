using MediatR;
using ARAP.Modules.ResearchProposal.Domain.Repositories;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Application.Commands.SubmitProposal;

public sealed class SubmitProposalCommandHandler : IRequestHandler<SubmitProposalCommand, Unit>
{
    private readonly IResearchProposalRepository _repository;

    public SubmitProposalCommandHandler(IResearchProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(SubmitProposalCommand request, CancellationToken cancellationToken)
    {
        var proposalId = ResearchProposalId.Create(request.ProposalId);
        var proposal = await _repository.GetByIdAsync(proposalId, cancellationToken);

        if (proposal is null)
            throw new InvalidOperationException("Proposal not found");

        if (proposal.StudentId != request.StudentId)
            throw new InvalidOperationException("Only the proposal owner can submit it");

        var result = proposal.Submit();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        await _repository.UpdateAsync(proposal, cancellationToken);

        return Unit.Value;
    }
}
