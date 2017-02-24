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

        public void Log(LogLevel logLevel, string message)
        {
            if (!ShouldLog(logLevel)) return;
            switch (logLevel)
            {
                case LogLevel.Critical:
                    Log4NetLog.Fatal(message);
                    break;
                case LogLevel.Error:
                    Log4NetLog.Error(message);
                    break;
                case LogLevel.Warning:
                    Log4NetLog.Warn(message);
                    break;
                case LogLevel.Information:
                    Log4NetLog.Info(message);
                    break;
                case LogLevel.Verbose:
                    Log4NetLog.Debug(message);
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
    }
}
