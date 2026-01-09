namespace ARAP.Modules.ProgressTracking.Application.DTOs;

public sealed record ProgressSummaryDto(
    Guid ProposalId,
    int TotalActivities,
    int CompletedActivities,
    int InProgressActivities,
    int AverageProgressPercentage,
    int TotalHoursSpent);
