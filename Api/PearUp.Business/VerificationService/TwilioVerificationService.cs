using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PearUp.CommonEntities;
using PearUp.DTO.PhoneVerification;
using PearUp.IBusiness;
using PearUp.IRepository;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PearUp.Business
{
    public class TwilioVerificationService : UserVerificationService, IPhoneVerificationService
    {
        private readonly HttpClient _client;
        private readonly IUserRepository _userRepository;
        private readonly TwilioNetworkCredentials _twilioNetworkCredentials;

        public TwilioVerificationService(HttpClient client, IOptions<TwilioNetworkCredentials> twilioNetworkCredentials, IUserRepository userRepository) : base(userRepository)
        {
            _client = client;
            _twilioNetworkCredentials = twilioNetworkCredentials.Value;
            _userRepository = userRepository;
        }

        public async Task<Result> GenerateVerificationCodeAsync(string countryCode, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Result.Fail(Constants.PhoneVerifyMessages.Country_Phone_Empty);
            }
            var userResult = await _userRepository.GetUserByPhoneNumber( phoneNumber);
            if (!userResult.IsSuccessed)
            {
                return Result.Fail(Constants.UserErrorMessages.User_Does_Not_Exist_With_Given_PhoneNumber);
            }
            var dictionary = new Dictionary<string, string>()
                {
                    { "via", "sms"},
                    { "locale", "en"},
                    { "phone_number",phoneNumber},
                    { "country_code", countryCode},
                    { "api_key",_twilioNetworkCredentials.Authy_Api_key }
                };
            var requestContent = new FormUrlEncodedContent(dictionary);

            // https://api.authy.com/protected/$AUTHY_API_FORMAT/phones/verification/start?via=$VIA&country_code=$USER_COUNTRY&phone_number=$USER_PHONE
            var response = await _client.PostAsync(_twilioNetworkCredentials.SendUrl, requestContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return Result.Ok(true);

            return await HandleFailedResponse(response);
        }

        public async Task<Result> ValidateVerificationCodeAsync(string countryCode, string phoneNumber, int verificationCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(phoneNumber) || verificationCode == default(int))
            {
                return Result.Fail(Constants.PhoneVerifyMessages.Country_Phone_VerifyCode_Empty);
            }
            var userResult = await _userRepository.GetUserByPhoneNumber(phoneNumber);
            if (!userResult.IsSuccessed)
            {
                return Result.Fail(Constants.UserErrorMessages.User_Does_Not_Exist_With_Given_PhoneNumber);
            }
            phoneNumber = phoneNumber + countryCode;
            var url = string.Format(_twilioNetworkCredentials.VerifyUrl, _twilioNetworkCredentials.Authy_Api_key, phoneNumber, countryCode, verificationCode);

            // https://api.authy.com/protected/$AUTHY_API_FORMAT/phones/verification/check?phone_number=$USER_PHONE&country_code=$USER_COUNTRY&verification_code=$VERIFY_CODE
            var response = await _client.GetAsync(requestUri: url);

            if (response.IsSuccessStatusCode)
            {
                return Result.Ok(true);
            }
            return await HandleFailedResponse(response);
        }

        private async Task<Result> HandleFailedResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Result.Fail(Constants.PhoneVerifyMessages.Twilio_Unauthorized);

            var errorMessage = JsonConvert.DeserializeObject<TwilioErrorResponse>(await response.Content.ReadAsStringAsync());
            return Result.Fail(errorMessage.errors?.message);
        }
    }
}
