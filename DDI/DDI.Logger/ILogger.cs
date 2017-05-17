using System;

namespace DDI.Logger
{
    public interface ILogger
    {
        
        void Log(LogLevel logLevel, string format, params object[] args);
        void LogCritical(Exception exception);
        void LogError(Exception exception);
        void LogWarning(Exception exception);
        void LogInformation(Exception exception);
        void LogVerbose(Exception exception);
        
        void LogCritical(string message);
        void LogError(string message);
        void LogWarning(string message);
        void LogInformation(string message);
        void LogVerbose(string message);

        void Log(LogLevel logLevel, Func<string> messageFunc);
        void LogCritical(Func<string> messageFunc);
        void LogError(Func<string> messageFunc);
        void LogWarning(Func<string> messageFunc);
        void LogInformation(Func<string> messageFunc);
        void LogVerbose(Func<string> messageFunc);

        void LogCritical(string format, params object[] args);
        void LogError(string format, params object[] args);
        void LogWarning(string format, params object[] args);
        void LogInformation(string format, params object[] args);
        void LogVerbose(string format, params object[] args);

        bool ShouldLog(LogLevel logLevel);
    }
}
