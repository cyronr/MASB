using Azure;
using MABS.Application.Common.Http;
using Microsoft.Extensions.Logging;
using System.Net;

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
                HttpResponseMessage response = await client.GetAsync(url);
                await LogHTTPResponse(response);
                
                return response;

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
