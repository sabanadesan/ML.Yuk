using System;

using BenchmarkDotNet.Running;

namespace ML.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<NDArrayPerformanceTest>();
        }
    }
}
