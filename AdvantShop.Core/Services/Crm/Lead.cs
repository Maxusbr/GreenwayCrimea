using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Core.Services.Crm
{
    public class Lead : IBizObject
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public float Sum { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        
        public Guid? CustomerId { get; set; }

        private Customer _customer = null;
        public Customer Customer
        {
            get
            {
                if (_customer != null)
                    return _customer;

                _customer = CustomerId != null ? CustomerService.GetCustomer(CustomerId.Value) : null;

                return _customer;
            }
            set { _customer = value; }
        }

        [Obsolete]
        public LeadStatus LeadStatus { get; set; }

        private DealStatus _dealStatus;

        public DealStatus DealStatus
        {
            get { return _dealStatus ?? (_dealStatus = DealStatusService.Get(DealStatusId)); }
        }

        public int DealStatusId { get; set; }

        public string Comment { get; set; }

        //[Obsolete]
        //public string AdminComment { get; set; }

        public int? ManagerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public float Discount { get; set; }
        public float DiscountValue { get; set; }

        public int OrderSourceId { get; set; }

        public bool IsFromAdminArea { get; set; }

        private List<LeadItem> _leadItems;
        public List<LeadItem> LeadItems
        {
            get
            {
                return Id != 0 || _leadItems != null
                    ? _leadItems ?? (_leadItems = LeadService.GetLeadItems(Id))
                    : (_leadItems = new List<LeadItem>());
            }
            set { _leadItems = value; }
        }

        private LeadCurrency _leadCurrency;
        public LeadCurrency LeadCurrency
        {
            get
            {
                return Id != 0 || _leadCurrency != null
                    ? _leadCurrency ?? (_leadCurrency = LeadService.GetLeadCurrency(Id))
                    : _leadCurrency;
            }
            set { _leadCurrency = value; }
        }

        public DateTime? DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public int ShippingMethodId { get; set; }
        public string ShippingName { get; set; }
        public float ShippingCost { get; set; }
        public string ShippingPickPoint { get; set; }
    }

    public enum LeadStatus
    {
        [Localize("Core.Crm.LeadStatus.New")]
        [Color("#ff7f00")]
        New = 1,

        [Localize("Core.Crm.LeadStatus.Processing")]
        [Color("#77ff77")]
        Processing = 2,

        [Localize("Core.Crm.LeadStatus.ClosedDeal")]
        [Color("#ff9bb9")]
        ClosedDeal = 3,

        [Localize("Core.Crm.LeadStatus.NotClosedDeal")]
        [Color("#58bae9")]
        NotClosedDeal = 4
    }

    public class LeadCurrency
    {
        public string CurrencyCode { get; set; }
        public int CurrencyNumCode { get; set; }
        public float CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsCodeBefore { get; set; }
        public float RoundNumbers { get; set; }
        public bool EnablePriceRounding { get; set; }

        public static implicit operator LeadCurrency(Currency cur)
        {
            return new LeadCurrency
            {
                CurrencyCode = cur.Iso3,
                CurrencyNumCode = cur.NumIso3,
                CurrencyValue = cur.Rate,
                CurrencySymbol = cur.Symbol,
                IsCodeBefore = cur.IsCodeBefore,
                EnablePriceRounding = cur.EnablePriceRounding,
                RoundNumbers = cur.RoundNumbers
            };
        }

        public static implicit operator Currency(LeadCurrency cur)
        {
            return new Currency
            {
                Iso3 = cur.CurrencyCode,
                NumIso3 = cur.CurrencyNumCode,
                Rate = cur.CurrencyValue,
                Symbol = cur.CurrencySymbol,
                IsCodeBefore = cur.IsCodeBefore,
                EnablePriceRounding = cur.EnablePriceRounding,
                RoundNumbers = cur.RoundNumbers
            };
        }
    }

    public static class LeadExtensions
    {
        public static float GetTotalDiscount(this Lead lead, Currency leadCurrency)
        {
            var totalDiscount = lead.Discount > 0
                ? (lead.Discount*lead.LeadItems.Sum(x => x.Price*x.Amount)/100).RoundPrice(leadCurrency)
                : 0;

            totalDiscount += lead.DiscountValue;

            return totalDiscount;
        }
    }
}
