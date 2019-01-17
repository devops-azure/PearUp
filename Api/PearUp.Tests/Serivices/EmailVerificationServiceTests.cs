using FluentAssertions;
using Moq;
using NUnit.Framework;
using PearUp.Business;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.IBusiness;
using PearUp.Infrastructure;
using PearUp.IRepository;
using PearUp.Tests.Builders;
using PearUp.Utilities;
using System.Threading.Tasks;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    public class EmailVerificationServiceTests
    {
        private const string TEST_NUMBER = "8897322553";
        private const string TEST_NUMBER_OTHER = "8897322500";
        private const string TEST_COUNTRY_CODE = "+91";
        private const int TEST_VERIFICATION_CODE = 9097;
        private const int TEST_VERIFICATION_CODE_OTHER = 9000;

        [TestCase(null, null)]
        [TestCase(TEST_COUNTRY_CODE, null)]
        [TestCase(null, TEST_NUMBER)]
        [TestCase("", "")]
        [TestCase("", TEST_NUMBER)]
        [TestCase(TEST_COUNTRY_CODE, "")]
        public async Task GenerateVerificationCode_Should_Return_Error_If_countryCode_Or_phoneNumber_Is_InvalidAsync(string countryCode, string phoneNumber)
        {
            var emailHelperMock = new Mock<IEmailHelper>();
            var randomNumberProvider = new Mock<IRandomNumberProvider>();
            var verificationCodeDataStoreMock = new Mock<IVerificationCodeDataStore>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var emailVerificationService = new EmailVerificationService(emailHelperMock.Object, randomNumberProvider.Object, verificationCodeDataStoreMock.Object, userRepositoryMock.Object);
            var result = await emailVerificationService.GenerateVerificationCodeAsync(countryCode, phoneNumber);
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.PhoneVerifyMessages.Country_Phone_Empty);
        }

        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER)]
        public async Task GenerateVerificationCode_Should_Return_True_If_countryCode_And_phoneNumber_Are_ValidAsync(string countryCode, string phoneNumber)
        {
            var emailHelperMock = new Mock<IEmailHelper>();
            var randomNumberProvider = new Mock<IRandomNumberProvider>();
            var verificationCodeDataStoreMock = new Mock<IVerificationCodeDataStore>();

            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));

            var emailVerificationService = new EmailVerificationService(emailHelperMock.Object, randomNumberProvider.Object, verificationCodeDataStoreMock.Object, userRepositoryMock.Object);
            var result = await emailVerificationService.GenerateVerificationCodeAsync(countryCode, phoneNumber);
            result.IsSuccessed.Should().BeTrue();
        }

        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER)]
        public async Task SendEmailAsync_Gets_Invoked_If_GenerateVerificationCode_Is_Called_First_Time_With_Valid_PhoneNumber(string countryCode, string phoneNumber)
        {
            var emailHelperMock = new Mock<IEmailHelper>();
            var randomNumberProvider = new Mock<IRandomNumberProvider>();
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));
            var verificationCodeDataStoreMock = new Mock<IVerificationCodeDataStore>();
            verificationCodeDataStoreMock.Setup(x => x.GetValueOrDefault(It.IsAny<string>())).Returns(TEST_VERIFICATION_CODE);
            verificationCodeDataStoreMock.Setup(x => x.ContainsKey(It.IsAny<string>())).Returns(false);
            var emailVerificationService = new EmailVerificationService(emailHelperMock.Object, randomNumberProvider.Object, verificationCodeDataStoreMock.Object, userRepositoryMock.Object);
            var result = await emailVerificationService.GenerateVerificationCodeAsync(countryCode, phoneNumber);
            Assert.IsTrue(result.IsSuccessed);
            emailHelperMock.Verify(x => x.SendAsync(It.IsAny<string>()), Times.Once);
        }

        [TestCase(null, null, null)]
        [TestCase(TEST_COUNTRY_CODE, null, null)]
        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, null)]
        [TestCase(TEST_COUNTRY_CODE, null, TEST_VERIFICATION_CODE)]
        [TestCase(null, TEST_NUMBER, null)]
        [TestCase(null, TEST_NUMBER, TEST_VERIFICATION_CODE)]
        [TestCase(null, null, TEST_VERIFICATION_CODE)]
        [TestCase("", "", TEST_VERIFICATION_CODE)]
        [TestCase("", TEST_NUMBER, TEST_VERIFICATION_CODE)]
        [TestCase(TEST_COUNTRY_CODE, "", TEST_VERIFICATION_CODE)]
        [TestCase("", "", 0000)]
        [TestCase("", TEST_NUMBER, 0000)]
        [TestCase(TEST_COUNTRY_CODE, "", 0000)]
        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, 0000)]
        public async Task ValidateVerificationCode_Should_Return_Error_If_CountryCode_Or_PhoneNumber_Or_VerificationCode_Is_InvalidAsync(string countryCode, string phoneNumber, int verificationCode)
        {
            var emailHelperMock = new Mock<IEmailHelper>();
            var randomNumberProvider = new Mock<IRandomNumberProvider>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var verificationCodeDataStoreMock = new Mock<IVerificationCodeDataStore>();
            var emailVerificationService = new EmailVerificationService(emailHelperMock.Object, randomNumberProvider.Object, verificationCodeDataStoreMock.Object, userRepositoryMock.Object);
            var result = await emailVerificationService.ValidateVerificationCodeAsync(countryCode, phoneNumber, verificationCode);
            Assert.IsFalse(result.IsSuccessed);
            Assert.AreEqual(Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Empty, result.GetErrorString());
        }

        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, TEST_VERIFICATION_CODE)]
        public async Task ValidateVerificationCode_Should_Return_Error_If_VerificationCode_Not_Yet_Generated(string countryCode, string phoneNumber, int verificationCode)
        {
            var emailHelperMock = new Mock<IEmailHelper>();
            var randomNumberProvider = new Mock<IRandomNumberProvider>();
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));
            var verificationCodeDataStoreMock = new Mock<IVerificationCodeDataStore>();
            var emailVerificationService = new EmailVerificationService(emailHelperMock.Object, randomNumberProvider.Object, verificationCodeDataStoreMock.Object, userRepositoryMock.Object);
            var result = await emailVerificationService.ValidateVerificationCodeAsync(countryCode, phoneNumber, verificationCode);
            Assert.IsFalse(result.IsSuccessed);
            Assert.AreEqual(Constants.PhoneVerifyMessages.Error_Message, result.GetErrorString());
        }

        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, TEST_VERIFICATION_CODE)]
        public async Task ValidateVerificationCode_Should_Return_Error_If_VerificationCode_Generated_But_Supplied_Incorrect_VerificationCode(string countryCode, string phoneNumber, int verificationCode)
        {
            var emailHelperMock = new Mock<IEmailHelper>();
            var randomNumberProvider = new Mock<IRandomNumberProvider>();
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));
            randomNumberProvider.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(verificationCode);
            var verificationCodeDataStoreMock = new Mock<IVerificationCodeDataStore>();
            verificationCodeDataStoreMock.Setup(x => x.GetValueOrDefault(It.IsAny<string>())).Returns(verificationCode);
            verificationCodeDataStoreMock.Setup(x => x.ContainsKey(It.IsAny<string>())).Returns(true);
            var emailVerificationService = new EmailVerificationService(emailHelperMock.Object, randomNumberProvider.Object, verificationCodeDataStoreMock.Object, userRepositoryMock.Object);
            var result = await emailVerificationService.ValidateVerificationCodeAsync(countryCode, phoneNumber, TEST_VERIFICATION_CODE_OTHER);
            Assert.IsFalse(result.IsSuccessed);
            Assert.AreEqual(Constants.PhoneVerifyMessages.Code_Error, result.GetErrorString());
        }

        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, TEST_VERIFICATION_CODE)]
        public async Task ValidateVerificationCode_Should_Return_True_When_Supplied_Correct_VerificationCode(string countryCode, string phoneNumber, int verificationCode)
        {
            var emailHelperMock = new Mock<IEmailHelper>();
            var randomNumberProvider = new Mock<IRandomNumberProvider>();
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));

            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok()));
            randomNumberProvider.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(verificationCode);
            var verificationCodeDataStoreMock = new Mock<IVerificationCodeDataStore>();
            verificationCodeDataStoreMock.Setup(x => x.GetValueOrDefault(It.IsAny<string>())).Returns(verificationCode);
            verificationCodeDataStoreMock.Setup(x => x.ContainsKey(It.IsAny<string>())).Returns(true);
            var emailVerificationService = new EmailVerificationService(emailHelperMock.Object, randomNumberProvider.Object, verificationCodeDataStoreMock.Object, userRepositoryMock.Object);
            var result = await emailVerificationService.ValidateVerificationCodeAsync(countryCode, phoneNumber, verificationCode);
            Assert.IsTrue(result.IsSuccessed);
        }
    }
}