namespace MABS.Domain.Extensions
{
    public static class CharExtensions
    {
        public static Int32 ToInt32(this Char character)
        {
            return (Int32)Char.GetNumericValue(character);
        }
    }
}
