using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using ML.Yuk;

namespace ML.Test
{
    public class SeriesTest
    {
        [Fact]
        public void TestIndexerIndex()
        {
            NDArray nd = new NDArray(10, 20, 30);
            NDArray index = new NDArray("cat", "hat", "bat");

            Series s = new Series(nd, index);

            int t = s["hat"];

            Assert.True(t.Equals(20), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerIndexSet()
        {
            NDArray nd = new NDArray(10, 20, 30);
            NDArray index = new NDArray("cat", "hat", "bat");

            Series s = new Series(nd, index);

            int i = s["hat"];

            s["hat"] = 100;

            int j = s["hat"];

            Assert.True(j.Equals(100), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerSet()
        {
            NDArray nd = new NDArray(10, 20, 30);

            Series s = new Series(nd);

            int i = s[1];

            s[1] = 100;

            int j = s[1];

            Assert.True(j.Equals(100), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerNoIndex()
        {
            NDArray nd = new NDArray(10, 20, 30);

            Series s = new Series(nd);

            int t = s[1];

            Assert.True(t.Equals(20), "Arrays are not equal.");
        }

        [Fact]
        public void TestEquals()
        {
            NDArray nd = new NDArray(10, 20, 30);
            Series s = new Series(nd);

            NDArray nd1 = new NDArray(10, 20, 30);
            Series s1 = new Series(nd1);

            Assert.True(s.Equals(s1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAdd()
        {
            NDArray nd = new NDArray(10, 20, 30);
            Series s = new Series(nd);

            s.Add(5);

            NDArray nd1 = new NDArray(10, 20, 30, 5);
            Series s1 = new Series(nd1);

            Assert.True(s.Equals(s1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddIndex()
        {
            NDArray nd = new NDArray(10, 20, 30);
            Series s = new Series(nd);

            s.Add(5, "moo");

            NDArray nd1 = new NDArray(10, 20, 30, 5);
            Series s1 = new Series(nd1, new NDArray(0, 1, 2, "moo"));

            Assert.True(s.Equals(s1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddIndexReplace()
        {
            NDArray nd = new NDArray(10, 20, 30);
            Series s = new Series(nd, new NDArray(0, "cat", 2));

            s.Add(5, "cat");

            NDArray nd1 = new NDArray(10, 20, 30, 5);
            Series s1 = new Series(nd1, new NDArray(0, "cat", 2, "cat"));

            Assert.True(s.Equals(s1), "Arrays are not equal.");
        }

        [Fact]
        public void TestMax()
        {
            NDArray nd = new NDArray(10, 20, 30);
            Series s = new Series(nd, new NDArray(0, "cat", 2));

            double t = s.Max();

            Assert.True(t.Equals(30), "Arrays are not equal.");
        }
    }
}
