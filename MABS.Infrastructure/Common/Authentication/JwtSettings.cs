namespace MABS.Infrastructure.Common.Authentication
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Issuer { get; init; } = null!;
        public int ExpiryMinutes { get; init; }
        public string Key { get; init; } = null!;
    }
}
