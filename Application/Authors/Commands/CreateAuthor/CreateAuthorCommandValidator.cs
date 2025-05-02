using FluentValidation;

namespace Application.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Author name is required")
                .MaximumLength(100).WithMessage("Author name must be less than 100 characters");
        }
    }
}
