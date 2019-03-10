using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Module.Roistat.Domain;
using AdvantShop.Module.Roistat.Models.Roistat;
using AdvantShop.Orders;

namespace AdvantShop.Module.Roistat.Handlers
{
    public class GetOrders
    {
        #region Ctor

        private const int CountPerPage = 1000;
        private readonly RoistatOrdersExportModel _model;

        public GetOrders(RoistatOrdersExportModel model)
        {
            _model = model;
        }
        
        #endregion

        public RoistatOrdersResponse Execute()
        {
            var modifiedDate = DateTimeConverter.UnixTimeStampToDateTime(_model.Date);
            var countPerPage = _model.Limit > 0 ? _model.Limit : CountPerPage;

            var ordersRoistat =
                GetOrdersList(modifiedDate, countPerPage).Select(x => new RoistatOrder()
                {
                    Id = "order_" + x.OrderID.ToString(),
                    DateCreate = x.OrderDate.ToUnixTime().ToString(),
                    Status = string.Format("{0}_{1}_{2}", "order", x.OrderStatusId, x.Payed),
                    Price = x.Sum.ToString("F2", CultureInfo.InvariantCulture),
                    Cost = x.SupplyTotal.ToString("F2", CultureInfo.InvariantCulture),
                    Roistat = RoistatService.GetRoistatOrderCookie(x.OrderID, RoistatEntityType.Order),
                    ClientId = x.OrderCustomer != null ? x.OrderCustomer.CustomerID.ToString() : null,
                    Fields = GetFields(x)
                });
            
            var leadsRoistat =
                GetLeadsList(modifiedDate, countPerPage).Select(x => new RoistatOrder()
                {
                    Id = "lead_" + x.Id.ToString(),
                    DateCreate = x.CreatedDate.ToUnixTime().ToString(),
                    Status = string.Format("{0}_{1}_{2}", "lead", x.DealStatusId, false),
                    Price = x.Sum.ToString("F2", CultureInfo.InvariantCulture),
                    Cost = 0.ToString("F2", CultureInfo.InvariantCulture),
                    Roistat = RoistatService.GetRoistatOrderCookie(x.Id, RoistatEntityType.Lead),
                    ClientId = x.Customer != null ? x.Customer.Id.ToString() : null,
                    Fields = GetFields(x)
                });

            var orderStatuses = OrderStatusService.GetOrderStatuses();
            var leadDealStatuses = DealStatusService.GetList();

            var statuses =
                orderStatuses.Select(x => new RoistatStatus("order", x.StatusID, x.StatusName, false))
                    .Concat(orderStatuses.Select(x => new RoistatStatus("order", x.StatusID, x.StatusName, true)))
                    .Concat(leadDealStatuses.Select(x => new RoistatStatus("lead", x.Id, x.Name, null)));

            var response = new RoistatOrdersResponse()
            {
                Pagination = new RoistatPagination()
                {
                    Limit = countPerPage,
                    TotalCount = GetOrdersCount(modifiedDate) + GetLeadsCount(modifiedDate)
                },
                Orders = ordersRoistat.Concat(leadsRoistat).ToList(),
                Statuses = statuses.ToList(),
                Fields = RoistatOrderField.GetDefaultFields()
            };

            return response;
        }


        private Dictionary<string, string> GetFields(Order order)
        {
            var dic = new Dictionary<string, string>();

            if (order.Manager != null)
                dic.Add("Менеджер", order.Manager.FullName);

            if (!string.IsNullOrEmpty(order.ArchivedShippingName))
                dic.Add("Способ доставки", order.ArchivedShippingName);

            if (!string.IsNullOrEmpty(order.PaymentMethodName))
                dic.Add("Способ оплаты", order.PaymentMethodName);

            if (order.OrderCustomer != null && !string.IsNullOrEmpty(order.OrderCustomer.Street))
                dic.Add("Адрес доставки", order.OrderCustomer.Street);
            
            return dic;
        }

        private Dictionary<string, string> GetFields(Lead lead)
        {
            var dic = new Dictionary<string, string>();

            if (lead.ManagerId != null)
            {
                var manager = ManagerService.GetManager(lead.ManagerId.Value);
                if (manager != null)
                    dic.Add("Менеджер", manager.FullName);
            }

            return dic;
        }

        private int GetOrdersCount(DateTime modifiedDate)
        {
            return
                Convert.ToInt32(
                    SQLDataAccess.ExecuteScalar(
                        "SELECT Count(OrderId) FROM [Order].[Order] Where IsDraft <> 1 and ModifiedDate > @ModifiedDate",
                        CommandType.Text,
                        new SqlParameter("@ModifiedDate", modifiedDate)));
        }

        private List<Order> GetOrdersList(DateTime modifiedDate, int countPerPage)
        {
            var orders =
                SQLDataAccess.ExecuteReadIEnumerable(
                    "SELECT * FROM [Order].[Order] Where IsDraft <> 1 and ModifiedDate > @modifiedDate",
                    CommandType.Text,
                    OrderService.GetOrderFromReader,
                    new SqlParameter("@modifiedDate", modifiedDate))

                    .Skip(_model.Offset)
                    .Take(countPerPage)
                    .ToList();

            return orders;
        }

        private int GetLeadsCount(DateTime modifiedDate)
        {
            return
                Convert.ToInt32(
                    SQLDataAccess.ExecuteScalar(
                        "SELECT Count(Id) FROM [Order].[Lead] Where ModifiedDate > @ModifiedDate",
                        CommandType.Text,
                        new SqlParameter("@ModifiedDate", modifiedDate)));
        }

        private List<Lead> GetLeadsList(DateTime modifiedDate, int countPerPage)
        {
            var orders =
                SQLDataAccess.Query<Lead>(
                    "SELECT * FROM [Order].[Lead] Where ModifiedDate > @modifiedDate",
                    new {modifiedDate})
                    .Skip(_model.Offset)
                    .Take(countPerPage)
                    .ToList();

            return orders;
        }

    }
}
