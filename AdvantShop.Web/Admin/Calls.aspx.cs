//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Helpers;
using Resources;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;

namespace Admin
{
    public partial class Calls : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private bool _onlyMissedCalls;
        protected IPTelephonyOperator CurrentOperator;

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

        public Calls()
        {
            CurrentOperator = IPTelephonyOperator.Current;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentOperator.Type == EOperatorType.None)
            {
                Response.Redirect("CommonSettings.aspx#tabid=telephony");
                return;
            }

            MsgErr(true);
            SetMeta(string.Format("{0} - Журнал вызовов", SettingsMain.ShopName));

            _onlyMissedCalls = Request["Type"].IsNotEmpty() && Request["Type"] == ECallType.Missed.ToString();

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Customers].[Call]",
                    ItemsPerPage = 50
                };

                _paging.AddFieldsRange(
                    new Field("Id") { IsDistinct = true },
                    new Field("CallId"),
                    new Field("Type"),
                    new Field("SrcNum"),
                    new Field("DstNum"),
                    new Field("Extension"),
                    new Field("CallDate") { Sorting = SortDirection.Descending },
                    new Field("CallAnswerDate"),
                    new Field("Duration"),
                    new Field("RecordLink"),
                    new Field("CalledBack"),
                    new Field("HangupStatus"),
                    new Field("OperatorType")
                    );

                if (_onlyMissedCalls)
                {
                    _paging.Fields["Type"].Filter = new EqualFieldFilter { ParamName = "@Type", Value = ECallType.Missed.ToString() };
                    tdCallHangupStatus.Visible = tdCalledBack.Visible = true;
                    tdDuration.Visible = tdRecordLink.Visible = false;

                    lblSubHead.Text = "Пропущенные вызовы";
                    ddlCallType.SelectedValue = ECallType.Missed.ToString();
                    ddlCallType.Visible = false;

                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        if (new List<string> { "CalledBack", "HangupStatus" }.Contains(grid.Columns[i].AccessibleHeaderText))
                            grid.Columns[i].Visible = true;
                        else if (new List<string> { "Duration", "RecordLink" }.Contains(grid.Columns[i].AccessibleHeaderText))
                            grid.Columns[i].Visible = false;
                    }
                }

                if (SettingsCheckout.EnableManagersModule)
                {
                    _paging.TableName +=
                        " LEFT JOIN [Customers].[Managers] ON [Call].[ManagerId] = [Managers].[ManagerID] " +
                        " LEFT JOIN [Customers].[Customer] as ManagerCustomer ON [Managers].[CustomerId] = [ManagerCustomer].[CustomerId] ";
                    _paging.AddField(new Field("[ManagerCustomer].FirstName + ' ' + [ManagerCustomer].LastName as ManagerName"));
                    _paging.AddField(new Field("ManagerCustomer.CustomerId"));

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
                    ddlManagers.Items.Add(new ListItem(Resource.Admin_Catalog_Any, "any"));
                    ddlManagers.Items.Add(new ListItem(Resource.Admin_Catalog_No, "null"));
                    foreach (var manager in ManagerService.GetCustomerManagersList())
                        ddlManagers.Items.Add(new ListItem(string.Format("{0} {1}", manager.FirstName, manager.LastName), manager.Id.ToString()));
                }
                else
                {
                    //_paging.AddField(new Field("null as ManagerCustomer.CustomerId"));
                    _paging.AddField(new Field("'' as ManagerName"));
                }

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                grid.ChangeHeaderImageUrl("arrowCallDate", "images/arrowdown.gif");

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
                    txtSrcNum.Text = Request["search"];
                    btnFilter_Click(null, null);
                }
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

            if (Request["manager"].IsNotEmpty() && ddlManagers.Items.FindByValue(Request["manager"]) != null)
            {
                ddlManagers.SelectedValue = Request["manager"];

                _paging.Fields["ManagerCustomer.CustomerId"].Filter = new EqualFieldFilter
                {
                    ParamName = "@ManagerId",
                    Value = ddlManagers.SelectedValue
                };
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

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
            data.Columns.Add(clmn);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    int intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["Id"]).ToString()))
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
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    else
                        _selectionFilter = null;
                }
                flogical.AddFilter(_selectionFilter);
            }

            _paging.Fields["Id"].Filter = flogical.FilterCount() > 0 ? flogical : null;

            DateTime? dateFrom;
            DateTime? dateTo;
            var callDateFilter = new DateTimeRangeFieldFilter
            {
                ParamName = "@CallDate",
                From = (dateFrom = txtDateFrom.Text.TryParseDateTime(true)).HasValue
                    ? new DateTime(dateFrom.Value.Year, dateFrom.Value.Month, dateFrom.Value.Day, 0, 0, 0, 0)
                    : (DateTime?)null,
                To = (dateTo = txtDateTo.Text.TryParseDateTime(true)).HasValue
                    ? new DateTime(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, 23, 59, 59, 99)
                    : (DateTime?)null
            };
            _paging.Fields["CallDate"].Filter = callDateFilter.From.HasValue || callDateFilter.To.HasValue
                ? callDateFilter
                : null;

            _paging.Fields["SrcNum"].Filter = txtSrcNum.Text.IsNotEmpty()
                ? new CompareFieldFilter { ParamName = "@SrcNum", Expression = txtSrcNum.Text }
                : null;

            _paging.Fields["DstNum"].Filter = txtDstNum.Text.IsNotEmpty()
                ? new CompareFieldFilter { ParamName = "@DstNum", Expression = txtDstNum.Text }
                : null;

            _paging.Fields["Extension"].Filter = txtExtension.Text.IsNotEmpty()
                ? new CompareFieldFilter { ParamName = "@Extension", Expression = txtExtension.Text }
                : null;

            if (_onlyMissedCalls)
            {
                _paging.Fields["HangupStatus"].Filter = ddlCallHangupStatus.SelectedValue != "any"
                    ? new EqualFieldFilter { ParamName = "@HangupStatus", Value = ddlCallHangupStatus.SelectedValue }
                    : null;

                _paging.Fields["CalledBack"].Filter = ddlCalledBack.SelectedValue != "any"
                    ? new EqualFieldFilter { ParamName = "@CalledBack", Value = ddlCalledBack.SelectedValue }
                    : null;
            }
            else
            {
                _paging.Fields["Type"].Filter = ddlCallType.SelectedValue != "any"
                    ? new EqualFieldFilter { ParamName = "@Type", Value = ddlCallType.SelectedValue }
                    : null;

                var durationFilter = new RangeFieldFilter
                {
                    ParamName = "@Duration",
                    From = txtDurationFrom.Text.TryParseInt(true),
                    To = txtDurationTo.Text.TryParseInt(true)
                };
                _paging.Fields["Duration"].Filter = durationFilter.From.HasValue || durationFilter.To.HasValue
                    ? durationFilter
                    : null;
            }

            if (SettingsCheckout.EnableManagersModule)
            {
                _paging.Fields["ManagerCustomer.CustomerId"].Filter = ddlManagers.SelectedValue != "any"
                    ? ddlManagers.SelectedValue == "null"
                        ? (FieldFilter)new NullFieldFilter { ParamName = "@ManagerId", Null = true }
                        : new EqualFieldFilter { ParamName = "@ManagerId", Value = ddlManagers.SelectedValue }
                    : null;
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
            int pagen = txtPageNum.Text.TryParseInt();

            if (pagen >= 1 && pagen <= _paging.PageCount)
            {
                pageNumberer.CurrentPageIndex = pagen;
                _paging.CurrentPageIndex = pagen;
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"CallDate", "arrowCallDate"}
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

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
                        CallService.DeleteCall(id.TryParseInt());
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("Id");
                    foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
                    {
                        CallService.DeleteCall(id);
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

        }

        protected string RenderDuration(int seconds)
        {
            if (seconds <= 0)
                return string.Empty;

            var ts = TimeSpan.FromSeconds(seconds);
            var sb = new StringBuilder();
            if (ts.Hours > 0)
                sb.AppendFormat("{0} ч ", ts.Hours);
            if (ts.Minutes > 0)
                sb.AppendFormat("{0} мин ", ts.Minutes);
            if (ts.Seconds > 0)
                sb.AppendFormat("{0} с", ts.Seconds);
            return sb.ToString().Trim(' ');
        }

        protected void cbCalledBack_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var hf = (HiddenField)chk.NamingContainer.FindControl("hfId");

            CallService.SetCallCalledBack(hf.Value.TryParseInt(), chk.Checked);
        }

        protected void ddlFilter_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "any"));
        }
    }
}