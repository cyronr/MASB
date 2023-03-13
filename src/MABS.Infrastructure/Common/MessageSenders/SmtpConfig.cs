namespace MABS.Infrastructure.Common.MessageSenders
{
    public class SmtpConfig
    {
        public const string SectionName = "SmtpConfig";

        public string UserName { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string Host { get; init; } = null!;
        public int Port { get; init; }
        public bool EnableSsl { get; init; }
        public string SendFrom { get; init; }
    }
}
