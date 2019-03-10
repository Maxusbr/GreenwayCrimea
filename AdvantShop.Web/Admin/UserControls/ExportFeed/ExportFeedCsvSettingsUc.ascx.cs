using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;

using Newtonsoft.Json;

namespace AdvantShop.Admin.UserControls.ExportFeedUc
{
    public partial class ExportFeedCsvSettingsUc : ExportFeedControl
    {
        protected int ExportFeedId;

        protected List<ProductFields> FieldMapping = new List<ProductFields>();
        protected List<CSVField> ModuleFieldMapping = new List<CSVField>();

        protected string CategorySort = "categorySort";

        protected void Page_Load(object sender, EventArgs e)
        {
            ExportFeedCsvOptions csvExportFeedSettings = new ExportFeedCsvOptions();
            ExportFeedId = 0;
            if (!string.IsNullOrEmpty(Request["feedid"]) && int.TryParse(Request["feedid"], out ExportFeedId) && !IsPostBack)
            {
                var commonSettings = ExportFeedSettingsProvider.GetSettings(ExportFeedId);
                csvExportFeedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedCsvOptions>(commonSettings.AdvancedSettings);
                
                ChbCategorySort.Checked = csvExportFeedSettings.CsvCategorySort;

                ddlEncoding.Items.Clear();
                foreach (EncodingsEnum enumItem in Enum.GetValues(typeof(EncodingsEnum)))
                {
                    ddlEncoding.Items.Add(new ListItem(enumItem.ToString(), enumItem.StrName()));
                }
                ddlEncoding.SelectedValue = csvExportFeedSettings.CsvEnconing;

                ddlSeparators.Items.Clear();
                foreach (SeparatorsEnum enumItem in Enum.GetValues(typeof(SeparatorsEnum)))
                {
                    ddlSeparators.Items.Add(new ListItem(enumItem.Localize(), enumItem.StrName()));
                }

                if (ddlSeparators.Items.FindByValue(csvExportFeedSettings.CsvSeparator) != null)
                {
                    ddlSeparators.SelectedValue = csvExportFeedSettings.CsvSeparator;
                }
                else
                {
                    ddlSeparators.SelectedValue = SeparatorsEnum.Custom.StrName();
                    txtCustomSeparator.Text = csvExportFeedSettings.CsvSeparator;
                }

                txtColumSeparator.Text = csvExportFeedSettings.CsvColumSeparator;
                txtPropertySeparator.Text = csvExportFeedSettings.CsvPropertySeparator;
                chbCsvExportNoInCategory.Checked = csvExportFeedSettings.CsvExportNoInCategory;
                FieldMapping = csvExportFeedSettings.FieldMapping ?? new List<ProductFields>();
                ModuleFieldMapping = csvExportFeedSettings.ModuleFieldMapping ?? new List<CSVField>();
            }
            CommonHelper.DisableBrowserCache();
        }

        public override string GetData()
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(ExportFeedId);
            var advancedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedCsvOptions>(commonSettings.AdvancedSettings);

            if (ChbCategorySort.Checked)
            {
                var ind = advancedSettings.FieldMapping.IndexOf(ProductFields.Category);
                if (ind > 0)
                    advancedSettings.FieldMapping.Insert(ind + 1, ProductFields.Sorting);
                else
                    advancedSettings.FieldMapping.Add(ProductFields.Sorting);
            }
            else
            {
                advancedSettings.FieldMapping.Remove(ProductFields.Sorting);
            }
                                    
            advancedSettings.CsvEnconing = ddlEncoding.SelectedValue;
            advancedSettings.CsvSeparator = ddlSeparators.SelectedValue == SeparatorsEnum.Custom.StrName()
                ? txtCustomSeparator.Text
                : ddlSeparators.SelectedValue;
            advancedSettings.CsvColumSeparator = txtColumSeparator.Text;
            advancedSettings.CsvPropertySeparator = txtPropertySeparator.Text;
            advancedSettings.CsvExportNoInCategory = chbCsvExportNoInCategory.Checked;
            advancedSettings.CsvCategorySort = ChbCategorySort.Checked;

            return JsonConvert.SerializeObject(advancedSettings);

        }
    }
}