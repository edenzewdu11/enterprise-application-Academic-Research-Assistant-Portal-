using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Represents the abstract/summary of a research proposal
/// </summary>
public sealed class Abstract : ValueObject
{
    public string Value { get; }

    private Abstract(string value)
    {
        Value = value;
    }

    public static Result<Abstract> Create(string abstractText)
    {
        if (string.IsNullOrWhiteSpace(abstractText))
            return Result.Failure<Abstract>("Abstract cannot be empty");

        if (abstractText.Length < 100)
            return Result.Failure<Abstract>("Abstract must be at least 100 characters");

        if (abstractText.Length > 3000)
            return Result.Failure<Abstract>("Abstract cannot exceed 3000 characters");

        return Result.Success(new Abstract(abstractText.Trim()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
