using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.ILoggingFramework
{
    public interface IPearUpLogger
    {
        void LogError(Exception ex, string message = "");
        void LogInfo(string message);
        void LogWrite(LogLevel logLevel, string message = "");
        void LogWarning(Exception ex, string message = "");
        void LogFatal(Exception ex);
    }

    public enum LogLevel
    {
        Verbose = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Fatal = 5
    }
}
