//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("AlfabankUa")]
    public class AlfabankUa : PaymentMethod, ICreditPaymentMethod
    {
        public string PartnerId { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public float MinimumPrice { get; set; }
        public float FirstPayment { get; set; }

        public override Dictionary<string,string > Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {AlfabankUaTemplate.PartnerId, PartnerId},
                    {AlfabankUaTemplate.MinimumPrice, MinimumPrice.ToString()},
                    {AlfabankUaTemplate.FirstPayment, FirstPayment.ToString()}
                };
            }
            set
            {
                PartnerId = value.ElementOrDefault(AlfabankUaTemplate.PartnerId);
                MinimumPrice = value.ElementOrDefault(AlfabankUaTemplate.MinimumPrice).TryParseFloat();
                FirstPayment = value.ElementOrDefault(AlfabankUaTemplate.FirstPayment).TryParseFloat();
            }
        }

        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.GET,
                Url = "http://www.alfabank.ua/credit/bpk/index.php" ,
                InputValues = new Dictionary<string, string>
                {
                    {"partner", PartnerId},
                    {"surname", order.OrderCustomer.LastName},
                    {"name", order.OrderCustomer.FirstName},
                    {"midname", ""},
                    {"phone", (order.OrderCustomer.StandardPhone.HasValue ? order.OrderCustomer.StandardPhone.ToString() : string.Empty)},
                    {"email", order.OrderCustomer.Email},
                    {"product", string.Join(",", order.OrderItems.Select(item => HttpUtility.HtmlEncode(item.Name)))}
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.GET,
                Url = "http://www.alfabank.ua/credit/bpk/index.php",
                InputValues = new Dictionary<string, string>
                {
                    {"partner", PartnerId},
                    {"surname", order.OrderCustomer.LastName},
                    {"name", order.OrderCustomer.FirstName},
                    {"midname", ""},
                    {"phone", (order.OrderCustomer.StandardPhone.HasValue ? order.OrderCustomer.StandardPhone.ToString() : string.Empty)},
                    {"email", order.OrderCustomer.Email},
                    {"product", string.Join(",", order.OrderItems.Select(item => HttpUtility.HtmlEncode(item.Name)))}
                }
            }.ProcessRequest();
        }
    }
}