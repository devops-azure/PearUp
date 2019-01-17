using Moq;
using NUnit.Framework;
using PearUp.Authentication;

using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.IBusiness;
using PearUp.IRepository;
using PearUp.BusinessEntity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using PearUp.Tests.Builders;
using PearUp.BusinessEntity.Builders;
using PearUp.Business;

namespace PearUp.Tests.Serivices
{

    [TestFixture]
    public class PearUpAuthenticationServiceUnitTests
    {
        public Admin admin = null;
        public PearUpAuthenticationServiceUnitTests()
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
        [TestCase("987654321", "123456")]
        public async Task Authenticate_Should_Return_SuccessResult_When_Credentials_Are_Valid(string PhoneNumber, string Password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var hashingService = new Mock<IHashingService>();
            var user = await Builders.UserBuilder.Builder()
                    .WithPhoneNumber("9876543210", "91")
                    .BuildAsync();
            var successResult = Result.Ok<User>(user);

            userRepositoryMock.Setup(us => us.GetUserByPhoneNumber(It.IsAny<string>()))
                .ReturnsAsync(successResult);

            hashingService.Setup(hash => hash.SlowEquals(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
               .Returns(true);

            var authService = new PearUpAuthenticationService(userRepositoryMock.Object, hashingService.Object, adminRepositoryMock.Object);
            var actualResult = await authService.Authenticate(PhoneNumber, Password);

            Assert.IsTrue(actualResult.IsSuccessed);
            Assert.AreEqual(actualResult.Value, user);
        }

        [TestCase("987654321", "123456")]
        public async Task Authenticate_Should_Return_Failure_Result_When_PhoneNumberExist_And_Password_Is_InCorrect(string phoneNumber, string password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var hashingService = new Mock<IHashingService>();

            var successResult = Result.Ok<User>(await Builders.UserBuilder.Builder()
                    .WithPhoneNumber("9876543210", "91")
                    .BuildAsync());

            userRepositoryMock.Setup(us => us.GetUserByPhoneNumber(It.IsAny<string>()))
                .ReturnsAsync(successResult);

            hashingService.Setup(hash => hash.SlowEquals(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
              .Returns(false);

            var authService = new PearUpAuthenticationService(userRepositoryMock.Object, hashingService.Object, adminRepositoryMock.Object);
            var actualResult = await authService.Authenticate(phoneNumber, password);

            Assert.IsFalse(actualResult.IsSuccessed);
            Assert.AreEqual(actualResult.GetErrorString(), UserErrorMessages.Password_Is_Incorrect);
        }

        [TestCase("", "")]
        public async Task Authenticate_Should_Return_Failure_Result_When_Credentials_Are_Empity(string phoneNumber, string password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var hashingService = new Mock<IHashingService>();

            var authService = new PearUpAuthenticationService(userRepositoryMock.Object, hashingService.Object, adminRepositoryMock.Object);
            var actualResult = await authService.Authenticate(phoneNumber, password);

            Assert.IsFalse(actualResult.IsSuccessed);
            Assert.AreEqual(actualResult.GetErrorString(), UserErrorMessages.Phone_Number_And_Password_Are_Required);
        }

        [TestCase("admin@pearup.com", "123456")]
        public async Task AuthenticateAdmin_Should_Return_SuccessResult_When_Credentials_Are_Valid(string email, string password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var hashingService = new Mock<IHashingService>();


            adminRepositoryMock.Setup(us => us.GetAdminByEmailId(It.IsAny<string>()))
                .ReturnsAsync(Result.Ok(admin));

            hashingService.Setup(hash => hash.SlowEquals(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
               .Returns(true);

            var authService = new PearUpAuthenticationService(userRepositoryMock.Object, hashingService.Object, adminRepositoryMock.Object);
            var actualResult = await authService.AuthenticateAdmin(email, password);

            Assert.IsTrue(actualResult.IsSuccessed);
            Assert.AreEqual(actualResult.Value, admin);
        }

        [TestCase("admin@pearup.com", "123456")]
        public async Task AuthenticateAdmin_Should_Return_Failure_Result_When_Email_Exist_And_Password_Is_InCorrect(string email, string password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var hashingService = new Mock<IHashingService>();

            adminRepositoryMock.Setup(us => us.GetAdminByEmailId(It.IsAny<string>()))
                .ReturnsAsync(Result.Ok(admin));

            hashingService.Setup(hash => hash.SlowEquals(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
              .Returns(false);

            var authService = new PearUpAuthenticationService(userRepositoryMock.Object, hashingService.Object, adminRepositoryMock.Object);
            var actualResult = await authService.AuthenticateAdmin(email, password);

            Assert.IsFalse(actualResult.IsSuccessed);
            Assert.AreEqual(actualResult.GetErrorString(), UserErrorMessages.Password_Is_Incorrect);
        }

        [TestCase("", "")]
        public async Task AuthenticateAdmin_Should_Return_Failure_Result_When_Credentials_Are_Empity(string email, string password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var hashingService = new Mock<IHashingService>();

            var authService = new PearUpAuthenticationService(userRepositoryMock.Object, hashingService.Object, adminRepositoryMock.Object);
            var actualResult = await authService.AuthenticateAdmin(email, password);

            Assert.IsFalse(actualResult.IsSuccessed);
            Assert.AreEqual(UserErrorMessages.EmailAddress_And_Password_Are_Required, actualResult.GetErrorString());
        }
    }
}
