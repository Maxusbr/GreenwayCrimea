//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;

//using UPSRateServiceWebReference;

namespace AdvantShop.Shipping.Ups
{
    [ShippingKey("Ups")]
    public class Ups : BaseShipping
    {
        private const float MaxPackageWeight = 68.03F;

        private string AccessKey { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private UpsCustomerClassification CustomerType { get; set; }

        private UpsPickupType PickupType { get; set; }
        private UpsPackagingType PackagingType { get; set; }
        private float Extracharge { get; set; }
        private float Rate { get; set; }

        private string CountryCodeFrom { get; set; }
        private string PostalCodeFrom { get; set; }

        public string CountryCodeTo { get; set; }
        public string PostalCodeTo { get; set; }
        public string StateTo { get; set; }
        public string CityTo { get; set; }
        public string AddressTo { get; set; }

        public string EnabledService { get; private set; }


        public Ups(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            AccessKey = _method.Params.ElementOrDefault(UpsTemplate.AccessKey);
            UserName = _method.Params.ElementOrDefault(UpsTemplate.UserName);
            Password = _method.Params.ElementOrDefault(UpsTemplate.Password);
            PostalCodeFrom = _method.Params.ElementOrDefault(UpsTemplate.PostalCode);
            CountryCodeFrom = _method.Params.ElementOrDefault(UpsTemplate.CountryCode);
            CustomerType = (UpsCustomerClassification)_method.Params.ElementOrDefault(UpsTemplate.CustomerClassification).TryParseInt();
            PickupType = (UpsPickupType)_method.Params.ElementOrDefault(UpsTemplate.PickupType).TryParseInt();
            PackagingType = (UpsPackagingType)_method.Params.ElementOrDefault(UpsTemplate.PackagingType).TryParseInt();
            Extracharge = _method.Params.ElementOrDefault(UpsTemplate.Extracharge).TryParseFloat();
            Rate = _method.Params.ElementOrDefault(UpsTemplate.Rate).TryParseFloat();
            EnabledService = GetEnabledService(_method.Params);
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var shippingOptions = new List<AbstractShippingOption>();

            throw new Exception("TODO");
            //TODO: Uncomment!!!
            //try
            //{
            //    var service = new RateService();
            //    RateRequest request = CreateRateRequest(service);
            //    RateResponse rateResponse = service.ProcessRate(request);
            //    shippingOptions = ParseAnswer(rateResponse);
            //}
            //catch (System.Web.Services.Protocols.SoapException ex)
            //{
            //    Debug.Log.Error(ex, false);
            //}
            //catch (System.ServiceModel.CommunicationException ex)
            //{
            //    Debug.Log.Error(ex, false);
            //}
            //catch (Exception ex)
            //{
            //    Debug.Log.Error(ex);
            //}
            //return shippingOptions;
        }      

        private static string GetEnabledService(Dictionary<string, string> items)
        {
            var res = new StringBuilder();
            if (items.ElementOrDefault(UpsTemplate.UpsNextDayAir).TryParseBool())
            {
                res.Append(UpsTemplate.UpsNextDayAir + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.Ups2NdDayAir).TryParseBool())
            {
                res.Append(UpsTemplate.Ups2NdDayAir + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsGround).TryParseBool())
            {
                res.Append(UpsTemplate.UpsGround + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsWorldwideExpress).TryParseBool())
            {
                res.Append(UpsTemplate.UpsWorldwideExpress + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsWorldwideExpedited).TryParseBool())
            {
                res.Append(UpsTemplate.UpsWorldwideExpedited + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsStandard).TryParseBool())
            {
                res.Append(UpsTemplate.UpsStandard + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.Ups3DaySelect).TryParseBool())
            {
                res.Append(UpsTemplate.Ups3DaySelect + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsNextDayAirSaver).TryParseBool())
            {
                res.Append(UpsTemplate.UpsNextDayAirSaver + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsNextDayAirEarlyAm).TryParseBool())
            {
                res.Append(UpsTemplate.UpsNextDayAirEarlyAm + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsWorldwideExpressPlus).TryParseBool())
            {
                res.Append(UpsTemplate.UpsWorldwideExpressPlus + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.Ups2NdDayAirAm).TryParseBool())
            {
                res.Append(UpsTemplate.Ups2NdDayAirAm + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsSaver).TryParseBool())
            {
                res.Append(UpsTemplate.UpsSaver + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsTodayStandard).TryParseBool())
            {
                res.Append(UpsTemplate.UpsTodayStandard + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsTodayDedicatedCourrier).TryParseBool())
            {
                res.Append(UpsTemplate.UpsTodayDedicatedCourrier + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsTodayExpress).TryParseBool())
            {
                res.Append(UpsTemplate.UpsTodayExpress + ";");
            }
            if (items.ElementOrDefault(UpsTemplate.UpsTodayExpressSaver).TryParseBool())
            {
                res.Append(UpsTemplate.UpsTodayExpressSaver + ";");
            }

            return res.ToString();
        }

        public static string GetServiceId(string service)
        {
            string serviceId = string.Empty;
            switch (service)
            {
                case "UPS Next Day Air":
                    serviceId = "01";
                    break;
                case "UPS 2nd Day Air":
                    serviceId = "02";
                    break;
                case "UPS Ground":
                    serviceId = "03";
                    break;
                case "UPS Worldwide Express":
                    serviceId = "07";
                    break;
                case "UPS Worldwide Expedited":
                    serviceId = "08";
                    break;
                case "UPS Standard":
                    serviceId = "11";
                    break;
                case "UPS 3 Day Select":
                    serviceId = "12";
                    break;
                case "UPS Next Day Air Saver":
                    serviceId = "13";
                    break;
                case "UPS Next Day Air Early A.M.":
                    serviceId = "14";
                    break;
                case "UPS Worldwide Express Plus":
                    serviceId = "54";
                    break;
                case "UPS 2nd Day Air A.M.":
                    serviceId = "59";
                    break;
                case "UPS Saver":
                    serviceId = "65";
                    break;
                case "UPS Today Standard": //82-86, for Polish Domestic Shipments
                    serviceId = "82";
                    break;
                case "UPS Today Dedicated Courier":
                    serviceId = "83";
                    break;
                case "UPS Today Express":
                    serviceId = "85";
                    break;
                case "UPS Today Express Saver":
                    serviceId = "86";
                    break;
                default:
                    break;
            }
            return serviceId;
        }

        #region PrivateMethods
        //private RateRequest CreateRateRequest(RateService rate)
        //{
        //    RateRequest rateRequest = new RateRequest();
        //    UPSSecurity upss = new UPSSecurity();
        //    UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
        //    upssSvcAccessToken.AccessLicenseNumber = AccessKey;
        //    upss.ServiceAccessToken = upssSvcAccessToken;

        //    UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
        //    upssUsrNameToken.Username = UserName;
        //    upssUsrNameToken.Password = Password;
        //    upss.UsernameToken = upssUsrNameToken;
        //    rate.UPSSecurityValue = upss;

        //    RequestType request = new RequestType();
        //    String[] requestOption = { "Shop" };
        //    request.RequestOption = requestOption;
        //    rateRequest.Request = request;

        //    ShipmentType shipment = new ShipmentType();

        //    ShipperType shipper = new ShipperType();
        //    //shipper.ShipperNumber = "ISUS01";
        //    AddressType shipperAddress = new AddressType();
        //    String[] addressLine = { "Shipper\'s address line" };
        //    shipperAddress.AddressLine = addressLine;
        //    shipperAddress.City = "Shipper\'s city";
        //    shipperAddress.PostalCode = PostalCodeFrom;
        //    //shipperAddress.StateProvinceCode = UpsItem.CountryCode;
        //    shipperAddress.CountryCode = CountryCodeFrom;
        //    shipperAddress.AddressLine = addressLine;
        //    shipper.Address = shipperAddress;
        //    shipment.Shipper = shipper;

        //    ShipFromType shipFrom = new ShipFromType();
        //    AddressType shipFromAddress = new AddressType();
        //    shipFromAddress.AddressLine = addressLine;
        //    shipFromAddress.City = "ShipFrom city";
        //    shipFromAddress.PostalCode = PostalCodeFrom;
        //    //shipFromAddress.StateProvinceCode = "GA";
        //    shipFromAddress.CountryCode = CountryCodeFrom;
        //    shipFrom.Address = shipFromAddress;
        //    shipment.ShipFrom = shipFrom;

        //    ShipToType shipTo = new ShipToType();
        //    ShipToAddressType shipToAddress = new ShipToAddressType();
        //    String[] addressLine1 = { AddressTo };
        //    shipToAddress.AddressLine = addressLine1;
        //    shipToAddress.City = CityTo;
        //    shipToAddress.PostalCode = PostalCodeTo;
        //    shipToAddress.StateProvinceCode = StateTo;
        //    shipToAddress.CountryCode = CountryCodeTo;
        //    shipTo.Address = shipToAddress;
        //    shipment.ShipTo = shipTo;

        //    //CodeDescriptionType service = new CodeDescriptionType();
        //    //service.Code = "02";
        //    //shipment.Service = service;
        //    float weight = MeasureUnits.ConvertWeight(ShoppingCart.TotalShippingWeight, MeasureUnits.WeightUnit.Kilogramm, MeasureUnits.WeightUnit.Pound);

        //    var data = new List<PackageType>();
        //    if (!IsPackageTooHeavy(weight))
        //    {

        //        PackageType package = new PackageType();
        //        PackageWeightType packageWeight = new PackageWeightType();
        //        packageWeight.Weight = weight.ToString("F3").Replace(',', '.');

        //        CodeDescriptionType uom = new CodeDescriptionType();
        //        uom.Code = "LBS";
        //        uom.Description = "Pounds";
        //        packageWeight.UnitOfMeasurement = uom;
        //        package.PackageWeight = packageWeight;

        //        CodeDescriptionType packType = new CodeDescriptionType();
        //        packType.Code = "02";
        //        package.PackagingType = packType;
        //        data.Add(package);
        //    }
        //    else
        //    {
        //        int totalPackages = 1;
        //        int totalPackagesWeights = 1;
        //        if (IsPackageTooHeavy(weight))
        //        {
        //            totalPackagesWeights = SQLDataHelper.GetInt(Math.Ceiling(weight / MaxPackageWeight));
        //        }

        //        totalPackages = totalPackagesWeights;
        //        if (totalPackages == 0)
        //            totalPackages = 1;

        //        float weight2 = weight / totalPackages;

        //        if (weight2 < 1)
        //            weight2 = 1;
        //        for (int i = 0; i < totalPackages; i++)
        //        {
        //            PackageType package = new PackageType();
        //            PackageWeightType packageWeight = new PackageWeightType();
        //            packageWeight.Weight = weight2.ToString("F3");

        //            CodeDescriptionType uom = new CodeDescriptionType();
        //            uom.Code = "LBS";
        //            uom.Description = "Pounds";
        //            packageWeight.UnitOfMeasurement = uom;
        //            package.PackageWeight = packageWeight;

        //            CodeDescriptionType packType = new CodeDescriptionType();
        //            packType.Code = GetPackagingTypeCode(PackagingType);
        //            package.PackagingType = packType;
        //            data.Add(package);
        //        }
        //    }

        //    PackageType[] pkgArray = data.ToArray();
        //    shipment.Package = pkgArray;
        //    rateRequest.Shipment = shipment;

        //    CodeDescriptionType pckup = new CodeDescriptionType() { Code = GetPickupTypeCode(PickupType) };
        //    rateRequest.PickupType = pckup;

        //    CodeDescriptionType ccustomer = new CodeDescriptionType() { Code = GetCustomerClassificationCode(CustomerType) };
        //    rateRequest.CustomerClassification = ccustomer;

        //    System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

        //    return rateRequest;
        //}

        private static bool IsPackageTooHeavy(float weight)
        {
            if (weight > MaxPackageWeight)
                return true;
            return false;
        }

        private static string GetCustomerClassificationCode(UpsCustomerClassification customerClassification)
        {
            switch (customerClassification)
            {
                case UpsCustomerClassification.Wholesale:
                    return "01";
                case UpsCustomerClassification.Occasional:
                    return "03";
                case UpsCustomerClassification.Retail:
                    return "04";
                default:
                    return string.Empty;
            }
        }
        private static string GetPackagingTypeCode(UpsPackagingType packagingType)
        {
            switch (packagingType)
            {
                case UpsPackagingType.Letter:
                    return "01";
                case UpsPackagingType.CustomerSuppliedPackage:
                    return "02";
                case UpsPackagingType.Tube:
                    return "03";
                case UpsPackagingType.PAK:
                    return "04";
                case UpsPackagingType.ExpressBox:
                    return "21";
                case UpsPackagingType._10KgBox:
                    return "25";
                case UpsPackagingType._25KgBox:
                    return "24";
                default:
                    return string.Empty;
            }
        }
        private static string GetPickupTypeCode(UpsPickupType pickupType)
        {
            switch (pickupType)
            {
                case UpsPickupType.DailyPickup:
                    return "01";
                case UpsPickupType.CustomerCounter:
                    return "03";
                case UpsPickupType.OneTimePickup:
                    return "06";
                case UpsPickupType.OnCallAir:
                    return "07";
                case UpsPickupType.SuggestedRetailRates:
                    return "11";
                case UpsPickupType.LetterCenter:
                    return "19";
                case UpsPickupType.AirServiceCenter:
                    return "20";
                default:
                    return string.Empty;
            }
        }
        private static string GetServiceName(string serviceID)
        {
            switch (serviceID)
            {
                case "01":
                    return "UPS Next Day Air";
                case "02":
                    return "UPS 2nd Day Air";
                case "03":
                    return "UPS Ground";
                case "07":
                    return "UPS Worldwide Express";
                case "08":
                    return "UPS Worldwide Expedited";
                case "11":
                    return "UPS Standard";
                case "12":
                    return "UPS 3 Day Select";
                case "13":
                    return "UPS Next Day Air Saver";
                case "14":
                    return "UPS Next Day Air Early A.M.";
                case "54":
                    return "UPS Worldwide Express Plus";
                case "59":
                    return "UPS 2nd Day Air A.M.";
                case "65":
                    return "UPS Saver";
                case "82": //82-86, for Polish Domestic Shipments
                    return "UPS Today Standard";
                case "83":
                    return "UPS Today Dedicated Courier";
                case "85":
                    return "UPS Today Express";
                case "86":
                    return "UPS Today Express Saver";
                default:
                    return string.Empty;
            }
        }
        //private List<ShippingOption> ParseAnswer(RateResponse rateResponse)
        //{
        //    var res = new List<ShippingOption>();
        //    var enabledServices = EnabledService;
        //    foreach (var rateDetail in rateResponse.RatedShipment)
        //    {
        //        var shippingOption = new ShippingOption();
        //        if (!String.IsNullOrEmpty(enabledServices) && !enabledServices.Contains(GetServiceName(rateDetail.Service.Code)))
        //        {
        //            continue;
        //        }

        //        string serviceName = GetServiceName(rateDetail.Service.Code);
        //        shippingOption.Name = serviceName;

        //        var shippingRate = (Rate > 0) ? rateDetail.TotalCharges.MonetaryValue.TryParseFloat() * Rate + Extracharge
        //                                      : rateDetail.TotalCharges.MonetaryValue.TryParseFloat() + Extracharge;
        //        shippingOption.Rate = shippingRate;
        //        res.Add(shippingOption);
        //    }
        //    return res;
        //}
        #endregion
    }
}