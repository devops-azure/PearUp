using Serilog.Events;

namespace PearUp.LoggingFramework
{
    public class SerilogConfiguration
    {
        public LogEventLevel MinimumLevel { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
    }
}
