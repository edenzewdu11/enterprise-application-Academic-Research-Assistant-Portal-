using MediatR;
using ARAP.Modules.AcademicIntegrity.Application.DTOs;

namespace ARAP.Modules.AcademicIntegrity.Application.Queries.GetCheckById;

public sealed record GetCheckByIdQuery(Guid CheckId) : IRequest<PlagiarismCheckDto?>;
