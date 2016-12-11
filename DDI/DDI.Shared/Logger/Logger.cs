using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace DDI.Shared.Logger
{
	/// <summary>
	/// Manages log message creation.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Acts as a wrapper to encapsulate the actual logging library being used (log4net). A Logger
	/// instance is created for each class from which a log method is called. This class also
	/// provides static methods for access to those class-specific Logger objects.
	/// </para>
	/// <para>
	/// To use the Logger instance within a class, create a private static field to store it. That
	/// same instance will also be available by using the static logging methods.
	/// <code>
	/// private static readonly Logger logger = Logger.GetLogger(typeof(ThisClass));
	/// </code>
	/// </para>
	/// <para>
	/// Alternatively, use the static methods and provide the Type from which logging is being
	/// performed.
	/// </para>
	/// </remarks>
	public class Logger
	{
		#region Private Fields

		private static Dictionary<Type, Logger> _logs = new Dictionary<Type, Logger>();

		private ILog _log;

		#endregion Private Fields

		#region Private Constructors

		/// <summary>
		/// Gets the appropriate <see cref="ILog"/> instance from the <see cref="LogManager"/> and
		/// exposes its functionality.
		/// </summary>
		/// <param name="callerType"></param>
		/// <returns></returns>
		private Logger(Type callerType)
		{
			_log = LogManager.GetLogger(callerType);
		}

		#endregion Private Constructors

		#region Public Methods

		public static void Debug(Type callerType, object message, Exception exception = null)
		{
			GetLogger(callerType).Debug(message, exception);
		}

		public static void DebugWithFormat(Type callerType, string format, params object[] data)
		{
			GetLogger(callerType).DebugWithFormat(format, data);
		}

		public static void Error(Type callerType, object message, Exception exception = null)
		{
			GetLogger(callerType).Error(message, exception);
		}

		public static void ErrorWithFormat(Type callerType, string format, params object[] data)
		{
			GetLogger(callerType).ErrorWithFormat(format, data);
		}

		public static void Fatal(Type callerType, object message, Exception exception = null)
		{
			GetLogger(callerType).Fatal(message, exception);
		}

		public static void FatalWithFormat(Type callerType, string format, params object[] data)
		{
			GetLogger(callerType).FatalWithFormat(format, data);
		}

		public static Logger GetLogger(Type callerType)
		{
			Logger log;

			if (!(_logs.TryGetValue(callerType, out log)))
			{
				log = new Logger(callerType);
				_logs.Add(callerType, log);
			}

			return log;
		}

		public static void Info(Type callerType, object message, Exception exception = null)
		{
			GetLogger(callerType).Info(message, exception);
		}

		public static void InfoWithFormat(Type callerType, string format, params object[] data)
		{
			GetLogger(callerType).InfoWithFormat(format, data);
		}

		public static void Warn(Type callerType, object message, Exception exception = null)
		{
			GetLogger(callerType).Warn(message, exception);
		}

		public static void WarnWithFormat(Type callerType, string format, params object[] data)
		{
			GetLogger(callerType).WarnWithFormat(format, data);
		}

		public void Debug(object message, Exception exception = null)
		{
			_log.Debug(message, exception);
		}

		public void DebugWithFormat(string format, params object[] data)
		{
			_log.DebugFormat(format, data);
		}

		public void Error(object message, Exception exception = null)
		{
			_log.Error(message, exception);
		}

		public void ErrorWithFormat(string format, params object[] data)
		{
			_log.ErrorFormat(format, data);
		}

		public void Fatal(object message, Exception exception = null)
		{
			_log.Fatal(message, exception);
		}

		public void FatalWithFormat(string format, params object[] data)
		{
			_log.FatalFormat(format, data);
		}

		public void Info(object message, Exception exception = null)
		{
			_log.Info(message, exception);
		}

		public void InfoWithFormat(string format, params object[] data)
		{
			_log.InfoFormat(format, data);
		}

		public void Warn(object message, Exception exception = null)
		{
			_log.Warn(message, exception);
		}

		public void WarnWithFormat(string format, params object[] data)
		{
			_log.WarnFormat(format, data);
		}

		#endregion Public Methods
	}
}
