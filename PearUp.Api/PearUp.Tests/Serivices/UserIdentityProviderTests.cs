using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.IRepository;
using PearUp.Tests.Builders;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PearUp.Tests.Serivices
{
    [TestFixture]
    public class UserIdentityProviderTests
    {
        [Test]
        public void GetCurrentUser_Should_Return_CurrentUser_When_User_Is_Authenticated()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var climsIdentity = new ClaimsIdentity();
            var userRepositoryMock = new Mock<IUserRepository>();
            climsIdentity.AddClaim(new Claim(AuthConstants.UserId, "1"));
            httpContextAccessorMock.Setup(context => context.HttpContext.User).Returns(new ClaimsPrincipal(climsIdentity));
            var userIdentityProvider = new IdentityProvider(httpContextAccessorMock.Object, userRepositoryMock.Object);
            var currentUser = userIdentityProvider.GetCurrentUser();
            Assert.IsTrue(currentUser.IsSuccessed);
            Assert.AreEqual(1, currentUser.Value.UserId);
        }

        [Test]
        public void GetCurrentUser_Should_Return_User_Not_Authanticated_When_User_Id_Is_Zero()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var climsIdentity = new ClaimsIdentity();
            climsIdentity.AddClaim(new Claim(AuthConstants.UserId, "0"));
            httpContextAccessorMock.Setup(context => context.HttpContext.User).Returns(new ClaimsPrincipal(climsIdentity));
            var userIdentityProvider = new IdentityProvider(httpContextAccessorMock.Object, userRepositoryMock.Object);
            var currentUser = userIdentityProvider.GetCurrentUser();
            Assert.IsFalse(currentUser.IsSuccessed);
            Assert.AreEqual(Constants.CommonErrorMessages.User_Not_Authenticated, currentUser.GetErrorString());
        }

        [Test]
        public void GetCurrentUser_Should_Return_User_Not_Authanticated_When_UserId_Claim_Not_Exist()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var climsIdentity = new ClaimsIdentity();
            climsIdentity.AddClaim(new Claim("TestClim", "1"));
            httpContextAccessorMock.Setup(context => context.HttpContext.User).Returns(new ClaimsPrincipal(climsIdentity));
            var userIdentityProvider = new IdentityProvider(httpContextAccessorMock.Object, userRepositoryMock.Object);
            var currentUser = userIdentityProvider.GetCurrentUser();
            Assert.IsFalse(currentUser.IsSuccessed);
            Assert.AreEqual(Constants.CommonErrorMessages.User_Not_Authenticated, currentUser.GetErrorString());
        }

        [Test]
        public void GetCurrentUser_Should_Return_User_Not_Authanticated_When_User_IS_Not_Authenticated()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var instance = new IdentityProvider(httpContextAccessorMock.Object, userRepositoryMock.Object);
            var currentUser = instance.GetCurrentUser();
            Assert.IsFalse(currentUser.IsSuccessed);
            Assert.AreEqual(Constants.CommonErrorMessages.User_Not_Authenticated, currentUser.GetErrorString());
        }

        [Test]
        public async Task  GetUser_Should_Return_Success_Result_For_Valid_Current_User()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var climsIdentity = new ClaimsIdentity();
            climsIdentity.AddClaim(new Claim(AuthConstants.UserId, "1"));
            var testUser =await new UserBuilder().BuildAsync();
            httpContextAccessorMock.Setup(context => context.HttpContext.User).Returns(new ClaimsPrincipal(climsIdentity));
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(Result.Ok(testUser));
            var instance = new IdentityProvider(httpContextAccessorMock.Object, userRepositoryMock.Object);
            var currentUser = await instance.GetUser();
            currentUser.IsSuccessed.Should().BeTrue();
            currentUser.Value.Should().Be(testUser);
        }

        [Test]
        public async Task GetUser_Should_Return_Failure_Result_For_Invalid_Current_User()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var climsIdentity = new ClaimsIdentity();
            climsIdentity.AddClaim(new Claim(AuthConstants.UserId, "1"));
            var testUser = await new UserBuilder().BuildAsync();
            httpContextAccessorMock.Setup(context => context.HttpContext.User).Returns(new ClaimsPrincipal(climsIdentity));
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(Result.Fail<User>(string.Empty));
            var instance = new IdentityProvider(httpContextAccessorMock.Object, userRepositoryMock.Object);
            var currentUser = await instance.GetUser();
            currentUser.IsSuccessed.Should().BeFalse();
            currentUser.GetErrorString().Should().Be(string.Empty);
        }
    }
}