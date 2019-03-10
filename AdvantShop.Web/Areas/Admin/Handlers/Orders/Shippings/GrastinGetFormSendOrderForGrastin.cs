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
    public class GrastinGetFormSendOrderForGrastin
    {
        private readonly int _orderId;

        public GrastinGetFormSendOrderForGrastin(int orderId)
        {
            _orderId = orderId;
        }

        public SendOrderForGrastinModel Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return null;

            var model = new SendOrderForGrastinModel()
            {
                OrderId = _orderId,
                OrderNumber = order.Number,
                Seats = 1,
                AssessedCost = order.Sum,
                DeliveryDate = DateTime.Now,
            };

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

                model.Index = order.OrderCustomer.Zip;

                model.AddressCourier = string.Join(", ",
                    (new[] {order.OrderCustomer.Country, order.OrderCustomer.Region, order.OrderCustomer.City, order.OrderCustomer.Street, order.OrderCustomer.House })
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

            if (order.PaymentDetails != null)
            {
                model.Organization = order.PaymentDetails.CompanyName;
                model.Inn = order.PaymentDetails.INN;
            }

            var pointId = grastinEventWidget != null &&
                          grastinEventWidget.DeliveryType == EnDeliveryType.PickPoint
                ? grastinEventWidget.PickPointId
                : null;

            model.TakeWarehouse = grastinEventWidget != null
                ? grastinEventWidget.CityFrom
                : null;

            model.TypeRecipients =
                Enum.GetValues(typeof (EnTypeRecipient))
                    .Cast<EnTypeRecipient>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Localize(),
                        Value = ((int) x).ToString()
                    })
                    .ToList();

            model.Services =
                Enum.GetValues(typeof (EnCourierService))
                    .Cast<EnCourierService>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Localize(),
                        Value = ((int) x).ToString()
                    })
                    .ToList();

            var paymentsCash = new[]
            {
                AttributeHelper.GetAttributeValue<PaymentKeyAttribute, string>(typeof (Payment.Cash)),
                AttributeHelper.GetAttributeValue<PaymentKeyAttribute, string>(typeof (Payment.CashOnDelivery)),
                AttributeHelper.GetAttributeValue<PaymentKeyAttribute, string>(typeof (Payment.PickPoint))
            };

            model.CashOnDelivery = !order.Payed || (order.PaymentMethod != null && paymentsCash.Contains(order.PaymentMethod.PaymentKey));

            model.Times = new List<SelectListItem>();
            for (int hour = 10; hour <= 23; hour++)
            {
                model.Times.Add(new SelectListItem()
                {
                    Text = string.Format("{0}:00", hour),
                    Value = string.Format("{0}:00", hour),
                });
            }
            model.DeiveryTimeFrom = "10:00";
            model.DeiveryTimeTo = "18:00";

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod != null &&
                shippingMethod.ShippingType ==
                ((ShippingKeyAttribute)
                    typeof (Shipping.Grastin.Grastin).GetCustomAttributes(typeof (ShippingKeyAttribute), false).First())
                    .Value)
            {
                var grastinMethod = new Shipping.Grastin.Grastin(shippingMethod, null);
                model.OrderPrefix = grastinMethod.OrderPrefix;

                model.AssessedCost = grastinMethod.Insure ? order.Sum : 1f;

                if (string.IsNullOrEmpty(model.TakeWarehouse))
                    model.TakeWarehouse = grastinMethod.WidgetFromCity;

                model.Service = grastinEventWidget == null || grastinEventWidget.DeliveryType != EnDeliveryType.PickPoint
                    ? !order.Payed || (order.PaymentMethod != null && paymentsCash.Contains(order.PaymentMethod.PaymentKey)) ? grastinMethod.TypePaymentDelivery : EnCourierService.DeliverWithoutPayment
                    : !order.Payed || (order.PaymentMethod != null && paymentsCash.Contains(order.PaymentMethod.PaymentKey)) ? grastinMethod.TypePaymentPickup : EnCourierService.PickupWithoutPaying;

                var service = new GrastinApiService(grastinMethod.ApiKey);

                var grastinSelfPickups = service.GetGrastinSelfPickups();
                model.AddressPoints = grastinSelfPickups != null
                    ? grastinSelfPickups.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Name,
                        Selected = x.Id == pointId
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

                var transportCompanyOffices = service.GetTransportCompanyOffices();
                model.Offices = transportCompanyOffices != null
                    ? transportCompanyOffices.Select(x => new SelectListItem()
                    {
                        Text = x.Title,
                        Value = x.Id
                    }).ToList()
                    : new List<SelectListItem>();
            }

            return model;
        }
    }
}
