using MediatR;
using ARAP.Modules.AcademicIntegrity.Application.DTOs;
using ARAP.Modules.AcademicIntegrity.Domain.Repositories;

namespace ARAP.Modules.AcademicIntegrity.Application.Queries.GetChecksByProposal;

public sealed class GetChecksByProposalQueryHandler : IRequestHandler<GetChecksByProposalQuery, IReadOnlyList<PlagiarismCheckDto>>
{
    private readonly IPlagiarismCheckRepository _repository;

    public GetChecksByProposalQueryHandler(IPlagiarismCheckRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PlagiarismCheckDto>> Handle(GetChecksByProposalQuery request, CancellationToken cancellationToken)
    {
        var checks = await _repository.GetByProposalIdAsync(request.ProposalId, cancellationToken);

        return checks.Select(check => new PlagiarismCheckDto(
            check.Id.Value,
            check.DocumentId,
            check.ProposalId,
            check.InitiatedBy,
            check.Status.Name,
            check.Score?.Value,
            check.ExternalCheckId,
            check.Notes?.Value,
            check.InitiatedAt,
            check.CompletedAt)).ToList();
    }
}
