//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using AdvantShop.CMS;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;
using AdvantShop.Configuration;

namespace Admin
{
    public partial class m_StaticBlock : AdvantShopAdminPage
    {
        //#region eCategoryMode enum

        public enum eStaicBlockMode
        {
            Edit,
            Create,
            Err
        }
        //#endregion
        private eStaicBlockMode _mode = eStaicBlockMode.Create;

        private int _staticBlockId = -1;

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
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_PagePart_Create));

            if (string.IsNullOrEmpty(Request["id"]) || Request["id"].ToLower() == "addnew")
            {
                _mode = eStaicBlockMode.Create;
                btnAdd.Text = Resource.Admin_m_Category_Add;
            }
            else
            {
                Int32.TryParse(Request["id"], out _staticBlockId);
                if (_staticBlockId != -1)
                {
                    _mode = eStaicBlockMode.Edit;
                    btnAdd.Text = Resource.Admin_m_Category_Save;
                }
                if (_staticBlockId == -1) _mode = eStaicBlockMode.Err;
            }
            if (!IsPostBack)
            {
                if (_mode == eStaicBlockMode.Edit)
                {
                    LoadStaticBlock();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (_mode == eStaicBlockMode.Edit)
            {
                if (lblError.Visible == false)
                {
                    SaveStaticBlock();

                    if (lblError.Visible == false)
                    {
                        CommonHelper.RegCloseScript(this, string.Empty);
                    }
                }
            }
            else if (_mode == eStaicBlockMode.Create)
            {
                int staicPageId = CreateStaticBlock();
                if (staicPageId != 0 && lblError.Visible == false)
                {
                    CommonHelper.RegCloseScript(this, "'StaticBlocks.aspx';");
                }
            }
        }

        private void LoadStaticBlock()
        {
            try
            {
                var part = StaticBlockService.GetPagePart(_staticBlockId);
                txtKey.Text = part.Key;
                txtPageTitle.Text = part.InnerName;
                CKEditorControl1.Text = part.Content;
                chbEnabled.Checked = part.Enabled;
                lblSubHead.Text = Resource.Admin_PagePart_Edit;

                if (part.Key == "LeftBottom") {
                    lCopyrightMessage.Text = "<br/>Данный блок будет отображаться в левой части подвала сайта, только если отключена опция \"Показывать Copyright\" в общих настройках магазина." +
                                             "<br/><br/>Пункт меню \"Настройки - Общие\", опция \"Показывать Copyright\" - Отключить.";
                    lCopyrightMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " at LoadStaticBlock");
                //Debug.Log.Error(ex, _staticBlockId);
                Debug.Log.Error("at LoadStaticBlock", ex);
            }
        }

        private void SaveStaticBlock()
        {
            if (_mode == eStaicBlockMode.Err)
            {
                return;
            }

            if (!ValidateInput())
            {
                return;
            }

            var part = new StaticBlock(_staticBlockId)
                {
                    Content = CKEditorControl1.Text,
                    InnerName = txtPageTitle.Text,
                    Key = txtKey.Text,
                    Enabled = chbEnabled.Checked
                };

            if (!StaticBlockService.UpdatePagePart(part))
                MsgErr("Failed to save page part");
        }

        private int CreateStaticBlock()
        {
            // Validation
            MsgErr(true);

            if (!ValidateInput())
            {
                return 0;
            }

            var part = new StaticBlock
                {
                    Content = CKEditorControl1.Text,
                    InnerName = txtPageTitle.Text,
                    Key = txtKey.Text,
                    Enabled = chbEnabled.Checked
                };
            var id = StaticBlockService.AddStaticBlock(part);
            if (id != 0)
            {
                return id;
            }
            return 0;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                MsgErr(Resource.Admin_PageParts_KeyRequired);
                return false;
            }
            if (StaticBlockService.IsPagePartKeyExist(_staticBlockId, txtKey.Text))
            {
                MsgErr(Resource.Admin_PageParts_KeyDuplicate);
                return false;
            }
            return true;
        }
    }
}