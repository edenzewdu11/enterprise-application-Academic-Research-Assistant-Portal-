using ARAP.SharedKernel;

namespace ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

/// <summary>
/// Enumeration for plagiarism check status
/// </summary>
public sealed class CheckStatus : Enumeration
{
    public static readonly CheckStatus Pending = new(1, nameof(Pending));
    public static readonly CheckStatus InProgress = new(2, nameof(InProgress));
    public static readonly CheckStatus Completed = new(3, nameof(Completed));
    public static readonly CheckStatus Failed = new(4, nameof(Failed));

    private CheckStatus(int value, string name) : base(value, name) { }

    public bool IsPending => this == Pending;
    public bool IsInProgress => this == InProgress;
    public bool IsCompleted => this == Completed;
    public bool IsFailed => this == Failed;
}
