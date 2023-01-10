using System.Net;

namespace MABS.Domain.Exceptions
{
    public class WrongTaxIdentificationNumberException : BaseException
    {
        public WrongTaxIdentificationNumberException() => SetProperties();
        public WrongTaxIdentificationNumberException(string message) : base(message) => SetProperties();
        public WrongTaxIdentificationNumberException(string message, Exception inner) : base(message, inner) => SetProperties();
        public WrongTaxIdentificationNumberException(string message, string debugMessage) : base(message, debugMessage) => SetProperties();

        private void SetProperties()
        {
            Title = "Wrong Tax Identification Number.";
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}