using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PearUp.Constants;
using PearUp.Infrastructure.Azure;
using System.Net;
using System.Threading.Tasks;

namespace PearUp.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Storage")]
    [Authorize(Policy = AuthConstants.PolicyUser)]

    public class StorageController : Controller
    {
        private readonly IAzureSasProvider _azureSasProvider;

        public StorageController(IAzureSasProvider azureSasProvider)
        {
            this._azureSasProvider = azureSasProvider;
        }

        /// <summary>
        ///  Will create policy on top of container, and return saskey
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSasKey")]
        public async Task<IActionResult> GetSasKey()
        {
            var sasKey = await _azureSasProvider.GetContainerSAS();
            return Ok(sasKey);
        }
    }
}