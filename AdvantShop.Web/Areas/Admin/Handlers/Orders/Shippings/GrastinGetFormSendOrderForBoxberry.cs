using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping.Grastin;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Grastin;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class GrastinGetFormSendOrderForBoxberry
    {
        private readonly int _orderId;

        public GrastinGetFormSendOrderForBoxberry(int orderId)
        {
            _orderId = orderId;
        }

        public SendOrderForBoxberryModel Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return null;

            var model = new SendOrderForBoxberryModel()
            {
                OrderId = _orderId,
                OrderNumber = order.Number,
                Seats = 1,
                AssessedCost = order.Sum,
                Weight = order.OrderItems.Sum(x => x.Weight * x.Amount)
            };

            var paymentsCash = new[]
            {
                AttributeHelper.GetAttributeValue<PaymentKeyAttribute, string>(typeof(Payment.Cash)),
                AttributeHelper.GetAttributeValue<PaymentKeyAttribute, string>(typeof(Payment.CashOnDelivery)),
                AttributeHelper.GetAttributeValue<PaymentKeyAttribute, string>(typeof(Payment.PickPoint))
            };
            model.CashOnDelivery = !order.Payed || (order.PaymentMethod != null && paymentsCash.Contains(order.PaymentMethod.PaymentKey));

            GrastinEventWidgetData grastinEventWidget = null;
            var orderPickPoint = OrderService.GetOrderPickPoint(_orderId);
            if (orderPickPoint != null && orderPickPoint.AdditionalData.IsNotEmpty())
            {
                try
                {
                    grastinEventWidget = JsonConvert.DeserializeObject<GrastinEventWidgetData>(orderPickPoint.AdditionalData);
                }
                catch (Exception)
                {
                    // ignored
                }
            }


            if (order.OrderCustomer != null)
            {
                model.Buyer = string.Join(" ",
                    (new[] {order.OrderCustomer.LastName, order.OrderCustomer.FirstName, order.OrderCustomer.Patronymic})
                        .Where(x => !string.IsNullOrEmpty(x)));

                var phone = order.OrderCustomer.StandardPhone.HasValue
                    ? order.OrderCustomer.StandardPhone.Value.ToString()
                    : string.Empty;

                if (phone.StartsWith("7"))
                    phone = "8" + phone.Substring(1);

                if (phone.Length == 10)
                    phone = "8" + phone;

                model.Phone = phone;
                model.Email = order.OrderCustomer.Email;

                model.Address = string.Join(", ",
                    (new[] { order.OrderCustomer.Zip, order.OrderCustomer.Country, order.OrderCustomer.Region, order.OrderCustomer.City, order.OrderCustomer.Street, order.OrderCustomer.House })
                        .Where(x => !string.IsNullOrEmpty(x)));

                if (grastinEventWidget == null || grastinEventWidget.DeliveryType != EnDeliveryType.PickPoint)
                {
                    var comments = new List<string>();

                    if (order.OrderCustomer.Apartment.IsNotEmpty())
                        comments.Add("кв. " + order.OrderCustomer.Apartment);

                    if (order.OrderCustomer.Structure.IsNotEmpty())
                        comments.Add("строение/Корпус " + order.OrderCustomer.Structure);

                    if (order.OrderCustomer.Entrance.IsNotEmpty())
                        comments.Add("подъезд " + order.OrderCustomer.Entrance);

                    if (order.OrderCustomer.Floor.IsNotEmpty())
                        comments.Add("этаж " + order.OrderCustomer.Floor);

                    model.Comment = string.Join(", ", comments);
                }
            }

            model.PointId = grastinEventWidget != null &&
                                 grastinEventWidget.DeliveryType == EnDeliveryType.PickPoint
                ? grastinEventWidget.PickPointId
                : null;

            model.TakeWarehouse = grastinEventWidget != null
                ? grastinEventWidget.CityFrom
                : null;

            model.Services =
                Enum.GetValues(typeof (EnBoxberryService))
                    .Cast<EnBoxberryService>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Localize(),
                        Value = ((int) x).ToString()
                    })
                    .ToList();

            model.Service = grastinEventWidget != null &&
                                 grastinEventWidget.DeliveryType == EnDeliveryType.PickPoint
                ? EnBoxberryService.PickPoint
                : EnBoxberryService.Courier;

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod != null &&
                shippingMethod.ShippingType ==
                ((ShippingKeyAttribute)
                    typeof (Shipping.Grastin.Grastin).GetCustomAttributes(typeof (ShippingKeyAttribute), false).First())
                    .Value)
            {
                var grastinMethod = new Shipping.Grastin.Grastin(shippingMethod, null);
                model.OrderPrefix = grastinMethod.OrderPrefix;

                model.AssessedCost = grastinMethod.Insure ? order.Sum : 10f;

                if (string.IsNullOrEmpty(model.TakeWarehouse))
                    model.TakeWarehouse = grastinMethod.WidgetFromCity;

                var service = new GrastinApiService(grastinMethod.ApiKey);

                var city = grastinEventWidget != null && !string.IsNullOrEmpty(grastinEventWidget.CityTo)
                    ? grastinEventWidget.CityTo
                    : order.OrderCustomer != null ? order.OrderCustomer.City : string.Empty;

                var index = order.OrderCustomer != null ? order.OrderCustomer.Zip : string.Empty;

                var boxberrySelfPickups = service.GetBoxberrySelfPickup(city);
                model.AddressPoints = boxberrySelfPickups != null
                    ? boxberrySelfPickups.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id,
                    }).ToList()
                    : new List<SelectListItem>();

                var takeWarehouses = service.GetWarehouses();
                model.TakeWarehouses = takeWarehouses != null
                    ? takeWarehouses.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Name
                    }).ToList()
                    : new List<SelectListItem>();

                var boxberryPostCodes = service.GetBoxberryPostCode(city);
                model.Postcodes = boxberryPostCodes != null
                    ? boxberryPostCodes.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id,
                        Selected = !string.IsNullOrEmpty(index) && x.Name.Contains(index)
                    }).ToList()
                    : new List<SelectListItem>();
            }

            return model;
        }
    }
}
