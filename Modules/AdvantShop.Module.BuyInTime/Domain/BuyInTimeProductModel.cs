using System;

namespace AdvantShop.Module.BuyInTime.Domain
{
    public class BuyInTimeProductModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateExpired { get; set; }

        public float DiscountInTime { get; set; }

        public string ActionText { get; set; }

        public bool IsRepeat { get; set; }

        public int DaysRepeat { get; set; }

        public int ShowMode { get; set; }

        public string Picture { get; set; }

        public int SortOrder { get; set; }

        public bool ShowInMobile { get; set; }

        public string MobileActionText { get; set; }
    }
}