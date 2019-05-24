using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using VersionCheck.API.Config;
using VersionCheck.Common;

namespace VersionCheck.API.VersionCheck
{
    public class VersionCheckService : IVersionCheckService
    {
        public VersionCheckService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public static readonly string ApiVersionString = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private readonly AppSettings _appSettings;

        public string MinimumClientVersion => _appSettings.MinimumSupportedClientVersion;

        public void PerformVersionCheck(HttpRequest request)
        {
            var minimumSupportedClientVersionString = _appSettings.MinimumSupportedClientVersion;

            if (request.Headers.TryGetValue(Common.HeaderKeys.ClientVersion, out StringValues clientVersionString) &&
                !string.IsNullOrWhiteSpace(minimumSupportedClientVersionString))
            {
                var clientVersion = new Version(clientVersionString.SingleOrDefault());
                var minimumSupportedAppVersion = new Version(minimumSupportedClientVersionString);
                if (clientVersion.CompareTo(minimumSupportedAppVersion) < 0)
                {
                    // min supported version is > client version
                    throw new ClientVersionNotSupportedException(minimumSupportedClientVersionString, ApiVersionString,
                        clientVersionString);
                }
            }
        }
    }
}
