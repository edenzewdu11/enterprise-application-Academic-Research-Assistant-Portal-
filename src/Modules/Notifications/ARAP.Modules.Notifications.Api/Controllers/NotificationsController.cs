using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ARAP.Modules.Notifications.Application.Commands.SendNotification;
using ARAP.Modules.Notifications.Application.Commands.MarkAsRead;
using ARAP.Modules.Notifications.Application.Queries.GetNotifications;
using ARAP.Modules.Notifications.Application.Queries.GetUnreadCount;
using System.Security.Claims;

namespace ARAP.Modules.Notifications.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly ISender _sender;

    public NotificationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Roles = "advisor,admin")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
    {
        var command = new SendNotificationCommand(
            request.RecipientId,
            request.Type,
            request.Title,
            request.Message,
            request.ActionUrl,
            request.Channel,
            request.RelatedEntityId,
            request.ExpiresAt);

        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok(new { notificationId = result.Value })
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var command = new MarkAsReadCommand(id);
        var result = await _sender.Send(command);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(new { error = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] bool onlyUnread = false)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var query = new GetNotificationsQuery(userGuid, onlyUnread);
        var result = await _sender.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var query = new GetUnreadCountQuery(userGuid);
        var result = await _sender.Send(query);

        return result.IsSuccess
            ? Ok(new { count = result.Value })
            : BadRequest(new { error = result.Error });
    }
}

public record SendNotificationRequest(
    Guid RecipientId,
    string Type,
    string Title,
    string Message,
    string? ActionUrl,
    string Channel,
    Guid? RelatedEntityId,
    DateTime? ExpiresAt
);
