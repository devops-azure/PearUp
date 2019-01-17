using Moq;
using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.DTO;
using PearUp.Factories.Implementation;
using PearUp.IDomainServices;
using PearUp.IRepository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PearUp.Tests.Builders
{
    class UserBuilder
    {
        private byte[] randomBytes;
        private Mock<IHashingService> mockHashingService;
        UserRegistrationDTO registrationDTO;
        private Mock<IUserDomainService> userDomainServiceMock;
    public UserBuilder()
        {
            randomBytes = new byte[32];
            mockHashingService = new Mock<IHashingService>();
            mockHashingService.Setup(x => x.GenerateSalt()).Returns(randomBytes);
            mockHashingService.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(randomBytes);
            userDomainServiceMock = new Mock<IUserDomainService>();
            userDomainServiceMock.Setup(x => x.SetUserInterests(It.IsAny<User>(), It.IsAny<int[]>())).ReturnsAsync(Result.Ok());
            SetUserDetails();
        }

        private void SetUserDetails()
        {
            registrationDTO = new UserRegistrationDTO
            {
                BucketList = "test",
                CountryCode = "91",
                DateOfBirth = new DateTime(1999, 2, 19),
                Distance = 1,
                FunAndInteresting = "test",
                Gender = 1,
                LookingFor = 2,
                MaxAge = 24,
                MinAge = 21,
                Password = "test",
                Profession = "test",
                PhoneNumber = "123456789",
                School = "test",
                FullName = "test name",
                Latitude = 84.444454,
                Longitude = -1.548454
            };
        }

        public static UserBuilder Builder()
        {
            return new UserBuilder();
        }

        public UserBuilder WithName(string name)
        {
            this.registrationDTO.FullName = name;
            return this;
        }

        public UserBuilder WithPassword(string password)
        {
            this.registrationDTO.Password = password;
            return this;
        }

        public UserBuilder WithPhoneNumber(string phoneNumber, string countryCode)
        {
            this.registrationDTO.PhoneNumber = phoneNumber;
            this.registrationDTO.CountryCode = countryCode;
            return this;
        }

        public UserBuilder WithMinAndMaxAge(int minAge, int maxAge)
        {
            this.registrationDTO.MinAge = minAge;
            this.registrationDTO.MaxAge = maxAge;
            return this;
        }

        public UserBuilder WithGender(int gender) {
            this.registrationDTO.Gender = gender;
            return this;
        }

        public UserBuilder WithFunInterestsAndBucket(string funInterests, string bucketList) {
            this.registrationDTO.FunAndInteresting = funInterests;
            this.registrationDTO.BucketList = bucketList;
            return this;
        }

        public UserBuilder WithProfession(string profession) {
            this.registrationDTO.Profession = profession;
            return this;
        }
        public UserBuilder WithSchool(string school)
        {
            this.registrationDTO.School = school;
            return this;
        }

        public UserBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            this.registrationDTO.DateOfBirth = dateOfBirth;
            return this;
        }

        public UserBuilder WithInterests(int[] ids)
        {
            this.registrationDTO.InterestIds = ids;
            return this;
        }


        public async System.Threading.Tasks.Task<User> BuildAsync()
        {
            var factory = new UserFactory(mockHashingService.Object, userDomainServiceMock.Object);
            var result = await factory.CreateUserAsync(registrationDTO);
            return result.Value;
        }
    }
}
