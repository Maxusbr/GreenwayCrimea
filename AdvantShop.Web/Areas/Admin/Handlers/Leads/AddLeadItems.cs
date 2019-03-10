using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class AddLeadItems
    {
        private readonly Lead _lead;
        private readonly List<int> _offerIds;

        public AddLeadItems(Lead lead, List<int> offerIds)
        {
            _lead = lead;
            _offerIds = offerIds;
        }

        public bool Execute()
        {
            var saveChanges = false;

            foreach (var offerId in _offerIds)
            {
                var offer = OfferService.GetOffer(offerId);
                if (offer == null)
                    continue;

                var product = offer.Product;

                var prodMinAmount = product.MinAmount == null
                            ? product.Multiplicity
                            : product.Multiplicity > product.MinAmount
                                ? product.Multiplicity
                                : product.MinAmount.Value;

                var item = new LeadItem
                {
                    Name = product.Name,
                    Price = PriceService.GetFinalPrice(offer.RoundedPrice, offer.Product.Discount),
                    ProductId = product.ProductId,
                    Amount = prodMinAmount,
                    ArtNo = offer.ArtNo,
                    Color = offer.Color != null ? offer.Color.ColorName : null,
                    Size = offer.Size != null ? offer.Size.SizeName : null,
                    PhotoId = offer.Photo != null ? offer.Photo.PhotoId : default(int),
                    Weight = product.Weight,
                    Width = product.Width,
                    Height = product.Height,
                    Length = product.Length
                };

                var oItem = _lead.LeadItems.Find(x => x == item);
                if (oItem != null)
                {
                    oItem.Amount += 1;
                    LeadService.UpdateLeadItem(_lead.Id, oItem);
                }
                else
                {
                    LeadService.AddLeadItem(_lead.Id, item);
                }
                saveChanges = true;
            }

            return saveChanges;
        }
    }
}
