using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.BusinessEntity.Builders;
using PearUp.CommonEntities;
using PearUp.DTO;
using PearUp.Factories.Interfaces;
using PearUp.IRepository;
using System.Threading.Tasks;
using PearUp.IDomainServices;

namespace PearUp.Factories.Implementation
{
    public class UserFactory : IUserFactory
    {
        public const string Password_Is_Required = "Password is required.";
        public const string Invalid_Interests = "Invalid interests";

        private readonly IHashingService _hashingService;
        private readonly IUserDomainService _userDomainService;

        public UserFactory(IHashingService hashingService, IUserDomainService userDomainService)
        {
            _hashingService = hashingService;
            _userDomainService = userDomainService;
        }

        public async Task<Result<User>> CreateUserAsync(UserRegistrationDTO userRegistrationDTO)
        {
            if (string.IsNullOrWhiteSpace(userRegistrationDTO.Password))
                return Result.Fail<User>(Password_Is_Required);

            var ageResult = Age.Create(userRegistrationDTO.DateOfBirth);
            var lookingForGenderResult = Gender.Create(userRegistrationDTO.LookingFor);
            var matchPreference = UserMatchPreference.Create(lookingForGenderResult.Value, userRegistrationDTO.MinAge, userRegistrationDTO.MaxAge, userRegistrationDTO.Distance);
            var phoneNumberResult = UserPhoneNumber.Create(userRegistrationDTO.PhoneNumber, userRegistrationDTO.CountryCode);
            var genderResult = Gender.Create(userRegistrationDTO.Gender);

            var passwordSalt = _hashingService.GenerateSalt();
            var passwordHash = _hashingService.GetHash(userRegistrationDTO.Password, passwordSalt);
            var passwordResult = Password.Create(passwordHash, passwordSalt);

            var locationResult = Location.Create(userRegistrationDTO.Latitude, userRegistrationDTO.Longitude);

            var finalResult = Result.Combine(ageResult, 
                lookingForGenderResult, 
                matchPreference, 
                phoneNumberResult, 
                genderResult,
                passwordResult,
                locationResult);

            if (!finalResult.IsSuccessed)
                return Result.Fail<User>(finalResult.GetErrorString());

            var userResult = UserBuilder.Builder()
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
             .WithLocation(locationResult.Value)
             .Build();

            var setUserInterestsResult = await _userDomainService.SetUserInterests(userResult.Value, userRegistrationDTO.InterestIds);
            if (!setUserInterestsResult.IsSuccessed)
                return Result.Fail<User>(setUserInterestsResult.GetErrorString());

            return userResult;
        }
    }
}
