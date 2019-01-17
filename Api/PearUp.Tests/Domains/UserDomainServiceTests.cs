using FluentAssertions;
using Moq;
using NUnit.Framework;
using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.DomainServices;
using PearUp.DTO;
using PearUp.Factories.Implementation;
using PearUp.IDomainServices;
using PearUp.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    public class UserDomainServiceTests
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
                Latitude = -34.343544,
                Longitude = 23.232244
            };
            var hashingServiceMock = new Mock<IHashingService>();
            var userDomainServiceMock = new Mock<IUserDomainService>();
            userDomainServiceMock.Setup(x => x.SetUserInterests(It.IsAny<User>(), It.IsAny<int[]>())).ReturnsAsync(Result.Ok());
            var userFactory = new UserFactory(hashingServiceMock.Object, userDomainServiceMock.Object);
            var userResult = await userFactory.CreateUserAsync(userRegistrationDTO);
            _user = userResult.Value;
        }

        [TestCase(null)]
        [TestCase(new int[] { })]
        [TestCase(new int[] { 1, 2, 3 })]
        public async Task SetUserInterests_Should_Set_User_Interests_For_Valid_Interest_IdsAsync(int[] interestIds)
        {
            List<Interest> interests = new List<Interest>();
            List<UserInterest> userInterests = new List<UserInterest>();
            CreateInterestsAndUserInterests(out interests, out userInterests, interestIds);
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(It.IsAny<int[]>())).ReturnsAsync(Result.Ok((IReadOnlyList<Interest>)interests));


            var userDomainService = new UserDomainService(interestRepositoryMock.Object);
            var setUserInterestsResult = await userDomainService.SetUserInterests(_user, interestIds);

            setUserInterestsResult.IsSuccessed.Should().BeTrue();
            _user.Interests.Should().BeEquivalentTo(userInterests);
        }

        [Test]
        public async Task SetUserInterests_Should_Return_Fail_Result_For_Invalid_Interest_IdsAsync()
        {
            const string Error_Message = "Invalid Interests";
            int[] interestIds = new int[] { 98, 99, 100 };
            List<Interest> interests = new List<Interest>();
            List<UserInterest> userInterests = new List<UserInterest>();
            CreateInterestsAndUserInterests(out interests, out userInterests, interestIds);
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(It.IsAny<int[]>())).ReturnsAsync(Result.Fail<IReadOnlyList<Interest>>(Error_Message));

            var userDomainService = new UserDomainService(interestRepositoryMock.Object);
            var setUserInterestsResult = await userDomainService.SetUserInterests(_user, interestIds);

            setUserInterestsResult.IsSuccessed.Should().BeFalse();
            setUserInterestsResult.GetErrorString().Should().Be(Error_Message);
        }

        [Test]
        public async Task SetUserInterests_Should_Return_Fail_Result_For_Valid_And_Invalid_Interest_IdsAsync()
        {
            var interestIds = new int[] { 1, 2, 3, 98, 99, 100 };
            var inputForCreatingInterestForMockingInterestRepository = new int[] { 1, 2, 3 };
            List<Interest> interests = new List<Interest>();
            List<UserInterest> userInterests = new List<UserInterest>();
            CreateInterestsAndUserInterests(out interests, out userInterests, inputForCreatingInterestForMockingInterestRepository);
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(It.IsAny<int[]>())).ReturnsAsync(Result.Ok((IReadOnlyList<Interest>)interests));

            var userDomainService = new UserDomainService(interestRepositoryMock.Object);
            var setUserInterestsResult = await userDomainService.SetUserInterests(_user, interestIds);

            setUserInterestsResult.IsSuccessed.Should().BeFalse();
            setUserInterestsResult.GetErrorString().Should().Be(UserDomainService.Invalid_Interest);
        }

        private void CreateInterestsAndUserInterests(out List<Interest> interests, out List<UserInterest> userInterests, int[] interestIds)
        {
            interests = new List<Interest>();
            userInterests = new List<UserInterest>();

            if (interestIds == null)
                return;

            foreach (var i in interestIds)
            {
                var interest = Interest.Create("test", "test").Value;
                interest.Id = i;
                var userInterestResult = UserInterest.Create(i);

                interests.Add(interest);
                userInterests.Add(userInterestResult.Value);
            }
        }
    }
}