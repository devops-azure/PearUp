using NUnit.Framework;
using PearUp.BusinessEntity;
using System;
using FluentAssertions;
using System.Collections.Generic;
using PearUp.DTO;
using PearUp.Factories.Implementation;
using Moq;
using PearUp.Authentication;
using PearUp.IRepository;
using System.Threading.Tasks;
using PearUp.CommonEntities;
using PearUp.IDomainServices;
using PearUp.DomainServices;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class UserFactoryTests
    {
        private static IEnumerable<UserRegistrationDTO> GetValidInputs
        {
            get
            {
                yield return new UserRegistrationDTO
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
                    InterestIds = new int[] { 1, 2, 3 },
                    Latitude = 84.444454,
                    Longitude = -1.548454
                };
                yield return new UserRegistrationDTO
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
                    InterestIds = new int[] { },
                    Latitude = 84.444454,
                    Longitude = -1.548454
                };
            }
        }

        private static IEnumerable<UserRegistrationDTO> GetInputsWithInvalidPassword
        {
            get
            {
                yield return new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
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
                yield return new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
                    CountryCode = "91",
                    Password = "",
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
            }
        }

        private static IEnumerable<UserRegistrationDTO> WithLongitudeAsNull
        {
            get
            {
                yield return new UserRegistrationDTO
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
                };
                yield return new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
                    Password = "test",
                    CountryCode = "91",
                    FullName = "test",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25),
                    Gender = 1,
                    Profession = "test",
                    LookingFor = 1,
                    MinAge = 25,
                    MaxAge = 26,
                    Distance = 25,
                    FunAndInteresting = "test",
                    BucketList = "test",
                    Latitude = 84.444454,
                };
            }
        }

        private static IEnumerable<UserRegistrationDTO> WithLatitudeAsNull
        {
            get
            {
                yield return new UserRegistrationDTO
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
                    Longitude = -1.548454
                };
                yield return new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
                    Password = "test",
                    CountryCode = "91",
                    FullName = "test",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25),
                    Gender = 1,
                    Profession = "test",
                    LookingFor = 1,
                    MinAge = 25,
                    MaxAge = 26,
                    Distance = 25,
                    FunAndInteresting = "test",
                    BucketList = "test",
                    Longitude = -1.548454
                };
            }
        }

        private static IEnumerable<UserRegistrationDTO> GetInputsWithInvlaidInterests
        {
            get
            {
                yield return new UserRegistrationDTO
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
                    InterestIds = new int[] { 99, 100 },
                    Latitude = 84.444454,
                    Longitude = -1.548454
                };
            }
        }

        [TestCaseSource(nameof(GetValidInputs))]
        public async Task Create_Should_Return_Success_User_ResultAsync(UserRegistrationDTO userRegistrationDTO)
        {
            byte[] randomByteArray = new byte[32];
            Random random = new Random();
            random.NextBytes(randomByteArray);
            var hashingServiceMock = new Mock<IHashingService>();
            hashingServiceMock.Setup(x => x.GenerateSalt()).Returns(randomByteArray);
            hashingServiceMock.Setup(x => x.GetHash(userRegistrationDTO.Password, It.IsAny<byte[]>())).Returns(randomByteArray);
            List<Interest> interests;
            List<UserInterest> userInterests;
            CreateInterestsAndUserInterests(out interests, out userInterests, userRegistrationDTO.InterestIds);
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(It.IsAny<int[]>())).ReturnsAsync(Result.Ok((IReadOnlyList<Interest>)interests));
            var userDomainService = new UserDomainService(interestRepositoryMock.Object);
            var userFactory = new UserFactory(hashingServiceMock.Object, userDomainService);

            var userResult = await userFactory.CreateUserAsync(userRegistrationDTO);
            userResult.IsSuccessed.Should().BeTrue();
            userResult.Value.Should().BeAssignableTo<User>();
            userResult.Value.PhoneNumber.PhoneNumber.Should().Be(userRegistrationDTO.PhoneNumber);
            userResult.Value.PhoneNumber.CountryCode.Should().Be(userRegistrationDTO.CountryCode);
            userResult.Value.Password.PasswordHash.Should().BeEquivalentTo(randomByteArray);
            userResult.Value.Password.PasswordSalt.Should().BeEquivalentTo(randomByteArray);
            userResult.Value.FullName.Should().Be(userRegistrationDTO.FullName);
            userResult.Value.Age.DateOfBirth.Should().Be(userRegistrationDTO.DateOfBirth);
            userResult.Value.Gender.GenderType.Should().Be((GenderType)userRegistrationDTO.Gender);
            userResult.Value.Profession.Should().Be(userRegistrationDTO.Profession);
            userResult.Value.School.Should().Be(userRegistrationDTO.School);
            userResult.Value.MatchPreference.LookingFor.GenderType.Should().Be(userRegistrationDTO.LookingFor);
            userResult.Value.MatchPreference.MinAge.Should().Be(userRegistrationDTO.MinAge);
            userResult.Value.MatchPreference.MaxAge.Should().Be(userRegistrationDTO.MaxAge);
            userResult.Value.MatchPreference.Distance.Should().Be(userRegistrationDTO.Distance);
            userResult.Value.FunAndInterestingThings.Should().Be(userRegistrationDTO.FunAndInteresting);
            userResult.Value.BucketList.Should().Be(userRegistrationDTO.BucketList);
            userResult.Value.Interests.Should().BeEquivalentTo(userInterests);
        }

        [TestCaseSource(nameof(GetInputsWithInvalidPassword))]
        public async Task Create_Should_Return_Fail_User_ResultAsync(UserRegistrationDTO userRegistrationDTO)
        {
            byte[] randomByteArray = new byte[32];
            Random random = new Random();
            random.NextBytes(randomByteArray);
            var hashingServiceMock = new Mock<IHashingService>();
            hashingServiceMock.Setup(x => x.GenerateSalt()).Returns(randomByteArray);
            hashingServiceMock.Setup(x => x.GetHash(userRegistrationDTO.Password, It.IsAny<byte[]>())).Returns(randomByteArray);
            var userDomainServiceMock = new Mock<IUserDomainService>();
            var userFactory = new UserFactory(hashingServiceMock.Object, userDomainServiceMock.Object);
            var userResult = await userFactory.CreateUserAsync(userRegistrationDTO);
            userResult.IsSuccessed.Should().BeFalse();
            userResult.GetErrorString().Should().Be(UserFactory.Password_Is_Required);
        }

        [TestCaseSource(nameof(GetInputsWithInvlaidInterests))]
        public async Task Create_Should_Return_Fail_Result_For_Invalid_Interests(UserRegistrationDTO userRegistrationDTO)
        {
            var errorMessage = "Error Message";
            byte[] randomByteArray = new byte[32];
            Random random = new Random();
            random.NextBytes(randomByteArray);
            var hashingServiceMock = new Mock<IHashingService>();
            hashingServiceMock.Setup(x => x.GenerateSalt()).Returns(randomByteArray);
            hashingServiceMock.Setup(x => x.GetHash(userRegistrationDTO.Password, It.IsAny<byte[]>())).Returns(randomByteArray);
            List<Interest> interests;
            List<UserInterest> userInterests;
            CreateInterestsAndUserInterests(out interests, out userInterests, userRegistrationDTO.InterestIds);
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(It.IsAny<int[]>())).ReturnsAsync(Result.Fail<IReadOnlyList<Interest>>(errorMessage));
            var userDomainService = new UserDomainService(interestRepositoryMock.Object);
            var userFactory = new UserFactory(hashingServiceMock.Object, userDomainService);

            var userResult = await userFactory.CreateUserAsync(userRegistrationDTO);
            userResult.IsSuccessed.Should().BeFalse();
            userResult.GetErrorString().Should().Be(errorMessage);
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

        [TestCaseSource(nameof(WithLatitudeAsNull))]
        public async Task Create_Should_Return_Success_User_Result_With_Latitude_As_Null(UserRegistrationDTO userRegistrationDTO)
        {
            byte[] randomByteArray = new byte[32];
            Random random = new Random();
            random.NextBytes(randomByteArray);
            List<Interest> interests;
            List<UserInterest> userInterests;
            CreateInterestsAndUserInterests(out interests, out userInterests, userRegistrationDTO.InterestIds);
            var hashingServiceMock = new Mock<IHashingService>();
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(It.IsAny<int[]>())).ReturnsAsync(Result.Ok((IReadOnlyList<Interest>)interests));
            var userDomainService = new UserDomainService(interestRepositoryMock.Object);
            hashingServiceMock.Setup(x => x.GenerateSalt()).Returns(randomByteArray);
            hashingServiceMock.Setup(x => x.GetHash(userRegistrationDTO.Password, It.IsAny<byte[]>())).Returns(randomByteArray);
            var userFactory = new UserFactory(hashingServiceMock.Object, userDomainService);
            var userResult = await userFactory.CreateUserAsync(userRegistrationDTO);
            userResult.IsSuccessed.Should().BeTrue();
            userResult.Value.Location.Longitude.Should().Be(-1.548454);
            userResult.Value.Location.Latitude.Should().Be(0);
        }

        [TestCaseSource(nameof(WithLongitudeAsNull))]
        public async Task Create_Should_Return_Success_User_Result_With_Longitude_As_Null(UserRegistrationDTO userRegistrationDTO)
        {
            byte[] randomByteArray = new byte[32];
            Random random = new Random();
            random.NextBytes(randomByteArray);
            List<Interest> interests;
            List<UserInterest> userInterests;
            CreateInterestsAndUserInterests(out interests, out userInterests, userRegistrationDTO.InterestIds);
            var hashingServiceMock = new Mock<IHashingService>();
            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(x => x.GetInterestsByIds(It.IsAny<int[]>())).ReturnsAsync(Result.Ok((IReadOnlyList<Interest>)interests));
            var userDomainService = new UserDomainService(interestRepositoryMock.Object);

            hashingServiceMock.Setup(x => x.GenerateSalt()).Returns(randomByteArray);
            hashingServiceMock.Setup(x => x.GetHash(userRegistrationDTO.Password, It.IsAny<byte[]>())).Returns(randomByteArray);
            var userFactory = new UserFactory(hashingServiceMock.Object, userDomainService);
            var userResult = await userFactory.CreateUserAsync(userRegistrationDTO);
            userResult.IsSuccessed.Should().BeTrue();
            userResult.Value.Location.Longitude.Should().Be(0);
            userResult.Value.Location.Latitude.Should().Be(84.444454);

        }


    }
}
