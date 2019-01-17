using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;

using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.IRepository;
using PearUp.BusinessEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PearUp.IBusiness;
using System.Linq;
using PearUp.Tests.Builders;
using System.Linq.Expressions;
using FluentAssertions;
using PearUp.DTO.PhoneVerification;
using PearUp.Business;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    public class PhoneVerificationServiceTests
    {
        #region Initializer
        private const string TEST_NUMBER = "8897322553";
        private const string TEST_COUNTRY_CODE = "91";
        private const int TEST_VERIFICATION_CODE = 9097;
        private TwilioNetworkCredentials TestCredentials { get; set; }
        private TwilioNetworkCredentials InvalidCredentials { get; set; }
        public PhoneVerificationServiceTests()
        {
            TestCredentials = new TwilioNetworkCredentials()
            {
                SendUrl = "https://api.authy.com/protected/json/phones/verification/start",
                VerifyUrl = "https://api.authy.com/protected/json/phones/verification/check?api_key={0}&phone_number={1}&country_code={2}&verification_code={3}",
                Authy_Api_key = "ObFyJkK1sUNl7hYk9Hw8pZ0qrUqM0YBB"
            };
            InvalidCredentials = new TwilioNetworkCredentials()
            {
                SendUrl = "https://api.authy.com/protected/json/phones/verification/start",
                VerifyUrl = "https://api.authy.com/protected/json/phones/verification/check?api_key={0}&phone_number={1}&country_code={2}&verification_code={3}",
                Authy_Api_key = "sdgasdfadhasefasef"
            };
        }

        #endregion

        #region Generate_Verification_Code_Tests
        [Test]
        public async Task GenerateVerificationCode_ShouldReturn_ErrorMessage_If_Phone_Country_params_are_Empty()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Test Content")
                }));
            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.GenerateVerificationCodeAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.PhoneVerifyMessages.Country_Phone_Empty);
        }

        [Test]
        public async Task GenerateVerificationCode_ShouldReturn_ErrorMessage_If_Country_Code_Is_Invalid()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));
            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new TwilioErrorResponse() { errors = new Errors() { message = "test" } }))
                }));

            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.GenerateVerificationCodeAsync("eg", TEST_NUMBER);

            // Assert
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be("test");
        }


        [Test]
        public async Task GenerateVerificationCode_ShouldReturn_ErrorMessage_If_Phone_Number_Is_Invalid()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));
            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new TwilioErrorResponse() { errors = new Errors() { message = "test" } }))
                }));

            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.GenerateVerificationCodeAsync(TEST_COUNTRY_CODE, "xx");

            // Assert
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be("test");
        }


        [Test]
        // api key invalid case
        public async Task GenerateVerificationCode_ShouldReturn_ErrorMessage_If_Api_Key_Is_Invalid()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));
            var iCredentials = Options.Create<TwilioNetworkCredentials>(InvalidCredentials);

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("Test Unauthorized User")
                }));

            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.GenerateVerificationCodeAsync("91", "8897322553");

            // Assert
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.PhoneVerifyMessages.Twilio_Unauthorized);
        }


        [Test]
        public async Task GenerateVerificationCode_ShouldReturn_SuccessMessage_If_Phone_Country_Are_Valid()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);
            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber(TEST_NUMBER, TEST_COUNTRY_CODE)
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Test Content")
                }));

            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.GenerateVerificationCodeAsync(TEST_COUNTRY_CODE, TEST_NUMBER);

            // Assert
            result.IsSuccessed.Should().BeTrue();
        }
        #endregion

        #region Validate_Verification_Code_Tests
        [TestCase(null, null, null)]
        [TestCase(TEST_COUNTRY_CODE, null, null)]
        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, null)]
        [TestCase(TEST_COUNTRY_CODE, null, TEST_VERIFICATION_CODE)]
        [TestCase(null, TEST_NUMBER, null)]
        [TestCase(null, TEST_NUMBER, TEST_VERIFICATION_CODE)]
        [TestCase(null, null, TEST_VERIFICATION_CODE)]
        [TestCase("", "", TEST_VERIFICATION_CODE)]
        [TestCase("", "xx", TEST_VERIFICATION_CODE)]
        [TestCase("xx", "", TEST_VERIFICATION_CODE)]
        [TestCase(TEST_COUNTRY_CODE, "xx", 0000)]
        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, 0000)]
        [TestCase("xx", TEST_NUMBER, 0000)]
        [TestCase("xx", "xx", 0000)]

        public async Task ValidateVerificationCode_ShouldReturn_ErrorMessage_If_Params_Country_Phone_VerifyCode_Are_Empty(string countryCode, string phoneNumber, int verificationCode)
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Test Content")
                }));
            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.ValidateVerificationCodeAsync(countryCode, phoneNumber, verificationCode);

            // Assert
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Empty);
        }

        [TestCase(TEST_COUNTRY_CODE, TEST_NUMBER, 845)]
        [TestCase(TEST_COUNTRY_CODE, "xx", TEST_VERIFICATION_CODE)]
        [TestCase("xx", TEST_NUMBER, TEST_VERIFICATION_CODE)]
        [TestCase("xx", "xx", TEST_VERIFICATION_CODE)]
        [TestCase("xx", "xx", 45435)]
        public async Task ValidateVerificationCode_ShouldReturn_ErrorMessage_If_Params_Country_Phone_VerifyCode_Are_InCorrect(string countryCode, string phoneNumber, int verificationCode)
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var dummyUser = await UserBuilder.Builder()
                    .WithPhoneNumber("9876543210", "91")
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(dummyUser)));

            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new TwilioErrorResponse() { errors = new Errors() { message = "test" } }))
                }));
            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.ValidateVerificationCodeAsync(countryCode, phoneNumber, verificationCode);

            // Assert
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be("test");
        }


        [Test]
        public async Task ValidateVerificationCode_ShouldReturn_ErrorMessage_If_Phone_Country_Verification_Are_Invalid()
        {
            // Arrange
            var _user = await UserBuilder.Builder()
                    .WithPhoneNumber("9876543210", "91")
                    .BuildAsync();

            var mockMessageHandler = new Mock<HttpMessageHandler>();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(_user)));

            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new TwilioErrorResponse() { errors = new Errors() { message = Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Invalid } }))
                }));
            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.ValidateVerificationCodeAsync("dfgsd", "fdgsdfg", 8484);

            // Assert
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Invalid);
        }
        [Test]
        public async Task ValidateVerificationCode_ShouldReturn_SuccessMessage_If_Params_Phone_Country_Verify_Are_Valid()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            var expectedOutput = await UserBuilder.Builder()
                    .WithPhoneNumber("9876543210", "91")
                    .BuildAsync();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).Returns(Task.FromResult(Result.Ok<User>(expectedOutput)));

            var iCredentials = Options.Create<TwilioNetworkCredentials>(TestCredentials);

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Test Content")
                }));
            var service = new TwilioVerificationService(new HttpClient(mockMessageHandler.Object), iCredentials, userRepositoryMock.Object);

            // Act
            var result = await service.ValidateVerificationCodeAsync(TEST_COUNTRY_CODE, expectedOutput.PhoneNumber.PhoneNumber, TEST_VERIFICATION_CODE);

            // Assert
            result.IsSuccessed.Should().BeTrue();
        }

        #endregion
    }
}
