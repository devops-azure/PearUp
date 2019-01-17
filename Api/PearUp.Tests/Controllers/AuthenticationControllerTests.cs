using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PearUp.Api.Controllers;
using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.BusinessEntity.Builders;
using PearUp.CommonEntities;
using PearUp.CommonEntity;
using PearUp.Dto;
using PearUp.DTO;
using PearUp.IBusiness;
using System;
using System.Threading.Tasks;

namespace PearUp.Tests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private const string VALID_AUTH_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJNaWxpbmQiLCJqdGkiOiJjYWU2MTNkYS0wZTYxLTQ1MzMtOGQ4Yy0xOTk4OWI4NDFiMjAiLCJVc2VySWQiOiIwIiwiZXhwIjoxNTE5NjQ1MTg3LCJpc3MiOiJQZWFyVXAiLCJhdWQiOiJQZWFyVXAifQ.EsU6htcjmweGLYGKl50FfQ_rT4k8btF95DvLPlt4SUw";
        Admin admin = null;
        public AuthenticationControllerTests()
        {
            byte[] randomByteArray = new byte[32];
            Random random = new Random();
            random.NextBytes(randomByteArray);

            admin = AdminBuilder.Builder()
                 .WithName("Admin1")
                 .WithEmail(Email.Create("admin1@pearup.com").Value)
                 .WithPassword(Password.Create(randomByteArray, randomByteArray).Value)
                 .Build().Value;
        }
        [Test]
        public async Task Login_Should_Return_AuthenticationToken_When_Credentials_Are_Valid()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var tokenProviderMock = new Mock<ITokenProvider>();
            var userLoginDTO = new UserCredentialsDTO
            {
                PhoneNumber = "9999999999",
                Password = "test"
            };

            var user = await Builders.UserBuilder.Builder().BuildAsync();
            var authTokenMock = new Mock<IAuthToken>();
            authTokenMock.Setup(a => a.Value).Returns(VALID_AUTH_TOKEN);

            userServiceMock.Setup(us => us.Authenticate(userLoginDTO.PhoneNumber, userLoginDTO.Password)).ReturnsAsync(Result.Ok(user));
            tokenProviderMock.Setup(tp => tp.CreateToken(It.IsAny<TokenInfo>(), It.IsAny<bool>())).Returns(Result.Ok(authTokenMock.Object));
            var controller = new AuthenticationController(userServiceMock.Object, tokenProviderMock.Object);

            var actualResult = await controller.Login(userLoginDTO);
            Assert.IsAssignableFrom<OkObjectResult>(actualResult);
            var contentResult = actualResult as OkObjectResult;
            Assert.AreEqual(200, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<LoginResponseDTO>(contentResult.Value);
            var actualValue = contentResult.Value as LoginResponseDTO;
            Assert.AreEqual(VALID_AUTH_TOKEN, actualValue.Token);
        }

        [Test]
        public async Task Login_Should_Return_ErrorMessage_When_Credentials_Are_InValid()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var failureResult = Result.Fail<User>(Constants.UserErrorMessages.PhoneNumber_Or_Password_Is_Incorrect);
            userServiceMock.Setup(us => us.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(failureResult);
            var controller = new AuthenticationController(userServiceMock.Object, null);
            var userLoginDTO = new UserCredentialsDTO
            {
                PhoneNumber = "9999999999",
                Password = "test"
            };
            var actualResult = await controller.Login(userLoginDTO);
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            Assert.AreEqual(400, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            Assert.AreEqual(Constants.UserErrorMessages.PhoneNumber_Or_Password_Is_Incorrect, contentResult.Value);
        }

        [Test]
        public async Task Login_Should_Return_ErrorMessage_When_Credentials_Are_Empty()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var failureResult = Result.Fail<User>(Constants.UserErrorMessages.Phone_Number_And_Password_Are_Required);
            userServiceMock.Setup(us => us.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(failureResult);
            var controller = new AuthenticationController(userServiceMock.Object, null);
            var userLoginDTO = new UserCredentialsDTO
            {
                PhoneNumber = "",
                Password = ""
            };
            var actualResult = await controller.Login(userLoginDTO);
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            Assert.AreEqual(400, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            Assert.AreEqual(Constants.UserErrorMessages.Phone_Number_And_Password_Are_Required, contentResult.Value);
        }

        [Test]
        public async Task Login_Should_Return_ErrorMessage_When_Credentials_Model_Is_Null()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var controller = new AuthenticationController(userServiceMock.Object, null);
            var actualResult = await controller.Login(null);
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            Assert.AreEqual(400, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            Assert.AreEqual(Constants.CommonErrorMessages.Request_Is_Not_Valid, contentResult.Value);
        }

        //Admin Login Action
        [Test]
        public async Task AdminLogin_Should_Return_AuthenticationToken_When_Credentials_Are_Valid()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var adminLoginDTO = new AdminCredentialsDTO
            {
                EmailId = "admin@pearup.com",
                Password = "Pearup1234$"
            };
            userServiceMock.Setup(us => us.AuthenticateAdmin(adminLoginDTO.EmailId, adminLoginDTO.Password)).ReturnsAsync(Result.Ok(admin));
            var tokenProviderMock = new Mock<ITokenProvider>();

            var authTokenMock = new Mock<IAuthToken>();
            authTokenMock.Setup(a => a.Value).Returns(VALID_AUTH_TOKEN);
            tokenProviderMock.Setup(tp => tp.CreateToken(It.IsAny<TokenInfo>(), It.IsAny<bool>())).Returns(Result.Ok(authTokenMock.Object));
            var controller = new AuthenticationController(userServiceMock.Object, tokenProviderMock.Object);

            var actualResult = await controller.AdminLogin(adminLoginDTO);
            Assert.IsAssignableFrom<OkObjectResult>(actualResult);
            var contentResult = actualResult as OkObjectResult;
            Assert.AreEqual(200, contentResult.StatusCode.Value);
            var actualValue = contentResult.Value as Result<AdminAuthDto>;
            Assert.IsTrue(actualValue.IsSuccessed);
        }

        [Test]
        public async Task AdminLogin_Should_Return_ErrorMessage_When_Credentials_Are_InValid()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var failureResult = Result.Fail<Admin>(Constants.UserErrorMessages.Email_Or_Password_Is_Incorrect);
            userServiceMock.Setup(us => us.AuthenticateAdmin(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(failureResult);
            var controller = new AuthenticationController(userServiceMock.Object, null);
            var adminLoginDTO = new AdminCredentialsDTO
            {
                EmailId = "admin@pearup.com123",
                Password = "test"
            };
            var actualResult = await controller.AdminLogin(adminLoginDTO);
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            Assert.AreEqual(400, contentResult.StatusCode.Value);
            var actualValue = contentResult.Value as string;
            Assert.AreEqual(Constants.UserErrorMessages.Email_Or_Password_Is_Incorrect, actualValue);
        }

        [Test]
        public async Task AdminLogin_Should_Return_ErrorMessage_When_Credentials_Are_Empty()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var failureResult = Result.Fail<Admin>(Constants.UserErrorMessages.EmailAddress_And_Password_Are_Required);
            userServiceMock.Setup(us => us.AuthenticateAdmin(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(failureResult);
            var controller = new AuthenticationController(userServiceMock.Object, null);
            var adminLoginDTO = new AdminCredentialsDTO
            {
                EmailId = "",
                Password = ""
            };
            var actualResult = await controller.AdminLogin(adminLoginDTO);
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            var actualValue = contentResult.Value as string;
            Assert.AreEqual(400, contentResult.StatusCode.Value);
            Assert.AreEqual(Constants.UserErrorMessages.EmailAddress_And_Password_Are_Required, actualValue);

        }

        [Test]
        public async Task AdminLogin_Should_Return_ErrorMessage_When_Credentials_Model_Is_Null()
        {
            var userServiceMock = new Mock<IPearUpAuthenticationService>();
            var controller = new AuthenticationController(userServiceMock.Object, null);
            var actualResult = await controller.AdminLogin(null);
            Assert.IsAssignableFrom<BadRequestObjectResult>(actualResult);
            var contentResult = actualResult as BadRequestObjectResult;
            Assert.AreEqual(400, contentResult.StatusCode.Value);
            Assert.IsAssignableFrom<string>(contentResult.Value);
            Assert.AreEqual(Constants.CommonErrorMessages.Request_Is_Not_Valid, contentResult.Value);
        }
    }
}
