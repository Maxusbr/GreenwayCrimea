using System;
using AdvantShop.Module.ReturnCustomer.Service;
using Newtonsoft.Json;

namespace AdvantShop.Module.ReturnCustomer.Models
{
    public class ReturnCustomerRecord
    {
        [JsonIgnore]
        public Guid CustomerID { get; set; }

        [JsonIgnore]
        public DateTime LastActionDate { get; set; }

        [JsonIgnore]
        public DateTime ExpirationDate { get; set; }

        public string ExpirationDateString
        {
            get { return ExpirationDate.ToString("dd.MM.yy HH:mm"); }
            set {  }
        }

        [JsonIgnore]
        public bool? IsNotNeedChecked { get; set; }

        public string IsNotNeedCheckedString {
            get { if (!IsNotNeedChecked.HasValue) { return "Не отправлено"; } else { return IsNotNeedChecked.Value ? "Отправлено" : "Не отправлено"; } }
            set {  }
        }

        public string Email
        {
            get
            {
                return RCService.GetEmailByCustomerID(CustomerID);
            }
            set
            {
                Email = value;
            }
        }

        public bool WaitingVisit { get; set; }

        public string LastSendingDate { get; set; }
    }
}
