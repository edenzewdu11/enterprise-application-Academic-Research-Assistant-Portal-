using MediatR;

namespace ARAP.Modules.Notifications.Application.Commands.SendNotification;

public sealed record SendNotificationCommand(
    Guid RecipientId,
    string Type,
    string Title,
    string Message,
    string? ActionUrl,
    string Channel,
    Guid? RelatedEntityId,
    DateTime? ExpiresAt
) : IRequest<ARAP.SharedKernel.Result<Guid>>;
