using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.ExportImport;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;

namespace AdvantShop.Admin.UserControls.ExportFeedUc
{
    public partial class ExportFeedListUc : System.Web.UI.UserControl
    {
        protected int StatusId = 0;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var exportFeedType = EExportFeedType.None;
            if (!string.IsNullOrEmpty(Request["type"]) && Enum.TryParse(Request["type"], true, out exportFeedType))
            {
                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExportFeeds &&
                    (exportFeedType == EExportFeedType.YandexMarket || exportFeedType == EExportFeedType.GoogleMerchentCenter))
                {
                    Response.Redirect("NotInTariff.aspx");
                    return;
                }

                lvExportFeeds.DataSource = ExportFeedService.GetExportFeeds(exportFeedType);
                lvExportFeeds.DataBind();

                ddlExportFeedType.Items.Add(new ListItem(exportFeedType.Localize(), exportFeedType.ToString()));
            }
            else
            {
                var exportFeeds = ExportFeedService.GetExportFeeds();
                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExportFeeds)
                {
                    exportFeeds = exportFeeds.Where(item => item.Type != EExportFeedType.YandexMarket && item.Type != EExportFeedType.GoogleMerchentCenter).ToList();
                }

                lvExportFeeds.DataSource = exportFeeds;
                lvExportFeeds.DataBind();

                ddlExportFeedType.Items.Add(new ListItem(EExportFeedType.Csv.Localize(), EExportFeedType.Csv.ToString()));
                ddlExportFeedType.Items.Add(new ListItem(EExportFeedType.GoogleMerchentCenter.Localize(), EExportFeedType.GoogleMerchentCenter.ToString()));
                ddlExportFeedType.Items.Add(new ListItem(EExportFeedType.YandexMarket.Localize(), EExportFeedType.YandexMarket.ToString()));

                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExportFeeds)
                {
                    ddlExportFeedType.Items.FindByValue(EExportFeedType.YandexMarket.ToString()).Attributes.Add("disabled", "disabled");
                    ddlExportFeedType.Items.FindByValue(EExportFeedType.GoogleMerchentCenter.ToString()).Attributes.Add("disabled", "disabled");
                }
            }
        }

        protected void btnAddFeed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtExportFeedName.Text))
            {
                return;
            }

            var exportFeedId = ExportFeedService.AddExportFeed(new ExportFeed
            {
                Name = txtExportFeedName.Text,
                Type = ddlExportFeedType.SelectedValue.TryParseEnum<EExportFeedType>(),
                Description = txtExportFeedDescription.Text
            });

            ExportFeedService.InsertCategory(exportFeedId, 0);

            switch (ddlExportFeedType.SelectedValue.TryParseEnum<EExportFeedType>())
            {
                case EExportFeedType.YandexMarket:
                    ExportFeedSettingsProvider.SetSettings(exportFeedId, new ExportFeedYandexOptions
                    {
                        PriceMargin = 0,
                        FileName = System.IO.File.Exists(Server.MapPath("~/export/yamarket.xml")) ? "export/yamarket" + exportFeedId : "export/yamarket",
                        FileExtention = ExportFeedYandex.AvailableFileExtentions[0],
                        CompanyName = "#STORE_NAME#",
                        ShopName = "#STORE_NAME#",
                        ProductDescriptionType = "short",
                        Currency = ExportFeedYandex.AvailableCurrencies[0],
                        //ExportNotActiveProducts = true,
                        //ExportNotAmountProducts = true,
                        GlobalDeliveryCost = "[]",
                        LocalDeliveryOption = "{\"Cost\":null,\"Days\":\"\",\"OrderBefore\":\"\"}"
                    });
                    break;
                case EExportFeedType.GoogleMerchentCenter:
                    ExportFeedSettingsProvider.SetSettings(exportFeedId, new ExportFeedGoogleMerchantCenterOptions
                    {
                        PriceMargin = 0,
                        FileName = System.IO.File.Exists(Server.MapPath("~/export/google.xml")) ? "export/google" + exportFeedId : "export/google",
                        FileExtention = ExportFeedGoogleMerchantCenter.AvailableFileExtentions[0],
                        ProductDescriptionType = "short",
                        DatafeedTitle = "#STORE_NAME#",
                        DatafeedDescription = "#STORE_NAME#",
                        Currency = CurrencyService.CurrentCurrency.Iso3,
                        //ExportNotActiveProducts = true,
                        //ExportNotAmountProducts = true
                    });
                    break;
                case EExportFeedType.Csv:
                    ExportFeedSettingsProvider.SetSettings(exportFeedId, new ExportFeedCsvOptions()
                    {
                        PriceMargin = 0,
                        FileName = System.IO.File.Exists(Server.MapPath("~/export/catalog.csv")) ? "export/catalog" + exportFeedId : "catalog",
                        CsvSeparator = ";",
                        CsvColumSeparator = ";",
                        CsvPropertySeparator = ":",
                        CsvEnconing = EncodingsEnum.Utf8.StrName(),
                        FileExtention = "csv",
                        FieldMapping = new List<ProductFields>(Enum.GetValues(typeof(ProductFields)).OfType<ProductFields>().Where(item => item != ProductFields.None).ToList()),
                        //ExportNotActiveProducts = true,
                        //ExportNotAmountProducts = true
                    });
                    break;
            }

            Response.Redirect("exportfeed.aspx?feedid=" + exportFeedId);
        }

        protected string RenderExportFeedBorderColor(string type)
        {
            const string style = "border-left-color:{0};";
            switch (type.TryParseEnum<EExportFeedType>())
            {
                case EExportFeedType.Csv:
                    return string.Format(style, "#206b42");
                case EExportFeedType.YandexMarket:
                    return string.Format(style, "#e61319");
                case EExportFeedType.GoogleMerchentCenter:
                    return string.Format(style, "#19429c");
                case EExportFeedType.Reseller:
                    return string.Format(style, "#000000");
                default:
                    return string.Format(style, string.Empty);
            }
        }

        protected string GetJobActiveImage(int feedId)
        {
            var exportFeedSettings = ExportFeedSettingsProvider.GetSettings(feedId);
            return exportFeedSettings != null && exportFeedSettings.Active ? "<img src=\"images/activeTask.png\" class=\"txttooltip\" abbr=\"<div class='tooltipDiv'><span class='tooltipBold'>Настроена выгрузка по расписанию.</span></div>\"" : string.Empty;
        }

        protected string TypeParametr()
        {
            return !string.IsNullOrEmpty(Request["type"]) ? "&type=" + Request["type"] : string.Empty;
        }
    }
}