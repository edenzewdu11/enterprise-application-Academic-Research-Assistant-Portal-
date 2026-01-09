using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.ValueObjects;

/// <summary>
/// Represents the type/category of a research document
/// </summary>
public sealed class DocumentType : ValueObject
{
    public string Value { get; }

    // Predefined document types
    public static readonly DocumentType ProposalDraft = new("ProposalDraft");
    public static readonly DocumentType LiteratureReview = new("LiteratureReview");
    public static readonly DocumentType ResearchPaper = new("ResearchPaper");
    public static readonly DocumentType ThesisDraft = new("ThesisDraft");
    public static readonly DocumentType FinalThesis = new("FinalThesis");
    public static readonly DocumentType Presentation = new("Presentation");
    public static readonly DocumentType DataAnalysis = new("DataAnalysis");

    private DocumentType(string value)
    {
        Value = value;
    }

    public static DocumentType FromString(string value)
    {
        return value switch
        {
            "ProposalDraft" => ProposalDraft,
            "LiteratureReview" => LiteratureReview,
            "ResearchPaper" => ResearchPaper,
            "ThesisDraft" => ThesisDraft,
            "FinalThesis" => FinalThesis,
            "Presentation" => Presentation,
            "DataAnalysis" => DataAnalysis,
            _ => throw new ArgumentException($"Invalid document type: {value}", nameof(value))
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
