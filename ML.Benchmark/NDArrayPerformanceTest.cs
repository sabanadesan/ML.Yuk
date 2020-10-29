using System;
using System.Collections.Generic;
using System.Text;

using BenchmarkDotNet.Attributes;

using ML.Yuk;

namespace ML.Benchmark
{
    public class NDArrayPerformanceTest
    {
        [Benchmark]
        public void TestShape()
        {
            NDArray r1 = new NDArray(0, 1, 2);
            NDArray r2 = new NDArray(3, 4, 5);
            NDArray r3 = new NDArray(6, 7, 8);

            NDArray s1 = new NDArray(9, 10, 11);
            NDArray s2 = new NDArray(12, 13, 14);
            NDArray s3 = new NDArray(15, 16, 17);

            NDArray t1 = new NDArray(18, 19, 20);
            NDArray t2 = new NDArray(21, 22, 23);
            NDArray t3 = new NDArray(24, 25, 26);

            NDArray c1 = new NDArray(r1, r2, r3);
            NDArray c2 = new NDArray(s1, s2, s3);
            NDArray c3 = new NDArray(t1, t2, t3);

            NDArray nd = new NDArray(c1, c2, c3);

            NDArray t = nd.Shape();
        }
    }
}
