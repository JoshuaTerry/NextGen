using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DDI.Conversion
{
	/// <summary>
	/// Class for importing CSV files
	/// </summary>
	public class FileImport : IDisposable
	{
		#region Private fields

		private Microsoft.VisualBasic.FileIO.TextFieldParser _csvReader = null;
		private string[] _data = null;
		private bool _eof = true;
		private bool _isABLData = false;   // true if delimiter is space, implying that data came from OpenEdge ABL EXPORT statement.
		private ILog _logger;

		#endregion


		#region Constructors

		/// <summary>
		/// FileImport - Initialize a FileImport object for an input file, specifying a delimiter
		/// </summary>
		/// <param name="filename">Filename of import file</param>
		/// <param name="delimiter">A delimiter character</param>
		public FileImport(string filename, string logName, char delimiter = ',')
		{
			_csvReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(filename, Encoding.GetEncoding(1252));
			_csvReader.Delimiters = new String[1] { delimiter.ToString() };
			_csvReader.HasFieldsEnclosedInQuotes = true;
			_csvReader.CommentTokens = new string[] { "#" };

			CurrentLineNumber = 0;
			_eof = false;
			_isABLData = (delimiter == ' ');

			if (string.IsNullOrWhiteSpace(logName))
				logName = "FileImport";
			_logger = LogManager.GetLogger(logName);
		}

		#endregion

		#region Public Properties
		/// <summary>
		/// Line number of current row.
		/// </summary>
		public int CurrentLineNumber { get; private set; }

		/// <summary>
		/// Text representation of current row.
		/// </summary>
		public string CurrentLineText
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				bool isFirst = true;
				int len = 0;
				foreach (var field in _data)
				{
					if (!isFirst)
					{
						sb.Append(",");
						len++;
					}
					if (len + 2 + field.Length > 60)
					{
						sb.Append("\r\n");
						len = 0;
					}
					sb.Append('"').Append(field.Replace('\n', ' ').Replace('\r', ' ')).Append('"');
					len += 2 + field.Length;
					isFirst = false;
				}
				return sb.ToString();
			}
		}

		/// <summary>
		/// Length - Return number of entries in current row
		/// </summary>
		public int Length
		{
			get
			{
				if (_data == null)
				{
					return 0;
				}

				return _data.Length;
			}
		}


		/// <summary>
		/// Indexer - provides 
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		public string this[int idx]
		{
			get
			{
				return GetString(idx);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Truncate a string to a specified number of characters, logging an error if truncation occurs.
		/// </summary>
		private string Truncate(string text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			if (text.Length > maxLength)
			{
				LogError(string.Format("Field truncated to {0} characters.", maxLength));

				text = text.Substring(0, maxLength);
			}

			return text;

		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing || _csvReader != null)
			{
				_csvReader.Close();
				_csvReader = null;
				_eof = true;
			}
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Log an informational message 
		/// </summary>
		/// <param name="text">Message</param>
		/// <param name="separator">True to include a separator</param>
		public void LogMessage(string text, bool separator = false)
		{
			if (separator)
			{
				_logger.Info(new string('=', text.Length));
			}
			_logger.Info(text);            
			
		}

		/// <summary>
		/// Log an error message.
		/// </summary>
		/// <param name="text">Message</param>
		/// <param name="includeContext">True to include current line number and current line data.</param>
		public void LogError(string text, bool includeContext = true)
		{
			_logger.Error(text);

			if (includeContext)
			{
				text = string.Format("** Import line #{0}: ", CurrentLineNumber);
				_logger.Error(text);

				int indent = text.Length;
				text = CurrentLineText.Replace('\n', ' ').Replace("\r", string.Empty);
				_logger.Error(text);
			}
		}

		/// <summary>
		/// Dispose resources
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}


		/// <summary>
		/// Import - Get next row in input file, return TRUE if row was read, FALSE otherwise.
		/// </summary>
		public bool GetNextRow()
		{
			// Don't even attempt to read past logical EOF
			if (_eof)
			{
				return false;
			}

			// Get next set of data
			_data = _csvReader.ReadFields();

			// Physical EOF or logical EOF (line with '.' and nothing else)
			if (_data == null || (_isABLData && _data.Length == 1 && _data[0] == "."))
			{
				_eof = true;
				return false;
			}
			CurrentLineNumber++;
			return true;
		}

		// Methods to retrieve specific entries from current row:
		//   GetString (index)
		//   GetDateTime (index)
		//   GetInt (index)
		//   GetDecimal (index)
		//   GetBool (index)

		/// <summary>
		/// Get a string value from the current import row at specified index (zero-based)
		/// </summary>
		public string GetString(int fieldNum)
        {
            string str = (_data == null || _data.Length <= fieldNum ? null : _data[fieldNum]);
			if (str == null || str == "?")
			{
				return null;
			}
            return str.TrimEnd(' ');
        }

		/// <summary>
		/// Get a string value from the current import row at specified index (zero-based)
		/// </summary>
		public string GetString(int fieldNum, int maxLength)
		{
			return Truncate(GetString(fieldNum), maxLength);
		}

        /// <summary>
        /// Get a string value from the current import row at specified index (zero-based).  
        /// Replace line breaks with \n.
        /// </summary>
        public string GetMultilineString(int fieldNum)
        {
            return GetString(fieldNum).Replace("\r\n", "\n");
        }

		/// <summary>
		/// Get a string value from the current import row at specified index (zero-based).  
		/// Replace line breaks with \n.
		/// </summary>
		public string GetMultilineString(int fieldNum, int maxLength)
		{
			return Truncate(GetMultilineString(fieldNum), maxLength);
		}

        /// <summary>
        /// Get a nullable DateTime value from the current import row at specified index (zero-based)
        /// </summary>
        public DateTime? GetDateTime(int fieldNum)
        {
            string dtstr = (_data == null || _data.Length <= fieldNum ? null : _data[fieldNum]);

			if (dtstr == null || dtstr.Length == 0 || dtstr[0] == '?')
			{
				return null;
			}

			try
            {
                DateTime dt = Convert.ToDateTime(dtstr);
                return dt;
            }
            catch
            {
                return null;

            }
        }

        /// <summary>
        /// Get a nullable int value from the current import row at specified index (zero-based)
        /// </summary>
        public int? GetIntNullable(int fieldNum)
        {
            string numstr = (_data == null || _data.Length <= fieldNum ? null : _data[fieldNum]);

			if (numstr == null)
			{
				return null;
			}

			if (numstr.Length == 0)
			{
				return 0;
			}

			if (numstr[0] == '?')
			{
				return null;
			}

			try
            {
                int i = Convert.ToInt32(numstr);
                return i;
            }
            catch
            {
                return null;
            }
        }

		/// <summary>
		/// Get an int value from the current import row at specified index (zero-based)
		/// </summary>
		public int GetInt(int fieldNum)
		{
			return GetIntNullable(fieldNum) ?? 0;
        }

        /// <summary>
        /// Get a nullable Int64 value from the current import row at specified index (zero-based)
        /// </summary>
        public Int64? GetInt64Nullable(int fieldNum)
        {
            string numstr = (_data == null || _data.Length <= fieldNum ? null : _data[fieldNum]);

			if (numstr == null)
			{
				return null;
			}

			if (numstr.Length == 0)
			{
				return 0;
			}

			if (numstr[0] == '?')
			{
				return null;
			}

			try
            {
                Int64 i = Convert.ToInt64(numstr);
                return i;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get an Int64 value from the current import row at specified index (zero-based)
        /// </summary>
        public Int64 GetInt64(int fieldNum)
        {
			return GetInt64Nullable(fieldNum) ?? 0;
        }

		/// <summary>
		/// Get a byte value from import file.
		/// </summary>
		public byte GetByte(int fieldNum)
		{
			int val = GetInt(fieldNum);
			checked
			{
				try
				{
					return (byte)val;
				}
				catch
				{
					LogError(string.Format("Unable to convert value {0} to an 8-bit integer.", val));
				}
			}
			return 0;
		}

		/// <summary>
		/// Get a short (16-bit) value from import file.
		/// </summary>
		public short GetShort(int fieldNum)
		{
			int val = GetInt(fieldNum);
			checked
			{
				try
				{
					return (short)val;
				}
				catch
				{
					LogError(string.Format("Unable to convert value {0} to a 16-bit integer.", val));
				}
			}
			return 0;
		}


		/// <summary>
		/// Get a nullable decimal value from the current import row at specified index (zero-based)
		/// </summary>
		public decimal? GetDecimalNullable(int idx)
        {
            string numstr = (_data == null || _data.Length <= idx ? null : _data[idx]);

			if (numstr == null)
			{
				return null;
			}

			if (numstr.Length == 0)
			{
				return 0;
			}

			if (numstr[0] == '?')
			{
				return null;
			}

			try
			{
                decimal n = Convert.ToDecimal(numstr);
                return n;
            }
            catch
            {
                return null;
            }
        }


		/// <summary>
		/// Get a nullable decimal value from the current import row at specified index (zero-based)
		/// </summary>
		public decimal GetDecimal(int idx)
		{
			return GetDecimalNullable(idx) ?? 0m;
		}

        /// <summary>
        /// Get a nullable boolean value from the current import row at specified index (zero-based)
        /// </summary>
        public bool? GetBoolNullable(int idx)
        {
            string boolstr = (_data == null || _data.Length <= idx ? null : _data[idx]);

			if (boolstr == null)
			{
				return null;
			}

			if (boolstr.Length == 0)
			{
				return false;
			}

			if (boolstr[0] == '?')
			{
				return null;
			}

			boolstr = boolstr.ToLower();
            return boolstr == "yes" || boolstr == "true";
        }

        /// <summary>
        /// Get a boolean value from the current import row at specified index (zero-based)
        /// </summary>
        public bool GetBool(int idx)
        {
			return GetBoolNullable(idx) ?? false;
        }


		/// <summary>
		/// Get an Enum value from the import file.
		/// </summary>
		public T GetEnum<T>(int fieldNum) where T : struct, IConvertible
		{
			if (!(typeof(T).IsEnum))
			{
				throw new ArgumentException("T must be an enumerated type.");
			}

			string val = GetString(fieldNum);
			T enumVal = default(T);

			try
			{
				enumVal = (T)Enum.Parse(typeof(T), val);
			}
			catch
			{
				LogError(string.Format("Cannot convert \"{0}\" to enum type {1}.", val, typeof(T).Name));
			}

			return enumVal;
		}

		#endregion
	}


}
