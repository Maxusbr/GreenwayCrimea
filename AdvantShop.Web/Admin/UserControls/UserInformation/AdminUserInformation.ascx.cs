using AdvantShop.Configuration;
using AdvantShop.Customers;
using RestSharp;
using System;
using System.Collections.Generic;

namespace AdvantShop.Admin.UserControls.UserInformation
{
    public class AdditionClientInfo
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string CompanyName { get; set; }
        public List<PropertyMap> Map { get; set; }
    }

    public class PropertyMap
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public partial class AdminUserInformation : System.Web.UI.UserControl
    {
        protected AdditionClientInfo Model;
        const string Url = "http://modules.advantshop.net/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Trial.TrialService.IsTrialEnabled || CustomerContext.CurrentCustomer.IsVirtual)
            {
                this.Visible = false;
                return;
            }

            var client = new RestClient(Url);
            var request = new RestRequest(string.Format("Shop/ClientPropertyJson/{0}", SettingsLic.LicKey), Method.GET);
            request.Timeout = 3000;

            var model = client.Execute<AdditionClientInfo>(request);
            if (model.Data != null)
            {
                Model = model.Data;
            }

            if (Model == null)
            {
                this.Visible = false;
            }
        }       
    }
}