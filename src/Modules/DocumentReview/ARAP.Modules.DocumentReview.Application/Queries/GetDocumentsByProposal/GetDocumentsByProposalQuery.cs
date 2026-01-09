using MediatR;
using ARAP.Modules.DocumentReview.Application.DTOs;

namespace ARAP.Modules.DocumentReview.Application.Queries.GetDocumentsByProposal;

public sealed record GetDocumentsByProposalQuery(Guid ProposalId) : IRequest<IReadOnlyList<DocumentDto>>;
