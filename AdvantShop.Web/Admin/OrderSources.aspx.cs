//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Resources;

using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Helpers;
using AdvantShop.Core.SQL;
using AdvantShop.Orders;

namespace Admin
{
    public partial class OrderSources : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                Message.Visible = false;
                Message.Text = "";
            }
            else
            {
                Message.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            Message.Visible = true;
            Message.Text = "<br/>" + messageText;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(true);
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Leads_Header));

            if (!IsPostBack)
            {
                foreach (OrderType type in Enum.GetValues(typeof(OrderType)))
                {
                    ddlFilterOrderSourceType.Items.Add(new ListItem(type.Localize(), type.ToString()));
                }

                _paging = new SqlPaging
                {
                    TableName = "[Order].[OrderSource] ",
                    ItemsPerPage = 100
                };

                _paging.AddFieldsRange(
                    new Field("OrderSource.Id as ID") { IsDistinct = true },
                    new Field("OrderSource.Name"),
                    new Field("OrderSource.[Main]"),
                    new Field("OrderSource.[Type]"),
                    new Field("OrderSource.SortOrder") { Sorting = SortDirection.Ascending },
                    new Field("(Select Count(OrderId) From [Order].[Order] Where OrderSourceId = OrderSource.Id) as OrdersCount"),
                    new Field("(Select Count([Lead].Id) From [Order].[Lead] Where OrderSourceId = OrderSource.Id) as LeadsCount"));

                //pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowdown.gif");
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                //_paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }

                string strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    var arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length];

                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        var t = arrids[idx];
                        if (t.Replace(" ", "") != "-1")
                        {
                            ids[idx] = t;
                        }
                        else
                        {
                            _selectionFilter.IncludeValues = false;
                            _inverseSelection = true;
                        }
                    }

                    _selectionFilter.Values = ids;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                var id = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]);
                var orderSource = OrderSourceService.GetOrderSource(id);

                var sortOrder = 0;
                if (int.TryParse(grid.UpdatedRow["SortOrder"], out sortOrder))
                {
                    orderSource.Name = grid.UpdatedRow["Name"];
                    orderSource.SortOrder = sortOrder;
                    orderSource.Main = Convert.ToBoolean(grid.UpdatedRow["Main"]);
                    orderSource.Type = grid.UpdatedRow["Type"].TryParseEnum<OrderType>();
                    OrderSourceService.UpdateOrderSource(orderSource);
                }
            }

            DataTable data = _paging.PageItems;

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
            data.Columns.Add(clmn);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    int intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["ID"]).ToString()))
                    {
                        data.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }

            grid.DataSource = data;
            grid.DataBind();

            lblFound.Text = _paging.TotalRowsCount.ToString();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var flogical = new LogicalFilter();
            //-----Selection filter
            if (ddSelect.SelectedIndex != 0)
            {
                if (ddSelect.SelectedIndex == 2)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                    else
                    {
                        _selectionFilter = null;
                    }
                }
                flogical.AddFilter(_selectionFilter);
            }

            _paging.Fields["ID"].Filter = flogical.FilterCount() > 0 ? flogical : null;

            _paging.Fields["OrderSource.Name"].Filter =
                !string.IsNullOrWhiteSpace(txtFilterOrderSourceName.Text)
                    ? new CompareFieldFilter() { ParamName = "@Name", Expression = txtFilterOrderSourceName.Text }
                    : null;

            _paging.Fields["OrderSource.[Main]"].Filter =
                (ddlFilterOrderSourceMain.SelectedValue != "any")
                    ? new EqualFieldFilter { ParamName = "@Main", Value = (ddlFilterOrderSourceMain.SelectedValue == "yes" ? "1" : "0") }
                    : null;

            _paging.Fields["OrderSource.[Type]"].Filter =
                ddlFilterOrderSourceType.SelectedValue != "None"
                    ? new EqualFieldFilter { ParamName = "@Type", Value = ddlFilterOrderSourceType.SelectedValue }
                    : null;


            _paging.Fields["OrderSource.SortOrder"].Filter =
                !string.IsNullOrWhiteSpace(txtFilterOrderSourceSortOrder.Text)
                    ? new CompareFieldFilter() { ParamName = "@SortOrder", Expression = txtFilterOrderSourceSortOrder.Text }
                    : null;

            _paging.Fields["OrdersCount"].Filter =
                !string.IsNullOrWhiteSpace(txtFilterOrdersCount.Text)
                    ? new CompareFieldFilter() { ParamName = "@OrdersCount", Expression = txtFilterOrdersCount.Text }
                    : null;

            _paging.Fields["LeadsCount"].Filter =
                !string.IsNullOrWhiteSpace(txtFilterLeadsCount.Text)
                    ? new CompareFieldFilter() { ParamName = "@OrdersCount", Expression = txtFilterLeadsCount.Text }
                    : null;

            _paging.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteOrderSource")
            {
                OrderSourceService.DeleteOrderSource(int.Parse(e.CommandArgument.ToString()));
            }
            if (e.CommandName == "AddOrderSource")
            {
                GridViewRow footer = grid.FooterRow;
                int sortOrder;

                if (!int.TryParse(((TextBox)footer.FindControl("txtOrderSourceNewSortOrder")).Text, out sortOrder)
                    || string.IsNullOrEmpty(((TextBox)footer.FindControl("txtOrderSourceNewName")).Text))
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                OrderSourceService.AddOrderSource(new OrderSource
                {
                    Name = ((TextBox)footer.FindControl("txtOrderSourceNewName")).Text,
                    SortOrder = sortOrder,
                    Main = ((CheckBox)footer.FindControl("ckbOrderSourceNewMain")).Checked,
                    Type = ((DropDownList)footer.FindControl("ddlOrderSourceNewType")).SelectedValue.TryParseEnum<OrderType>()
                });

                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    //{ "ID", "arrowId" },
                    { "OrderSource.Name", "arrowName" },
                    { "OrderSource.[Main]","arrowMain" },
                    { "OrderSource.[Type]", "arrowType" },
                    { "OrderSource.SortOrder", "arrowSortOrder" },
                    { "OrdersCount", "arrowOrdersCount" },
                    { "LeadsCount", "arrowLeadsCount" }
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = null;

            if (_paging.Fields.ContainsKey(e.SortExpression))
                nsf = _paging.Fields[e.SortExpression];
            else if (_paging.Fields.ContainsKey("OrderSource." + e.SortExpression))
                nsf = _paging.Fields["OrderSource." + e.SortExpression];

            if (nsf != null)
            {
                if (nsf.Name.Equals(csf.Name))
                {
                    csf.Sorting = csf.Sorting == SortDirection.Ascending
                        ? SortDirection.Descending
                        : SortDirection.Ascending;
                    grid.ChangeHeaderImageUrl(arrows[csf.Name],
                        (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
                }
                else
                {
                    csf.Sorting = null;

                    grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                    nsf.Sorting = SortDirection.Ascending;
                    grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
                }
            }
            _paging.CurrentPageIndex = 1;
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        LeadService.DeleteLead(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("[OrderSource].Id as ID");
                    foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
                    {
                        LeadService.DeleteLead(id);
                    }
                }
            }
        }

        protected string SelectedCount()
        {
            if (_inverseSelection)
                return lblFound.Text;
            if (_selectionFilter != null && _selectionFilter.Values != null)
                return _selectionFilter.Values.Length.ToString();
            return "0";
        }

        protected void grid_OnDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((LinkButton)e.Row.FindControl("buttonDelete")).Attributes["data-confirm"] =
                    string.Format(Resource.Admin_Leads_DeleteConfirmation, ((DataRowView)e.Row.DataItem)["Id"]);

                var ddlTypeControl = (DropDownList)e.Row.FindControl("ddlOrderSourceType");
                foreach (OrderType type in Enum.GetValues(typeof(OrderType)))
                {
                    ddlTypeControl.Items.Add(new ListItem(type.Localize(), type.ToString()));
                }
                if (((DataRowView)e.Row.DataItem)["Type"] != null && ddlTypeControl.Items.FindByValue(((DataRowView)e.Row.DataItem)["Type"].ToString()) != null)
                {
                    ddlTypeControl.SelectedValue = ((DataRowView)e.Row.DataItem)["Type"].ToString();
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var ddlNewTypeControl = (DropDownList)e.Row.FindControl("ddlOrderSourceNewType");
                foreach (OrderType type in Enum.GetValues(typeof(OrderType)))
                {
                    ddlNewTypeControl.Items.Add(new ListItem(type.Localize(), type.ToString()));
                }
            }
        }

        private void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtOrderSourceNewName").Focus();
            }
        }

        protected void btnAddOrderSource_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }
    }
}