using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.ExportImport;
using AdvantShop.Repository.Currencies;
using Newtonsoft.Json;

namespace AdvantShop.Admin.UserControls.ExportFeedUc
{
    public partial class ExportFeedYandexSettingsUc : ExportFeedControl
    {
        private List<ExportFeedYandexDeliveryCostOption> GlobalDeliveryOptionsList
        {
            get
            {
                var globalDeliveryOptions = new List<ExportFeedYandexDeliveryCostOption>();

                try
                {
                    var options = ViewState["GlobalDeliveryOptionsList"] as string;

                    if (!string.IsNullOrWhiteSpace(options))
                    {
                        globalDeliveryOptions =
                            JsonConvert.DeserializeObject<List<ExportFeedYandexDeliveryCostOption>>(options);
                    }
                }
                finally
                {
                    if (globalDeliveryOptions == null)
                        globalDeliveryOptions = new List<ExportFeedYandexDeliveryCostOption>();
                }

                return globalDeliveryOptions;
            }
            set { ViewState["GlobalDeliveryOptionsList"] = value != null ? JsonConvert.SerializeObject(value) : null; }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (IsPostBack)
            //    return;
            var exportFeedId = 0;
            if (string.IsNullOrEmpty(Request["feedid"]) || !int.TryParse(Request["feedid"], out exportFeedId))
                return;

            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var yaSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedYandexOptions>(commonSettings.AdvancedSettings);

            txtUrlTags.Text = yaSettings.AdditionalUrlTags;
            txtCompanyName.Text = yaSettings.CompanyName;
            txtShopName.Text = yaSettings.ShopName;
            txtSalesNotes.Text = yaSettings.SalesNotes;

            ckbExportProductDiscount.Checked = yaSettings.ExportProductDiscount;
            chbDelivery.Checked = yaSettings.Delivery;
            chbPickup.Checked = yaSettings.Pickup;
            chbRemoveHTML.Checked = yaSettings.RemoveHtml;
            chbProperties.Checked = yaSettings.ExportProductProperties;
            ckbColorSizeToName.Checked = yaSettings.ColorSizeToName;
            ddlProductDescriptionType.SelectedValue = yaSettings.ProductDescriptionType;
            ddlOfferIdType.SelectedValue = yaSettings.OfferIdType;
            chbExportNotAvailable.Checked = yaSettings.ExportNotAvailable;
            //chbAvailable.Checked = yaSettings.Available;
            chbPurchasePrice.Checked = yaSettings.ExportPurchasePrice;
            chbStoreSetting.Checked = yaSettings.Store;
            txtBid.Text = yaSettings.Bid != null ? yaSettings.Bid.Value.ToString() : "0";
            chkExportBarCode.Checked = yaSettings.ExportBarCode;
            chkExportAllPhotos.Checked = yaSettings.ExportAllPhotos;

            chbExportRelatedProducts.Checked = yaSettings.ExportRelatedProducts;

            CurrencyListBox.Items.Clear();

            foreach (var item in CurrencyService.GetAllCurrencies()
                .Where(item => ExportFeedYandex.AvailableCurrencies.Contains(item.Iso3)).ToList())
            {
                CurrencyListBox.Items.Add(new ListItem { Text = item.Name, Value = item.Iso3 });
            }

            string selectCurrency = yaSettings.Currency;
            if (selectCurrency != null && CurrencyListBox.Items.FindByValue(selectCurrency) != null)
            {
                CurrencyListBox.SelectedValue = selectCurrency;
            }

            lblUrlTagsNote.Text = (new ExportFeedYandex()).GetAvailableVariables().Aggregate(string.Empty, (current, item) => current + (item + ", "));
            ltrlCompanyNameDote.Text = SettingsMain.ShopName;
            companyName2.Text = SettingsMain.ShopName;

            if (!IsPostBack)
            {
                var deliveryOptions = new List<ListItem>();
                foreach (ExportFeedYandexDeliveryCost option in Enum.GetValues(typeof(ExportFeedYandexDeliveryCost)))
                {
                    deliveryOptions.Add(new ListItem()
                    {
                        Text = option.Localize(),
                        Value = ((int)option).ToString(),
                        Selected = option == yaSettings.DeliveryCost
                    });
                }

                rbDeliveryCost.Items.AddRange(deliveryOptions.ToArray());
            }


            var globalDeliveryOptions = new List<ExportFeedYandexDeliveryCostOption>();
            if (!IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(yaSettings.GlobalDeliveryCost))
                    {
                        globalDeliveryOptions =
                            JsonConvert.DeserializeObject<List<ExportFeedYandexDeliveryCostOption>>(yaSettings.GlobalDeliveryCost);
                    }
                }
                finally
                {
                    if (globalDeliveryOptions == null)
                        globalDeliveryOptions = new List<ExportFeedYandexDeliveryCostOption>();
                }
            }
            else
            {
                globalDeliveryOptions = GlobalDeliveryOptionsList;
            }


            grid.DataSource = globalDeliveryOptions;
            grid.DataBind();
            GlobalDeliveryOptionsList = globalDeliveryOptions;

            var localDeliveryOption = new ExportFeedYandexDeliveryCostOption();
            try
            {
                if (!string.IsNullOrWhiteSpace(yaSettings.LocalDeliveryOption))
                {
                    localDeliveryOption =
                        JsonConvert.DeserializeObject<ExportFeedYandexDeliveryCostOption>(yaSettings.LocalDeliveryOption);
                }
            }
            finally
            {
                if (localDeliveryOption == null)
                    localDeliveryOption = new ExportFeedYandexDeliveryCostOption();
            }

            txtLocalDeliveryDays.Text = localDeliveryOption.Days;
            txtLocalDeliveryOrderBefore.Text = localDeliveryOption.OrderBefore;
        }

        public override string GetData()
        {
            var advancedSettings = new ExportFeedYandexOptions
            {
                AdditionalUrlTags = txtUrlTags.Text.Trim(new char[] { '?', '&' }),
                CompanyName = txtCompanyName.Text,
                ShopName = txtShopName.Text,
                SalesNotes = txtSalesNotes.Text,
                Pickup = chbPickup.Checked,
                Delivery = chbDelivery.Checked,
                Store = chbStoreSetting.Checked,
                RemoveHtml = chbRemoveHTML.Checked,
                ExportProductProperties = chbProperties.Checked,
                Currency = CurrencyListBox.SelectedValue,
                ColorSizeToName = ckbColorSizeToName.Checked,
                ProductDescriptionType = ddlProductDescriptionType.SelectedValue,
                ExportProductDiscount = ckbExportProductDiscount.Checked,
                OfferIdType = ddlOfferIdType.SelectedValue,
                ExportBarCode = chkExportBarCode.Checked,
                ExportAllPhotos = chkExportAllPhotos.Checked,
                DeliveryCost = (ExportFeedYandexDeliveryCost)rbDeliveryCost.SelectedValue.TryParseInt(),
                GlobalDeliveryCost = JsonConvert.SerializeObject(GlobalDeliveryOptionsList),
                ExportNotAvailable = chbExportNotAvailable.Checked,
                //Available = chbAvailable.Checked,
                ExportPurchasePrice = chbPurchasePrice.Checked,
                LocalDeliveryOption = JsonConvert.SerializeObject(new ExportFeedYandexDeliveryCostOption()
                {
                    Days = txtLocalDeliveryDays.Text.Trim(),
                    OrderBefore = txtLocalDeliveryOrderBefore.Text.Trim()
                }),

                Bid = txtBid.Text.TryParseFloat(),
                ExportRelatedProducts = chbExportRelatedProducts.Checked

        };

            return JsonConvert.SerializeObject(advancedSettings);
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                GridViewRow footer = ((AdvGridView)sender).FooterRow;

                var list = GlobalDeliveryOptionsList;
                list.Add(new ExportFeedYandexDeliveryCostOption()
                {
                    Cost = ((TextBox)footer.FindControl("txtNewCost")).Text,
                    Days = ((TextBox)footer.FindControl("txtNewDays")).Text,
                    OrderBefore = ((TextBox)footer.FindControl("txtNewOrderBefore")).Text,
                });
                GlobalDeliveryOptionsList = list;
                grid.ShowFooter = false;
            }

            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }

            if (e.CommandName == "UpdateItem")
            {
                var index = Convert.ToInt32(e.CommandArgument);
                if (GlobalDeliveryOptionsList.Count > index)
                {
                    var list = GlobalDeliveryOptionsList;
                    list[index] = new ExportFeedYandexDeliveryCostOption()
                    {
                        Cost = ((TextBox)grid.Rows[index].FindControl("txtCost")).Text,
                        Days = ((TextBox)grid.Rows[index].FindControl("txtDays")).Text,
                        OrderBefore = ((TextBox)grid.Rows[index].FindControl("txtOrderBefore")).Text,
                    };
                    GlobalDeliveryOptionsList = list;
                }
            }

            if (e.CommandName == "DeleteItem")
            {
                var index = Convert.ToInt32(e.CommandArgument);
                if (GlobalDeliveryOptionsList.Count > index)
                {
                    var list = GlobalDeliveryOptionsList;
                    list.RemoveAt(index);
                    GlobalDeliveryOptionsList = list;
                }
            }
        }

        protected void lbAddDeliveryCostOption_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
        }
    }
}