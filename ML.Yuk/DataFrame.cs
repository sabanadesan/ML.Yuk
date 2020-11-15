using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

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

                _indexes = NDArray.Unique(_indexes.Concat(t.GetIndex()));
            }
        }

        public void SetIndex(string column)
        {
            int i = FindIndexCol(column);

            Series col = _data[i];

            _data.Remove(i);
            _columns.Remove(i);

            NDArray new_index = col.GetValue();

            for (int j = 0; j < _data.Length; j++)
            {
                Series scol = _data[j];

                NDArray col_index = scol.GetIndex();

                NDArray index = Map(_indexes, col_index, new_index);

                scol.SetIndex(index);
            }

            _indexes = new_index;
        }

        private NDArray Map(NDArray old_index, NDArray col_index, NDArray new_index)
        {
            NDArray t = new NDArray();

            for (int i = 0; i < col_index.Length; i++)
            {
                int index = FindInArray(old_index, col_index[i]);
                
                if (index != -1)
                {
                    t.Add(new_index[index]);
                }
            }

            return t;
        }

        private int FindInArray(NDArray array, dynamic item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public void AddColumn(Pair p)
        {
            String k = p.Key();
            Series t = p.Value();

            _columns.Add(k);
            _data.Add(t);

            //To Do: Get all indexes and unique
            _indexes = NDArray.Unique(_indexes.Concat(t.GetIndex()));
        }

        public DataFrame GetRow(dynamic index)
        {
            int row = FindIndexRow(index);

            NDArray array = new NDArray();

            for (int i = 0; i < _data.Length; i++)
            {
                Series scol = _data[i];
                dynamic a = scol[row];
                array.Add(a);
            }

            DataFrame df = new DataFrame();
            df.Add(array, new NDArray(_indexes[row]), _columns.Copy());

            return df;
        }

        private DataFrame GetCol(string column)
        {
            int i = FindIndexCol(column);

            return GetColByIndex(i);
        }

        private DataFrame GetColByIndex(int column)
        {
            Series scol = _data[column];

            DataFrame df = new DataFrame();
            df.AddColumn(new Pair(_columns[column], scol));

            return df;
        }

        private Series GetColBySeries(string column)
        {
            int i = FindIndexCol(column);

            return GetColBySeriesByIndex(i);
        }

        private Series GetColBySeriesByIndex(int column)
        {
            Series scol = _data[column];

            return scol;
        }

        private void SetCol(string column, NDArray value)
        {
            int i = FindIndexCol(column);

            SetColByIndex(i, value);
        }

        private void SetColByIndex(int column, NDArray value)
        {
            Series t = new Series(value);

            _data[column] = t;

            _indexes = NDArray.Unique(_indexes.Concat(t.GetIndex()));
        }


        private void SetCol(string column, Series value)
        {
            int i = FindIndexCol(column);

            SetColByIndex(i, value);
        }

        private void SetColByIndex(int column, Series value)
        {
            _data[column] = value;

            _indexes = NDArray.Unique(_indexes.Concat(value.GetIndex()));
        }

        public dynamic this[string col]
        {
            get => GetCol(col);
            set => SetCol(col, value);
        }

        public dynamic this[int col]
        {
            get => GetColByIndex(col);
            set => SetColByIndex(col, value);
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
            dynamic t = scol[row];

            return t;
        }

        private DataFrame Get(Slice row, int col)
        {
            Series scol = _data[col];
            NDArray t = scol[row];

            DataFrame df = new DataFrame();

            df.AddColumn(new Pair(_columns[col], new Series(t, _indexes[row])));

            return df;
        }

        private DataFrame Get(int row, Slice col)
        {
            int startCol = GetIndex(col.Start);
            int endCol = GetIndex(col.End);

            DataFrame df = new DataFrame();

            for (int i = 0; i < _data.Length; i++)
            {
                if (i >= startCol && i < endCol)
                {
                    Series scol = _data[i];
                    dynamic a = scol[row];

                    df.AddColumn(new Pair(_columns[i], new Series(new NDArray(a), new NDArray(_indexes[row]))));
                }
            }

            return df;
        }

        private void Set(int row, int col, dynamic value)
        {
            Series scol = _data[col];
            scol[row] = value;
        }

        private DataFrame Get(Slice row, Slice col)
        {
            int startRow = GetIndex(row.Start);
            int endRow = GetIndex(row.End);

            int startCol = GetIndex(col.Start);
            int endCol = GetIndex(col.End);

            DataFrame df = new DataFrame();

            for (int i = 0; i < _data.Length; i++)
            {
                if (i >= startCol && i < endCol)
                {
                    Series scol = _data[i];
                    NDArray a = scol[row];

                    df.AddColumn(new Pair(_columns[i], new Series(a, _indexes[row])));
                }
            }
         
            return df;
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

        public NDArray Columns
        {
            get { return _columns.Copy(); }
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
            dynamic new_value = SetType(value, type);
            array.Add(new_value, index);
        }

        private void SetToSeries(Series array, int index, dynamic value, Type type)
        {

            dynamic new_value = SetType(value, type);
            array[index] = new_value;
        }

        private static dynamic SetType(dynamic value, Type type)
        {
            dynamic new_value = null;

            if (type.Equals(typeof(int)))
            {
                new_value = Convert.ToInt32(value);
            }
            else if (type.Equals(typeof(bool)))
            {
                new_value = Convert.ToBoolean(value);
            }
            else if (type.Equals(typeof(double)))
            {
                new_value = Convert.ToDouble(value);
            }
            else if (type.Equals(typeof(DateTime)))
            {
                new_value = Convert.ToDateTime(value);
            }
            else
            {
                new_value = Convert.ToString(value);
            }

            return new_value;
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
        public static DataFrame LoadCsv(string filename, char separator = ',', bool header = true, NDArray columnNames = null, NDArray dataTypes = null, bool addIndexColumn = false)
        {
            FileStream fs = File.OpenRead(filename);

            DataFrame df = LoadCsv(fs, separator, header, columnNames, dataTypes, addIndexColumn);

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
        public static DataFrame LoadCsv(Stream csvStream, char separator = ',', bool header = true, NDArray columnNames = null, NDArray dataTypes = null, bool addIndexColumn = false)
        {
            DataFrame df = new DataFrame();

            String s = Read(csvStream);

            string[] lines = s.Split('\n');

            NDArray array;
            NDArray cols = columnNames;

            NDArray indexVal = null;
            Type type = null;

            if (addIndexColumn)
            {
                type = dataTypes[0];
                dataTypes = RemoveIndexFromDataType(dataTypes);
            }

            for (int i = 0; i < lines.Length - 1; i++)
            {
                array = GetLine(lines[i], addIndexColumn);
                
                if (array.Length > 0)
                {
                    if (header == true && i == 0)
                    {
                        cols = array;
                    }
                    else
                    {
                        if (addIndexColumn)
                        {
                            indexVal = GetLineIndex(lines[i], type);
                        }

                        df.Add(array, indexVal, cols, dataTypes);
                    }
                }
            }

            return df;
        }

        private static NDArray RemoveIndexFromDataType(NDArray dataTypes)
        {
            NDArray nd = new NDArray();

            for(int i = 1; i < dataTypes.Length; i++)
            {
                nd.Add(dataTypes[i]);
            }

            return nd;
        }

        private static NDArray GetLine(String line, bool ignoreIndex = false)
        {
            NDArray nd = new NDArray();

            TextFieldParser parser = new TextFieldParser();

            string[] fields;

            line = line.Replace("\r", "");
            line = line.Replace("\0", "");

            fields = parser.ParseFields(line);
            for(int i = 0; i < fields.Length; i++)
            {
                if (ignoreIndex == true && i == 0)
                {
                    // Skip
                }
                else
                {
                    string field = fields[i];
                    nd.Add(field);
                }
            }

            return nd;
        }

        private static NDArray GetLineIndex(String line, Type type)
        {
            NDArray nd = null;

            TextFieldParser parser = new TextFieldParser();

            string[] fields;

            line = line.Replace("\r", "");
            line = line.Replace("\0", "");

            fields = parser.ParseFields(line);

            if (fields.Length > 0)
            {
                string field = fields[0];

                nd = new NDArray();

                dynamic new_field = SetType(field, type);

                nd.Add(new_field);
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

        public DataFrame PctChange()
        {
            return ApplyPreviousFunc(this, CalcPctChange);
        }

        public DataFrame CumProd()
        {
            return ApplyPreviousAllFunc(this, Product, true);
        }

        private static double Sum(double a, double b)
        {
            return a + b;
        }

        private static double Minus(double a, double b)
        {
            return a - b;
        }

        private static double Product(double a, double b)
        {
            return a * b;
        }

        private static double Div(double a, double b)
        {
            return a / b;
        }

        private static double CalcPctChange(double a, double b)
        {
            return ((b - a) / a);
        }

        private static DataFrame ApplyFunc(DataFrame a, DataFrame b, Func<double, double, double> c)
        {
            DataFrame z = new DataFrame();

            for (int i = 0; i < a.Columns.Length; i++)
            {
                Series t1 = new Series();

                Series t = a.GetColBySeriesByIndex(i);

                for (int j = 0; j < t.Length; j++)
                {
                    t1.Add(c(a[j, i], b[j, i])); 
                }

                z.AddColumn(new Pair(a.Columns[i], t1));
            }

            return z;
        }

        private static DataFrame ApplyFuncDouble(DataFrame a, double b, Func<double, double, double> c)
        {
            DataFrame z = new DataFrame();

            for (int i = 0; i < a.Columns.Length; i++)
            {
                Series t1 = new Series();

                Series t = a.GetColBySeriesByIndex(i);

                for (int j = 0; j < t.Length; j++)
                {
                    t1.Add(c(a[j, i], b));
                }

                z.AddColumn(new Pair(a.Columns[i], t1));
            }

            return z;
        }

        private static DataFrame ApplyPreviousFunc(DataFrame a, Func<double, double, double> c, bool isDefault = false)
        {
            DataFrame z = new DataFrame();

            for (int i = 0; i < a.Columns.Length; i++)
            {
                Series t1 = new Series();

                Series t = a.GetColBySeriesByIndex(i);

                for (int j = 0; j < t.Length; j++)
                {
                    if (j == 0)
                    {
                        if (isDefault)
                        {
                            t1.Add(a[j, i]);
                        }
                        else
                        {
                            t1.Add(null);
                        }
                    }
                    else
                    {
                        t1.Add(c(a[j-1, i], a[j, i]));
                    }
                }

                z.AddColumn(new Pair(a.Columns[i], t1));
            }

            return z;
        }

        private static DataFrame ApplyPreviousAllFunc(DataFrame a, Func<double, double, double> c, bool isDefault = false)
        {
            DataFrame z = new DataFrame();

            for (int i = 0; i < a.Columns.Length; i++)
            {
                Series t1 = new Series();

                Series t = a.GetColBySeriesByIndex(i);

                double total = 0;

                for (int j = 0; j < t.Length; j++)
                {
                    if (j == 0)
                    {
                        if (isDefault)
                        {
                            t1.Add(a[j, i]);
                        }
                        else
                        {
                            t1.Add(null);
                        }
                    }
                    else if (j == 1)
                    {
                        total = c(a[j-1, i], a[j, i]);
                        t1.Add(total);
                    }
                    else
                    {
                        total = c(total, a[j, i]);
                        t1.Add(total);
                    }
                }

                z.AddColumn(new Pair(a.Columns[i], t1));
            }

            return z;
        }

        public static DataFrame operator +(DataFrame a)
        {
            return a;
        }

        public static DataFrame operator -(DataFrame a)
        {
            return a * -1;
        }

        public static DataFrame operator +(DataFrame a, DataFrame b)
        {
            return ApplyFunc(a, b, Sum);
        }

        public static DataFrame operator +(DataFrame a, double b)
        {
            return ApplyFuncDouble(a, b, Sum);
        }

        public static DataFrame operator +(double a, DataFrame b)
        {
            return ApplyFuncDouble(b, a, Sum);
        }

        public static DataFrame operator -(DataFrame a, DataFrame b)
        {
            return a + (-b);
        }

        public static DataFrame operator -(DataFrame a, double b)
        {
            return a + (-b);
        }

        public static DataFrame operator *(DataFrame a, DataFrame b)
        {
            return ApplyFunc(a, b, Product);
        }

        public static DataFrame operator *(DataFrame a, double b)
        {
            return ApplyFuncDouble(a, b, Product);
        }

        public static DataFrame operator *(double a, DataFrame b)
        {
            return ApplyFuncDouble(b, a, Product);
        }

        public static DataFrame operator /(DataFrame a, DataFrame b)
        {
            return ApplyFunc(a, b, Div);
        }

        public static DataFrame operator /(DataFrame a, double b)
        {
            return ApplyFuncDouble(a, b, Div);
        }

        public NDArray GetValue()
        {
            NDArray nd = new NDArray();

            for (int i = 0; i < _data.Length; i++)
            {
                Series col = _data[i];

                NDArray val = col.GetValue();

                nd.Add(val);
            }

            return nd;
        }
    }
}