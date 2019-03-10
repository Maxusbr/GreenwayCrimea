using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace Admin.UserControls.Products
{
    public partial class RightNavigation : UserControl
    {
        protected SqlPaging Paging;
        public int CategoryID { get; set; }

        public int ProductID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                Paging = (SqlPaging)(ViewState["Paging"]);
            }
            else
            {
                Paging = new SqlPaging
                    {
                        TableName = CategoryID == CategoryService.DefaultNonCategoryId ? "[Catalog].[GetProductsWithoutCategories]()" : string.Format("[Catalog].[CategoryContent](\'{0}\')", CategoryID),
                        ItemsPerPage = 18
                    };

                Paging.AddFieldsRange(new[]
                    {
                        new Field { Name = "ID", IsDistinct = true },
                        new Field { Name = "ItemType", Sorting = SortDirection.Ascending },
                        new Field { Name = "PhotoName" },
                        new Field { Name = "Name" },
                        new Field { Name = "sortOrder", Sorting = SortDirection.Ascending }
                    });

                int pageIndex = 1;

                if (!string.IsNullOrEmpty(Request["pn"]))
                {
                    int.TryParse(Request["pn"], out pageIndex);
                }

                Paging.CurrentPageIndex = pageIndex;
                if (CategoryID != CategoryService.DefaultNonCategoryId)
                {
                    //hlAddProduct.NavigateUrl = "~/admin/Product.aspx?CategoryId=" + CategoryID;
                    IList<Category> parentCategories = CategoryService.GetParentCategories(CategoryID);
                    if (parentCategories != null)
                    {
                        if (CategoryID == CategoryService.DefaultNonCategoryId)
                        {
                            ddlCategory.SelectedValue = CategoryService.DefaultNonCategoryId.ToString();
                        }
                        if (parentCategories.Count != 0)
                        {
                            ddlCategory.SelectedValue = parentCategories[parentCategories.Count - 1].CategoryId.ToString();
                        }
                    }
                }
                else
                {
                    ddlCategory.SelectedValue = CategoryService.DefaultNonCategoryId.ToString();
                }

                FillRootCategory();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            UpdateCategoryContentPanel();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            CategoryID = SQLDataHelper.GetInt(ddlCategory.SelectedValue);

            // if "products without category" was selected
            if (ddlCategory.SelectedValue == CategoryService.DefaultNonCategoryId.ToString())
            {
                Paging.TableName = "[Catalog].[GetProductsWithoutCategories]()";
                //hlAddProduct.NavigateUrl = "~/admin/Product.aspx?CategoryId=" + "0";
            }
            else
            {
                Paging.TableName = "[Catalog].[CategoryContent](\'" + CategoryID + "\')";
                //hlAddProduct.NavigateUrl = "~/admin/Product.aspx?CategoryId=" + CategoryID;
            }

            Paging.CurrentPageIndex = 1;
        }
        protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Paging.CurrentPageIndex = SQLDataHelper.GetInt(ddlCurrentPage.SelectedValue);
        }

        private void UpdateCategoryContentPanel()
        {
            rCategoryContent.DataSource = Paging.PageItems;
            rCategoryContent.DataBind();
            if (Paging.CurrentPageIndex > 1)
            {
                lbPreviousPage.Enabled = true;
            }
            var pageCount = Paging.PageCount;
            if (pageCount > 1 && Paging.CurrentPageIndex < pageCount)
            {
                lbNextPage.Enabled = true;
            }

            ddlCurrentPage.Items.Clear();

            for (int i = 1; i <= pageCount; i++)
            {
                var itm = new ListItem(i.ToString(), i.ToString());
                if (i == Paging.CurrentPageIndex)
                {
                    itm.Selected = true;
                }
                ddlCurrentPage.Items.Add(itm);
            }
            ViewState["Paging"] = Paging;

        }

        protected void lbNextPage_Click(object sender, EventArgs e)
        {
            Paging.CurrentPageIndex++;
        }

        protected void lbPreviousPage_Click(object sender, EventArgs e)
        {
            Paging.CurrentPageIndex--;
        }

        protected void rCategoryContent_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                CategoryID = SQLDataHelper.GetInt(e.CommandArgument);
                Paging.TableName = string.Format("[Catalog].[CategoryContent](\'{0}\')", CategoryID);
                Paging.CurrentPageIndex = 1;
                //if (CategoryID != 0)
                //{
                //    hlAddProduct.NavigateUrl = "~/admin/Product.aspx?CategoryId=" + CategoryID;
                //}
            }

            if (e.CommandName.Equals("DeleteCategory"))
            {
                var parentCategory = (CategoryService.GetCategory(SQLDataHelper.GetInt(e.CommandArgument))).ParentCategoryId;
                CategoryService.DeleteCategoryAndPhotos(SQLDataHelper.GetInt(e.CommandArgument));
                if (Request["categoryid"] == e.CommandArgument.ToString())
                {
                    RedirectFromUpdatePanel(string.Format("catalog.aspx?CategoryId={0}", parentCategory));
                }
            }

            if (e.CommandName.Equals("DeleteProduct"))
            {
                ProductService.DeleteProduct(SQLDataHelper.GetInt(e.CommandArgument), true);
                if (Request["productid"] == e.CommandArgument.ToString())
                {
                    RedirectFromUpdatePanel(string.Format("catalog.aspx?CategoryId={0}", CategoryID));
                }
            }

            if (e.CommandName.Equals("DeleteFromCategory"))
            {
                ProductService.EnableDynamicProductLinkRecalc();
                ProductService.DeleteProductLink(SQLDataHelper.GetInt(e.CommandArgument), CategoryID);
                ProductService.DisableDynamicProductLinkRecalc();
                if (Request["productid"] == e.CommandArgument.ToString())
                {
                    RedirectFromUpdatePanel(string.Format("Product.aspx?ProductID={0}", Request["productid"]));
                }

            }
        }
        protected void FillRootCategory()
        {
            ddlCategory.DataSource = CategoryService.GetChildCategoriesAndNonCategory(0);
            ddlCategory.DataBind();
        }

        public void RedirectFromUpdatePanel(string url)
        {
            string redirectUrl = Page.ResolveClientUrl(url);
            string script = "window.location = '" + redirectUrl + "';";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "RedirectFromUpdatePanel", script, true);
        }
    }
}