using FluentValidation;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Book ID must be greater than 0.");

            RuleFor(command => command.Title)
                .NotEmpty().WithMessage("Book title is required.")
                .MaximumLength(150).WithMessage("Book title must be 150 characters or fewer.");

            RuleFor(command => command.AuthorId)
                .GreaterThan(0).WithMessage("Author ID must be greater than 0.");
        }
    }

}
