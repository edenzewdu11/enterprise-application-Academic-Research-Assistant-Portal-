using MediatR;
using ARAP.Modules.DocumentReview.Application.DTOs;
using ARAP.Modules.DocumentReview.Domain.Repositories;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;

namespace ARAP.Modules.DocumentReview.Application.Queries.GetDocumentById;

public sealed class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto?>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentByIdQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentDto?> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var documentId = DocumentId.Create(request.DocumentId);
        var document = await _repository.GetByIdAsync(documentId, cancellationToken);

        if (document is null)
            return null;

        return new DocumentDto
        {
            Id = document.Id.Value,
            ProposalId = document.ProposalId,
            StudentId = document.StudentId,
            Type = document.Type.Value,
            FileName = document.FileName,
            FileUrl = document.FileUrl,
            Status = document.Status.Value,
            Version = document.Version,
            SubmittedAt = document.SubmittedAt,
            ReviewedAt = document.ReviewedAt,
            ReviewerId = document.ReviewerId,
            Feedback = document.Feedback?.Value
        };
    }
}
