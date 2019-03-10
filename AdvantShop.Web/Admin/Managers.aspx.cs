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
using System.IO;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.SQL;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using Resources;

namespace Admin
{
    public partial class Managers : AdvantShopAdminPage
    {
        #region private

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        private List<Department> departments;

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

        public class respCanDelete
        {
            public string message { get; set; }
            public bool result { get; set; }
        }

        private void MsgErr(string messageText)
        {
            Message.Visible = true;
            Message.Text = "<br/>" + messageText;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(true);

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Departments_Header));


            if(SaasDataService.IsSaasEnabled && ManagerService.GetManagersCount() >= SaasDataService.CurrentSaasData.EmployeesCount)
            {
                btnAddManager.Attributes["disabled"] = "disabled";
            }

            departments = DepartmentService.GetDepartmentsList();

            if (!IsPostBack)
            {
                chkManagersEnabled.Checked = SettingsCheckout.EnableManagersModule;
                chkManagersPageShow.Checked = SettingsCheckout.ShowManagersPage;

                var now = DateTime.Now;

                txtDateFrom.Text = new DateTime(now.Year, now.Month, 1).ToString("dd.MM.yyyy");
                txtDateFrom.Attributes.Add("data-default-value", new DateTime(now.Year, now.Month, 1).ToString("dd.MM.yyyy"));
                txtDateTo.Text = now.ToString("dd.MM.yyyy");
                txtDateTo.Attributes.Add("data-default-value", now.ToString("dd.MM.yyyy"));

                _paging = new SqlPaging
                {
                    TableName = "[Customers].[Customer] " +
                                "INNER JOIN [Customers].[Managers] ON [Customer].CustomerID = [Managers].[CustomerId] " +
                                "LEFT JOIN [Catalog].[Photo] ON [ObjId] = [Managers].ManagerId and [type]=@TypePhoto",
                    ItemsPerPage = 20,
                    CurrentPageIndex = 1
                };

                _paging.AddParam(new SqlParam() { ParameterName = "@TypePhoto", Value = PhotoType.Manager.ToString() });

                _paging.AddFieldsRange(new[]
                    {
                        new Field {Name = "[Managers].ManagerId as ID", IsDistinct = true},
                        new Field {Name = "[Customer].CustomerID"},
                        new Field {Name = "Email"},
                        new Field {Name = "Firstname", Sorting = SortDirection.Ascending},
                        new Field {Name = "Lastname"},
                        new Field {Name = "PhotoName as Photo"},
                        new Field {Name = "DepartmentId"},
                        new Field {Name = "Active"},
                        new Field {Name = "(Select count(orderId) from [order].[order] inner join [order].[orderstatus] on [orderstatus].[OrderStatusID]=[order].[OrderStatusID]  " +
                                          "where ManagerId=[Managers].ManagerId ) as OrdersCountAssign"},
                        new Field {Name = "(Select count(orderId) from [order].[order] inner join [order].[orderstatus] on [orderstatus].[OrderStatusID]=[order].[OrderStatusID]  " +
                                          "where ManagerId=[Managers].ManagerId and [IsCompleted]=1 and PaymentDate is not null and OrderDate > @DateFrom and OrderDate < @DateTo) as OrdersCount"},
                        new Field {Name = "(Select Sum(sum) from [order].[order] inner join [order].[orderstatus] on [orderstatus].[OrderStatusID]=[order].[OrderStatusID]  " +
                                          "where ManagerId=[Managers].ManagerId and [IsCompleted]=1 and PaymentDate is not null and OrderDate > @DateFrom and OrderDate < @DateTo) as OrdersSum"},
                    });

                _paging.AddParam(new SqlParam() { ParameterName = "@DateFrom", Value = txtDateFrom.Text.TryParseDateTime() });
                _paging.AddParam(new SqlParam() { ParameterName = "@DateTo", Value = txtDateTo.Text.TryParseDateTime().AddDays(1) });
                grid.ChangeHeaderImageUrl("arrowLastname", "images/arrowup.gif");
                pageNumberer.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                ddlDepartment.Items.Clear();

                ddlDepartment.Items.Add(new ListItem(Resource.Admin_Catalog_Any, ""));
                ddlDepartment.Items.Add(new ListItem(Resource.Admin_Catalog_No, "null"));
                foreach (var department in departments)
                    ddlDepartment.Items.Add(new ListItem(department.Name, department.DepartmentId.ToString()));

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                var manager = ManagerService.GetManager(grid.UpdatedRow["ID"].TryParseInt());

                manager.DepartmentId = grid.UpdatedRow["DepartmentId"].TryParseInt(true);
                manager.Active = grid.UpdatedRow["Active"].TryParseBool();

                ManagerService.AddOrUpdateManager(manager);
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {
                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0 && _selectionFilter != null)
                {
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Firstname filter
            if (!string.IsNullOrEmpty(txtSearchFirstName.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchFirstName.Text, ParamName = "@firstname" };
                _paging.Fields["Firstname"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Firstname"].Filter = null;
            }

            //----lastname filter
            if (!string.IsNullOrEmpty(txtSearchLastname.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchLastname.Text, ParamName = "@lastname" };
                _paging.Fields["Lastname"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Lastname"].Filter = null;
            }

            //----email filter
            if (!string.IsNullOrEmpty(txtSearchEmail.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchEmail.Text, ParamName = "@email" };
                _paging.Fields["Email"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Email"].Filter = null;
            }

            if (ddlDepartment.SelectedValue != "")
            {
                if (ddlDepartment.SelectedValue == "null")
                    _paging.Fields["DepartmentId"].Filter = new NullFieldFilter
                    {
                        ParamName = "@DepartmentId",
                        Null = true
                    };
                else
                    _paging.Fields["DepartmentId"].Filter = new EqualFieldFilter
                    {
                        ParamName = "@DepartmentId",
                        Value = ddlDepartment.SelectedValue
                    };
            }
            else
                _paging.Fields["DepartmentId"].Filter = null;
            
            _paging.AddParam(new SqlParam() { ParameterName = "@DateFrom", Value = txtDateFrom.Text.TryParseDateTime() });
            _paging.AddParam(new SqlParam() { ParameterName = "@DateTo", Value = txtDateTo.Text.TryParseDateTime().AddDays(1) });

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

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {

                        int managerid = id.TryParseInt();
                        var message = string.Empty;
                        var manager = ManagerService.GetManager(managerid);
                        ManagerService.CanDelete(managerid, out message);

                        if (manager != null && !string.IsNullOrEmpty(message))
                        {
                            if (Message.Visible)
                            {
                                Message.Text = Message.Text + @"<br />" + message;
                            }
                            else
                            {
                                MsgErr(message);
                            }
                        }
                        else
                        {
                            ManagerService.ForcedDeleteManager(manager);
                        }
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<string>("[Managers].ManagerId as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        int managerid = id.TryParseInt();
                        var message = string.Empty;
                        var manager = ManagerService.GetManager(managerid);
                        ManagerService.CanDelete(managerid, out message);

                        if (manager != null && !string.IsNullOrEmpty(message))
                        {
                            if (Message.Visible)
                            {
                                Message.Text = Message.Text + @"<br />" + message;
                            }
                            else
                            {
                                MsgErr(message);
                            }
                        }
                        else
                        {
                            ManagerService.ForcedDeleteManager(manager);
                        }
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteManager")
            {
                var message = string.Empty;
                var manager = ManagerService.GetManager(SQLDataHelper.GetInt(e.CommandArgument));
                if (manager != null)
                {
                    ManagerService.ForcedDeleteManager(manager);
                }
                else
                {
                    MsgErr("Менеджер не найден");
                    return;
                }
                ManagerService.CanDelete(SQLDataHelper.GetInt(e.CommandArgument), out message);
                MsgErr(message);
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
                    {"Email", "arrowEmail"},
                    {"Lastname", "arrowLastname"},
                    {"Firstname", "arrowFirstname"},
                    {"DepartmentId", "arrowDepartmentId"},
                    {"Active", "arrowActive"},
                    {"OrdersCountAssign", "arrowOrdersCountAssign"},
                    {"OrdersCount", "arrowOrdersCount"},
                    {"OrdersSum", "arrowOrdersSum"}
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



        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        private void grid_DataBound(object sender, EventArgs e)
        {
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ddl = ((DropDownList)e.Row.FindControl("ddlDepartmentIDET"));
                ddl.Items.Clear();

                ddl.Items.Add(new ListItem(Resource.Admin_Catalog_No, "null"));
                foreach (var department in departments)
                    ddl.Items.Add(new ListItem(department.Name, department.DepartmentId.ToString()));

                ddl.SelectedValue =
                    ((DataRowView)e.Row.DataItem)["DepartmentId"].ToString();
            }
        }
        protected string GetImageItem(string photoName)
        {
            var abbr = "";

            if (!string.IsNullOrEmpty(photoName) && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.ManagerPhoto, photoName)))
            {
                abbr = FoldersHelper.GetPath(FolderType.ManagerPhoto, photoName, true);
            }
            return abbr == "" ? "" : string.Format("<img abbr='{0}' class='imgtooltip' src='images/adv_photo_ico.gif'>", abbr);
        }

        protected void chkManagersEnabled_CheckedChanged(object sender, EventArgs e)
        {
            SettingsCheckout.EnableManagersModule = chkManagersEnabled.Checked;
        }

        protected void chkManagersPageShow_CheckedChanged(object sender, EventArgs e)
        {
            SettingsCheckout.ShowManagersPage = chkManagersPageShow.Checked;
        }

        protected respCanDelete CanDelete(int managerId)
        {
            var result = new respCanDelete();
            var message = string.Empty;
            result.result = ManagerService.CanDelete(managerId, out message);
            result.message = message;
            return result;
        }

    }
}