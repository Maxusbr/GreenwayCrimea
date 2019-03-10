using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class CheckControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                bool validateForm = ValidateFormData(new[] {  txtCompanyName,
                    txtPhone,
                    txtCountry,
                    txtState,
                    txtCity,
                    txtAddress,
                    txtFax,
                    txtIntPhone }, null,null);


                if (_valid || validateForm)
                {
                    return new Dictionary<string, string>
                        {
                            {CheckTemplate.CompanyName, txtCompanyName.Text},
                            {CheckTemplate.Phone, txtPhone.Text},
                            {CheckTemplate.Country, txtCountry.Text},
                            {CheckTemplate.State, txtState.Text},
                            {CheckTemplate.City, txtCity.Text},
                            {CheckTemplate.Address, txtAddress.Text},
                            {CheckTemplate.Fax, txtFax.Text},
                            {CheckTemplate.IntPhone, txtIntPhone.Text}
                        };
                }

                return null;
            }
            set
            {
                txtCompanyName.Text = value.ElementOrDefault(CheckTemplate.CompanyName);
                txtPhone.Text = value.ElementOrDefault(CheckTemplate.Phone);
                txtCountry.Text = value.ElementOrDefault(CheckTemplate.Country);
                txtState.Text = value.ElementOrDefault(CheckTemplate.State);
                txtCity.Text = value.ElementOrDefault(CheckTemplate.City);
                txtAddress.Text = value.ElementOrDefault(CheckTemplate.Address);
                txtFax.Text = value.ElementOrDefault(CheckTemplate.Fax);
                txtIntPhone.Text = value.ElementOrDefault(CheckTemplate.IntPhone);
            }
        }
   
    }
}