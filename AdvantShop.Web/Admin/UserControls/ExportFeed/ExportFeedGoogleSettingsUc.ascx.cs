using System;
using System.Linq;
using System.Web.UI.WebControls;

using AdvantShop.Core.Controls;
using AdvantShop.ExportImport;
using AdvantShop.Repository.Currencies;
using Newtonsoft.Json;

namespace AdvantShop.Admin.UserControls.ExportFeedUc
{
    public partial class ExportFeedGoogleSettingsUc : ExportFeedControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var exportFeedId = 0;
            if (!string.IsNullOrEmpty(Request["feedid"]) && int.TryParse(Request["feedid"], out exportFeedId))
            {
                var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
                var googleExportFeed = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedGoogleMerchantCenterOptions>(commonSettings.AdvancedSettings);               

                txtUrlTags.Text = googleExportFeed.AdditionalUrlTags;
                txtDatafeedTitle.Text = googleExportFeed.DatafeedTitle;
                txtDatafeedDescription.Text = googleExportFeed.DatafeedDescription;
                txtGoogleProductCategory.Text = googleExportFeed.GoogleProductCategory;
                chbRemoveHTML.Checked = googleExportFeed.RemoveHtml;
                ddlProductDescriptionType.SelectedValue = googleExportFeed.ProductDescriptionType;
                chbExportNotAvailable.Checked = googleExportFeed.ExportNotAvailable;
                chbAddedColorAndSizeForName.Checked = googleExportFeed.ColorSizeToName;

                ddlOfferIdType.SelectedValue = googleExportFeed.OfferIdType;

                CurrencyListBox.Items.Clear();

                foreach (var item in CurrencyService.GetAllCurrencies())
                {
                    CurrencyListBox.Items.Add(new ListItem { Text = item.Name, Value = item.Iso3 });
                }

                string selectCurrency = googleExportFeed.Currency;
                if (selectCurrency != null)
                {
                    CurrencyListBox.SelectedValue = selectCurrency;
                }

                lblUrlTagsNote.Text = (new ExportFeedGoogleMerchantCenter()).GetAvailableVariables().Aggregate(string.Empty, (current, item) => current + (item + ", "));
            }
        }

        public override string GetData()
        {
            return JsonConvert.SerializeObject(new ExportFeedGoogleMerchantCenterOptions()
            {
                AdditionalUrlTags = txtUrlTags.Text,
                DatafeedTitle = txtDatafeedTitle.Text,
                DatafeedDescription = txtDatafeedDescription.Text,
                GoogleProductCategory = txtGoogleProductCategory.Text,
                RemoveHtml = chbRemoveHTML.Checked,
                Currency = CurrencyListBox.SelectedValue,
                ProductDescriptionType = ddlProductDescriptionType.SelectedValue,
                OfferIdType = ddlOfferIdType.SelectedValue,
                ExportNotAvailable = chbExportNotAvailable.Checked,
                ColorSizeToName = chbAddedColorAndSizeForName.Checked
                //AllowPreOrderProducts = chbAlowPreOrder.Checked
            });
        }
    }
}