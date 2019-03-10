//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using Resources;

namespace Admin
{
    public partial class Catalog : AdvantShopAdminPage
    {
        #region EShowMethod enum

        public enum EShowMethod
        {
            Normal,
            AllProducts,
            OnlyInCategories,
            OnlyWithoutCategories
        }

        #endregion

        protected int CategoryId = -1;
        private bool _inverseSelection;
        private bool _needReloadTree;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        protected EShowMethod ShowMethod = EShowMethod.Normal;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_MasterPageAdminCatalog_Catalog));

            Category cat = null;

            if (!string.IsNullOrEmpty(Request["categoryid"]))
            {
                if (Request["categoryid"].ToLower().Equals("WithoutCategory".ToLower()))
                {
                    ShowMethod = EShowMethod.OnlyWithoutCategories;
                }
                else if (Request["categoryid"].ToLower().Equals("InCategories".ToLower()))
                {
                    ShowMethod = EShowMethod.OnlyInCategories;
                }
                else if (Request["categoryid"].ToLower().Equals("AllProducts".ToLower()))
                {
                    ShowMethod = EShowMethod.AllProducts;
                }
                else
                {
                    ShowMethod = EShowMethod.Normal;
                    int.TryParse(Request["categoryid"], out CategoryId);
                    cat = CategoryService.GetCategory(CategoryId);
                    adminCategoryView.CategoryID = CategoryId;
                }
            }
            else
            {
                CategoryId = 0;
                ShowMethod = EShowMethod.Normal;
            }

            if (cat == null)
            {
                CategoryId = 0;
                if (ShowMethod == EShowMethod.Normal)
                {
                    ShowMethod = EShowMethod.AllProducts;
                    ShowMethod = EShowMethod.Normal;
                }
            }
            else
            {
                CategoryId = cat.CategoryId;
                lblCategoryName.Text = cat.Name;
                hlDeleteCategory.Attributes["data-confirm"] =
                    string.Format(Resource.Admin_MasterPageAdminCatalog_Confirmation, cat.Name);
            }

            

            hlEditCategory.NavigateUrl = "m_Category.aspx?CategoryID=" + CategoryId;

            if (!IsPostBack)
            {
                var node2 = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = "0", Selected = true };
                tree2.Nodes.Add(node2);

                LoadChildCategories2(tree2.Nodes[0]);

                _paging = new SqlPaging()
                    {
                        ItemsPerPage = 10,
                    };

                switch (ShowMethod)
                {
                    case EShowMethod.AllProducts:
                        lblCategoryName.Text = Resource.Admin_Catalog_AllProducts;
                        _paging.TableName =
                            "[Catalog].[Product] left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and [Offer].[Main] = 1 LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId]  and Type='Product' AND [Photo].[Main] = 1 LEFT JOIN [Catalog].[ProductCategories] ON [Catalog].[ProductCategories].[ProductID] = [Product].[ProductID] LEFT JOIN [Catalog].[Currency] ON [Product].[CurrencyID] = [Currency].[CurrencyID]";
                        break;

                    case EShowMethod.OnlyInCategories:
                        lblCategoryName.Text = Resource.Admin_Catalog_AllProductsInCategories;
                        _paging.TableName =
                            "[Catalog].[Product] left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and [Offer].[Main] = 1 LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type='Product' AND [Photo].[Main] = 1 inner JOIN [Catalog].[ProductCategories] ON [Catalog].[ProductCategories].[ProductID] = [Product].[ProductID] LEFT JOIN [Catalog].[Currency] ON [Product].[CurrencyID] = [Currency].[CurrencyID]";
                        break;

                    case EShowMethod.OnlyWithoutCategories:
                        lblCategoryName.Text = Resource.Admin_Catalog_AllProductsWithoutCategories;
                        _paging.TableName =
                            "[Catalog].[Product] inner join (select ProductId from Catalog.Product where ProductId not in(Select ProductId from Catalog.ProductCategories)) as tmp on tmp.ProductId=[Product].[ProductID] Left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and [Offer].[Main] = 1 LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId]  and Type='Product' AND [Photo].[Main] = 1 LEFT JOIN [Catalog].[Currency] ON [Product].[CurrencyID] = [Currency].[CurrencyID]";
                        break;

                    case EShowMethod.Normal:
                        _paging.TableName =
                            "[Catalog].[Product] left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and [Offer].[Main] = 1 LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type='Product' AND [Photo].[Main] = 1 INNER JOIN Catalog.ProductCategories on ProductCategories.ProductId = [Product].[ProductID] LEFT JOIN [Catalog].[Currency] ON [Product].[CurrencyID] = [Currency].[CurrencyID]";
                        break;
                }

                _paging.AddFieldsRange(new List<Field>()
                    {
                        new Field {Name = "Product.ProductID as ID", IsDistinct = true},
                    
                        new Field {Name = "[Settings].[ArtNoToString](Product.ProductID) as ArtNo", Sorting = ShowMethod!= EShowMethod.Normal ? SortDirection.Ascending : (SortDirection?)null},
                        new Field {Name = "Product.ArtNo as ProductArtNo"},
                        new Field {Name = "PhotoName"},
                        new Field {Name = "(Select Count(ProductID) From Catalog.ProductCategories Where ProductID=Product.ProductID) as ProductCategoriesCount"},
                        new Field {Name = "BriefDescription"},
                        new Field {Name = "[Product].Name as Name"},
                        new Field {Name = "Price"},
                        new Field {Name = "(Select sum (Amount) from catalog.Offer where Offer.ProductID=Product.productID) as Amount"},
                        new Field {Name = "(Select count (offerid) from catalog.Offer where Offer.ProductID=Product.productID) as OffersCount"},
                        new Field {Name = "Enabled"},

                        new Field {Name = "[Currency].Code"},
                        new Field {Name = "[Currency].CurrencyIso3"},
                        new Field {Name = "[Currency].CurrencyValue"},
                        new Field {Name = "[Currency].IsCodeBefore"},
                        new Field {Name = "Offer.ArtNo as OfferArtNo"},

                        new Field
                            {
                                Name = ShowMethod == EShowMethod.Normal ? "ProductCategories.SortOrder as SortOrder" : "-1 as SortOrder",
                                Sorting = SortDirection.Ascending
                            },
                        new Field {Name = "Offer.ColorID", NotInQuery = true},
                        new Field {Name = "Offer.SizeID", NotInQuery = true}
                    });

                if (ShowMethod == EShowMethod.Normal)
                {
                    _paging.AddField(new Field
                        {
                            Name = "ProductCategories.CategoryID",
                            NotInQuery = true,
                            Filter = new EqualFieldFilter
                                {
                                    Value = CategoryId.ToString(),
                                    ParamName = "@CategoryID"
                                }
                        });

                    grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");
                }
                else
                {
                    grid.ChangeHeaderImageUrl("arrowArtNo", "images/arrowup.gif");
                }

                pageNumberer.CurrentPageIndex = 1;
                pnlProducts.Visible = CategoryId != 0 || ShowMethod != EShowMethod.Normal;
                productsHeader.Visible = ShowMethod == EShowMethod.Normal;
                adminCategoryView.Visible = ShowMethod == EShowMethod.Normal;
                grid.Columns[9].Visible = ShowMethod == EShowMethod.Normal;
                pnlAddProduct.Visible = CategoryId != 0;

                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;


                if (Request["search"].IsNotEmpty())
                {
                    var product = ProductService.GetProduct(Request["search"], true);
                    if (product != null)
                    {
                        Response.Redirect("Product.aspx?productID=" + product.ProductId);
                        return;
                    }

                    txtName.Text = Request["search"];
                    txtArtNo.Text = Request["search"];

                    btnFilter_Click(null, null);
                }

                if (Request["colorId"].IsNotEmpty())
                {
                    var color = ColorService.GetColor(Request["colorId"].TryParseInt());
                    if (color != null)
                    {
                        lblCategoryName.Text += string.Format(" {0}: {1}", Resource.Admin_Catalog_Color, color.ColorName);
                        _paging.Fields["Offer.ColorID"].Filter = new EqualFieldFilter { ParamName = "@colorId", Value = Request["colorId"] };
                    }
                }

                if (Request["sizeid"].IsNotEmpty())
                {
                    var size = SizeService.GetSize(Request["sizeid"].TryParseInt());
                    if (size != null)
                    {
                        lblCategoryName.Text += string.Format(" {0}: {1}", Resource.Admin_Catalog_Size, size.SizeName);
                        _paging.Fields["Offer.SizeID"].Filter = new EqualFieldFilter { ParamName = "@sizeId", Value = Request["sizeId"] };
                    }
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
                        string t = arrids[idx];
                        var idParts = t.Split('_');
                        switch (idParts[0])
                        {
                            case "Product":
                                if (idParts[1] != "-1")
                                {
                                    ids[idx] = idParts[1];
                                }
                                else
                                {
                                    _selectionFilter.IncludeValues = false;
                                    _inverseSelection = true;
                                }
                                break;
                            default:
                                _inverseSelection = true;
                                break;
                        }
                    }
                    _selectionFilter.Values = ids.Distinct().Where(item => item != null).ToArray();
                }
            }
        }


        protected void hlDeleteCategory_Click(object sender, EventArgs e)
        {
            var needRedirect = false;
            var cat = CategoryService.GetCategory(CategoryId);
            try
            {

                if (CategoryId == -1)
                {
                    return;
                }
                CategoryService.DeleteCategoryAndPhotos(CategoryId);
                CategoryService.DeleteCategoryLink(CategoryId);
                CategoryService.RecalculateProductsCountManual();
                needRedirect = true;

            }
            catch (Exception ex)
            {
                lMessage.Text = ex.Message;
                lMessage.Visible = true;
                Debug.Log.Error(ex);
            }

            if (needRedirect)
            {
                Response.Redirect("Catalog.aspx?CategoryID=" + cat.ParentCategoryId);
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
                if (_selectionFilter != null)
                {
                    _paging.Fields["ID"].Filter = _selectionFilter;
                }
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Enabled filter
            if (ddlEnabled.SelectedIndex != 0)
            {
                var efilter = new EqualFieldFilter { ParamName = "@enabled" };
                if (ddlEnabled.SelectedIndex == 1)
                {
                    efilter.Value = "1";
                }
                if (ddlEnabled.SelectedIndex == 2)
                {
                    efilter.Value = "0";
                }
                _paging.Fields["Enabled"].Filter = efilter;
            }
            else
            {
                _paging.Fields["Enabled"].Filter = null;
            }

            //----Price filter

            var priceFilter = new RangeFieldFilter { ParamName = "@price" };

            int priceFrom;
            priceFilter.From = int.TryParse(txtPriceFrom.Text, out priceFrom) ? priceFrom : 0;

            int priceTo;
            priceFilter.To = int.TryParse(txtPriceTo.Text, out priceTo) ? priceTo : int.MaxValue;

            if (!string.IsNullOrEmpty(txtPriceFrom.Text) || !string.IsNullOrEmpty(txtPriceTo.Text))
            {
                _paging.Fields["Price"].Filter = priceFilter;
            }
            else
            {
                _paging.Fields["Price"].Filter = null;
            }

            //----Qty filter
            var qtyFilter = new RangeFieldFilter { ParamName = "@Amount" };
            int from;
            qtyFilter.From = int.TryParse(txtQtyFrom.Text, out from) ? from : int.MinValue;

            int to;
            qtyFilter.To = int.TryParse(txtQtyTo.Text, out to) ? to : int.MaxValue;


            if (!string.IsNullOrEmpty(txtQtyFrom.Text) || !string.IsNullOrEmpty(txtQtyTo.Text))
            {
                _paging.Fields["Amount"].Filter = qtyFilter;
            }
            else
            {
                _paging.Fields["Amount"].Filter = null;
            }

            //----SortOrder filter
            var soFilter = new RangeFieldFilter { ParamName = "@SortOrder" };

            try
            {
                soFilter.From = int.Parse(txtSortOrderFrom.Text);
            }
            catch (Exception)
            {
                soFilter.From = int.MinValue;
            }

            try
            {
                soFilter.To = int.Parse(txtSortOrderTo.Text);
            }
            catch (Exception)
            {
                soFilter.To = int.MaxValue;
            }

            if (ShowMethod == EShowMethod.Normal)
            {
                if (!string.IsNullOrEmpty(txtSortOrderFrom.Text) || !string.IsNullOrEmpty(txtSortOrderTo.Text))
                {
                    _paging.Fields["SortOrder"].Filter = soFilter;
                }
                else
                {
                    _paging.Fields["SortOrder"].Filter = null;
                }
            }

            if (!string.IsNullOrEmpty(txtArtNo.Text))
            {
                _paging.Fields["ProductArtNo"].Filter = new CompareFieldFilter() { Expression = txtArtNo.Text.Trim(), ParamName = "@artno" };
                _paging.Fields["OfferArtNo"].Filter = new CompareFieldFilter() { Expression = txtArtNo.Text.Trim(), ParamName = "@offerArtNo" };
            }
            else
            {
                _paging.Fields["ProductArtNo"].Filter = null;
                _paging.Fields["OfferArtNo"].Filter = null;
            }

            //----Name filter
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtName.Text, ParamName = "@name" };
                _paging.Fields["Name"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Name"].Filter = null;
            }

            //---Photo filter
            if (ddPhoto.SelectedIndex != 0)
            {
                var phfilter = new NullFieldFilter();
                if (ddPhoto.SelectedIndex == 1)
                {
                    phfilter.Null = false;
                }
                if (ddPhoto.SelectedIndex == 2)
                {
                    phfilter.Null = true;
                }
                phfilter.ParamName = "@PhotoName";
                _paging.Fields["PhotoName"].Filter = phfilter;
            }
            else
            {
                _paging.Fields["PhotoName"].Filter = null;
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            //lblFound.Text = _paging.TotalRowsCount.ToString();
            //pnlFilterCount.Visible = true;
            //pnlNormalCount.Visible = false;
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
            if (pagen < 1 || pagen > _paging.PageCount) return;
            pageNumberer.CurrentPageIndex = pagen;
            _paging.CurrentPageIndex = pagen;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                var cproduct = ProductService.GetProduct(SQLDataHelper.GetInt(grid.UpdatedRow["Id"].Replace("Product_", "")));
                cproduct.Name = grid.UpdatedRow["Name"];

                bool enabledChanged = cproduct.Enabled != bool.Parse(grid.UpdatedRow["Enabled"]);
                cproduct.Enabled = bool.Parse(grid.UpdatedRow["Enabled"]);
                cproduct.ModifiedBy = CustomerContext.CustomerId.ToString();

                if (grid.UpdatedRow.ContainsKey("SortOrder"))
                {
                    int srtOrd;
                    int.TryParse(grid.UpdatedRow["SortOrder"], out srtOrd);
                    if (ShowMethod == EShowMethod.Normal && CategoryId > 0)
                    {
                        ProductService.UpdateProductLinkSort(cproduct.ProductId, srtOrd, SQLDataHelper.GetInt(Request["categoryid"]));
                    }
                }

                if (cproduct.Offers.Count == 1)
                {
                    cproduct.Offers[0].Amount = grid.UpdatedRow["Amount"].Replace(" ", "").TryParseFloat(); //replacing not a simple 'space'
                    cproduct.Offers[0].BasePrice = grid.UpdatedRow["Price"].Replace(" ", "").TryParseFloat(); //replacing not a simple 'space'
                }

                ProductService.UpdateProduct(cproduct, true);

                if (enabledChanged)
                {
                    CategoryService.RecalculateProductsCountManual();
                    _needReloadTree = true;
                }
            }

            var data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
            data.Columns.Add(clmn);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (var i = 0; i <= data.Rows.Count - 1; i++)
                {
                    var intIndex = i;
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


            if (!IsPostBack || _needReloadTree)
            {
                //ibRecalculate.Attributes.Add("onmouseover", "this.src=\'images/broundarrow.gif\';");
                //tree.Nodes.Clear();
                //LoadRootCategories(tree.Nodes);

                //var parentCategories = CategoryService.GetParentCategories(CategoryId);

                //var nodes = tree.Nodes;

                //if (CategoryId == 0 && ShowMethod == EShowMethod.Normal)
                //{
                //    var node = (from TreeNode n in nodes select n).First();
                //    node.Select();
                //}

                //for (var i = parentCategories.Count - 1; i >= 0; i--)
                //{
                //    var ii = i;
                //    var tn = (from TreeNode n in nodes where n.Value == parentCategories[ii].CategoryId.ToString() select n).SingleOrDefault();
                //    if (i == 0)
                //    {
                //        tn.Select();
                //        tn.Expand();
                //    }
                //    else
                //    {
                //        tn.Expand();
                //    }

                //    nodes = tn.ChildNodes;
                //}
            }

            grid.DataSource = data;
            grid.DataBind();

            pageNumberer.PageCount = _paging.PageCount;

            switch (ShowMethod)
            {
                case EShowMethod.AllProducts:
                    
                    hlEditCategory.Visible = false;
                    hlDeleteCategory.Visible = false;
                    lblSeparator.Visible = false;
                    sn.Visible = false;
                    break;
                case EShowMethod.OnlyWithoutCategories:
                    
                    hlEditCategory.Visible = false;
                    hlDeleteCategory.Visible = false;
                    lblSeparator.Visible = false;
                    sn.Visible = false;
                    break;
                case EShowMethod.OnlyInCategories:
                    
                    hlEditCategory.Visible = false;
                    hlDeleteCategory.Visible = false;
                    lblSeparator.Visible = false;
                    sn.Visible = false;
                    break;

                case EShowMethod.Normal:
                    if (CategoryId == 0)
                    {
                        lblSeparator.Visible = false;
                        hlDeleteCategory.Visible = false;
                    }
                    break;
            }

            lblProducts.Text = _paging.TotalRowsCount.ToString();

            if (CategoryId != 0)
            {
                sn.BuildNavigationAdmin(CategoryId);
            }
            else
            {
                sn.Visible = false;
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"ProductArtNo", "arrowArtNo"},
                    {"Name", "arrowName"},
                    {"Price", "arrowPrice"},
                    {"Amount", "arrowAmount"},
                    {"Enabled", "arrowEnabled"},
                    {"SortOrder", "arrowSortOrder"},
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            var nsf = new Field();
            if (e.SortExpression.Equals("SortOrder"))
            {
                switch (ShowMethod)
                {
                    case EShowMethod.Normal:
                        nsf = _paging.Fields["SortOrder"];
                        break;
                }
            }
            else
            {
                nsf = _paging.Fields[e.SortExpression];
            }

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

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (SaasDataService.IsSaasEnabled)
            {
                var maxProductCount = SaasDataService.CurrentSaasData.ProductsCount;
                var productsCount = ProductService.GetProductsCount();

                if (productsCount >= maxProductCount)
                {
                    lMessage.Text = Resource.Admin_Product_MaximumProducts + " - " + maxProductCount;
                    lMessage.Visible = true;
                    return;
                }
            }

            Response.Redirect("Product.aspx?categoryid=" + CategoryId);
        }

        protected void lbDeleteSelected1_Click(object sender, EventArgs e)
        {
            if (!_inverseSelection)
            {
                if (_selectionFilter != null)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        ProductService.DeleteProduct(SQLDataHelper.GetInt(id), true);
                    }
                }
            }
            else
            {
                foreach (int id in _paging.ItemsIds<int>("[Product].[ProductID] as ID").Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    ProductService.DeleteProduct(id, true);
                }
            }
            CategoryService.RecalculateProductsCountManual();
            _needReloadTree = true;
            //Response.Redirect("Catalog.aspx" + Request.Url.Query);
        }

        protected void lbDeleteSelectedFromCategory_Click(object sender, EventArgs e)
        {
            if (!_inverseSelection)
            {
                if (_selectionFilter != null)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        CategoryService.DeleteCategoryAndLink(SQLDataHelper.GetInt(id), SQLDataHelper.GetInt(Request["categoryid"]));
                    }
                }
            }
            else
            {
                foreach (var p in _paging.ItemsIds<int>("[Product].[ProductID] as ID").Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    CategoryService.DeleteCategoryAndLink(p, SQLDataHelper.GetInt(Request["categoryid"]));
                }
            }
            CategoryService.RecalculateProductsCountManual();
            Response.Redirect("Catalog.aspx" + Request.Url.Query);
        }

        protected void lbActivateSelected_Click(object sender, EventArgs e)
        {
            if (!_inverseSelection)
            {
                if (_selectionFilter != null)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        ProductService.SetActive(SQLDataHelper.GetInt(id), true);
                    }
                }
            }
            else
            {
                foreach (var id in _paging.ItemsIds<int>("[Product].[ProductID] as ID").Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    ProductService.SetActive(id, true);
                }
            }
            CategoryService.RecalculateProductsCountManual();
            _needReloadTree = true;
        }

        protected void lbDisableSelected_Click(object sender, EventArgs e)
        {
            if (!_inverseSelection)
            {
                if (_selectionFilter != null)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        ProductService.SetActive(SQLDataHelper.GetInt(id), false);
                    }
                }
            }
            else
            {
                foreach (var id in _paging.ItemsIds<int>("[Product].[ProductID] as ID").Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    ProductService.SetActive(id, false);
                }
            }
            CategoryService.RecalculateProductsCountManual();
            _needReloadTree = true;
        }


        protected void tree_TreeNodeCommand(object sender, CommandEventArgs e)
        {
            var needRedirect = false;
            try
            {
                if (e.CommandName.StartsWith("DeleteCategory"))
                {
                    int catId = CategoryId;
                    if (e.CommandName.Contains("#"))
                    {
                        catId = SQLDataHelper.GetInt(e.CommandName.Substring(e.CommandName.IndexOf("#") + 1));
                    }

                    if (catId == -1)
                    {
                        return;
                    }
                    if (catId != 0)
                    {
                        CategoryService.DeleteCategoryAndPhotos(catId);
                        CategoryService.DeleteCategoryLink(catId);
                        CategoryService.RecalculateProductsCountManual();
                        needRedirect = true;
                    }
                    else
                    {
                        lMessage.Text = Resource.Admin_Catalog_CantDellRoot;
                        lMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lMessage.Text = ex.Message;
                lMessage.Visible = true;
                //Debug.Log.Error(ex, sender, e);
                Debug.Log.Error(ex);
            }

            if (needRedirect)
            {
                Response.Redirect("Catalog.aspx");
            }
        }


        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Deletelink":
                    string cat = Request["categoryid"] ?? CategoryId.ToString();
                    CategoryService.DeleteCategoryAndLink(SQLDataHelper.GetInt(e.CommandArgument), SQLDataHelper.GetInt(cat));
                    _needReloadTree = true;
                    break;
                case "DeleteAll":
                    ProductService.DeleteProduct(SQLDataHelper.GetInt(e.CommandArgument), true);
                    _needReloadTree = true;
                    break;
                case "DeleteCategory":
                    CategoryService.DeleteCategoryAndPhotos(SQLDataHelper.GetInt(e.CommandArgument));
                    break;
            }
            CategoryService.RecalculateProductsCountManual();
        }


        protected string RenderDivHeader()
        {
            string divHeader;
            if (Request.Browser.Browser == "IE")
            {
                var c = new CultureInfo("en-us");
                divHeader = double.Parse(Request.Browser.Version, c.NumberFormat) < 7 ? "<div class=\'mtree_ie6\'>" : "<div class=\'mtree_ie\'>";
            }
            else
            {
                divHeader = "<div class=\'mtree\'>";
            }
            return divHeader;
        }

        protected string RenderDivBottom()
        {
            return "</div>";
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null) return;
            var t = (DataRowView)e.Row.DataItem;

            if (ShowMethod == EShowMethod.Normal)
            {
                ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).HRef =
                    "Product.aspx?ProductID=" + t["Id"] + "&categoryid=" + CategoryId;
            }
            else
            {
                ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).HRef =
                    "Product.aspx?ProductID=" + t["Id"];
            }
            ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).Title =
                Resource.Admin_MasterPageAdminCatalog_Edit;
            ((LinkButton)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).CommandName =
                "DeleteAll";

            ((LinkButton)
             (e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).Attributes["data-confirm"]
                = string.Format(Resource.Admin_Product_ConfirmDeletingProduct, t["Name"]);

            e.Row.Attributes["rowType"] = "product";
            e.Row.Attributes["element_id"] = t["Id"].ToString();
            e.Row.Attributes["categoryId"] = CategoryId.ToString();

            var tr = new AsyncPostBackTrigger
                {
                    ControlID = ((e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).UniqueID,
                    EventName = "Click"
                };

            UpdatePanel1.Triggers.Add(tr);
        }

        protected string GetImageItem(string photoName)
        {
            if (File.Exists(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, photoName)))
            {
                var abbr = FoldersHelper.GetImageProductPath(ProductImageType.Small, photoName, true);
                return string.Format("<img abbr='{0}' class='imgtooltip' src='{1}'>", abbr, "images/tooltip_product.gif");
            }

            return "";
        }
        
        #region Перенос товаров в другую категорию

        protected void btnChangeProductCategory_Click(object sender, EventArgs e)
        {
            // Перенос продуктов в другую категорию
            if (!string.IsNullOrEmpty(tree2.SelectedValue) && tree2.SelectedValue != "0")
            {
                ChangeProductsCategory();
                Response.Redirect("Catalog.aspx" + Request.Url.Query);
            }
        }

        /// <summary>
        /// Перенос выбранных товаров в другую категорию
        /// </summary>
        private void ChangeProductsCategory()
        {
            var categoryId = SQLDataHelper.GetInt(tree2.SelectedValue);
            var removeFromCurrentCategories = !chkChangeStatus.Checked;

            if (!_inverseSelection)
            {
                if (_selectionFilter != null)
                    foreach (var id in _selectionFilter.Values)
                    {
                        int productId = SQLDataHelper.GetInt(id);

                        if (removeFromCurrentCategories)
                        {
                            foreach (int catId in ProductService.GetCategoriesIDsByProductId(productId))
                                ProductService.DeleteProductLink(productId, catId);
                        }
                        ProductService.AddProductLink(productId, categoryId, 0, false);
                    }
            }
            else
            {
                foreach (int id in _paging.ItemsIds<int>("[Product].[ProductID] as ID").Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    if (removeFromCurrentCategories)
                    {
                        foreach (int catId in ProductService.GetCategoriesIDsByProductId(id))
                            ProductService.DeleteProductLink(id, catId);
                    }
                    ProductService.AddProductLink(id, categoryId, 0, false);
                }
            }

            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(categoryId);
            ProductService.PreCalcProductParamsMassInBackground();
            CategoryService.ClearCategoryCache();
        }

        public void PopulateNode2(object sender, TreeNodeEventArgs e)
        {
            LoadChildCategories2(e.Node);
        }

        public void OnSelectedNodeChanged2(object sender, EventArgs e)
        {
            hhl2.Text = tree2.SelectedNode.Value;
            mpeTree2.Show();
        }


        private void LoadChildCategories2(TreeNode node)
        {
            foreach (var c in CategoryService.GetChildCategoriesByCategoryId(SQLDataHelper.GetInt(node.Value), false))
            {
                var newNode = new TreeNode
                    {
                        Text = string.Format("{0} - ({1})", c.Name, c.ProductsCount),
                        Value = c.CategoryId.ToString()
                    };
                if (c.HasChild)
                {
                    newNode.Expanded = false;
                    newNode.PopulateOnDemand = true;
                }
                else
                {
                    newNode.Expanded = true;
                    newNode.PopulateOnDemand = false;
                }
                node.ChildNodes.Add(newNode);
            }
        }

        protected void lbChangeCategory_Click(object sender, EventArgs e)
        {
            mpeTree2.Show();
        }

        #endregion
    }
}