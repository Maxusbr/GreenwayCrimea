/*
//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.FedexRateServiceWebReference;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

//using FedExRateServiceWebReference;
//using RateWebServiceClient.RateServiceWebReference;

namespace AdvantShop.Shipping.FedEx
{
    [ShippingKey("FedEx")]
    public class FedEx : BaseShipping
    {
        private const float MaxPackageWeight = 68.03F;
        //***
        private string AccountNumber { get; set; }
        private string MeterNumber { get; set; }
        private float Rate { get; set; }
        private float Extracharge { get; set; }
        private string Key { get; set; }
        private readonly string _password;

        //**
        private readonly string _countryCodeFrom;
        private readonly string _postalCodeFrom;
        private readonly string _stateFrom;
        private readonly string _cityFrom;
        private readonly string _addressFrom;

        private readonly float _totalPrice;
        //***
        public string EnabledService { get; private set; }

        //Constructor
        public FedEx(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {

            _countryCodeFrom = _method.Params.ElementOrDefault(FedExTemplate.CountryCode);
            _postalCodeFrom = _method.Params.ElementOrDefault(FedExTemplate.PostalCode);
            _stateFrom = _method.Params.ElementOrDefault(FedExTemplate.State);
            _cityFrom = _method.Params.ElementOrDefault(FedExTemplate.City);
            _addressFrom = _method.Params.ElementOrDefault(FedExTemplate.Address);
            AccountNumber = _method.Params.ElementOrDefault(FedExTemplate.AccountNumber);
            MeterNumber = _method.Params.ElementOrDefault(FedExTemplate.MeterNumber);
            Rate = _method.Params.ElementOrDefault(FedExTemplate.Rate).TryParseFloat();
            Extracharge = _method.Params.ElementOrDefault(FedExTemplate.Extracharge).TryParseFloat();
            Key = _method.Params.ElementOrDefault(FedExTemplate.Key);
            _password = _method.Params.ElementOrDefault(FedExTemplate.Password);
            EnabledService = GetEnabledService(_method.Params);
            _totalPrice = _preOrder.Items.Sum(item => item.Price * item.Amount);
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var shippingOptions = new List<BaseShippingOption>();

            var request = CreateRateRequest();

            var service = new RateService(); // Initialize the service

            // Call the web service passing in a RateRequest and returning a RateReply
            RateReply reply = service.getRates(request);

            if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING) // check if the call was successful
            {
                shippingOptions = ParseAnswer(reply);
            }
            else
            {
                Debug.Log.Error(new Exception(reply.Notifications[0].Message));
            }

            return shippingOptions;
        }

        public string GetServiceName(string serviceType)
        {
            switch (serviceType)
            {
                case "EUROPE_FIRST_INTERNATIONAL_PRIORITY":
                    return "FedEx Europe First International Priority";
                case "FEDEX_1_DAY_FREIGHT":
                    return "FedEx 1Day Freight";
                case "FEDEX_2_DAY":
                    return "FedEx 2Day";
                case "FEDEX_2_DAY_FREIGHT":
                    return "FedEx 2Day Freight";
                case "FEDEX_3_DAY_FREIGHT":
                    return "FedEx 3Day Freight";
                case "FEDEX_EXPRESS_SAVER":
                    return "FedEx Express Saver";
                case "FEDEX_GROUND":
                    return "FedEx Ground";
                case "FIRST_OVERNIGHT":
                    return "FexEx First Overnight";
                case "GROUND_HOME_DELIVERY":
                    return "FedEx Ground Home Delivery";
                case "INTERNATIONAL_DISTRIBUTION_FREIGHT":
                    return "FedEx International Distribution Freight";
                case "INTERNATIONAL_ECONOMY":
                    return "FedEx International Economy";
                case "INTERNATIONAL_ECONOMY_DISTRIBUTION":
                    return "FedEx International Economy Distribution";
                case "INTERNATIONAL_ECONOMY_FREIGHT":
                    return "FedEx International Economy Freight";
                case "INTERNATIONAL_FIRST":
                    return "FedEx International First";
                case "INTERNATIONAL_PRIORITY":
                    return "FedEx International Priority";
                case "INTERNATIONAL_PRIORITY_FREIGHT":
                    return "FedEx International Priority Freight";
                case "PRIORITY_OVERNIGHT":
                    return "FedEx Priority Overnight";
                case "SMART_POST":
                    return "FedEx Smart Post";
                case "STANDARD_OVERNIGHT":
                    return "FedEx Standard Overnight";
                case "FEDEX_FREIGHT":
                    return "FedEx Freight";
                case "FEDEX_NATIONAL_FREIGHT":
                    return "FedEx National Freight";
                default:
                    return "UNKNOWN";
            }
        }

        private static string GetEnabledService(Dictionary<string, string> items)
        {
            var res = new StringBuilder();


            if (items.ElementOrDefault(FedExTemplate.EuropeFirstInternationalPriority).TryParseBool())
            {
                res.Append(FedExTemplate.EuropeFirstInternationalPriority + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.Fedex1DayFreight).TryParseBool())
            {
                res.Append(FedExTemplate.Fedex1DayFreight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.Fedex2Day).TryParseBool())
            {
                res.Append(FedExTemplate.Fedex2Day + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.Fedex2DayFreight).TryParseBool())
            {
                res.Append(FedExTemplate.Fedex2DayFreight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.Fedex3DayFreight).TryParseBool())
            {
                res.Append(FedExTemplate.Fedex3DayFreight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.FedexExpressSaver).TryParseBool())
            {
                res.Append(FedExTemplate.FedexExpressSaver + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.FedexGround).TryParseBool())
            {
                res.Append(FedExTemplate.FedexGround + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.FirstOvernight).TryParseBool())
            {
                res.Append(FedExTemplate.FirstOvernight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.GroundHomeDelivery).TryParseBool())
            {
                res.Append(FedExTemplate.GroundHomeDelivery + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.InternationalDistributionFreight).TryParseBool())
            {
                res.Append(FedExTemplate.InternationalDistributionFreight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.InternationalEconomy).TryParseBool())
            {
                res.Append(FedExTemplate.InternationalEconomy + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.InternationalEconomyDistribution).TryParseBool())
            {
                res.Append(FedExTemplate.InternationalEconomyDistribution + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.InternationalEconomyFreight).TryParseBool())
            {
                res.Append(FedExTemplate.InternationalEconomyFreight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.InternationalFirst).TryParseBool())
            {
                res.Append(FedExTemplate.InternationalFirst + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.InternationalPriority).TryParseBool())
            {
                res.Append(FedExTemplate.InternationalPriority + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.InternationalPriorityFreight).TryParseBool())
            {
                res.Append(FedExTemplate.InternationalPriorityFreight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.PriorityOvernight).TryParseBool())
            {
                res.Append(FedExTemplate.PriorityOvernight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.SmartPost).TryParseBool())
            {
                res.Append(FedExTemplate.SmartPost + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.StandardOvernight).TryParseBool())
            {
                res.Append(FedExTemplate.StandardOvernight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.FedexFreight).TryParseBool())
            {
                res.Append(FedExTemplate.FedexFreight + ";");
            }
            if (items.ElementOrDefault(FedExTemplate.FedexNationalFreight).TryParseBool())
            {
                res.Append(FedExTemplate.FedexNationalFreight + ";");
            }

            return res.ToString();
        }

        private RateRequest CreateRateRequest()
        {
            // Build the RateRequest
            var request = new RateRequest();
            //
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = Key; // Replace "XXX" with the Key
            request.WebAuthenticationDetail.UserCredential.Password = _password; // Replace "XXX" with the Password
            //
            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = AccountNumber; // Replace "XXX" with client's account number
            request.ClientDetail.MeterNumber = MeterNumber; // Replace "XXX" with client's meter number
            //
            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***Rate for AdvantShop***"; // This is a reference field for the customer.  Any value can be used and will be provided in the response.
            //
            request.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl
            //
            request.ReturnTransitAndCommit = true;
            request.ReturnTransitAndCommitSpecified = true;
            request.CarrierCodes = new CarrierCodeType[2];
            request.CarrierCodes[0] = CarrierCodeType.FDXE;
            request.CarrierCodes[1] = CarrierCodeType.FDXG;
            //
            SetShipmentDetails(request);
            //
            SetOrigin(request);
            //
            SetDestination(request);
            //
            SetPayment(request);
            //
            SetIndividualPackageLineItems(request);
            //
            return request;
        }

        private void SetShipmentDetails(RateRequest request)
        {
            request.RequestedShipment = new RequestedShipment();
            request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP; //Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
            //request.RequestedShipment.ServiceType = ServiceType.INTERNATIONAL_PRIORITY; // Service types are STANDARD_OVERNIGHT, PRIORITY_OVERNIGHT, FEDEX_GROUND ...
            //request.RequestedShipment.ServiceTypeSpecified = true;
            //request.RequestedShipment.PackagingType = PackagingType.YOUR_PACKAGING; // Packaging type FEDEX_BOK, FEDEX_PAK, FEDEX_TUBE, YOUR_PACKAGING, ...
            //request.RequestedShipment.PackagingTypeSpecified = true;

            request.RequestedShipment.TotalInsuredValue = new Money();

            request.RequestedShipment.TotalInsuredValue.Amount = (decimal)_totalPrice; // Не использовать ShoppingCart.TotalPrice - не доступен из потока
            request.RequestedShipment.TotalInsuredValue.Currency = _preOrder.Currency.Iso3;
            request.RequestedShipment.ShipTimestamp = DateTime.Now; // Shipping date and time
            request.RequestedShipment.ShipTimestampSpecified = true;
            request.RequestedShipment.RateRequestTypes = new RateRequestType[2];
            request.RequestedShipment.RateRequestTypes[0] = RateRequestType.ACCOUNT;
            request.RequestedShipment.RateRequestTypes[1] = RateRequestType.LIST;
            request.RequestedShipment.PackageDetail = RequestedPackageDetailType.INDIVIDUAL_PACKAGES;
            request.RequestedShipment.PackageDetailSpecified = true;
        }

        private void SetOrigin(RateRequest request)
        {
            request.RequestedShipment.Shipper = new Party();
            request.RequestedShipment.Shipper.Address = new Address();
            request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { _addressFrom };
            request.RequestedShipment.Shipper.Address.City = _cityFrom;
            request.RequestedShipment.Shipper.Address.StateOrProvinceCode = _stateFrom;
            request.RequestedShipment.Shipper.Address.PostalCode = _postalCodeFrom;
            request.RequestedShipment.Shipper.Address.CountryCode = _countryCodeFrom;
        }

        private void SetDestination(RateRequest request)
        {
            request.RequestedShipment.Recipient = new Party();
            request.RequestedShipment.Recipient.Address = new Address();
            request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { _preOrder.AddressDest };
            request.RequestedShipment.Recipient.Address.City = _preOrder.CityDest;
            request.RequestedShipment.Recipient.Address.StateOrProvinceCode = _preOrder.RegionDest;
            request.RequestedShipment.Recipient.Address.PostalCode = _preOrder.ZipDest;
            request.RequestedShipment.Recipient.Address.CountryCode = _preOrder.CountryIso;
        }

        private void SetPayment(RateRequest request)
        {
            request.RequestedShipment.ShippingChargesPayment = new AdvantShop.Core.FedexRateServiceWebReference.Payment(); //new RateWebServiceClient.RateServiceWebReference.Payment(); 
            request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER; // Payment options are RECIPIENT, SENDER, THIRD_PARTY
            request.RequestedShipment.ShippingChargesPayment.PaymentTypeSpecified = true;
            request.RequestedShipment.ShippingChargesPayment.Payor = new Payor();
            request.RequestedShipment.ShippingChargesPayment.Payor.AccountNumber = AccountNumber; // Replace "XXX" with client's account number
        }

        private void SetIndividualPackageLineItems(RateRequest request)
        {
            // ------------------------------------------
            // Passing individual pieces rate request
            // ------------------------------------------
            var weight = _preOrder.Items.Sum(item => item.Weight * item.Amount);
            if (!IsPackageTooHeavy(weight))
            {
                request.RequestedShipment.PackageCount = "1";

                request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber = "1"; // package sequence number            
                request.RequestedShipment.RequestedPackageLineItems[0].Weight = new Weight(); // package weight
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Units = WeightUnits.KG;
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Value = (decimal)weight;
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions(); // package dimensions

                //it's better to don't pass dims now
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length = "0";
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width = "0";
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height = "0";
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units = LinearUnits.IN;
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue = new Money(); // insured value

                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount = (decimal)_totalPrice;
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency = _preOrder.Currency.Iso3;

            }
            else
            {
                int totalPackages = 1;
                int totalPackagesWeights = 1;
                if (IsPackageTooHeavy(weight))
                {
                    totalPackagesWeights = SQLDataHelper.GetInt(Math.Ceiling(weight / MaxPackageWeight));
                }

                totalPackages = totalPackagesWeights;
                if (totalPackages == 0)
                    totalPackages = 1;

                float weight2 = weight / totalPackages;

                if (weight2 < 1)
                    weight2 = 1;

                float orderSubTotal2 = _preOrder.Items.Sum(item => item.Price * item.Amount) / totalPackages;

                request.RequestedShipment.PackageCount = totalPackages.ToString();
                request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[totalPackages];

                for (int i = 0; i < totalPackages; i++)
                {
                    request.RequestedShipment.RequestedPackageLineItems[i] = new RequestedPackageLineItem();
                    request.RequestedShipment.RequestedPackageLineItems[i].SequenceNumber = (i + 1).ToString(); // package sequence number            
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight = new Weight(); // package weight
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = WeightUnits.KG;
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight.Value = (decimal)weight2;
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions = new Dimensions(); // package dimensions

                    //it's better to don't pass dims now
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Length = "0";
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Width = "0";
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Height = "0";
                    request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = LinearUnits.CM;
                    request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue = new Money(); // insured value
                    request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Amount = (decimal)orderSubTotal2;
                    request.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Currency = _preOrder.Currency.Iso3;
                }
            }
        }

        private static bool IsPackageTooHeavy(float weight)
        {
            if (weight > MaxPackageWeight)
                return true;
            return false;
        }

        private List<BaseShippingOption> ParseAnswer(RateReply reply)
        {
            var res = new List<BaseShippingOption>();
            var enabledServices = EnabledService;
            foreach (var rateDetail in reply.RateReplyDetails)
            {
                var shippingOption = new BaseShippingOption(_method);
                if (!String.IsNullOrEmpty(enabledServices) && !enabledServices.Contains(rateDetail.ServiceType.ToString()))
                {
                    continue;
                }
                string serviceName = GetServiceName(rateDetail.ServiceType.ToString());
                shippingOption.NameRate = serviceName;
                foreach (RatedShipmentDetail shipmentDetail in rateDetail.RatedShipmentDetails)
                {
                    shippingOption.Rate = Rate > 0 ? (float)shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount * Rate + Extracharge
                                                   : (float)shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount + Extracharge;

                    // Vladimir: Старый код вытаскивал только некоторые Rate. Не знаю зачем. Пусть будут все.
                    //if (shipmentDetail.ShipmentRateDetail.RateType == ReturnedRateType.PAYOR_ACCOUNT_SHIPMENT)
                    //{
                    //    shippingOption.Rate = shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount + Extracharge;
                    //    if (Rate > 0)
                    //    {
                    //        shippingOption.Rate *= Rate;
                    //    }
                    //    break;
                    //}

                    //if (shipmentDetail.ShipmentRateDetail.RateType == ReturnedRateType.PAYOR_LIST_SHIPMENT) // Get List Rates (not discount rates)
                    //{
                    //    shippingOption.Rate = Rate > 0 ? shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount * Rate + Extracharge 
                    //                                   : shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount + Extracharge;
                    //    break;
                    //}

                    //var shippingRate = shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount + Extracharge;
                    //if (Rate > 0)
                    //{
                    //    shippingRate *= Rate;
                    //}
                    //shippingOption.Rate = shippingRate;
                }
                res.Add(shippingOption);
            }
            return res;
        }
    }
}
*/