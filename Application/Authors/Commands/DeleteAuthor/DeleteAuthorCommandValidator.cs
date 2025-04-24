using Application.Authors.Commands.DeleteAuthor;
using FluentValidation;

public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
{
    public DeleteAuthorCommandValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0).WithMessage("Author ID must be greater than 0.");
    }
}
