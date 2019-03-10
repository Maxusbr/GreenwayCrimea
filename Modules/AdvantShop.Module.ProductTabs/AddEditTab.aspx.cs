//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using System.Web.UI;
using AdvantShop.Module.ProductTabs.Domain;

namespace AdvantShop.Module.ProductTabs
{
    public partial class Modules_DetailsCommonTabs_AddEditTab : Page
    {
        private ProductTab tab;
        private int tabBodyId;
        private int tabTitleId;

        protected void Page_Load(object sender, EventArgs e)
        {
            lBase.Text = string.Format("<base href='{0}'/>",
                           Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath +
                           (!Request.ApplicationPath.EndsWith("/") ? "/" : string.Empty) + "modules/producttabs/");

            if (!string.IsNullOrEmpty(Request["tabTitleId"]) && Int32.TryParse(Request["tabTitleId"], out tabTitleId)
                && !string.IsNullOrEmpty(Request["tabBodyId"]) && Int32.TryParse(Request["tabBodyId"], out tabBodyId))
            {
                tab = ProductTabsRepository.GetProductCommonTab(tabTitleId, tabBodyId);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            if (tab == null)
            {
                tab = new ProductTab {Active = true};
                btnSave.Text = (string) GetLocalResourceObject("DetailsCommonTabsAddEdit_Create");
            }

            txtTabTitle.Text = tab.Title;
            txtTabBody.Text = tab.Body;
            ckbActive.Checked = tab.Active;
            txtSortOrder.Text = tab.SortOrder.ToString();
        }

        protected void btnSaveClick(object sender, EventArgs e)
        {
            if (tab == null)
            {
                tab = new ProductTab();
            }

            int sortOrder = 0;
            if (int.TryParse(txtSortOrder.Text, out sortOrder))
            {
                tab.SortOrder = sortOrder;
                tab.Title = txtTabTitle.Text;
                tab.Body = txtTabBody.Text;
                tab.Active = ckbActive.Checked;
                tab.ProductId = 0;
            }
            else
            {
                lblError.Visible = true;
                return;
            }


            if (tab.TabTitleId != 0)
            {
                ProductTabsRepository.UpdateProductTabTitle(
                    new TabTitle
                        {
                            SortOrder = tab.SortOrder,
                            TabTitleId = tab.TabTitleId,
                            Title = tab.Title,
                            Active = tab.Active
                        });
            }
            else
            {
                tab.TabTitleId = ProductTabsRepository.AddProductTabTitle(
                    new TabTitle
                        {
                            SortOrder = tab.SortOrder,
                            Active = tab.Active,
                            Title = tab.Title
                        });
            }

            if (tab.TabTitleId != 0)
            {
                ProductTabsRepository.AddUpdateProductTabBody(
                    new TabBody
                        {
                            ProductId = 0,
                            Body = tab.Body,
                            TabTitleId = tab.TabTitleId
                        });
            }

            var jScript = new StringBuilder();
            jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
            if (string.IsNullOrEmpty(string.Empty))
                jScript.Append("window.opener.location.reload();");
            else
                jScript.Append("window.opener.location =" + string.Empty);
            jScript.Append("self.close();");
            jScript.Append("</script>");
            Type csType = GetType();
            ClientScriptManager clScriptMng = ClientScript;
            clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
        }
    }
}