//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Module.ProductTabs.Domain;

namespace AdvantShop.Module.ProductTabs
{
    public partial class Admin_DetailsProductTabsSettings : UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtTabTitle.Text = string.Empty;
            txtSortOrder.Text = @"0";

            LoadData();
        }

        protected void LoadData()
        {
            lvTabs.DataSource = ProductTabsRepository.GetProductTabTitles();
            lvTabs.DataBind();
        }

        protected void lvTabs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int tabId;
            if (!int.TryParse(e.CommandArgument.ToString(), out tabId))
            {
                return;
            }

            switch (e.CommandName)
            {
                case "deleteTab":
                    ProductTabsRepository.DeleteProductTabTitle(tabId);
                    break;
                case "updateTab":
                    string title = ((TextBox) lvTabs.EditItem.FindControl("txtEditTitle")).Text;
                    string sortOrderString = ((TextBox) lvTabs.EditItem.FindControl("txtEditSortOrder")).Text;
                    bool active = ((CheckBox) lvTabs.EditItem.FindControl("ckbEditActive")).Checked;
                    int sortOrder = 0;
                    if (!string.IsNullOrEmpty(title) && int.TryParse(sortOrderString, out sortOrder))
                    {
                        ProductTabsRepository.UpdateProductTabTitle(new TabTitle
                            {
                                TabTitleId = tabId,
                                Title = title,
                                SortOrder = sortOrder,
                                Active = active
                            });
                    }
                    lvTabs.EditIndex = -1;
                    break;
            }
        }

        protected void lvTabs_ItemEditing(object sender, ListViewEditEventArgs e)
        {
        }

        protected void lvTabs_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvTabs.EditIndex = -1;
        }

        protected void btnAddTabTitle_OnClick(object sender, EventArgs e)
        {
            int sortOrder = 0;
            if (int.TryParse(txtSortOrder.Text, out sortOrder) && !string.IsNullOrEmpty(txtTabTitle.Text))
            {
                var title = ProductTabsRepository.GetProductTabTitle(txtTabTitle.Text);
                if (title != null)
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = @"¬кладка с таким заголовком уже существует";
                    return;
                }

                var id = ProductTabsRepository.AddProductTabTitle(
                    new TabTitle
                    {
                        Title = txtTabTitle.Text,
                        SortOrder = sortOrder,
                        Active = ckbActive.Checked
                    });

                if (id != 0)
                {
                    txtTabTitle.Text = "";
                    txtSortOrder.Text = "0";
                }
            }
        }
    }
}