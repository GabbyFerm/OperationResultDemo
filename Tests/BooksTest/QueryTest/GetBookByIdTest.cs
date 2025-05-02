using Application.Books.Queries.GetBookById;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.BooksTest.QueryTest
{
    [TestFixture] // Marks this class as a test class for NUnit
    public class GetBookByIdTest
    {
        // Fields used by all tests
        private AppDbContext _context;
        private IGenericRepository<Book> _repository;
        private IMapper _mapper;
        private GetBookByIdQueryHandler _handler;

        [SetUp] // Runs before each test
        public void SetUp()
        {
            // Create an in-memory database with a unique name for each test
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            // Create a fresh in-memory context
            _context = new AppDbContext(options);

            // Add one test book to the in-memory database
            _context.Books.Add(new Book { Id = 1, Title = "Test Book" });
            _context.SaveChanges(); // Save to make it available for querying

            // Use real repository that works with the in-memory DB
            _repository = new GenericRepository<Book>(_context);

            // Mock the AutoMapper behavior for Book → BookDto
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>()))
                .Returns((Book book) => new BookDto { Id = book.Id, Title = book.Title });

            _mapper = mapperMock.Object;

            // Create the handler that we are testing
            _handler = new GetBookByIdQueryHandler(_repository, _mapper);
        }

        [TearDown] // Runs after every test
        public void TearDown()
        {
            _context.Dispose(); // Clean up the in-memory database
        }

        [Test] // Test: Valid ID should return the correct book
        public async Task Handle_ValidId_ReturnsCorrectBook()
        {
            // Arrange: Prepare the query with an existing book ID
            var query = new GetBookByIdQuery(1);

            // Act: Call the handler with the query
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert: Make sure the result is successful and contains the correct data
            Assert.IsTrue(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.That(result.Data!.Id, Is.EqualTo(1));
            Assert.That(result.Data.Title, Is.EqualTo("Test Book"));
        }

        [Test] // Test: Invalid ID should return a failure result
        public async Task Handle_InvalidId_ReturnsFailure()
        {
            // Arrange: Prepare the query with a non-existent book ID
            var query = new GetBookByIdQuery(99); // non-existent

            // Act: Call the handler with the invalid query
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert: Should fail and return a specific error message
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNull(result.Data);
            Assert.That(result.ErrorMessage, Is.EqualTo("Entity not found"));
        }
    }
}