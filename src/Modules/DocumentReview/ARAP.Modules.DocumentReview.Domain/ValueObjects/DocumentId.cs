using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.ValueObjects;

/// <summary>
/// Unique identifier for Document aggregate
/// </summary>
public sealed class DocumentId : ValueObject
{
    public Guid Value { get; }

    private DocumentId(Guid value)
    {
        Value = value;
    }

    public static DocumentId Create(Guid value) => new(value);

    public static DocumentId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
