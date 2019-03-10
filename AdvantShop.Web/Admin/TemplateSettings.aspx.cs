//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using Resources;

namespace Admin
{
    public partial class TemplateSettings : AdvantShopAdminPage
    {
        #region Error
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
            lblError.Text = @"<br/>" + messageText + @"<br/>";
        }
        #endregion

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadSettings();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_TemplateSettings_Title));
            MsgErr(true);
            lblInfo.Text = string.Empty;

            OutDiv.Visible = CommonStatistic.IsRun;
            linkCancel.Visible = CommonStatistic.IsRun;
        }

        private void LoadSettings()
        {
            pnlDesignSettings.Controls.Clear();

            var settingsBox = TemplateSettingsProvider.GetTemplateSettingsBox();

            if (settingsBox.Message.IsNotEmpty())
            {
                MsgErr(settingsBox.Message);
                btnSave.Visible = false;
                return;
            }

            if (settingsBox.Settings.Count == 0)
            {
                MsgErr(Resource.Admin_TemplateSettings_ErrorReadConfig);
                btnSave.Visible = false;
                return;
            }

            string sectionName = "";

            foreach (var setting in settingsBox.Settings)
            {
                var rowDiv = new HtmlGenericControl("div");
                rowDiv.Attributes.Add("class", "tpl-settings-row");

                if (setting.SectionName != sectionName)
                {
                    var rowDivSection = new HtmlGenericControl("div");
                    rowDivSection.Attributes.Add("class", "tpl-setting-section");
                    rowDivSection.Controls.Add(new Label() { Text = setting.SectionName });
                    rowDiv.Controls.Add(rowDivSection);
                    sectionName = setting.SectionName;
                }

                var rowTitleDiv = new HtmlGenericControl("div");
                rowTitleDiv.Attributes.Add("class", "tpl-settings-title");
                rowTitleDiv.Controls.Add(new Label() { Text = setting.Title });

                var rowControlDiv = new HtmlGenericControl("div");
                rowControlDiv.Attributes.Add("class", "tpl-settings-control");

                switch (setting.Type)
                {
                    case "Checkbox":
                        var chk = new HtmlInputCheckBox()
                        {
                            ID = setting.Name,
                            Checked = Convert.ToBoolean(setting.Value),
                            ClientIDMode = System.Web.UI.ClientIDMode.Static
                        };
                        chk.Attributes.Add("datatype", "bool");
                        rowControlDiv.Controls.Add(chk);
                        break;

                    case "TextBox":
                        var txt = new TextBox()
                        {
                            ID = setting.Name,
                            Text = setting.Value,
                            ClientIDMode = System.Web.UI.ClientIDMode.Static
                        };
                        txt.Attributes.Add("datatype", setting.DataType);
                        rowControlDiv.Controls.Add(txt);
                        break;

                    case "MultiLineTextBox":
                        var mtxt = new TextBox()
                        {
                            ID = setting.Name,
                            Text = setting.Value,
                            TextMode = TextBoxMode.MultiLine,
                            Width = 300,
                            Height = 30,
                            ClientIDMode = System.Web.UI.ClientIDMode.Static
                        };
                        mtxt.Attributes.Add("datatype", "string");
                        rowControlDiv.Controls.Add(mtxt);
                        break;

                    case "DropDownList":
                        var ddl = new DropDownList()
                        {
                            ID = setting.Name,
                            Width = 300,
                            ClientIDMode = System.Web.UI.ClientIDMode.Static
                        };
                        foreach (var option in setting.Options)
                        {
                            ddl.Items.Add(new ListItem(option.Title, option.Value));
                        }
                        ddl.SelectedValue = setting.Value;
                        ddl.Attributes.Add("datatype", "string");
                        rowControlDiv.Controls.Add(ddl);
                        break;
                }

                rowDiv.Controls.Add(rowTitleDiv);
                rowDiv.Controls.Add(rowControlDiv);

                if (setting.Description.IsNotEmpty())
                {
                    var rowDescriptionDiv = new HtmlGenericControl("div");
                    rowDescriptionDiv.Attributes.Add("class", "help-block");
                    rowDescriptionDiv.Attributes.Add("data-plugin", "help");
                    rowDescriptionDiv.InnerHtml = RenderHelpDescription(setting.Title, setting.Description);
                    rowDiv.Controls.Add(rowDescriptionDiv);
                }

                pnlDesignSettings.Controls.Add(rowDiv);
            }
        }

        private string RenderHelpDescription(string title, string description)
        {
            return string.Format(@"<div class='help-icon js-help-icon'></div>
                <article class='bubble help js-help'>
                    <header class='help-header'>
                        {0}
                    </header>
                    <div class='help-content'>
                        {1}
                    </div>
                </article>", title, description);
        }

        #region ResizePhoto

        protected void btnResizePhoto_Click(object sender, EventArgs e)
        {
            if (CommonStatistic.IsRun) return;

            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = Request.Url.PathAndQuery;
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnSave.Visible = false;
            try
            {
                CommonStatistic.TotalRow = PhotoService.GetCountPhotos(0, PhotoType.Product);
                CommonStatistic.StartNew(Resize);
                //CommonStatistic.ThreadImport = new Thread(Resize) { IsBackground = true };
                //CommonStatistic.ThreadImport.Start();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                lError.Text = ex.Message;
                lError.Visible = true;
            }
        }


        //get file withous lock file
        public static System.Drawing.Image ImageFromFile(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            using (var lStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                return System.Drawing.Image.FromStream(lStream);
        }

        protected void Resize()
        {
            foreach (var photo in PhotoService.GetNamePhotos(0, PhotoType.Product, true))
            {
                try
                {
                    var originalPath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Original, photo);
                    if (!File.Exists(originalPath))
                    {
                        var bigPath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photo);
                        if (File.Exists(bigPath))
                        {
                            File.Copy(bigPath, originalPath);
                        }
                    }

                    if (File.Exists(originalPath))
                    {
                        using (var image = ImageFromFile(originalPath))
                        {
                            FileHelpers.SaveProductImageUseCompress(photo, image, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    CommonStatistic.TotalErrorRow++;
                }
                CommonStatistic.RowPosition++;
            }

            CommonStatistic.IsRun = false;
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            //if (!CommonStatistic.ThreadImport.IsAlive) return;
            CommonStatistic.IsRun = false;
        }

        #endregion

        protected void btnSetDefaultSettings_Click(object sender, EventArgs e)
        {
            TemplateSettingsProvider.SetTemplateSettings(setDefault:true);
        }
    }
}