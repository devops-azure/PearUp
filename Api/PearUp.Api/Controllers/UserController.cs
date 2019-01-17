using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PearUp.Authentication;
using PearUp.Constants;
using PearUp.DTO;
using PearUp.IBusiness;
using System.Threading.Tasks;

namespace PearUp.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : TokenController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenProvider _tokenProvider;

        public UserController(IUserService userService,
            IMapper mapper,
            ITokenProvider tokenProvider) : base(tokenProvider)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Register New User
        /// </summary>
        /// <param name="userRegistrationDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegistrationDTO userRegistrationDTO)
        {
            if (userRegistrationDTO == null)
            {
                return BadRequest(Constants.CommonErrorMessages.Request_Is_Not_Valid);
            }

            var userResult = await _userService.RegisterUserAsync(userRegistrationDTO);

            if (!userResult.IsSuccessed)
                return BadRequest(userResult.GetErrorString());

            var tokenResult = CreateUserToken(userResult.Value);

            if (!tokenResult.IsSuccessed)
                return BadRequest(tokenResult.GetErrorString());

            var response = new LoginResponseDTO
            {
                Token = tokenResult.Value.Value,
            };
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = AuthConstants.PolicyUser)]
        [Route("Interests")]
        public async Task<IActionResult> SetUserInterests([FromBody]int[] interestIds)
        {
            var updateResult = await _userService.SetUserInterestsAsync(interestIds);
            if (!updateResult.IsSuccessed)
                return BadRequest(updateResult.GetErrorString());
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = AuthConstants.PolicyUser)]
        [Route("Location")]
        public async Task<IActionResult> SetLocation([FromBody] UserLocationDTO userLocationDTO)
        {
            var updateLocationResult = await _userService.SetLocationAsync(userLocationDTO);
            if (!updateLocationResult.IsSuccessed)
                return BadRequest(updateLocationResult.GetErrorString());
            return Ok();
        }
    }
}
