using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VersionCheck.Common;

namespace VersionCheck
{
    public class VersionCheckHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await ProcessRequest(request, cancellationToken);
            var response = await base.SendAsync(request, cancellationToken);
            await ProcessResponse(response, cancellationToken);
            return response;
        }

        protected virtual Task<HttpRequestMessage> ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add(HeaderKeys.ClientVersion, Xamarin.Essentials.VersionTracking.CurrentVersion);
            return Task.FromResult(request);
        }

        protected virtual async Task<HttpResponseMessage> ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.BadRequest)
            {
                // check for ClientVersionNotSupportedException and throw
                var responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains(nameof(ClientVersionNotSupportedException)))
                {
                    var versionException = JsonConvert.DeserializeObject<ClientVersionNotSupportedException>(responseContent);
                    throw versionException;
                }
            }
            return response;
        }
    }
}
