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
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using Resources;

namespace Admin
{
    public partial class MailFormatPage : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_MailFormat_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "Settings.MailFormat", ItemsPerPage = 10 };

                var f = new Field { Name = "MailFormatID as ID", IsDistinct = true };
                _paging.AddField(f);

                f = new Field { Name = "FormatName" };
                _paging.AddField(f);

                f = new Field { Name = "MailFormatTypeId" };
                _paging.AddField(f);

                f = new Field { Name = "SortOrder", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                f = new Field { Name = "Enable" };
                _paging.AddField(f);

                f = new Field { Name = "AddDate" };
                _paging.AddField(f);

                f = new Field { Name = "ModifyDate" };
                _paging.AddField(f);


                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
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

                    _selectionFilter = new InSetFieldFilter();
                    if (arrids.Contains("-1"))
                    {
                        _selectionFilter.IncludeValues = false;
                        _inverseSelection = true;
                    }
                    else
                    {
                        _selectionFilter.IncludeValues = true;
                    }
                    _selectionFilter.Values = arrids.Where(id => id != "-1").ToArray();
                }

            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {
                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                    else
                    {
                        _selectionFilter = null; //New InSetFieldFilter()
                        //_SelectionFilter.IncludeValues = True
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Name filter
            _paging.Fields["FormatName"].Filter = !string.IsNullOrEmpty(txtName.Text) ? new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" } : null;

            //----Enabled filter
            if (ddluseInFilter.SelectedIndex != 0)
            {
                var efilter = new EqualFieldFilter { ParamName = "@Enable" };
                if (ddluseInFilter.SelectedIndex == 1)
                {
                    efilter.Value = "1";
                }
                if (ddluseInFilter.SelectedIndex == 2)
                {
                    efilter.Value = "0";
                }
                _paging.Fields["Enable"].Filter = efilter;
            }
            else
            {
                _paging.Fields["Enable"].Filter = null;
            }

            //----sort filter
            _paging.Fields["SortOrder"].Filter = !string.IsNullOrEmpty(txtSortOrder.Text) ? new CompareFieldFilter { Expression = txtSortOrder.Text, ParamName = "@sort" } : null;
            _paging.Fields["MailFormatTypeId"].Filter = ddlFormatType.SelectedValue != "0"
                                                      ? new EqualFieldFilter
                                                          {
                                                              ParamName = "@FormatType",
                                                              Value = ddlFormatType.SelectedValue
                                                          }
                                                      : null;
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

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {

                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        MailFormatService.Delete(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("MailFormatID as ID");
                    foreach (int id in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        MailFormatService.Delete(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteMailFormat")
            {
                MailFormatService.Delete(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"FormatName", "arrowName"},
                    {"AddDate", "arrowAddDate"},
                    {"ModifyDate", "arrowModifyDate"},
                    {"SortOrder", "arrowSortOrder"},
                    {"Enable", "arrowEnable"}
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
                var temp = MailFormatService.Get(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));
                temp.FormatName = grid.UpdatedRow["FormatName"];
                temp.Enable = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enable"]);
                temp.SortOrder = SQLDataHelper.GetInt(grid.UpdatedRow["SortOrder"]);
                temp.MailFormatTypeId = grid.UpdatedRow["MailFormatTypeId"].TryParseInt();

                MailFormatService.Update(temp);
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
                var formatTypes = MailFormatService.GetMailFormatTypes();
                var ddl = ((DropDownList) e.Row.FindControl("ddlFormatTypeET"));
                var mailFormatTypeId = SQLDataHelper.GetNullableInt(((DataRowView) e.Row.DataItem)["MailFormatTypeId"]);

                ddl.Items.Add(new ListItem("---", ""));

                foreach (var formatType in formatTypes)
                    ddl.Items.Add(new ListItem(formatType.TypeName, formatType.MailFormatTypeId.ToString()));
                

                if (mailFormatTypeId != null && ddl.Items.FindByValue(mailFormatTypeId.Value.ToString()) != null)
                    ddl.SelectedValue = mailFormatTypeId.Value.ToString();
            }
        }

        protected void btnAddMailFormat_Click(object sender, EventArgs e)
        {
            Response.Redirect("MailFormatDetail.aspx?ID=add");
        }
        protected void ddlFormatType_DataBound(object sender, EventArgs e)
        {
            ddlFormatType.Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "0"));
        }
        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }
    }
}
