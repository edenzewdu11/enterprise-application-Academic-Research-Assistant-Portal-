using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.ValueObjects;

/// <summary>
/// Represents a feedback comment with validation
/// </summary>
public sealed class FeedbackComment : ValueObject
{
    public string Value { get; }

    private FeedbackComment(string value)
    {
        Value = value;
    }

    public static Result<FeedbackComment> Create(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            return Result.Failure<FeedbackComment>("Feedback comment cannot be empty");

        if (comment.Length < 10)
            return Result.Failure<FeedbackComment>("Feedback comment must be at least 10 characters");

        if (comment.Length > 2000)
            return Result.Failure<FeedbackComment>("Feedback comment cannot exceed 2000 characters");

        return Result.Success(new FeedbackComment(comment.Trim()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
