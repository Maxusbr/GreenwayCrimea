using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Loging.TrafficSource;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Admin.Models.Leads;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class GetLead
    {
        private readonly int _id;

        public GetLead(int id)
        {
            _id = id;
        }

        public LeadModel Execute()
        {
            var model = new LeadModel() {Id = _id, Lead = LeadService.GetLead(_id)};

            var lead = model.Lead;

            if (lead == null)
                return model;

            if (LeadService.CheckAccess(lead) == false)
                return null;

            if (model.Statuses.Find(x => x.Value == lead.DealStatusId.ToString()) == null)
                lead.DealStatusId = 0;

            model.TrafficSource = OrderTrafficSourceService.Get(lead.Id, TrafficSourceType.Lead);

            if (lead.LeadItems != null)
            {
                var leadCurrency = lead.LeadCurrency != null
                    ? (Currency)lead.LeadCurrency
                    : CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);

                var itemsSum = lead.LeadItems.Sum(x => x.Price * x.Amount);
                var totalDiscount = lead.GetTotalDiscount(leadCurrency);

                var sum = itemsSum - totalDiscount;
                if (lead.Sum != sum && itemsSum > 0)
                {
                    lead.Sum = sum;
                    LeadService.UpdateLead(lead);
                }
            }

            if (lead.CustomerId != null)
            {
                model.VkUser = VkService.GetUser(lead.CustomerId.Value);
            }

            var orderId = OrderService.GetOrderIdByLeadId(lead.Id);
            var order = orderId != 0 ? OrderService.GetOrder(orderId) : null;
            if (order != null)
                model.Order = order;
            

            return model;
        }
    }
}
