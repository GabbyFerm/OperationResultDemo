using BenchmarkDotNet.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkTests.Classes
{
    [MemoryDiagnoser] // Adds memory usage tracking to benchmark
    public class AuthorQueryBenchmark
    {
        private AppDbContext _context;

        public AuthorQueryBenchmark(AppDbContext context)
        {
            _context = context;
        }

        [GlobalSetup] // Runs once before any benchmark
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            _context = new AppDbContext(options);
        }

        [Benchmark] // Benchmark using LINQ
        public async Task GetAuthorsWithLinq()
        {
            var authors = await _context.Authors.ToListAsync();
            _ = authors.Count; // Materialize result to ensure accurate benchmark
        }

        [Benchmark] // Benchmark using raw SQL
        public async Task GetAuthorsWithRawSql()
        {
            var authors = await _context.Authors
                .FromSqlRaw("SELECT * FROM Authors")
                .ToListAsync();
            _ = authors.Count; // Force full materialization
        }
    }
}