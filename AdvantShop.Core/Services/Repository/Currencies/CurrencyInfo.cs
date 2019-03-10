//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Repository.Currencies
{
    [Serializable]
    public class CurrencyInfo
    {
        public CurrencyInfo(bool isLeftSymbol, string sformat, string smbl)
        {
            IsLeftSymbol = isLeftSymbol;
            Format = sformat;
            Symbol = smbl;
        }

        public bool IsLeftSymbol { get; set; }

        public string Format { get; set; }

        public string Symbol { get; set; }
    }

}
