using System.Linq;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Orders;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class GetLeadShippings
    {
        private readonly int _leadId;
        private readonly string _country;
        private readonly string _city;
        private readonly string _region;
        private readonly BaseShippingOption _shipping;
        private readonly bool _getAll;

        public GetLeadShippings(int leadId, string country, string city, string region,
                                BaseShippingOption shipping = null, bool getAll = true)
        {
            _leadId = leadId;
            _country = country;
            _city = city;
            _region = region;
            _shipping = shipping;
            _getAll = getAll;
        }

        public OrderShippingsModel Execute()
        {
            var lead = LeadService.GetLead(_leadId);
            if (lead == null)
                return null;

            var model = new OrderShippingsModel();

            if (lead.LeadItems == null || lead.LeadItems.Count == 0)
                return model;
            
            var preOrder = new PreOrder
            {
                Items = lead.LeadItems.Select(x => new PreOrderItem(x)).ToList(),
                CountryDest = _country ?? "",
                CityDest = _city ?? "",
                RegionDest = _region ?? "",
                Currency = lead.LeadCurrency ?? CurrencyService.CurrentCurrency,
                ShippingOption = _shipping,
                TotalDiscount = lead.GetTotalDiscount(lead.LeadCurrency)
            };

            var manager = new ShippingManager(preOrder);
            model.Shippings = manager.GetOptions(_getAll);

            model.CustomShipping = new BaseShippingOption()
            {
                Name = "",
                Rate = 0,
                IsCustom = true
            };

            return model;
        }
    }
}
