using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PearUp.DTO;
using PearUp.IApplicationServices;
using System.Threading.Tasks;

namespace PearUp.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Interest")]
    [Authorize]
    public class InterestController : Controller
    {
        private readonly IInterestService _interestService;

        public InterestController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        /// <summary>
        /// Used to Retreive all the Interests available.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _interestService.GetInterests();
            if (!result.IsSuccessed)
                return BadRequest(result.GetErrorString());
            return Ok(result.Value);
        }

        /// <summary>
        /// Used to create new Interest.
        /// </summary>
        /// <param name="createInterestDTO">interests object as parameter.</param>
        /// <returns>Result object.</returns>
        [HttpPost]
        [Authorize(Policy = Constants.AuthConstants.PolicyAdmin)]
        public async Task<IActionResult> Insert([FromBody] CreateInterestDTO createInterestDTO)
        {
            if (createInterestDTO == null)
                return BadRequest(Constants.InterestErrorMessages.Interest_Should_Not_Be_Empty);
            var result = await _interestService.InsertInterest(createInterestDTO);
            if (!result.IsSuccessed)
                return BadRequest(result.GetErrorString());
            return Ok(result.IsSuccessed);
        }

        /// <summary>
        /// Used to update existing Interest.
        /// </summary>
        /// <param name="interestDTO">properties that need to be updated.</param>
        /// <returns>Result object.</returns>
        [HttpPut]
        [Authorize(Policy = Constants.AuthConstants.PolicyAdmin)]
        public async Task<IActionResult> Update([FromBody] InterestDTO interestDTO)
        {
            if (interestDTO == null)
                return BadRequest(Constants.InterestErrorMessages.Interest_Should_Not_Be_Empty);
            var result = await _interestService.UpdateInterest(interestDTO);
            if (!result.IsSuccessed)
                return BadRequest(result.GetErrorString());
            return Ok(result.IsSuccessed);
        }



    }
}