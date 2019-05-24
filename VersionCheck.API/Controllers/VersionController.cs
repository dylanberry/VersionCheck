using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VersionCheck.API.VersionCheck;

namespace VersionCheck.API.Controllers
{
    [ServiceFilter(typeof(MinimumClientVersionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionCheckService _versionCheckService;

        public VersionController(IVersionCheckService versionCheckService)
        {
            _versionCheckService = versionCheckService;
        }
        
        [HttpGet]
        public ActionResult<(string ApiVersion, string MinimumClientVersion)> Get()
        {
            return (VersionCheckService.ApiVersionString, _versionCheckService.MinimumClientVersion);
        }
    }
}
