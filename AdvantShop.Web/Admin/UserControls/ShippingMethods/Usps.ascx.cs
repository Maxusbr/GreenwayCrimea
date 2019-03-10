using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.Usps;

namespace Admin.UserControls.ShippingMethods
{
    public partial class UspsControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[]
                    {
                        txtUserId,
                        txtPostalCode,
                        txtRate,
                        txtExtracharge,
                        txtPassword,
                    },
                                                  new[] {txtRate, txtExtracharge},
                                                  new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {UspsTemplate.Rate, txtRate.Text},
                                   {UspsTemplate.Extracharge, txtExtracharge.Text},
                                   {UspsTemplate.UserId, txtUserId.Text},
                                   {UspsTemplate.Password, txtPassword.Text},
                                   {UspsTemplate.PostalCode, txtPostalCode.Text},


                                   {UspsTemplate.FirstClass, chkFirstClass.Checked.ToString()},
                                   {
                                       UspsTemplate.ExpressMailSundayHolidayGuarantee,
                                       chkExpressMailSundayHolidayGuarantee.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.ExpressMailFlatRateEnvelopeSundayHolidayGuarantee,
                                       chkExpressMailFlatRateEnvelopeSundayHolidayGuarantee.Checked.ToString()
                                   },
                                   {UspsTemplate.ExpressMailHoldForPickup, chkExpressMailHoldForPickup.Checked.ToString()},
                                   {
                                       UspsTemplate.ExpressMailFlatRateEnvelopeHoldForPickup,
                                       chkExpressMailFlatRateEnvelopeHoldForPickup.Checked.ToString()
                                   },
                                   {UspsTemplate.ExpresMail, chkExpresMail.Checked.ToString()},
                                   {
                                       UspsTemplate.ExpressMailFlatRateEnvelope,
                                       chkExpressMailFlatRateEnvelope.Checked.ToString()
                                   },
                                   {UspsTemplate.PriorityMail, chkPriorityMail.Checked.ToString()},
                                   {
                                       UspsTemplate.PriorityMailFlatRateEnvelope,
                                       chkPriorityMailFlatRateEnvelope.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.PriorityMailSmallFlatRateBox,
                                       chkPriorityMailSmallFlatRateBox.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.PriorityMailMediumFlatRateBox,
                                       chkPriorityMailMediumFlatRateBox.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.PriorityMailLargeFlatRateBox,
                                       chkPriorityMailLargeFlatRateBox.Checked.ToString()
                                   },
                                   {UspsTemplate.ParcelPost, chkParcelPost.Checked.ToString()},
                                   {UspsTemplate.BoundPrintedMatter, chkBoundPrintedMatter.Checked.ToString()},
                                   {UspsTemplate.MediaMail, chkMediaMail.Checked.ToString()},
                                   {UspsTemplate.LibraryMail, chkLibraryMail.Checked.ToString()},
                                   {UspsTemplate.GlobalExpressGuaranteed, chkGlobalExpressGuaranteed.Checked.ToString()},
                                   {
                                       UspsTemplate.GlobalExpressGuaranteedNonDocumentRectangular,
                                       chkGlobalExpressGuaranteedNonDocumentRectangular.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.GlobalExpressGuaranteedNonDocumentNonRectangular,
                                       chkGlobalExpressGuaranteedNonDocumentNonRectangular.Checked.ToString()
                                   },
                                   {UspsTemplate.UspsGxgEnvelopes, chkUspsGxgEnvelopes.Checked.ToString()},
                                   {
                                       UspsTemplate.ExpressMailInternationalFlatRateEnvelope,
                                       chkExpressMailInternationalFlatRateEnvelope.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.PriorityMailInternational,
                                       chkPriorityMailInternational.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.PriorityMailInternationalLargeFlatRateBox,
                                       chkPriorityMailInternationalLargeFlatRateBox.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.PriorityMailInternationalMediumFlatRateBox,
                                       chkPriorityMailInternationalMediumFlatRateBox.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.PriorityMailInternationalSmallFlatRateBox,
                                       chkPriorityMailInternationalSmallFlatRateBox.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.FirstClassMailInternationalLargeEnvelope,
                                       chkFirstClassMailInternationalLargeEnvelope.Checked.ToString()
                                   },
                                   {UspsTemplate.ExpressMailInternational, chkExpressMailInternational.Checked.ToString()},
                                   {
                                       UspsTemplate.PriorityMailInternationalFlatRateEnvelope,
                                       chkPriorityMailInternationalFlatRateEnvelope.Checked.ToString()
                                   },
                                   {
                                       UspsTemplate.FirstClassMailInternationalPackage,
                                       chkFirstClassMailInternationalPackage.Checked.ToString()
                                   },


                               }
                           : null;
            }
            set
            {
                txtRate.Text = value.ElementOrDefault(UspsTemplate.Rate);
                txtExtracharge.Text = value.ElementOrDefault(UspsTemplate.Extracharge);
                txtUserId.Text = value.ElementOrDefault(UspsTemplate.UserId);
                txtPassword.Text = value.ElementOrDefault(UspsTemplate.Password);
                txtPostalCode.Text = value.ElementOrDefault(UspsTemplate.PostalCode);


                chkFirstClass.Checked = value.ElementOrDefault(UspsTemplate.FirstClass).TryParseBool();
                chkExpressMailSundayHolidayGuarantee.Checked = value.ElementOrDefault(UspsTemplate.ExpressMailSundayHolidayGuarantee).TryParseBool();
                chkExpressMailFlatRateEnvelopeSundayHolidayGuarantee.Checked = value.ElementOrDefault(UspsTemplate.ExpressMailFlatRateEnvelopeSundayHolidayGuarantee).TryParseBool();
                chkExpressMailHoldForPickup.Checked = value.ElementOrDefault(UspsTemplate.ExpressMailHoldForPickup).TryParseBool();
                chkExpressMailFlatRateEnvelopeHoldForPickup.Checked = value.ElementOrDefault(UspsTemplate.ExpressMailFlatRateEnvelopeHoldForPickup).TryParseBool();
                chkExpresMail.Checked = value.ElementOrDefault(UspsTemplate.ExpresMail).TryParseBool();
                chkExpressMailFlatRateEnvelope.Checked = value.ElementOrDefault(UspsTemplate.ExpressMailFlatRateEnvelope).TryParseBool();
                chkPriorityMail.Checked = value.ElementOrDefault(UspsTemplate.PriorityMail).TryParseBool();
                chkPriorityMailFlatRateEnvelope.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailFlatRateEnvelope).TryParseBool();
                chkPriorityMailSmallFlatRateBox.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailSmallFlatRateBox).TryParseBool();
                chkPriorityMailMediumFlatRateBox.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailMediumFlatRateBox).TryParseBool();
                chkPriorityMailLargeFlatRateBox.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailLargeFlatRateBox).TryParseBool();
                chkParcelPost.Checked = value.ElementOrDefault(UspsTemplate.ParcelPost).TryParseBool();
                chkBoundPrintedMatter.Checked = value.ElementOrDefault(UspsTemplate.BoundPrintedMatter).TryParseBool();
                chkMediaMail.Checked = value.ElementOrDefault(UspsTemplate.MediaMail).TryParseBool();
                chkLibraryMail.Checked = value.ElementOrDefault(UspsTemplate.LibraryMail).TryParseBool();
                chkGlobalExpressGuaranteed.Checked = value.ElementOrDefault(UspsTemplate.GlobalExpressGuaranteed).TryParseBool();
                chkGlobalExpressGuaranteedNonDocumentRectangular.Checked = value.ElementOrDefault(UspsTemplate.GlobalExpressGuaranteedNonDocumentRectangular).TryParseBool();
                chkGlobalExpressGuaranteedNonDocumentNonRectangular.Checked = value.ElementOrDefault(UspsTemplate.GlobalExpressGuaranteedNonDocumentNonRectangular).TryParseBool();
                chkUspsGxgEnvelopes.Checked = value.ElementOrDefault(UspsTemplate.UspsGxgEnvelopes).TryParseBool();
                chkExpressMailInternationalFlatRateEnvelope.Checked = value.ElementOrDefault(UspsTemplate.ExpressMailInternationalFlatRateEnvelope).TryParseBool();
                chkPriorityMailInternational.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailInternational).TryParseBool();
                chkPriorityMailInternationalLargeFlatRateBox.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailInternationalLargeFlatRateBox).TryParseBool();
                chkPriorityMailInternationalMediumFlatRateBox.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailInternationalMediumFlatRateBox).TryParseBool();
                chkPriorityMailInternationalSmallFlatRateBox.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailInternationalSmallFlatRateBox).TryParseBool();
                chkFirstClassMailInternationalLargeEnvelope.Checked = value.ElementOrDefault(UspsTemplate.FirstClassMailInternationalLargeEnvelope).TryParseBool();
                chkExpressMailInternational.Checked = value.ElementOrDefault(UspsTemplate.ExpressMailInternational).TryParseBool();
                chkPriorityMailInternationalFlatRateEnvelope.Checked = value.ElementOrDefault(UspsTemplate.PriorityMailInternationalFlatRateEnvelope).TryParseBool();
                chkFirstClassMailInternationalPackage.Checked = value.ElementOrDefault(UspsTemplate.FirstClassMailInternationalPackage).TryParseBool();
            }
        }

    }
}