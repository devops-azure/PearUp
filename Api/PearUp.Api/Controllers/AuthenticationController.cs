using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PearUp.Authentication;
using PearUp.Dto;
using PearUp.DTO;
using PearUp.IBusiness;
using System.Threading.Tasks;

namespace PearUp.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Authentication")]
    public class AuthenticationController : TokenController
    {
        private readonly IPearUpAuthenticationService _authenticationService;
        private readonly ITokenProvider _tokenProvider;

        public AuthenticationController(IPearUpAuthenticationService authenticationService, ITokenProvider tokenProvider):base(tokenProvider)
        {
            this._authenticationService = authenticationService;
            this._tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Validate User Credentials 
        /// </summary>
        /// <param name="userCredentialsDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]UserCredentialsDTO userCredentialsDTO)
        {
            if (userCredentialsDTO == null)
            {
                return BadRequest(PearUp.Constants.CommonErrorMessages.Request_Is_Not_Valid);
            }
            var user = await _authenticationService.Authenticate(userCredentialsDTO.PhoneNumber, userCredentialsDTO.Password);
            if (!user.IsSuccessed)
                return BadRequest(user.GetErrorString());

            var token = CreateUserToken(user.Value);
            if (!token.IsSuccessed)
                return BadRequest(token.GetErrorString());
            var response = new LoginResponseDTO
            {
                Token = token.Value.Value,
            };
            return Ok(response);
        }

        /// <summary>
        /// Validate Admin Credentials 
        /// </summary>
        /// <param name="adminCredentialsDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AdminLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin([FromBody]AdminCredentialsDTO adminCredentialsDTO)
        {
            if (adminCredentialsDTO == null)
            {
                return BadRequest(PearUp.Constants.CommonErrorMessages.Request_Is_Not_Valid);
            }
            var result = await _authenticationService.AuthenticateAdmin(adminCredentialsDTO.EmailId, adminCredentialsDTO.Password);
            if (!result.IsSuccessed)
                return BadRequest(result.GetErrorString());

            var token = CreateAdminToken(result.Value);
            if (!token.IsSuccessed)
                return BadRequest(token.GetErrorString());

            return Ok(CommonEntities.Result.Ok(new AdminAuthDto
            {
                PearAdmin = result.Value,
                Token = token.Value
            }));
        }
    }
}