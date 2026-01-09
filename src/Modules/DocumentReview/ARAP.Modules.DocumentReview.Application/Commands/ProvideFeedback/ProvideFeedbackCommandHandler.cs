using MediatR;
using ARAP.Modules.DocumentReview.Domain.Repositories;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;

namespace ARAP.Modules.DocumentReview.Application.Commands.ProvideFeedback;

public sealed class ProvideFeedbackCommandHandler : IRequestHandler<ProvideFeedbackCommand, Unit>
{
    private readonly IDocumentRepository _repository;

    public ProvideFeedbackCommandHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ProvideFeedbackCommand request, CancellationToken cancellationToken)
    {
        var documentId = DocumentId.Create(request.DocumentId);
        var document = await _repository.GetByIdAsync(documentId, cancellationToken);

        if (document is null)
            throw new InvalidOperationException("Document not found");

        var commentResult = FeedbackComment.Create(request.Comment);
        if (commentResult.IsFailure)
            throw new InvalidOperationException(commentResult.Error);

        var result = document.ProvideFeedback(request.ReviewerId, commentResult.Value!);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        await _repository.UpdateAsync(document, cancellationToken);

        return Unit.Value;
    }
}
