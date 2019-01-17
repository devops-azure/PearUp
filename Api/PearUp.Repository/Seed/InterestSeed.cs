using PearUp.BusinessEntity;
using System.Collections.Generic;

namespace PearUp.Repository.Seed
{
    internal static partial class SeedDataHelper
    {
        public static List<Interest> GetInterests()
        {
            var interests = new List<Interest>();

            for (int i = 1; i <= 10; i++)
            {
                var interestResult = Interest.Create("Interest" + i, "Interest Description " + i);
                interests.Add(interestResult.Value);
            }
            return interests;
        }
    }
}
