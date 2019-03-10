//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Admin.UserControls;
using AdvantShop;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Properties;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using AdvantShop.Trial;
using Resources;
using AdvantShop.Core.UrlRewriter;

namespace Admin
{
    public partial class EditStaticPage : AdvantShopAdminPage
    {
        protected bool AddingNew
        {
            get { return string.IsNullOrEmpty(Request["pageid"]) || Request["pageid"].ToLower() == "addnew"; }
        }

        protected int ParentPageID
        {
            get
            {
                int res = 0;
                int.TryParse(Request["parentid"], out res);
                return res;
            }
        }

        protected int StaticPageId
        {
            get
            {
                int res;
                int.TryParse(Request["pageid"], out res);
                return res;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_StaticPage_AuxEdit));

            //fckPageText.Language = CultureInfo.CurrentCulture.ToString();

            MsgErr(false);
            if (!IsPostBack)
            {
                popTree.ExceptId = StaticPageId;
                popTree.UpdateTree();
                MsgErr(true);

                btnSave.Text = AddingNew ? Resource.Admin_StaticPage_Create : Resource.Admin_StaticPage_Save;
                if (!AddingNew)
                {
                    lblSubHead.Text = Resource.Admin_StaticPage_AuxEdit;
                    LoadStaticPage();
                }
                else
                {
                    lblParentName.Text = ParentPageID == 0 ? Resources.Resource.Admin_StaticPage_Root : StaticPageService.GetStaticPage(ParentPageID).PageName;
                    txtPageTitle.Text = string.Empty;
                    txtMetaKeywords.Text = string.Empty;
                    txtMetaDescription.Text = string.Empty;
                    hfParentId.Value = ParentPageID.ToString();
                    chkIndexAtSitemap.Checked = true;
                }
            }
            Header.Title = lblHead.Text + " - " + lblSubHead.Text;
        }

        protected void LoadStaticPage()
        {
            var page = StaticPageService.GetStaticPage(StaticPageId);
            if (page == null)
            {
                spAuxFoundNotification.InnerHtml = Resource.Client_AuxView_AuxNotFound;
                return;
            }
            lblHead.Text = page.PageName;
            txtPageName.Text = page.PageName;
            chkEnabled.Checked = page.Enabled;
            chkIndexAtSitemap.Checked = page.IndexAtSiteMap;
            txtSynonym.Text = page.UrlPath;
            txtSortOrder.Text = page.SortOrder.ToString();
            if (page.Parent != null)
                lblParentName.Text = page.Parent.PageName;
            hfParentId.Value = page.ParentId.ToString();
            //meta
            //hfMetaId.Value = page.MetaId.ToString();

            page.Meta = MetaInfoService.GetMetaInfo(StaticPageId, MetaType.StaticPage) ??
                        new MetaInfo(0, 0, MetaType.StaticPage, string.Empty, string.Empty, string.Empty, string.Empty);
            txtPageTitle.Text = page.Meta.Title;
            txtH1.Text = page.Meta.H1;
            txtMetaKeywords.Text = page.Meta.MetaKeywords;
            txtMetaDescription.Text = page.Meta.MetaDescription;

            //ddlParentPage.SelectedValue = page.ParentPageId.ToString();
            fckPageText.Text = page.PageText;
            popTree.ExceptId = StaticPageId;
            //popTree.UpdateTree();
            aToClient.HRef = "../" + UrlService.GetLinkDB(ParamType.StaticPage, StaticPageId);
            aToClient.Visible = true;
        }

        protected void CreateStaticPage()
        {
            if (!ValidateInput())
                return;
            var id = StaticPageService.AddStaticPage(new StaticPage
                {
                    PageName = txtPageName.Text,
                    PageText = fckPageText.Text,
                    UrlPath = txtSynonym.Text,
                    ParentId = hfParentId.Value == "" ? 0 : SQLDataHelper.GetInt(hfParentId.Value),
                    Meta = new MetaInfo
                        {
                            Type = MetaType.StaticPage ,
                            Title = txtPageTitle.Text,
                            MetaKeywords = txtMetaKeywords.Text,
                            MetaDescription = txtMetaDescription.Text,
                            H1 = txtH1.Text
                        },
                    IndexAtSiteMap = chkIndexAtSitemap.Checked,
                    HasChildren = false,
                    Enabled = chkEnabled.Checked,
                    SortOrder = txtSortOrder.Text.TryParseInt()
                });
            Response.Redirect(string.Format("StaticPage.aspx?PageID={0}", id));
        }

        protected void SaveStaticPage()
        {
            if (!ValidateInput())
                return;

            var page = StaticPageService.GetStaticPage(StaticPageId);
            page.PageName = txtPageName.Text;
            page.PageText = fckPageText.Text;
            page.UrlPath = txtSynonym.Text;
            page.ParentId = SQLDataHelper.GetInt(hfParentId.Value);
            page.IndexAtSiteMap = chkIndexAtSitemap.Checked;
            page.ModifyDate = DateTime.Now;
            page.Enabled = chkEnabled.Checked;
            page.SortOrder = txtSortOrder.Text.TryParseInt();

            page.Meta = new MetaInfo(0, StaticPageId, MetaType.StaticPage, txtPageTitle.Text, txtMetaKeywords.Text, txtMetaDescription.Text, txtH1.Text);

            StaticPageService.UpdateStaticPage(page);
            LoadStaticPage();
        }

        protected bool ValidateInput()
        {
            txtSynonym.Text = StringHelper.TransformUrl(txtSynonym.Text);

            string synonym = txtSynonym.Text;
            if (string.IsNullOrEmpty(synonym))
            {
                MsgErr(Resource.Admin_StaticPage_URLIsRequired);
                return false;
            }

            var pattern = !SettingsMain.EnableCyrillicUrl ? "^[a-zA-Z0-9_-]*$" : "^[a-zA-Zà-ÿÀ-ß0-9_-]*$";
            var r = new Regex(pattern);

            if (!r.IsMatch(synonym))
            {
                MsgErr(Resource.Admin_m_Category_SynonymInfo);
                return false;
            }
            if (StaticPageId == 0 ? !UrlService.IsAvailableUrl(ParamType.StaticPage, synonym) : !UrlService.IsAvailableUrl(StaticPageId, ParamType.StaticPage, synonym))
            {
                MsgErr(Resource.Admin_SynonymExist);
                return false;
            }

            if (string.IsNullOrEmpty(txtPageName.Text))
            {
                MsgErr(Resource.Client_AuxView_EnterTitle);
                return false;
            }

            if (string.IsNullOrEmpty(fckPageText.Text))
            {
                MsgErr(Resource.Client_AuxView_EnterText);
                return false;
            }

            int ti;
            if (!Int32.TryParse(txtSortOrder.Text, out ti))
            {
                txtSortOrder.Text = "0";
            }
            if (StaticPageId == 132)
                TrialService.TrackEvent(TrialEvents.ChangeContactPage, "");
            return true;

        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                Message.Visible = false;
                Message.Text = "";
            }
            else
            {
                Message.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            Message.Visible = true;
            Message.Text = "<br/>" + messageText;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (AddingNew)
                CreateStaticPage();
            else
                SaveStaticPage();
        }

        protected void btnSelectParent_Click(object sender, EventArgs e)
        {
            popTree.Show();
        }

        protected void popTree_TreeNodeSelected(object sender, PopupTreeView.TreeNodeSelectedArgs args)
        {
            lblParentName.Text = args.SelectedTexts[0];
            hfParentId.Value = args.SelectedValues[0];
        }
    }
}