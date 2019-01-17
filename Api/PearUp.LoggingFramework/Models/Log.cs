using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.LoggingFramework.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
    }
}
