using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using ML.Yuk;

namespace ML.Test
{
    public class NDArrayUnitTest
    {
        [Fact]
        public void TestShapeStr()
        {
            NDArray nd = new NDArray("Car", "Teddy", "Ho");

            NDArray t = nd.Shape();

            Assert.True(t.Equals(new NDArray(3)), "Arrays are not equal.");
        }

        [Fact]
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

            Assert.True(t.Equals(new NDArray ( 3, 3, 3 )), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerInt3D()
        {
            NDArray r1 = new NDArray(0, 1);
            NDArray r2 = new NDArray(3, 4);

            NDArray s1 = new NDArray(9, 10);
            NDArray s2 = new NDArray(12, 13);

            NDArray t1 = new NDArray(18, 19);
            NDArray t2 = new NDArray(21, 22);

            NDArray c1 = new NDArray(r1, r2);
            NDArray c2 = new NDArray(s1, s2);
            NDArray c3 = new NDArray(t1, t2);

            NDArray nd = new NDArray(c1, c2, c3);

            int i = nd[1, 1, 1];

            Assert.True(i.Equals(13), "Index is not valid.");
        }

        
        [Fact]
        public void TestIndexerNDArray()
        {

            NDArray r1 = new NDArray(0, 1);
            NDArray r2 = new NDArray(3, 4);

            NDArray s1 = new NDArray(9, 10);
            NDArray s2 = new NDArray(12, 13);

            NDArray t1 = new NDArray(18, 19);
            NDArray t2 = new NDArray(21, 22);

            NDArray c1 = new NDArray(r1, r2);
            NDArray c2 = new NDArray(s1, s2);
            NDArray c3 = new NDArray(t1, t2);

            NDArray nd = new NDArray(c1, c2, c3);

            int i = nd[1, 1, 1];

            NDArray z = new NDArray(0, 1);

            nd[1, 1, 1] = z;

            NDArray j = nd[1, 1, 1];

            Assert.True(j.Equals(new NDArray(0, 1)), "Index is not valid.");
        }

        [Fact]
        public void Test3DEquals()
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

            NDArray r11 = new NDArray(0, 1, 2);
            NDArray r22 = new NDArray(3, 4, 5);
            NDArray r33 = new NDArray(6, 7, 8);

            NDArray s11 = new NDArray(9, 10, 11);
            NDArray s22 = new NDArray(12, 13, 14);
            NDArray s33 = new NDArray(15, 16, 17);

            NDArray t11 = new NDArray(18, 19, 20);
            NDArray t22 = new NDArray(21, 22, 23);
            NDArray t33 = new NDArray(24, 25, 26);

            NDArray c11 = new NDArray(r11, r22, r33);
            NDArray c22 = new NDArray(s11, s22, s33);
            NDArray c33 = new NDArray(t11, t22, t33);

            NDArray nd1 = new NDArray(c11, c22, c33);

            Assert.True(nd.Equals(nd1), "Arrays are not equal.");
        }

        [Fact]
        public void TestIntEquals()
        {
            NDArray nd = new NDArray(1, 2, 3);

            NDArray nd1 = new NDArray(1, 2, 3);

            Assert.True(nd.Equals(nd1), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerInt()
        {
            NDArray nd = new NDArray(10, 20, 30);

            int t = nd[1];

            Assert.True(t.Equals(20), "Arrays are not equal.");
        }

        [Fact]
        public void TestSlice3D()
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

            Slice p1 = new Slice(0, 2);
            Slice p2 = new Slice(1, 2);
            Slice p3 = new Slice(0, 1);

            NDArray t = nd[p1, p2, p3];

            NDArray z1 = new NDArray(0, 1, 2);
            NDArray z2 = new NDArray(3, 4, 5);

            NDArray x1 = new NDArray(12, 13, 14);

            NDArray y1 = new NDArray(18, 19, 20);

            NDArray z = new NDArray(new NDArray(z1, z2), new NDArray(x1), new NDArray(y1));

            Assert.True(t.Equals(z), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceInt()
        {
            NDArray nd = new NDArray(10, 20, 30);

            Slice s = new Slice(0, 1);

            NDArray t = nd[s];
            NDArray t2 = new NDArray(10);

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceStr()
        {
            NDArray nd = new NDArray("Car", "Teddy", "Ho");

            Slice s = new Slice(0, 1);

            NDArray t = nd[s];
            NDArray t2 = new NDArray("Car");

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestCopy()
        {
            NDArray nd = new NDArray("Car", "Teddy", "Ho");

            NDArray nd1 = nd.Copy();

            String s = nd[1];

            nd[1] = "Moo";

            String s1 = nd1[1];

            Assert.True(s1.Equals("Teddy"), "Arrays are not equal.");
        }

        [Fact]
        public void TestDeepCopy()
        {
            NDArray r1 = new NDArray(0, 1, 2);
            NDArray r2 = new NDArray(3, 4, 5);
            NDArray r3 = new NDArray(6, 7, 8);

            NDArray nd = new NDArray(r1, r2, r3);

            NDArray nd1 = nd.Copy();

            NDArray s = nd[1];

            nd[1] = new NDArray(0, 0, 0);

            NDArray s1 = nd1[1];

            Assert.True(s1.Equals(new NDArray(3, 4, 5)), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceTwoInt()
        {
            NDArray nd = new NDArray(10, 20, 30);

            Slice s = new Slice(0, 1);
            Slice s1 = new Slice(0, 1);

            NDArray t = nd[s, s1];
            NDArray t2 = new NDArray(10 , 10);

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceIregular()
        {
            NDArray nd = new NDArray(10, new NDArray(20), 30);

            Slice s = new Slice(0, 1);
            Slice s1 = new Slice(0, 1);

            NDArray t = nd[s, s1];
            NDArray t2 = new NDArray(10, new NDArray(20));

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceNDArray()
        {
            NDArray nd = new NDArray(new NDArray(10, 20, 30));

            Slice s = new Slice(0, 1);

            NDArray t = nd[s];
            NDArray t2 = new NDArray(new NDArray(10));

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceTwoNDArray()
        {
            NDArray nd = new NDArray(new NDArray(10, 20), new NDArray(30, 40), new NDArray(50, 60));

            Slice s = new Slice(0, 1);
            Slice s1 = new Slice(1, 2);

            NDArray t = nd[s, s1];
            NDArray t2 = new NDArray(new NDArray(10), new NDArray(40));

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceNDArraySingle()
        {
            NDArray nd = new NDArray(new NDArray(10), new NDArray(30), new NDArray(50, 60));

            Slice s = new Slice(0, 1);
            Slice s1 = new Slice(0, 1);

            NDArray t = nd[s, s1];
            NDArray t2 = new NDArray(new NDArray(10), new NDArray(30));

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestSlice()
        {
            NDArray nd = new NDArray(new NDArray(10, 20, 30), new NDArray(15, 25), new NDArray(1, 2));

            Slice p1 = new Slice(0, 2);
            Slice p2 = new Slice(1, 2);
            Slice p3 = new Slice(0, 1);

            NDArray t = nd[p1, p2, p3];
            NDArray t2 = new NDArray(new NDArray(10, 20), new NDArray(25), new NDArray(1));

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestSliceTwoArrays()
        {
            NDArray nd = new NDArray(new NDArray(10, 20, 30), new NDArray(15, 25), new NDArray(1, 2));

            NDArray nd2 = new NDArray(new NDArray(10, 20, 30), new NDArray(15, 25), new NDArray(1, 2));

            Slice p1 = new Slice(0, 2);
            Slice p2 = new Slice(1, 2);
            Slice p3 = new Slice(0, 1);

            NDArray t = nd[p1, p2, p3];
            NDArray t2 = nd2[p1, p2, p3];

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestRange()
        {
            NDArray nd = new NDArray(new NDArray(10, 20, 30), new NDArray(15, 25), new NDArray(1, 2));

            Range p3 = new Range(0, 1);

            NDArray t = nd[.., 0..1, p3];
            NDArray t2 = new NDArray(new NDArray(10, 20, 30), new NDArray(15), new NDArray(1));

            Assert.True(t.Equals(t2), "Arrays are not equal.");
        }

        [Fact]
        public void TestRemove()
        {
            NDArray nd = new NDArray(1, 2, 3);

            nd.Remove(2);

            NDArray nd1 = new NDArray(1, 2);

            Assert.True(nd.Equals(nd1), "Arrays are not equal.");
        }

        [Fact]
        public void TestStd()
        {
            NDArray nd = new NDArray(1, 2, 3);

            double i = NDArray.Std(nd);

            Assert.True(i.Equals(0.816496580927726), "Arrays are not equal.");
        }

        [Fact]
        public void TestMean()
        {
            NDArray nd = new NDArray(1, 2, 3);

            double i = NDArray.Mean(nd);

            Assert.True(i.Equals(2), "Arrays are not equal.");
        }
    }
}
