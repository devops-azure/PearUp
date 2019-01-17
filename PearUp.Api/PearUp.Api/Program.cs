using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PearUp.Repository;
using PearUp.Authentication;
using PearUp.LoggingFramework.Models;
using PearUp.Factories.Interfaces;

namespace PearUp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PearUpContext>();
                    var hashingServie = services.GetRequiredService<IHashingService>();
                    var userFactory = services.GetRequiredService<IUserFactory>();
                    DbContextSeedHelper.Initialize(context, hashingServie, userFactory);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
