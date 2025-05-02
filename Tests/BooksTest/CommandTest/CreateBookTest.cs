using Application.Books.Commands.CreateBook;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.BooksTest.CommandTest
{
    [TestFixture] // Marks this class as a test class for NUnit
    public class CreateBookTest
    {
        // Fields used by all tests
        private AppDbContext _context;
        private IGenericRepository<Book> _repository;
        private IMapper _mapper;
        private CreateBookCommandHandler _handler;

        [SetUp] // Runs before each test
        public void SetUp()
        {
            // Create an in-memory database with a unique name for each test
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            // Set up the fake database context and repository
            _context = new AppDbContext(options);
            _repository = new GenericRepository<Book>(_context);

            // Mock AutoMapper to simulate real mapping logic
            var mapperMock = new Mock<IMapper>();

            // Simulate mapping from CreateBookCommand to Book
            mapperMock.Setup(map => map.Map<Book>(It.IsAny<CreateBookCommand>()))
                .Returns((CreateBookCommand cmd) => new Book { Title = cmd.Title });

            // Simulate mapping from Book to BookDto
            mapperMock.Setup(map => map.Map<BookDto>(It.IsAny<Book>()))
                .Returns((Book book) => new BookDto { Id = book.Id, Title = book.Title });

            // Use the mock mapper in the test handler
            _mapper = mapperMock.Object;
            _handler = new CreateBookCommandHandler(_repository, _mapper);
        }

        [TearDown] // Runs after every test
        public void TearDown()
        {
            _context.Dispose(); // Clean up the in-memory database
        }

        [Test] // Test: Valid book creation
        public async Task Handle_ValidCommand_ReturnsCreatedBook()
        {
            // Arrange: Create a command with a valid book title
            var command = new CreateBookCommand
            {
                Title = "New Book"
            };

            // Act: Call the handler to create the book
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert: The result should be successful and return a BookDto with the correct title
            Assert.IsTrue(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.That(result.Data.Title, Is.EqualTo("New Book"));
        }

        [Test] // Test: Simulate a failed mapping scenario
        public async Task Handle_MapperReturnsNull_ReturnsFailure()
        {
            // Arrange: Create a new handler where the mapper returns null (simulating a mapping failure)
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Book>(It.IsAny<CreateBookCommand>()))
                      .Returns(() => null); // simulate failure

            var failHandler = new CreateBookCommandHandler(_repository, mapperMock.Object);

            var command = new CreateBookCommand { Title = "Broken" };

            // Act: Try to handle the command
            var result = await failHandler.Handle(command, CancellationToken.None);

            // Assert: It should fail because mapping returned null
            Assert.IsFalse(result.IsSuccess);
        }
    }
}
