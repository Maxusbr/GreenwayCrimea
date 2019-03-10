
using System.Collections.Generic;
using System.Diagnostics;

namespace AdvantShop.Shipping.CheckoutRu
{

    public class CheckoutDeliveryCost
    {
        public string cost { get; set; }
        public string minDeliveryTerm { get; set; }
        public string maxDeliveryTerm { get; set; }
        public List<string> addresses { get; set; }
        public List<string> codes { get; set; }
        public List<string> costs { get; set; }
        public List<string> deliveries { get; set; }
        public List<string> minTerms { get; set; }
        public List<string> maxTerms { get; set; }
        public List<string> latiudes { get; set; }
        public List<string> longitudes { get; set; }
        public List<string> additionalInfo { get; set; }
        public string deliveryId { get; set; }
        public List<bool> npp { get; set; }
    }
}