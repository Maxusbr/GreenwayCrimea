//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Controls;
using AdvantShop.Helpers;
using Resources;

namespace Admin.UserControls
{
    public partial class PopupTreeView : UserControl
    {
        public enum eTreeType
        {
            None = 0,
            StaticPage,
            Category,
            CategoryProduct,
            MainMenu,
            CategoryMultiSelect,
            CategoryOffer
        }

        public class TreeNodeSelectedArgs : EventArgs
        {
            public List<string> SelectedValues = new List<string>();
            public List<string> SelectedTexts = new List<string>();
        }


        public List<int> SelectedCategoriesIds { set; get; }
        public bool CheckChildrenNodes = false;
        private eTreeType _type = eTreeType.None;
        public string HeaderText { get; set; }
        public eTreeType Type { get { return _type; } set { _type = value; } }

        public int ExceptId
        {
            get { return (int) (ViewState["ExceptId"] ?? 0); }
            set { ViewState["ExceptId"] = value; }
        }

        protected List<int> AnotherExceptIds
        {
            get { return (List<int>) ViewState["AnotherIds"]; }
            set { ViewState["AnotherIds"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Type == eTreeType.CategoryProduct || Type == eTreeType.CategoryOffer)
                tree.SelectedNodeChanged -= tree_NodeSelected;
            else
                tree.SelectedNodeChanged += tree_NodeSelected;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && tree.Nodes.Count == 0)
                UpdateTree(AnotherExceptIds);
            btnOk.Visible = Type == eTreeType.CategoryProduct || Type == eTreeType.CategoryMultiSelect || Type == eTreeType.CategoryOffer;
            btnCancel.Visible = Type != eTreeType.CategoryMultiSelect;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (CheckChildrenNodes)
            {
                tree.Attributes.Add("onClick", "TreeViewCheckBoxClicked(event)");
            }
        }

        protected void LoadRoot()
        {
            tree.Nodes.Clear();
            switch (Type)
            {
                case eTreeType.MainMenu:
                    tree.Nodes.Add(new TreeNode { Text = Resource.Admin_m_Category_Root, Value = "0", Selected = true });
                    LoadChildMenuItems(tree.Nodes[0]);

                    var nodesMenuItems = tree.Nodes[0].ChildNodes;
                    foreach (var parentMenuItem in MenuService.GetParentMenuItems(SQLDataHelper.GetInt(Page.Request["menuid"])))
                    {
                        var tn =
                            (from TreeNode n in nodesMenuItems where n.Value == parentMenuItem.ToString() select n).SingleOrDefault();
                        if (tn != null)
                        {
                            tn.Select();
                            tn.Expand();
                        }
                        else
                        {
                            break;
                        }
                        nodesMenuItems = tn.ChildNodes;
                    }
                    return;
                case eTreeType.Category:
                    tree.Nodes.Add(new TreeNode { Text = Resource.Admin_m_Category_Root, Value = "0", Selected = true });
                    LoadChildCategories(tree.Nodes[0]);
                    var parentCategories = CategoryService.GetParentCategories(SQLDataHelper.GetInt(Page.Request["categoryid"]));

                    if (parentCategories != null)
                    {
                        var nodes = tree.Nodes[0].ChildNodes;
                        for (int i = parentCategories.Count - 1; i >= 0; i--)
                        {
                            int ii = i;
                            var tn =
                                (from TreeNode n in nodes where n.Value == parentCategories[ii].CategoryId.ToString() select n).SingleOrDefault();
                            if (tn != null)
                            {
                                tn.Select();
                                tn.Expand();
                            }
                            else
                            {
                                break;
                            }
                            nodes = tn.ChildNodes;
                        }
                    }
                    return;
                case eTreeType.StaticPage:
                    tree.Nodes.Add(new TreeNode(Resource.Admin_StaticPage_Root, "0"));
                    foreach (var node in StaticPageService.GetRootStaticPages().Where(page => page.StaticPageId != ExceptId).Select(page => new TreeNode { Value = page.StaticPageId.ToString(), Text = page.PageName, PopulateOnDemand = page.HasChildren }))
                    {
                        //LoadChildStaticPages(node);
                        tree.Nodes.Add(node);
                    }
                    return;
                case eTreeType.CategoryProduct:

                    tree.Nodes.Add(new TreeNode { Text = Resource.Admin_m_Category_Root, Value = "0" });
                    tree.ShowCheckBoxes = TreeNodeTypes.Leaf;
                    LoadChildCategoriesAndProducts(tree.Nodes[0]);
                    return;

                case eTreeType.CategoryOffer:

                    tree.Nodes.Add(new TreeNode { Text = Resource.Admin_m_Category_Root, Value = "0" });
                    tree.ShowCheckBoxes = TreeNodeTypes.Leaf;
                    LoadChildCategoriesAndOffers(tree.Nodes[0]);
                    return;

                case eTreeType.CategoryMultiSelect:
                    tree.Nodes.Add(new TreeNode { Text = Resource.Admin_m_Category_Root, Value = "0", ShowCheckBox = false });
                    tree.ShowCheckBoxes = TreeNodeTypes.All;
                    LoadChildCategoriesMultiSelect(tree.Nodes[0]);
                    return;
            }
        }

        public void PopulateNode(object sender, TreeNodeEventArgs e)
        {
            switch (Type)
            {
                case eTreeType.Category:
                    LoadChildCategories(e.Node);
                    break;
                case eTreeType.CategoryMultiSelect:
                    LoadChildCategoriesMultiSelect(e.Node);
                    break;
                case eTreeType.StaticPage:
                    LoadChildStaticPages(e.Node);
                    break;
                case eTreeType.CategoryProduct:
                    LoadChildCategoriesAndProducts(e.Node);
                    break;
                case eTreeType.CategoryOffer:
                    LoadChildCategoriesAndOffers(e.Node);
                    break;
                case eTreeType.MainMenu:
                    LoadChildMenuItems(e.Node);
                    break;
            }
        }

        private void LoadChildStaticPages(TreeNode node)
        {
            foreach (var childNode in
                from page in StaticPageService.GetChildStaticPages(SQLDataHelper.GetInt(node.Value), false)
                where page.StaticPageId != ExceptId && !AnotherExceptIds.Contains(page.StaticPageId)
                select new TreeNode { Text = page.PageName, Value = page.StaticPageId.ToString(), PopulateOnDemand = page.HasChildren })
            {
                node.ChildNodes.Add(childNode);
            }
        }

        private void LoadChildCategoriesMultiSelect(TreeNode node)
        {
            foreach (var c in CategoryService.GetChildCategoriesByCategoryId(SQLDataHelper.GetInt(node.Value), false).Where(c => c.CategoryId != ExceptId && !AnotherExceptIds.Contains(c.CategoryId)))
            {
                var newNode = new TreeNode
                    {
                        Text = string.Format("{0} ({1})", c.Name, c.ProductsCount),
                        Value = c.CategoryId.ToString(),
                        NavigateUrl = "javascript:void(0)",
                        Checked = SelectedCategoriesIds != null && SelectedCategoriesIds.Contains(c.ID)
                    };
                if (c.HasChild)
                {
                    newNode.Expanded = false;
                    //newNode.PopulateOnDemand = true;
                    LoadChildCategoriesMultiSelect(newNode);
                }
                else
                {
                    newNode.Expanded = true;
                    newNode.PopulateOnDemand = false;
                }
                node.ChildNodes.Add(newNode);
            }
        }

        private void LoadChildCategories(TreeNode node)
        {
            foreach (var c in CategoryService.GetChildCategoriesByCategoryId(SQLDataHelper.GetInt(node.Value), false).Where(c => c.CategoryId != ExceptId && !AnotherExceptIds.Contains(c.CategoryId)))
            {
                var newNode = new TreeNode
                    {
                        Text = string.Format("{0} ({1})", c.Name, c.ProductsCount),
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

        private void LoadChildCategoriesAndProducts(TreeNode node)
        {
            foreach (var c in CategoryService.GetChildCategoriesAndProducts(SQLDataHelper.GetInt(node.Value)).Where(c => c.Type == CatalogItemType.Category || (c.Id != ExceptId && !AnotherExceptIds.Contains(c.Id))).OrderBy(item => item.Type))
            {
                node.ChildNodes.Add(new AdvAsyncTreeNode
                    {
                        //No postback!!!
                        NavigateUrl = "javascript:void(0)",
                        Text = c.Type == CatalogItemType.Product ? c.ProductArtNo + " - " + c.Name : c.Name,
                        Value = c.Id.ToString(),
                        Bold = c.Type == CatalogItemType.Category,
                        Enabled = c.Type == CatalogItemType.Product,
                        Expanded = c.ChildCount == 0,
                        PopulateOnDemand = c.ChildCount != 0,
                        ShowCheckBox = c.Type == CatalogItemType.Product,
                    });
            }
        }

        private void LoadChildCategoriesAndOffers(TreeNode node)
        {
            foreach (var c in CategoryService.GetChildCategoriesAndOffers(SQLDataHelper.GetInt(node.Value))
                        .Where(c => c.Type == CatalogItemType.Category || (c.Id != ExceptId && !AnotherExceptIds.Contains(c.Id)))
                        .OrderBy(item => item.Type))
            {
                node.ChildNodes.Add(new AdvAsyncTreeNode
                {
                    //No postback!!!
                    NavigateUrl = "javascript:void(0)",
                    Text = c.Type == CatalogItemType.Product ? c.ProductArtNo + " - " + c.Name : c.Name,
                    Value = c.Id.ToString(),
                    Bold = c.Type == CatalogItemType.Category,
                    Enabled = c.Type == CatalogItemType.Product,
                    Expanded = c.ChildCount == 0,
                    PopulateOnDemand = c.ChildCount != 0,
                    ShowCheckBox = c.Type == CatalogItemType.Product,
                });
            }
        }

        private static void LoadChildMenuItems(TreeNode node)
        {
            foreach (var c in MenuService.GetChildMenuItemsByParentId(SQLDataHelper.GetInt(node.Value), EMenuType.Top))
            {
                var newNode = new TreeNode
                    {
                        Text = c.MenuItemName,
                        Value = c.MenuItemID.ToString()
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

        public void Show()
        {
            UpdatePanel1.Visible = true;
            mpeTree.Show();
        }
        public void Hide()
        {
            Hiding(this, new EventArgs());
            UpdatePanel1.Visible = false;
            mpeTree.Hide();
        }
        public void UnSelectAll()
        {
            UncheckAllNodes(tree.Nodes);
        }

        public void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                CheckChildren(node, false);
            }
        }

        private void CheckChildren(TreeNode rootNode, bool isChecked)
        {
            foreach (TreeNode node in rootNode.ChildNodes)
            {
                CheckChildren(node, isChecked);
                node.Checked = isChecked;
            }
        }

        protected void tree_NodeSelected(object sender, EventArgs e)
        {
            switch (Type)
            {
                case eTreeType.CategoryProduct:
                case eTreeType.CategoryOffer:
                case eTreeType.CategoryMultiSelect:
                    var args = new TreeNodeSelectedArgs();
                    foreach (TreeNode node in tree.CheckedNodes)
                    {
                        args.SelectedTexts.Add(node.Text);
                        args.SelectedValues.Add(node.Value);
                    }
                    TreeNodeSelected(this, args);
                    break;
                default:
                    TreeNodeSelected(this, new TreeNodeSelectedArgs
                        {
                            SelectedValues = new List<string> { tree.SelectedValue },
                            SelectedTexts = new List<string> { tree.SelectedNode.Text }
                        });
                    break;
            }
            Hide();
        }

        public event Action<object, TreeNodeSelectedArgs> TreeNodeSelected;

        public event Action<object, EventArgs> Hiding;

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public void UpdateTree()
        {
            UpdateTree(AnotherExceptIds ?? new List<int>());
        }

        public void UpdateTree(IEnumerable<int> anotherExceptIds)
        {
            AnotherExceptIds = anotherExceptIds != null ? anotherExceptIds.ToList() : new List<int>();
            LoadRoot();
        }

        protected void tree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            foreach (TreeNode node in e.Node.ChildNodes)
            {
                CheckNode(node, e.Node.Checked);
            }
            mpeTree.Show();
        }

        protected void CheckNode(TreeNode node, bool isChecked)
        {
            node.Checked = isChecked;
            foreach (TreeNode subNode in node.ChildNodes)
            {
                CheckNode(subNode, isChecked);
            }
        }
    }
}