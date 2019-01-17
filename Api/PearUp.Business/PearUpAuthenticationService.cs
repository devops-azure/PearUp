using System;
using System.Threading.Tasks;
using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.IBusiness;
using PearUp.IRepository;

namespace PearUp.Business
{
    public class PearUpAuthenticationService : IPearUpAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly IAdminRepository _adminRepository;

        public PearUpAuthenticationService(IUserRepository userRepository
            , IHashingService hashingService
            ,IAdminRepository adminRepository)
        {
            this._userRepository = userRepository;
            this._hashingService = hashingService;
            this._adminRepository = adminRepository;
        }

        public async Task<Result<User>> Authenticate(string phoneNumber, string password)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(password))
                return Result.Fail<User>(UserErrorMessages.Phone_Number_And_Password_Are_Required);

            //getting user details with phone number
            var user = await _userRepository.GetUserByPhoneNumber(phoneNumber);
            if (!user.IsSuccessed)
                return Result.Fail<User>(user.GetErrorString());

            var isValidUser = ValidatePassword(user.Value.Password.PasswordSalt, user.Value.Password.PasswordHash, password);
            if (!isValidUser)
                return Result.Fail<User>(UserErrorMessages.Password_Is_Incorrect);

            return Result.Ok(user.Value);
        }

        public async Task<Result<Admin>> AuthenticateAdmin(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return Result.Fail<Admin>(UserErrorMessages.EmailAddress_And_Password_Are_Required);

            //getting user details with phone number
            var admin = await _adminRepository.GetAdminByEmailId(email);
            if (!admin.IsSuccessed)
                return Result.Fail<Admin>(admin.GetErrorString());

            var isValidAdmin = ValidatePassword(admin.Value.Password.PasswordSalt, admin.Value.Password.PasswordHash, password);
            if (!isValidAdmin)
                return Result.Fail<Admin>(UserErrorMessages.Password_Is_Incorrect);
            return Result.Ok(admin.Value);
        }

        private bool ValidatePassword(byte[] passwordSalt, byte[] passwordHash, string password)
        {
            //checking password is correct
            byte[] newPasswordHash = _hashingService.GetHash(password, passwordSalt);
            return _hashingService.SlowEquals(newPasswordHash, passwordHash);
        }

    }
}
