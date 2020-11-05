using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.IO;

namespace ML.Yuk
{
    public class DataFrame
    {
        private NDArray _data;
        private NDArray _indexes;
        private NDArray _columns;

        public DataFrame()
        {
            InitDataFrame();
        }

        public void InitDataFrame()
        {
            _data = new NDArray();
            _indexes = new NDArray();
            _columns = new NDArray();
        }

        public DataFrame(NDArray data, NDArray index, NDArray columns)
        {
            _data = data;
            _indexes = index;
            _columns = columns;
        }

        public NDArray GetIndex()
        {
            return _indexes.Copy();
        }

        public DataFrame(params Pair[] pair)
        {
            InitDataFrame();

            for (int i = 0; i < pair.Length; i++)
            {
                Pair p = pair[i];

                String k = p.Key();
                Series t = p.Value();

                _columns.Add(k);
                _data.Add(t);

                //To Do: Get all indexes and unique
                _indexes = t.GetIndex();
            }
        }

        public dynamic this[Slice row, Slice col]
        {
            get => Get(row, col);
        }

        public dynamic this[string row, string col]
        {
            get => Get(FindIndexRow(row), FindIndexCol(col));
            set => Set(FindIndexRow(row), FindIndexCol(col), value);
        }

        public dynamic this[int row, int col]
        {
            get => Get(row, col);
            set => Set(row, col, value);
        }

        public dynamic this[String row, int col]
        {
            get => Get(FindIndexRow(row), col);
            set => Set(FindIndexRow(row), col, value);
        }

        public dynamic this[int row, String col]
        {
            get => Get(row, FindIndexCol(col));
            set => Set(row, FindIndexCol(col), value);
        }

        public dynamic this[Slice row, String col]
        {
            get => Get(row, FindIndexCol(col));
        }

        public dynamic this[String row, Slice col]
        {
            get => Get(FindIndexRow(row), col);
        }

        public dynamic this[Slice row, int col]
        {
            get => Get(row, col);
        }

        public dynamic this[int row, Slice col]
        {
            get => Get(row, col);
        }

        private dynamic Get(int row, int col)
        {
            Series scol = _data[col];
            return scol[row];
        }

        private dynamic Get(Slice row, int col)
        {
            Series scol = _data[col];
            dynamic t = scol[row];

            return t;
        }

        private dynamic Get(int row, Slice col)
        {
            int startCol = GetIndex(col.Start);
            int endCol = GetIndex(col.End);

            NDArray array = new NDArray();

            for (int i = 0; i < _data.Length; i++)
            {
                if (i >= startCol && i < endCol)
                {
                    Series scol = _data[i];
                    dynamic a = scol[row];
                    array.Add(a);
                }
            }

            return array;
        }

        private void Set(int row, int col, dynamic value)
        {
            Series scol = _data[col];
            scol[row] = value;
        }

        private NDArray Get(Slice row, Slice col)
        {
            int startRow = GetIndex(row.Start);
            int endRow = GetIndex(row.End);

            int startCol = GetIndex(col.Start);
            int endCol = GetIndex(col.End);

            NDArray array = new NDArray();

            for (int i = 0; i < _data.Length; i++)
            {
                if (i >= startCol && i < endCol)
                {
                    Series scol = _data[i];
                    NDArray a = scol[row];
                    array.Add(a);
                }
            }
         
            return array;
        }

        private int GetIndex(Index index)
        {
            int i = index.Value;

            if (index.IsFromEnd)
            {
                i = this.Length - index.Value;
            }

            return i;
        }

        public int Length
        {
            get { return GetLength(); }
        }

        private int GetLength()
        {
            NDArray array = _data;
            
            int max = 0;

            for(int i = 0; i < array.Length; i++)
            {
                Series s = array[i];
                int j = s.Length;

                if (j > max)
                {
                    max = j;
                }
            }

            return max;
        }

        private int FindIndexCol(dynamic index)
        {
            return FindIndex(_columns, index);
        }

        private int FindIndexRow(dynamic index)
        {
            return FindIndex(_indexes, index);
        }

        private int FindIndex(NDArray array, dynamic index)
        {
            for (int i = 0; i < array.Length; i++)
            {
                dynamic t = array[i];
                if (t.Equals(index))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Equals(DataFrame obj)
        {
            return _data.Equals(obj._data) && _columns.Equals(obj._columns) && _indexes.Equals(obj._indexes);
        }

        public void Add(NDArray data, NDArray index = null, NDArray columns = null, NDArray dataTypes = null)
        {
            AddFind(data, true, 0, 0, index, columns, dataTypes);
        }

        private bool isString(dynamic array)
        {
            try
            {
                String s = array;
                return true;
            }
            catch(Exception e)
            {

            }

            return false;
        }

        private void AddFind(dynamic array, bool isRow, int row, int col, NDArray index = null, NDArray columns = null, NDArray dataTypes = null)
        {
            bool isArray = false;

            try
            {
                int max = 0;

                if (!isString(array))
                {
                    max = array.Length;
                }

                if (max > 0)
                {
                    isArray = true;

                    for (int i = 0; i < max; i++)
                    {
                        if (isRow)
                        {
                            AddFind(array[i], false, row, i, index, columns, dataTypes);
                        }
                        else
                        {
                            AddFind(array[i], true, i, col, index, columns, dataTypes);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            if (!isArray)
            {
                AddToDataFrame(row, col, array, index, columns, dataTypes);
            }
        }

        private void AddToDataFrame(int row, int col, dynamic value, NDArray index = null, NDArray columns = null, NDArray dataTypes = null)
        {
            int i = -1;

            dynamic lbl_col = _columns.Length;
            if (columns != null)
            {
                lbl_col = columns[col];
                i = FindIndexCol(lbl_col);
            }
            else
            {
                i = col;
            }
            
            if (i == -1)
            {
                Series s = new Series();
                _data.Add(s);
                _columns.Add(lbl_col);
                i = FindIndexCol(lbl_col);
            }

            Series t = _data[i];

            int j = -1;
            dynamic lbl_index = _indexes.Length;

            if (index != null)
            {
                lbl_index = index[row];
                j = t.FindIndex(lbl_index);
            }
            else
            {
                j = -1;
            }

            if (j == -1)
            {
                dynamic ind = null;

                if (index != null)
                {
                    ind = lbl_index;
                }

                if (dataTypes != null)
                {
                    Type type = dataTypes[col];
                    AddToSeries(t, value, type, ind);
                }
                else
                {
                   t.Add(value, ind);
                }
                    
                if (col == 0)
                {
                    _indexes.Add(lbl_index);
                }

                j = FindIndexRow(lbl_index);
            }
            else
            {
                if (dataTypes != null)
                {
                    Type type = dataTypes[col];
                    SetToSeries(t, j, value, type);
                }
                else
                {
                    t[j] = value;
                }
            }
        }

        private void AddToSeries(Series array, dynamic value, Type type, dynamic index)
        {
            if (type.Equals(typeof(int)))
            {
                int new_value = Convert.ToInt32(value);
                array.Add(new_value, index);
            }
            else if (type.Equals(typeof(bool)))
            {
                bool new_value = Convert.ToBoolean(value);
                array.Add(new_value, index);
            }
            else if (type.Equals(typeof(double)))
            {
                double new_value = Convert.ToDouble(value);
                array.Add(new_value, index);
            }
            else if (type.Equals(typeof(DateTime)))
            {
                DateTime new_value = Convert.ToDateTime(value);
                array.Add(new_value, index);
            }
            else
            {
                string new_value = Convert.ToString(value);
                array.Add(new_value, index);
            }
        }

        private void SetToSeries(Series array, int index, dynamic value, Type type)
        {
            if (type.Equals(typeof(int)))
            {
                int new_value = Convert.ToInt32(value);
                array[index] = new_value;
            }
            else if (type.Equals(typeof(double)))
            {
                double new_value = Convert.ToDouble(value);
                array[index] = new_value;
            }
            else if (type.Equals(typeof(DateTime)))
            {
                DateTime new_value = Convert.ToDateTime(value);
                array[index] = new_value;
            }
            else
            {
                string new_value = Convert.ToString(value);
                array[index] = new_value;
            }
        }

        //
        // Summary:
        //     Reads a text file as a DataFrame. Follows pandas API.
        //
        // Parameters:
        //   filename:
        //     filename
        //
        //   separator:
        //     column separator
        //
        //   header:
        //     has a header or not
        //
        //   columnNames:
        //     column names (can be empty)
        //
        //   dataTypes:
        //     column types (can be empty)
        //
        //   numRows:
        //     number of rows to read
        //
        //   guessRows:
        //     number of rows used to guess types
        //
        //   addIndexColumn:
        //     add one column with the row index
        //
        //   encoding:
        //     The character encoding. Defaults to UTF8 if not specified
        //
        // Returns:
        //     DataFrame
        public static DataFrame LoadCsv(string filename, char separator = ',', bool header = true, NDArray columnNames = null, NDArray dataTypes = null, int numRows = -1, int guessRows = 10, bool addIndexColumn = false, Encoding encoding = null)
        {
            FileStream fs = File.OpenRead(filename);

            DataFrame df = LoadCsv(fs, separator, header, columnNames, dataTypes, numRows, guessRows, addIndexColumn, encoding);

            fs.Close();

            return df;
        }

        //
        // Summary:
        //     Reads a seekable stream of CSV data into a DataFrame. Follows pandas API.
        //
        // Parameters:
        //   csvStream:
        //     stream of CSV data to be read in
        //
        //   separator:
        //     column separator
        //
        //   header:
        //     has a header or not
        //
        //   columnNames:
        //     column names (can be empty)
        //
        //   dataTypes:
        //     column types (can be empty)
        //
        //   numberOfRowsToRead:
        //     number of rows to read not including the header(if present)
        //
        //   guessRows:
        //     number of rows used to guess types
        //
        //   addIndexColumn:
        //     add one column with the row index
        //
        //   encoding:
        //     The character encoding. Defaults to UTF8 if not specified
        //
        // Returns:
        //     DataFrame
        public static DataFrame LoadCsv(Stream csvStream, char separator = ',', bool header = true, NDArray columnNames = null, NDArray dataTypes = null, long numberOfRowsToRead = -1, int guessRows = 10, bool addIndexColumn = false, Encoding encoding = null)
        {
            DataFrame df = new DataFrame();

            String s = Read(csvStream);

            string[] lines = s.Split('\n');

            NDArray array;
            NDArray cols = null;

            for (int i = 0; i < lines.Length - 1; i++)
            {
                array = GetLine(lines[i]);

                if (array.Length > 0)
                {
                    if (header == true && i == 0)
                    {
                        cols = array;
                    }
                    else
                    {
                        df.Add(array, null, cols, dataTypes);
                    }
                }
            }
               
            return df;
        }

        private static NDArray GetLine(String line)
        {
            NDArray nd = new NDArray();

            TextFieldParser parser = new TextFieldParser();

            string[] fields;

            line = line.Replace("\r", "");
            line = line.Replace("\0", "");

            fields = parser.ParseFields(line);
            foreach (string field in fields)
            {
                nd.Add(field);
            }

            return nd;
        }

        private static String Read(Stream stream)
        {
            byte[] bytes = new byte[stream.Length + 10];
            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;

            do
            {
                // Read may return anything from 0 to 10.
                int n = stream.Read(bytes, numBytesRead, 10);
                numBytesRead += n;
                numBytesToRead -= n;
            } while (numBytesToRead > 0);

            String converted = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            return converted;
        }
    }
}
