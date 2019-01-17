using FluentAssertions;
using Moq;
using NUnit.Framework;
using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.DTO;
using PearUp.Factories.Implementation;
using PearUp.IDomainServices;
using PearUp.IRepository;
using System;
using System.Collections.Generic;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class UserTests
    {
        private User _user;

        [OneTimeSetUp]
        public async System.Threading.Tasks.Task SetUpAsync()
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
            var hashingServiceMock = new Mock<IHashingService>();
            var userDomainServiceMock = new Mock<IUserDomainService>();
            userDomainServiceMock.Setup(x => x.SetUserInterests(It.IsAny<User>(), It.IsAny<int[]>())).ReturnsAsync(Result.Ok());
            var userFactory = new UserFactory(hashingServiceMock.Object, userDomainServiceMock.Object);
            var userResult = await userFactory.CreateUserAsync(userRegistrationDTO);
            _user = userResult.Value;
        }

        [Test]
        public void SetInterests_Should_Set_Interests()
        {
            List<Interest> interests;
            List<UserInterest> userInterests;
            CreateInterestsAndUserInterests(out interests, out userInterests, 10);

            _user.SetInterests(interests);
            _user.Interests.Should().BeEquivalentTo(userInterests);
        }

        [Test]
        public void SetInterests_Should_Modify_Interests()
        {
            List<Interest> interests;
            List<UserInterest> userInterests;

            CreateInterestsAndUserInterests(out interests, out userInterests, 5);
            _user.SetInterests(interests);
            CreateInterestsAndUserInterests(out interests, out userInterests, 10);
            _user.SetInterests(interests);
            _user.Interests.Should().BeEquivalentTo(userInterests);
        }

        [Test]
        public void SetInterests_Should_Throw_Exception_For_Invalid_Interests()
        {
            Exception actualException = Assert.Throws<ArgumentException>(() => _user.SetInterests(null));
            actualException.Message.Should().BeEquivalentTo(User.Atleast_One_Interest_Is_Required);
        }

        [Test]
        public void SetInterests_Should_Throw_Exception_For_Zero_Interests()
        {
            List<Interest> interests = new List<Interest>();
            Exception actualException = Assert.Throws<ArgumentException>(() => _user.SetInterests(interests));
            actualException.Message.Should().BeEquivalentTo(User.Atleast_One_Interest_Is_Required);
        }

        [Test]
        public void SetInterests_Should_Throw_Exception_For_Interests_Greater_Than_Ten()
        {
            List<Interest> interests;
            List<UserInterest> userInterests;

            CreateInterestsAndUserInterests(out interests, out userInterests, 11);
            Exception actualException = Assert.Throws<ArgumentException>(() => _user.SetInterests(interests));
            actualException.Message.Should().BeEquivalentTo(User.Interests_Cant_Be_Greater_Than_Ten);
        }

        private void CreateInterestsAndUserInterests(out List<Interest> interests, out List<UserInterest> userInterests, int numberOfInterests)
        {
            interests = new List<Interest>();
            userInterests = new List<UserInterest>();
            for (int i = 1; i <= numberOfInterests; i++)
            {
                var interest = Interest.Create("test", "test").Value;
                interest.Id = i;
                var userInterestResult = UserInterest.Create(i);

                interests.Add(interest);
                userInterests.Add(userInterestResult.Value);
            }
        }

        [Test]
        public void SetPhoto_Should_Be_Add_Photo_To_User()
        {
            var photo = UserPhoto.Create(1, "test");
            _user.SetPhoto(photo.Value);
            _user.Photos[0].Should().BeEquivalentTo(photo.Value);
        }

        [Test]
        public void SetPhoto_Should_Be_Throw_Error_When_Photo_Is_Null()
        {
            Exception actualException = Assert.Throws<ArgumentException>(() => _user.SetPhoto(null));
            actualException.Message.Should().BeEquivalentTo(User.User_Photo_Is_Required);
        }

        [Test]
        public void SetLocation_Should_Return_Success_Message()
        {
            var locationResult = Location.Create(83.283743, 2.324756);
            _user.SetLocation(locationResult.Value);
            _user.Location.Latitude.Should().Be(83.283743);
            _user.Location.Longitude.Should().Be(2.324756);
        }

        [Test]
        public void SetLocation_Should_Return_Sucess_Message()
        {
            var locationResult = Location.Create(0, 0);
            locationResult.IsSuccessed.Should().BeTrue();
            locationResult.Value.Longitude.Should().Be(0);
            locationResult.Value.Latitude.Should().Be(0);
        }
    }
}