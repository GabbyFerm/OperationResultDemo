using FluentValidation;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Author ID must be greater than 0.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Author name is required.")
                .MaximumLength(100).WithMessage("Author name must be 100 characters or fewer.");
        }
    }
}
