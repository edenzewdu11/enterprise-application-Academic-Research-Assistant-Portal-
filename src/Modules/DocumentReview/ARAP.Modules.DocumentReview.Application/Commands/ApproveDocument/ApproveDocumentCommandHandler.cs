using MediatR;
using ARAP.Modules.DocumentReview.Domain.Repositories;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;

namespace ARAP.Modules.DocumentReview.Application.Commands.ApproveDocument;

public sealed class ApproveDocumentCommandHandler : IRequestHandler<ApproveDocumentCommand, Unit>
{
    private readonly IDocumentRepository _repository;

    public ApproveDocumentCommandHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ApproveDocumentCommand request, CancellationToken cancellationToken)
    {
        var documentId = DocumentId.Create(request.DocumentId);
        var document = await _repository.GetByIdAsync(documentId, cancellationToken);

        if (document is null)
            throw new InvalidOperationException("Document not found");

        var result = document.Approve(request.ReviewerId);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        await _repository.UpdateAsync(document, cancellationToken);

        return Unit.Value;
    }
}
