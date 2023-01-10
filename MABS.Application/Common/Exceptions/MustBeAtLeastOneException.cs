using System.Net;

namespace MABS.Application.Common.Exceptions
{
    public class MustBeAtLeastOneException : BaseException
    {
        public MustBeAtLeastOneException() => SetProperties();
        public MustBeAtLeastOneException(string message) : base(message) => SetProperties();
        public MustBeAtLeastOneException(string message, Exception inner) : base(message, inner) => SetProperties();
        public MustBeAtLeastOneException(string message, string debugMessage) : base(message, debugMessage) => SetProperties();

        private void SetProperties()
        {
            Title = "Must be specified at least one.";
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}