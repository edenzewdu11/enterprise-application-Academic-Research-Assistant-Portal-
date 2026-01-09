using MediatR;
using ARAP.Modules.DocumentReview.Application.DTOs;

namespace ARAP.Modules.DocumentReview.Application.Queries.GetDocumentById;

public sealed record GetDocumentByIdQuery(Guid DocumentId) : IRequest<DocumentDto?>;
