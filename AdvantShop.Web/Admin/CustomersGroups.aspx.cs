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
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class CustomersGroups : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        public CustomersGroups()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CustomerGroup_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Customers].[CustomerGroup]", ItemsPerPage = 10 };

                var f = new Field { Name = "CustomerGroupId as ID", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);

                f = new Field { Name = "GroupName", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                f = new Field { Name = "GroupDiscount" };
                _paging.AddField(f);

                f = new Field { Name = "MinimumOrderPrice" };
                _paging.AddField(f);

                grid.ChangeHeaderImageUrl("arrowGroupName", "images/arrowup.gif");

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
                    string[] arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length ];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        int t = int.Parse(arrids[idx]);
                        if (t != -1)
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
                    //_InverseSelection = If(ids(0) = -1, True, False)
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
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
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = _selectionFilter;
            }


            //----Name filter
            if (!string.IsNullOrEmpty(txtGroupName.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtGroupName.Text, ParamName = "@GroupName" };
                _paging.Fields["GroupName"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["GroupName"].Filter = null;
            }

            pageNumberer.CurrentPageIndex = 1;
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
                        CustomerGroupService.DeleteCustomerGroup(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CustomerGroupId as ID");
                    //IEnumerable<int> ids = CustomerGroupService.GetCustomerGroupListIds();
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        CustomerGroupService.DeleteCustomerGroup(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCustomerGroup")
            {
                CustomerGroupService.DeleteCustomerGroup(SQLDataHelper.GetInt(e.CommandArgument));
            }

            if (e.CommandName == "AddCustomerGroup")
            {
                GridViewRow footer = grid.FooterRow;

                var customerGroup = new CustomerGroup
                {
                    GroupName = ((TextBox)footer.FindControl("txtNewGroupName")).Text,
                    GroupDiscount = ((TextBox)footer.FindControl("txtNewGroupDiscount")).Text.TryParseFloat(),
                    MinimumOrderPrice = ((TextBox)footer.FindControl("txtNewMinimumOrderPrice")).Text.TryParseFloat()
                };

                if (customerGroup.GroupDiscount >= 100 || customerGroup.GroupDiscount < 0)
                {
                    customerGroup.GroupDiscount = 0;
                }

                CustomerGroupService.AddCustomerGroup(customerGroup);
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
                    {"GroupName", "arrowGroupName"},
                    {"GroupDiscount", "arrowGroupDiscount"},
                    {"MinimumOrderPrice", "arrowMinimumOrderPrice"}
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
            float discount = 0;
            float minimumOrderPrice = 0;
            if (grid.UpdatedRow != null && (float.TryParse(grid.UpdatedRow["GroupDiscount"].Replace("%", ""), out discount)) && (discount >= 0) && (discount < 100) &&
                float.TryParse(grid.UpdatedRow["MinimumOrderPrice"], out minimumOrderPrice))
            {
                int customerGroupId = 0;
                int.TryParse(grid.UpdatedRow["ID"], out customerGroupId);

                var customerGroup = new CustomerGroup
                    {
                        CustomerGroupId = customerGroupId,
                        GroupName = grid.UpdatedRow["GroupName"],
                        GroupDiscount = discount,
                        MinimumOrderPrice = minimumOrderPrice
                };

                CustomerGroupService.UpdateCustomerGroup(customerGroup);
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
                    if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["ID"].ToString()))
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

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void btnAddCustomerGroup_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        private void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNewGroupName").Focus();
            }
        }
    }
}