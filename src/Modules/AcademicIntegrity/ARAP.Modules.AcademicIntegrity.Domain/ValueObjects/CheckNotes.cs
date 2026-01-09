using ARAP.SharedKernel;

namespace ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

/// <summary>
/// Value object for check notes/comments
/// </summary>
public sealed class CheckNotes : ValueObject
{
    public string Value { get; }

    private CheckNotes(string value)
    {
        Value = value;
    }

    public static Result<CheckNotes> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<CheckNotes>("Check notes cannot be empty");

        var trimmed = value.Trim();

        if (trimmed.Length > 2000)
            return Result.Failure<CheckNotes>("Check notes cannot exceed 2000 characters");

        return Result.Success(new CheckNotes(trimmed));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(CheckNotes notes) => notes.Value;
}
