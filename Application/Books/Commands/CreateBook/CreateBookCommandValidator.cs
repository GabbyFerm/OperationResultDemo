using FluentValidation;

namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(command => command.Title)
                .NotEmpty().WithMessage("Book title is required.")
                .MaximumLength(150).WithMessage("Book title must be 150 characters or fewer.");

            RuleFor(command => command.AuthorId)
                .GreaterThan(0).WithMessage("Author ID must be greater than 0.");
        }
    }
}
