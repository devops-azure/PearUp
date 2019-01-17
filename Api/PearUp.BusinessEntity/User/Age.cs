using PearUp.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.BusinessEntity
{
    public class Age : ValueObject
    {

        public const string Date_Of_Birth_Is_Required = "Date of birth is required.";
        public const string Age_Should_Be_Atleast_18 = "Age should be atleast 18";
        private const int MinimumAge = 18;

        public DateTime DateOfBirth { get; private set; }
        
        public int CurrentAge
        {
            get
            {
                return DateTime.Now.Year - DateOfBirth.Year;
            }
        }

        private Age()
        {

        }

        private Age(DateTime dateOfBirth)
        {
            this.DateOfBirth = dateOfBirth;
        }

        public static Result<Age> Create(DateTime dateOfBirth)
        {
            if (dateOfBirth == default(DateTime))
                return Result.Fail<Age>(Date_Of_Birth_Is_Required);
            if (!IsValidAge(dateOfBirth))
                return Result.Fail<Age>(Age_Should_Be_Atleast_18);
            return Result.Ok(new Age(dateOfBirth));
        }

        private static bool IsValidAge(DateTime dateOfBirth)
        {
            var endRange = DateTime.Today.AddYears(-1 * MinimumAge).AddDays(1);
            var comparedToEnd = DateTime.Compare(dateOfBirth, endRange);
            return (comparedToEnd <= 0);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DateOfBirth;
        }
    }
}
