using PearUp.ILoggingFramework;
using Serilog.Events;
using System;
using System.Data.SqlClient;

namespace PearUp.LoggingFramework
{
    /*
     * Serilog not supporting in azure function due to below issue.
     * So that creating logs through ADO Dot net.
     * This Logger only Consumed in Azure functions.
    https://github.com/serilog/serilog-sinks-mssqlserver/issues/108
    */
    public class PearupSqlLogger : IPearUpLogger
    {
        private readonly SerilogConfiguration _serilogConfiguration;

        public PearupSqlLogger(SerilogConfiguration serilogConfiguration)
        {
            this._serilogConfiguration = serilogConfiguration;
        }
        public void LogError(Exception ex, string message = "")
        {
            CrateLog(ex.GetType().ToString(), LogEventLevel.Error.ToString(), message, ex.StackTrace);
        }

        public void LogFatal(Exception ex)
        {
            CrateLog(ex.GetType().ToString(), LogEventLevel.Fatal.ToString(), string.Empty, ex.StackTrace);
        }

        public void LogInfo(string message)
        {
            CrateLog(string.Empty, LogEventLevel.Information.ToString(), message, string.Empty);
        }

        public void LogWarning(Exception ex, string message = "")
        {
            CrateLog(ex.GetType().ToString(), LogEventLevel.Warning.ToString(), message, ex.StackTrace);
        }

        public void LogWrite(LogLevel logLevel, string message = "")
        {
            CrateLog(string.Empty, logLevel.ToString(), message, string.Empty);
        }
        private void CrateLog(string exception, string level, string message, string stackTrace)
        {
            string query = $"Insert Into dbo.log (Exception, Level, Message, StackTrace,TimeStamp) " +
                      $"VALUES (@Exception, @Level, @Message, @StackTrace, '{DateTime.Now}')";
            using (SqlConnection cn = new SqlConnection(_serilogConfiguration.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@Exception", System.Data.SqlDbType.NVarChar, 100).Value = exception;
                cmd.Parameters.Add("@Level", System.Data.SqlDbType.NVarChar, 100).Value = level;
                cmd.Parameters.Add("@Message", System.Data.SqlDbType.NVarChar, 100).Value = message;
                cmd.Parameters.Add("@StackTrace", System.Data.SqlDbType.NVarChar, 100).Value = stackTrace;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
