using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Xunit;

using System.Linq;
using ML.Yuk;
using System.Xml.Serialization;
using Xunit.Sdk;
using System.Reflection.Metadata.Ecma335;

namespace ML.Test
{
    public class DataFrameUnitTest
    {
        private String GetPath(String symbol, String csvDir)
        {
            // Append symbol name to file extension
            string fileName = symbol + ".csv";

            // Get current directory from build folder
            string currentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

            // Append current directory to csv directory
            string path = Path.Combine(currentDir, csvDir);

            // Append csv directory path to file name
            var filePath = Path.Combine(path, fileName);

            return filePath;
        }

        [Fact]
        public void TestEquals()
        {
            Pair col1 = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col2 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col1, col2);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df1 = new DataFrame(col3, col4);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerGet()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            int i = df[2, 1];

            Assert.True(i.Equals(6), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerSet()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            int i = df[2, 1];

            df[2, 1] = 10;

            int j = df[2, 1];

            Assert.True(j.Equals(10), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerGetColumn()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            int i = df[1, "Column2"];

            Assert.True(i.Equals(5), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerGetSliceCol()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = df[new Slice(0, 2), "Column2"];

            Assert.True(i.Equals(new NDArray(4, 5)), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerGetSliceRow()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = df[1, new Slice(0, 2)];

            Assert.True(i.Equals(new NDArray(2, 5)), "Arrays are not equal.");
        }

        [Fact]
        public void TestIndexerGetSlice()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = df[new Slice(1, 3), new Slice(0, 2)];

            Assert.True(i.Equals(new NDArray(new NDArray(2, 3), new NDArray(5, 6))), "Arrays are not equal.");
        }

        [Fact]
        public void TestAdd()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(new NDArray(7, 8), new NDArray(9, 10));

            df.Add(i);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 2, 3, 7, 8)));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 5, 6, 9, 10)));

            DataFrame df1 = new DataFrame(col3, col4);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddGetIndex()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(new NDArray(7, 8), new NDArray(9, 10));

            df.Add(i);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 2, 3, 7, 8)));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 5, 6, 9, 10)));

            NDArray index = df.GetIndex();

            Assert.True(index.Equals(new NDArray(0, 1, 2, 3, 4)), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddSingle()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(7, 8);

            df.Add(i);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 2, 3, 7)));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 5, 6, 8)));

            DataFrame df1 = new DataFrame(col3, col4);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddIndexColumn()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(new NDArray(7, 8), new NDArray(9, 10));

            NDArray index = new NDArray(1, 2);
            NDArray columns = new NDArray("Column1", "Column2");

            df.Add(i, index, columns);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 7, 8)));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 9, 10)));

            DataFrame df1 = new DataFrame(col3, col4);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddNewIndex()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(new NDArray(7, 8), new NDArray(9, 10));

            NDArray index = new NDArray("Hat", "Cat");

            df.Add(i, index, null);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 2, 3, 7, 8), new NDArray(0, 1, 2, "Hat", "Cat")));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 5, 6, 9, 10), new NDArray(0, 1, 2, "Hat", "Cat")));

            DataFrame df1 = new DataFrame(col3, col4);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddNewIndexGetIndex()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(new NDArray(7, 8), new NDArray(9, 10));

            NDArray index = new NDArray("Hat", "Cat");

            df.Add(i, index, null);

            NDArray t = df.GetIndex();

            Assert.True(t.Equals(new NDArray(0, 1, 2, "Hat", "Cat")), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddIndex()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(new NDArray(7, 8), new NDArray(9, 10));

            NDArray index = new NDArray(1, 2);

            df.Add(i, index, null);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 7, 8)));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 9, 10)));

            DataFrame df1 = new DataFrame(col3, col4);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void TestAddColumn()
        {
            Pair col = new Pair("Column1", new Series(new NDArray(1, 2, 3)));
            Pair col1 = new Pair("Column2", new Series(new NDArray(4, 5, 6)));

            DataFrame df = new DataFrame(col, col1);

            NDArray i = new NDArray(new NDArray(7, 8), new NDArray(9, 10));

            NDArray columns = new NDArray("Column1", "Column2");

            df.Add(i, null, columns);

            Pair col3 = new Pair("Column1", new Series(new NDArray(1, 2, 3, 7, 8)));
            Pair col4 = new Pair("Column2", new Series(new NDArray(4, 5, 6, 9, 10)));

            DataFrame df1 = new DataFrame(col3, col4);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void ReadCsv()
        {
            String filePath = GetPath("AMZN", "csv");

            NDArray data = new NDArray(typeof(DateTime), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(int));

            FileStream fs = File.OpenRead(filePath);

            DataFrame df = DataFrame.LoadCsv(fs, ',', true, null, data);

            fs.Close();

            Pair col = new Pair("Date", new Series(new NDArray(DateTime.Parse("2018-03-12"), DateTime.Parse("2018-03-13"), DateTime.Parse("2018-03-14"))));
            Pair col1 = new Pair("Open", new Series(new NDArray(1592.599976, 1615.959961, 1597.000000)));
            Pair col2 = new Pair("High", new Series(new NDArray(1605.329956, 1617.540039, 1606.439941)));
            Pair col3 = new Pair("Low", new Series(new NDArray(1586.699951, 1578.010010, 1590.890015)));
            Pair col4 = new Pair("Close", new Series(new NDArray(1598.390015, 1588.180054, 1591.000000)));
            Pair col5 = new Pair("Adj Close", new Series(new NDArray(1598.390015, 1588.180054, 1591.000000)));
            Pair col6 = new Pair("Volume", new Series(new NDArray(5174200, 6531900, 4175400)));

            DataFrame df1 = new DataFrame(col, col1, col2, col3, col4, col5, col6);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void ReadCsvFile()
        {
            String filePath = GetPath("AMZN", "csv");

            NDArray data = new NDArray(typeof(DateTime), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(int));

            DataFrame df = DataFrame.LoadCsv(filePath, ',', true, null, data);

            Pair col = new Pair("Date", new Series(new NDArray(DateTime.Parse("2018-03-12"), DateTime.Parse("2018-03-13"), DateTime.Parse("2018-03-14"))));
            Pair col1 = new Pair("Open", new Series(new NDArray(1592.599976, 1615.959961, 1597.000000)));
            Pair col2 = new Pair("High", new Series(new NDArray(1605.329956, 1617.540039, 1606.439941)));
            Pair col3 = new Pair("Low", new Series(new NDArray(1586.699951, 1578.010010, 1590.890015)));
            Pair col4 = new Pair("Close", new Series(new NDArray(1598.390015, 1588.180054, 1591.000000)));
            Pair col5 = new Pair("Adj Close", new Series(new NDArray(1598.390015, 1588.180054, 1591.000000)));
            Pair col6 = new Pair("Volume", new Series(new NDArray(5174200, 6531900, 4175400)));

            DataFrame df1 = new DataFrame(col, col1, col2, col3, col4, col5, col6);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }

        [Fact]
        public void ReadCsvFileIndex()
        {
            String filePath = GetPath("AMZN", "csv");

            NDArray data = new NDArray(typeof(DateTime), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(int));

            DataFrame df = DataFrame.LoadCsv(filePath, ',', true, null, data, true);

            NDArray index = new NDArray(DateTime.Parse("2018-03-12"), DateTime.Parse("2018-03-13"), DateTime.Parse("2018-03-14"));

            Pair col1 = new Pair("Open", new Series(new NDArray(1592.599976, 1615.959961, 1597.000000), index));
            Pair col2 = new Pair("High", new Series(new NDArray(1605.329956, 1617.540039, 1606.439941), index));
            Pair col3 = new Pair("Low", new Series(new NDArray(1586.699951, 1578.010010, 1590.890015), index));
            Pair col4 = new Pair("Close", new Series(new NDArray(1598.390015, 1588.180054, 1591.000000), index));
            Pair col5 = new Pair("Adj Close", new Series(new NDArray(1598.390015, 1588.180054, 1591.000000), index));
            Pair col6 = new Pair("Volume", new Series(new NDArray(5174200, 6531900, 4175400), index));

            DataFrame df1 = new DataFrame(col1, col2, col3, col4, col5, col6);

            Assert.True(df.Equals(df1), "Arrays are not equal.");
        }
    }
}
