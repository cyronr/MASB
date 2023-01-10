using System.Net;

namespace MABS.Application.Common.Exceptions
{
    public class WrongPasswordException : BaseException
    {
        public WrongPasswordException() => SetProperties();
        public WrongPasswordException(string message) : base(message) => SetProperties();
        public WrongPasswordException(string message, Exception inner) : base(message, inner) => SetProperties();
        public WrongPasswordException(string message, string debugMessage) : base(message, debugMessage) => SetProperties();

        private void SetProperties()
        {
            Title = "Wrong password.";
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}