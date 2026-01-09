using MediatR;
using ARAP.Modules.AcademicIntegrity.Domain.Aggregates;
using ARAP.Modules.AcademicIntegrity.Domain.Repositories;

namespace ARAP.Modules.AcademicIntegrity.Application.Commands.InitiateCheck;

public sealed class InitiateCheckCommandHandler : IRequestHandler<InitiateCheckCommand, Guid>
{
    private readonly IPlagiarismCheckRepository _repository;

    public InitiateCheckCommandHandler(IPlagiarismCheckRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(InitiateCheckCommand request, CancellationToken cancellationToken)
    {
        var checkResult = PlagiarismCheck.Create(
            request.DocumentId,
            request.ProposalId,
            request.InitiatedBy);

        if (checkResult.IsFailure)
            throw new InvalidOperationException(checkResult.Error);

        var check = checkResult.Value!;
        await _repository.AddAsync(check, cancellationToken);

        return check.Id.Value;
    }
}
