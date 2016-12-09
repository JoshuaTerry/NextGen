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
	/// <remarks>Acts as a wrapper to encapsulate the actual logging library being used (log4net).</remarks>
	public static class Logger
	{
		#region Public Methods

		public static void Debug(Type callerType, string message, Exception exception = null)
		{
			LogManager.GetLogger(callerType).Debug(message, exception);
		}

		public static void Error(Type callerType, string message, Exception exception = null)
		{
			LogManager.GetLogger(callerType).Error(message, exception);
		}

		public static void Fatal(Type callerType, string message, Exception exception = null)
		{
			LogManager.GetLogger(callerType).Fatal(message, exception);
		}

		public static void Info(Type callerType, string message, Exception exception = null)
		{
			LogManager.GetLogger(callerType).Info(message, exception);
		}

		public static void Warn(Type callerType, string message, Exception exception = null)
		{
			LogManager.GetLogger(callerType).Warn(message, exception);
		}

		#endregion Public Methods
	}
}
