using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.LoggingFramework.Models
{
    //public class LoggerContext : DbContext
    //{
    //    public LoggerContext(DbContextOptions<LoggerContext> options) : base(options)
    //    {
    //    }

    //    public DbSet<Logs> Logs { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Logs>(entity =>
    //        {
    //            entity.HasKey(x => x.Id);
    //            entity.Property(x => x.Message);
    //            entity.Property(x => x.MessageTemplate);
    //            entity.Property(e => e.Level).HasMaxLength(128);
    //            entity.Property(x => x.TimeStamp).HasColumnType("datetime").IsRequired(true);
    //            entity.Property(x => x.Exception);
    //            entity.Property(x => x.Properties);
    //        });

    //    }

    //}
}
