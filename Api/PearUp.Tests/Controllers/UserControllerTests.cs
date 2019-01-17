using NUnit.Framework;
using Moq;
using FluentAssertions;
using PearUp.IBusiness;
using AutoMapper;
using PearUp.Authentication;
using PearUp.Api.Controllers;
using PearUp.DTO;
using System;
using PearUp.CommonEntity;
using Microsoft.AspNetCore.Mvc;
using PearUp.CommonEntities;
using PearUp.Factories.Implementation;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PearUp.IRepository;
using PearUp.IDomainServices;
using PearUp.BusinessEntity;

namespace PearUp.Tests.Controllers
{
    namespace PearUp.Tests.Controllers
    {
        [TestFixture]
        public class UserControllerTests
        {
            private UserFactory _userFactory;
            [OneTimeSetUp]
            public void Setup()
            {
                var hashAlgorithm = new SHA256CryptoServiceProvider();
                var randomNumberGenerator = new RNGCryptoServiceProvider();
                var hashingService = new HashingService(randomNumberGenerator, hashAlgorithm);
                var userDomainServiceMock = new Mock<IUserDomainService>();
                userDomainServiceMock.Setup(x => x.SetUserInterests(It.IsAny<User>(), It.IsAny<int[]>())).ReturnsAsync(Result.Ok());
                _userFactory = new UserFactory(hashingService, userDomainServiceMock.Object);
            }

            [Test]
            public async Task RegisterUser_Should_Retrun_Token_If_User_Registration_Is_SuccessfulAsync()
            {
                var userRegistrationDTO = new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
                    Password = "test",
                    CountryCode = "91",
                    FullName = "test",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25),
                    Gender = 1,
                    Profession = "test",
                    School = "test",
                    LookingFor = 1,
                    MinAge = 25,
                    MaxAge = 26,
                    Distance = 25,
                    FunAndInteresting = "test",
                    BucketList = "test",
                    Latitude = 84.444454,
                    Longitude = -1.548454
                };
                var userResult = await _userFactory.CreateUserAsync(userRegistrationDTO);
                var user = userResult.Value;
                var expectedResponse = new LoginResponseDTO
                {
                    Token = "Token",
                };

                var userServiceMock = new Mock<IUserService>();
                var mapperMock = new Mock<IMapper>();
                var tokenProviderMock = new Mock<ITokenProvider>();
                var authTokenMock = new Mock<IAuthToken>();

                userServiceMock.Setup(x => x.RegisterUserAsync(It.IsAny<UserRegistrationDTO>())).ReturnsAsync(Result.Ok(user));
                authTokenMock.Setup(x => x.Value).Returns("Token");
                tokenProviderMock.Setup(x => x.CreateToken(It.IsAny<TokenInfo>(), It.IsAny<bool>())).Returns(Result.Ok(authTokenMock.Object));

                var userController = new UserController(userServiceMock.Object, mapperMock.Object, tokenProviderMock.Object);
                var result = await userController.RegisterUser(userRegistrationDTO);

                var contentResult = result as OkObjectResult;
                contentResult.StatusCode.Should().Be(200);
                contentResult.Value.Should().BeAssignableTo<LoginResponseDTO>();
                var actualValue = contentResult.Value as LoginResponseDTO;
                actualValue.Token.Should().Be(expectedResponse.Token);
            }

            [Test]
            public async Task RegisterUser_Should_Retrun_Error_When_UserRegistrationDTO_Is_Null()
            {
                UserRegistrationDTO userRegistrationDTO = null;

                var userServiceMock = new Mock<IUserService>();
                var mapperMock = new Mock<IMapper>();
                var tokenProviderMock = new Mock<ITokenProvider>();
                var authTokenMock = new Mock<IAuthToken>();


                var userController = new UserController(userServiceMock.Object, mapperMock.Object, tokenProviderMock.Object);
                var result = await userController.RegisterUser(userRegistrationDTO);

                var contentResult = result as BadRequestObjectResult;
                contentResult.StatusCode.Should().Be(400);
                contentResult.Value.Should().BeAssignableTo<string>();
                contentResult.Value.Should().Be(Constants.CommonErrorMessages.Request_Is_Not_Valid);
            }

            [Test]
            public async Task SetUserInterests_Should_Set_UserInterestsAsync()
            {
                var userServiceMock = new Mock<IUserService>();
                var mapperMock = new Mock<IMapper>();
                var tokenProviderMock = new Mock<ITokenProvider>();
                var authTokenMock = new Mock<IAuthToken>();

                userServiceMock.Setup(x => x.SetUserInterestsAsync(It.IsAny<int[]>())).ReturnsAsync(Result.Ok());

                var userController = new UserController(userServiceMock.Object, mapperMock.Object, tokenProviderMock.Object);
                var result = await userController.SetUserInterests(It.IsAny<int[]>());

                var contentResult = result as OkResult;
                contentResult.StatusCode.Should().Be(200);
            }

            [Test]
            public async Task SetUserInterests_Return_BadRequest_Error_Message_On_Failure()
            {
                const string ErrorMessage = "Error Message";

                var userServiceMock = new Mock<IUserService>();
                var mapperMock = new Mock<IMapper>();
                var tokenProviderMock = new Mock<ITokenProvider>();
                var authTokenMock = new Mock<IAuthToken>();

                userServiceMock.Setup(x => x.SetUserInterestsAsync(It.IsAny<int[]>())).ReturnsAsync(Result.Fail(ErrorMessage));

                var userController = new UserController(userServiceMock.Object, mapperMock.Object, tokenProviderMock.Object);
                var result = await userController.SetUserInterests(It.IsAny<int[]>());

                var contentResult = result as BadRequestObjectResult;
                contentResult.StatusCode.Should().Be(400);
                contentResult.Value.Should().Be(ErrorMessage);

            }

            [Test]
            public async Task SetLocation_Return_Success_Message_For_Vaild_Inputs()
            {
                var userServiceMock = new Mock<IUserService>();
                var mapperMock = new Mock<IMapper>();
                var tokenProviderMock = new Mock<ITokenProvider>();
                var authTokenMock = new Mock<IAuthToken>();

                userServiceMock.Setup(x => x.SetLocationAsync(It.IsAny<UserLocationDTO>())).ReturnsAsync(Result.Ok());

                var userController = new UserController(userServiceMock.Object, mapperMock.Object, tokenProviderMock.Object);
                var result = await userController.SetLocation(It.IsAny<UserLocationDTO>());

                var contentResult = result as OkResult;
                contentResult.StatusCode.Should().Be(200);
            }

            [Test]
            public async Task SetLocation_Return_BadRequest_Message_For_On_Failure()
            {
                const string ErrorMessage = "Error Message";

                var userServiceMock = new Mock<IUserService>();
                var mapperMock = new Mock<IMapper>();
                var tokenProviderMock = new Mock<ITokenProvider>();
                var authTokenMock = new Mock<IAuthToken>();

                userServiceMock.Setup(x => x.SetLocationAsync(It.IsAny<UserLocationDTO>())).ReturnsAsync(Result.Fail(ErrorMessage));

                var userController = new UserController(userServiceMock.Object, mapperMock.Object, tokenProviderMock.Object);
                var result = await userController.SetLocation(It.IsAny<UserLocationDTO>());

                var contentResult = result as BadRequestObjectResult;
                contentResult.StatusCode.Should().Be(400);
                contentResult.Value.Should().Be(ErrorMessage);
            }
        }
    }
}

