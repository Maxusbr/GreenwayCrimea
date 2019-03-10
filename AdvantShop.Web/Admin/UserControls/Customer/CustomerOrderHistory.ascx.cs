//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Web.UI.WebControls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using Resources;

namespace Admin.UserControls
{
    public partial class CustomerOrderHistory : System.Web.UI.UserControl
    {
        private SqlPaging _paging;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = @"[Order].[Order] 
                            INNER JOIN [Order].[OrderStatus] ON [Order].[OrderStatusID] = [OrderStatus].[OrderStatusID] 
                            INNER JOIN [Order].[OrderCurrency] ON [Order].[OrderID] = [OrderCurrency].[OrderID] 
                            INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderID] 
                            LEFT JOIN [Order].[ShippingMethod] ON [Order].[ShippingMethodID] = [ShippingMethod].[ShippingMethodID] 
                            LEFT JOIN [Order].[PaymentMethod] ON [Order].[PaymentMethodID] = [PaymentMethod].[PaymentMethodID]",
                        ItemsPerPage = 10,
                        CurrentPageIndex = 1
                    };

                _paging.AddFieldsRange(new[]
                    {
                        new Field {Name = "[Order].OrderID" },
                        new Field {Name = "[Order].Number" },
                        new Field {Name = "[Order].OrderDiscount"},
                        new Field {Name = "[OrderStatus].StatusName"},
                        new Field {Name = "[OrderStatus].OrderStatusID"}, // , NotInQuery =true 
                        new Field {Name = "[Order].Sum"},
                        new Field {Name = "[Order].OrderDate", Sorting = SortDirection.Descending},
                        new Field {Name = "[Order].ShippingMethod.Name as ShippingMethod"},
                        new Field {Name = "[Order].PaymentMethod.Name as PaymentMethod"},
                        new Field {Name = "[OrderCustomer].FirstName"},
                        new Field {Name = "[OrderCustomer].LastName"},
                        new Field {Name = "[OrderCustomer].CustomerID"},
                        new Field {Name = "[OrderCurrency].CurrencyCode"},
                        new Field {Name = "[OrderCurrency].CurrencyNumCode"},
                        new Field {Name = "[OrderCurrency].CurrencyValue"},
                        new Field {Name = "[OrderCurrency].CurrencySymbol"},
                        new Field {Name = "[OrderCurrency].IsCodeBefore"}
                    });

                _paging.Fields["[OrderCustomer].CustomerID"].Filter = string.IsNullOrEmpty(Request["customerid"]) ? null : new CompareFieldFilter { Expression = Request["customerid"], ParamName = "@CustomerID" };
                //grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");
                pageNumberer.CurrentPageIndex = 1;
                ViewState["CustomerOrderHistoryAdminPaging"] = _paging;

                ddlOrderStatus.Items.Clear();
                ddlOrderStatus.Items.Add(new ListItem(Resource.Admin_Catalog_Any, "-1"));
                foreach (var status in OrderStatusService.GetOrderStatuses())
                {
                    ddlOrderStatus.Items.Add(status.StatusName);
                }
            }
            else
            {
                _paging = (SqlPaging)(ViewState["CustomerOrderHistoryAdminPaging"]);
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }
            if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }
            grid.DataSource = data;
            grid.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            _paging.Fields["[Order].OrderID"].Filter = !string.IsNullOrEmpty(txtOrderId.Text) 
                                                           ? new CompareFieldFilter { Expression = txtOrderId.Text, ParamName = "@OrderID" } 
                                                           : null;

            _paging.Fields["[OrderStatus].StatusName"].Filter = ddlOrderStatus.SelectedValue != "-1"
                                                                    ? new CompareFieldFilter { Expression = ddlOrderStatus.SelectedValue, ParamName = "@StatusName" }
                                                                    : null;

            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtOrderId.Text = String.Empty;
            ddlOrderStatus.SelectedValue = "-1";

            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void linkGO_Click(object sender, EventArgs e)
        {
            int pagen;
            try
            {
                pagen = int.Parse(txtPageNum.Text);
            }
            catch (Exception)
            {
                pagen = -1;
            }
            if (pagen >= 1 && pagen <= _paging.PageCount)
            {
                pageNumberer.CurrentPageIndex = pagen;
                _paging.CurrentPageIndex = pagen;
            }
        }
    }
}