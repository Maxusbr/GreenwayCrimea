using System;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Core.SQL;

namespace AdvantShop.Admin.UserControls.Order
{
    public partial class OrdersSearchUc : System.Web.UI.UserControl
    {
        protected SqlPaging _paging;
        protected int StatusId = 0;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lvOrders.DataSource = _paging.PageItems;
            lvOrders.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StatusId = Request["statusid"].TryParseInt();
            if (!IsPostBack)
            {
                LoadOrders(StatusId, Request["page"].TryParseInt(1));
                lblTotalOrdersCount.Text = StatisticService.GetOrdersCount().ToString();
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
            }

        }

        protected void sdsStatuses_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }

        protected void lvOrderStatuses_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowOrdersByStatus")
            {
                StatusId = Convert.ToInt32(e.CommandArgument);
                LoadOrders(StatusId);
            }
        }

        protected void lbtnAllOrders_Click(object sender, EventArgs e)
        {
            StatusId = 0;
            LoadOrders();
        }

        protected void LoadOrders(int orderStatusId = 0, int page = 1)
        {
            lvOrderStatuses.DataBind();

            _paging = new SqlPaging
            {
                TableName = "[Order].[Order] " +
                            "INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderID] " +
                            "INNER JOIN [Order].[OrderCurrency] ON [Order].[OrderID] = [OrderCurrency].[OrderID] " +
                            "INNER JOIN [Order].[OrderStatus] ON [Order].[OrderStatusID] = [OrderStatus].[OrderStatusID]"
            };

            _paging.AddFieldsRange(new Field[]
            {
                new Field { Name = "[Order].[OrderID]" },
                new Field { Name = "[Order].[Number]" },
                new Field { Name = "[Order].[OrderStatusID]" },
                new Field { Name = "LastName + ' ' +  FirstName as CustomerName" },
                new Field { Name = "[Order].[OrderDate]", Sorting = SortDirection.Descending },
                new Field { Name = "[Order].[Sum]" },
                new Field { Name = "[Order].[PaymentDate]" },
                new Field { Name = "[OrderCurrency].[CurrencyCode]" },
                new Field { Name = "[OrderCurrency].[CurrencyValue]" },
                new Field { Name = "[OrderCurrency].[CurrencySymbol]" },
                new Field { Name = "[OrderCurrency].[IsCodeBefore]" },
                new Field { Name = "[OrderStatus].[Color]" }
            });

            if (orderStatusId != 0)
            {
                _paging.Fields["[Order].[OrderStatusID]"].Filter = new EqualFieldFilter { ParamName = "@OrderStatusID", Value = orderStatusId.ToString() };
            }

            _paging.ItemsPerPage = 10;
            _paging.CurrentPageIndex = page;

            pageNumberer.PageCount = _paging.PageCount;
            pageNumberer.CurrentPageIndex = _paging.CurrentPageIndex;
            ViewState["Paging"] = _paging;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected string RenderOrederLink(int orderId, int page)
        {
            return "vieworder.aspx?orderid=" + orderId + (StatusId != 0 ? "&statusid=" + StatusId : string.Empty) + "&page=" + page;
        }
    }
}