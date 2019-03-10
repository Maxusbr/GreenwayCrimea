
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Modules.RetailCRM.Models
{
     public class PaymentTypeResponse : ResponseSimple
    {
        [JsonConverter(typeof(RetailDictionaryConverter<PaymentType>))]
         public List<PaymentType> paymentTypes { get; set; }
    }


    public class EditPaymentType
    {
        public PaymentType paymentType { get; set; }
    }

    public class PaymentType : BaseRetailDictionary
    {
        public string name { get; set; }
        public bool defaultForApi { get; set; }
    }
}