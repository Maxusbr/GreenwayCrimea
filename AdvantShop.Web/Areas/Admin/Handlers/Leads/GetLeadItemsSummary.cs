using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Admin.Models.OrdersEdit;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class GetLeadItemsSummary
    {
        private readonly int _leadId;

        public GetLeadItemsSummary(int leadId)
        {
            _leadId = leadId;
        }

        public LeadItemsSummaryModel Execute()
        {
            var lead = _leadId != 0 ? LeadService.GetLead(_leadId) : null;

            var leadCurrency =
                lead != null && lead.LeadCurrency != null
                    ? (Currency) lead.LeadCurrency
                    : CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);

            if (lead == null)
                return new LeadItemsSummaryModel() {LeadCurrency = leadCurrency};

            var itemsSum = lead.LeadItems.Sum(x => x.Price*x.Amount);
            var totalDiscount = lead.GetTotalDiscount(leadCurrency);

            var sum = itemsSum - totalDiscount + lead.ShippingCost;

            var model = new LeadItemsSummaryModel
            {
                LeadCurrency = leadCurrency,
                LeadItemsCount = lead.LeadItems.Count,

                ProductsCost = itemsSum,

                ProductsDiscountPrice = totalDiscount,
                Discount = lead.Discount,
                DiscountValue = lead.DiscountValue,

                ShippingName = lead.ShippingName,
                ShippingCost = lead.ShippingCost,
                ShippingPickPoint =
                    !string.IsNullOrEmpty(lead.ShippingPickPoint)
                        ? JsonConvert.DeserializeObject<OrderPickPoint>(lead.ShippingPickPoint)
                        : null,
                DeliveryDate = lead.DeliveryDate != null ? lead.DeliveryDate.Value.ToShortDateString() : null,
                DeliveryTime = lead.DeliveryTime,

                Sum = sum > 0 ? sum : 0,
                
            };

            if (itemsSum != 0 && lead.Sum != model.Sum)
            {
                lead.Sum = model.Sum;
                LeadService.UpdateLead(lead);
            }

            if (lead.Customer != null && lead.Customer.Contacts != null && lead.Customer.Contacts.Count > 0)
            {
                model.Country = lead.Customer.Contacts[0].Country;
                model.Region = lead.Customer.Contacts[0].Region;
                model.City = lead.Customer.Contacts[0].City;
            }

            return model;
        }
    }
}
