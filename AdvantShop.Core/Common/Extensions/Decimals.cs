using System;

namespace AdvantShop
{
    public static class Decimals
    {
        public static string ToInvatiant(this decimal value)
        {
            var format = value < 1000
                ? value%1 == 0 ? "#0.##" : "#0.00"
                : value%1 == 0 ? "### ### ##0.##" : "### ### ##0.00";

            return value.ToString(format);
        }
    }
}
