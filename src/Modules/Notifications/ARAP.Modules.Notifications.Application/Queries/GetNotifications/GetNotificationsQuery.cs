using ARAP.Modules.Notifications.Application.DTOs;
using MediatR;

namespace ARAP.Modules.Notifications.Application.Queries.GetNotifications;

public sealed record GetNotificationsQuery(
    Guid RecipientId,
    bool OnlyUnread = false
) : IRequest<ARAP.SharedKernel.Result<IReadOnlyList<NotificationDto>>>;
