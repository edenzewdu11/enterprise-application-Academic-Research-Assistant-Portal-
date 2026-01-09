using ARAP.SharedKernel;

namespace ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

/// <summary>
/// Value object representing a plagiarism similarity score (0-100%)
/// </summary>
public sealed class SimilarityScore : ValueObject
{
    public decimal Value { get; }

    private SimilarityScore(decimal value)
    {
        Value = value;
    }

    public static Result<SimilarityScore> Create(decimal value)
    {
        if (value < 0)
            return Result.Failure<SimilarityScore>("Similarity score cannot be negative");

        if (value > 100)
            return Result.Failure<SimilarityScore>("Similarity score cannot exceed 100%");

        return Result.Success(new SimilarityScore(value));
    }

    public bool IsHighRisk => Value >= 50;
    public bool IsMediumRisk => Value >= 25 && Value < 50;
    public bool IsLowRisk => Value < 25;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
