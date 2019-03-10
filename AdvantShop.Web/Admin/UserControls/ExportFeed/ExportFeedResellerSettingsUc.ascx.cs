using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;

using Newtonsoft.Json;

namespace AdvantShop.Admin.UserControls.ExportFeedUc
{
    public partial class ExportFeedResellerSettingsUc : ExportFeedControl
    {
        protected int ExportFeedId;

        protected List<ProductFields> FieldMapping = new List<ProductFields>();
        protected List<CSVField> ModuleFieldMapping = new List<CSVField>();
        
        protected string CategorySort = "categorySort";

        private enum SelectState
        {
            None,
            Deselect,
            Select
        }

        private SelectState State
        {
            // при постбэке не учитываем state из параметров
            get { return IsPostBack ? SelectState.None : Request["state"].TryParseEnum<SelectState>(); }
        }

        /// <summary>
        /// Формирует таблицу для выбора полей
        /// </summary>

        protected void Page_Init(object sender, EventArgs e)
        {
            var resellerExportFeedSettings = new ExportFeedResellerOptions();
            ExportFeedId = 0;
            if (!string.IsNullOrEmpty(Request["feedid"]) && int.TryParse(Request["feedid"], out ExportFeedId) && !IsPostBack)
            {
                var  commonSettings = ExportFeedSettingsProvider.GetSettings(ExportFeedId);
                resellerExportFeedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);

                FieldMapping = resellerExportFeedSettings.FieldMapping ?? new List<ProductFields>();
                ModuleFieldMapping = resellerExportFeedSettings.ModuleFieldMapping ?? new List<CSVField>();
                lblResellerCode.Text = resellerExportFeedSettings.ResellerCode;
                txtRecommendedPriceMargin.Text = resellerExportFeedSettings.RecomendedPriceMargin.ToString();
            }
            CommonHelper.DisableBrowserCache();
        }

        public override string GetData()
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(ExportFeedId);
            var advancedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);

            //var settings = ExportFeedSettingsProvider.GetSettings<ExportFeedResellerOptions>(ExportFeedId);
            advancedSettings.RecomendedPriceMargin = Convert.ToSingle(txtRecommendedPriceMargin.Text);
            advancedSettings.CsvCategorySort = true;
            advancedSettings.FileName = ExportFeedSettingsTab.FileName;
            return JsonConvert.SerializeObject(advancedSettings);
        }
    }
}