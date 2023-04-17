using System.Net;

namespace MABS.Domain.Exceptions
{
    public class WrongAddressException : BaseException
    {
        public WrongAddressException() => SetProperties();
        public WrongAddressException(string message) : base(message) => SetProperties();
        public WrongAddressException(string message, Exception inner) : base(message, inner) => SetProperties();
        public WrongAddressException(string message, string debugMessage) : base(message, debugMessage) => SetProperties();

        private void SetProperties()
        {
            Title = "Wrong address.";
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}