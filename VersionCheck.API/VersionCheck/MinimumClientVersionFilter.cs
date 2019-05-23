using Microsoft.AspNetCore.Mvc.Filters;

namespace VersionCheck.API.VersionCheck
{
    public class MinimumClientVersionFilter : IActionFilter
    {
        private readonly IVersionCheckService _versionCheckHelper;

        public MinimumClientVersionFilter(IVersionCheckService versionCheckHelper)
        {
            _versionCheckHelper =  versionCheckHelper;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _versionCheckHelper.PerformVersionCheck(context.HttpContext.Request);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
