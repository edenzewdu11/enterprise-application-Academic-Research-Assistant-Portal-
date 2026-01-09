using MediatR;

namespace ARAP.Modules.DocumentReview.Application.Commands.ApproveDocument;

public sealed record ApproveDocumentCommand : IRequest<Unit>
{
    public Guid DocumentId { get; init; }
    public Guid ReviewerId { get; init; }
}
