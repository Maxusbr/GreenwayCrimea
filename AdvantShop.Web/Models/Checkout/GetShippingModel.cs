namespace AdvantShop.Models.Checkout
{
    public class GetShippingModel
    {
        public string ContactId { get; set; }

        public int CountryId { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public int Distance { get; set; }

        public int PickpointId { get; set; }

        public string PickpointAddress { get; set; }

        public string AdditionalData { get; set; }

        public GetShippingModel()
        {
            Region = string.Empty;
            City = string.Empty;
            Zip = string.Empty;
            PickpointAddress = string.Empty;
            AdditionalData = string.Empty;
        }
    }
}