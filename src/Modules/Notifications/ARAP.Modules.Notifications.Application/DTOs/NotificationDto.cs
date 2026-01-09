namespace ARAP.Modules.Notifications.Application.DTOs;

public sealed record NotificationDto(
    Guid Id,
    Guid RecipientId,
    string Type,
    string Title,
    string Message,
    string? ActionUrl,
    string Channel,
    Guid? RelatedEntityId,
    bool IsRead,
    DateTime? ReadAt,
    DateTime SentAt,
    DateTime? ExpiresAt,
    bool IsExpired
);
