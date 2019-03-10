using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Grastin;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class GrastinSendOrderForBoxberry
    {
        private readonly SendOrderForBoxberryModel _model;

        public List<string> Errors { get; set; }

        public GrastinSendOrderForBoxberry(SendOrderForBoxberryModel model)
        {
            _model = model;
        }

        public bool Execute()
        {
            var order = OrderService.GetOrder(_model.OrderId);
            if (order != null)
            {
                var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
                if (shippingMethod != null &&
                    shippingMethod.ShippingType ==
                    ((ShippingKeyAttribute)
                        typeof (Shipping.Grastin.Grastin).GetCustomAttributes(typeof (ShippingKeyAttribute), false)
                            .First())
                        .Value)
                {
                    var grastinMethod = new Shipping.Grastin.Grastin(shippingMethod, null);

                    var service = new GrastinApiService(grastinMethod.ApiKey);

                    var boxberryOrder = new BoxberryOrder()
                    {
                        Number = string.Format("{0}{1}", grastinMethod.OrderPrefix, order.Number),
                        Comment = _model.Comment.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                        Buyer = _model.Buyer.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                        Weight = _model.Weight,
                        Phone = _model.Phone.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                        Phone2 = _model.Phone2.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                        Seats = _model.Seats,
                        //IsTest = false,
                        TakeWarehouse = _model.TakeWarehouse,
                        SiteName = SettingsMain.ShopName.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                        Email = _model.Email,
                        CargoType = _model.CargoType.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                        BarCode = _model.BarCode.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                        PickupId = _model.Service == EnBoxberryService.PickPoint ? _model.PointId : null,
                        PostcodeId = _model.Service == EnBoxberryService.Courier ? _model.PostcodeId : null,
                        Address = _model.Service == EnBoxberryService.Courier ? _model.Address.RemoveInvalidXmlChars().RemoveEscapeXmlChars() : null,
                        CostDelivery = 0f,//order.ShippingCost,
                    };

                    if (order.OrderItems != null && order.OrderItems.Count > 0)
                    {
                        var orderItems = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems,
                            order.ShippingCost, order.Sum);
                        boxberryOrder.Products = orderItems.Select(x => new GrastinProduct()
                        {
                            ArtNo = x.ArtNo.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                            Name = x.Name.RemoveInvalidXmlChars().RemoveEscapeXmlChars(),
                            Price = x.Price,
                            Amount = x.Amount
                        }).ToList();

                        if (order.ShippingCost > 0)
                        {
                            boxberryOrder.Products.Add(new GrastinProduct()
                            {
                                ArtNo = "Доставка",
                                Name = "Доставка",
                                Price = order.ShippingCost,
                                Amount = 1
                            });
                        }

                        boxberryOrder.OrderSum = boxberryOrder.Products.Sum(x => x.Price*x.Amount) + boxberryOrder.CostDelivery;
                    }
                    else
                    {
                        boxberryOrder.OrderSum = order.Sum;
                    }

                    boxberryOrder.AssessedCost = _model.CashOnDelivery ? boxberryOrder.OrderSum : _model.AssessedCost;

                    if (!_model.CashOnDelivery)
                    {
                        boxberryOrder.OrderSum = 0f;
                        boxberryOrder.CostDelivery = 0f;
                    }

                    var response = service.AddBoxberryOrder(new BoxberryOrderContainer() { Orders = new List<BoxberryOrder>() { boxberryOrder } });

                    if (response != null && response.Count == 1)
                    {
                        if (string.IsNullOrEmpty(response[0].Error))
                        {
                            OrderService.AddUpdateOrderAdditionalData(order.OrderID, "GrastinSendOrder", true.ToString());

                            return true;
                        }

                        Errors = new List<string>() { response[0].Error };
                    }
                    else
                    {
                        Errors = service.LastActionErrors;
                    }
                }
            }

            return false;
        }
    }
}
