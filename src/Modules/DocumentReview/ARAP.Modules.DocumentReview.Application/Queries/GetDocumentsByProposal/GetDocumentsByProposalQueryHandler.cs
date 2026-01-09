using MediatR;
using ARAP.Modules.DocumentReview.Application.DTOs;
using ARAP.Modules.DocumentReview.Domain.Repositories;

namespace ARAP.Modules.DocumentReview.Application.Queries.GetDocumentsByProposal;

public sealed class GetDocumentsByProposalQueryHandler : IRequestHandler<GetDocumentsByProposalQuery, IReadOnlyList<DocumentDto>>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentsByProposalQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<DocumentDto>> Handle(GetDocumentsByProposalQuery request, CancellationToken cancellationToken)
    {
        var documents = await _repository.GetByProposalIdAsync(request.ProposalId, cancellationToken);

        return documents.Select(d => new DocumentDto
        {
            Id = d.Id.Value,
            ProposalId = d.ProposalId,
            StudentId = d.StudentId,
            Type = d.Type.Value,
            FileName = d.FileName,
            FileUrl = d.FileUrl,
            Status = d.Status.Value,
            Version = d.Version,
            SubmittedAt = d.SubmittedAt,
            ReviewedAt = d.ReviewedAt,
            ReviewerId = d.ReviewerId,
            Feedback = d.Feedback?.Value
        }).ToList();
    }
}
