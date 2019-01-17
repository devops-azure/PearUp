using FluentAssertions;
using NUnit.Framework;
using PearUp.BusinessEntity;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    public class EmailTests
    {
        [TestCase("Admin@pearup.com")]
        [TestCase("email@123.123.123.123")]
        [TestCase("email@[123.123.123.123]")]
        [TestCase("\"email\"@domain.com")]
        [TestCase("1234567890@domain.com")]
        [TestCase("email@111.222.333.44444")]
        [TestCase("email@domain.co.jp")]
        public void Create_Should_Return_Email_Object_When_Input_Email_Is_Valid(string emailId)
        {
            var email = Email.Create(emailId);
            email.IsSuccessed.Should().BeTrue();
            email.Value.EmailId.Should().BeEquivalentTo(emailId);
        }
                
        [TestCase("")]
        [TestCase(null)]
        public void Create_Should_Return_Error_Result_When_Input_Email_Is_Empty(string emailId)
        {
            var email = Email.Create(emailId);
            email.IsSuccessed.Should().BeFalse();
            email.GetErrorString().Should().Be(Email.Email_Should_Not_Be_Empty);
        }

        [TestCase("admin@")]
        [TestCase("admin@admin")]
        [TestCase("admin@@admin.com")]
        [TestCase("#@%^%#$@#$@#.com")]
        [TestCase("email.domain.com")]
        [TestCase("email@domain..com")]
        [TestCase("Joe Smith <email@domain.com>")]
        [TestCase("あいうえお@domain.com")]
        public void Create_Should_Return_Error_Result_When_Input_Email_Is_Invalid_Format(string emailId)
        {
            var email = Email.Create(emailId);
            email.IsSuccessed.Should().BeFalse();
            email.GetErrorString().Should().Be(Email.Email_Should_Be_Valid_Format);
        }
    }
}
