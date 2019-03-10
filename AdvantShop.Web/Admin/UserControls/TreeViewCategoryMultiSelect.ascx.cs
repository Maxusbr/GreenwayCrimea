using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Helpers;
using Resources;

namespace AdvantShop.Admin.UserControls
{
    public partial class TreeViewCategoryMultiSelect : System.Web.UI.UserControl
    {
        public class TreeNodeSelectedArgs : EventArgs
        {
            public List<string> SelectedValues = new List<string>();
            public List<string> SelectedTexts = new List<string>();
        }

        public event Action<object, TreeNodeSelectedArgs> TreeNodeSelected;

        protected void Page_Init(object sender, EventArgs e)
        {
            tree.SelectedNodeChanged += tree_NodeSelected;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && tree.Nodes.Count == 0)
                LoadRoot();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            tree.Attributes.Add("onClick", "TreeViewCheckBoxClicked(event)");
        }

        private void LoadRoot()
        {
            tree.Nodes.Clear();
            Category rootCategory;
            if ((rootCategory  = CategoryService.GetCategory(0)) == null)
                return;

            tree.Nodes.Add(new TreeNode
            {
                Text = rootCategory.Name,
                Value = rootCategory.CategoryId.ToString(),
                ShowCheckBox = true
            });
            LoadChildCategoriesMultiSelect(tree.Nodes[0]);
        }

        public void tree_NodeSelected(object sender, EventArgs e)
        {
            var args = new TreeNodeSelectedArgs();
            foreach (TreeNode node in tree.CheckedNodes)
            {
                args.SelectedTexts.Add(node.Text);
                args.SelectedValues.Add(node.Value);
            }
            TreeNodeSelected(this, args);
        }

        private void LoadChildCategoriesMultiSelect(TreeNode node)
        {
            foreach (var c in CategoryService.GetChildCategoriesByCategoryId(SQLDataHelper.GetInt(node.Value), false))
            {
                var newNode = new TreeNode
                {
                    Text = string.Format("{0} ({1})", c.Name, c.ProductsCount),
                    Value = c.CategoryId.ToString(),
                    NavigateUrl = "javascript:void(0)"
                };
                if (c.HasChild)
                {
                    newNode.Expanded = false;
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
    }
}