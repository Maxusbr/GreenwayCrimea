using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Core.Services.Payment
{
    public class PaymentMethodAdminModel
    {
        public PaymentMethodAdminModel()
        {
            Parameters = new Dictionary<string, string>();
        }

        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        
        public ExtrachargeType ExtrachargeType { get; set; }

        public List<SelectListItem> ExtrachargeTypes
        {
            get
            {
                var extrachargeTypes = new List<SelectListItem>();

                foreach (ExtrachargeType type in Enum.GetValues(typeof(ExtrachargeType)))
                {
                    extrachargeTypes.Add(new SelectListItem()
                    {
                        Text = type.Localize(),
                        Value = type.ToString(),
                        Selected = type == ExtrachargeType
                    });
                }
                return extrachargeTypes;
            }
        }

        public float Extracharge { get; set; }


        public string PaymentKey { get; set; }
        public string PaymentTypeLocalized
        {
            get
            {
                var list = AdvantshopConfigService.GetDropdownPayments();
                var type = list.FirstOrDefault(x => x.Value.ToLower() == PaymentKey.ToLower());
                return type != null ? type.Text : PaymentKey;
            }
        }

        public int CurrencyId { get; set; }

        public List<SelectListItem> Currencies
        {
            get
            {
                var currencies = new List<SelectListItem>();
                var currentCurrency = CurrencyService.CurrentCurrency;

                foreach (var currency in CurrencyService.GetAllCurrencies().OrderBy(x => x.CurrencyId != currentCurrency.CurrencyId))
                {
                    currencies.Add(new SelectListItem() {Text = currency.Name, Value = currency.CurrencyId.ToString()});
                }

                var selected = currencies.Find(x => x.Value == CurrencyId.ToString());
                if (selected != null)
                {
                    selected.Selected = true;
                }
                else
                {
                    currencies[0].Selected = true;
                    CurrencyId = currencies[0].Value.TryParseInt();
                }

                return currencies;
            }
        }
        
        public string Icon { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public virtual string ModelType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        public virtual string PaymentViewPath
        {
            get { return "_" + PaymentKey; }
        }

        public virtual Tuple<string, string> Instruction { get { return null;} }


        public ProcessType ProcessType { get; set; }
        public NotificationType NotificationType { get; set; }
        public UrlStatus ShowUrls { get; set; }

        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
        public string FailUrl { get; set; }
        public string NotificationUrl { get; set; }

        public bool ShowCurrency { get; set; }

    }
}
