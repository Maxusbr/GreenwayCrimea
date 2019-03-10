using System;
using System.Linq;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models.OrdersEdit;
using AdvantShop.Core.Modules;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class UpdateOrder
    {
        private readonly OrderModel _model;
        private readonly Order _order;

        public UpdateOrder()
        {
        }

        public UpdateOrder(OrderModel model)
        {
            _model = model;
            _order = _model.Order;
        }

        public bool Execute()
        {
            var order = OrderService.GetOrder(_order.OrderID);
            if (order == null || !OrderService.CheckAccess(order))
                return false;

            try
            {
                order.StatusComment = _order.StatusComment;
                order.AdminOrderComment = _order.AdminOrderComment;
                order.ManagerConfirmed = _order.ManagerConfirmed;
                order.OrderSourceId = _order.OrderSourceId;
                order.ManagerId = _order.ManagerId;
                order.TrackNumber = _order.TrackNumber;
                order.OrderDate = _order.OrderDate;

                if (order.OrderDate == DateTime.MinValue)
                {
                    var d = DateTime.Now;
                    order.OrderDate = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, d.Kind);
                }

                if (Settings1C.Enabled)
                    order.UseIn1C = _order.UseIn1C;

                if (order.OrderCustomer == null)
                {
                    order.OrderCustomer = new OrderCustomer();
                }

                order.OrderCustomer.OrderID = _order.OrderID;
                order.OrderCustomer.CustomerID = _order.OrderCustomer.CustomerID != Guid.Empty ? _order.OrderCustomer.CustomerID : Guid.NewGuid();
                order.OrderCustomer.FirstName = _order.OrderCustomer.FirstName;
                order.OrderCustomer.LastName = _order.OrderCustomer.LastName;
                order.OrderCustomer.Patronymic = _order.OrderCustomer.Patronymic;
                order.OrderCustomer.Email = _order.OrderCustomer.Email;
                order.OrderCustomer.Phone = _order.OrderCustomer.Phone;
                order.OrderCustomer.StandardPhone =
                    !string.IsNullOrEmpty(order.OrderCustomer.Phone)
                        ? StringHelper.ConvertToStandardPhone(order.OrderCustomer.Phone)
                        : null;

                order.OrderCustomer.Country = _order.OrderCustomer.Country;
                order.OrderCustomer.Region = _order.OrderCustomer.Region;
                order.OrderCustomer.City = _order.OrderCustomer.City;
                order.OrderCustomer.Zip = _order.OrderCustomer.Zip;
                //order.OrderCustomer.Address = _order.OrderCustomer.Address;
                order.OrderCustomer.CustomField1 = _order.OrderCustomer.CustomField1;
                order.OrderCustomer.CustomField2 = _order.OrderCustomer.CustomField2;
                order.OrderCustomer.CustomField3 = _order.OrderCustomer.CustomField3;

                order.OrderCustomer.Street = _order.OrderCustomer.Street;
                order.OrderCustomer.House = _order.OrderCustomer.House;
                order.OrderCustomer.Apartment = _order.OrderCustomer.Apartment;
                order.OrderCustomer.Structure = _order.OrderCustomer.Structure;
                order.OrderCustomer.Entrance = _order.OrderCustomer.Entrance;
                order.OrderCustomer.Floor = _order.OrderCustomer.Floor;

                var isDraftChanged = order.IsDraft != _order.IsDraft;

                order.IsDraft = _order.IsDraft;

                var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);
                var isDraft = _order.IsDraft;

                OrderService.UpdateOrderMain(order, !isDraft, changedBy: changedBy, trackChanges: !isDraft);

                if (OrderService.GetOrderCustomer(order.OrderID) == null)
                {
                    OrderService.AddOrderCustomer(order.OrderID, order.OrderCustomer);
                }
                else
                {
                    OrderService.UpdateOrderCustomer(order.OrderCustomer, changedBy, trackChanges: !isDraft);
                }

                OrderService.PayOrder(_order.OrderID, _model.IsPayed, trackChanges: !_model.Order.IsDraft);

                if (!_order.IsDraft && isDraftChanged)
                {
                    OrderService.CreateCustomerByOrderCustomer(order.OrderCustomer);

                    ModulesExecuter.SendNotificationsOnOrderAdded(order);
                    OrderService.SendOrderMail(order, order.TotalDiscount, 0, order.ArchivedShippingName, order.PaymentMethodName);

                    BizProcessExecuter.OrderAdded(order);
                    TrialService.TrackEvent(ETrackEvent.Trial_AddOrder);
                }

                if (_model.CustomerFields != null)
                {
                    var customer = CustomerService.GetCustomer(_order.OrderCustomer.CustomerID);
                    if (customer != null)
                    {
                        foreach (var customerField in _model.CustomerFields)
                        {
                            CustomerFieldService.AddUpdateMap(customer.Id, customerField.Id, customerField.Value ?? "");
                        }
                    }
                }

                new UpdateOrderTotal(order).Execute();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }

            return true;
        }
    }
}
