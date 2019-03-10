using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Scheduler.Jobs;
using AdvantShop.ExportImport;

using Resources;
using System.Security.Cryptography;
using System.Text;

namespace AdvantShop.Admin.UserControls.ExportFeedUc
{
    public partial class ExportFeedSettingsTab : System.Web.UI.UserControl
    {

        public static string PhysicalAppPath { get; set; }

        public static string FileName { get; set; }

        private ExportFeedControl _ucFeedSettings;

        public ExportFeed CurrentExportFeed { get; set; }
        public ExportFeedSettings ExportFeedSettings { get; set; }

        public int ExportFeedId
        {
            get
            {
                var id = 0;
                int.TryParse(Request["feedId"], out id);
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentExportFeed == null)
            {
                return;
            }

            LoadFeedData();

            PhysicalAppPath = Request.PhysicalApplicationPath;
            ltrlFileName.Text = Resource.Admin_ExportFeed_Salt;
        }

        private ListItem[] LoadFileExtention(EExportFeedType moduleName)
        {
            switch (moduleName)
            {
                case EExportFeedType.YandexMarket:
                    return new[] { new ListItem("xml", "xml"), new ListItem("yml", "yml") };
                case EExportFeedType.GoogleMerchentCenter:
                    return new[] { new ListItem("xml", "xml") };
                case EExportFeedType.Csv:
                    return new[] { new ListItem("csv", "csv"), new ListItem("txt", "txt") };
                case EExportFeedType.Reseller:
                    return new[] { new ListItem("csv", "csv") };
                default:
                    return new[] { new ListItem("xml", "xml") };
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valid())
            {
                if (txtFileName.Text.Length > 50)
                    lblSaveMessage.Text = Resource.Admin_ExportFeed_ErrorLengthFileName;
                else
                    lblSaveMessage.Text = Resource.Admin_ExportFeed_WrongData;
                lblSaveMessage.Text = Resource.Admin_ExportFeed_WrongData;
                lblSaveMessage.ForeColor = System.Drawing.Color.Red;
                lblSaveMessage.Visible = true;
                return;
            }

            var exportFeed = ExportFeedService.GetExportFeed(ExportFeedId);

            exportFeed.Name = txtExportFeedName.Text;
            FileName = txtFileName.Text;
            exportFeed.Description = txtExportFeedDescription.Text;

            ExportFeedService.UpdateExportFeed(exportFeed);


            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeed.Id);
            commonSettings.AdvancedSettings = _ucFeedSettings.GetData();


            var invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            invalid = invalid.Replace("/", "");
            txtFileName.Text = invalid.Aggregate(txtFileName.Text, (current, c) => current.Replace(c.ToString(), ""));            

            commonSettings.FileName = txtFileName.Text;
            commonSettings.FileExtention = ddlFileExtention.SelectedValue;
            commonSettings.PriceMargin = Convert.ToSingle(txtPriceMargin.Text);
            //commonSettings.ExportNotActiveProducts = ckbExportNotActive.Checked;
            //commonSettings.ExportNotAmountProducts = ckbExportNotAmount.Checked;

            commonSettings.Active = ckbExportFeedActive.Checked;
            commonSettings.Interval = Convert.ToInt32(txtTimeInterval.Text);
            if (ddlIntervalType.SelectedValue == TimeIntervalType.Days.ToString())
            {
                commonSettings.JobStartTime = new DateTime(2000, 1, 1, Convert.ToInt32(txtHours.Text), Convert.ToInt32(txtMinutes.Text), 0);
            }
            commonSettings.IntervalType = ddlIntervalType.SelectedValue.TryParseEnum<TimeIntervalType>();

            ExportFeedSettingsProvider.SetSettings(exportFeed.Id, commonSettings);


            if (!string.Equals(commonSettings.FileName + "." + commonSettings.FileExtention,
                ExportFeedSettings.FileName + "." + ExportFeedSettings.FileExtention)
                && File.Exists(Server.MapPath("~/" + ExportFeedSettings.FileName + "." + ExportFeedSettings.FileExtention)))
            {
                File.Delete(Server.MapPath("~/" + ExportFeedSettings.FileName + "." + ExportFeedSettings.FileExtention));
            }

            lblSaveMessage.Text = Resource.Admin_ExportFeed_SettingsSaved;
            lblSaveMessage.ForeColor = System.Drawing.Color.Blue;
            lblSaveMessage.Visible = true;

            var item = new TaskSetting
            {
                Enabled = commonSettings.Active,
                JobType = typeof(GenerateExportFeedJob).ToString(),
                TimeInterval = commonSettings.Interval,
                TimeHours = ddlIntervalType.SelectedValue == TimeIntervalType.Days.ToString() ? commonSettings.JobStartTime.Hour : 0,
                TimeMinutes = ddlIntervalType.SelectedValue == TimeIntervalType.Days.ToString() ? commonSettings.JobStartTime.Minute : 0,
                TimeType = commonSettings.IntervalType,
                DataMap = ExportFeedId
            };

            var settings = TaskSettings.ExportFeedSettings;

            var setting = settings.FirstOrDefault(x => x.JobType == item.JobType && Convert.ToInt32(x.DataMap) == Convert.ToInt32(item.DataMap));
            if (setting != null)
            {
                settings.Remove(setting);
                settings.Add(item);
            }
            else
            {
                settings.Add(item);
            }

            TaskSettings.ExportFeedSettings = settings;
            TaskManager.TaskManagerInstance().ManagedTask(settings);
        }

        protected bool Valid()
        {

            var valid = !string.IsNullOrEmpty(txtFileName.Text);
            txtFileName.CssClass = string.IsNullOrEmpty(txtFileName.Text)
                ? "niceTextBox_faild shortTextBoxClass"
                : "niceTextBox shortTextBoxClass";

            var priceMargin = 0f;
            valid &= !string.IsNullOrEmpty(txtPriceMargin.Text) && float.TryParse(txtPriceMargin.Text, out priceMargin);
            txtPriceMargin.CssClass = string.IsNullOrEmpty(txtPriceMargin.Text) || !float.TryParse(txtPriceMargin.Text, out priceMargin)
                ? "niceTextBox_faild shortTextBoxClass"
                : "niceTextBox shortTextBoxClass";

            valid &= !ckbExportFeedActive.Checked || ddlIntervalType.SelectedValue != TimeIntervalType.None.ToString();

            var interval = 0;
            valid &= !string.IsNullOrEmpty(txtTimeInterval.Text) && int.TryParse(txtTimeInterval.Text, out interval);
            txtTimeInterval.CssClass = string.IsNullOrEmpty(txtTimeInterval.Text) || !int.TryParse(txtTimeInterval.Text, out interval)
                ? "niceTextBox_faild shortTextBoxClass"
                : "niceTextBox shortTextBoxClass";

            valid &= !ckbExportFeedActive.Checked || interval != 0;

            if (ddlIntervalType.SelectedValue == TimeIntervalType.Days.ToString())
            {
                var hours = 0;
                var minutes = 0;

                valid &= !string.IsNullOrEmpty(txtHours.Text) && int.TryParse(txtHours.Text, out hours) &&
                         !string.IsNullOrEmpty(txtMinutes.Text) && int.TryParse(txtMinutes.Text, out minutes) &&
                         !string.IsNullOrEmpty(txtTimeInterval.Text) && int.TryParse(txtTimeInterval.Text, out interval);

                txtHours.CssClass = string.IsNullOrEmpty(txtHours.Text) || !int.TryParse(txtHours.Text, out hours)
                      ? "niceTextBox_faild shortTextBoxClass"
                      : "niceTextBox shortTextBoxClass";
                txtMinutes.CssClass = string.IsNullOrEmpty(txtMinutes.Text) || !int.TryParse(txtMinutes.Text, out hours)
                      ? "niceTextBox_faild shortTextBoxClass"
                      : "niceTextBox shortTextBoxClass";
            }

            var isExistFile = ExportFeedService.IsExistFile(ExportFeedId, txtFileName.Text,
                ddlFileExtention.SelectedValue);
            valid &= !isExistFile;

            lblFileNameMessage.Visible = isExistFile;

            if (isExistFile)
            {
                txtFileName.CssClass = "niceTextBox_faild shortTextBoxClass";
            }

            return valid;
        }

        private void LoadFeedData()
        {

            if (ExportFeedSettings == null)
            {
                return;
            }

            if (!IsPostBack)
            {
                ddlIntervalType.Items.Clear();
                ddlIntervalType.Items.Add(new ListItem { Text = Resource.Admin_CommonSettings_NotSelected, Value = TimeIntervalType.None.ToString() });
                ddlIntervalType.Items.Add(new ListItem { Text = Resource.Admin_CommonSettings_InDays, Value = TimeIntervalType.Days.ToString() });
                ddlIntervalType.Items.Add(new ListItem { Text = Resource.Admin_CommonSettings_InHours, Value = TimeIntervalType.Hours.ToString() });

                txtExportFeedName.Text = CurrentExportFeed.Name;
                txtExportFeedDescription.Text = CurrentExportFeed.Description;

                ckbExportFeedActive.Checked = ExportFeedSettings.Active;

                ddlIntervalType.SelectedValue = ExportFeedSettings.IntervalType.ToString();
                txtTimeInterval.Text = ExportFeedSettings.Interval.ToString();
                txtHours.Text = ExportFeedSettings.JobStartTime.Hour.ToString();
                txtMinutes.Text = ExportFeedSettings.JobStartTime.Minute.ToString();

                txtFileName.Text = ExportFeedSettings.FileName;
                txtPriceMargin.Text = ExportFeedSettings.PriceMargin.ToString();
                ddlFileExtention.Items.AddRange(LoadFileExtention(CurrentExportFeed.Type));
                ddlFileExtention.SelectedValue = ExportFeedSettings.FileExtention;
                //ckbExportNotActive.Checked = ExportFeedSettings.ExportNotActiveProducts;
                //ckbExportNotAmount.Checked = ExportFeedSettings.ExportNotAmountProducts;

            }

            switch (CurrentExportFeed.Type)
            {
                case EExportFeedType.YandexMarket:
                    _ucFeedSettings =
                          (ExportFeedControl)
                              Page.LoadControl("~/admin/usercontrols/exportfeed/ExportFeedYandexSettingsUc.ascx");

                    break;

                case EExportFeedType.GoogleMerchentCenter:
                    _ucFeedSettings =
                         (ExportFeedControl)
                             Page.LoadControl("~/admin/usercontrols/exportfeed/ExportFeedGoogleSettingsUc.ascx");

                    break;
                case EExportFeedType.Csv:
                    _ucFeedSettings =
                        (ExportFeedControl)
                            Page.LoadControl("~/admin/usercontrols/exportfeed/ExportFeedCsvSettingsUc.ascx");
                    break;
                case EExportFeedType.Reseller:
                    _ucFeedSettings =
                        (ExportFeedControl)
                            Page.LoadControl("~/admin/usercontrols/exportfeed/ExportFeedResellerSettingsUc.ascx");
                    break;
            }

            pnlAdditionalSettings.Controls.Add(_ucFeedSettings);

            lShopUrl.Text = string.Format("{0}/", SettingsMain.SiteUrl);
        }
    }
}