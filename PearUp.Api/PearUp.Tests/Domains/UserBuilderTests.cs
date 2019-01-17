using NUnit.Framework;
using PearUp.BusinessEntity;
using System;
using FluentAssertions;
using System.Collections.Generic;
using PearUp.DTO;
using Moq;
using PearUp.Authentication;
using PearUp.BusinessEntity.Builders;
using PearUp.CommonEntities;

namespace PearUp.Tests.Domains
{
    [TestFixture]
    class UserBuilderTests
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
                    Latitude = 44.968046,
                    Longitude = -94.420307
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
                    Latitude = 44.968046,
                    Longitude = -94.420307
                };
                yield return new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
                    Password = "test",
                    CountryCode = "91",
                    FullName = "test",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25),
                    Gender = 1,
                    School = "test",
                    LookingFor = 1,
                    MinAge = 25,
                    MaxAge = 26,
                    Distance = 25,
                    FunAndInteresting = "test",
                    BucketList = "test",
                    Latitude = 44.968046,
                    Longitude = -94.420307
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
                    BucketList = "test",
                    Latitude = 44.968046,
                    Longitude = -94.420307
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
                    Latitude = 44.968046,
                    Longitude = -94.420307
                };
            }
        }
        private static IEnumerable<UserRegistrationDTO> GetInputsWithInvalidFullName
        {
            get
            {
                yield return new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
                    Password = "test",
                    CountryCode = "91",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25),
                    Gender = 1,
                    Profession = "test",
                    School = "test",
                    LookingFor = 1,
                    MinAge = 25,
                    MaxAge = 26,
                    Distance = 25,
                    FunAndInteresting = "test",
                    BucketList = "test"
                };
                yield return new UserRegistrationDTO
                {
                    PhoneNumber = "9999999999",
                    Password = "test",
                    CountryCode = "91",
                    FullName = "",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25),
                    Gender = 1,
                    Profession = "test",
                    School = "test",
                    LookingFor = 1,
                    MinAge = 25,
                    MaxAge = 26,
                    Distance = 25,
                    FunAndInteresting = "test",
                    BucketList = "test"
                };
            }
        }

        private static IEnumerable<UserRegistrationDTO> GetInputsWithFunAndInterestingLengthGreaterThan140Char
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
                    FunAndInteresting = "start...ulbkhpemrcbjwemiddbdouhxanzuexlzafcxomtjtbvowappuebvkadocwsgaqzzndo" +
                                    "xgjfhbswyryufjtbndgseyrwqpypoyatsxkflllahxoqpfvixypeag...end...141",
                    BucketList = "test"
                };
            }
        }

        private static IEnumerable<UserRegistrationDTO> GetInputsWithBucketListLengthGreaterThan140Char
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
                    BucketList = "start...ulbkhpemrcbjwemiddbdouhxanzuexlzafcxomtjtbvowappuebvkadocwsgaqzzndo" +
                                    "xgjfhbswyryufjtbndgseyrwqpypoyatsxkflllahxoqpfvixypeag...end...141",
                    Latitude = 44.968046,
                    Longitude = -94.420307
                };
            }
        }

        private static IEnumerable<UserRegistrationDTO> GetInputWithOutLocation
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
                    //Latitude = 44.968046,
                    //Longitude = -94.420307
                };
            }
        }

        [TestCaseSource(nameof(GetValidInputs))]
        public void Build_Should_Return_Success_User_Result_For_Valid_Inputs(UserRegistrationDTO userRegistrationDTO)
        {
            byte[] randomByteArray = GetRandomByteArray();
            var result = BuildUser(userRegistrationDTO, randomByteArray);

            result.IsSuccessed.Should().BeTrue();
            result.Value.Should().BeAssignableTo<User>();
            result.Value.PhoneNumber.PhoneNumber.Should().Be(userRegistrationDTO.PhoneNumber);
            result.Value.PhoneNumber.CountryCode.Should().Be(userRegistrationDTO.CountryCode);
            result.Value.Password.PasswordHash.Should().BeEquivalentTo(randomByteArray);
            result.Value.Password.PasswordSalt.Should().BeEquivalentTo(randomByteArray);
            result.Value.FullName.Should().Be(userRegistrationDTO.FullName);
            result.Value.Age.DateOfBirth.Should().Be(userRegistrationDTO.DateOfBirth);
            result.Value.Gender.GenderType.Should().Be((GenderType)userRegistrationDTO.Gender);
            result.Value.Profession.Should().Be(userRegistrationDTO.Profession);
            result.Value.School.Should().Be(userRegistrationDTO.School);
            result.Value.MatchPreference.LookingFor.GenderType.Should().Be(userRegistrationDTO.LookingFor);
            result.Value.MatchPreference.MinAge.Should().Be(userRegistrationDTO.MinAge);
            result.Value.MatchPreference.MaxAge.Should().Be(userRegistrationDTO.MaxAge);
            result.Value.MatchPreference.Distance.Should().Be(userRegistrationDTO.Distance);
            result.Value.FunAndInterestingThings.Should().Be(userRegistrationDTO.FunAndInteresting);
            result.Value.BucketList.Should().Be(userRegistrationDTO.BucketList);
            result.Value.Location.Latitude.Should().Be(userRegistrationDTO.Latitude);
            result.Value.Location.Longitude.Should().Be(userRegistrationDTO.Longitude);
        }

        [TestCaseSource(nameof(GetInputsWithInvalidFullName))]
        public void Build_Should_Return_Fail_User_Result_For_Invalid_FullName(UserRegistrationDTO userRegistrationDTO)
        {
            var result = BuildUser(userRegistrationDTO, GetRandomByteArray());

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserBuilder.FullName_Is_Required);
        }

        [TestCaseSource(nameof(GetInputsWithFunAndInterestingLengthGreaterThan140Char))]
        public void Build_Should_Return_Fail_User_Result_For_FunAndInteresting_Longer_Than_140_Char(UserRegistrationDTO userRegistrationDTO)
        {
            var result = BuildUser(userRegistrationDTO, GetRandomByteArray());

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserBuilder.Fun_And_Interests_Should_Not_Exceed_140_Characters);
        }

        [TestCaseSource(nameof(GetInputsWithBucketListLengthGreaterThan140Char))]
        public void Build_Should_Return_Fail_User_Result_For_BucketList_Longer_Than_140_Char(UserRegistrationDTO userRegistrationDTO)
        {
            var result = BuildUser(userRegistrationDTO, GetRandomByteArray());

            result.IsSuccessed.Should().BeFalse();
            result.GetErrorString().Should().Be(UserBuilder.BucketList_Should_Not_Exceed_140_Characters);
        }

        private static Result<User> BuildUser(UserRegistrationDTO userRegistrationDTO, byte[] randomByteArray)
        {
            var hashingServiceMock = new Mock<IHashingService>();


            var ageResult = Age.Create(userRegistrationDTO.DateOfBirth);
            var lookingForGenderResult = Gender.Create(userRegistrationDTO.LookingFor);
            var matchPreference = UserMatchPreference.Create(lookingForGenderResult.Value, userRegistrationDTO.MinAge, userRegistrationDTO.MaxAge, userRegistrationDTO.Distance);
            var phoneNumberResult = UserPhoneNumber.Create(userRegistrationDTO.PhoneNumber, userRegistrationDTO.CountryCode);
            var genderResult = Gender.Create(userRegistrationDTO.Gender);
            var loctionResult = Location.Create(userRegistrationDTO.Latitude, userRegistrationDTO.Longitude);


            hashingServiceMock.Setup(x => x.GenerateSalt()).Returns(randomByteArray);
            hashingServiceMock.Setup(x => x.GetHash(userRegistrationDTO.Password, It.IsAny<byte[]>())).Returns(randomByteArray);
            var passwordSalt = hashingServiceMock.Object.GenerateSalt();
            var passwordHash = hashingServiceMock.Object.GetHash(userRegistrationDTO.Password, passwordSalt);
            var passwordResult = Password.Create(passwordHash, passwordSalt);

            var result = UserBuilder.Builder()
             .WithName(userRegistrationDTO.FullName)
             .WithPhoneNumber(phoneNumberResult.Value)
             .WithPassword(passwordResult.Value)
             .WithGender(genderResult.Value)
             .WithMatchPreference(matchPreference.Value)
             .WithAge(ageResult.Value)
             .WithBucketList(userRegistrationDTO.BucketList)
             .WithFunAndInterestingThings(userRegistrationDTO.FunAndInteresting)
             .WithSchool(userRegistrationDTO.School)
             .WithProfession(userRegistrationDTO.Profession)
             .WithLocation(loctionResult.Value)
             .Build();
            return result;
        }

        private static byte[] GetRandomByteArray()
        {
            byte[] randomByteArray = new byte[32];
            Random random = new Random();
            random.NextBytes(randomByteArray);
            return randomByteArray;
        }
    }
}
