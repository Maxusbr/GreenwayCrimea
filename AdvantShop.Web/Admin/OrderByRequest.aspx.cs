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
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using Resources;
using AdvantShop.Catalog;

namespace Admin
{
    public partial class OrderByRequest : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        public OrderByRequest()
        {
            _inverseSelection = false;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_OrderByRequest_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Order].[OrderByRequest]", ItemsPerPage = 10 };

                var f = new Field { Name = "OrderByRequestID as ID", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);

                f = new Field { Name = "ProductId" };
                _paging.AddField(f);

                f = new Field { Name = "ProductName" };
                _paging.AddField(f);

                f = new Field { Name = "UserName" };
                _paging.AddField(f);

                f = new Field { Name = "Email" };
                _paging.AddField(f);

                f = new Field { Name = "Phone" };
                _paging.AddField(f);

                f = new Field { Name = "IsComplete" };
                _paging.AddField(f);

                f = new Field { Name = "RequestDate", Sorting = SortDirection.Descending };
                _paging.AddField(f);

                grid.ChangeHeaderImageUrl("arrowRequestDate", "images/arrowdown.gif");

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
                    int t;
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        t = int.Parse(arrids[idx]);
                        if (t != -1)
                        {
                            ids[idx] = t.ToString(CultureInfo.InvariantCulture);
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


            if (!string.IsNullOrEmpty(txtProductName.Text.Trim()))
            {
                var nfilter = new CompareFieldFilter { Expression = txtProductName.Text, ParamName = "@ProductName" };
                _paging.Fields["ProductName"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["ProductName"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                var nfilter = new CompareFieldFilter { Expression = txtUserName.Text, ParamName = "@FLName" };
                _paging.Fields["UserName"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["UserName"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtEmail.Text, ParamName = "@Email" };
                _paging.Fields["Email"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Email"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtPhone.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtPhone.Text, ParamName = "@Phone" };
                _paging.Fields["Phone"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Phone"].Filter = null;
            }

            if (ddlIsComplete.SelectedIndex != 0)
            {
                var isCompleteFilter = new EqualFieldFilter { ParamName = "@IsComplete" };
                if (ddlIsComplete.SelectedIndex == 1)
                {
                    isCompleteFilter.Value = "1";
                }
                if (ddlIsComplete.SelectedIndex == 2)
                {
                    isCompleteFilter.Value = "0";
                }
                _paging.Fields["IsComplete"].Filter = isCompleteFilter;
            }
            else
            {
                _paging.Fields["IsComplete"].Filter = null;
            }

            var dfilter = new DateTimeRangeFieldFilter { ParamName = "@RequestDate" };
            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                DateTime d;
                if (DateTime.TryParse(txtDateFrom.Text, out d))
                {
                    var dt = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0, 0);
                    dfilter.From = dt;
                }
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                DateTime d;
                if (DateTime.TryParse(txtDateTo.Text, out d))
                {
                    var dt = new DateTime(d.Year, d.Month, d.Day, 23, 59, 59, 99);
                    dfilter.To = dt;
                }
            }

            _paging.Fields["RequestDate"].Filter = dfilter.From.HasValue || dfilter.To.HasValue ? dfilter : null;

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
                    foreach (string id in _selectionFilter.Values)
                    {
                        OrderByRequestService.DeleteOrderByRequest(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    foreach (int id in OrderByRequestService.GetIdList().Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        OrderByRequestService.DeleteOrderByRequest(id);
                    }
                }
            }
        }

        protected void lbSendMailSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        OrderByRequestService.SendConfirmationMessage(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("OrderByRequestID as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        OrderByRequestService.SendConfirmationMessage(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteOrderByRequest")
            {
                OrderByRequestService.DeleteOrderByRequest(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"ID", "arrowOrderByRequestID"},
                    {"Email", "arrowEmail"}, 
                    {"Phone", "arrowPhone"}, 
                    {"ProductName", "arrowProductName"}, 
                    {"IsComplete", "arrowIsComplete"}, 
                    {"RequestDate", "arrowRequestDate"}
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
                var orderByRequest = OrderByRequestService.GetOrderByRequest(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));

                if ((orderByRequest != null) && (orderByRequest.OrderByRequestId != 0))
                {
                    orderByRequest.IsComplete = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsComplete"]);
                    OrderByRequestService.UpdateOrderByRequest(orderByRequest);
                }
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
            lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void btnAddOrderByRequest_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        private void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNewName").Focus();
            }
        }

        protected string GetProductLink(int productId, string productName)
        {
            if (ProductService.IsExists(productId))
            {
                return string.Format("<a href=\"Product.aspx?ProductID={0}\">{1}</a>", productId, productName);
            }

            return productName;
        }
    }
}