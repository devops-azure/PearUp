using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PearUp.BusinessEntity;
using PearUp.DTO.PhoneVerification;
using PearUp.IBusiness;
using System.Threading.Tasks;

namespace PearUp.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PhoneVerification")]
    [AllowAnonymous]
    public class PhoneVerificationController : Controller
    {
        private readonly IPhoneVerificationService _phoneVerificationService;

        public PhoneVerificationController(IPhoneVerificationService phoneVerificationService)
        {
            this._phoneVerificationService = phoneVerificationService;
        }


        [HttpPost]
        [Route("GenerateVerificationCode")]
        public async Task<IActionResult> GenerateVerificationCode([FromBody]GeneratePhoneVerificationDTO generateDTO)
        {
            if (generateDTO == null)
                return BadRequest(Constants.CommonErrorMessages.Request_Is_Not_Valid);

            var phoneResult = UserPhoneNumber.Create(generateDTO.PhoneNumber, generateDTO.PhoneNumber);
            if (!phoneResult.IsSuccessed)
                return BadRequest(phoneResult.GetErrorString());

            var result = await _phoneVerificationService.GenerateVerificationCodeAsync(generateDTO.CountryCode, generateDTO.PhoneNumber);
            if (!result.IsSuccessed)
                return BadRequest(result.GetErrorString());

            return Ok(Constants.PhoneVerifyMessages.Verification_Code_Sent);

        }

        [HttpPost]
        [Route("ValidateVerificationCode")]
        public async Task<IActionResult> ValidateVerificationCode([FromBody]ValidatePhoneVerificationDTO validateDTO)
        {
            if (validateDTO == null)
                return BadRequest(Constants.CommonErrorMessages.Request_Is_Not_Valid);

            var phoneResult = UserPhoneNumber.Create(validateDTO.PhoneNumber, validateDTO.CountryCode);
            if (!phoneResult.IsSuccessed)
                return BadRequest(phoneResult.GetErrorString());

            var result = await _phoneVerificationService.ValidateVerificationCodeAsync(validateDTO.CountryCode, validateDTO.PhoneNumber, validateDTO.VerificationCode);
            if (!result.IsSuccessed)
                return BadRequest(result.GetErrorString());

            return Ok(Constants.PhoneVerifyMessages.Verification_Completed);
        }

    }
}
