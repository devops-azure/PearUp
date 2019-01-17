using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PearUp.BusinessEntity;
using PearUp.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PearUp.Tests.Repository
{
    [TestFixture]
    public class UserRepositoryUnitTests
    {
        private PearUpContext _pearUpContext;
        private List<User> _users;
        private User _expectedResult;

        public UserRepositoryUnitTests()
        {
            InitializeUsersAsync();
            InitContext();
        }
        private async Task InitializeUsersAsync()
        {
            _users = new List<User>();
            var result = await new PearUp.Tests.Builders.UserBuilder().BuildAsync();
            _expectedResult = result;
            _users.Add(result);
        }
        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<PearUpContext>()
                 .UseInMemoryDatabase("PearUp");
            _pearUpContext = new PearUpContext(builder.Options);
            _pearUpContext.Users.AddRange(_users);
            _pearUpContext.SaveChanges();
        }

        [Test]
        [TestCase("123456789")]
        public async Task GetUserByPhoneNumber_ShouldReturn_SuccessResult_When_PhoneNumber_Is_Correct(string phoneNumber)
        {
            var userRepository = new UserRepository(_pearUpContext);
            var actualResults = await userRepository.GetUserByPhoneNumber(phoneNumber);
            actualResults.IsSuccessed.Should().BeTrue();
            actualResults.Value.Should().Be(_expectedResult);
        }

        [TestCase("123456654123")]
        public async Task GetUserByPhoneNumber_ShouldReturn_FailureResult_When_PhoneNumber_Is_Not_Present(string phoneNumber)
        {
            var userRepository = new UserRepository(_pearUpContext);
            var actualResults = await userRepository.GetUserByPhoneNumber(phoneNumber);
            actualResults.IsSuccessed.Should().BeFalse();
            actualResults.GetErrorString().Should().Be(Constants.UserErrorMessages.User_Does_Not_Exist);
        }

        [Test]
        [TestCase("")]
        public async Task GetUserByPhoneNumber_ShouldReturn_FailureResult_When_PhoneNumber_Is_Empty(string phoneNumber)
        {
            var userRepository = new UserRepository(_pearUpContext);
            var actualResults = await userRepository.GetUserByPhoneNumber(phoneNumber);
            actualResults.IsSuccessed.Should().BeFalse();
            actualResults.GetErrorString().Should().Be(Constants.UserErrorMessages.User_Does_Not_Exist);
        }

        [Test]
        [TestCase(1)]
        public async Task GetUserByIdAsync_ShouldReturn_SuccessResult_When_UserId_IsValid(int userId)
        {
            var userRepository = new UserRepository(_pearUpContext);
            var actualResults = await userRepository.GetUserByIdAsync(userId);
            actualResults.IsSuccessed.Should().BeTrue();
            actualResults.Value.Should().Be(_expectedResult);
        }

        [Test]
        [TestCase(2)]
        public async Task GetUserByIdAsync_ShouldReturn_FailureResult_When_UserId_Not_Present(int userId)
        {
            var userRepository = new UserRepository(_pearUpContext);
            var actualResults = await userRepository.GetUserByIdAsync(userId);
            actualResults.IsSuccessed.Should().BeFalse();
            actualResults.GetErrorString().Should().Be(Constants.UserErrorMessages.User_Does_Not_Exist);
        }

        [Test]
        [TestCase(0)]
        public async Task GetUserByIdAsync_ShouldReturn_FailureResult_When_UserId_Receives_Default_Int(int userId)
        {
            var userRepository = new UserRepository(_pearUpContext);
            var actualResults = await userRepository.GetUserByIdAsync(userId);
            actualResults.IsSuccessed.Should().BeFalse();
            actualResults.GetErrorString().Should().Be(Constants.UserErrorMessages.User_Does_Not_Exist);
        }
    }
}
