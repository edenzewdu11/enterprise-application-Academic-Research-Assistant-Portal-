using MediatR;

namespace ARAP.Modules.AcademicIntegrity.Application.Commands.CompleteCheck;

public sealed record CompleteCheckCommand(
    Guid CheckId,
    decimal SimilarityScore,
    string? Notes) : IRequest;
