using System.Text.Json;

namespace MABS.Application.Common.Middleware.Exceptions
{
    public class ErrorDetails
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string traceId { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}                