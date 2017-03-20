using log4net;
using System;

namespace DDI.Logger
{
    public class Log4NetLogger : ILogger
    {
        protected ILog Log4NetLog { get; set; }

        public Log4NetLogger(string name)
        {
            Log4NetLog = LogManager.GetLogger(name);
        }

        public Log4NetLogger(Type type)
        {
            Log4NetLog = LogManager.GetLogger(type);
        }

        public void Log(LogLevel logLevel, string message, Exception exception = null)
        {
            if (!ShouldLog(logLevel)) return;
            switch (logLevel)
            {
                case LogLevel.Critical:
                    if (exception == null)
                        Log4NetLog.Fatal(message);
                    else
                        Log4NetLog.Fatal(message, exception);
                    break;
                case LogLevel.Error:                    
                    if (exception == null)
                        Log4NetLog.Error(message);
                    else
                        Log4NetLog.Error(message, exception);
                    break;
                case LogLevel.Warning:
                    
                    if (exception == null)
                        Log4NetLog.Warn(message);
                    else
                        Log4NetLog.Warn(message, exception);
                    break;
                case LogLevel.Information:

                    if (exception == null)
                        Log4NetLog.Info(message);
                    else
                        Log4NetLog.Info(message, exception);
                    break;
                case LogLevel.Verbose:
                    
                    if (exception == null)
                        Log4NetLog.Debug(message);
                    else
                        Log4NetLog.Debug(message, exception);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            Log(logLevel, () => string.Format(format, args));
        }
        public void LogCritical(string message)
        {
            Log(LogLevel.Critical, message);
        }
        public void LogError(string message)
        {
            Log(LogLevel.Error, message);
        }
        public void LogWarning(string message)
        {
            Log(LogLevel.Warning, message);
        }
        public void LogInformation(string message)
        {
            Log(LogLevel.Information, message);
        }
        public void LogVerbose(string message)
        {
            Log(LogLevel.Verbose, message);
        }
        public void LogCritical(string format, params object[] args)
        {
            Log(LogLevel.Critical, () => string.Format(format, args));
        }
        public void LogError(string format, params object[] args)
        {
            Log(LogLevel.Error, () => string.Format(format, args));
        }
        public void LogWarning(string format, params object[] args)
        {
            Log(LogLevel.Warning, () => string.Format(format, args));
        }
        public void LogInformation(string format, params object[] args)
        {
            Log(LogLevel.Information, () => string.Format(format, args));
        }
        public void LogVerbose(string format, params object[] args)
        {
            Log(LogLevel.Verbose, () => string.Format(format, args));
        }
        public void Log(LogLevel logLevel, Func<string> messageFunc)
        {
            if (ShouldLog(logLevel))
                Log(logLevel, messageFunc());
        }
        public void LogCritical(Func<string> messageFunc)
        {
            Log(LogLevel.Critical, messageFunc);
        }
        public void LogError(Func<string> messageFunc)
        {
            Log(LogLevel.Error, messageFunc);
        }
        public void LogWarning(Func<string> messageFunc)
        {
            Log(LogLevel.Warning, messageFunc);
        }
        public void LogInformation(Func<string> messageFunc)
        {
            Log(LogLevel.Information, messageFunc);
        }
        public void LogVerbose(Func<string> messageFunc)
        {
            Log(LogLevel.Verbose, messageFunc);
        }
        public bool ShouldLog(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return Log4NetLog.IsFatalEnabled;
                case LogLevel.Error:
                    return Log4NetLog.IsErrorEnabled;
                case LogLevel.Warning:
                    return Log4NetLog.IsWarnEnabled;
                case LogLevel.Information:
                    return Log4NetLog.IsInfoEnabled;
                case LogLevel.Verbose:
                    return Log4NetLog.IsDebugEnabled;
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }

        public void LogCritical(Exception exception)
        {
            Log(LogLevel.Critical, string.Empty, exception);
        }

        public void LogError(Exception exception)
        {
            Log(LogLevel.Error, string.Empty, exception);
        }

        public void LogWarning(Exception exception)
        {
            Log(LogLevel.Warning, string.Empty, exception);
        }

        public void LogInformation(Exception exception)
        {
            Log(LogLevel.Information, string.Empty, exception);
        }

        public void LogVerbose(Exception exception)
        {
            Log(LogLevel.Verbose, string.Empty, exception);
        }
         
    }
}
