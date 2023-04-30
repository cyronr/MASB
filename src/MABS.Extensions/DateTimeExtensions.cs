namespace MABS.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfDay(this DateTime date)
        {
            return date.Date + TimeSpan.Zero;
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.AddDays(1).Date.AddTicks(-1);
        }
    }
}
