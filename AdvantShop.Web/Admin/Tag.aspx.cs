//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.SEO;
using Resources;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Admin
{
    public partial class EditTag : AdvantShopAdminPage
    {
        protected bool AddingNew
        {
            get { return string.IsNullOrEmpty(Request["id"]) || Request["id"].ToLower() == "addnew"; }
        }

        protected int Id
        {
            get
            {
                return Request["id"].TryParseInt();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Tags_EditTag));
            
            MsgErr(false);
            if (!IsPostBack)
            {
                MsgErr(true);

                btnSave.Text = AddingNew ? Resource.Admin_Tags_Create : Resource.Admin_Tags_Save;
                if (!AddingNew)
                {
                    lblSubHead.Text = Resource.Admin_Tags_EditTag;
                    LoadTag();
                }
                //else
                //{
                //    txtPageTitle.Text = string.Empty;
                //    txtMetaKeywords.Text = string.Empty;
                //    txtMetaDescription.Text = string.Empty;
                //}
            }
            Header.Title = lblHead.Text + " - " + lblSubHead.Text;
        }

        protected void LoadTag()
        {
            var model = TagService.Get(Id);
            if (model == null)
            {
                spAuxFoundNotification.InnerHtml = Resource.Admin_Tags_TagNotFound;
                return;
            }
            lblHead.Text = model.Name;
            txtName.Text = model.Name;
            chkEnabled.Checked = model.Enabled;
            txtSynonym.Text = model.UrlPath;
            fckBrief.Text = model.BriefDescription;
            fckDesc.Text = model.Description;

            var meta = MetaInfoService.GetMetaInfo(Id, MetaType.Tag);

            if (meta == null)
                return;

            chbDefaultMeta.Checked = model.Meta.H1.IsNullOrEmpty() && model.Meta.Title.IsNullOrEmpty() &&
                model.Meta.MetaKeywords.IsNullOrEmpty() && model.Meta.MetaDescription.IsNullOrEmpty();
            txtH1.Text = model.Meta.H1;
            txtTitle.Text = model.Meta.Title;
            txtMetaKeywords.Text = model.Meta.MetaKeywords;
            txtMetaDescription.Text = model.Meta.MetaDescription;

        }

        protected void Create()
        {
            if (!ValidateInput())
                return;

            var tag = TagService.Get(txtName.Text);
            if (tag != null)
            {
                MsgErr("“ег с таким названием уже существует.");
                return;
            }

            var meta = new MetaInfo();
            if (!string.IsNullOrEmpty(txtTitle.Text) || !string.IsNullOrEmpty(txtH1.Text) ||
                    !string.IsNullOrEmpty(txtMetaKeywords.Text) ||
                    !string.IsNullOrEmpty(txtMetaDescription.Text))
            {
                meta.Type = MetaType.Tag;
                meta.Title = txtTitle.Text;
                meta.H1 = txtH1.Text;
                meta.MetaKeywords = txtMetaKeywords.Text;
                meta.MetaDescription = txtMetaDescription.Text;
            }

            var id = TagService.Add(new Tag
            {
                Name = txtName.Text,
                BriefDescription = fckBrief.Text,
                Description = fckDesc.Text,
                UrlPath = txtSynonym.Text,
                Meta = meta,
                Enabled = chkEnabled.Checked,
            });
            Response.Redirect(string.Format("Tag.aspx?id={0}", id));
        }

        protected void Save()
        {
            if (!ValidateInput())
                return;

            var tag = TagService.Get(txtName.Text);
            if (tag != null && Id != tag.Id)
            {
                MsgErr("“ег с таким названием уже существует.");
                return;
            }

            var model = TagService.Get(Id);
            model.Name = txtName.Text;
            model.BriefDescription = fckBrief.Text;
            model.Description = fckDesc.Text;
            model.UrlPath = txtSynonym.Text;
            model.Enabled = chkEnabled.Checked;
            if (!chbDefaultMeta.Checked)
            {
                model.Meta = new MetaInfo
                {
                    Type = MetaType.Tag,
                    Title = txtTitle.Text,
                    MetaKeywords = txtMetaKeywords.Text,
                    MetaDescription = txtMetaDescription.Text,
                    H1 = txtH1.Text,
                    ObjId = Id
                };
            }

            MetaInfoService.DeleteMetaInfo(Id, MetaType.Tag);
            TagService.Update(model);
            LoadTag();
        }

        protected bool ValidateInput()
        {
            string synonym = txtSynonym.Text;
            if (string.IsNullOrEmpty(synonym))
            {
                MsgErr(Resource.Admin_StaticPage_URLIsRequired);
                return false;
            }
            var r = new Regex("^[a-zA-Z0-9_-]*$");

            if (!r.IsMatch(synonym))
            {
                MsgErr(Resource.Admin_m_Category_SynonymInfo);
                return false;
            }
            if (Id == 0 ? !UrlService.IsAvailableUrl(ParamType.Tag, synonym) : !UrlService.IsAvailableUrl(Id, ParamType.Tag, synonym))
            {
                MsgErr(Resource.Admin_SynonymExist);
                return false;
            }

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MsgErr(Resource.Client_AuxView_EnterTitle);
                return false;
            }

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
                Create();
            else
                Save();
        }
    }
}