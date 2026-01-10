using ARAP.SharedKernel;

namespace ARAP.Modules.ProgressTracking.Domain.ValueObjects;

/// <summary>
/// Enumeration for types of research activities
/// </summary>
public sealed class ActivityType : Enumeration
{
    public static readonly ActivityType LiteratureReview = new(1, nameof(LiteratureReview));
    public static readonly ActivityType DataCollection = new(2, nameof(DataCollection));
    public static readonly ActivityType DataAnalysis = new(3, nameof(DataAnalysis));
    public static readonly ActivityType Writing = new(4, nameof(Writing));
    public static readonly ActivityType Revision = new(5, nameof(Revision));
    public static readonly ActivityType Meeting = new(6, nameof(Meeting));
    public static readonly ActivityType Presentation = new(7, nameof(Presentation));
    public static readonly ActivityType Other = new(8, nameof(Other));

    private ActivityType(int id, string name) : base(id, name) { }
}
