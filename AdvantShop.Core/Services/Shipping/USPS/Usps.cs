//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace AdvantShop.Shipping.Usps
{
    [ShippingKey("Usps")]
    public class Usps : BaseShipping
    {
        private string UserId { get; set; }
        private string Password { get; set; }
        private float Rate { get; set; }
        private float Extracharge { get; set; }

        private string PostalCodeFrom { get; set; }
        public string CountryTo { get; set; }
        public string CountryToIso2 { get; set; }
        public string PostalCodeTo { get; set; }

        public PackageType Container { get; set; }
        public PackageSize Size { get; set; }
        public ServiceType Service { get; set; }

        private const string UrlDomenic = "http://production.shippingapis.com/ShippingAPI.dll?API=RateV4&XML=";
        private const string UrlInternational = "http://production.shippingapis.com/ShippingAPI.dll?API=IntlRateV2&XML=";

        private float _weight;
        private bool _isDomenic;

        public List<string> EnabledService { get; private set; }

        private readonly float _totalPrice;

        public Usps(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {

            Rate = _method.Params.ElementOrDefault(UspsTemplate.Rate).TryParseFloat();
            Extracharge = _method.Params.ElementOrDefault(UspsTemplate.Extracharge).TryParseFloat();
            UserId = _method.Params.ElementOrDefault(UspsTemplate.UserId);
            Password = _method.Params.ElementOrDefault(UspsTemplate.Password);
            PostalCodeFrom = _method.Params.ElementOrDefault(UspsTemplate.PostalCode);

            EnabledService = new List<string>();
            EnabledService.AddRange(GetEnabledDomesticService(_method.Params));
            EnabledService.AddRange(GetEnabledInternationalService(_method.Params));
            _totalPrice = _preOrder.Items.Sum(item => item.Weight * item.Amount);
            Size = PackageSize.Regular;
        }


        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            try
            {
                _weight = MeasureUnits.ConvertWeight(_preOrder.Items.Sum(item=>item.Weight * item.Amount), MeasureUnits.WeightUnit.Kilogramm, MeasureUnits.WeightUnit.Pound);

                _isDomenic = CountryToIso2.ToUpper().Trim() == "US";
                return GetShippingOption();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new List<BaseShippingOption>();
            }
        }

        public enum PackageType { None, Flat_Rate_Envelope, Flat_Rate_Box };
        public enum PackageSize { None, Regular, Large, Oversize };
        public enum LabelImageType { TIF, PDF, None };
        public enum ServiceType { Priority, First_Class, Parcel_Post, Bound_Printed_Matter, Media_Mail, Library_Mail };
        public enum LabelType { FullLabel = 1, DeliveryConfirmationBarcode = 2 };

        #region PrivateMethods
        private static IEnumerable<string> GetEnabledDomesticService(Dictionary<string, string> items)
        {
            if (items.ElementOrDefault(UspsTemplate.FirstClass).TryParseBool())
            {
                yield return UspsTemplate.FirstClass;

            }
            if (items.ElementOrDefault(UspsTemplate.ExpressMailSundayHolidayGuarantee).TryParseBool())
            {
                yield return UspsTemplate.ExpressMailSundayHolidayGuarantee;
            }
            if (items.ElementOrDefault( UspsTemplate.ExpressMailFlatRateEnvelopeSundayHolidayGuarantee).TryParseBool())
            {
                yield return UspsTemplate.ExpressMailFlatRateEnvelopeSundayHolidayGuarantee;
            }
            if (items.ElementOrDefault(UspsTemplate.ExpressMailHoldForPickup).TryParseBool())
            {
                yield return UspsTemplate.ExpressMailHoldForPickup;
            }
            if (items.ElementOrDefault(UspsTemplate.ExpressMailFlatRateEnvelopeHoldForPickup).TryParseBool())
            {
                yield return UspsTemplate.ExpressMailFlatRateEnvelopeHoldForPickup;
            }
            if (items.ElementOrDefault(UspsTemplate.ExpresMail).TryParseBool())
            {
                yield return UspsTemplate.ExpresMail;
            }
            if (items.ElementOrDefault(UspsTemplate.ExpressMailFlatRateEnvelope).TryParseBool())
            {
                yield return UspsTemplate.ExpressMailFlatRateEnvelope;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMail).TryParseBool())
            {
                yield return UspsTemplate.PriorityMail;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailFlatRateEnvelope).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailFlatRateEnvelope;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailSmallFlatRateBox).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailSmallFlatRateBox;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailMediumFlatRateBox).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailMediumFlatRateBox;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailLargeFlatRateBox).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailLargeFlatRateBox;
            }
            if (items.ElementOrDefault(UspsTemplate.ParcelPost).TryParseBool())
            {
                yield return UspsTemplate.ParcelPost;
            }

            if (items.ElementOrDefault(UspsTemplate.BoundPrintedMatter).TryParseBool())
            {
                yield return UspsTemplate.BoundPrintedMatter;
            }
            if (items.ElementOrDefault(UspsTemplate.MediaMail).TryParseBool())
            {
                yield return UspsTemplate.MediaMail;
            }
            if (items.ElementOrDefault(UspsTemplate.LibraryMail).TryParseBool())
            {
                yield return UspsTemplate.LibraryMail;
            }
        }

        private static IEnumerable<string> GetEnabledInternationalService(Dictionary<string, string> items)
        {
            if (items.ElementOrDefault(UspsTemplate.GlobalExpressGuaranteed).TryParseBool())
            {
                yield return UspsTemplate.GlobalExpressGuaranteed;
            }
            if (items.ElementOrDefault(UspsTemplate.GlobalExpressGuaranteedNonDocumentRectangular).TryParseBool())
            {
                yield return UspsTemplate.GlobalExpressGuaranteedNonDocumentRectangular;
            }
            if (items.ElementOrDefault(UspsTemplate.GlobalExpressGuaranteedNonDocumentNonRectangular).TryParseBool())
            {
                yield return UspsTemplate.GlobalExpressGuaranteedNonDocumentNonRectangular;
            }
            if (items.ElementOrDefault(UspsTemplate.UspsGxgEnvelopes).TryParseBool())
            {
                yield return UspsTemplate.UspsGxgEnvelopes;
            }
            if (items.ElementOrDefault(UspsTemplate.ExpressMailInternationalFlatRateEnvelope).TryParseBool())
            {
                yield return UspsTemplate.ExpressMailInternationalFlatRateEnvelope;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailInternational).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailInternational;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailInternationalLargeFlatRateBox).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailInternationalLargeFlatRateBox;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailInternationalMediumFlatRateBox).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailInternationalMediumFlatRateBox;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailInternationalSmallFlatRateBox).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailInternationalSmallFlatRateBox;
            }
            if (items.ElementOrDefault(UspsTemplate.FirstClassMailInternationalLargeEnvelope).TryParseBool())
            {
                yield return UspsTemplate.FirstClassMailInternationalLargeEnvelope;
            }
            if (items.ElementOrDefault(UspsTemplate.ExpressMailInternational).TryParseBool())
            {
                yield return UspsTemplate.ExpressMailInternational;
            }
            if (items.ElementOrDefault(UspsTemplate.PriorityMailInternationalFlatRateEnvelope).TryParseBool())
            {
                yield return UspsTemplate.PriorityMailInternationalFlatRateEnvelope;
            }
            if (items.ElementOrDefault(UspsTemplate.FirstClassMailInternationalPackage).TryParseBool())
            {
                yield return UspsTemplate.FirstClassMailInternationalPackage;
            }
        }

        private List<BaseShippingOption> GetShippingOption()
        {
            string request = _isDomenic
                ? UrlDomenic + GetXmlDomenicPackage()
                : UrlInternational + GetXmlInternationalPackage();

            string xml = new WebClient().DownloadString(request);
            if (xml.Contains("<Error>"))
            {
                int idx1 = xml.IndexOf("<Description>") + 13;
                int idx2 = xml.IndexOf("</Description>");
                string errorText = xml.Substring(idx1, idx2 - idx1);

                Debug.Log.Error(new Exception("USPS Error returned: " + errorText));
            }

            return ParseResponseMessage(xml);
        }

        private string GetXmlDomenicPackage()
        {
            var lb = (int)_weight;
            int oz = ((int)(_weight * 16)) % 16;

            var sb = new StringBuilder();
            sb.AppendFormat("<RateV4Request USERID=\"{0}\" PASSWORD=\"{1}\">", UserId, Password);
            sb.Append("<Package ID=\"0\">");

            sb.AppendFormat("<Service>{0}</Service>", "ALL");
            sb.AppendFormat("<ZipOrigination>{0}</ZipOrigination>", PostalCodeFrom);
            sb.AppendFormat("<ZipDestination>{0}</ZipDestination>", PostalCodeTo);
            sb.AppendFormat("<Pounds>{0}</Pounds>", lb);
            sb.AppendFormat("<Ounces>{0}</Ounces>", oz);
            sb.AppendFormat("<Container />");
            sb.AppendFormat("<Size>{0}</Size>", Size);
            sb.Append("<Machinable>FALSE</Machinable>");

            sb.Append("</Package>");
            sb.Append("</RateV4Request>");

            return sb.ToString();
        }

        private string GetXmlInternationalPackage()
        {
            var lb = (int)_weight;
            int oz = ((int)(_weight * 16)) % 16;

            // TODO: Ширина, длинна, высота и обхват посылки. Измеряется в дюймах и округляется до ближайшего целого.
            int width = 0;
            int length = 0;
            int height = 0;
            int girth = 0;

            var sb = new StringBuilder();
            sb.AppendFormat("<IntlRateV2Request USERID=\"{0}\" PASSWORD=\"{1}\">", UserId, Password);
            sb.Append("<Package ID=\"0\">");

            sb.AppendFormat("<Pounds>{0}</Pounds>", lb);
            sb.AppendFormat("<Ounces>{0}</Ounces>", oz);
            sb.AppendFormat("<Machinable>{0}</Machinable>", "FALSE");
            sb.AppendFormat("<MailType>{0}</MailType>", "ALL");
            sb.AppendFormat("<ValueOfContents>{0}</ValueOfContents>", _totalPrice);
            sb.AppendFormat("<Country>{0}</Country>", CountryTo);
            sb.AppendFormat("<Container>{0}</Container>", "RECTANGULAR");
            sb.AppendFormat("<Size>{0}</Size>", Size);
            sb.AppendFormat("<Width>{0}</Width>", width);
            sb.AppendFormat("<Length>{0}</Length>", length);
            sb.AppendFormat("<Height>{0}</Height>", height);
            sb.AppendFormat("<Girth>{0}</Girth>", girth);
            sb.AppendFormat("<CommercialFlag>{0}</CommercialFlag>", "Y");

            sb.Append("</Package>");
            sb.Append("</IntlRateV2Request>");

            return sb.ToString();
        }

        private List<BaseShippingOption> ParseResponseMessage(string response)
        {
            string serviceTag = "Service";
            string serviceNameTag = "SvcDescription";
            string rateTag = "Postage";

            if (_isDomenic)
            {
                serviceTag = "Postage";
                serviceNameTag = "MailService";
                rateTag = "Rate";
            }

            var shippingOptions = new List<BaseShippingOption>();
            using (var sr = new StringReader(response))
            using (var tr = new XmlTextReader(sr))
            {
                do
                {
                    tr.Read();

                    if ((tr.Name == serviceTag) && (tr.NodeType == XmlNodeType.Element))
                    {
                        string serviceCode = string.Empty;
                        string postalRate = string.Empty;

                        do
                        {
                            tr.Read();

                            if ((tr.Name == serviceNameTag) && (tr.NodeType == XmlNodeType.Element))
                            {
                                serviceCode = tr.ReadString().Replace("**", "");
                                int idx1 = serviceCode.IndexOf("&lt;sup&gt;");
                                int idx2 = serviceCode.IndexOf("&lt;/sup&gt;") + 12;

                                if (idx1 >= 0)
                                {
                                    serviceCode = serviceCode.Remove(idx1, idx2 - idx1);
                                }

                                tr.ReadEndElement();
                                if ((tr.Name == serviceNameTag) && (tr.NodeType == XmlNodeType.EndElement))
                                    break;
                            }

                            if ((tr.Name == rateTag) && (tr.NodeType == XmlNodeType.Element))
                            {
                                postalRate = tr.ReadString();
                                tr.ReadEndElement();
                                if ((tr.Name == rateTag) && (tr.NodeType == XmlNodeType.EndElement))
                                    break;
                            }

                        }
                        while (!((tr.Name == serviceTag) && (tr.NodeType == XmlNodeType.EndElement)));

                        if ((EnabledService.Contains(serviceCode)) && (shippingOptions.Find(s => s.NameRate == serviceCode) == null))
                        {
                            var shippingRate = (Rate > 0) ? postalRate.TryParseFloat() * Rate + Extracharge
                                                          : postalRate.TryParseFloat() + Extracharge;
                            //if (Rate > 0)
                            //{
                            //    shippingRate *= Rate;
                            //}

                            var shippingOption = new BaseShippingOption(_method)
                                                     {
                                                         Rate = shippingRate,
                                                         NameRate = serviceCode
                                                     };
                            shippingOptions.Add(shippingOption);
                        }
                    }
                }
                while (!tr.EOF);
            }
            return shippingOptions;
        }
        #endregion
    }
}