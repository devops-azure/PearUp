using Microsoft.EntityFrameworkCore;
using PearUp.Authentication;
using PearUp.Factories.Interfaces;
using PearUp.LoggingFramework.Models;
using PearUp.Repository.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PearUp.Repository
{
    public static class DbContextSeedHelper
    {
        public static void Initialize(PearUpContext context, IHashingService hashingService, IUserFactory userFactory)
        {
            context.Database.Migrate();

            if (!context.Admins.Any())
            {
                var admins = SeedDataHelper.GetAdmins(hashingService);
                context.Admins.AddRange(admins);
                context.SaveChanges();
            }

            if (!context.Interests.Any())
            {
                var interests = SeedDataHelper.GetInterests();
                context.Interests.AddRange(interests);
                context.SaveChanges();
            }
        }
    }
}
