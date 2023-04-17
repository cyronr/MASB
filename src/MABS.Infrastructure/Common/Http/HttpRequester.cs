using MABS.Application.Common.Http;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.Common.Http
{
    public class HttpRequester : IHttpRequester
    {
        private readonly ILogger<HttpRequester> _logger;

        static readonly HttpClient client = new HttpClient();
        
        public HttpRequester(ILogger<HttpRequester> logger)
        {
            _logger = logger;
        }


        public async Task<HttpResponseMessage> HttpGet(string url)
        {
            _logger.LogInformation($"Sending a HTTP Get request ({url}).");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    await LogHTTPResponse(response);
                    return response;
                }  
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e.Message);
                return new HttpResponseMessage();
            }
        }

        public async Task<HttpResponseMessage> HttpGet(string url, Dictionary<string, string> headers)
        {
            _logger.LogInformation($"Sending a HTTP Get request ({url}).");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }

                    HttpResponseMessage response = await client.SendAsync(request);
                    await LogHTTPResponse(response);
                    return response;
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogWarning(e.Message);
                return new HttpResponseMessage();
            }
        }

        private async Task LogHTTPResponse(HttpResponseMessage response)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
                return;

            _logger.LogDebug($"Request returned response status {response.StatusCode} and body {await response.Content.ReadAsStringAsync()}");
         
        }
    }
}
