using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services;

namespace AdvantShop.Admin.UserControls.Customer
{
    public partial class CustomerEmailCall : System.Web.UI.UserControl
    {
        public class CustomerEmailCallViewModel
        {
            public string NotifyType { get; set; }
            public DateTime EventTime { get; set; }
            public string Desc { get; set; }
            public int Duration { get; set; }
        }

        public Customers.Customer Customer { get; set; }
        protected List<CustomerEmailCallViewModel> ViewModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Customer.StandardPhone != null)
            {
                var telfin = new Telfin(SettingsTelephony.TelphinLogin, SettingsTelephony.TelphinSecretKey);
                var responce = telfin.GetReport(Customer.StandardPhone.ToString());
                if (responce.Exception != null)
                {
                    throw new Exception(responce.Exception.Message);
                }
                if (responce.CallList != null)
                {
                    ViewModel = responce.CallList.Select(x => new CustomerEmailCallViewModel
                    {
                        NotifyType = "Call",
                        EventTime = x.StartDate,
                        Desc = x.Disposition.ResourceKey(),
                        Duration = x.Duration
                    }).OrderByDescending(x => x.EventTime).ToList();
                }
                else ViewModel = new List<CustomerEmailCallViewModel>();
            }

        }
    }
}