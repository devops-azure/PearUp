using PearUp.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.BusinessEntity
{
    public class UserMatchPreference : ValueObject
    {
        public const string Gender_Preference_Is_Required = "Gender preference is required";
        public const string Minimum_Age_Preference_Should_Be_Greater_Than__Or_Equal_To_18 = "Minimum age preference should be greater than  or equal to 18";
        public const string Maximum_age_Preference_Should_Not_Cross_100 = "Maximum age preference should not cross 100";
        public const string Maximum_age_Preference_Should_Be_Greater_Than_Or_Equal_To_Minimum_Age_Preference = "Maximum age preference should be greater than or equal to minimum age preference";
        public const string Distance_Should_Be_Less_Than_Or_Equal_To_100 = "Distance should be less than or equal to 100";
        public const string Distance_Should_Be_Greater_Than_0 = "Distance should be greater than 0";

        private const int Min_Age_Allowed = 18;
        private const int Max_Age_Allowed = 100;
        private const int Max_Distance_Allowed = 100;
        
        public Gender LookingFor { get; private set; }
        public int MinAge { get; private set; }
        public int MaxAge { get; private set; }
        public int Distance { get; private set; }

        private UserMatchPreference()
        {

        }

        private UserMatchPreference(Gender lookingFor, int minAge, int maxAge, int distance)
        {
            this.LookingFor = lookingFor;
            this.MinAge = minAge;
            this.MaxAge = maxAge;
            this.Distance = distance;
        }

        public static Result<UserMatchPreference> Create(Gender lookingFor, int minAge, int maxAge, int distance)
        {
            if (lookingFor == null)
                return Result.Fail<UserMatchPreference>(Gender_Preference_Is_Required);
            if (minAge < Min_Age_Allowed)
                return Result.Fail<UserMatchPreference>(Minimum_Age_Preference_Should_Be_Greater_Than__Or_Equal_To_18);
            if (maxAge > Max_Age_Allowed)
                return Result.Fail<UserMatchPreference>(Maximum_age_Preference_Should_Not_Cross_100);
            if (maxAge < minAge)
                return Result.Fail<UserMatchPreference>(Maximum_age_Preference_Should_Be_Greater_Than_Or_Equal_To_Minimum_Age_Preference);
            if (distance <= 0)
                return Result.Fail<UserMatchPreference>(Distance_Should_Be_Greater_Than_0);
            if (distance > Max_Distance_Allowed)
                return Result.Fail<UserMatchPreference>(Distance_Should_Be_Less_Than_Or_Equal_To_100);
            return Result.Ok(new UserMatchPreference(lookingFor, minAge, maxAge, distance));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LookingFor.GenderType;
            yield return MinAge;
            yield return MaxAge;
            yield return Distance;
        }
    }
}
