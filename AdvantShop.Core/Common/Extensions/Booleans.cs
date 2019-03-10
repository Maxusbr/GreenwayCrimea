namespace AdvantShop.Core.Common.Extensions
{
    public static class Booleans
    {
        public static string ToLowerString(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}
