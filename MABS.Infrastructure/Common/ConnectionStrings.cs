namespace MABS.Infrastructure.Common
{
    public class ConnectionStrings
    {
        public const string SectionName = "ConnectionStrings";

        public string DefaultConnection { get; init; } = null!;
    }
}
