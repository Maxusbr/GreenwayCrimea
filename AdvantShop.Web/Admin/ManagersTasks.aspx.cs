//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using Resources;

namespace Admin
{
    public partial class ManagersTasks : AdvantShopAdminPage
    {
        #region private

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private void MsgError(string error)
        {
            lblError.Visible = true;
            lblError.Text = error;
            upErrors.Update();
        }

        private void MsgError()
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Departments_Header));
            
            MsgError();

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Customers].[ManagerTask] " +
                                "INNER JOIN Customers.Managers as AppointedManager ON ManagerTask.AppointedManagerId = AppointedManager.ManagerId " +
                                "INNER JOIN Customers.Customer as AppointedCustomer ON AppointedCustomer.CustomerID = AppointedManager.CustomerId " +
                                "LEFT JOIN Customers.Customer as ClientCustomer ON ManagerTask.CustomerId = ClientCustomer.CustomerId ",
                    ItemsPerPage = 20,
                    CurrentPageIndex = 1
                };

                _paging.AddParam(new SqlParam() { ParameterName = "@TypePhoto", Value = PhotoType.Manager.ToString() });

                _paging.AddFieldsRange(new[]
                    {
                        new Field {Name = "TaskId as ID", IsDistinct = true},
                        new Field {Name = "AssignedManagerId"},
                        new Field {Name = "AppointedCustomer.CustomerId as AppointedCustomerId"},
                        new Field {Name = "AppointedCustomer.LastName + ' ' + AppointedCustomer.FirstName as AppointedName"},
                        new Field {Name = "Name"},
                        new Field {Name = "Status"},
                        new Field {Name = "DueDate"},
                        new Field {Name = "DateCreated", Sorting = SortDirection.Descending},
                        new Field {Name = "OrderId"},
                        new Field {Name = "(SELECT Number FROM [Order].[Order] WHERE OrderId = ManagerTask.OrderId) as OrderNumber"},
                        new Field {Name = @"(case ClientCustomer.FirstName when ISNULL(ClientCustomer.FirstName, ClientCustomer.Email) then  ClientCustomer.FirstName + 
							' ' + ClientCustomer.LastName end) as Email"},
                        new Field {Name = "ClientCustomer.CustomerID as ClientCustomerId"}
                    });

                if (CustomerContext.CurrentCustomer.IsManager && !CustomerContext.CurrentCustomer.IsAdmin)
                {
                    var manager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id);
                    ddlSearchAssignedManager.SelectedValue = manager.ManagerId.ToString();
                    _paging.Fields["AssignedManagerId"].Filter = new EqualFieldFilter { Value = manager.ManagerId.ToString(), ParamName = "@AssignedManagerId" };
                }

                grid.ChangeHeaderImageUrl("arrowDateCreated", "images/arrowdown.gif");
                pageNumberer.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
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
                        var t = arrids[idx].TryParseInt();
                        if (t != 0)
                        {
                            ids[idx] = t.ToString();
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {
                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0 && _selectionFilter != null)
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
                _paging.Fields["ID"].Filter = null;

            _paging.Fields["ID"].Filter = !string.IsNullOrWhiteSpace(txtTaskId.Text)
                ? new EqualFieldFilter { Value = txtTaskId.Text.Trim(), ParamName = "@TaskId" }
                : null;

            _paging.Fields["Status"].Filter = ddlSearchStatus.SelectedValue != "any"
                ? new EqualFieldFilter {Value = ddlSearchStatus.SelectedValue, ParamName = "@Status"}
                : null;

            _paging.Fields["Name"].Filter = txtSearchName.Text.IsNullOrEmpty()
                ? null
                : new CompareFieldFilter {Expression = txtSearchName.Text, ParamName = "@Name"};

            _paging.Fields["AssignedManagerId"].Filter = ddlSearchAssignedManager.SelectedValue != "any"
                ? new EqualFieldFilter { Value = ddlSearchAssignedManager.SelectedValue, ParamName = "@AssignedManagerId" }
                : null;

            _paging.Fields["AppointedName"].Filter = txtSearchAppointedManager.Text.IsNullOrEmpty()
                ? null
                : new CompareFieldFilter { Expression = txtSearchAppointedManager.Text, ParamName = "@AppointedName" };

            _paging.Fields["OrderId"].Filter = txtSearchOrderId.Text.IsNullOrEmpty()
                ? null
                : new EqualFieldFilter { Value = txtSearchOrderId.Text, ParamName = "@OrderId" };

            _paging.Fields["ClientCustomer.Email"].Filter = txtSearchClientEmail.Text.IsNullOrEmpty()
                ? null
                : new CompareFieldFilter { Expression = txtSearchClientEmail.Text, ParamName = "@Email" };

            pageNumberer.CurrentPageIndex = 1;
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
            int pagen = txtPageNum.Text.TryParseInt(-1);
            if (pagen >= 1 && pagen <= _paging.PageCount)
            {
                pageNumberer.CurrentPageIndex = pagen;
                _paging.CurrentPageIndex = pagen;
            }
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        ManagerTaskService.DeleteManagerTask(id.TryParseInt());
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<string>("TaskId as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        ManagerTaskService.DeleteManagerTask(id.TryParseInt());
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteManagerTask")
            {
                ManagerTaskService.DeleteManagerTask(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"TaskId", "arrowTaskId"},
                    {"Name", "arrowName"},
                    {"DueDate", "arrowDueDate"},
                    {"DateCreated", "arrowDateCreated"},
                    {"OrderId", "arrowOrderId"}
                };
            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                var task = ManagerTaskService.GetManagerTask(grid.UpdatedRow["ID"].TryParseInt());

                var prevStatus = task.Status;
                var prevAssignedManagerId = task.AssignedManagerId;

                task.Status = (ManagerTaskStatus)grid.UpdatedRow["Status"].TryParseInt(); ;
                task.AssignedManagerId = grid.UpdatedRow["AssignedManagerId"].TryParseInt();
                task.Name = grid.UpdatedRow["Name"];

                ManagerTaskService.UpdateManagerTask(task);

                if (task.AssignedManagerId != prevAssignedManagerId)
                    ManagerTaskService.OnSetManagerTask(task);
                if (task.Status != prevStatus)
                    ManagerTaskService.OnChangeManagerTaskStatus(task);
            }

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
            lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ddlStatus = ((DropDownList)e.Row.FindControl("ddlStatus"));
                ddlStatus.SelectedValue = ((DataRowView) e.Row.DataItem)["Status"].ToString();

                var ddlAssignedManager = ((DropDownList)e.Row.FindControl("ddlAssignedManager"));
                ddlAssignedManager.SelectedValue = ((DataRowView)e.Row.DataItem)["AssignedManagerId"].ToString();
            }
        }

        protected void ddlFilter_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "any"));
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }
    }
}