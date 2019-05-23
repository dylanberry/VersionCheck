using Microsoft.AspNetCore.Mvc.Filters;
using VersionCheck.Common;

namespace VersionCheck.API.VersionCheck
{
    public class ApiVersionFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context?.HttpContext?.Response?.Headers?.Add(HeaderKeys.ApiVersion, VersionCheckService.ApiVersionString);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
