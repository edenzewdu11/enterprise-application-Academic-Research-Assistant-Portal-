using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ARAP.Modules.ResearchProposal.Application.Commands.CreateProposal;
using ARAP.Modules.ResearchProposal.Application.Commands.SubmitProposal;
using ARAP.Modules.ResearchProposal.Application.Commands.ApproveProposal;
using ARAP.Modules.ResearchProposal.Application.Queries.GetProposalById;
using ARAP.Modules.ResearchProposal.Application.Queries.GetProposalsByStudent;
using ARAP.Modules.ResearchProposal.Application.DTOs;

namespace ARAP.Modules.ResearchProposal.Api.Controllers;

[ApiController]
[Route("api/proposals")]
[Produces("application/json")]
public class ProposalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProposalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new research proposal draft
    /// </summary>
    /// <param name="command">Proposal details</param>
    /// <returns>Created proposal ID</returns>
    [HttpPost]
    [Authorize(Roles = "student")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateProposal([FromBody] CreateProposalCommand command)
    {
        // Create a new command with the student ID from the authenticated user
        var commandWithUserId = command with { StudentId = GetCurrentUserId() };
        
        var proposalId = await _mediator.Send(commandWithUserId);

        return CreatedAtAction(
            nameof(GetProposalById),
            new { id = proposalId },
            new { proposalId = proposalId }
        );
    }

    /// <summary>
    /// Submit a draft proposal for advisor review
    /// </summary>
    /// <param name="id">Proposal ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id}/submit")]
    [Authorize(Roles = "student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitProposal(Guid id)
    {
        var studentId = GetCurrentUserId();
        if (studentId == Guid.Empty)
        {
            return Unauthorized(new { error = "Unable to identify current user" });
        }

        var command = new SubmitProposalCommand { ProposalId = id, StudentId = studentId };
        await _mediator.Send(command);

        return Ok(new { message = "Proposal submitted successfully" });
    }

    /// <summary>
    /// Approve a submitted research proposal (Advisor only)
    /// </summary>
    /// <param name="id">Proposal ID</param>
    /// <param name="request">Optional approval comments</param>
    /// <returns>Success status</returns>
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "advisor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveProposal(Guid id, [FromBody] ApproveProposalRequest request)
    {
        var advisorId = GetCurrentUserId();
        if (advisorId == Guid.Empty)
        {
            return Unauthorized(new { error = "Unable to identify current user" });
        }

        var command = new ApproveProposalCommand 
        { 
            ProposalId = id, 
            AdvisorId = advisorId, 
            Comments = request.Comments ?? string.Empty 
        };
        await _mediator.Send(command);

        return Ok(new { message = "Proposal approved successfully" });
    }

    /// <summary>
    /// Get a research proposal by ID
    /// </summary>
    /// <param name="id">Proposal ID</param>
    /// <returns>Proposal details</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "student,advisor")]
    [ProducesResponseType(typeof(ProposalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProposalById(Guid id)
    {
        var query = new GetProposalByIdQuery { ProposalId = id };
        var proposal = await _mediator.Send(query);

        if (proposal == null)
        {
            return NotFound(new { error = "Proposal not found" });
        }

        return Ok(proposal);
    }

    /// <summary>
    /// Get all proposals for the current student
    /// </summary>
    /// <returns>List of student's proposals</returns>
    [HttpGet("my")]
    [Authorize(Roles = "student")]
    [ProducesResponseType(typeof(List<ProposalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyProposals()
    {
        var studentId = GetCurrentUserId();
        if (studentId == Guid.Empty)
        {
            return Unauthorized(new { error = "Unable to identify current user" });
        }

        var query = new GetProposalsByStudentQuery { StudentId = studentId };
        var proposals = await _mediator.Send(query);

        return Ok(proposals);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}

/// <summary>
/// Request model for approving a proposal
/// </summary>
public record ApproveProposalRequest(string? Comments);
