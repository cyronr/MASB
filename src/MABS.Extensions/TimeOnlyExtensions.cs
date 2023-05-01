namespace MABS.Extensions
{
    public static class TimeOnlyExtensions
    {
        public static TimeOnly StripSeconds(this TimeOnly time)
        {
            return time.Add(-TimeSpan.FromSeconds(time.Second));
        }
    }
}
