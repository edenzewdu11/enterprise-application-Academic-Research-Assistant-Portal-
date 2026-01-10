namespace ARAP.Modules.ProgressTracking.Application.DTOs;

public sealed record ActivityLogDto(
    Guid Id,
    Guid ProposalId,
    Guid StudentId,
    string ActivityType,
    string Description,
    int ProgressPercentage,
    int HoursSpent,
    DateTime LoggedAt,
    DateTime? CompletedAt);
