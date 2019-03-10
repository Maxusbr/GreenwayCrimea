//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Payment
{
    [PaymentKey("Check")]
    public class Check : PaymentMethod
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string Fax { get; set; }
        public string IntPhone { get; set; }
        
        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                            {
                                {CheckTemplate.Address, Adress},
                                {CheckTemplate.City, City},
                                {CheckTemplate.CompanyName, CompanyName},
                                {CheckTemplate.Country, Country},
                                {CheckTemplate.Fax, Fax},
                                {CheckTemplate.IntPhone, IntPhone},
                                {CheckTemplate.Phone, Phone},
                                {CheckTemplate.State, State}
                            };
            }
            set
            {
                Adress = value.ElementOrDefault(CheckTemplate.Address);
                City = value.ElementOrDefault(CheckTemplate.City);
                CompanyName = value.ElementOrDefault(CheckTemplate.CompanyName);
                Country = value.ElementOrDefault(CheckTemplate.Country);
                Fax = value.ElementOrDefault(CheckTemplate.Fax);
                IntPhone = value.ElementOrDefault(CheckTemplate.IntPhone);
                Phone = value.ElementOrDefault(CheckTemplate.Phone);
                State = value.ElementOrDefault(CheckTemplate.State);
            }
        }
        
        public override string ProcessJavascriptButton(Orders.Order order)
        {
            //string[] companyAccount = {
            //                              order.PaymentDetails != null ? order.PaymentDetails.CompanyName : string.Empty,
            //                              order.PaymentDetails != null ? order.PaymentDetails.INN : string.Empty
            //                          };

            //string companyName = string.Empty;
            //string inn = string.Empty;
            //if (companyAccount.Length > 0)
            //    companyName = "&companyname=" + companyAccount[0];
            //if (companyAccount.Length > 1)
            //    inn = "&inn=" + companyAccount[1];
            return String.Format("javascript:window.open('paymentreceipt/check?ordernumber={0}');", order.Number);
        }

        public override string ButtonText
        {
            get { return LocalizationService.GetResource("Core.Payment.Check.PrintCheck"); }
        }
    }
}