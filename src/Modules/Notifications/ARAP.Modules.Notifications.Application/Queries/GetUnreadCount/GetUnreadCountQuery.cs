using MediatR;

namespace ARAP.Modules.Notifications.Application.Queries.GetUnreadCount;

public sealed record GetUnreadCountQuery(Guid RecipientId) : IRequest<ARAP.SharedKernel.Result<int>>;
