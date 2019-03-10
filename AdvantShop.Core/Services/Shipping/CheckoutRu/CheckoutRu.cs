using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Payment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AdvantShop.Core.Services.Orders;

namespace AdvantShop.Shipping.CheckoutRu
{
    [ShippingKey("CheckoutRu")]
    public class CheckoutRu : BaseShippingWithCargoAndCache
    {
        private readonly Dictionary<DeliveryType, string> _deliveryTypeName = new Dictionary<DeliveryType, string>
        {
            {DeliveryType.Postamat, "Постоматы"},
            {DeliveryType.Pvz, "Пункты выдачи"},
            {DeliveryType.Express, "Курьерская доставка"},
            {DeliveryType.Mail, "Доставка почтой"}
        };

        private readonly Dictionary<int, string> _deliveryCompanies = new Dictionary<int, string>{
            {1, "DPD"},
            {2, "PickPoint"},
            {3, "Собственная доставка магазина"},
            {4, "Hermes"},
            {5, "СПСР-Экспресс"},
            {6, "DPD"},
            {7, "DPD"},
            {8, "ShopLogistics"},
            {9, "Boxberry"},
            {10, "DPD"},
            {11, "Почта России"},
            {13, "B2Cpl"},
            {14, "ПЭК"},
            {16, "СДЭК"}
          };

        private const string Url = "http://platform.checkout.ru/service/";
        protected string ClientId { get; set; }
        protected bool Grouping { get; set; }
        //public float Extracharge { get; set; }
        //public ExtrachargeType ExtrachargeType { get; set; }

        public CheckoutRu(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            ClientId = _method.Params.ElementOrDefault(CheckoutRuTemplate.ClientId);
            Grouping = Convert.ToBoolean(_method.Params.ElementOrDefault(CheckoutRuTemplate.Grouping));
            //ExtrachargeType = (ExtrachargeType)_method.Params.ElementOrDefault(CheckoutRuTemplate.ExtrachargeType).TryParseInt();
            //Extracharge = _method.Params.ElementOrDefault(CheckoutRuTemplate.Extracharge).TryParseFloat();
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            this.DefaultIfNotSet();
            var listShippingOptions = new List<BaseShippingOption>();
            var ticket = GetTicket();
            var placeId = GetPlaceId(ticket, _preOrder.CityDest);

            var totalPrice = _preOrder.Items.Sum(item => item.Price * item.Amount);
            var totalWeight = _preOrder.Items.Sum(item => item.Weight * item.Amount);
            var totalItems = _preOrder.Items.Sum(item => item.Amount);

            var costs = GetCosts(ticket, placeId, totalPrice, totalWeight, totalItems);

            if (Grouping)
            {
                foreach (var deliveryTypeName in _deliveryTypeName)
                {
                    listShippingOptions.AddRange(
                        GetShippingOptionsHelper(costs.GetDeliveries(deliveryTypeName.Key), deliveryTypeName.Key, deliveryTypeName.Value));
                }
            }
            else
            {
                foreach (var deliveryTypeName in _deliveryTypeName)
                {
                    foreach (var deliveryCompany in _deliveryCompanies)
                    {
                        listShippingOptions.AddRange(
                            GetShippingOptionsHelper(costs.GetDeliveries(deliveryTypeName.Key, deliveryCompany.Key), deliveryTypeName.Key, string.Format("{0} - {1}", deliveryCompany.Value, deliveryTypeName.Value)));
                    }
                }
            }

            return listShippingOptions;
        }

        private List<BaseShippingOption> GetShippingOptionsHelper(List<CheckoutDeliveryModel> deliveryCost, DeliveryType deliveryType, string deliveryName)
        {
            var listShippingOptions = new List<BaseShippingOption>();

            if (!deliveryCost.Any())
            {
                return new List<BaseShippingOption>();
            }

            if (deliveryType == DeliveryType.Postamat || deliveryType == DeliveryType.Pvz)
            {
                var shippingPoints = new List<CheckoutPoint>();
                int index = 1;
                foreach (var delivery in deliveryCost)
                {
                    shippingPoints.Add(
                        new CheckoutPoint
                        {
                            Id = index,
                            Code = delivery.Code,
                            Address = delivery.Address,
                            Description = delivery.AdditionalInfo,
                            Delivery = delivery.DeliveryId.ToString(),
                            DeliveryType = deliveryType.ToString(),
                            MinDeliveryTerm = delivery.MinDeliveryTerm.ToString(),
                            MaxDeliveryTerm = delivery.MaxDeliveryTerm.ToString(),
                            Rate = ProcessRate(delivery.Cost)
                        });

                    index++;
                }

                listShippingOptions.Add(
                    new CheckoutPointOption(_method)
                    {
                        Name = deliveryName,
                        Rate = shippingPoints[0].Rate,
                        DeliveryTime = (deliveryCost[0].MinDeliveryTerm != deliveryCost[0].MaxDeliveryTerm
                            ? deliveryCost[0].MinDeliveryTerm + " - " + deliveryCost[0].MaxDeliveryTerm
                            : deliveryCost[0].MinDeliveryTerm.ToString()) + " дн.",
                        ShippingPoints = shippingPoints,
                        DeliveryType = deliveryType.ToString(),
                        IconName = ShippingIcons.GetShippingIcon(_method.ShippingType, _method.IconFileName.PhotoName, deliveryName),
                    });
            }
            else if (deliveryType == DeliveryType.Express || deliveryType == DeliveryType.Mail)
            {

                listShippingOptions.Add(
                    new CheckoutOption(_method)
                    {
                        Name = deliveryName,
                        Rate = ProcessRate(deliveryCost[0].Cost),
                        DeliveryTime = (deliveryCost[0].MinDeliveryTerm != deliveryCost[0].MaxDeliveryTerm
                            ? deliveryCost[0].MinDeliveryTerm + " - " + deliveryCost[0].MaxDeliveryTerm
                            : deliveryCost[0].MinDeliveryTerm.ToString()) + " дн.",
                        DeliveryId = deliveryCost[0].DeliveryId.ToString(),
                        DeliveryType = deliveryType.ToString(),
                        MinDeliveryTerm = deliveryCost[0].MinDeliveryTerm.ToString(),
                        MaxDeliveryTerm = deliveryCost[0].MaxDeliveryTerm.ToString(),
                        IconName = ShippingIcons.GetShippingIcon(_method.ShippingType, _method.IconFileName.PhotoName, deliveryName),
                        //DisplayCustomFields = false
                    });

            }

            return listShippingOptions;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public CheckoutResponse CreateOrder(Order order)
        {
            if (order == null)
            {
                return new CheckoutResponse { error = true, errorMessage = "Не найден заказ" };
            }
            var additionalData = new CheckoutOption();

            try
            {
                additionalData = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckoutOption>(order.OrderPickPoint.AdditionalData);
            }
            catch (Exception)
            {
                return new CheckoutResponse { error = true, errorMessage = "Невозможно разобрать место доставки" };
            }

            var ticket = GetTicket();

            var placeId = GetPlaceId(ticket, order.OrderCustomer.City);
            var streetId = GetStreetFias(ticket, placeId, order.OrderCustomer.Street);

            if (string.IsNullOrEmpty(streetId) && (string.Equals(additionalData.DeliveryType, DeliveryType.Express.ToString()) || string.Equals(additionalData.DeliveryType, DeliveryType.Mail.ToString())))
            {
                return new CheckoutResponse { error = true, errorMessage = "Неверный адрес, не удается получить код улицы" };
            }

            var zip = 0;
            if (!Int32.TryParse(order.OrderCustomer.Zip, out zip) && (string.Equals(additionalData.DeliveryType, DeliveryType.Express.ToString()) || string.Equals(additionalData.DeliveryType, DeliveryType.Mail.ToString())))
            {
                return new CheckoutResponse { error = true, errorMessage = "Неверный почтовый индекс" };
            }

            var createorder = new
            {
                apiKey = ClientId,
                order = new
                {
                    goods = order.OrderItems.Select(orderItem => new
                    {
                        payCost = orderItem.Price.ToString("F2").Replace(",", "."),
                        assessedCost = orderItem.Price.ToString("F2").Replace(",", "."),
                        code = orderItem.ArtNo,
                        name = orderItem.Name,
                        quantity = orderItem.Amount < 1 ? 1 : Convert.ToInt32(orderItem.Amount),
                        variantCode = string.Empty,
                        weight = orderItem.Weight.ToString().Replace(",", ".")
                    }).ToList(),
                    user = new
                    {
                        email = order.OrderCustomer.Email,
                        fullname = order.OrderCustomer.LastName + " " + order.OrderCustomer.FirstName,
                        phone = order.OrderCustomer.Phone
                    },
                    comment = order.CustomerComment,
                    shopOrderId = order.OrderID,
                    paymentMethod = order.PaymentMethod is Cash ? "cash" : "nocashpay",
                    delivery = new
                    {
                        deliveryId = additionalData.DeliveryId, //order.OrderPickPoint.PickPointId,
                        placeFiasId = placeId,
                        courierOptions = new List<string> { "none" },
                        addressExpress = additionalData.DeliveryType == DeliveryType.Express.ToString() || additionalData.DeliveryType == DeliveryType.Mail.ToString() ? new { postindex = order.OrderCustomer.Zip, streetFiasId = streetId, house = order.OrderCustomer.GetCustomerAddress() } : null,
                        addressPvz = additionalData.DeliveryType != DeliveryType.Express.ToString() && additionalData.DeliveryType != DeliveryType.Mail.ToString() ? order.OrderPickPoint.PickPointAddress : null,
                        cost = Math.Round(order.ShippingCost, 2).ToString("F2").Replace(",", "."),
                        type = additionalData.DeliveryType.ToLower(),
                        minTerm = additionalData.MinDeliveryTerm,
                        maxTerm = additionalData.MaxDeliveryTerm
                    },

                    forcedCost = Math.Round(order.ShippingCost, 2).ToString("F2").Replace(",", ".")
                    }
                
            };
            return PostRequestGetObject<CheckoutResponse>(
                 "order/create",
                 Newtonsoft.Json.JsonConvert.SerializeObject(createorder),
                 "POST");
        }

        private string GetTicket()
        {
            var response = PostRequestGetObject<CheckoutTicket>("login/ticket/" + ClientId);
            if (response != null)
            {
                return response.ticket;
            }
            return string.Empty;
        }

        private string GetPlaceId(string ticket, string city)
        {
            var response = PostRequestGetObject<CheckoutCitySuggestions>(string.Format(
                "checkout/getPlacesByQuery?ticket={0}&place={1}", ticket, city));

            if (response.suggestions.Count > 0)
            {
                return response.suggestions[0].id;
            }

            return string.Empty;
        }

        protected string GetStreetFias(string ticket, string placeId, string streetname)
        {
            var response = PostRequestGetObject<CheckoutStreetSuggestions>(string.Format("checkout/getStreetsByQuery?ticket={0}&placeId={1}&street={2}", ticket, placeId, streetname.Split(new[] { ',', ' ' })[0]));

            if (response.suggestions.Count > 0)
            {
                return response.suggestions[0].id;
            }

            return string.Empty;
        }

        protected CheckoutCost GetCosts(string ticket, string placeId, float totalPrice, float totalWeight, float totalItems)
        {
            var str =
                string.Format(
                    "checkout/calculation?ticket={0}&placeId={1}&totalSum={2}&assessedSum={3}&totalWeight={4}&itemsCount={5}&paymentMethod=cash",
                    ticket,
                    placeId,
                    totalPrice.ToString("F2").Replace(",", "."),
                    totalPrice.ToString("F2").Replace(",", "."),
                    totalWeight.ToString().Replace(",", "."),
                    Math.Ceiling(totalItems));
            return PostRequestGetObject<CheckoutCost>(str);
        }
        
        private T PostRequestGetObject<T>(string urlPath, string data = "", string requestMethod = "GET")
        {
            var request = WebRequest.Create(Url + urlPath);
            request.Method = requestMethod;

            if (!string.IsNullOrEmpty(data))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                request.Timeout = 3000;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
            }

            using (var response = request.GetResponse())
            {
                var responseString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

                return typeof(T) != typeof(string) ? Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseString) : (T)(object)responseString;
            }
        }

        protected override int GetHashForCache()
        {
            var totalPrice = _preOrder.Items.Sum(item => item.Price * item.Amount);
            var str = string.Format("checkout/calculation?ClientId={0}&CityDest={1}&totalSum={2}&assessedSum={3}&totalWeight={4}&itemsCount={5}&paymentMethod=cash",
                ClientId,
                _preOrder.CityDest,
                Math.Ceiling(totalPrice),
                Math.Ceiling(totalPrice),
                 _preOrder.Items.Sum(item => item.Weight * item.Amount).ToString().Replace(",", "."),
                Math.Ceiling(_preOrder.Items.Sum(x => x.Amount)));
            var hash = _method.ShippingMethodId ^ str.GetHashCode();
            return hash;
        }

        private float ProcessRate(float rate)
        {
            //if (ExtrachargeType == ExtrachargeType.Percent)
            //    return (rate + rate * Extracharge / 100) *_preOrder.Currency.Rate;
            return rate;
        }
    }
}