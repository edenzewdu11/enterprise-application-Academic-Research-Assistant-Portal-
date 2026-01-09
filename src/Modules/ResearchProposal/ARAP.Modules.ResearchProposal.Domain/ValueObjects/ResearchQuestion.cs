using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Represents a research question with validation
/// </summary>
public sealed class ResearchQuestion : ValueObject
{
    public string Value { get; }

    private ResearchQuestion(string value)
    {
        Value = value;
    }

    public static Result<ResearchQuestion> Create(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            return Result.Failure<ResearchQuestion>("Research question cannot be empty");

        if (question.Length < 20)
            return Result.Failure<ResearchQuestion>("Research question must be at least 20 characters");

        if (question.Length > 500)
            return Result.Failure<ResearchQuestion>("Research question cannot exceed 500 characters");

        if (!question.TrimEnd().EndsWith('?'))
            return Result.Failure<ResearchQuestion>("Research question should end with a question mark");

        return Result.Success(new ResearchQuestion(question.Trim()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
