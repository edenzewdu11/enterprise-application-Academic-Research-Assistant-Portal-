using MediatR;

namespace ARAP.Modules.Notifications.Application.Commands.MarkAsRead;

public sealed record MarkAsReadCommand(Guid NotificationId) : IRequest<ARAP.SharedKernel.Result>;
