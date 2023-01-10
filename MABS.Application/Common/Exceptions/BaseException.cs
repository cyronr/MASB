using System.Net;

namespace MABS.Application.Common.Exceptions
{
    public class BaseException : Exception
    {
        public string Title { get; set; } = string.Empty;
        public int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;
        public string DebugMessage { get; set; } = string.Empty;

        public BaseException() { }
        public BaseException(string message) : base(message) { }
        public BaseException(string message, Exception inner) : base(message, inner) { }
        public BaseException(string message, string debugMessage) : base(message)
        {
            DebugMessage = debugMessage;
        }
    }
}