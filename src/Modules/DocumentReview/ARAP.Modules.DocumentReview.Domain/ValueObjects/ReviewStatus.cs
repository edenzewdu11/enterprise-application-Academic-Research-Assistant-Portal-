using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.ValueObjects;

/// <summary>
/// Represents the review status of a document
/// </summary>
public sealed class ReviewStatus : ValueObject
{
    public string Value { get; }

    public static readonly ReviewStatus Pending = new("Pending");
    public static readonly ReviewStatus InReview = new("InReview");
    public static readonly ReviewStatus ChangesRequested = new("ChangesRequested");
    public static readonly ReviewStatus Approved = new("Approved");
    public static readonly ReviewStatus Rejected = new("Rejected");

    private ReviewStatus(string value)
    {
        Value = value;
    }

    public bool IsPending => Value == Pending.Value;
    public bool IsInReview => Value == InReview.Value;
    public bool IsChangesRequested => Value == ChangesRequested.Value;
    public bool IsApproved => Value == Approved.Value;
    public bool IsRejected => Value == Rejected.Value;
    public bool IsFinal => IsApproved || IsRejected;

    public static ReviewStatus FromString(string value)
    {
        return value switch
        {
            "Pending" => Pending,
            "InReview" => InReview,
            "ChangesRequested" => ChangesRequested,
            "Approved" => Approved,
            "Rejected" => Rejected,
            _ => throw new ArgumentException($"Invalid review status: {value}", nameof(value))
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
