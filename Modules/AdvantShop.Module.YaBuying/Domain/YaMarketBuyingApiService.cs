//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using Newtonsoft.Json;
using Debug = AdvantShop.Diagnostics.Debug;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Shipping.ShippingYandexDelivery;
using AdvantShop.Taxes;

namespace AdvantShop.Module.YaBuying.Domain
{
    public class YaMarketByuingApiService
    {
        public const string ApiUrl = "yamarket/api";

        private const string MarketApiUrl = "https://api.partner.market.yandex.ru/v2/campaigns/{0}/orders/{1}/status.{2}";


        #region Help methods

        private static bool CheckAuthorizationToken(string auth)
        {
            return auth.IsNotEmpty() && YaMarketBuyingSettings.AuthToken.IsNotEmpty() && YaMarketBuyingSettings.AuthToken == auth;
        }

        private static string GetByType(string type, YaMarketRegion region)
        {
            if (region == null)
                return string.Empty;

            if (region.type == type)
                return region.name;

            return GetByType(type, region.parent);
        }

        private static void WriteError(string error)
        {
            var context = HttpContext.Current;

            context.Response.Status = "400 Bad Request";
            context.Response.Write(error);
            context.Response.End();
        }

        private static void WriteUnauthorized(string error)
        {
            var context = HttpContext.Current;

            context.Response.Status = "403 Forbidden";
            context.Response.Write(error);
            context.Response.End();
        }

        private static List<DateTime> CalcDeliveryDays(DateTime from, DateTime to)
        {
            var timeDeliveryStore = YaMarketBuyingSettings.TimeDeliveryForSchedule.Split(':');
            List<DateTime> dateList = new List<DateTime>(), dateListShort = new List<DateTime>();
            var date = from;
            while (date <= to)
            {
                dateList.Add(date);
                date = date.AddDays(1);
            }

            dateListShort = dateList.Where(x => YaMarketBuyingSettings.ScheduleDelivery.Contains(x.DayOfWeek.ToString().ToLower())).ToList();

            if (dateListShort.Count > 0)
            {
                var firstDate = dateList[0] <= DateTime.Now;
                for (DateTime item = dateList[0]; dateList.Contains(item); item = item.AddDays(1))
                {
                    if (dateListShort.Contains(item) || firstDate && dateListShort.Contains(item.AddDays(1)) && 
                        dateList[0] > new DateTime(item.Year,item.Month,item.Day,timeDeliveryStore[0].TryParseInt(), timeDeliveryStore[1].TryParseInt(),0))
                    {
                        dateList.Remove(item);
                        dateList.Add(dateList.Last().AddDays(1));
                    }

                    firstDate = false;
                }
            }
                        
            return dateList.OrderBy(x => x.Date).ToList();
        }
        
        #endregion

        public static bool RewritePath(string rawUrl, ref string newUrl)
        {
            if (string.IsNullOrEmpty(rawUrl) || !rawUrl.Contains(ApiUrl))
                return false;

            var url = rawUrl.Split("?").FirstOrDefault();

            var auth = HttpContext.Current.Request.Headers["Authorization"];
            if (!CheckAuthorizationToken(auth))
            {
                Debug.Log.Error("Yamarket Invalid AuthToken: " + url + " auth: " + auth);
                WriteUnauthorized("Invalid AuthToken");
            }

            var json = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();

            Debug.Log.Error("Yamarket start: " + url + " json: " + json);

            var method = url.Replace(ApiUrl, "").Trim(new[] { '/' });
            if (method.Contains("cart"))
            {
                Cart(json);
            }
            else if (method.Contains("order/accept"))
            {
                Accept(json);
            }
            else if (method.Contains("order/status"))
            {
                Status(json);
            }

            return true;
        }

        #region POST /cart
        protected static void Cart(string json)
        {
            var yaCart = JsonConvert.DeserializeObject<YaCart>(json);
            if (yaCart == null || yaCart.cart == null)
            {
                Debug.Log.Error("Yamarket Empty cart. json: " + json);
                WriteError("Empty cart");
                return;
            }

            var yaResponse = new YaMarketCartResponse();

            try
            {
                var shoppingCart = new ShoppingCart();

                foreach (var marketItem in yaCart.cart.items)
                {
                    var offer = OfferService.GetOffer(marketItem.offerId) ?? OfferService.GetOffer(Convert.ToInt32(marketItem.offerId));
                    var isEnabled = offer != null && offer.Product.Enabled;

                    var priceWithDiscount = (float)Math.Round(PriceService.GetFinalPrice(offer.BasePrice, offer.Product.Discount, offer.Product.Currency.Rate));

                    yaResponse.items.Add(new YaMarketItem(marketItem)
                    {
                        price = isEnabled ? priceWithDiscount : 0,
                        count = isEnabled
                                    ? (offer.Amount > 0
                                        ? offer.Amount
                                        : (offer.Product.AllowPreOrder ? 99999 : 0))
                                    : 0,
                        delivery = isEnabled
                    });

                    if (isEnabled)
                    {
                        shoppingCart.Add(new ShoppingCartItem()
                        {
                            OfferId = offer.OfferId,
                            Amount = marketItem.count,
                            AttributesXml = string.Empty,
                            ShoppingCartType = ShoppingCartType.ShoppingCart,
                        });
                    }
                }

                var city = GetByType("CITY", yaCart.cart.delivery.region) ?? string.Empty;
                var region = GetByType("REGION", yaCart.cart.delivery.region);
                if (string.IsNullOrEmpty(region))
                    region = GetByType("SUBJECT_FEDERATION", yaCart.cart.delivery.region) ?? string.Empty;

                var country = GetByType("COUNTRY", yaCart.cart.delivery.region) ?? "Россия";
                if (string.IsNullOrWhiteSpace(country))
                    country = "Россия";

                var preOrder = new PreOrder
                {
                    Items = shoppingCart.Select(shpItem => new PreOrderItem(shpItem)).ToList(), //ShoppingCartService.CurrentShoppingCart.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                    CountryDest = country,
                    RegionDest = region,
                    CityDest = city,
                    Currency = CurrencyService.GetCurrencyByIso3(SettingsCatalog.DefaultCurrencyIso3)
                };
                var manager = new ShippingManager(preOrder);
                var options = manager.GetOptions();

                var yaMarketShippings = YaMarketByuingService.GetShippings();

                foreach (var shippingRate in options.OrderBy(x => x.Rate))
                {
                    var shipping = yaMarketShippings.FirstOrDefault(x => x.ShippingMethodId == shippingRate.MethodId);
                    if (shipping == null)
                        continue;

                    if (shippingRate is YandexDeliveryPickupPointOption)
                        continue;

                    var deliveryName = shippingRate.NameRate ?? shippingRate.Name;
                    var datesDilivery = CalcDeliveryDays(DateTime.Now.AddDays(shipping.MinDate), DateTime.Now.AddDays(shipping.MaxDate));

                    var delyveryType = YaMarketByuingService.GetShippingType(deliveryName, shipping.Type);

                    var delivery = new YaMarketDeliveryResponse()
                    {
                        id = shippingRate.Id.ToString(),
                        type = delyveryType,
                        serviceName = deliveryName.Reduce(50),
                        price = shippingRate.Rate > 0 ? shippingRate.Rate : 0,
                        outlets = delyveryType == "PICKUP" ? new List<YaMarketOutlet> { new YaMarketOutlet() { id = shippingRate.MethodId } } : null,
                        dates = new YaMarketDate()
                        {
                            fromDate = datesDilivery.First().ToString("dd-MM-yyyy"),   // todo Vladimir: пока выводим текущую дату  + shipping.MinDate и + shipping.MaxDate
                            toDate = datesDilivery.Last().ToString("dd-MM-yyyy"),      // а так должно актуальное возвращаться
                            reservedUntil = delyveryType == "PICKUP" ? datesDilivery.Last().ToString("dd-MM-yyyy") : null
                        }
                    };
                    
                    if (delivery.type == string.Empty)
                        continue;
                    
                    yaResponse.deliveryOptions.Add(delivery);
                }

                if (yaResponse.deliveryOptions.Count == 0 && YaMarketBuyingSettings.EnableDefaultShipping && YaMarketBuyingSettings.DefaultShippingType.IsNotEmpty())
                {
                    var defaultDelivery = new YaMarketDeliveryResponse()
                    {
                        id = "default-shipping",
                        type = YaMarketBuyingSettings.DefaultShippingType,
                        serviceName = YaMarketBuyingSettings.DefaultShippingName,
                        price = YaMarketBuyingSettings.DefaultShippingPrice,
                        dates = new YaMarketDate()
                        {
                            fromDate = DateTime.Now.AddDays(YaMarketBuyingSettings.DefaultShippingMinDate).ToString("dd-MM-yyyy"),
                            toDate = DateTime.Now.AddDays(YaMarketBuyingSettings.DefaultShippingMaxDate).ToString("dd-MM-yyyy"),
                            reservedUntil =
                                YaMarketBuyingSettings.DefaultShippingType == "PICKUP"
                                    ? DateTime.Now.AddDays(YaMarketBuyingSettings.DefaultShippingMaxDate).ToString("dd-MM-yyyy")
                                    : null
                        }
                    };
                    yaResponse.deliveryOptions.Add(defaultDelivery);
                }

                foreach (var payment in YaMarketBuyingSettings.Payments.Split(';'))
                {
                    if (!string.IsNullOrEmpty(country))
                    {
                        var countries = YaMarketByuingService.GetPaymentCountries(payment);
                        if (countries != null && countries.Count > 0)
                        {
                            if (countries.Find(x => x.Name.ToLower() == country.ToLower()) == null)
                                continue;
                        }
                    }

                    if (!string.IsNullOrEmpty(city))
                    {
                        var cities = YaMarketByuingService.GetPaymentCities(payment);
                        if (cities != null && cities.Count > 0)
                        {
                            if (cities.Find(x => x.Name.ToLower() == city.ToLower()) == null)
                                continue;
                        }
                    }

                    yaResponse.paymentMethods.Add(payment);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            try
            {
                var context = HttpContext.Current;
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(new {cart = yaResponse}));

                context.Response.Flush(); // Sends all currently buffered output to the client.
                context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                context.Response.End();
            }
            catch (ThreadAbortException ex)
            {
                
            }
        }
        #endregion

        #region POST /order/accept
        protected static void Accept(string json)
        {
            var yaOrder = JsonConvert.DeserializeObject<YaMarketOrderRequest>(json);
            if (yaOrder == null || yaOrder.order == null)
                return;

            var yaExistOrder = YaMarketByuingService.GetOrder(yaOrder.order.id.TryParseInt());
            var orderId = yaExistOrder != null ? yaExistOrder.OrderId : 0;

            if (orderId == 0)
            {
                try
                {
                    Order order = null;

                    var adminComment = "";

                    adminComment = "Заказ номер: " + yaOrder.order.id + (yaOrder.order.fake ? "(тестовый)" : "") + "\r\n";

                    if (yaOrder.order.paymentType.IsNotEmpty())
                        adminComment += "Тип оплаты заказа: " +
                                        (yaOrder.order.paymentType == "PREPAID"
                                            ? "предоплата"
                                            : "постоплата при получении заказа") + "\r\n";

                    if (yaOrder.order.paymentMethod.IsNotEmpty())
                    {
                        adminComment += "Способ оплаты заказа: ";
                        switch (yaOrder.order.paymentMethod)
                        {
                            case "YANDEX":
                                adminComment += "оплата при оформлении";
                                break;
                            case "SHOP_PREPAID":
                                adminComment += "предоплата напрямую магазину (только для Украины)";
                                break;
                            case "CASH_ON_DELIVERY":
                                adminComment += "наличный расчет при получении заказа";
                                break;
                            case "CARD_ON_DELIVERY":
                                adminComment += "оплата банковской картой при получении заказа";
                                break;
                        }
                    }

                    adminComment += "\r\n";

                    var orderCustomer = new OrderCustomer()
                    {
                        CustomerID = Guid.NewGuid(),
                        Email = "YaMarket",
                        CustomerIP = "127.0.0.1",
                    };
                    var shippingCost = 0f;
                    var shippingMethodName = "";
                    var shippingMethodId = 0;
                    var shippingTaxType = TaxType.Without;

                    if (yaOrder.order.delivery != null)
                    {
                        adminComment += string.Format("Доставка: {0}, стоимость доставки: {1}, даты: {2} до {3}\r\n",
                            yaOrder.order.delivery.serviceName, yaOrder.order.delivery.price ?? 0,
                            yaOrder.order.delivery.dates.fromDate, yaOrder.order.delivery.dates.toDate);

                        if (yaOrder.order.delivery.address != null)
                        {
                            var address = new StringBuilder();

                            if (yaOrder.order.delivery.address.street.IsNotEmpty())
                            {
                                orderCustomer.Street = yaOrder.order.delivery.address.street;
                                address.AppendFormat("ул. {0} ", yaOrder.order.delivery.address.street);
                            }

                            if (yaOrder.order.delivery.address.house.IsNotEmpty())
                            {
                                orderCustomer.House = yaOrder.order.delivery.address.house;
                                address.AppendFormat("д. {0} ", yaOrder.order.delivery.address.house);
                            }

                            if (yaOrder.order.delivery.address.subway.IsNotEmpty())
                            {
                                orderCustomer.Street += "метро " + yaOrder.order.delivery.address.subway;
                                address.AppendFormat("метро {0} ", yaOrder.order.delivery.address.subway);
                            }

                            if (yaOrder.order.delivery.address.block.IsNotEmpty())
                            {
                                orderCustomer.Structure = yaOrder.order.delivery.address.block;
                                address.AppendFormat("строение {0} ", yaOrder.order.delivery.address.block);
                            }

                            if (yaOrder.order.delivery.address.entrance.IsNotEmpty())
                            {
                                orderCustomer.Entrance = yaOrder.order.delivery.address.entrance;
                                address.AppendFormat("подъезд {0} ", yaOrder.order.delivery.address.entrance);
                            }

                            if (yaOrder.order.delivery.address.entryphone.IsNotEmpty())
                            {
                                orderCustomer.Entrance += "домофон: " + yaOrder.order.delivery.address.entryphone;
                                address.AppendFormat("домофон: {0}, ", yaOrder.order.delivery.address.entryphone);
                            }

                            if (yaOrder.order.delivery.address.apartment.IsNotEmpty())
                            {
                                orderCustomer.Apartment = yaOrder.order.delivery.address.apartment;
                                address.AppendFormat("кв. {0} ", yaOrder.order.delivery.address.apartment);
                            }

                            if (yaOrder.order.delivery.address.recipient.IsNotEmpty())
                            {
                                orderCustomer.FirstName = yaOrder.order.delivery.address.recipient;
                                address.AppendFormat("ФИО получателя: {0} ", yaOrder.order.delivery.address.recipient);
                            }

                            if (yaOrder.order.delivery.address.phone.IsNotEmpty())
                            {
                                orderCustomer.Phone = yaOrder.order.delivery.address.phone;
                                address.AppendFormat("Номер телефона получателя: {0} ", yaOrder.order.delivery.address.phone);
                            }

                            orderCustomer.Country = yaOrder.order.delivery.address.country;
                            orderCustomer.Region = string.Empty;
                            orderCustomer.City = yaOrder.order.delivery.address.city;
                            orderCustomer.Zip = yaOrder.order.delivery.address.postcode ?? string.Empty;
                        }

                        if (yaOrder.order.delivery.price != null)
                            shippingCost = (float)yaOrder.order.delivery.price;

                        shippingMethodName = yaOrder.order.delivery.serviceName;

                        if (!string.IsNullOrWhiteSpace(yaOrder.order.delivery.id))
                        {
                            shippingMethodId = yaOrder.order.delivery.id.Split('_')[0].TryParseInt();
                            var shipping = ShippingMethodService.GetShippingMethod(shippingMethodId);
                            if (shipping != null)
                                shippingTaxType = shipping.TaxType;
                        }
                    }

                    var orderItems = (from item in yaOrder.order.items
                                      let offer = OfferService.GetOffer(Convert.ToInt32(item.offerId))
                                      where offer != null
                                      let product = offer.Product
                                      let tax = product.TaxId != null ? TaxService.GetTax(product.TaxId.Value) : null
                                      select new OrderItem()
                                      {
                                          ProductID = product.ProductId,
                                          ArtNo = offer.ArtNo,
                                          Name = product.Name,
                                          Price = item.price,
                                          Amount = item.count,
                                          SupplyPrice = product.Offers[0].SupplyPrice,
                                          IsCouponApplied = false,
                                          Weight = product.Weight,
                                          Color = offer.ColorID != null ? offer.Color.ColorName : null,
                                          Size = offer.SizeID != null ? offer.Size.SizeName : null,
                                          PhotoID = offer.Photo != null ? offer.Photo.PhotoId : (int?)null,

                                          TaxId = tax != null ? tax.TaxId : default(int?),
                                          TaxName = tax != null ? tax.Name : null,
                                          TaxRate = tax != null ? tax.Rate : default(float?),
                                          TaxShowInPrice = tax != null ? tax.ShowInPrice : default(bool?),
                                          TaxType = tax != null ? tax.TaxType : default(TaxType?)
                                      }).ToList();

                    var currencies = CurrencyService.GetAllCurrencies(true);
                    var orderCurrency = yaOrder.order.currency == "RUR"
                        ? (currencies.FirstOrDefault(x => x.Iso3 == yaOrder.order.currency || x.Iso3 == "RUB") ?? currencies.FirstOrDefault())
                        : (currencies.FirstOrDefault(x => x.Iso3 == yaOrder.order.currency) ?? currencies.FirstOrDefault());

                    var orderSource = OrderSourceService.GetOrderSources();
                    if(orderSource.Count(x => x.Type == OrderType.None && x.Name == "Покупка на маркете") > 0)
                    {
                        orderSource = orderSource.Where(x => x.Type == OrderType.None && x.Name == "Покупка на маркете").ToList();
                    }
                    else
                    {
                        var newOrderSource = new OrderSource() {
                            Name = "Покупка на маркете",
                            SortOrder = 1000,
                            Main = true,
                            Type = OrderType.None
                        };
                        newOrderSource.Id = OrderSourceService.AddOrderSource(newOrderSource);
                        orderSource.Insert(0, newOrderSource);
                    }

                    order = new Order()
                    {
                        AdminOrderComment = adminComment,
                        CustomerComment = yaOrder.order.notes,
                        OrderCustomer = orderCustomer,
                        OrderItems = orderItems,
                        OrderCurrency = orderCurrency,
                        OrderStatusId = OrderStatusService.DefaultOrderStatus,
                        ShippingCost = shippingCost,
                        ShippingMethodId = shippingMethodId,
                        ShippingTaxType = shippingTaxType,
                        ArchivedShippingName = shippingMethodName,
                        OrderDate = DateTime.Now,
                        Number = yaOrder.order.id + "-ym",
                        OrderSourceId = orderSource == null ? 0 : orderSource.First().Id
                    };

                    var changedBy = new OrderChangedBy("Яндекс.Маркет бот");

                    order.OrderID = OrderService.AddOrder(order, changedBy);

                    OrderStatusService.ChangeOrderStatus(order.OrderID, OrderStatusService.DefaultOrderStatus, "Оформление заказа через Яндекс-маркет");

                    orderId = order.OrderID;
                    if (order.OrderID != 0)
                    {
                        YaMarketByuingService.AddOrder(new YaOrder()
                        {
                            MarketOrderId = yaOrder.order.id.TryParseInt(),
                            OrderId = order.OrderID,
                            Status = string.Format("[{0}] Создан заказ {1}", DateTime.Now.ToString("g"), order.OrderID)
                        });

                        try
                        {
                            var orderTable = OrderService.GenerateOrderItemsHtml(order.OrderItems, order.OrderCurrency,
                                orderItems.Sum(x => x.Price * x.Amount), 0, 0, null, null, 0, 0, 0, 0, 0, 0);

                            var mailTemplate = new BuyInOneClickMailTemplate(order.OrderID.ToString(), "", "", "", orderTable, OrderService.GetBillingLinkHash(order));
                            mailTemplate.BuildMail();

                            SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, "Заказ через Яндекс.Маркет", mailTemplate.Body, true);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log.Error(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }

            /*
             * Если магазин считает запрос, поступающий от Яндекс.Маркета, некорректным, 
             * магазин должен вернуть статус ответа 400 с описанием причины ошибки в теле ответа. 
             * Такие ответы будут анализироваться на предмет нарушений и недоработок API со стороны Яндекс.Маркета.
             * 
             */
            var orderResponse = new YaMarketOrderResponse()
            {
                order = new YaMarketOrderAccept()
                {
                    accepted = orderId != 0,
                    id = yaOrder.order.id
                }
            };

            try
            {
                var context = HttpContext.Current;
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(orderResponse));

                context.Response.Flush(); // Sends all currently buffered output to the client.
                context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                context.Response.End();
            }
            catch (ThreadAbortException ex)
            {

            }
        }
        #endregion

        #region POST order/status
        protected static void Status(string json)
        {
            try
            {
                var yaOrderStatus = JsonConvert.DeserializeObject<YaMarketOrderStatusRequest>(json);
                if (yaOrderStatus == null || yaOrderStatus.order == null)
                    return;

                var marketOrderId = yaOrderStatus.order.id.TryParseInt();

                var yaOrder = YaMarketByuingService.GetOrder(marketOrderId);
                if (yaOrder == null)
                {
                    Debug.Log.Error("нет заказа с данным id " + marketOrderId + " json: " + json);
                    WriteError("нет заказа с данным id");
                    return;
                }

                var order = OrderService.GetOrder(yaOrder.OrderId);
                if (order == null)
                {
                    // Люди удалют заказ из магазина, а яндекс продолжает слать запросы пока не забанит,
                    // поэтому отдаем сразу OK
                    var contextErr = HttpContext.Current;
                    contextErr.Response.ContentType = "application/json";
                    contextErr.Response.Write("OK");
                    contextErr.Response.End();

                    return;

                    //Debug.LogError("нет заказа с данным id " + yaOrder.OrderId + " json: " + json);
                    //WriteError("нет заказа с данным id");
                    //return;
                }

                var status = string.Format("[{0}] Статус: ", DateTime.Now.ToString("g"));

                switch (yaOrderStatus.order.status)
                {
                    case "UNPAID":
                        status += "заказ оформлен, но еще не оплачен (если выбрана оплата при оформлении)";
                        if (YaMarketBuyingSettings.UpaidStatusId != 0)
                        {
                            OrderStatusService.ChangeOrderStatus(order.OrderID, YaMarketBuyingSettings.UpaidStatusId, "Смена статуса заказа на Яндекс-маркет" + marketOrderId, false);
                            order.OrderStatusId = YaMarketBuyingSettings.UpaidStatusId;
                        }
                        break;
                    case "PROCESSING":
                        status += "заказ можно выполнять";
                        var processingStatusIds = YaMarketBuyingSettings.ProcessingStatusesIds;
                        if (processingStatusIds != null && processingStatusIds.Count > 0)
                        {
                            var statusId = processingStatusIds.FirstOrDefault();
                            OrderStatusService.ChangeOrderStatus(order.OrderID, statusId, "Смена статуса заказа на Яндекс-маркет" + marketOrderId, false);
                            order.OrderStatusId = statusId;
                        }
                        break;
                    case "CANCELLED":
                        status += "заказ отменен: ";
                        var canceledStatusId = 0;

                        if (yaOrderStatus.order.substatus.IsNotEmpty())
                        {
                            switch (yaOrderStatus.order.substatus)
                            {
                                case "RESERVATION_EXPIRED":
                                    status += "покупатель не завершил оформление зарезервированного заказа вовремя";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_RESERVATION_EXPIRED;
                                    break;
                                case "USER_NOT_PAID":
                                    status += "покупатель не оплатил заказ (для типа оплаты PREPAID)";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_USER_NOT_PAID;
                                    break;
                                case "USER_UNREACHABLE":
                                    status += "не удалось связаться с покупателем";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_USER_UNREACHABLE;
                                    break;
                                case "USER_CHANGED_MIND":
                                    status += "покупатель отменил заказ по собственным причинам";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_USER_CHANGED_MIND;
                                    break;
                                case "USER_REFUSED_DELIVERY":
                                    status += "покупателя не устраивают условия доставки";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_DELIVERY;
                                    break;
                                case "USER_REFUSED_PRODUCT":
                                    status += "покупателю не подошел товар";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_PRODUCT;
                                    break;
                                case "SHOP_FAILED":
                                    status += "магазин не может выполнить заказ";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_SHOP_FAILED;
                                    break;
                                case "USER_REFUSED_QUALITY":
                                    status += "покупателя не устраивает качество товара";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_QUALITY;
                                    break;
                                case "REPLACING_ORDER":
                                    status += "покупатель изменяет состав заказа";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_REPLACING_ORDER;
                                    break;
                                case "PROCESSING_EXPIRED":
                                    status += "магазин не обработал заказ вовремя";
                                    canceledStatusId = YaMarketBuyingSettings.CanceledStatusId_PROCESSING_EXPIRED;
                                    break;
                            }
                        }

                        if (canceledStatusId == 0)
                            canceledStatusId = OrderStatusService.CanceledOrderStatus;

                        OrderStatusService.ChangeOrderStatus(order.OrderID, canceledStatusId, "Смена статуса заказа на Яндекс-маркет" + marketOrderId, false);
                        order.OrderStatusId = canceledStatusId;
                        break;

                    case "DELIVERED":
                        status += "заказ получен покупателем ";
                        if (YaMarketBuyingSettings.DeliveredStatusId != 0)
                        {
                            OrderStatusService.ChangeOrderStatus(order.OrderID, YaMarketBuyingSettings.DeliveredStatusId, "Смена статуса заказа на Яндекс-маркет" + marketOrderId, false);
                            order.OrderStatusId = YaMarketBuyingSettings.DeliveredStatusId;
                        }
                        break;

                    case "DELIVERY":
                        status += "заказ передан в доставку ";
                        var deliveryStatusIds = YaMarketBuyingSettings.DeliveryStatusesIds;
                        if (deliveryStatusIds != null && deliveryStatusIds.Count > 0)
                        {
                            var statusId = deliveryStatusIds.FirstOrDefault();
                            OrderStatusService.ChangeOrderStatus(order.OrderID, statusId, "Смена статуса заказа на Яндекс-маркет" + marketOrderId, false);
                            order.OrderStatusId = statusId;
                        }
                        break;

                    case "PICKUP":
                        status += "заказ доставлен в пункт самовывоза ";
                        if (YaMarketBuyingSettings.PickupStatusId != 0)
                        {
                            OrderStatusService.ChangeOrderStatus(order.OrderID, YaMarketBuyingSettings.PickupStatusId, "Смена статуса заказа на Яндекс-маркет" + marketOrderId, false);
                            order.OrderStatusId = YaMarketBuyingSettings.PickupStatusId;
                        }
                        break;

                    case "RESERVED":
                        status += "заказ в резерве (ожидается подтверждение от пользователя) ";
                        if (YaMarketBuyingSettings.ReservedStatusId != 0)
                        {
                            OrderStatusService.ChangeOrderStatus(order.OrderID, YaMarketBuyingSettings.ReservedStatusId, "Смена статуса заказа на Яндекс-маркет" + marketOrderId, false);
                            order.OrderStatusId = YaMarketBuyingSettings.ReservedStatusId;
                        }
                        break;
                }

                var html = new StringBuilder();

                var paymentMethod = "";
                switch (yaOrderStatus.order.paymentMethod)
                {
                    case "YANDEX":
                        paymentMethod = "оплата при оформлении";
                        break;
                    case "SHOP_PREPAID":
                        paymentMethod = "предоплата напрямую магазину (только для Украины)";
                        break;
                    case "CASH_ON_DELIVERY":
                        paymentMethod = "наличный расчет при получении заказа";
                        break;
                    case "CARD_ON_DELIVERY":
                        paymentMethod = "оплата банковской картой при получении заказа";
                        break;
                }

                html.AppendFormat("<div>Заказ #{0} изменил свой статус.</div>", yaOrder.OrderId);
                html.AppendFormat("<div>{0}</div>", status);
                html.AppendFormat("<div>Дата оформления заказа: {0}</div>", yaOrderStatus.order.creationDate);
                html.AppendFormat("<div>Валюта: {0}</div>", yaOrderStatus.order.currency == "RUR" ? "российский рубль" : "украинская гривна");
                html.AppendFormat("<div>Сумма заказа без учета доставки: {0}</div>", yaOrderStatus.order.itemsTotal);
                html.AppendFormat("<div>Сумма заказа с учетом доставки: {0}</div>", yaOrderStatus.order.total);
                html.AppendFormat("<div>Тип оплаты заказа: {0}</div>", yaOrderStatus.order.paymentType == "PREPAID" ? "предоплата" : "постоплата при получении заказа");
                html.AppendFormat("<div>Способ оплаты заказа: {0}</div>", paymentMethod);
                html.AppendFormat("<div>Тестовый: {0}</div>", yaOrderStatus.order.fake ? "Да" : "Нет");
                html.AppendFormat("<div>Комментарий к заказу: {0}</div>", yaOrderStatus.order.notes);

                if (yaOrderStatus.order.buyer != null)
                {
                    html.Append("<div>Пользователь</div>");
                    html.AppendFormat("<div>Идентификатор покупателя: {0}</div>", yaOrderStatus.order.buyer.id);
                    html.AppendFormat("<div>Имя покупателя: {0}</div>", yaOrderStatus.order.buyer.firstName);
                    html.AppendFormat("<div>Номер телефона: {0}</div>", yaOrderStatus.order.buyer.phone);
                    html.AppendFormat("<div>Email: {0}</div>", yaOrderStatus.order.buyer.email);
                    html.AppendFormat("<div>Фамилия Отчество: {0} {1}</div>", yaOrderStatus.order.buyer.lastName, yaOrderStatus.order.buyer.middleName);

                    order.OrderCustomer.FirstName = yaOrderStatus.order.buyer.firstName;
                    order.OrderCustomer.Phone = yaOrderStatus.order.buyer.phone;
                    order.OrderCustomer.StandardPhone = StringHelper.ConvertToStandardPhone(yaOrderStatus.order.buyer.phone);
                    order.OrderCustomer.Email = yaOrderStatus.order.buyer.email;
                    order.OrderCustomer.LastName = (yaOrderStatus.order.buyer.lastName ?? string.Empty) +
                                                   (yaOrderStatus.order.buyer.middleName ?? string.Empty);

                    OrderService.UpdateOrderCustomer(order.OrderCustomer);
                }

                if (yaOrderStatus.order.delivery != null)
                {
                    html.Append("<div>Доставка</div>");
                    html.AppendFormat("<div>Метод: {0}</div>", yaOrderStatus.order.delivery.serviceName);
                    html.AppendFormat("<div>Стоимость: {0}</div>", yaOrderStatus.order.delivery.price);
                    html.AppendFormat("<div>Время: {0} до {1}</div>", yaOrderStatus.order.delivery.dates.fromDate, yaOrderStatus.order.delivery.dates.toDate);

                    if (yaOrderStatus.order.delivery.outletId != 0)
                        html.AppendFormat("<div>Id пункта самовывоза: {0}</div>", yaOrderStatus.order.delivery.outletId);

                    if (yaOrderStatus.order.delivery.address != null)
                    {
                        html.Append("<div>Адрес</div>");
                        html.AppendFormat("<div>Страна: {0}</div>", yaOrderStatus.order.delivery.address.country);
                        html.AppendFormat("<div>Город: {0}</div>", yaOrderStatus.order.delivery.address.city);
                        html.AppendFormat("<div>Номер дома: {0}</div>", yaOrderStatus.order.delivery.address.house);

                        if (yaOrderStatus.order.delivery.address.postcode.IsNotEmpty())
                            html.AppendFormat("<div>Почтовый индекс: {0}</div>", yaOrderStatus.order.delivery.address.postcode);

                        if (yaOrderStatus.order.delivery.address.street.IsNotEmpty())
                            html.AppendFormat("<div>Улица: {0}</div>", yaOrderStatus.order.delivery.address.street);

                        if (yaOrderStatus.order.delivery.address.subway.IsNotEmpty())
                            html.AppendFormat("<div>Станция метро: {0}</div>", yaOrderStatus.order.delivery.address.subway);

                        if (yaOrderStatus.order.delivery.address.block.IsNotEmpty())
                            html.AppendFormat("<div>Номер корпуса либо строения: {0}</div>", yaOrderStatus.order.delivery.address.block);

                        if (yaOrderStatus.order.delivery.address.entrance.IsNotEmpty())
                            html.AppendFormat("<div>Номер подъезда: {0}</div>", yaOrderStatus.order.delivery.address.entrance);

                        if (yaOrderStatus.order.delivery.address.entryphone.IsNotEmpty())
                            html.AppendFormat("<div>Код домофона: {0}</div>", yaOrderStatus.order.delivery.address.entryphone);

                        if (yaOrderStatus.order.delivery.address.floor.IsNotEmpty())
                            html.AppendFormat("<div>Этаж: {0}</div>", yaOrderStatus.order.delivery.address.floor);

                        if (yaOrderStatus.order.delivery.address.apartment.IsNotEmpty())
                            html.AppendFormat("<div>Номер квартиры либо офиса: {0}</div>", yaOrderStatus.order.delivery.address.apartment);

                        if (yaOrderStatus.order.delivery.address.recipient.IsNotEmpty())
                            html.AppendFormat("<div>ФИО получателя заказа: {0}</div>", yaOrderStatus.order.delivery.address.recipient);

                        if (yaOrderStatus.order.delivery.address.phone.IsNotEmpty())
                            html.AppendFormat("<div>Номер телефона получателя заказа: {0}</div>", yaOrderStatus.order.delivery.address.phone);
                    }
                }


                var result = html.ToString();

                order.AdminOrderComment = result.Replace("<div>", "").Replace("</div>", "\r\n");
                OrderService.UpdateOrderMain(order);

                YaMarketByuingService.UpdateOrder(new YaOrder()
                {
                    MarketOrderId = yaOrder.MarketOrderId,
                    OrderId = order.OrderID,
                    Status = yaOrder.Status + "\r\n------\r\n" + result.Replace("<div>", "").Replace("</div>", "\r\n")
                });

                SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, "Заказ через Яндекс.Маркет. Изменение статуса заказа", result, true);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                WriteError(ex.StackTrace);
                return;
            }

            try
            {
                var context = HttpContext.Current;
                context.Response.ContentType = "application/json";
                context.Response.Write("OK");

                context.Response.Flush(); // Sends all currently buffered output to the client.
                context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                context.Response.End();
            }
            catch (ThreadAbortException ex)
            {

            }
        }

        #endregion

        #region Status

        public static bool ChangeStatus(IOrderStatus status, IOrder order)
        {
            if (status.StatusID == YaMarketBuyingSettings.DeliveredStatusId)
            {
                return ChangeMarketStatus(order, "DELIVERED");
            }

            var processingStatusesIds = YaMarketBuyingSettings.ProcessingStatusesIds;
            if (processingStatusesIds != null && processingStatusesIds.Contains(status.StatusID))
            {
                return ChangeMarketStatus(order, "PROCESSING");
            }

            if (status.StatusID == YaMarketBuyingSettings.UpaidStatusId)
            {
                return ChangeMarketStatus(order, "UNPAID");
            }

            if (status.StatusID == YaMarketBuyingSettings.ReservedStatusId)
            {
                return ChangeMarketStatus(order, "RESERVED");
            }

            if (status.StatusID == YaMarketBuyingSettings.PickupStatusId)
            {
                return ChangeMarketStatus(order, "PICKUP");
            }

            var deliveryStatusesIds = YaMarketBuyingSettings.DeliveryStatusesIds;
            if (deliveryStatusesIds != null && deliveryStatusesIds.Contains(status.StatusID))
            {
                return ChangeMarketStatus(order, "DELIVERY");
            }

            // Сначала статусы с причиной отмены, если не нашли и статус отменяющий, то причина будет USER_UNREACHABLE — не удалось связаться с покупателем. 

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_PROCESSING_EXPIRED)
            {
                return ChangeMarketStatus(order, "CANCELLED", "PROCESSING_EXPIRED");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_REPLACING_ORDER)
            {
                return ChangeMarketStatus(order, "CANCELLED", "REPLACING_ORDER");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_RESERVATION_EXPIRED)
            {
                return ChangeMarketStatus(order, "CANCELLED", "RESERVATION_EXPIRED");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_SHOP_FAILED)
            {
                return ChangeMarketStatus(order, "CANCELLED", "SHOP_FAILED");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_USER_CHANGED_MIND)
            {
                return ChangeMarketStatus(order, "CANCELLED", "USER_CHANGED_MIND");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_USER_NOT_PAID)
            {
                return ChangeMarketStatus(order, "CANCELLED", "USER_NOT_PAID");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_DELIVERY)
            {
                return ChangeMarketStatus(order, "CANCELLED", "USER_REFUSED_DELIVERY");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_PRODUCT)
            {
                return ChangeMarketStatus(order, "CANCELLED", "USER_REFUSED_PRODUCT");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_QUALITY)
            {
                return ChangeMarketStatus(order, "CANCELLED", "USER_REFUSED_QUALITY");
            }

            if (status.StatusID == YaMarketBuyingSettings.CanceledStatusId_USER_UNREACHABLE)
            {
                return ChangeMarketStatus(order, "CANCELLED", "USER_UNREACHABLE");
            }

            if (status.IsCanceled)
            {
                return ChangeMarketStatus(order, "CANCELLED", "USER_UNREACHABLE");
            }

            return false;
        }

        protected static bool ChangeMarketStatus(IOrder order, string statusName, string substatusName = null)
        {
            var status = new YaStatus() { order = new YaStatusOrder() { status = statusName, substatus = substatusName } };

            var orderId = YaMarketByuingService.GetMarketOrderId(order.OrderID);
            if (orderId == 0)
                return false;

            var res = MakeRequest(string.Format(MarketApiUrl, YaMarketBuyingSettings.CampaignId, orderId, "json"),
                JsonConvert.SerializeObject(status, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

            if (!res) // Vladimir: вот такой вот Маркет, надо второй раз отсылать, если в первый раз ошибка
            {
                System.Threading.Thread.Sleep(3000);
                MakeRequest(string.Format(MarketApiUrl, YaMarketBuyingSettings.CampaignId, orderId, "json"),
                JsonConvert.SerializeObject(status, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            }
            return true;
        }

        private static bool MakeRequest(string api, string data)
        {
            string auth = string.Format("OAuth oauth_token=\"{0}\", oauth_client_id=\"{1}\"",
                YaMarketBuyingSettings.AuthTokenToMarket, YaMarketBuyingSettings.AuthClientId);
            try
            {
                var request = WebRequest.Create(api) as HttpWebRequest;
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Headers["Authorization"] =
                string.Format("OAuth oauth_token=\"{0}\", oauth_client_id=\"{1}\"",
                        YaMarketBuyingSettings.AuthTokenToMarket, YaMarketBuyingSettings.AuthClientId);

                if (data.IsNotEmpty())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    Debug.Log.Error(error + " **** " + data + " (auth=" + auth + ") (api=" + api + ")");
                                    return false;
                                }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
            return true;
        }

        #endregion
    }
}