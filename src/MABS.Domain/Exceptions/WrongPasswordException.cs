using System.Net;

namespace MABS.Domain.Exceptions
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
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}