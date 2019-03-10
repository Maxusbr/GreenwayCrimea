//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Admin.UserControls.Products;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.SEO;
using AdvantShop.Trial;
using Resources;
using System.Web;
using AdvantShop.Core.SQL;
using AdvantShop.Saas;
using AdvantShop.Taxes;

namespace Admin
{
    public partial class ProductPage : AdvantShopAdminPage
    {
        protected Product _product;
        private string _productPhoto;

        protected int ProductId
        {
            get
            {
                int id;
                int.TryParse(Request["productid"], out id);
                return id;
            }
        }

        private int CategoryID
        {
            get
            {
                var id = CategoryService.DefaultNonCategoryId;
                if (!int.TryParse(Request["categoryid"], out id))
                {
                    id = ProductService.GetFirstCategoryIdByProductId(ProductId);
                }
                return id;
            }
        }

        protected bool AddingNewProduct
        {
            //get { return (ProductId == 0); }
            get { return string.IsNullOrEmpty(Request["productid"]); }
        }

        private List<ProductAdminControl> _moduleAdminControls;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (ProductId != 0)
                LoadModuleControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["categoryid"]) && string.IsNullOrEmpty(Request["productid"]))
            {
                Page.Response.Redirect("Catalog.aspx");
            }

            trTags.Visible = !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveTags;
            btnSave.Text = AddingNewProduct ? Resource.Admin_Product_AddProduct : Resource.Admin_Product_Save;
            btnCopyProduct.Visible = !AddingNewProduct;


            relatedProducts.ProductID
                = alternativeProducts.ProductID
                = gifts.ProductID
                = productPhotos.ProductID
                = productPhotos360.ProductID
                = productVideos.ProductID
                = productCustomOption.ProductId
                = productProperties.ProductId
                = rightNavigation.ProductID
                = landingPage.ProductID
                = ProductId;

            productProperties.CategoryId = CategoryID;
            rightNavigation.CategoryID = CategoryID;

            lRelatedProduct.Text = SettingsCatalog.RelatedProductName;
            lAlternativeProduct.Text = SettingsCatalog.AlternativeProductName;


            if (!IsPostBack)
            {
                ddlCurrecy.Items.AddRange(CurrencyService.GetAllCurrencies().Select(c => new ListItem(c.Name, c.CurrencyId.ToString())).ToArray());

                var taxes = new List<ListItem>();
                taxes.AddRange(TaxService.GetTaxes().Where(x => x.Enabled).Select(x => new ListItem(x.Name, x.TaxId.ToString())));

                ddlTax.DataSource = taxes;
                ddlTax.DataBind();

                if (!AddingNewProduct)
                {
                    _product = ProductService.GetProduct(ProductId);
                    if (_product == null)
                    {
                        Response.Redirect("Catalog.aspx");
                        return;
                    }
                    SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, _product.Name));

                    addProduct.CategoryId = _product.CategoryId;

                    addProductCopy.ProductName = productPhotos.ProductName = _product.Name;
                    addProductCopy.ProductUrl = UrlService.GetAvailableValidUrl(0, ParamType.Product, _product.UrlPath);

                    LoadProduct(_product);
                }
                else
                {
                    _product = new Product
                    {
                        Name = Resource.Admin_Product_AddNewProduct,
                        Offers = new List<Offer> { new Offer() }
                    };
                    txtTitle.Text = string.Empty;
                    txtMetaKeywords.Text = string.Empty;
                    txtMetaDescription.Text = string.Empty;

                    SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Product_AddNewProduct));

                    LoadProduct(_product);
                }
                txtName.Focus();
            }

            productOffers.ArtNo = txtStockNumber.Text;

            UpdateMainPhoto();
            LoadSiteNavigation();

            if (SettingsMain.EnableCyrillicUrl)
            {
                reValidatorUrl.ValidationExpression = "^[a-zA-Zа-€ј-я0-9_-]*$";
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var brand = popUpBrand.SelectBrandId != 0 ? BrandService.GetBrandById(popUpBrand.SelectBrandId) : null;
            lBrand.Text = brand != null ? brand.Name : Resource.Admin_Product_NotSelected;
            ibRemoveBrand.Visible = popUpBrand.SelectBrandId != 0;

            if (!AddingNewProduct)
            {
                aToClient.HRef = "../" + UrlService.GetLinkDB(ParamType.Product, ProductId);
                aToClient.Visible = true;
            }
            else
            {
                aToClient.Visible = false;
            }
        }

        private void LoadSiteNavigation()
        {
            if (CategoryID == CategoryService.DefaultNonCategoryId)
            {
                sn.Visible = false;
                Localize_Admin_Catalog_CategoryLocation.Visible = false;
            }
            else
            {
                sn.Visible = true;
                Localize_Admin_Catalog_CategoryLocation.Visible = true;
                sn.BuildNavigationAdmin(CategoryID);
            }
        }

        private void LoadProduct(Product product)
        {
            SetMeta(GetPageTitle());

            lProductName.Text = HttpUtility.HtmlEncode(product.Name);

            if (product.ProductId != 0)
            {
                rbActiveProduct.Attributes.Add("onclick", string.Format("setProductEnabled('1',{0} )", _product.ProductId));
                rbNotActiveProduct.Attributes.Add("onclick", string.Format("setProductEnabled('0',{0} )", _product.ProductId));
                rbActiveProduct.Checked = _product.Enabled;
                rbNotActiveProduct.Checked = !_product.Enabled;

                lblProductId.Text = product.ProductId.ToString();
                txtStockNumber.Text = product.ArtNo;
                txtName.Text = product.Name;
                txtSynonym.Text = product.UrlPath;
                chkAllowPreOrder.Checked = product.AllowPreOrder;
                txtWeight.Text = product.Weight.ToString();

                lbTag.Items.Clear();
                foreach (var tag in product.Tags)
                {
                    var item = new ListItem(tag.Name, tag.Name);
                    item.Selected = true;
                    lbTag.Items.Add(item);
                }

                txtBarCode.Text = product.BarCode;
                txtSizeLength.Text = product.Length.ToString("F3");
                txtSizeWidth.Text = product.Width.ToString("F3");
                txtSizeHeight.Text = product.Height.ToString("F3");

                popUpBrand.SelectBrandId = product.BrandId;
                lBrand.Text = product.BrandId == 0 ? Resource.Admin_Product_NotSelected : product.Brand.Name;

                chkBestseller.Checked = product.BestSeller;
                chkRecommended.Checked = product.Recomended;
                chkNew.Checked = product.New;
                chkOnSale.Checked = product.OnSale;
                var flagEnabled = product.CategoryEnabled;
                chkBestseller.Enabled = flagEnabled;
                chkRecommended.Enabled = flagEnabled;
                chkNew.Enabled = flagEnabled;
                chkOnSale.Enabled = flagEnabled;
                lblMarkersDisabled.Visible = !flagEnabled;

                productPhotos360.ActiveView360 = product.ActiveView360;

                txtShippingPrice.Text = product.ShippingPrice == null ? "" : product.ShippingPrice.Value.ToString("#0.00");
                txtUnit.Text = product.Unit;

                txtMaxAmount.Text = product.MaxAmount.ToString();
                txtMinAmount.Text = product.MinAmount.ToString();
                txtMultiplicity.Text = product.Multiplicity.ToString();
                txtSalesNote.Text = product.SalesNote;
                txtGtin.Text = product.Gtin;
                txtGoogleProductCategory.Text = product.GoogleProductCategory;
                txtYandexMarketCategory.Text = product.YandexMarketCategory;

                txtYandexTypePrefix.Text = product.YandexTypePrefix;
                txtYandexModel.Text = product.YandexModel;
                ddlYandexSizeUnit.SelectedValue = product.YandexSizeUnit;

                txtCbid.Text = product.Cbid.ToString();
                txtFee.Text = product.Fee.ToString();

                chbAdult.Checked = product.Adult;
                chbManufacturerWarranty.Checked = product.ManufacturerWarranty;

                txtDiscount.Text = product.Discount.Percent.ToString();
                txtDiscountAmount.Text = product.Discount.Amount.ToString();
                fckDescription.Text = product.Description;
                fckBriefDescription.Text = product.BriefDescription;

                ddlCurrecy.SelectedValue = product.CurrencyID.ToString();

                if (product.TaxId != null && ddlTax.Items.FindByValue(product.TaxId.Value.ToString()) != null)
                    ddlTax.SelectedValue = product.TaxId.Value.ToString();

                productOffers.Offers = product.Offers;
                productOffers.HasMultiOffer = product.HasMultiOffer;
                productOffers.ProductID = ProductId;
                productOffers.ArtNo = product.ArtNo;

                var meta = MetaInfoService.GetMetaInfo(product.ProductId, MetaType.Product);
                if (meta == null)
                {
                    _product.Meta = new MetaInfo(0, 0, MetaType.Product, string.Empty, string.Empty, string.Empty, string.Empty);
                    chbDefaultMeta.Checked = true;
                }
                else
                {
                    chbDefaultMeta.Checked = false;
                    _product.Meta = meta;
                    txtTitle.Text = _product.Meta.Title;
                    txtMetaKeywords.Text = _product.Meta.MetaKeywords;
                    txtMetaDescription.Text = _product.Meta.MetaDescription;
                    txtH1.Text = _product.Meta.H1;
                }

                //ddlCustomView.SelectedValue = ddlCustomView.Items.FindByValue(product.CustomViewName) != null ? product.CustomViewName : string.Empty;

                LoadCategoryTree();

                int reviewsCount = ReviewService.GetReviewsCount(product.ProductId, EntityType.Product);
                lblReviewsCount.Text = reviewsCount.ToString();
                hlReviews.NavigateUrl = UrlService.GetAdminAbsoluteLink(string.Format("Reviews.aspx?{0}={1}",
                    reviewsCount > 0 ? "ArtNo" : "AddReview", HttpUtility.UrlEncode(product.ArtNo)));
                hlReviews.Text = reviewsCount > 0 ? Resource.Admin_Product_ViewReviews : Resource.Admin_Product_AddReview;
                lblRewReq.Visible = !(reviewsCount > 0);

                try
                {
                    var module = AttachedModules.GetModuleById("MoySklad");
                    if (module != null)
                    {
                        trMs.Visible = true;
                        lblExternMsCode.Text =
                            SQLDataAccess.Query<string>(
                                "SELECT ProductExternalId FROM [Catalog].[ProductFromMoysklad] WHERE ProductId=@id",
                                new { id = product.ProductId }).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }
            else
            {
                productOffers.HasMultiOffer = false;
                productOffers.ProductID = 0;

                trReviews.Visible = false;
            }
        }

        protected string GetPageTitle()
        {
            return string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Product_SubHeader);
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }

        private void Msg(string messageText)
        {
            lMessage.Visible = true;
            lMessage.Text = messageText;
        }

        #region Photos
        protected string HtmlProductImage()
        {
            return string.IsNullOrEmpty(_productPhoto)
                       ? "<img style=\'border: solid 1px gray;\' src=\'images/nophoto.gif\' class='img-adaptive' />"
                              //: (_productPhoto.Contains("://")
                              //       ? "<a rel=\"lightbox\" href=\"" + _productPhoto +
                              //         "\"><img class='img-adaptive' style=\'border: solid 1px gray;\' width=\'120\' src=\'" +
                              //         _productPhoto + "\' /></a>"
                              : "<a rel=\"lightbox\" href=\"" +
                                FoldersHelper.GetImageProductPath(ProductImageType.Big, _productPhoto, true) +
                                "\"><img  class='img-adaptive' style=\'border: solid 1px gray;\' src=\'" +
                                FoldersHelper.GetImageProductPath(ProductImageType.Small, _productPhoto, true) + "\' /></a>";
        }
        protected void productPhotos_OnPhotoMessage(object sender, ProductPhotos.PhotoMessageEventArgs e)
        {
            Msg(e.Message);
        }

        protected void productPhotos_OnMainPhotoUpdate(object sender, EventArgs e)
        {
            UpdateMainPhoto();
        }

        protected void UpdateMainPhoto()
        {
            var product = ProductService.GetProduct(ProductId);
            _productPhoto = product == null ? null : product.Photo;
            ltPhoto.Text = HtmlProductImage();
            upPhoto.Update();
        }
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            productOffers.ArtNo = txtStockNumber.Text;
            if (!productOffers.RefreshOffers())
                return;

            string redir = null;
            txtShippingPrice.Text = txtShippingPrice.Text.Replace(" ", string.Empty);

            if (AddingNewProduct)
            {
                var id = CreateProduct();
                var catId = 0;
                if (id != 0 && int.TryParse(Request["categoryid"], out catId) && catId > 0)
                {
                    if (CategoryService.IsExistCategory(catId))
                    {
                        ProductService.EnableDynamicProductLinkRecalc();
                        ProductService.AddProductLink(id, catId, 0, true);
                        ProductService.DisableDynamicProductLinkRecalc();
                        ProductService.SetProductHierarchicallyEnabled(id);
                    }
                }

                redir = id == 0 ? null : string.Format("Product.aspx?ProductID={0}{1}", id, catId == 0 ? "" : "&CategoryID=" + Request["categoryid"]);
            }
            else
            {
                if (string.IsNullOrEmpty(txtStockNumber.Text.Trim()))
                {
                    lStockNumberError.Text = Resource.Admin_Product_ArtNoEmpty;
                    return;
                }
                if (UpdateProduct())
                {
                    if (ProductId != 0)
                    {
                        SaveModuleControls(ProductId);
                    }
                    //Response.Redirect(Request.RawUrl + "#tabid=" + tabid.Value);
                }
            }

            if (!string.IsNullOrEmpty(redir))
            {
                //Response.Redirect(redir);
            }
        }

        private int CreateProduct()
        {
            var id = 0;
            var artNo = txtStockNumber.Text;
            var validArtNo = true;
            var isValidPage = true;

            try
            {
                // провер€ем свободен ли артикул
                if (ProductService.GetProductId(artNo) != 0)
                {
                    validArtNo = false;
                    Msg(Resource.Admin_Product_Duplicate);
                }

                Validate();
                //провер€ем свободен ли урл
                if (!UrlService.IsAvailableUrl(ParamType.Product, txtSynonym.Text))
                {
                    validArtNo = false;
                    Msg(Resource.Admin_SynonymExist);
                }

                imgWarningTab1.Visible = !IsValidTab(1) || !validArtNo;
                imgWarningTab3.Visible = !IsValidTab(2);

                isValidPage &= !imgWarningTab1.Visible && !imgWarningTab3.Visible;

                if (isValidPage)
                {
                    ProductService.EnableDynamicProductLinkRecalc();
                    var productToCreate = GetProductFromForm();
                    if (productToCreate == null)
                        return 0;

                    id = ProductService.AddProduct(productToCreate, true);
                    ProductService.DisableDynamicProductLinkRecalc();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                Msg("Erorr at create product");
                return 0;
            }
            TrialService.TrackEvent(TrialEvents.AddProduct, "");
            return id;
        }

        private bool UpdateProduct()
        {
            var validArtNo = true;
            var isValidPage = true;
            var artNo = txtStockNumber.Text;
            try
            {
                //провер€ем свободен ли артикул
                var tempId = ProductService.GetProductId(artNo);
                if (tempId != 0 && tempId != ProductId)
                {
                    validArtNo = false;
                    Msg(Resource.Admin_Product_Duplicate);
                }
                // почему артикул продукта не может совпадать с артикулом офера у другого продукта
                //var offer = OfferService.GetOffer(artNo);
                //if (offer != null && offer.ProductId != ProductId)
                //{
                //    validArtNo = false;
                //    Msg(Resource.Admin_Product_Duplicate);
                //}

                Validate();

                if (IsValidTab(1))
                {
                    var synonym = txtSynonym.Text;
                    if (!string.IsNullOrEmpty(synonym))
                    {
                        if (!UrlService.IsAvailableUrl(ProductId, ParamType.Product, synonym))
                        {
                            Msg(Resource.Admin_SynonymExist);
                            return false;
                        }
                    }
                }

                imgWarningTab1.Visible = !IsValidTab(1) || !validArtNo;
                imgWarningTab3.Visible = !IsValidTab(2);

                isValidPage &= !imgWarningTab1.Visible && !imgWarningTab3.Visible;

                if (isValidPage)
                {
                    ProductService.EnableDynamicProductLinkRecalc();
                    var productToUpdate = GetProductFromForm();
                    if (productToUpdate == null)
                        return false;

                    ProductService.UpdateProduct(productToUpdate, true);
                    ProductService.DisableDynamicProductLinkRecalc();

                    imgExcl6.Visible = !productCustomOption.SaveCustomOption();
                    landingPage.Save();
                    productProperties.SaveProperties();
                    _product = ProductService.GetProduct(ProductId);

                    LoadProduct(_product);
                    UpdateMainPhoto();
                }
                else
                {
                    return false;
                }

                LoadSiteNavigation();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
            return true;
        }

        private Product GetProductFromForm()
        {
            _product = AddingNewProduct
                           ? new Product { Meta = new MetaInfo(), Offers = new List<Offer>() }
                           : ProductService.GetProduct(ProductId);
            if (_product == null)
                return null;
            _product.ArtNo = txtStockNumber.Text;
            _product.Name = txtName.Text;
            _product.UrlPath = txtSynonym.Text;
            _product.BriefDescription = fckBriefDescription.Text == "<br />" || fckBriefDescription.Text == "&nbsp;" || fckBriefDescription.Text == "\r\n"
                                            ? string.Empty
                                            : fckBriefDescription.Text;

            _product.Description = fckDescription.Text == "<br />" || fckDescription.Text == "&nbsp;" || fckDescription.Text == "\r\n"
                                       ? string.Empty
                                       : fckDescription.Text;

            _product.Weight = txtWeight.Text.TryParseFloat();
            _product.Length = txtSizeLength.Text.TryParseFloat();
            _product.Width = txtSizeWidth.Text.TryParseFloat();
            _product.Height = txtSizeHeight.Text.TryParseFloat();
            _product.Discount = new Discount(txtDiscount.Text.TryParseFloat(), txtDiscountAmount.Text.TryParseFloat());
            _product.Enabled = rbActiveProduct.Checked;// chkEnabled.Checked;
            _product.AllowPreOrder = chkAllowPreOrder.Checked;
            _product.BarCode = txtBarCode.Text;
            var tagsTitle = Request.Params[lbTag.UniqueID]; //lbTag.Items.AsQueryable().Where<ListItem>(x => x.Selected).ToList();
            if (tagsTitle.IsNotEmpty())
            {
                var titles = tagsTitle.Split(',');
                _product.Tags = titles.Select(x => new Tag
                {
                    Name = x,
                    UrlPath = StringHelper.TransformUrl(StringHelper.Translit(x)),
                    Enabled = true,
                    VisibilityForUsers = true
                }).ToList();
            }
            else
            {
                _product.Tags = new List<Tag>();
            }

            _product.BestSeller = chkBestseller.Checked;
            _product.Recomended = chkRecommended.Checked;
            _product.New = chkNew.Checked;
            _product.OnSale = chkOnSale.Checked;
            _product.BrandId = popUpBrand.SelectBrandId;
            _product.SalesNote = txtSalesNote.Text;

            _product.Gtin = txtGtin.Text;
            _product.GoogleProductCategory = txtGoogleProductCategory.Text;
            _product.YandexMarketCategory = txtYandexMarketCategory.Text;

            _product.YandexTypePrefix = txtYandexTypePrefix.Text;
            _product.YandexModel = txtYandexModel.Text;

            _product.YandexSizeUnit = ddlYandexSizeUnit.SelectedValue;

            _product.Cbid = txtCbid.Text.IsNotEmpty() ? txtCbid.Text.TryParseFloat() : 0;
            _product.Fee = txtFee.Text.IsNotEmpty() ? txtFee.Text.TryParseFloat() : 0;

            _product.Adult = chbAdult.Checked;
            _product.ManufacturerWarranty = chbManufacturerWarranty.Checked;

            _product.ActiveView360 = productPhotos360.ActiveView360;

            _product.ShippingPrice = txtShippingPrice.Text.TryParseFloat(true);
            _product.Unit = txtUnit.Text;

            _product.Multiplicity = txtMultiplicity.Text.TryParseFloat();
            _product.MaxAmount = txtMaxAmount.Text.IsNotEmpty() ? txtMaxAmount.Text.TryParseFloat() : (float?)null;
            _product.MinAmount = txtMinAmount.Text.IsNotEmpty() ? txtMinAmount.Text.TryParseFloat() : (float?)null;

            _product.CurrencyID = ddlCurrecy.SelectedValue.TryParseInt();
            _product.TaxId = ddlTax.SelectedValue.TryParseInt(true);
            _product.Offers = productOffers.Offers;
            _product.HasMultiOffer = productOffers.HasMultiOffer;

            _product.Meta.Title = txtTitle.Text;
            _product.Meta.H1 = txtH1.Text;
            _product.Meta.MetaDescription = txtMetaDescription.Text;
            _product.Meta.MetaKeywords = txtMetaKeywords.Text;
            _product.Meta.Type = MetaType.Product;
            _product.Meta.ObjId = _product.ProductId;

            _product.ModifiedBy = CustomerContext.CustomerId.ToString();
            //_product.CustomViewName = ddlCustomView.SelectedValue.IsNotEmpty() ? ddlCustomView.SelectedValue : null;

            return _product;
        }

        protected bool IsValidTab(int tab)
        {
            return
                (from BaseValidator v in Validators
                 where v.ValidationGroup.Equals(tab.ToString()) && !v.IsValid
                 select v).ToArray().Length == 0;
        }


        #region CategoryTree

        protected void LoadCategoryTree()
        {
            if (!IsPostBack)
            {
                var node = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = @"0", Selected = true, SelectAction = TreeNodeSelectAction.None };
                LinksProductTree.Nodes.Add(node);
                LoadChildCategories(LinksProductTree.Nodes[0]);
                FillListBox();
            }
        }

        protected void btnDelLink_Click(object sender, EventArgs e)
        {
            int categoryId;
            if (!string.IsNullOrEmpty(ListlinkBox.SelectedValue) && int.TryParse(ListlinkBox.SelectedValue, out categoryId))
            {
                ProductService.DeleteProductLink(ProductId, categoryId);
                CategoryService.RecalculateProductsCountManual();
                FillListBox();
            }
        }
        protected void lnAddLink_Click(object sender, EventArgs e)
        {
            if ((LinksProductTree.SelectedValue != null) && LinksProductTree.SelectedValue != "0" && LinksProductTree.SelectedValue.IsInt())
            {
                int temp;
                Int32.TryParse(LinksProductTree.SelectedValue, out temp);
                if (temp != 0)
                {
                    ProductService.EnableDynamicProductLinkRecalc();
                    ProductService.AddProductLink(ProductId, temp, 0, true);
                    ProductService.DisableDynamicProductLinkRecalc();
                    ProductService.SetProductHierarchicallyEnabled(ProductId);
                }
                FillListBox();
            }
        }
        public void PopulateNode(object sender, TreeNodeEventArgs e)
        {
            LoadChildCategories(e.Node);
        }

        private void LoadChildCategories(TreeNode node)
        {
            foreach (Category c in CategoryService.GetChildCategoriesByCategoryId(Convert.ToInt32(node.Value), false))
            {
                if (c.CategoryId != Convert.ToInt32(Request["id"]))
                {
                    var newNode = new TreeNode { Text = string.Format("{0}", c.Name), Value = c.CategoryId.ToString() };
                    if (c.HasChild)
                    {
                        newNode.Expanded = false;
                        newNode.PopulateOnDemand = true;
                        //newNode.ShowCheckBox = true;
                        //newNode.NavigateUrl = "javascript:void(0)";
                    }
                    else
                    {
                        newNode.Expanded = true;
                        newNode.PopulateOnDemand = false;

                        //newNode.ShowCheckBox = true;
                        //newNode.NavigateUrl = "javascript:void(0)";
                    }
                    node.ChildNodes.Add(newNode);
                }
            }
        }
        public void FillListBox()
        {
            ListlinkBox.Items.Clear();
            try
            {
                foreach (var catId in ProductService.GetCategoriesIDsByProductId(ProductId))
                {
                    var item = new ListItem();

                    IList<Category> parentCategories = CategoryService.GetParentCategories(catId);

                    var way = new StringBuilder();
                    for (int i = parentCategories.Count - 1; i >= 0; i--)
                    {
                        if (way.Length == 0)
                        {
                            way.Append(parentCategories[i].Name);
                        }
                        else
                        {
                            way.Append(" > " + parentCategories[i].Name);
                        }
                    }
                    if (ProductService.IsMainLink(ProductId, catId))
                        way.AppendMany(" (", Resource.Admin_Product_MainCategory, ")");
                    item.Text = way.ToString();
                    item.Value = catId.ToString();
                    ListlinkBox.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                //Debug.Log.Error(ex, ProductId);
                Debug.Log.Error(ex);
            }
        }
        #endregion

        protected void btnMainLink_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ListlinkBox.SelectedValue)) return;
            ProductService.SetMainLink(ProductId, Convert.ToInt32(ListlinkBox.SelectedValue));
            ProductService.PreCalcProductParams(ProductId);
            ProductService.SetProductHierarchicallyEnabled(ProductId);

            FillListBox();
        }

        protected void ibRemoveBrand_Click(object sender, EventArgs e)
        {
            popUpBrand.SelectBrandId = 0;
            ibRemoveBrand.Visible = false;

            if (Request["ProductID"].TryParseInt() != 0)
            {
                ProductService.DeleteBrand(Request["ProductID"].TryParseInt());
            }
        }

        #region Modules

        private void LoadModuleControls()
        {
            _moduleAdminControls = new List<ProductAdminControl>();

            foreach (var detailsTabsModule in AttachedModules.GetModules<IProductAdminControls>())
            {
                var classInstance = (IProductAdminControls)Activator.CreateInstance(detailsTabsModule, null);
                if (ModulesRepository.IsActiveModule(classInstance.ModuleStringId) && classInstance.CheckAlive())
                {
                    _moduleAdminControls.AddRange(classInstance.GetProductAdminControls(this));
                }
            }

            foreach (var paCtrl in _moduleAdminControls)
            {
                if (paCtrl.Control != null)
                {
                    var div = new Panel() { CssClass = "tab-content" };
                    div.Controls.Add(paCtrl.Control);
                    pnlModuleControls.Controls.Add(div);
                }
            }

            lvModuleTabs.DataSource = _moduleAdminControls;
            lvModuleTabs.DataBind();
        }

        private void SaveModuleControls(int productId)
        {
            foreach (var paCtrl in _moduleAdminControls)
            {
                if (paCtrl.Control == null)
                    continue;
                paCtrl.Control.Save(productId);
            }
        }

        #endregion
    }
}