using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.Ups;
using AdvantShop.Shipping.Usps;

namespace Admin.UserControls.ShippingMethods
{
    public partial class UpsControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[]
                    {
                        txtAccessKey,
                        txtUserName,
                        txtCountryCode,
                        txtPostalCode,
                        txtRate,
                        txtExtracharge,
                        txtPassword,
                    },
                                                  new[] {txtRate, txtExtracharge},
                                                  new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {UpsTemplate.AccessKey, txtAccessKey.Text},
                                   {UpsTemplate.UserName, txtUserName.Text},
                                   {UpsTemplate.CountryCode, txtCountryCode.Text},


                                   {UpsTemplate.Password, txtPassword.Text},
                                   {UpsTemplate.PostalCode, txtPostalCode.Text},



                                   {UpsTemplate.Extracharge, txtExtracharge.Text},
                                   {UpsTemplate.Rate, txtRate.Text},


                                   {UpsTemplate.CustomerClassification, ddlCustomerType.SelectedValue},
                                   {UpsTemplate.PickupType, ddlPickupType.SelectedValue},
                                   {UpsTemplate.PackagingType, ddlPackagingType.SelectedValue},


                                   {UpsTemplate.UpsNextDayAir, chkUpsNextDayAir.Checked.ToString()},
                                   {UpsTemplate.Ups2NdDayAir, chkUps2NdDayAir.Checked.ToString()},
                                   {UpsTemplate.UpsGround, chkUpsGround.Checked.ToString()},
                                   {UpsTemplate.UpsWorldwideExpress, chkUpsWorldwideExpress.Checked.ToString()},
                                   {UpsTemplate.UpsWorldwideExpedited, chkUpsWorldwideExpedited.Checked.ToString()},
                                   {UpsTemplate.UpsStandard, chkUpsStandard.Checked.ToString()},
                                   {UpsTemplate.Ups3DaySelect, chkUps3DaySelect.Checked.ToString()},
                                   {UpsTemplate.UpsNextDayAirSaver, chkUpsNextDayAirSaver.Checked.ToString()},
                                   {UpsTemplate.UpsNextDayAirEarlyAm, chkUpsNextDayAirEarlyAm.Checked.ToString()},
                                   {UpsTemplate.UpsWorldwideExpressPlus, chkUpsWorldwideExpressPlus.Checked.ToString()},
                                   {UpsTemplate.Ups2NdDayAirAm, chkUps2NdDayAirAm.Checked.ToString()},
                                   {UpsTemplate.UpsSaver, chkUpsSaver.Checked.ToString()},
                                   {UpsTemplate.UpsTodayStandard, chkUpsTodayStandard.Checked.ToString()},
                                   {UpsTemplate.UpsTodayDedicatedCourrier, chkUpsTodayDedicatedCourrier.Checked.ToString()},
                                   {UpsTemplate.UpsTodayExpress, chkUpsTodayExpress.Checked.ToString()},
                                   {UpsTemplate.UpsTodayExpressSaver, chkUpsTodayExpressSaver.Checked.ToString()},
                               }
                           : null;
            }
            set
            {
                txtRate.Text = value.ElementOrDefault(UspsTemplate.Rate);
                txtExtracharge.Text = value.ElementOrDefault(UspsTemplate.Extracharge);

                txtPassword.Text = value.ElementOrDefault(UspsTemplate.Password);
                txtPostalCode.Text = value.ElementOrDefault(UspsTemplate.PostalCode);


                txtAccessKey.Text = value.ElementOrDefault(UpsTemplate.AccessKey);
                txtUserName.Text = value.ElementOrDefault(UpsTemplate.UserName);
                txtCountryCode.Text = value.ElementOrDefault(UpsTemplate.CountryCode);

                ddlCustomerType.SelectedValue = value.ElementOrDefault(UpsTemplate.CustomerClassification);
                ddlPickupType.SelectedValue = value.ElementOrDefault(UpsTemplate.PickupType);
                ddlPackagingType.SelectedValue = value.ElementOrDefault(UpsTemplate.PackagingType);

                chkUpsNextDayAir.Checked = value.ElementOrDefault(UpsTemplate.UpsNextDayAir).TryParseBool();
                chkUps2NdDayAir.Checked = value.ElementOrDefault(UpsTemplate.Ups2NdDayAir).TryParseBool();
                chkUpsGround.Checked = value.ElementOrDefault(UpsTemplate.UpsGround).TryParseBool();
                chkUpsWorldwideExpress.Checked = value.ElementOrDefault(UpsTemplate.UpsWorldwideExpress).TryParseBool();
                chkUpsWorldwideExpedited.Checked = value.ElementOrDefault(UpsTemplate.UpsWorldwideExpedited).TryParseBool();
                chkUpsStandard.Checked = value.ElementOrDefault(UpsTemplate.UpsStandard).TryParseBool();
                chkUps3DaySelect.Checked = value.ElementOrDefault(UpsTemplate.Ups3DaySelect).TryParseBool();
                chkUpsNextDayAirSaver.Checked = value.ElementOrDefault(UpsTemplate.UpsNextDayAirSaver).TryParseBool();
                chkUpsNextDayAirEarlyAm.Checked = value.ElementOrDefault(UpsTemplate.UpsNextDayAirEarlyAm).TryParseBool();
                chkUpsWorldwideExpressPlus.Checked = value.ElementOrDefault(UpsTemplate.UpsWorldwideExpressPlus).TryParseBool();
                chkUps2NdDayAirAm.Checked = value.ElementOrDefault(UpsTemplate.Ups2NdDayAirAm).TryParseBool();
                chkUpsSaver.Checked = value.ElementOrDefault(UpsTemplate.UpsSaver).TryParseBool();
                chkUpsTodayStandard.Checked = value.ElementOrDefault(UpsTemplate.UpsTodayStandard).TryParseBool();
                chkUpsTodayDedicatedCourrier.Checked = value.ElementOrDefault(UpsTemplate.UpsTodayDedicatedCourrier).TryParseBool();
                chkUpsTodayExpress.Checked = value.ElementOrDefault(UpsTemplate.UpsTodayExpress).TryParseBool();
                chkUpsTodayExpressSaver.Checked = value.ElementOrDefault(UpsTemplate.UpsTodayExpressSaver).TryParseBool();
            }
        }
    }
}