using FluentValidation;

namespace ARAP.Modules.ResearchProposal.Application.Commands.CreateProposal;

public sealed class CreateProposalCommandValidator : AbstractValidator<CreateProposalCommand>
{
    public CreateProposalCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("Student ID is required");

        RuleFor(x => x.AdvisorId)
            .NotEmpty()
            .WithMessage("Advisor ID is required");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MinimumLength(10)
            .WithMessage("Title must be at least 10 characters")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Abstract)
            .NotEmpty()
            .WithMessage("Abstract is required")
            .MinimumLength(100)
            .WithMessage("Abstract must be at least 100 characters")
            .MaximumLength(3000)
            .WithMessage("Abstract cannot exceed 3000 characters");

        RuleFor(x => x.ResearchQuestion)
            .NotEmpty()
            .WithMessage("Research question is required")
            .MinimumLength(20)
            .WithMessage("Research question must be at least 20 characters")
            .MaximumLength(500)
            .WithMessage("Research question cannot exceed 500 characters")
            .Must(x => x.TrimEnd().EndsWith('?'))
            .WithMessage("Research question should end with a question mark");

        RuleFor(x => x.Milestones)
            .NotEmpty()
            .WithMessage("At least one milestone is required");

        RuleForEach(x => x.Milestones).ChildRules(milestone =>
        {
            milestone.RuleFor(m => m.Type)
                .NotEmpty()
                .WithMessage("Milestone type is required");

            milestone.RuleFor(m => m.Description)
                .NotEmpty()
                .WithMessage("Milestone description is required")
                .MaximumLength(500)
                .WithMessage("Milestone description cannot exceed 500 characters");

            milestone.RuleFor(m => m.Deadline)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Milestone deadline must be in the future");
        });
    }
}
