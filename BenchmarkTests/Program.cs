using BenchmarkDotNet.Running;
using BenchmarkTests.Classes;

namespace BenchmarkTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AuthorQueryBenchmark>();
        }
    }
}
