using ARAP.SharedKernel;
using ARAP.Modules.Notifications.Domain.Repositories;
using MediatR;

namespace ARAP.Modules.Notifications.Application.Queries.GetUnreadCount;

internal sealed class GetUnreadCountQueryHandler : IRequestHandler<GetUnreadCountQuery, Result<int>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetUnreadCountQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<int>> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
    {
        var count = await _notificationRepository.GetUnreadCountAsync(request.RecipientId, cancellationToken);
        return Result<int>.Success(count);
    }
}
