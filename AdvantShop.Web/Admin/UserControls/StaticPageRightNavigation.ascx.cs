//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.CMS;
using AdvantShop.Core.Controls;
using AdvantShop.Helpers;
using Resources;

namespace Admin.UserControls
{
    public partial class StaticPageRightNavigation : System.Web.UI.UserControl
    {
        protected int PageId
        {
            get
            {
                int id;
                int.TryParse(Request["pageid"], out id);
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ReloadTree();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            LoadChildStaticPages(e.Node);
        }
        private void LoadChildStaticPages(TreeNode node)
        {
            foreach (var page in StaticPageService.GetChildStaticPages(SQLDataHelper.GetInt(node.Value), false))
            {
                node.ChildNodes.Add(new ButtonTreeNodeStaticPage
                    {
                        Text = page.PageName,
                        MessageToDel =
                            Server.HtmlEncode(string.Format(
                                Resource.Admin_MasterPageAdminCatalog_Confirmation, page.PageName)),
                        Value = page.StaticPageId.ToString(),
                        TreeView = tree,
                        ShowButtons = TreeButtonStatus.None,
                        Expanded = !page.HasChildren,
                        PopulateOnDemand = page.HasChildren,
                        NavigateUrl = "~/Admin/StaticPage.aspx?PageID=" + page.StaticPageId
                    });
            }
        }
        protected void ReloadTree()
        {
            tree.Nodes.Clear();

            //tree.Nodes.Add(new ButtonTreeNodeStaticPage()
            //{
            //    Value = "0",
            //    Text = Resources.Resource.Admin_StaticPage_Root,
            //    NavigateUrl = "StaticPages.aspx",
            //    TreeView = tree,
            //    Selected = true
            //});

            foreach (StaticPage page in StaticPageService.GetRootStaticPages())
            {
                tree.Nodes.Add(new ButtonTreeNodeStaticPage
                    {
                        NavigateUrl = "~/Admin/StaticPage.aspx?PageID=" + page.StaticPageId,
                        Value = page.StaticPageId.ToString(),
                        Text = page.PageName,
                        PopulateOnDemand = page.HasChildren,
                        Expanded = !page.HasChildren,
                        Selected = page.StaticPageId == PageId,
                        TreeView = tree,
                        ShowButtons = TreeButtonStatus.None
                    });
            }

            var parentPageIDs = StaticPageService.GetParentStaticPages(PageId);
            var nodes = tree.Nodes;

            for (var i = parentPageIDs.Count - 1; i >= 0; i--)
            {
                var tn = (from TreeNode n in nodes where n.Value == parentPageIDs[i].ToString() select n).SingleOrDefault();

                if (i == 0)
                    tn.Select();
                tn.Expand();

                nodes = tn.ChildNodes;
            }
        }
    }
}