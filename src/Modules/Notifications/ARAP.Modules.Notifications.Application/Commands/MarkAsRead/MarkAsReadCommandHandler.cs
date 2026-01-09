using ARAP.SharedKernel;
using ARAP.Modules.Notifications.Domain.Repositories;
using ARAP.Modules.Notifications.Domain.ValueObjects;
using MediatR;

namespace ARAP.Modules.Notifications.Application.Commands.MarkAsRead;

internal sealed class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, Result>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkAsReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var notificationId = NotificationId.From(request.NotificationId);
        var notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken);

        if (notification == null)
            return Result.Failure("Notification not found");

        var result = notification.MarkAsRead();
        if (result.IsFailure)
            return result;

        await _notificationRepository.UpdateAsync(notification, cancellationToken);

        return Result.Success();
    }
}
