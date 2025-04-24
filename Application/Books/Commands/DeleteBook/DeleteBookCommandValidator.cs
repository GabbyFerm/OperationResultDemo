using FluentValidation;

namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator() 
        {
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Book ID must be greater than 0.");
        }
    }
}
