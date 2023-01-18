namespace MABS.Application.Common.Http
{
    public interface IHttpRequester
    {
        Task<HttpResponseMessage> HttpGet(string url);
    }
}
