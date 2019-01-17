using Microsoft.Extensions.DependencyInjection;
using PearUp.ILoggingFramework;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace PearUp.LoggingFramework
{
    public static class PearUpLoggerExtension
    {
        public static void RegisterLogger(this IServiceCollection services, SerilogConfiguration serilogConfiguration)
        {

            var columnOptions = new ColumnOptions
            {
                AdditionalDataColumns = new Collection<DataColumn> {
                new DataColumn { DataType = typeof(string), ColumnName = "StackTrace" } }
            };
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.Properties);

            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(serilogConfiguration.ConnectionString, serilogConfiguration.TableName, columnOptions: columnOptions,
                    restrictedToMinimumLevel: serilogConfiguration.MinimumLevel)
                .CreateLogger();
            services.AddSingleton<IPearUpLogger, PearUpLogger>();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
