//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class m_PropertyGroup : AdvantShopAdminPage
    {
        private int _groupId;
        protected int GroupId
        {
            get { return _groupId != 0 ? _groupId : (_groupId = Request["groupId"].TryParseInt()); }
        }
        
        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (!Valid()) return;

            if (GroupId != 0)
                SaveGroup();
            else
                CreateGroup();

            if (lblError.Visible == false)
                CommonHelper.RegCloseScript(this, string.Empty);
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_PropertyGroup_Header));

            if (IsPostBack) 
                return;
            
            if (GroupId != 0)
            {
                btnOK.Text = Resource.Admin_m_News_Save;
                LoadGroup(GroupId);
            }
            else
            {
                btnOK.Text = Resource.Admin_m_News_Add;
                txtName.Text = string.Empty;
                txtSortOrder.Text = "0";
                categoriesTr.Visible = false;
            }
        }

        protected bool Valid()
        {
            return txtName.Text.Trim().IsNotEmpty();
        }

        protected void SaveGroup()
        {
            if (!Valid())
                return;
            try
            {
                PropertyGroupService.Update(new PropertyGroup()
                {
                    PropertyGroupId = GroupId,
                    Name = txtName.Text.Trim(),
                    NameDisplayed = txtNameDisplayed.Text.Trim(),
                    SortOrder = txtSortOrder.Text.TryParseInt()
                });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        protected void CreateGroup()
        {
            if (!Valid())
                return;

            try
            {
                PropertyGroupService.Add(new PropertyGroup()
                {
                    PropertyGroupId = GroupId,
                    Name = txtName.Text.Trim(),
                    NameDisplayed = txtNameDisplayed.Text.Trim(),
                    SortOrder = txtSortOrder.Text.TryParseInt()
                });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        protected void LoadGroup(int groupId)
        {
            var group = PropertyGroupService.Get(groupId);

            if (group == null)
            {
                MsgErr(Resource.Admin_m_PropertyGroup_ErrorLoading);
                return;
            }

            txtName.Text = group.Name;
            txtNameDisplayed.Text = string.IsNullOrEmpty(group.NameDisplayed) ? group.Name : group.NameDisplayed;
            txtSortOrder.Text = group.SortOrder.ToString();

            var categories = PropertyGroupService.GetGroupCategories(groupId);
            liCategories.Text = categories.Count > 0 ? string.Join(", ", categories) : Resource.Admin_m_PropertyGroup_NoCategories;
        }
    }
}