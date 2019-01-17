using NUnit.Framework;
using PearUp.BusinessEntity;
using FluentAssertions;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class UserPhoneNumberTests
    {
        [TestCase(null, null)]
        [TestCase(null, "91")]
        [TestCase("", "")]
        [TestCase("", "91")]
        public void Create_Should_Return_Fail_Result_For_Invalid_phoneNumber(string phoneNumber, string countryCode)
        {
            var userPhoneNumberResult = UserPhoneNumber.Create(phoneNumber, countryCode);
            userPhoneNumberResult.IsSuccessed.Should().BeFalse();
            userPhoneNumberResult.GetErrorString().Should().Be(UserPhoneNumber.Phonenumber_Should_Not_Be_Null_Or_Empty);
        }
        [TestCase("9999999999", null)]
        [TestCase("9999999999", "")]
        public void Create_Should_Return_Fail_Result_For_Invalid_countryCode(string phoneNumber, string countryCode)
        {
            var userPhoneNumberResult = UserPhoneNumber.Create(phoneNumber, countryCode);
            userPhoneNumberResult.IsSuccessed.Should().BeFalse();
            userPhoneNumberResult.GetErrorString().Should().Be(UserPhoneNumber.Country_Code_Should_Not_Be_Null_Or_Empty);
        }

        [TestCase("99999999999", "91")]
        public void Create_Should_Return_Fail_Result_For_PhoneNumber_Longer_Than_Ten_Digits(string phoneNumber, string countryCode)
        {
            var userPhoneNumberResult = UserPhoneNumber.Create(phoneNumber, countryCode);
            userPhoneNumberResult.IsSuccessed.Should().BeFalse();
            userPhoneNumberResult.GetErrorString().Should().Be(UserPhoneNumber.Phonenumber_Should_Not_Exceed_Ten_Characters);
        }

        [TestCase("test", "91")]
        public void Create_Should_Return_Fail_Result_For_Invalid_Characters_In_PhoneNumber(string phoneNumber, string countryCode)
        {
            var userPhoneNumberResult = UserPhoneNumber.Create(phoneNumber, countryCode);
            userPhoneNumberResult.IsSuccessed.Should().BeFalse();
            userPhoneNumberResult.GetErrorString().Should().Be(UserPhoneNumber.Phone_Contains_Special_Char);
        }

        [TestCase("9999999999", "test")]
        public void Create_Should_Return_Fail_Result_For_Invalid_Characters_In_CountryCode(string phoneNumber, string countryCode)
        {
            var userPhoneNumberResult = UserPhoneNumber.Create(phoneNumber, countryCode);
            userPhoneNumberResult.IsSuccessed.Should().BeFalse();
            userPhoneNumberResult.GetErrorString().Should().Be(UserPhoneNumber.Country_Contains_Special_Char);
        }

        [TestCase("9999999999", "91")]
        public void Create_Should_Return_Success_Result_For_Valid_phoneNumber_And_countryCode(string phoneNumber, string countryCode)
        {
            var userPhoneNumberResult = UserPhoneNumber.Create(phoneNumber, countryCode);
            userPhoneNumberResult.IsSuccessed.Should().BeTrue();
        }
    }
}
