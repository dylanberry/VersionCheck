using Microsoft.AspNetCore.Http;

namespace VersionCheck.API.VersionCheck
{
    public interface IVersionCheckService
    {
        void PerformVersionCheck(HttpRequest request);
    }
}