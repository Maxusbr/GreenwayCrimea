using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Module.SimaLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class AdvOfferService
    {
        public static int UpdateOrInsertOffer(int advProductId, SimalandProduct slProduct)
        {
            try
            {
                var offer = OfferService.GetOffer(slProduct.id.ToString());
                if (offer != null)
                {
                    offer.BasePrice = 0;
                    offer.SupplyPrice = slProduct.price;
                    offer.Amount = slProduct.balance.TryParseInteger() == 0 ? 1000 : slProduct.balance.TryParseInteger();
                    OfferService.UpdateOffer(offer);
                    return 0;
                }
                offer = new Offer();
                offer.ArtNo = slProduct.sid.ToString();
                offer.Main = true;
                offer.ProductId = advProductId;
                offer.BasePrice = 0;
                offer.SupplyPrice = slProduct.price;
                offer.Amount = slProduct.balance.TryParseInteger() == 0 ? 1000 : slProduct.balance.TryParseInteger();
                return OfferService.AddOffer(offer);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return -1;
            }
            
        }
    }
}
