//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.CMS;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class m_Menu : AdvantShopAdminPage
    {
        private EMenuType _menuType;
        private int _menuItemId;

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = messageText + @"<br/>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(true);
            _menuType = Request["type"].TryParseEnum<EMenuType>();
            _menuItemId = Request["menuid"].TryParseInt();

            var menuitem = _menuItemId != 0 ? MenuService.GetMenuItemById(_menuItemId) : null;
            var parentId = menuitem != null ? menuitem.MenuItemParentID : Request["parentid"].TryParseInt();
            var parentMenuItem = parentId != 0 ? MenuService.GetMenuItemById(parentId) : null;
            
            var addingNew = menuitem == null;
            
            if (!addingNew)
            {
                _menuType = menuitem.MenuType;
            }
            else if (parentMenuItem != null)
            {
                _menuType = parentMenuItem.MenuType;
            }

            SetMeta(string.Format("{0} - {1} - {2}", SettingsMain.ShopName, _menuType.Localize(),
                addingNew ? Resource.Admin_MenuManager_CreateItem : menuitem.MenuItemName));

            if (!IsPostBack)
            {
                lblBigHead.Text = _menuType.Localize();
                btnAdd.Text = addingNew ? Resource.Admin_m_Category_Add : Resource.Admin_m_Category_Save;
                lblSubHead.Text = addingNew ? Resource.Admin_MenuManager_CreateMenuItem : Resource.Admin_MenuManager_EditMenuItem;

                lParent.Text = parentMenuItem != null ? parentMenuItem.MenuItemName : Resource.Admin_MenuManager_RootItem;
                hParent.Value = parentMenuItem != null ? parentMenuItem.MenuItemID.ToString() : string.Empty;

                LoadMenuItem(menuitem);

                if (addingNew)
                {
                    pnlIcon.Visible = false;
                    txtName.Focus();
                }

                var node = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = @"0", Selected = true };
                tree.Nodes.Add(node);
                LoadChildItems(tree.Nodes[0]);
                TreeNodeCollection nodes = tree.Nodes[0].ChildNodes;
                foreach (var parentItems in MenuService.GetParentMenuItems(String.IsNullOrEmpty(hParent.Value) ? 0 : Convert.ToInt32(hParent.Value)))
                {
                    TreeNode tn = (nodes.Cast<TreeNode>().Where(n => n.Value == parentItems.ToString(CultureInfo.InvariantCulture))).SingleOrDefault();
                    if (tn != null)
                    {
                        tn.Expand();
                        nodes = tn.ChildNodes;
                    }
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var linkType = rblLinkType.SelectedValue.TryParseEnum<EMenuItemUrlType>();
            if (linkType == EMenuItemUrlType.Custom)
            {
                lbChooseUrl.Attributes.Add("style", "display:none;");
            }

            if (IsPostBack)
            {
                switch (linkType)
                {
                    case EMenuItemUrlType.Product:
                        if (gridProduct.SelectProductId == 0) break;
                        txtUrl.Text = UrlService.GetLinkDB(ParamType.Product, gridProduct.SelectProductId);
                        hfParamId.Value = gridProduct.SelectProductId.ToString(CultureInfo.InvariantCulture);
                        break;
                    case EMenuItemUrlType.Category:
                        if (gridCategory.SelectCategoryId == 0) break;
                        txtUrl.Text = UrlService.GetLinkDB(ParamType.Category, gridCategory.SelectCategoryId);
                        hfParamId.Value = gridCategory.SelectCategoryId.ToString(CultureInfo.InvariantCulture);
                        break;
                    case EMenuItemUrlType.StaticPage:
                        if (gridAux.SelectAuxId == 0) break;
                        txtUrl.Text = UrlService.GetLinkDB(ParamType.StaticPage, gridAux.SelectAuxId);
                        hfParamId.Value = gridAux.SelectAuxId.ToString(CultureInfo.InvariantCulture);
                        break;
                    case EMenuItemUrlType.News:
                        if (gridNews.SelectNewsId == 0) break;
                        txtUrl.Text = UrlService.GetLinkDB(ParamType.News, gridNews.SelectNewsId);
                        hfParamId.Value = gridNews.SelectNewsId.ToString(CultureInfo.InvariantCulture);
                        break;
                    case EMenuItemUrlType.Brand:
                        if (gridBrand.SelectBrandId == 0) break;
                        txtUrl.Text = UrlService.GetLinkDB(ParamType.Brand, gridBrand.SelectBrandId);
                        break;
                }
            }
        }

        protected void tree_SelectedNodeChange(object sender, EventArgs e)
        {
            mpeTree.Show();
            lParent.Text = tree.SelectedNode.Text;
            hParent.Value = tree.SelectedNode.Value;
        }

        protected void btnUpdateParent_Click(object sender, EventArgs e)
        {
            mpeTree.Hide();
            lParent.Text = tree.SelectedNode.Text;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            if (_menuItemId != 0)
            {
                SaveMenuItem();
            }
            else
            {
                CreateMenuItem();
            }

            if (lblError.Visible == false)
            {
                CommonHelper.RegCloseScript(this, "'Menu.aspx?MenuID=" + (hParent.Value.IsNullOrEmpty() ? "0" : hParent.Value) + "&type=" + _menuType + "';");
            }
        }

        private void LoadMenuItem(AdvMenuItem mItem)
        {
            if (mItem == null)
                return;

            txtName.Text = mItem.MenuItemName;
            txtSortOrder.Text = mItem.SortOrder.ToString(CultureInfo.InvariantCulture);

            txtUrl.Text = mItem.MenuItemUrlPath;
            hfParamId.Value = mItem.MenuItemUrlType != EMenuItemUrlType.Custom ? mItem.MenuItemUrlPath : "0";

            ckbBlank.Checked = mItem.Blank;
            ckbNofollow.Checked = mItem.NoFollow;
            ckbEnabled.Checked = mItem.Enabled;
            ddlShowMode.SelectedValue = ((int)mItem.ShowMode).ToString(CultureInfo.InvariantCulture);
            rblLinkType.SelectedValue = ((int)mItem.MenuItemUrlType).ToString(CultureInfo.InvariantCulture);

            if (File.Exists(Server.MapPath("~") + "\\pictures\\icons\\" + mItem.MenuItemIcon))
            {
                lblIconFileName.Text = mItem.MenuItemIcon;
                pnlIcon.Visible = true;

                imgIcon.ImageUrl = UrlService.GetAbsoluteLink("/pictures/icons/" + mItem.MenuItemIcon);
                imgIcon.ToolTip = mItem.MenuItemIcon;
            }
            else
            {
                lblIconFileName.Text = @"No picture";
                pnlIcon.Visible = false;
            }
        }

        private void SaveMenuItem()
        {
            var url = string.Empty;
            if (txtUrl.Text.Contains("www."))
            {
                url = txtUrl.Text.Contains("http://") || txtUrl.Text.Contains("https://") ? txtUrl.Text : "http://" + txtUrl.Text;
            }
            else
            {
                url = txtUrl.Text;
            }

            lblError.Text = String.Empty;
            var mItem = new AdvMenuItem
            {
                MenuItemID = _menuItemId,
                MenuItemName = txtName.Text,
                MenuItemParentID = string.IsNullOrEmpty(hParent.Value) ? 0 : hParent.Value.TryParseInt(),
                MenuItemUrlPath = url,
                SortOrder = txtSortOrder.Text.TryParseInt(),
                Blank = ckbBlank.Checked,
                Enabled = ckbEnabled.Checked,
                MenuItemUrlType = (EMenuItemUrlType)rblLinkType.SelectedValue.TryParseInt(),
                ShowMode = (EMenuItemShowMode) ddlShowMode.SelectedValue.TryParseInt(),
                NoFollow = ckbNofollow.Checked,
            };

            if (IconFileUpload.HasFile)
            {
                PhotoService.DeletePhotos(_menuItemId, PhotoType.MenuIcon);
                using (IconFileUpload.FileContent)
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, _menuItemId, PhotoType.MenuIcon) { OriginName = IconFileUpload.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        IconFileUpload.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, tempName));
                    }
                    mItem.MenuItemIcon = tempName;
                }
            }
            else
            {
                mItem.MenuItemIcon = pnlIcon.Visible ? imgIcon.ToolTip : null;
            }

            MenuService.UpdateMenuItem(mItem);
        }

        private void CreateMenuItem()
        {
            if (!ValidateData())
                return;

            var mItem = new AdvMenuItem
            {
                MenuItemName = txtName.Text,
                MenuItemParentID = string.IsNullOrEmpty(hParent.Value) ? 0 : hParent.Value.TryParseInt(),
                MenuItemUrlPath = txtUrl.Text,
                Enabled = ckbEnabled.Checked,
                Blank = ckbBlank.Checked,
                SortOrder = txtSortOrder.Text.TryParseInt(),
                MenuItemUrlType = (EMenuItemUrlType) rblLinkType.SelectedValue.TryParseInt(),
                ShowMode = (EMenuItemShowMode) ddlShowMode.SelectedValue.TryParseInt(),
                NoFollow = ckbNofollow.Checked,
                MenuType = _menuType
            };

            mItem.MenuItemID = MenuService.AddMenuItem(mItem);
            _menuItemId = mItem.MenuItemID;
            if (IconFileUpload.HasFile)
            {
                using (IconFileUpload.FileContent)
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, _menuItemId, PhotoType.MenuIcon) { OriginName = IconFileUpload.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        IconFileUpload.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, tempName));
                    }
                    mItem.MenuItemIcon = tempName;
                }
            }
            else
            {
                mItem.MenuItemIcon = pnlIcon.Visible ? imgIcon.ToolTip : null;
            }

            MenuService.UpdateMenuItem(mItem);
        }

        protected void btnDeleteIcon_Click(object sender, EventArgs e)
        {
            try
            {
                MenuService.DeleteMenuItemIconById(_menuItemId, Server.MapPath("~") + "\\pictures\\icons\\" + lblIconFileName.Text);
                pnlIcon.Visible = false;
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " at DeleteImage");
                Debug.Log.Error("DeleteImage", ex);
            }
        }

        private bool ValidateData()
        {
            bool valid = true;
            valid &= txtName.Text.IsNotEmpty();
            valid &= txtSortOrder.Text.IsNotEmpty();
            valid &= txtUrl.Text.IsNotEmpty() || rblLinkType.SelectedValue == "5";
            
            if (!valid)
                MsgErr("Заполните обязательные поля");

            return valid;
        }

        protected void PopulateNode(object sender, TreeNodeEventArgs e)
        {
            LoadChildItems(e.Node);
        }

        private void LoadChildItems(TreeNode node)
        {
            var childMenuItems = MenuService.GetChildMenuItemsByParentId(node.Value.TryParseInt(), _menuType);

            foreach (var c in childMenuItems)
            {
                node.ChildNodes.Add(new TreeNode
                {
                    Text = c.MenuItemName,
                    Value = c.MenuItemID.ToString(),
                    Selected = hParent.Value == c.MenuItemID.ToString(),
                    Expanded = !c.HasChild,
                    PopulateOnDemand = c.HasChild
                });
            }
        }

        protected void lbParentChange_Click(object sender, EventArgs e)
        {
            mpeTree.Show();
        }

    }
}