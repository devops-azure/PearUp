using PearUp.CommonEntities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PearUp.BusinessEntity
{
    public class UserPhoneNumber : ValueObject
    {
        public const string Phonenumber_Should_Not_Be_Null_Or_Empty = "Phonenumber should not be null or empty";
        public const string Country_Code_Should_Not_Be_Null_Or_Empty = "Country code should not be null or empty";
        public const string Phonenumber_Should_Not_Exceed_Ten_Characters = "Phonenumber should not exceed 10 characters";
        public const string Phone_Contains_Special_Char = "Phone number should contain only numbers.";
        public const string Country_Contains_Special_Char = "Country code should contain only numbers.";

        public string PhoneNumber { get; private set; }
        public string CountryCode { get; private set; }

        private UserPhoneNumber()
        {

        }

        private UserPhoneNumber(string phoneNumber, string countryCode)
        {
            this.PhoneNumber = phoneNumber.Replace("+", "");
            this.CountryCode = countryCode.Replace("+", "");
        }

        public static Result<UserPhoneNumber> Create(string phoneNumber, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result.Fail<UserPhoneNumber>(Phonenumber_Should_Not_Be_Null_Or_Empty);
            if (string.IsNullOrWhiteSpace(countryCode))
                return Result.Fail<UserPhoneNumber>(Country_Code_Should_Not_Be_Null_Or_Empty);
            if (phoneNumber.Length > 10)
                return Result.Fail<UserPhoneNumber>(Phonenumber_Should_Not_Exceed_Ten_Characters);
            if (!Regex.IsMatch(phoneNumber, "^[0-9/s]*$"))
                return Result.Fail<UserPhoneNumber>(Phone_Contains_Special_Char);
            if (!Regex.IsMatch(countryCode, "^[0-9/s]*$"))
                return Result.Fail<UserPhoneNumber>(Country_Contains_Special_Char);

            return Result.Ok<UserPhoneNumber>(new UserPhoneNumber(phoneNumber, countryCode));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PhoneNumber;
            yield return CountryCode;
        }
    }
}