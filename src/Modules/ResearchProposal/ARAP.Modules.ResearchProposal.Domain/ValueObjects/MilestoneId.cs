using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for Milestone entity
/// </summary>
public sealed class MilestoneId : ValueObject
{
    public Guid Value { get; }

    private MilestoneId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Milestone ID cannot be empty", nameof(value));
        
        Value = value;
    }

    public static MilestoneId Create(Guid value) => new(value);
    
    public static MilestoneId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
