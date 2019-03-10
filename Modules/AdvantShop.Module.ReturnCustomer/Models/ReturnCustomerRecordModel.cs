using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Module.ReturnCustomer.Service;

namespace AdvantShop.Module.ReturnCustomer.Models
{
    public partial class ReturnCustomerRecordModel
    {
        public Guid CustomerID { get; set; }
        
        public string CustomerName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        
        public DateTime LastActionDate { get; set; }

        public string LastSendingDate { get; set; }

        public Dictionary<int, string> RecentlyView
        {
            get
            {
                var recentlyView = new Dictionary<int, string>();
                //var recentlyViewProducts = Customers.RecentlyViewService.LoadViewDataByCustomer(CustomerID, 10);
                var recentlyViewProductsIds = RCService.GetViewLastProductsByCustomer(CustomerID, 10);

                foreach (var productId in recentlyViewProductsIds)
                {
                    recentlyView.Add(
                        productId, 
                        string.Format("{0} - {1}",
                        RCService.GetProductArtNoById(productId),
                        RCService.GetRecentlyViewDateProduct(CustomerID, productId).ToString("dd.MM.yy HH:mm")));
                }

                return recentlyView;
            }
        }
    }
}
