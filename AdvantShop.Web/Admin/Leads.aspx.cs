//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using Resources;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;

namespace Admin
{
    public partial class Leads : AdvantShopAdminPage
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


        protected void FillOrderSourceFilter()
        {
            ddlOrderSource.Items.Clear();

            ddlOrderSource.Items.Add(new ListItem(Resource.Admin_Leads_NotChoosen, ""));
            ddlOrderSource.Items.AddRange(OrderSourceService.GetOrderSources().Select(x => new ListItem(x.Name, x.Id.ToString())).ToArray());
        }

        private void FillLeadStatus()
        {
            ddlLeadStatus.Items.Clear();
            ddlLeadStatus.Items.Add(new ListItem(Resource.Admin_Leads_NotChoosen, ""));
            foreach (LeadStatus type in Enum.GetValues(typeof(LeadStatus)))
            {
                ddlLeadStatus.Items.Add(new ListItem(type.Localize(), type.ToString()));
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            MsgErr(true);
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Leads_Header));

            if (!IsPostBack)
            {
                FillOrderSourceFilter();
                FillLeadStatus();

                _paging = new SqlPaging
                {
                    TableName =
                        "[Order].[Lead] " +
                        "LEFT JOIN [Order].[LeadItem] ON [LeadItem].[LeadId]=[Lead].[Id] " +
                        "LEFT JOIN [Order].[LeadCurrency] ON [LeadCurrency].[LeadId]=[Lead].[Id]",
                    ItemsPerPage = 10
                };

                _paging.AddFieldsRange(
                    new Field("Lead.Id as ID") { IsDistinct = true },
                    new Field("(Lead.FirstName + ' ' + Lead.LastName) as Name"),
                    new Field("Lead.Phone"),
                    new Field("Lead.Email"),
                    new Field("Lead.OrderSourceId"),
                    new Field("LeadStatus"),
                    new Field("(Select Sum(Price*Amount) From [Order].[LeadItem] Where [LeadItem].[LeadId]=[Id]) as Sum"),
                    new Field("Lead.CreatedDate") { Sorting = SortDirection.Descending },
                    new Field("CurrencyValue"),
                    new Field("CurrencyCode"),
                    new Field("CurrencySymbol"),
                    new Field("IsCodeBefore")
                    );

                if (SettingsCheckout.EnableManagersModule)
                {
                    _paging.TableName +=
                        " LEFT JOIN [Customers].[Managers] ON [Lead].[ManagerId]=[Managers].[ManagerID] " +
                        " LEFT JOIN [Customers].[Customer] as ManagerCustomer ON [Managers].[CustomerId]=[ManagerCustomer].[CustomerId] ";

                    _paging.AddField(new Field("ManagerCustomer.CustomerId as ManagerCustomerId"));
                    _paging.AddField(new Field("[ManagerCustomer].FirstName + ' ' + [ManagerCustomer].LastName as ManagerName"));

                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        if (grid.Columns[i].AccessibleHeaderText == "ManagerName")
                        {
                            grid.Columns[i].Visible = true;
                            break;
                        }
                    }
                    tdManager.Visible = true;

                    ddlManagers.Items.Clear();
                    ddlManagers.Items.Add(new ListItem(Resource.Admin_Catalog_Any, ""));
                    ddlManagers.Items.Add(new ListItem(Resource.Admin_Catalog_No, "null"));
                    foreach (var manager in ManagerService.GetCustomerManagersList())
                        ddlManagers.Items.Add(new ListItem(string.Format("{0} {1}", manager.FirstName, manager.LastName), manager.Id.ToString()));
                }
                else
                {
                    _paging.AddField(new Field("null as ManagerCustomerId"));
                    _paging.AddField(new Field("'' as ManagerName"));
                }

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                grid.ChangeHeaderImageUrl("arrowLeadId", "images/arrowdown.gif");

                if (Request["filter"] != null)
                {
                    if (Request["filter"] == "lastmonth")
                    {
                        txtDateFrom.Text = DateTime.Now.Date.AddDays(-1).ToString("dd.MM.yyyy");
                        txtDateTo.Text = DateTime.Now.Date.AddMonths(1).ToString("dd.MM.yyyy");
                    }
                    if (Request["filter"] == "today")
                    {
                        txtDateFrom.Text = DateTime.Now.Date.ToString("dd.MM.yyyy");
                        txtDateTo.Text = DateTime.Now.Date.ToString("dd.MM.yyyy");

                    }
                    if (Request["filter"] == "yesterday")
                    {
                        txtDateFrom.Text = DateTime.Now.Date.AddDays(-1).ToString("dd.MM.yyyy");
                        txtDateTo.Text = DateTime.Now.Date.AddDays(-1).ToString("dd.MM.yyyy");
                    }
                    btnFilter_Click(sender, e);
                }


                if (Request["search"].IsNotEmpty())
                {
                    txtName.Text = Request["search"];
                    btnFilter_Click(null, null);
                }

                //ddlStatus.DataBind();
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

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

            //if (!IsPostBack && !string.IsNullOrEmpty(Request["status"]) && ddlOrderSource.Items.FindByValue(Request["status"].ToLower()) != null)
            //{
            //    ddlOrderSource.SelectedValue = Request["status"].ToLower();
            //    btnFilter_Click(new object(), new EventArgs());
            //}

            if (Request["manager"].IsNotEmpty() && ddlManagers.Items.FindByValue(Request["manager"]) != null)
            {
                ddlManagers.SelectedValue = Request["manager"];

                _paging.Fields["ManagerCustomerId"].Filter = new EqualFieldFilter
                {
                    ParamName = "@ManagerId",
                    Value = ddlManagers.SelectedValue
                };
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataTable data = _paging.PageItems;
            //while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            //{
            //    _paging.CurrentPageIndex--;
            //    data = _paging.PageItems;
            //}

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

            if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }

            grid.DataSource = data;
            grid.DataBind();

            pageNumberer.PageCount = _paging.PageCount;
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

            //----OrderID filter
            if (!string.IsNullOrEmpty(txtLeadId.Text))
            {
                flogical.AddLogicalOperation("AND");
                flogical.AddFilter(new EqualFieldFilter { Value = txtLeadId.Text.TryParseInt().ToString(), ParamName = "@Id" });
            }


            _paging.Fields["ID"].Filter = flogical.FilterCount() > 0 ? flogical : null;

            //----Status filter
            if (!string.IsNullOrEmpty(ddlOrderSource.SelectedValue))
            {
                if (ddlOrderSource.SelectedIndex != 0)
                {
                    _paging.Fields["Lead.OrderSourceId"].Filter = new EqualFieldFilter()
                    {
                        ParamName = "@OrderSourceId",
                        Value = ddlOrderSource.SelectedValue
                    };
                }
                else
                {
                    _paging.Fields["Lead.OrderSourceId"].Filter = null;
                }
            }
            else
            {
                _paging.Fields["Lead.OrderSourceId"].Filter = null;
            }

            _paging.Fields["LeadStatus"].Filter = !string.IsNullOrEmpty(ddlLeadStatus.SelectedValue)
                ? new EqualFieldFilter() { ParamName = "@LeadStatus", Value = ddlLeadStatus.SelectedValue }
                : null;

            //----Sum filter
            var sumFilter = new RangeFieldFilter { ParamName = "@Sum" };

            try
            {
                int priceFrom = 0;
                sumFilter.From = int.TryParse(txtSumFrom.Text, out priceFrom) ? priceFrom : (int?)null;
            }
            catch (Exception)
            {
            }

            try
            {
                int priceTo = 0;
                sumFilter.To = int.TryParse(txtSumTo.Text, out priceTo) ? priceTo : (int?)null;
            }
            catch (Exception)
            {
            }

            _paging.Fields["Sum"].Filter = sumFilter.From.HasValue || sumFilter.To.HasValue ? sumFilter : null;

            _paging.Fields["Lead.Name"].Filter =
                !string.IsNullOrWhiteSpace(txtName.Text)
                    ? new CompareFieldFilter() { ParamName = "@Name", Expression = txtName.Text }
                    : null;

            _paging.Fields["Lead.Phone"].Filter =
                !string.IsNullOrWhiteSpace(txtPhone.Text)
                    ? new CompareFieldFilter() { ParamName = "@Phone", Expression = txtPhone.Text }
                    : null;

            _paging.Fields["Lead.Email"].Filter =
                !string.IsNullOrWhiteSpace(txtEmail.Text)
                    ? new CompareFieldFilter() { ParamName = "@Email", Expression = txtEmail.Text }
                    : null;


            //---Manager filter
            if (!string.IsNullOrEmpty(ddlManagers.SelectedValue))
            {
                if (ddlManagers.SelectedValue == "null")
                    _paging.Fields["ManagerCustomerId"].Filter = new NullFieldFilter
                    {
                        ParamName = "@ManagerId",
                        Null = true
                    };
                else
                    _paging.Fields["ManagerCustomerId"].Filter = new EqualFieldFilter
                    {
                        ParamName = "@ManagerId",
                        Value = ddlManagers.SelectedValue
                    };
            }
            else
                _paging.Fields["ManagerCustomerId"].Filter = null;


            //---OrderDate filter
            var dfilter = new DateTimeRangeFieldFilter { ParamName = "@dateOrdSearch" };
            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtDateFrom.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                    dfilter.From = dt;
                }
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtDateTo.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                    dfilter.To = dt;
                }
            }

            if (dfilter.From.HasValue || dfilter.To.HasValue)
            {
                _paging.Fields["Lead.CreatedDate"].Filter = dfilter;
            }
            else
            {
                _paging.Fields["Lead.CreatedDate"].Filter = null;
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
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

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
                LeadService.DeleteLead(int.Parse(e.CommandArgument.ToString()));
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"ID", "arrowLeadId"},
                    {"Sum", "arrowSum"},
                    {"LeadStatus", "arrowLeadStatus"},
                    {"Lead.OrderType", "arrowOrderType"},
                    {"Lead.CreatedDate", "arrowCreatedDate"},
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = null;

            if (_paging.Fields.ContainsKey(e.SortExpression))
                nsf = _paging.Fields[e.SortExpression];
            else if (_paging.Fields.ContainsKey("Lead." + e.SortExpression))
                nsf = _paging.Fields["Lead." + e.SortExpression];

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

            pageNumberer.CurrentPageIndex = 1;
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
                    var itemsIds = _paging.ItemsIds<int>("[Lead].LeadId as ID");
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
            }
        }

        protected string RenderOrderSource(string orderSourceId)
        {
            var orderSource = OrderSourceService.GetOrderSource(orderSourceId.TryParseInt());
            if (orderSource != null)
                return orderSource.Name;
            
            return "";
        }

        protected string RenderLeadStatus(string type)
        {
            var t = (LeadStatus)Enum.Parse(typeof(LeadStatus), type);
            return t.Localize();
        }
    }
}