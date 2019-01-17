using Microsoft.EntityFrameworkCore;
using PearUp.LoggingFramework.Models;

namespace PearUp.LoggingFramework
{
    public interface ILoggerContext
    {
        DbSet<Log> Logs { get; set; }
    }
}
