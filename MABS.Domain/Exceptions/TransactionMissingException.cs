using System.Net;

namespace MABS.Domain.Exceptions
{
    public class TransactionMissingException : BaseException
    {
        public TransactionMissingException() => SetProperties();
        public TransactionMissingException(string message) : base(message) => SetProperties();
        public TransactionMissingException(string message, Exception inner) : base(message, inner) => SetProperties();
        public TransactionMissingException(string message, string debugMessage) : base(message, debugMessage) => SetProperties();

        private void SetProperties()
        {
            Title = "Missing transaction.";
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}