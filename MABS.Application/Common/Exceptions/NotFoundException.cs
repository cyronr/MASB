using System.Net;

namespace MABS.Application.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException() => SetProperties();
        public NotFoundException(string message) : base(message) => SetProperties();
        public NotFoundException(string message, Exception inner) : base(message, inner) => SetProperties();
        public NotFoundException(string message, string debugMessage) : base(message, debugMessage) => SetProperties();

        private void SetProperties()
        {
            Title = "Record not found.";
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}