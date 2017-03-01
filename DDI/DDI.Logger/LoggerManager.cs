using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;

namespace DDI.Logger
{
    public static class LoggerManager
    {
        public static ILogger GetLogger(string name)
        {
            return new Log4NetLogger(name);
        }
        public static ILogger GetLogger(Type type)
        {
            return new Log4NetLogger(type);
        }
        public static void LoadAndWatchConfiguration(FileInfo fileInfo)
        {
            XmlConfigurator.ConfigureAndWatch(fileInfo);
        }        
        public static void LoadConsoleConfiguration()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "[%thread] %date %-5level - %message%newline";
            patternLayout.ActivateOptions();

            var appender = new TraceAppender();
            appender.Layout = patternLayout;
            appender.ActivateOptions();

            hierarchy.Root.AddAppender(appender);
            hierarchy.Root.Level = Level.Verbose;
            hierarchy.Configured = true;
        }
    }
}
