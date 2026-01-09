using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Represents the type/category of a milestone
/// </summary>
public sealed class MilestoneType : ValueObject
{
    public string Value { get; }

    // Predefined milestone types
    public static readonly MilestoneType LiteratureReview = new("LiteratureReview");
    public static readonly MilestoneType ResearchDesign = new("ResearchDesign");
    public static readonly MilestoneType DataCollection = new("DataCollection");
    public static readonly MilestoneType Analysis = new("Analysis");
    public static readonly MilestoneType DraftSubmission = new("DraftSubmission");
    public static readonly MilestoneType FinalSubmission = new("FinalSubmission");
    public static readonly MilestoneType Presentation = new("Presentation");

    private MilestoneType(string value)
    {
        Value = value;
    }

    public static MilestoneType FromString(string value)
    {
        return value switch
        {
            "LiteratureReview" => LiteratureReview,
            "ResearchDesign" => ResearchDesign,
            "DataCollection" => DataCollection,
            "Analysis" => Analysis,
            "DraftSubmission" => DraftSubmission,
            "FinalSubmission" => FinalSubmission,
            "Presentation" => Presentation,
            _ => throw new ArgumentException($"Invalid milestone type: {value}", nameof(value))
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
