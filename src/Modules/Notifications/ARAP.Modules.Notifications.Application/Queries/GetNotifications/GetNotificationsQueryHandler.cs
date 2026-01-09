using ARAP.SharedKernel;
using ARAP.Modules.Notifications.Application.DTOs;
using ARAP.Modules.Notifications.Domain.Repositories;
using MediatR;

namespace ARAP.Modules.Notifications.Application.Queries.GetNotifications;

internal sealed class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<IReadOnlyList<NotificationDto>>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<IReadOnlyList<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = request.OnlyUnread
            ? await _notificationRepository.GetUnreadByRecipientAsync(request.RecipientId, cancellationToken)
            : await _notificationRepository.GetByRecipientAsync(request.RecipientId, cancellationToken);

        var dtos = notifications
            .Select(n => new NotificationDto(
                n.Id.Value,
                n.RecipientId,
                n.Type.Name,
                n.Content.Title,
                n.Content.Message,
                n.Content.ActionUrl,
                n.Channel.Name,
                n.RelatedEntityId,
                n.IsRead,
                n.ReadAt,
                n.SentAt,
                n.ExpiresAt,
                n.IsExpired()))
            .ToList();

        IReadOnlyList<NotificationDto> result = dtos;
        return Result.Success(result);
    }
}
