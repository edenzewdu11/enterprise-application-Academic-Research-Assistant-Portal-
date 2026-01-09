using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for ResearchProposal aggregate
/// </summary>
public sealed class ResearchProposalId : ValueObject
{
    public Guid Value { get; }

    private ResearchProposalId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ResearchProposal ID cannot be empty", nameof(value));
        
        Value = value;
    }

    public static ResearchProposalId Create(Guid value) => new(value);
    
    public static ResearchProposalId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
