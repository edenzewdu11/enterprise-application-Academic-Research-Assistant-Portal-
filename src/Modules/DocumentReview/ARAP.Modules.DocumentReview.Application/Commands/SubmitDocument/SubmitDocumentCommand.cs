using MediatR;

namespace ARAP.Modules.DocumentReview.Application.Commands.SubmitDocument;

public sealed record SubmitDocumentCommand : IRequest<Guid>
{
    public Guid ProposalId { get; init; }
    public Guid StudentId { get; init; }
    public string DocumentType { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public int Version { get; init; } = 1;
}
