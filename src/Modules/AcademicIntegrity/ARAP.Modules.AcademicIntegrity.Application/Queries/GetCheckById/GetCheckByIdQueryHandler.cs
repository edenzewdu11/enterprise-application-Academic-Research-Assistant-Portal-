using MediatR;
using ARAP.Modules.AcademicIntegrity.Application.DTOs;
using ARAP.Modules.AcademicIntegrity.Domain.Repositories;
using ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

namespace ARAP.Modules.AcademicIntegrity.Application.Queries.GetCheckById;

public sealed class GetCheckByIdQueryHandler : IRequestHandler<GetCheckByIdQuery, PlagiarismCheckDto?>
{
    private readonly IPlagiarismCheckRepository _repository;

    public GetCheckByIdQueryHandler(IPlagiarismCheckRepository repository)
    {
        _repository = repository;
    }

    public async Task<PlagiarismCheckDto?> Handle(GetCheckByIdQuery request, CancellationToken cancellationToken)
    {
        var check = await _repository.GetByIdAsync(
            PlagiarismCheckId.Create(request.CheckId),
            cancellationToken);

        if (check == null)
            return null;

        return new PlagiarismCheckDto(
            check.Id.Value,
            check.DocumentId,
            check.ProposalId,
            check.InitiatedBy,
            check.Status.Name,
            check.Score?.Value,
            check.ExternalCheckId,
            check.Notes?.Value,
            check.InitiatedAt,
            check.CompletedAt);
    }
}
