using MediatR;

namespace ARAP.Modules.DocumentReview.Application.Commands.AssignReviewer;

public sealed record AssignReviewerCommand : IRequest<Unit>
{
    public Guid DocumentId { get; init; }
    public Guid ReviewerId { get; init; }
}
