using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using PearUp.Authentication;
using PearUp.Business;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.DomainServices;
using PearUp.DTO;
using PearUp.Factories.Implementation;
using PearUp.Factories.Interfaces;
using PearUp.IDomainServices;
using PearUp.IRepository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    public class UserServiceTests
    {
        private User _user;
        private UserRegistrationDTO _userRegistrationDTO;
        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            var hashAlgorithm = new SHA256CryptoServiceProvider();
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            var hashingService = new Authentication.HashingService(randomNumberGenerator, hashAlgorithm);
            var userDomainServiceMock = new Mock<IUserDomainService>();
            userDomainServiceMock.Setup(x => x.SetUserInterests(It.IsAny<User>(), It.IsAny<int[]>())).ReturnsAsync(Result.Ok());
            var userFactory = new UserFactory(hashingService, userDomainServiceMock.Object);
            _userRegistrationDTO = new UserRegistrationDTO
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
            var userResult = await userFactory.CreateUserAsync(_userRegistrationDTO);
            _user = userResult.Value;
        }

        [Test]
        public async Task RegisterUserAsync_Should_Return_User_Result_For_Valid_UserRegistrationDTOAsync()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userFactoryMock.Setup(x => x.CreateUserAsync(It.IsAny<UserRegistrationDTO>())).ReturnsAsync(Result.Ok(_user));
            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).ReturnsAsync(Result.Fail<User>(string.Empty));

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var actualResult = await userService.RegisterUserAsync(_userRegistrationDTO);

            actualResult.IsSuccessed.Should().BeTrue();
            actualResult.Value.Should().BeAssignableTo<User>();
        }

        [Test]
        public async Task RegisterUserAsync_Should_Return_Error_If_User_Already_Exists_With_Same_PhoneNumber()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userFactoryMock.Setup(x => x.CreateUserAsync(It.IsAny<UserRegistrationDTO>())).ReturnsAsync(Result.Ok(_user));
            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());
            userRepositoryMock.Setup(x => x.GetUserByPhoneNumber(It.IsAny<string>())).ReturnsAsync(Result.Ok(_user));

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var actualResult = await userService.RegisterUserAsync(_userRegistrationDTO);

            actualResult.IsSuccessed.Should().BeFalse();
            actualResult.GetErrorString().Should().Be(UserErrorMessages.User_Already_Exists_With_Given_Phone_Number);
        }

        [Test]
        public async Task SetUserInterestsAsync_Should_Return_Success_Result()
        {
            var interestIds = new int[] { 1, 2, 3 };
            List<Interest> interests = new List<Interest>();
            for (int i = 1; i <= 3; i++)
            {
                var t = Interest.Create("mock", "mock").Value;
                t.Id = i;
                interests.Add(t);
            }
            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(interestIds)).ReturnsAsync(Result.Ok(interests as IReadOnlyList<Interest>));
            var userDomainService = new UserDomainService(interestRepositoryMock.Object);

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Ok(_user));
            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainService);
            var result = await userService.SetUserInterestsAsync(interestIds);

            result.IsSuccessed.Should().BeTrue();
        }

        [TestCase(null)]
        [TestCase(new int[] { })]
        public async Task SetUserInterestsAsync_Should_Return_Fail_Result_For_Invalid_Interest_Ids(int[] interestIds)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetUserInterestsAsync(interestIds);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserService.Atleast_One_Interest_Is_Required);
        }

        [Test]
        public async Task SetUserInterestsAsync_Should_Return_Fail_Result_If_User_Not_Authenticated()
        {
            var interestIds = new int[] { 1, 2, 3 };
            var errorMessage = "Error Message";

            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Fail<User>(errorMessage));


            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetUserInterestsAsync(interestIds);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(errorMessage);
        }

        [Test]
        public async Task SetUserInterestsAsync_Should_Return_Fail_Result_If_Logged_In_User_Not_Found()
        {
            var interestIds = new int[] { 1, 2, 3 };
            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Fail<User>(UserErrorMessages.User_Does_Not_Exist));

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetUserInterestsAsync(interestIds);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserErrorMessages.User_Does_Not_Exist);
        }

        [Test]
        public async Task SetUserInterestsAsync_Should_Return_Fail_Result_For_Invalid_interestIds()
        {
            var interestIds = new int[] { 1, 2, 3 };
            var Invalid_Interests = "Invalid interests";

            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(interestIds)).ReturnsAsync(Result.Fail<IReadOnlyList<Interest>>(Invalid_Interests));
            var userDomainService = new UserDomainService(interestRepositoryMock.Object);

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Ok(_user));

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainService);
            var result = await userService.SetUserInterestsAsync(interestIds);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(Invalid_Interests);

        }

        [Test]
        public async Task SetUserInterestsAsync_Should_Return_Fail_Result_When_Error_Occured_While_Updating_Interests()
        {
            var interestIds = new int[] { 1, 2, 3 };

            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Ok(_user));
            userDomainServiceMock.Setup(x => x.SetUserInterests(It.IsAny<User>(), It.IsAny<int[]>())).ReturnsAsync(Result.Fail(string.Empty));
            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Fail(UserService.Failed_To_Update_Interests));

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetUserInterestsAsync(interestIds);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(string.Empty);
        }

        [Test]
        public async Task SetLocationAsync_Should_Return_Success_Result()
        {
            var testLocationDTO = new UserLocationDTO() { Latitude = 23.23232, Longitude = -34.353434 };

            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var interestRepositoryMock = new Mock<IInterestRepository>();
            var userDomainService = new UserDomainService(interestRepositoryMock.Object);

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Ok(_user));
            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainService);
            var result = await userService.SetLocationAsync(testLocationDTO);

            result.IsSuccessed.Should().BeTrue();
        }

        [Test]
        public async Task SetLocationAsync_Should_Return_Fail_Result_For_Null_Location()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetLocationAsync(null);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserService.Location_Is_Required);
        }

        [Test]
        public async Task SetLocationAsync_Should_Return_Fail_Result_If_Logged_In_User_Not_Found()
        {
            var testLocationDTO = new UserLocationDTO() { Latitude = 23.23232, Longitude = -34.353434 };

            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Fail<User>(UserErrorMessages.User_Does_Not_Exist));
            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetLocationAsync(testLocationDTO);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserErrorMessages.User_Does_Not_Exist);
        }

        [Test]
        public async Task SetLocationAsync_Should_Return_Fail_Result_If_User_Not_Authenticated()
        {
            var errorMessage = "Error Message";
            var testLocationDTO = new UserLocationDTO() { Latitude = 23.23232, Longitude = -34.353434 };

            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Fail<User>(errorMessage));
            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetLocationAsync(testLocationDTO);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(errorMessage);
        }

        [Test]
        public async Task SetLocationAsync_Should_Return_Fail_Result_When_Error_Occured_While_Updating_Location()
        {
            var testLocationDTO = new UserLocationDTO() { Latitude = 23.23232, Longitude = -34.353434 };

            var userRepositoryMock = new Mock<IUserRepository>();
            var userFactoryMock = new Mock<IUserFactory>();
            var userIdentityProviderMock = new Mock<IIdentityProvider>();
            var mediatorMock = new Mock<IMediator>();
            var userDomainServiceMock = new Mock<IUserDomainService>();

            userIdentityProviderMock.Setup(x => x.GetUser()).ReturnsAsync(Result.Ok<User>(_user));
            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Fail(UserService.Failed_To_Update_Location));

            UserService userService = new UserService(userRepositoryMock.Object, userFactoryMock.Object, userIdentityProviderMock.Object, mediatorMock.Object, userDomainServiceMock.Object);
            var result = await userService.SetLocationAsync(testLocationDTO);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserService.Failed_To_Update_Location);
        }
    }
}