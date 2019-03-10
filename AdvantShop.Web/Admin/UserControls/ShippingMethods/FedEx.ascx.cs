using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.FedEx;

namespace Admin.UserControls.ShippingMethods
{
    public partial class FedExControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[]
                    {
                        txtCountryCode,
                        txtPostalCode,
                        txtState,
                        txtCity,
                        txtAddress,
                        txtAccountNumber,
                        txtMeterNumber,
                        txtRate,
                        txtExtracharge,
                        txtKey,
                        txtPassword
                    },
                                                  new[] {txtRate, txtExtracharge},
                                                  new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {FedExTemplate.CountryCode, txtCountryCode.Text},
                                   {FedExTemplate.PostalCode, txtPostalCode.Text},
                                   {FedExTemplate.State, txtState.Text},
                                   {FedExTemplate.City, txtCity.Text},
                                   {FedExTemplate.Address, txtAddress.Text},
                                   {FedExTemplate.AccountNumber, txtAccountNumber.Text},
                                   {FedExTemplate.MeterNumber, txtMeterNumber.Text},
                                   {FedExTemplate.Rate, txtRate.Text},
                                   {FedExTemplate.Extracharge, txtExtracharge.Text},
                                   {FedExTemplate.Key, txtKey.Text},
                                   {FedExTemplate.Password, txtPassword.Text},
                                   {FedExTemplate.EuropeFirstInternationalPriority,chkEuropeFirstInternationalPriority.Checked.ToString()},
                                   {FedExTemplate.Fedex1DayFreight, chkFedex1DayFreight.Checked.ToString()},
                                   {FedExTemplate.Fedex2Day, chkFedex2Day.Checked.ToString()},
                                   {FedExTemplate.Fedex2DayFreight, chkFedex2DayFreight.Checked.ToString()},
                                   {FedExTemplate.Fedex3DayFreight, chkFedex3DayFreight.Checked.ToString()},
                                   {FedExTemplate.FedexExpressSaver, chkFedexExpressSaver.Checked.ToString()},
                                   {FedExTemplate.FedexGround, chkFedexGround.Checked.ToString()},
                                   {FedExTemplate.FirstOvernight, chkFirstOvernight.Checked.ToString()},
                                   {FedExTemplate.GroundHomeDelivery, chkGroundHomeDelivery.Checked.ToString()},
                                   {FedExTemplate.InternationalDistributionFreight,chkInternationalDistributionFreight.Checked.ToString()},
                                   {FedExTemplate.InternationalEconomy, chkInternationalEconomy.Checked.ToString()},
                                   {FedExTemplate.InternationalEconomyDistribution,chkInternationalEconomyDistribution.Checked.ToString()},
                                   {FedExTemplate.InternationalEconomyFreight,chkInternationalEconomyFreight.Checked.ToString()},
                                   {FedExTemplate.InternationalFirst, chkInternationalFirst.Checked.ToString()},
                                   {FedExTemplate.InternationalPriority, chkInternationalPriority.Checked.ToString()},
                                   {FedExTemplate.InternationalPriorityFreight,chkInternationalPriorityFreight.Checked.ToString()},
                                   {FedExTemplate.PriorityOvernight, chkPriorityOvernight.Checked.ToString()},
                                   {FedExTemplate.SmartPost, chkSmartPost.Checked.ToString()},
                                   {FedExTemplate.StandardOvernight, chkStandardOvernight.Checked.ToString()},
                                   {FedExTemplate.FedexFreight, chkFedexFreight.Checked.ToString()},
                                   {FedExTemplate.FedexNationalFreight, chkFedexNationalFreight.Checked.ToString()},
                               }
                           : null;
            }
            set
            {

                txtCountryCode.Text = value.ElementOrDefault(FedExTemplate.CountryCode);
                txtPostalCode.Text = value.ElementOrDefault(FedExTemplate.PostalCode);
                txtState.Text = value.ElementOrDefault(FedExTemplate.State);
                txtCity.Text = value.ElementOrDefault(FedExTemplate.City);
                txtAddress.Text = value.ElementOrDefault(FedExTemplate.Address);
                txtAccountNumber.Text = value.ElementOrDefault(FedExTemplate.AccountNumber);
                txtMeterNumber.Text = value.ElementOrDefault(FedExTemplate.MeterNumber);
                txtRate.Text = value.ElementOrDefault(FedExTemplate.Rate);
                txtExtracharge.Text = value.ElementOrDefault(FedExTemplate.Extracharge);
                txtKey.Text = value.ElementOrDefault(FedExTemplate.Key);
                txtPassword.Text = value.ElementOrDefault(FedExTemplate.Password);

                chkEuropeFirstInternationalPriority.Checked = value.ElementOrDefault(FedExTemplate.EuropeFirstInternationalPriority).TryParseBool();
                chkFedex1DayFreight.Checked = value.ElementOrDefault(FedExTemplate.Fedex1DayFreight).TryParseBool();
                chkFedex2Day.Checked = value.ElementOrDefault(FedExTemplate.Fedex2Day).TryParseBool();
                chkFedex2DayFreight.Checked = value.ElementOrDefault(FedExTemplate.Fedex2DayFreight).TryParseBool();
                chkFedex3DayFreight.Checked = value.ElementOrDefault(FedExTemplate.Fedex3DayFreight).TryParseBool();
                chkFedexExpressSaver.Checked = value.ElementOrDefault(FedExTemplate.FedexExpressSaver).TryParseBool();
                chkFedexGround.Checked = value.ElementOrDefault(FedExTemplate.FedexGround).TryParseBool();
                chkFirstOvernight.Checked = value.ElementOrDefault(FedExTemplate.FirstOvernight).TryParseBool();
                chkGroundHomeDelivery.Checked = value.ElementOrDefault(FedExTemplate.GroundHomeDelivery).TryParseBool();
                chkInternationalDistributionFreight.Checked = value.ElementOrDefault(FedExTemplate.InternationalDistributionFreight).TryParseBool();
                chkInternationalEconomy.Checked = value.ElementOrDefault(FedExTemplate.InternationalEconomy).TryParseBool();
                chkInternationalEconomyDistribution.Checked = value.ElementOrDefault(FedExTemplate.InternationalEconomyDistribution).TryParseBool();
                chkInternationalEconomyFreight.Checked = value.ElementOrDefault(FedExTemplate.InternationalEconomyFreight).TryParseBool();
                chkInternationalFirst.Checked = value.ElementOrDefault(FedExTemplate.InternationalFirst).TryParseBool();
                chkInternationalPriority.Checked = value.ElementOrDefault(FedExTemplate.InternationalPriority).TryParseBool();
                chkInternationalPriorityFreight.Checked = value.ElementOrDefault(FedExTemplate.InternationalPriorityFreight).TryParseBool();
                chkPriorityOvernight.Checked = value.ElementOrDefault(FedExTemplate.PriorityOvernight).TryParseBool();
                chkSmartPost.Checked = value.ElementOrDefault(FedExTemplate.SmartPost).TryParseBool();
                chkStandardOvernight.Checked = value.ElementOrDefault(FedExTemplate.StandardOvernight).TryParseBool();
                chkFedexFreight.Checked = value.ElementOrDefault(FedExTemplate.FedexFreight).TryParseBool();
                chkFedexNationalFreight.Checked = value.ElementOrDefault(FedExTemplate.FedexNationalFreight).TryParseBool();
            }
        }

   
    
    }
}