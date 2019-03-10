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
using Admin.UserControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class ProductsOnMain : AdvantShopAdminPage
    {
        SqlPaging _paging;
        // private bool _needReloadTree;
        InSetFieldFilter _selectionFilter;
        bool _inverseSelection;
        private EProductOnMain _typeFlag = EProductOnMain.New;

        protected ProductsOnMain()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["type"]))
            {
                Enum.TryParse(Request["type"], true, out _typeFlag);
            }

            switch (_typeFlag)
            {
                case EProductOnMain.Best:
                    lblHead.Text = Resource.Admin_UserControls_MainPageProduct_Bestseller;
                    break;
                case EProductOnMain.New:
                    lblHead.Text = Resource.Admin_UserControls_MainPageProduct_New;
                    break;
                case EProductOnMain.Sale:
                    lblHead.Text = Resource.Admin_UserControls_MainPageProduct_Discount;
                    break;
            }

            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, lblHead.Text));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Catalog].[Product]", ItemsPerPage = 20 };

                var f = new Field { Name = "Product.ProductId as ID" };
                _paging.AddField(f);

                f = new Field { Name = "ArtNo" };
                _paging.AddField(f);

                f = new Field { Name = "Name" };
                _paging.AddField(f);

                if (_typeFlag == EProductOnMain.Best)
                {
                    f = new Field { Name = "Bestseller" };
                    var filterB = new EqualFieldFilter { ParamName = "@Bestseller", Value = "1" };
                    f.Filter = filterB;
                    _paging.AddField(f);

                    _paging.AddField(new Field { Name = "SortBestseller as Sort", Sorting = SortDirection.Ascending });
                    _paging.AddField(new Field { Name = "DateAdded as DateAddedSort", Sorting = SortDirection.Descending });
                }

                if (_typeFlag == EProductOnMain.New)
                {
                    f = new Field { Name = "New" };
                    var filterN = new EqualFieldFilter { ParamName = "@New", Value = "1" };
                    f.Filter = filterN;
                    _paging.AddField(f);

                    _paging.AddField(new Field { Name = "SortNew as Sort", Sorting = SortDirection.Ascending });
                    _paging.AddField(new Field { Name = "DateAdded as DateAddedSort", Sorting = SortDirection.Descending });
                }

                if (_typeFlag == EProductOnMain.Sale)
                {
                    f = new Field { Name = "Discount" };
                    var filterN = new NotEqualFieldFilter() { ParamName = "@Discount", Value = "0" };
                    f.Filter = filterN;
                    _paging.AddField(f);

                    _paging.AddField(new Field { Name = "SortDiscount as Sort", Sorting = SortDirection.Ascending });
                    _paging.AddField(new Field { Name = "Discount as DiscountSort", Sorting = SortDirection.Descending });
                    btnAddProduct.Visible = false;
                }

                grid.ChangeHeaderImageUrl("arrowSort", "images/arrowup.gif");

                _paging.ItemsPerPage = 20;

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

                    var ids = new string[arrids.Length];
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
                        _selectionFilter = null;
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Name filter
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtName.Text.Trim() , ParamName = "@Name" };
                _paging.Fields["Name"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Name"].Filter = null;
            }

            //----ArtNo filter
            if (!string.IsNullOrEmpty(txtArtNo.Text))
            {
                var nfilter = new EqualFieldFilter { Value = txtArtNo.Text.Trim(), ParamName = "@ArtNo" };
                _paging.Fields["ArtNo"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["ArtNo"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtSortOrder.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSortOrder.Text.TryParseInt().ToString(), ParamName = "@Sort" };
                _paging.Fields["Sort"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Sort"].Filter = null;
            }


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
                        ProductOnMain.DeleteProductByType(SQLDataHelper.GetInt(id), _typeFlag);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("Product.ProductId as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        ProductOnMain.DeleteProductByType(id, _typeFlag);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                if (_typeFlag == EProductOnMain.None) return;
                ProductOnMain.DeleteProductByType(SQLDataHelper.GetInt(e.CommandArgument), _typeFlag);
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    {"Sort", "arrowSort"},
                    {"ArtNo", "arrowArtNo"},
                };
            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
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
            popTree.UpdateTree(ProductOnMain.GetProductIdByType(_typeFlag));

            if (grid.UpdatedRow != null)
            {
                var prodcutId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]);
                var sortOrder = grid.UpdatedRow["Sort"].TryParseInt();
                ProductOnMain.UpdateProductByType(prodcutId, sortOrder, _typeFlag);
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

        protected void popTree_Selected(object sender, PopupTreeView.TreeNodeSelectedArgs args)
        {
            if (_typeFlag == EProductOnMain.None) return;

            foreach (var altId in args.SelectedValues)
            {
                ProductOnMain.AddProductByType(SQLDataHelper.GetInt(altId), _typeFlag);
            }
            popTree.UpdateTree(ProductOnMain.GetProductIdByType(_typeFlag));
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            popTree.Show();
        }
    }
}