using ARAP.SharedKernel;

namespace ARAP.Modules.Notifications.Domain.ValueObjects;

public sealed class NotificationType : Enumeration
{
    public static readonly NotificationType ProposalSubmitted = new(1, nameof(ProposalSubmitted));
    public static readonly NotificationType ProposalApproved = new(2, nameof(ProposalApproved));
    public static readonly NotificationType ProposalRejected = new(3, nameof(ProposalRejected));
    public static readonly NotificationType DocumentReviewAssigned = new(4, nameof(DocumentReviewAssigned));
    public static readonly NotificationType DocumentFeedbackProvided = new(5, nameof(DocumentFeedbackProvided));
    public static readonly NotificationType DocumentApproved = new(6, nameof(DocumentApproved));
    public static readonly NotificationType PlagiarismCheckCompleted = new(7, nameof(PlagiarismCheckCompleted));
    public static readonly NotificationType ProgressMilestoneReached = new(8, nameof(ProgressMilestoneReached));
    public static readonly NotificationType DeadlineReminder = new(9, nameof(DeadlineReminder));
    public static readonly NotificationType SystemAnnouncement = new(10, nameof(SystemAnnouncement));

    private NotificationType(int value, string name) : base(value, name)
    {
    }

    public bool IsProposalRelated => Value >= 1 && Value <= 3;
    public bool IsDocumentRelated => Value >= 4 && Value <= 6;
    public bool IsPlagiarismRelated => Value == 7;
    public bool IsProgressRelated => Value == 8;
    public bool IsReminder => Value == 9;
    public bool IsSystemMessage => Value == 10;
}
