using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class CatalogLayout : System.Web.UI.MasterPage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ibRecalculate.Attributes.Add("onmouseover", "this.src=\'images/broundarrow.gif\';");
                tree.Nodes.Clear();
                LoadRootCategories(tree.Nodes);


                var categoryId = Request["categoryid"].TryParseInt();
                if (categoryId != 0)
                {
                    var parentCategories = CategoryService.GetParentCategories(categoryId);

                    var nodes = tree.Nodes;

                    if (categoryId == 0)
                    {
                        var node = (from TreeNode n in nodes select n).First();
                        node.Select();
                    }

                    for (var i = parentCategories.Count - 1; i >= 0; i--)
                    {
                        var ii = i;
                        var tn = (from TreeNode n in nodes where n.Value == parentCategories[ii].CategoryId.ToString() select n).SingleOrDefault();
                        if (i == 0)
                        {
                            tn.Select();
                            tn.Expand();
                        }
                        else
                        {
                            tn.Expand();
                        }

                        nodes = tn.ChildNodes;
                    }
                }
            }
        }

        protected void tree_TreeNodeCommand(object sender, CommandEventArgs e)
        {
            var needRedirect = false;
            try
            {
                if (e.CommandName.StartsWith("DeleteCategory"))
                {

                    int catId = 0;
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
                Debug.Log.Error(ex);
            }

            if (needRedirect)
            {
                Response.Redirect("Catalog.aspx");
            }
        }

        protected void ibRecalculate_Click(object sender, ImageClickEventArgs e)
        {
            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(0);
            ProductService.PreCalcProductParamsMassInBackground();
            CategoryService.ClearCategoryCache();
            tree.Nodes.Clear();
            LoadRootCategories(tree.Nodes);
        }

        private void LoadRootCategories(TreeNodeCollection treeNodeCollection)
        {
            var rootCategory = CategoryService.GetCategory(0);
            var newNode = new ButtonTreeNodeCatalog
            {
                Text = string.Format("{3}{0} ({1}/{2}){4}", rootCategory.Name, rootCategory.ProductsCount, rootCategory.TotalProductsCount,
                                     rootCategory.ProductsCount == 0 ? "<span class=\"lightlink\">" : string.Empty,
                                     rootCategory.ProductsCount == 0 ? "</span>" : string.Empty),
                Value = rootCategory.CategoryId.ToString(),
                NavigateUrl = "Catalog.aspx?CategoryID=" + rootCategory.CategoryId,
                TreeView = tree,
                Expanded = true,
                PopulateOnDemand = false
            };

            treeNodeCollection.Add(newNode);

            foreach (var c in CategoryService.GetChildCategoriesByCategoryId(0, false))
            {
                newNode = new ButtonTreeNodeCatalog
                {
                    Text = string.Format("{3}{0} ({1}/{2}){4}", c.Name, c.ProductsCount, c.TotalProductsCount,
                                         c.ProductsCount == 0 ? "<span class=\"lightlink\">" : string.Empty,
                                         c.ProductsCount == 0 ? "</span>" : string.Empty),
                    MessageToDel = Server.HtmlEncode(string.Format(Resource.Admin_MasterPageAdminCatalog_Confirmation, c.Name)),
                    Value = c.CategoryId.ToString(),
                    NavigateUrl = "Catalog.aspx?CategoryID=" + c.CategoryId,
                    TreeView = tree
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

                treeNodeCollection.Add(newNode);
            }
        }

        private void LoadChildCategories(TreeNode node)
        {
            foreach (var c in CategoryService.GetChildCategoriesByCategoryId(SQLDataHelper.GetInt(node.Value), false))
            {
                var newNode = new ButtonTreeNodeCatalog
                {
                    Text = string.Format("{3}{0} ({1}/{2}){4}", c.Name, c.ProductsCount, c.TotalProductsCount,
                                         c.ProductsCount == 0 ? "<span class=\"lightlink\">" : string.Empty,
                                         c.ProductsCount == 0 ? "</span>" : string.Empty),
                    MessageToDel =
                        Server.HtmlEncode(string.Format(
                            Resource.Admin_MasterPageAdminCatalog_Confirmation, c.Name)),
                    Value = c.CategoryId.ToString(),
                    NavigateUrl = "Catalog.aspx?CategoryID=" + c.CategoryId,
                    TreeView = tree
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

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            LoadChildCategories(e.Node);
        }

        protected string RenderSplitter()
        {
            var str = new StringBuilder();
            str.Append("<td class=\'splitter\'  onclick=\'togglePanel();return false;\' >");
            str.Append("<div class=\'leftPanelTop\'></div>");
            switch (Resource.Admin_Catalog_SplitterLang)
            {
                case "rus":
                    str.Append("<div id=\'divHide\' class=\'left_hide_rus\'></div>");
                    str.Append("<div id=\'divShow\' class=\'left_show_rus\'></div>");
                    break;
                case "eng":
                    str.Append("<div id=\'divHide\' class=\'left_hide_en\'></div>");
                    str.Append("<div id=\'divShow\' class=\'left_show_en\'></div>");
                    break;
            }
            str.Append("</td>");
            return str.ToString();
        }

        protected string RenderTotalProductLink()
        {
            var res = new StringBuilder();
            res.Append("<div>");
            res.Append(Resource.Admin_Catalog_AllProducts);
            res.Append(" (");
            res.Append(CategoryService.GetTolatCounTofProducts());
            res.Append(")");
            res.Append("</div>");

            return res.ToString();
        }

        protected string RenderTotalProductWithoutCategoriesLink()
        {
            var res = new StringBuilder();
            res.Append("<div>");
            res.Append(Resource.Admin_Catalog_AllProductsWithoutCategories);
            res.Append(" (");
            res.Append(CategoryService.GetTolatCounTofProductsWithoutCategories());
            res.Append(")");
            res.Append("</div>");

            return res.ToString();
        }

        protected string RenderTotalProductInCategoriesLink()
        {
            var res = new StringBuilder();
            res.Append("<div>");
            res.Append(Resource.Admin_Catalog_AllProductsInCategories);
            res.Append(" (");
            res.Append(CategoryService.GetTolatCounTofProductsInCategories());
            res.Append(")");
            res.Append("</div>");

            return res.ToString();
        }
    }
}
