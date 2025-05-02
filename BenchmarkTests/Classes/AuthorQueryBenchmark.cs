using BenchmarkDotNet.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkTests.Classes
{
    [MemoryDiagnoser]
    public class AuthorQueryBenchmark
    {
        private AppDbContext _context;

        public AuthorQueryBenchmark(AppDbContext context)
        {
            _context = context;
        }

        [GlobalSetup]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);
        }

        [Benchmark]
        public async Task GetAuthorsWithLinq()
        {
            var authors = await _context.Authors.ToListAsync();
            var count = authors.Count(); // Force full materialization
        }

        [Benchmark]
        public async Task GetAuthorsWithRawSql()
        {
            var authors = await _context.Authors
                .FromSqlRaw("SELECT * FROM Authors")
                .ToListAsync();
            var count = authors.Count(); // Force full materialization
        }
    }
}
