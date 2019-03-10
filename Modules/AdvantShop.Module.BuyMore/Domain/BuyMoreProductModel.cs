using System;
using System.Collections.Generic;

namespace AdvantShop.Module.BuyMore.Domain
{
    public class BuyMoreProductModel
    {
        public int Id { get; set; }

        public float OrderPriceFrom { get; set; }
        
        public string GiftOffersIds { get; set; }

        //public float OrderDiscount { get; set; }

        public bool FreeShipping { get; set; }

        public List<int> GiftOffersIdsList
        {
            get
            {
                var result = new List<int>();
                if (!string.IsNullOrEmpty(GiftOffersIds))
                {
                    foreach (var giftOfferId in GiftOffersIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var id = 0;
                        if (int.TryParse(giftOfferId, out id))
                        {
                            result.Add(id);
                        }
                    }
                }
                return result;
            }
        }

    }
}