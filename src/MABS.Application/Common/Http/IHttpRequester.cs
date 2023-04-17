namespace MABS.Application.Common.Http
{
    public interface IHttpRequester
    {
        Task<HttpResponseMessage> HttpGet(string url);
        Task<HttpResponseMessage> HttpGet(string url, Dictionary<string, string> headers);
    }
}
