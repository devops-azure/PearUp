using PearUp.ILoggingFramework;
using Serilog;
using Serilog.Events;
using System;

namespace PearUp.LoggingFramework
{
    public class PearUpLogger : IPearUpLogger
    {
        private ILogger _logger;

        public PearUpLogger()
        {
            _logger = Serilog.Log.ForContext<PearUpLogger>();
        }

        public void LogError(Exception ex, string message = "")
        {
            _logger.ForContext("Exception", ex.GetType())
                    .ForContext("StackTrace", ex.StackTrace, true)
                    .Error(ex, message, null);
        }

        public void LogFatal(Exception ex)
        {
            _logger.ForContext("Exception", ex.GetType())
                    .ForContext("StackTrace", ex.StackTrace, true)
                    .Fatal(ex, null);
        }

        public void LogInfo(string message)
        {
            _logger.Information(message, null);
        }

        public void LogWarning(Exception ex, string message = "")
        {
            _logger.Warning(ex, message);
        }

        public void LogWrite(LogLevel logLevel, string message = "")
        {
            var level = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), logLevel.ToString());
            _logger.Write(level, message);
        }
    }
}
