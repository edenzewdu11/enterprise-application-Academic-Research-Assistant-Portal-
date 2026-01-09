using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Represents the current state of a research proposal in its lifecycle
/// </summary>
public sealed class ProposalState : ValueObject
{
    public string Value { get; }

    // Valid states
    public static readonly ProposalState Draft = new("Draft");
    public static readonly ProposalState Submitted = new("Submitted");
    public static readonly ProposalState UnderReview = new("UnderReview");
    public static readonly ProposalState RevisionRequired = new("RevisionRequired");
    public static readonly ProposalState Approved = new("Approved");
    public static readonly ProposalState Rejected = new("Rejected");

    private ProposalState(string value)
    {
        Value = value;
    }

    public static ProposalState FromString(string value)
    {
        return value switch
        {
            "Draft" => Draft,
            "Submitted" => Submitted,
            "UnderReview" => UnderReview,
            "RevisionRequired" => RevisionRequired,
            "Approved" => Approved,
            "Rejected" => Rejected,
            _ => throw new ArgumentException($"Invalid proposal state: {value}", nameof(value))
        };
    }

    public bool IsDraft => this == Draft;
    public bool IsSubmitted => this == Submitted;
    public bool IsUnderReview => this == UnderReview;
    public bool RequiresRevision => this == RevisionRequired;
    public bool IsApproved => this == Approved;
    public bool IsRejected => this == Rejected;
    public bool IsFinal => IsApproved || IsRejected;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
