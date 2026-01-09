using MediatR;

namespace ARAP.Modules.AcademicIntegrity.Application.Commands.InitiateCheck;

public sealed record InitiateCheckCommand(
    Guid DocumentId,
    Guid ProposalId,
    Guid InitiatedBy) : IRequest<Guid>;
