//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Payment
{
    public interface IPayment
    {
        ProcessType ProcessType { get; }
        NotificationType NotificationType { get; }
        UrlStatus ShowUrls { get; }
        string SuccessUrl { get; }
        string CancelUrl { get; }
        string FailUrl { get; }
        string NotificationUrl { get; }
        int PaymentMethodId { get; }
        void ProcessForm(Order order);
        string ProcessFormString(Order order, PageWithPaymentButton buttonSize);
        string ProcessJavascript(Order order);
        string ProcessJavascriptButton(Order order);
        string ProcessServerRequest(Order order);
        string ProcessResponse(HttpContext context);

        BasePaymentOption GetOption(BaseShippingOption shippingOption, float preCoast);
    }


    [Serializable]
    public abstract class PaymentMethod : IPayment
    {
        //public common settings
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        public ExtrachargeType ExtrachargeType { get; set; }
        public float Extracharge { get; set; }

        public float GetExtracharge(Order order)
        {
            if (ExtrachargeType == ExtrachargeType.Fixed)
                return Extracharge;

            return Extracharge *
                   (order.OrderItems.Sum(x => x.Price * x.Amount) - order.TotalDiscount - order.BonusCost +
                    order.ShippingCost + order.Taxes.Where(x => !x.ShowInPrice).Sum(x => x.Sum)) / 100;
        }

        private int _currencyId;
        public int CurrencyId
        {
            get
            {
                if (_currencyId != 0)
                    return _currencyId;

                var currency =
                    CurrencyService.GetAllCurrencies(true)
                        .FirstOrDefault(x => x.Iso3 == SettingsCatalog.DefaultCurrencyIso3);

                return (_currencyId = currency != null ? currency.CurrencyId : 0);
            }
            set { _currencyId = value; }
        }

        private Currency _paymentCurrency;
        public Currency PaymentCurrency
        {
            get
            {
                if (_paymentCurrency != null)
                    return _paymentCurrency;

                return
                    (_paymentCurrency =
                        CurrencyService.GetAllCurrencies(true).FirstOrDefault(x => x.CurrencyId == CurrencyId));
            }
        }

        public float GetCurrencyRate(OrderCurrency orderCurrency)
        {
            if (PaymentCurrency == null)
                return orderCurrency.CurrencyValue;

            return orderCurrency.CurrencyValue / PaymentCurrency.Rate;
        }

        private Photo _picture;
        public Photo IconFileName
        {
            get { return _picture ?? (_picture = PhotoService.GetPhotoByObjId(PaymentMethodId, PhotoType.Payment)); }
            set { _picture = value; }
        }

        private Dictionary<string, string> _parameters = new Dictionary<string, string>();
        public virtual Dictionary<string, string> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public string PaymentKey
        {
            get { return AttributeHelper.GetAttributeValue<PaymentKeyAttribute, string>(this); }
        }

        //public processing methods
        public virtual IEnumerable<string> ShippingKeys
        {
            get { return null; }
        }

        public virtual ProcessType ProcessType
        {
            get { return ProcessType.None; }
        }

        public virtual NotificationType NotificationType
        {
            get { return NotificationType.None; }
        }

        public virtual UrlStatus ShowUrls { get { return UrlStatus.None; } }

        public virtual string SuccessUrl
        {
            get
            {
                return string.Format("http://{0}/paymentreturnurl/{1}", SettingsMain.SiteUrl.ToLower().Replace("http://", "").Replace("https://", ""), PaymentMethodId);
            }
        }
        public virtual string CancelUrl
        {
            get
            {
                return string.Format("http://{0}/cancel", SettingsMain.SiteUrl.ToLower().Replace("http://", "").Replace("https://", ""));
            }
        }
        public virtual string FailUrl
        {
            get
            {
                return string.Format("http://{0}/fail", SettingsMain.SiteUrl.ToLower().Replace("http://", "").Replace("https://", ""));
            }
        }
        public virtual string NotificationUrl
        {
            get
            {
                return string.Format("http://{0}/paymentnotification/{1}", SettingsMain.SiteUrl.ToLower().Replace("http://", "").Replace("https://", ""), PaymentMethodId);
            }
        }

        public virtual string ButtonText
        {
            get { return LocalizationService.GetResource("Core.Payment.PaymentMethod.DefaultButtonText"); }
        }


        public virtual void ProcessForm(Order order)
        {

        }

        public virtual string ProcessFormString(Order order, PageWithPaymentButton buttonSize)
        {
            return string.Empty;
        }

        public virtual string ProcessJavascript(Order order)
        {
            return string.Empty;
        }

        public virtual string ProcessJavascriptButton(Order order)
        {
            return string.Empty;
        }

        public virtual string ProcessServerRequest(Order order)
        {
            throw new NotImplementedException();
        }

        public virtual string ProcessResponse(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public virtual BasePaymentOption GetOption(BaseShippingOption shippingOption, float preCoast)
        {
            var option = new BasePaymentOption(this, preCoast);
            return option;
        }        

        public virtual Dictionary<string,string> GetDefaultParameters()
        {
            return null;
        }

        public static string GetOrderDescription(string orderNumber)
        {
            return string.Format(LocalizationService.GetResource("Core.Payment.OrderDescription"), orderNumber);
        }

        public string Process(Order order, PageWithPaymentButton page)
        {
            if (this is ICreditPaymentMethod && (this as ICreditPaymentMethod).MinimumPrice >= order.Sum)
            {
                return string.Empty;
            }

            if (ProcessType == ProcessType.FormPost)
            {
                return ProcessFormString(order, page);
            }

            if (ProcessType == ProcessType.ServerRequest)
            {
                var href = ProcessServerRequest(order);
                return page == PageWithPaymentButton.myaccount
                    ? Button.RenderHtml(LocalizationService.GetResource("Core.Payment.PaymentMethod.ButtonTextPayOrder"), eType.Confirm, eSize.Small, href: href)
                    : Button.RenderHtml(LocalizationService.GetResource("Core.Payment.PaymentMethod.ButtonTextPayOrder"), eType.Submit, eSize.Middle, href: href);
            }

            if (ProcessType == ProcessType.Javascript)
            {
                var buttonText = (this is ICreditPaymentMethod)
                        ? LocalizationService.GetResource("Core.Payment.PaymentMethod.ButtonTextCredit")
                        : ButtonText;

                if (page == PageWithPaymentButton.myaccount)
                {
                    return ProcessJavascript(order) + " " +
                           Button.RenderHtml(buttonText, eType.Confirm, eSize.Small,
                               onClientClick: ProcessJavascriptButton(order));
                }
                return ProcessJavascript(order) + " " +
                       Button.RenderHtml(buttonText, eType.Submit, eSize.Middle,
                           onClientClick: ProcessJavascriptButton(order));
            }
            return string.Empty;
        }

        public virtual PaymentDetails PaymentDetails()
        {
            return null;
        }


        public static PaymentMethod Create(string paymentKey)
        {
            var type = ReflectionExt.GetTypeByAttributeValue<PaymentKeyAttribute>(typeof(IPayment), atr => atr.Value, paymentKey);
            return type != null ? (PaymentMethod)Activator.CreateInstance(type) : null;
        }
    }
}