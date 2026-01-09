using MediatR;
using ARAP.Modules.DocumentReview.Domain.Aggregates;
using ARAP.Modules.DocumentReview.Domain.Repositories;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;

namespace ARAP.Modules.DocumentReview.Application.Commands.SubmitDocument;

public sealed class SubmitDocumentCommandHandler : IRequestHandler<SubmitDocumentCommand, Guid>
{
    private readonly IDocumentRepository _repository;

    public SubmitDocumentCommandHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(SubmitDocumentCommand request, CancellationToken cancellationToken)
    {
        var documentType = DocumentType.FromString(request.DocumentType);
        
        var documentResult = Document.Create(
            request.ProposalId,
            request.StudentId,
            documentType,
            request.FileName,
            request.FileUrl,
            request.Version);

        if (documentResult.IsFailure)
            throw new InvalidOperationException(documentResult.Error);

        await _repository.AddAsync(documentResult.Value!, cancellationToken);

        return documentResult.Value!.Id.Value;
    }
}
