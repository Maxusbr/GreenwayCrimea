using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.UrlRewriter;
using Resources;

namespace AdvantShop.Admin.UserControls.Menu
{
    public partial class MenuItemsTree : System.Web.UI.UserControl
    {
        private int _menuId;
        public EMenuType MenuType;

        protected void Page_Load(object sender, EventArgs e)
        {
            ibtnAddInRoot.OnClientClick = string.Format("open_window('m_Menu.aspx?type={0}', 750, 700);return false;", MenuType);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            _menuId = Request["Type"].TryParseEnum<EMenuType>() == MenuType
                ? Request["menuid"].TryParseInt()
                : 0;
            LoadTree();
        }

        private void LoadTree()
        {
            tree.Nodes.Clear();

            tree.Nodes.Add(new ButtonTreeNodeMenu
            {
                NavigateUrl = UrlService.GetAdminAbsoluteLink("menu.aspx?type=" + MenuType),
                Text = MenuType.Localize(),
                Value = "0",
                TreeView = tree,
                MenuType = MenuType,
                Expanded = true
            });

            LoadRootMenuItems(tree.Nodes[0].ChildNodes, MenuType);

            if (_menuId == 0 && tree.Nodes.Count > 0)
            {
                if (Request["Type"].TryParseEnum<EMenuType>() ==  MenuType)
                    tree.Nodes[0].Select();
                tree.Nodes[0].Expand();
            }
            else
            {
                var nodes = tree.Nodes[0].ChildNodes;

                var parents = MenuService.GetParentMenuItems(_menuId);
                parents.Reverse();
                foreach (var parentMenuItem in parents)
                {
                    var tn = (nodes.Cast<TreeNode>().Where(n => n.Value == parentMenuItem.ToString())).SingleOrDefault();

                    if (tn != null)
                    {
                        tn.Expand();
                        nodes = tn.ChildNodes;
                    }
                }
            }
        }

        private void LoadChildMenuItems(TreeNode node, EMenuType mItemType)
        {
            foreach (var c in MenuService.GetChildMenuItemsByParentId(node.Value.TryParseInt(), mItemType))
            {
                node.ChildNodes.Add(new ButtonTreeNodeMenu
                {
                    Text = c.Enabled
                            ? c.MenuItemName
                            : string.Format("<span style=\"color:grey;\">{0}</span>", c.MenuItemName),
                    MessageToDel = Server.HtmlEncode(string.Format(Resource.Admin_MasterPageAdminCatalog_MenuConfirmation, c.MenuItemName)),
                    Value = c.MenuItemID.ToString(),
                    NavigateUrl = UrlService.GetAdminAbsoluteLink("menu.aspx?menuid=" + c.MenuItemID + "&type=" + mItemType),
                    TreeView = tree,
                    MenuType = mItemType,
                    Selected = c.MenuItemID == _menuId,
                    Expanded = !c.HasChild,
                    PopulateOnDemand = c.HasChild
                });
            }
        }

        private void LoadRootMenuItems(TreeNodeCollection treeNodeCollection, EMenuType mItemType)
        {
            foreach (var c in MenuService.GetChildMenuItemsByParentId(0, mItemType))
            {
                treeNodeCollection.Add(new ButtonTreeNodeMenu
                {
                    Text = c.MenuItemName,
                    MessageToDel = Server.HtmlEncode(string.Format(Resource.Admin_MasterPageAdminCatalog_MenuConfirmation, c.MenuItemName)),
                    Value = c.MenuItemID.ToString(),
                    NavigateUrl = UrlService.GetAdminAbsoluteLink("menu.aspx?menuid=" + c.MenuItemID + "&type=" + mItemType),
                    TreeView = tree,
                    MenuType = mItemType,
                    Selected = c.MenuItemID == _menuId,
                    Expanded = !c.HasChild,
                    PopulateOnDemand = c.HasChild
                });
            }
        }

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            LoadChildMenuItems(e.Node, MenuType);
        }

        protected void tree_TreeNodeCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName.StartsWith("DeleteMenuItem"))
            {
                int menuId = e.CommandName.Contains("#")
                    ? e.CommandName.Split(new []{'#'}).LastOrDefault().TryParseInt()
                    : tree.SelectedValue.TryParseInt();
                if (menuId == 0) return;

                bool needRedirect = false;
                foreach (var id in MenuService.GetAllChildIdByParent(menuId, MenuType))
                {
                    MenuService.DeleteMenuItemById(id);
                    needRedirect |= id == _menuId;
                }
                if (needRedirect)
                {
                    Response.Redirect("Menu.aspx?type=" + MenuType);
                    return;
                }
                LoadTree();
            }
        }
    }

}