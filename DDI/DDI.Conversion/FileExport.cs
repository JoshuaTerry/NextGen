using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;

namespace DDI.Conversion
{
    /// <summary>
    /// Class for exporting EF object into a CSV file format that can be imported by SSIS.
    /// </summary>
    /// <typeparam name="T">Row model: an Entity or class</typeparam>
    public class FileExport<T> : IDisposable where T : class
    {

        #region Private Fields

        private TextWriter _csvFile = null;
        private List<Column> _columns;
        private Type _objectType;
        private StringBuilder _line;
        private int _lineNumber;
        private bool _isOpen;
        private bool _isRowDirty;
        private char _delimiter;

        #endregion

        #region Public Properties

        public int LineNumber { get { return _lineNumber; } }

        #endregion;

        #region Constructors

        public FileExport(string filename, bool append = false, bool ascii = false, char delimiter = ',')
        {            
            _csvFile = new StreamWriter(filename, append, ascii ? Encoding.ASCII : Encoding.Unicode);
            _objectType = typeof(T);
            _lineNumber = 1;
            _isOpen = true;
            _isRowDirty = false;
            _delimiter = delimiter;
            _line = new StringBuilder();

            Initialize();
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Specify column names (which override the property names for the model class.)
        /// </summary>
        public void SetColumnNames(params string[] columnNames)
        {
            int idx = 0;
            foreach (var name in columnNames)
            {
                if (idx < _columns.Count)
                {
                    _columns[idx++].ColumnName = name;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Write a column header row to the output file.
        /// </summary>
        public void AddHeaderRow()
        {
            foreach (var column in _columns)
            {
                AddTextColumn(column.ColumnName);
            }
            NewLine();
        }


        /// <summary>
        /// Write a data row to the output file.
        /// </summary>
        public void AddRow(T row)
        {
            foreach (var column in _columns)
            {
                AddTextColumn(column.Converter(column.PropertyInfo.GetValue(row)));
            }
            NewLine();
        }        

        /// <summary>
        /// Add a text column to the current data line.
        /// </summary>
        public void AddTextColumn(string text)
        {
            if (_isRowDirty)
            {
                _line.Append(_delimiter);
            }

            _isRowDirty = true;

            if (text == null)
            {
                return;
            }

            _line.Append('"');
            if (text != null)
            {
                _line.Append(text.Replace("\"", "\"\"")); // Double any double quote chars.
            }

            _line.Append('"');
        }
        
        /// <summary>
        /// Write the current data line to the output file and start a new line.
        /// </summary>
        public void NewLine()
        {
            if (_isOpen)
            {
                _csvFile.WriteLine(_line.ToString());
                _line.Clear();
                _lineNumber++;
            }
            _isRowDirty = false;
        }

        /// <summary>
        /// Flush buffered data to the output file.
        /// </summary>
        public void Flush()
        {
            _csvFile.Flush();
        }


        /// <summary>
        /// Close the output file.
        /// </summary>
        public void Close()
        {
            if (_isOpen)
            {
                _csvFile.Close();
                _isOpen = false;
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            // Build columns for the model class.
            _columns = new List<Column>();

            // Get all writable properties
            foreach (var prop in _objectType.GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                // Ignore properties that have the NotMapped attribute
                if (prop.CustomAttributes.Any(p => p.AttributeType == typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute)))
                {
                    continue;
                }

                // Create a converter based on the property's data type.
                Func<object, string> converter = null;
                Type propType = prop.PropertyType;

                if (propType == typeof(string))
                {
                    converter = p => p?.ToString() ?? string.Empty;
                }
                else if (propType == typeof(DateTime))
                {
                    converter = p => DateTimeToString((DateTime)p);
                }
                else if (propType == typeof(DateTime?))
                {
                    converter = p => DateTimeNullableToString((DateTime?)p);
                }
                else if (propType == typeof(Guid))
                {
                    converter = p => GuidToString((Guid)p);
                }
                else if (propType == typeof(Guid?))
                {
                    converter = p => GuidNullableToString((Guid?)p);
                }
                else if (propType.IsEnum)
                {
                    converter = p => ((int)p).ToString();
                }
                else if (propType == typeof(bool))
                {
                    converter = p => BoolToString((bool)p);
                }
                else if (propType == typeof(bool?))
                {
                    converter = p => BoolNullableToString((bool?)p);
                }
                else if (propType == typeof(decimal))
                {
                    converter = p => DecimalToString((decimal)p);
                }
                else if (propType == typeof(decimal?))
                {
                    converter = p => DecimalNullableToString((decimal)p);
                }
                else if (propType.IsPrimitive)
                {
                    converter = p => p.ToString();
                }
                else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    converter = p => p?.ToString();
                }

                // If there's a valid conversion, create a column definition.
                if (converter != null)
                {
                    Column col = new Column() { ColumnName = prop.Name, PropertyInfo = prop, Converter = converter };
                    _columns.Add(col);
                }
            }
        }

        private string DateTimeToString(DateTime dt)
        {            
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string DateTimeNullableToString(DateTime? dt)
        {
            return dt.HasValue ? DateTimeToString(dt.Value) : null;
        }

        private string GuidToString(Guid guid)
        {
            return guid.ToString("b");
        }

        private string GuidNullableToString(Guid? guid)
        {
            return guid.HasValue ? guid.Value.ToString("b") : null;
        }

        private string BoolToString(bool b)
        {
            return b ? "1" : "0";
        }

        private string BoolNullableToString(bool? b)
        {
            return b.HasValue ?
                    (b.Value ? "1" : "0")
                    :
                    null;
        }

        private string DecimalToString(decimal d)
        {
            return d.ToString("G");
        }
        
        private string DecimalNullableToString(decimal? d)
        {
            return (d.HasValue) ?
                (d.Value.ToString("G"))
                :
                null;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
   
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _csvFile?.Close();
                    _csvFile?.Dispose();
                }

                _csvFile = null;
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

        #region Internal Classes

        private class Column
        {
            public string ColumnName { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
            public Func<object,string> Converter { get; set; }

            public override string ToString() => ColumnName;
        }

        #endregion

    }
}
