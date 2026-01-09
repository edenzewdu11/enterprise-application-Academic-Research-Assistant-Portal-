using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ARAP.Modules.ProgressTracking.Application.Commands.LogActivity;
using ARAP.Modules.ProgressTracking.Application.Commands.UpdateProgress;
using ARAP.Modules.ProgressTracking.Application.Queries.GetActivitiesByProposal;
using ARAP.Modules.ProgressTracking.Application.Queries.GetProgressSummary;

namespace ARAP.Modules.ProgressTracking.Api.Controllers;

[ApiController]
[Route("api/progress")]
public class ProgressController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Log a research activity
    /// </summary>
    [HttpPost("activities")]
    [Authorize(Roles = "student")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogActivity([FromBody] LogActivityRequest request)
    {
        var studentId = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found"));

        var command = new LogActivityCommand(
            request.ProposalId,
            studentId,
            request.ActivityType,
            request.Description,
            request.ProgressPercentage,
            request.HoursSpent);

        var activityId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetActivities), new { proposalId = request.ProposalId }, new { activityId });
    }

    /// <summary>
    /// Update progress percentage for an activity
    /// </summary>
    [HttpPut("activities/{activityId}/progress")]
    [Authorize(Roles = "student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProgress(Guid activityId, [FromBody] UpdateProgressRequest request)
    {
        var command = new UpdateProgressCommand(activityId, request.NewProgressPercentage);
        await _mediator.Send(command);

        return Ok();
    }

    /// <summary>
    /// Get all activities for a proposal
    /// </summary>
    [HttpGet("proposals/{proposalId}/activities")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivities(Guid proposalId)
    {
        var query = new GetActivitiesByProposalQuery(proposalId);
        var activities = await _mediator.Send(query);

        return Ok(activities);
    }

    /// <summary>
    /// Get progress summary for a proposal
    /// </summary>
    [HttpGet("proposals/{proposalId}/summary")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProgressSummary(Guid proposalId)
    {
        var query = new GetProgressSummaryQuery(proposalId);
        var summary = await _mediator.Send(query);

        return Ok(summary);
    }
}

public sealed record LogActivityRequest(
    Guid ProposalId,
    string ActivityType,
    string Description,
    int ProgressPercentage,
    int HoursSpent);

public sealed record UpdateProgressRequest(int NewProgressPercentage);
