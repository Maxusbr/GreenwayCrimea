using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Home;
using RestSharp;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetUserInformationModel
    {
        private const string Url = "http://modules.advantshop.net/";

        public GetUserInformationModel()
        {
        }

        public AdditionClientInfo Execute()
        {
            AdditionClientInfo model = null;

            try
            {
                var client = new RestClient(Url);
                var request = new RestRequest("shop/clientPropertyJson/" + SettingsLic.LicKey, Method.GET) {Timeout = 3000};

                var response = client.Execute<AdditionClientInfo>(request);
                model = response.Data;

                if (model != null)
                    model.Show = !IsValid(model);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return model;
        }

        public bool Save(AdditionClientInfo data)
        {
            if (!IsValid(data))
                return false;

            try
            {
                var client = new RestClient(Url);
                var request = new RestRequest("shop/setClientProperty/" + SettingsLic.LicKey, Method.POST) { Timeout = 3000 };
                request.AddJsonBody(data);

                var response = client.Execute(request);

                var customer = CustomerContext.CurrentCustomer;
                customer.FirstName = data.Name;
                customer.LastName = data.LastName;
                customer.Phone = data.Mobile;
                customer.StandardPhone = StringHelper.ConvertToStandardPhone(data.Mobile);

                CustomerService.UpdateCustomer(customer);

                Trial.TrialService.TrackEvent(Trial.ETrackEvent.Trial_FillUserData);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return true;
            }

            return true;
        }

        private bool IsValid(AdditionClientInfo data)
        {
            return 
                data != null &&

                !string.IsNullOrWhiteSpace(data.Name) &&
                !string.IsNullOrWhiteSpace(data.LastName) &&
                !string.IsNullOrWhiteSpace(data.CompanyName) &&
                !string.IsNullOrWhiteSpace(data.Mobile) &&

                data.Map != null && 
                data.Map.Count == 3 &&
                !data.Map.Any(x => string.IsNullOrWhiteSpace(x.Value));
        }
    }
}
