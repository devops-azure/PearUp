using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using PearUp.Authentication;
using PearUp.Business;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.DTO;
using PearUp.Factories.Implementation;
using PearUp.Factories.Interfaces;
using PearUp.IDomainServices;
using PearUp.IRepository;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    public class UserPhotoServiceTests
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
                Latitude = 23.232433,
                Longitude = -42.434345
            };
            var userResult = await userFactory.CreateUserAsync(_userRegistrationDTO);
            _user = userResult.Value;
        }
        //User photo Tests
        [TestCase(1, "test.com", 1)]
        public async Task SetUserPhotoAsync_Should_Return_Success_Result_When_Inputs_Are_Valid(int userId, string path, int order)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mediatorMock = new Mock<IMediator>();
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(Result.Ok(_user));
            userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<string>())).ReturnsAsync(Result.Ok());

            var userPhotoService = new UserPhotoService(userRepositoryMock.Object, mediatorMock.Object);
            var result = await userPhotoService.SetUserPhotoAsync(userId, path, order);

            result.IsSuccessed.Should().BeTrue();
        }

        [TestCase(2, "test.com", 1)]
        public async Task SetUserPhotoAsync_Should_Return_Failure_Result_When_User_Not_Present_In_System(int userId, string path, int order)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mediatorMock = new Mock<IMediator>();
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(Result.Fail<User>(UserErrorMessages.User_Does_Not_Exist));

            var userPhotoService = new UserPhotoService(userRepositoryMock.Object, mediatorMock.Object);
            var result = await userPhotoService.SetUserPhotoAsync(userId, path, order);
            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserErrorMessages.User_Does_Not_Exist);
        }

        
        [TestCase(1, "test.com", 0)]
        public async Task SetUserPhotoAsync_Should_Return_Failure_Result_When_Order_Is_Invalid(int userId, string path, int order)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mediatorMock = new Mock<IMediator>();

            var userPhotoService = new UserPhotoService(userRepositoryMock.Object, mediatorMock.Object);
            var result = await userPhotoService.SetUserPhotoAsync(userId, path, order);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserPhoto.Order_Should_Be_Greater_Than_Zero);
        }

        [TestCase(1, "", 1)]
        public async Task SetUserPhotoAsync_Should_Return_Failure_Result_When_Path_Is_Invalid(int userId, string path, int order)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mediatorMock = new Mock<IMediator>();

            var userPhotoService = new UserPhotoService(userRepositoryMock.Object, mediatorMock.Object);
            var result = await userPhotoService.SetUserPhotoAsync(userId, path, order);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserPhoto.Path_Should_Not_Be_Empty);
        }

        [TestCase(0, "test.com", 1)]
        public async Task SetUserPhotoAsync_Should_Return_Failure_Result_When_User_Id_Is_Invalid(int userId, string path, int order)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mediatorMock = new Mock<IMediator>();
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(Result.Fail<User>(UserErrorMessages.User_Does_Not_Exist));
            var userPhotoService = new UserPhotoService(userRepositoryMock.Object, mediatorMock.Object);
            var result = await userPhotoService.SetUserPhotoAsync(userId, path, order);

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserErrorMessages.User_Does_Not_Exist);
        }
    }
}
