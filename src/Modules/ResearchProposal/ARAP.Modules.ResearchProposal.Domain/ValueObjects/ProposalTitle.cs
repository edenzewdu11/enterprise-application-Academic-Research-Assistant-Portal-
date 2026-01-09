using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Represents the title of a research proposal with validation
/// </summary>
public sealed class ProposalTitle : ValueObject
{
    public string Value { get; }

    private ProposalTitle(string value)
    {
        Value = value;
    }

    public static Result<ProposalTitle> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<ProposalTitle>("Proposal title cannot be empty");

        if (title.Length < 10)
            return Result.Failure<ProposalTitle>("Proposal title must be at least 10 characters");

        if (title.Length > 200)
            return Result.Failure<ProposalTitle>("Proposal title cannot exceed 200 characters");

        return Result.Success(new ProposalTitle(title.Trim()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
