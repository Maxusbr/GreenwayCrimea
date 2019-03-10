//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Taxes
{
    public class TaxElement
    {
        public int TaxId { get; set; }
        public string Name { get; set; }
        public bool  Enabled { get; set; }
        public float Rate { get; set; }
        public bool ShowInPrice { get; set; }
        public TaxType TaxType { get; set; }

        public TaxElement()
        {
            ShowInPrice = true;
        }
    }
    
    public enum TaxType
    {
        /// <summary>
        /// Другой
        /// </summary>
        [Localize("Другой")]
        Other = 0,

        /// <summary>
        /// Без НДС
        /// </summary>
        [Localize("Без НДС")]
        Without = 1,

        /// <summary>
        /// НДС по ставке 0%
        /// </summary>
        [Localize("НДС по ставке 0%")]
        Zero = 2,

        /// <summary>
        /// НДС по ставке 10%
        /// </summary>
        [Localize("НДС по ставке 10%")]
        Ten = 3,

        /// <summary>
        /// НДС по ставке 18%
        /// </summary>
        [Localize("НДС по ставке 18%")]
        Eighteen = 4,
    }
}