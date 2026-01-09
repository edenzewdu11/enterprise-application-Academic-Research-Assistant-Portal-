using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ARAP.Modules.DocumentReview.Application.Commands.SubmitDocument;
using ARAP.Modules.DocumentReview.Application.Commands.AssignReviewer;
using ARAP.Modules.DocumentReview.Application.Commands.ProvideFeedback;
using ARAP.Modules.DocumentReview.Application.Commands.ApproveDocument;
using ARAP.Modules.DocumentReview.Application.Queries.GetDocumentById;
using ARAP.Modules.DocumentReview.Application.Queries.GetDocumentsByProposal;
using System.Security.Claims;

namespace ARAP.Modules.DocumentReview.Api.Controllers;

[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;

        return Guid.TryParse(userIdClaim, out var userId)
            ? userId
            : throw new InvalidOperationException("User ID not found in claims");
    }

    [HttpPost]
    [Authorize(Roles = "student")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitDocument([FromBody] SubmitDocumentCommand command)
    {
        var commandWithUserId = command with { StudentId = GetCurrentUserId() };
        var documentId = await _mediator.Send(commandWithUserId);

        return CreatedAtAction(
            nameof(GetDocumentById),
            new { id = documentId },
            new { documentId });
    }

    [HttpPost("{id}/assign-reviewer")]
    [Authorize(Roles = "advisor")]
    [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status200OK)]
    [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignReviewer(Guid id, [FromBody] AssignReviewerRequest request)
    {
        var command = new AssignReviewerCommand
        {
            DocumentId = id,
            ReviewerId = request.ReviewerId
        };

        await _mediator.Send(command);
        return Ok(new { message = "Reviewer assigned successfully" });
    }

    [HttpPost("{id}/feedback")]
    [Authorize(Roles = "advisor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProvideFeedback(Guid id, [FromBody] ProvideFeedbackRequest request)
    {
        var command = new ProvideFeedbackCommand
        {
            DocumentId = id,
            ReviewerId = GetCurrentUserId(),
            Comment = request.Comment
        };

        await _mediator.Send(command);
        return Ok(new { message = "Feedback provided successfully" });
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "advisor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveDocument(Guid id)
    {
        var command = new ApproveDocumentCommand
        {
            DocumentId = id,
            ReviewerId = GetCurrentUserId()
        };

        await _mediator.Send(command);
        return Ok(new { message = "Document approved successfully" });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "student,advisor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentById(Guid id)
    {
        var query = new GetDocumentByIdQuery(id);
        var document = await _mediator.Send(query);

        if (document is null)
            return NotFound();

        return Ok(document);
    }

    [HttpGet("proposal/{proposalId}")]
    [Authorize(Roles = "student,advisor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDocumentsByProposal(Guid proposalId)
    {
        var query = new GetDocumentsByProposalQuery(proposalId);
        var documents = await _mediator.Send(query);

        return Ok(documents);
    }
}

public record AssignReviewerRequest(Guid ReviewerId);
public record ProvideFeedbackRequest(string Comment);
