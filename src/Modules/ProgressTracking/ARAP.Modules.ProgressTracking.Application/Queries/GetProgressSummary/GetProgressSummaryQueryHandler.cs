using ARAP.Modules.ProgressTracking.Application.DTOs;
using ARAP.Modules.ProgressTracking.Domain.Repositories;
using MediatR;

namespace ARAP.Modules.ProgressTracking.Application.Queries.GetProgressSummary;

public sealed class GetProgressSummaryQueryHandler 
    : IRequestHandler<GetProgressSummaryQuery, ProgressSummaryDto>
{
    private readonly IActivityLogRepository _repository;

    public GetProgressSummaryQueryHandler(IActivityLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProgressSummaryDto> Handle(
        GetProgressSummaryQuery request, 
        CancellationToken cancellationToken)
    {
        var activities = await _repository.GetByProposalIdAsync(request.ProposalId, cancellationToken);
        var totalHours = await _repository.GetTotalHoursAsync(request.ProposalId, cancellationToken);

        var completedCount = activities.Count(a => a.CompletedAt.HasValue);
        var inProgressCount = activities.Count(a => !a.CompletedAt.HasValue);
        var averageProgress = activities.Any() 
            ? (int)activities.Average(a => a.Progress.Value) 
            : 0;

        return new ProgressSummaryDto(
            request.ProposalId,
            activities.Count,
            completedCount,
            inProgressCount,
            averageProgress,
            totalHours
        );
    }
}
