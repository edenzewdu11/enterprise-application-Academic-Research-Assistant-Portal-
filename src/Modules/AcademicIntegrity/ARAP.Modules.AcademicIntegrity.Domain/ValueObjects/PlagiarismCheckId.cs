using ARAP.SharedKernel;

namespace ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

/// <summary>
/// Value object representing the unique identifier for a plagiarism check
/// </summary>
public sealed class PlagiarismCheckId : ValueObject
{
    public Guid Value { get; }

    private PlagiarismCheckId(Guid value)
    {
        Value = value;
    }

    public static PlagiarismCheckId Create(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("PlagiarismCheckId cannot be empty", nameof(value));

        return new PlagiarismCheckId(value);
    }

    public static PlagiarismCheckId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
