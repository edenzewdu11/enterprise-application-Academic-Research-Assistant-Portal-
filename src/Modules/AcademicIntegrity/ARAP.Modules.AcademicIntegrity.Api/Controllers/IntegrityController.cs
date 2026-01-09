using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ARAP.Modules.AcademicIntegrity.Application.Commands.InitiateCheck;
using ARAP.Modules.AcademicIntegrity.Application.Commands.CompleteCheck;
using ARAP.Modules.AcademicIntegrity.Application.Queries.GetCheckById;
using ARAP.Modules.AcademicIntegrity.Application.Queries.GetChecksByProposal;

namespace ARAP.Modules.AcademicIntegrity.Api.Controllers;

[ApiController]
[Route("api/integrity")]
public class IntegrityController : ControllerBase
{
    private readonly IMediator _mediator;

    public IntegrityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Initiate a plagiarism check for a document
    /// </summary>
    [HttpPost("checks")]
    [Authorize(Roles = "advisor,student")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InitiateCheck([FromBody] InitiateCheckRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found"));

        var command = new InitiateCheckCommand(
            request.DocumentId,
            request.ProposalId,
            userId);

        var checkId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetCheckById), new { id = checkId }, new { checkId });
    }

    /// <summary>
    /// Complete a plagiarism check with results
    /// </summary>
    [HttpPost("checks/{id}/complete")]
    [Authorize(Roles = "advisor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteCheck(Guid id, [FromBody] CompleteCheckRequest request)
    {
        var command = new CompleteCheckCommand(id, request.SimilarityScore, request.Notes);
        await _mediator.Send(command);

        return Ok();
    }

    /// <summary>
    /// Get a plagiarism check by ID
    /// </summary>
    [HttpGet("checks/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCheckById(Guid id)
    {
        var query = new GetCheckByIdQuery(id);
        var check = await _mediator.Send(query);

        if (check == null)
            return NotFound();

        return Ok(check);
    }

    /// <summary>
    /// Get all plagiarism checks for a proposal
    /// </summary>
    [HttpGet("proposals/{proposalId}/checks")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChecksByProposal(Guid proposalId)
    {
        var query = new GetChecksByProposalQuery(proposalId);
        var checks = await _mediator.Send(query);

        return Ok(checks);
    }
}

public sealed record InitiateCheckRequest(
    Guid DocumentId,
    Guid ProposalId);

public sealed record CompleteCheckRequest(
    decimal SimilarityScore,
    string? Notes);
