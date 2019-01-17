using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PearUp.Business;
using PearUp.CommonEntity;
using PearUp.IBusiness;
using PearUp.ILoggingFramework;
using PearUp.IRepository;
using PearUp.LoggingFramework;
using PearUp.MongoRepository;
using PearUp.Repository;
using Serilog.Events;
using System;

namespace PearUp.Azure
{
    internal static class ServiceResolver
    {
        private static IServiceProvider _serviceProvider;
        private static readonly object padlock = new object();
        private static IServiceProvider serviceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    lock (padlock)
                    {
                        if (_serviceProvider == null)
                        {
                            _serviceProvider = GetServiceProvider();
                        }
                    }
                }
                return _serviceProvider;
            }
        }
        private static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = Register();
            return serviceCollection.BuildServiceProvider();
        }

        private static IServiceCollection Register()
        {
            var services = new ServiceCollection();
            services.AddScoped<IUserPhotoService, UserPhotoService>();
            services.AddMediatR();
            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMongoUserRepository, MongoUserRepository>();
            services.AddScoped<IInterestRepository, InterestRepository>();
            //End
            services.GetDbContext();
            services.GetMangoDbSettings();
            services.GetSerilog();

            return services;
        }

        public static T CreateInstance<T>()
        {
            return serviceProvider.GetRequiredService<T>();
        }

        private static void GetDbContext(this IServiceCollection services)
        {
            services.AddDbContext<PearUpContext>(options => options.UseSqlServer(GetEnvironmentVariable("SQLConnectionString")));
        }

        private static void GetMangoDbSettings(this IServiceCollection services)
        {
            var options = Options.Create<MongoSettings>(new MongoSettings
            {
                ConnectionString = GetEnvironmentVariable("MongoConnectionString"),
                DataBaseName = GetEnvironmentVariable("MongoDataBaseName"),
            });
            services.AddSingleton(typeof(PearUpMongoContext), new PearUpMongoContext(options));
        }
        private static void GetSerilog(this IServiceCollection services)
        {

            Enum.TryParse<LogEventLevel>(GetEnvironmentVariable("Serilog_MinimumLevel"), out LogEventLevel logEventLevel);
            var serilogConfiguration = new SerilogConfiguration
            {
                ConnectionString = GetEnvironmentVariable("SQLConnectionString"),
                MinimumLevel = logEventLevel,
                TableName = GetEnvironmentVariable("Serilog_TableName")
            };
            services.AddSingleton(typeof(IPearUpLogger), new PearupSqlLogger(serilogConfiguration));
        }
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
