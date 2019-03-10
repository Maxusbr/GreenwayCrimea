//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;

using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Scheduler.Jobs;
using AdvantShop.ExportImport;
using Resources;
using AdvantShop.Core.Common.Attributes;

namespace Admin
{
    public partial class ExportFeedPage : AdvantShopAdminPage
    {
        protected ExportFeed CurrentExportFeed;
        protected ExportFeedSettings _exportFeedSettings;
        public int ExportFeedId
        {
            get
            {
                var id = 0;
                int.TryParse(Request["feedId"], out id);
                return id;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (ExportFeedId == 0)
            {
                GotoFirstExportFeed();
                return;
            }

            CurrentExportFeed = ExportFeedService.GetExportFeed(ExportFeedId);
            if (CurrentExportFeed == null)
            {
                GotoFirstExportFeed();
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_MasterPageAdminCatalog_Catalog));

            if (CurrentExportFeed == null)
            {
                GotoFirstExportFeed();
                //return;
            }

            LoadFeedData();

            exportFeedCatalogTab.CurrentExportFeed = CurrentExportFeed;
            exportFeedCatalogTab.ExportFeedSettings = _exportFeedSettings;

            exportFeedSettingsTab.CurrentExportFeed = CurrentExportFeed;
            exportFeedSettingsTab.ExportFeedSettings = _exportFeedSettings;
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(CurrentExportFeed == null)
            {
                pnlMain.Visible = false;
                pnlEmpty.Visible = true;
                return;
            }

            btnExport.Attributes["href"] = "exportfeedprogress.aspx?feedid=" + ExportFeedId + TypeParametr();

            var type = ReflectionExt.GetTypeByAttributeValue<ExportFeedKeyAttribute>(typeof(BaseExportFeed), atr => atr.Value, CurrentExportFeed.Type.ToString());
            var currentExportFeed = (BaseExportFeed)Activator.CreateInstance(type);
            
            if (currentExportFeed.GetProductsCount(CurrentExportFeed.Id) == 0
                && currentExportFeed.GetCategoriesCount(CurrentExportFeed.Id) == 0)
            {
                btnExport.Attributes.Add("disabled", "disabled");
                btnExport.Attributes["style"] += "pointer-events: none;";
            }
            else
            {
                btnExport.Attributes.Remove("disabled");
                btnExport.Attributes["style"] = btnExport.Attributes["style"].Replace("pointer-events: none;", "");
            }
        }


        private void GotoFirstExportFeed()
        {
            var exportFeedType = !string.IsNullOrEmpty(Request["type"])
                ? Request["type"].TryParseEnum<EExportFeedType>()
                : EExportFeedType.None;

            var exportFeedFirst = exportFeedType == EExportFeedType.None
                ? ExportFeedService.GetExportFeedFirst()
                : ExportFeedService.GetExportFeedFirstByType(exportFeedType);

            if (exportFeedFirst != null)
            {
                Response.Redirect("exportfeed.aspx?feedid=" + exportFeedFirst.Id + TypeParametr(), true);
                return;
            }
            else
            {
                pnlMain.Visible = false;
                pnlEmpty.Visible = true;
                return;
            }
        }

        private void LoadFeedData()
        {
            if (CurrentExportFeed == null)
            {
                return;
            }
            
            _exportFeedSettings = ExportFeedSettingsProvider.GetSettings(CurrentExportFeed.Id);
                        
            if (_exportFeedSettings == null)
            {
                return;
            }

          if (!string.IsNullOrEmpty(CurrentExportFeed.LastExportFileFullName) && 
                File.Exists(CurrentExportFeed.LastExportFileFullName))
            {
                if (CurrentExportFeed.Type == EExportFeedType.Reseller && _exportFeedSettings.AdvancedSettings != null)
                {
                    var advancedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedResellerOptions>(_exportFeedSettings.AdvancedSettings);
                    lnkFile.Text = SettingsMain.SiteUrl + "/api/resellers/catalog?id=" + advancedSettings.ResellerCode;
                    lnkFile.NavigateUrl = SettingsMain.SiteUrl + "/api/resellers/catalog?id=" + advancedSettings.ResellerCode;
                }
                else
                {
                    lnkFile.Text = SettingsMain.SiteUrl + "/" + _exportFeedSettings.FileFullName;
                    lnkFile.NavigateUrl = SettingsMain.SiteUrl + "/" + _exportFeedSettings.FileFullName + "?rnd=" + (new Random()).Next();
                }
            }
            else
            {
                blockFileLink.Visible = false;
            }


            lblRightHead.Text = CurrentExportFeed.Name;
            ltrlExportTypeName.Text = CurrentExportFeed.Type.Localize();
            ltrlAdditionalInfo.Text = CurrentExportFeed.LastExport != null
                ? CurrentExportFeed.LastExport.ToString()
                : Resource.Admin_ExportFeed_NotExports;

            RenderBlockInstruction();
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            if (ExportFeedId == 0)
            {
                return;
            }

            //var exportFeedSettings = ExportFeedSettingsProvider.GetSettings<ExportFeedSettings>(ExportFeedId);
            //if (exportFeedSettings != null)
            //{
            //    var filePath = Server.MapPath("~/" + exportFeedSettings.FileName + "." + exportFeedSettings.FileExtention);
            //    if (File.Exists(filePath))
            //    {
            //        File.Delete(filePath);
            //    }
            //}

            // delete tasks
            var settings = TaskSettings.ExportFeedSettings;
            var jobType = typeof(GenerateExportFeedJob).ToString();

            var setting = settings.Find(x => x.JobType == jobType && x.DataMap != null && Convert.ToInt32(x.DataMap) == ExportFeedId);
            if (setting != null)
            {
                settings.Remove(setting);
                TaskSettings.ExportFeedSettings = settings;
            }

            ExportFeedService.DeleteExportFeed(ExportFeedId);

            Response.Redirect("ExportFeed.aspx");
        }

        protected void RenderBlockInstruction()
        {
            if (CurrentExportFeed == null)
            {
                return;
            }

            switch (CurrentExportFeed.Type)
            {
                case EExportFeedType.Csv:
                    linkHelp.NavigateUrl = "http://www.advantshop.net/help/pages/import-csv";
                    linkHelp.Text = @"Инструкция. Импорт и экспорт данных в формате CSV (Excel)";
                    break;
                case EExportFeedType.Reseller:
                    linkHelp.NavigateUrl = "http://www.advantshop.net/help/pages/import-csv";
                    linkHelp.Text = @"Инструкция. Импорт и экспорт данных в формате CSV (Excel)";
                    break;
                case EExportFeedType.YandexMarket:
                    linkHelp.NavigateUrl = "http://www.advantshop.net/help/pages/export-yandex-market";
                    linkHelp.Text = @"Инструкция. Выгрузка товаров в Яндекс.Маркет";
                    break;
                case EExportFeedType.GoogleMerchentCenter:
                    blockHelp.Visible = false;
                    break;
                default:
                    blockHelp.Visible = false;
                    break;

            }
        }

        protected string TypeParametr()
        {
            return !string.IsNullOrEmpty(Request["type"]) ? "&type=" + Request["type"] : string.Empty;
        }
    }
}