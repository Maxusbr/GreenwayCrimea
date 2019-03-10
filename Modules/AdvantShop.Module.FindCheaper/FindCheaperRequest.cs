//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Module.FindCheaper
{
    public class FindCheaperRequest
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public float WishPrice { get; set; }
        public string WhereCheaper { get; set; }
        
        public float Price { get; set; }
        public string OfferArtNo { get; set; }
        public string ProductName { get; set; }

        public bool IsProcessed { get; set; }
        public DateTime RequestDate { get; set; }
        public string ManagerComment { get; set; }
    }
}
