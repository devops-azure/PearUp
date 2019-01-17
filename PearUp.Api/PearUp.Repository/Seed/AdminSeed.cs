using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.BusinessEntity.Builders;
using System.Collections.Generic;

namespace PearUp.Repository.Seed
{
    internal static partial class SeedDataHelper
    {
        public static List<Admin> GetAdmins(IHashingService hashingService)
        {
            var salt = hashingService.GenerateSalt();
            var password = "Pearup1234$";
            var hashedPassword = hashingService.GetHash(password, salt);
            var admins = new List<Admin>
                {
                     AdminBuilder.Builder()
                     .WithEmail(Email.Create("admin@pearup.com").Value)
                     .WithName("Admin Pearup")
                     .WithPassword(Password.Create(hashedPassword,salt).Value)
                     .Build().Value,

                     AdminBuilder.Builder()
                     .WithEmail(Email.Create("admin2@pearup.com").Value)
                     .WithName("Admin2 Pearup")
                     .WithPassword(Password.Create(hashedPassword,salt).Value)
                     .Build().Value,
                };
            
            return admins;
        }

    }
}
