using ARAP.SharedKernel;

namespace ARAP.Modules.Notifications.Domain.ValueObjects;

public sealed class NotificationContent : ValueObject
{
    public string Title { get; }
    public string Message { get; }
    public string? ActionUrl { get; }

    private NotificationContent(string title, string message, string? actionUrl)
    {
        Title = title;
        Message = message;
        ActionUrl = actionUrl;
    }

    public static Result<NotificationContent> Create(string title, string message, string? actionUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<NotificationContent>("Title is required");

        if (title.Length > 200)
            return Result.Failure<NotificationContent>("Title cannot exceed 200 characters");

        if (string.IsNullOrWhiteSpace(message))
            return Result.Failure<NotificationContent>("Message is required");

        if (message.Length > 1000)
            return Result.Failure<NotificationContent>("Message cannot exceed 1000 characters");

        if (actionUrl != null && actionUrl.Length > 500)
            return Result.Failure<NotificationContent>("Action URL cannot exceed 500 characters");

        return Result.Success(new NotificationContent(title, message, actionUrl));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Title;
        yield return Message;
        yield return ActionUrl;
    }
}
