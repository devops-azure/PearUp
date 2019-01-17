using PearUp.CommonEntities;
using PearUp.Constants;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PearUp.BusinessEntity
{
    public class Email:ValueObject
    {
        public const string Email_Should_Not_Be_Empty = "Email should not be empty.";
        public const string Email_Should_Be_Valid_Format = "Email should be valid format.";
        public string EmailId { get; private set; }

        private Email()
        {

        }

        private Email(string email)
        {
            this.EmailId = email;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Fail<Email>(Email_Should_Not_Be_Empty);

            if (!Regex.IsMatch(email, Common.EmailRegex, RegexOptions.IgnoreCase))
                return Result.Fail<Email>(Email_Should_Be_Valid_Format);
            return Result.Ok(new Email(email));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailId;
        }
    }
}
