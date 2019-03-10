//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin.UserControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Diagnostics;
using Resources;

namespace Admin
{
    public partial class ProductListMapping : AdvantShopAdminPage
    {
        #region Fields

        private SqlPaging _paging;
        //private bool _needReloadTree;
        private InSetFieldFilter _selectionFilter;
        private bool _inverseSelection;
        private int listId;

        #endregion

        protected ProductListMapping()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            listId = Request["listid"].TryParseInt();

            var list = ProductListService.Get(listId);
            if (list == null)
            {
                Response.Redirect("ProductLists.aspx");
                return;
            }

            lblHead.Text = list.Name;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, lblHead.Text));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Catalog].[Product] Left Join Catalog.Product_ProductList On Product_ProductList.ProductId = Product.ProductId",
                    ItemsPerPage = 20
                };

                _paging.AddFieldsRange(
                    new List<Field>
                    {
                        new Field {Name = "Product.ProductId as ID"},
                        new Field {Name = "ArtNo"},
                        new Field {Name = "Name"},
                        new Field {Name = "Enabled"},
                        new Field {Name = "SortOrder", Sorting = SortDirection.Ascending},
                        new Field {Name = "Product_ProductList.ListId", Filter = new EqualFieldFilter(){ ParamName = "@ListId", Value = listId.ToString()}}
                    });

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
                var nfilter = new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" };
                _paging.Fields["Name"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Name"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtSortOrder.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSortOrder.Text.TryParseInt().ToString(), ParamName = "@Sort" };
                _paging.Fields["SortOrder"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["SortOrder"].Filter = null;
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
                        ProductListService.DeleteProduct(listId, SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("Product.ProductId as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        ProductListService.DeleteProduct(listId, id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                ProductListService.DeleteProduct(listId, SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    {"SortOrder", "arrowSort"},
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
            popTree.UpdateTree(ProductListService.GetProductIds(listId));
            
            if (grid.UpdatedRow != null)
            {
                var prodcutId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]);
                var sortOrder = grid.UpdatedRow["SortOrder"].TryParseInt();

                ProductListService.UpdateProduct(listId, prodcutId, sortOrder);
            }

            var currentList = ProductListService.Get(Request["listid"].TryParseInt());
            txtDescription.Text = currentList.Description;

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
            foreach (var altId in args.SelectedValues)
            {
                ProductListService.AddProduct(listId, SQLDataHelper.GetInt(altId), 0);
            }
            popTree.UpdateTree(ProductListService.GetProductIds(listId));
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            popTree.Show();
        }
        protected void btnAddDescription_Click(object sender, EventArgs e)
        {
            var listid = Request["listid"].TryParseInt();
            if (listid == 0)
                return;
            var currentList = ProductListService.Get(listid);
            currentList.Description = txtDescription.Text;
            ProductListService.Update(currentList);
        }
    }
}