//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Controls;
using AdvantShop.Helpers;

namespace AdvantShop.Module.Blog
{
    public partial class PopupTreeView : UserControl
    {

        public class TreeNodeSelectedArgs : EventArgs
        {
            public List<string> SelectedValues = new List<string>();
            public List<string> SelectedTexts = new List<string>();
        }

        public List<int> SelectedCategoriesIds { set; get; }
        public bool CheckChildrenNodes = false;
        public string HeaderText { get; set; }

        public int ExceptId
        {
            get { return (int)(ViewState["ExceptId"] ?? 0); }
            set { ViewState["ExceptId"] = value; }
        }

        protected List<int> AnotherExceptIds
        {
            get { return (List<int>)ViewState["AnotherIds"]; }
            set { ViewState["AnotherIds"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //if (Type == eTreeType.CategoryProduct || Type == eTreeType.CategoryOffer)
            //    tree.SelectedNodeChanged -= tree_NodeSelected;
            //else
            //   tree.SelectedNodeChanged += tree_NodeSelected;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && tree.Nodes.Count == 0)
                UpdateTree(AnotherExceptIds);
            btnOk.Visible = true;
            btnCancel.Visible = true;
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
            tree.Nodes.Add(new TreeNode { Text = (string)GetLocalResourceObject("Admin_Blog_Category_Root"), Value = "0" });
            tree.ShowCheckBoxes = TreeNodeTypes.Leaf;
            LoadChildCategoriesAndProducts(tree.Nodes[0]);
        }

        public void PopulateNode(object sender, TreeNodeEventArgs e)
        {
            LoadChildCategoriesAndProducts(e.Node);
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
            var args = new TreeNodeSelectedArgs();
            foreach (TreeNode node in tree.CheckedNodes)
            {
                args.SelectedTexts.Add(node.Text);
                args.SelectedValues.Add(node.Value);
            }
            TreeNodeSelected(this, args);
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