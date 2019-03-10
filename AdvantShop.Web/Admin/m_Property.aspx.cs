//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI.WebControls;
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
    public partial class m_Property : AdvantShopAdminPage
    {
        #region Fields

        private int _propertyId;
        protected int PropertyId
        {
            get { return _propertyId != 0 ? _propertyId : (_propertyId = Request["propertyId"].TryParseInt()); }
        }

        private int _groupId;
        protected int GroupId
        {
            get { return _groupId != 0 ? _groupId : (_groupId = Request["groupId"].TryParseInt()); }
        }

        #endregion

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (!IsValidData()) return;

            if (PropertyId != 0)
                SaveProperty();
            else
                CreateProperty();

            if (lblError.Visible == false)
                CommonHelper.RegCloseScript(this, string.Empty);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Properties_Header));

            if (IsPostBack) 
                return;

            ddlGroup.DataSource = PropertyGroupService.GetList();
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, Resource.Admin_m_Property_None);

            foreach (PropertyType item in Enum.GetValues(typeof(PropertyType)))
            {
                ddlTypes.Items.Add(new ListItem(item.Localize(), ((int)item).ToString()));
            }
            
            if (PropertyId != 0)
            {
                btnOK.Text = Resource.Admin_m_News_Save;
                LoadProperty(PropertyId);
            }
            else
            {
                btnOK.Text = Resource.Admin_m_News_Add;
                txtName.Text = string.Empty;

                if (GroupId != 0 && ddlGroup.Items.FindByValue(GroupId.ToString()) != null)
                    ddlGroup.SelectedValue = GroupId.ToString();
            }
        }

        private bool IsValidData()
        {
            var isValid = txtName.Text.Trim().IsNotEmpty();

            if (!isValid)
            {
                MsgErr(Resource.Admin_ContentRequired);
            }

            return isValid;
        }

        private bool IsValidValues(Property property, int newType)
        {
            if (property.Type != newType && newType == (int)PropertyType.Range)
            {
                foreach (var propertyValue in property.ValuesList)
                {
                    if (propertyValue.Value != "0" && propertyValue.Value.RemoveChars() == 0)
                        return false;
                }
            }
            return true;
        }

        protected void SaveProperty()
        {
            if (!IsValidData())
                return;

            try
            {
                var property = PropertyService.GetPropertyById(PropertyId);

                if (!IsValidValues(property, ddlTypes.SelectedValue.TryParseInt()))
                {
                    MsgErr(Resource.Admin_m_Property_NotValidNumber);
                    return;
                }

                property.Name = txtName.Text.Trim();
                property.NameDisplayed = txtNameDisplayed.Text.Trim();
                property.Unit = txtUnit.Text.Trim();
                property.Description = txtDescription.Text.Trim();
                property.Type = ddlTypes.SelectedValue.TryParseInt();

                var groupId = ddlGroup.SelectedValue.TryParseInt();
                property.GroupId = groupId != 0 ? groupId : default(int?);

                property.UseInFilter = chkUseInFilter.Checked;
                property.UseInDetails = chkUseInDetails.Checked;
                property.UseInBrief = chkUseInBrief.Checked;

                property.Expanded = chkExpanded.Checked;
                property.SortOrder = txtSortOrder.Text.TryParseInt();
                
                PropertyService.UpdateProperty(property);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        protected void CreateProperty()
        {
            if (!IsValidData())
                return;

            try
            {
                var property = new Property()
                {
                    Name = txtName.Text.Trim(),
                    NameDisplayed = txtNameDisplayed.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Unit = txtUnit.Text.Trim(),
                    Type = ddlTypes.SelectedValue.TryParseInt(),
                    GroupId = ddlGroup.SelectedValue.TryParseInt() != 0 
                                ? ddlGroup.SelectedValue.TryParseInt() 
                                : default(int?),
                    UseInFilter = chkUseInFilter.Checked,
                    UseInDetails = chkUseInDetails.Checked,
                    UseInBrief =  chkUseInBrief.Checked,

                    Expanded = chkExpanded.Checked,
                    SortOrder = txtSortOrder.Text.TryParseInt(),
                };

                PropertyService.AddProperty(property);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        protected void LoadProperty(int propertyId)
        {
            var property = PropertyService.GetPropertyById(propertyId);

            if (property == null)
            {
                MsgErr(Resource.Admin_m_PropertyGroup_ErrorLoading);
                return;
            }

            txtName.Text = property.Name;
            txtNameDisplayed.Text = string.IsNullOrEmpty(property.NameDisplayed)
                ? property.Name
                : property.NameDisplayed;
            txtSortOrder.Text = property.SortOrder.ToString();

            chkUseInFilter.Checked = property.UseInFilter;
            chkUseInDetails.Checked = property.UseInDetails;
            chkUseInBrief.Checked = property.UseInBrief;

            chkExpanded.Checked = property.Expanded;

            txtDescription.Text = property.Description;
            txtUnit.Text = property.Unit;

            if (ddlTypes.Items.FindByValue(property.Type.ToString()) != null)
                ddlTypes.SelectedValue = property.Type.ToString();

            if (property.GroupId != null && ddlGroup.Items.FindByValue(property.GroupId.ToString()) != null)
                ddlGroup.SelectedValue = property.GroupId.ToString();
        }
    }
}