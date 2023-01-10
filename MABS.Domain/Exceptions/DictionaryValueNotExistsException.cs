using System.Net;

namespace MABS.Domain.Exceptions
{
    public class DictionaryValueNotExistsException : BaseException
    {
        public DictionaryValueNotExistsException() => SetProperties();
        public DictionaryValueNotExistsException(string message) : base(message) => SetProperties();
        public DictionaryValueNotExistsException(string message, Exception inner) : base(message, inner) => SetProperties();
        public DictionaryValueNotExistsException(string message, string debugMessage) : base(message, debugMessage) => SetProperties();

        private void SetProperties()
        {
            Title = "Wrong dictionary value.";
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}