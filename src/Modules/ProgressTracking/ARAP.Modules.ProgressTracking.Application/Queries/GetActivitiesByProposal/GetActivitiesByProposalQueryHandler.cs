using ARAP.Modules.ProgressTracking.Application.DTOs;
using ARAP.Modules.ProgressTracking.Domain.Repositories;
using MediatR;

namespace ARAP.Modules.ProgressTracking.Application.Queries.GetActivitiesByProposal;

public sealed class GetActivitiesByProposalQueryHandler 
    : IRequestHandler<GetActivitiesByProposalQuery, IReadOnlyList<ActivityLogDto>>
{
    private readonly IActivityLogRepository _repository;

    public GetActivitiesByProposalQueryHandler(IActivityLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ActivityLogDto>> Handle(
        GetActivitiesByProposalQuery request, 
        CancellationToken cancellationToken)
    {
        var activities = await _repository.GetByProposalIdAsync(request.ProposalId, cancellationToken);

        return activities.Select(a => new ActivityLogDto(
            a.Id.Value,
            a.ProposalId,
            a.StudentId,
            a.Type.Name,
            a.Description.Value,
            a.Progress.Value,
            a.HoursSpent,
            a.LoggedAt,
            a.CompletedAt
        )).ToList();
    }
}
