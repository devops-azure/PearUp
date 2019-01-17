using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PearUp.CommonEntities;
using PearUp.CommonEntity;
using PearUp.IBusiness;
using PearUp.Infrastructure;
using PearUp.IRepository;
using PearUp.Utilities;

namespace PearUp.Business
{
    public class EmailVerificationService : UserVerificationService, IPhoneVerificationService
    {
        private readonly IEmailHelper _emailHelper;
        private readonly IRandomNumberProvider _randomNumberProvider;
        private readonly IVerificationCodeDataStore _verificationCodeDataStore;
        private readonly IUserRepository _userRepository;

        public EmailVerificationService(IEmailHelper emailHelper, IRandomNumberProvider randomNumberProvider, IVerificationCodeDataStore verificationCodesDataStore, IUserRepository userRepository) : base(userRepository)
        {
            _emailHelper = emailHelper;
            _randomNumberProvider = randomNumberProvider;
            _verificationCodeDataStore = verificationCodesDataStore;
            _userRepository = userRepository;
        }
        public async Task<Result> GenerateVerificationCodeAsync(string countryCode, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Result.Fail<bool>(Constants.PhoneVerifyMessages.Country_Phone_Empty);
            }
            var userResult = await _userRepository.GetUserByPhoneNumber(phoneNumber);
            if (!userResult.IsSuccessed)
            {
                return Result.Fail<bool>(Constants.UserErrorMessages.User_Does_Not_Exist_With_Given_PhoneNumber);
            }
            var exists = _verificationCodeDataStore.ContainsKey(countryCode + phoneNumber);
            if (exists)
                return Result.Fail<bool>(Constants.PhoneVerifyMessages.Verification_Code_Already_Sent);
            var newVerificationCode = GenerateRandomNumber();
            await _emailHelper.SendAsync(newVerificationCode.ToString());
            _verificationCodeDataStore.Add(countryCode + phoneNumber, newVerificationCode);
            return Result.Ok();
        }

        public async Task<Result> ValidateVerificationCodeAsync(string countryCode, string phoneNumber, int verificationCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(phoneNumber) || verificationCode == 0)
            {
                return Result.Fail<bool>(Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Empty);
            }

            var userResult = await _userRepository.GetUserByPhoneNumber(phoneNumber);
            if (!userResult.IsSuccessed)
            {
                return Result.Fail(Constants.UserErrorMessages.User_Does_Not_Exist_With_Given_PhoneNumber);
            }
            var user = userResult.Value;

            var exists = _verificationCodeDataStore.ContainsKey(countryCode + phoneNumber);
            if (!exists)
                return Result.Fail(Constants.PhoneVerifyMessages.Error_Message);

            var verificationCodeSent = _verificationCodeDataStore.GetValueOrDefault(countryCode + phoneNumber);
            if (verificationCode != verificationCodeSent)
                return Result.Fail(Constants.PhoneVerifyMessages.Code_Error);

            _verificationCodeDataStore.Remove(countryCode + phoneNumber);
            return Result.Ok();
        }



        private int GenerateRandomNumber()
        {
            int min = 1000;
            int max = 10000;

            return _randomNumberProvider.Next(min, max);
        }
    }
}
