using MediatR;

namespace ARAP.Modules.DocumentReview.Application.Commands.ProvideFeedback;

public sealed record ProvideFeedbackCommand : IRequest<Unit>
{
    public Guid DocumentId { get; init; }
    public Guid ReviewerId { get; init; }
    public string Comment { get; init; } = string.Empty;
}
