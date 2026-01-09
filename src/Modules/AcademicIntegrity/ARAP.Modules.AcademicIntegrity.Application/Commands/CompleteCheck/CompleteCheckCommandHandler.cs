using MediatR;
using ARAP.Modules.AcademicIntegrity.Domain.Repositories;
using ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

namespace ARAP.Modules.AcademicIntegrity.Application.Commands.CompleteCheck;

public sealed class CompleteCheckCommandHandler : IRequestHandler<CompleteCheckCommand>
{
    private readonly IPlagiarismCheckRepository _repository;

    public CompleteCheckCommandHandler(IPlagiarismCheckRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CompleteCheckCommand request, CancellationToken cancellationToken)
    {
        var check = await _repository.GetByIdAsync(
            PlagiarismCheckId.Create(request.CheckId),
            cancellationToken);

        if (check == null)
            throw new InvalidOperationException("Plagiarism check not found");

        var scoreResult = SimilarityScore.Create(request.SimilarityScore);
        if (scoreResult.IsFailure)
            throw new InvalidOperationException(scoreResult.Error);

        CheckNotes? notes = null;
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            var notesResult = CheckNotes.Create(request.Notes);
            if (notesResult.IsFailure)
                throw new InvalidOperationException(notesResult.Error);
            notes = notesResult.Value;
        }

        var result = check.Complete(scoreResult.Value!, notes);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        await _repository.UpdateAsync(check, cancellationToken);
    }
}
