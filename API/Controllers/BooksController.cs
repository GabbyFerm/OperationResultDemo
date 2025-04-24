using Application.Books.Commands.CreateBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _mediator.Send(new GetAllBooksQuery());
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var result = await _mediator.Send(new GetBookByIdQuery(id));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetBookById), new { id = result.Data!.Id }, result)
                : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}