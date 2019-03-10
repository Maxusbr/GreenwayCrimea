//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Repository.Currencies 
{
    [Serializable]
    public class Currency
    {
        public int CurrencyId { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public float Rate { get; set; }

        public string Iso3 { get; set; }

        public int NumIso3 { get; set; }

        public bool IsCodeBefore { get; set; }

        public float RoundNumbers { get; set; }

        public bool EnablePriceRounding { get; set; }
    }

}