using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Module.YaBuying.Domain;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Shipping;
using System.Globalization;

namespace AdvantShop.Module.YaBuying
{
    public partial class Admin_YaMarketBuyingModuleSetting : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
			YaMarketByuingService.UpdateModule();
            LoadSettings();
        }

        protected void lvShippings_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var item = (YaMarketShipping)e.Item.DataItem;

                if (!string.IsNullOrEmpty(item.Type))
                {
                    ((DropDownList)e.Item.FindControl("ddlType")).SelectedValue = item.Type;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
			lblApiUrl.Text = (SettingsMain.SiteUrl.Contains("https") ? SettingsMain.SiteUrl.TrimEnd('/') : SettingsMain.SiteUrl.TrimEnd('/').Replace("http", "https")) + "/" + YaMarketByuingApiService.ApiUrl;
            txtAuth.Text = YaMarketBuyingSettings.AuthToken;

            
            var shipppings = new List<YaMarketShipping>();
            var shippingMethods = ShippingMethodService.GetAllShippingMethods(true);
            var selshippings = YaMarketByuingService.GetShippings();

            foreach (var shipping in shippingMethods)
            {
                var ship = new YaMarketShipping() { Name = shipping.Name, ShippingMethodId = shipping.ShippingMethodId };

                var sel = selshippings.FirstOrDefault(x => x.ShippingMethodId == shipping.ShippingMethodId);
                if (sel != null)
                {
                    ship.Type = sel.Type;
                    ship.MinDate = sel.MinDate;
                    ship.MaxDate = sel.MaxDate;
                }
                shipppings.Add(ship);
            }

            lvShippings.DataSource = shipppings;
            lvShippings.DataBind();

            var payments = YaMarketBuyingSettings.Payments.Split(';');

            ddlYandex.SelectedValue = payments.Any(x => x == "YANDEX") ? "1" : "0";
            ddlShopprepaid.SelectedValue = payments.Any(x => x == "SHOP_PREPAID") ? "1" : "0";
            ddlCashOnDelivery.SelectedValue = payments.Any(x => x == "CASH_ON_DELIVERY") ? "1" : "0";
            ddlCardOnDelivery.SelectedValue = payments.Any(x => x == "CARD_ON_DELIVERY") ? "1" : "0";

            var statuses =
                OrderStatusService.GetOrderStatuses()
                    .Select(x => new ListItem(x.StatusName, x.StatusID.ToString()))
                    .ToList();

            ddlUnpaidStatus.Items.Clear();
            ddlUnpaidStatus.Items.AddRange(statuses.CloneItems());

            ddlProcessingStatus.Items.Clear();
            ddlProcessingStatus.Items.AddRange(statuses.CloneItems());

            ddlDeliveryStatus.Items.Clear();
            ddlDeliveryStatus.Items.AddRange(statuses.CloneItems());

            try
            {
                lvDeliveryStatuses.DataSource =
                    OrderStatusService.GetOrderStatuses()
                        .Where(x => YaMarketBuyingSettings.DeliveryStatusesIds.Contains(x.StatusID))
                        .ToList();
                lvDeliveryStatuses.DataBind();

                lvProcessingStatuses.DataSource =
                    OrderStatusService.GetOrderStatuses()
                        .Where(x => YaMarketBuyingSettings.ProcessingStatusesIds.Contains(x.StatusID))
                        .ToList();
                lvProcessingStatuses.DataBind();

            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            ddlDeliveredStatus.Items.Clear();
            ddlDeliveredStatus.Items.AddRange(statuses.CloneItems());

            var statusesTmp = new List<ListItem>() {new ListItem("Нет", "0")};
            statusesTmp.AddRange(statuses);

            var statusesWithNo = statusesTmp.CloneListItems();

            ddlPickupStatus.Items.Clear();
            ddlPickupStatus.Items.AddRange(statusesWithNo.CloneItems());

            ddlReservedStatus.Items.Clear();
            ddlReservedStatus.Items.AddRange(statusesWithNo.CloneItems());



            if (ddlUnpaidStatus.Items.FindByValue(YaMarketBuyingSettings.UpaidStatusId.ToString()) != null)
                ddlUnpaidStatus.SelectedValue = YaMarketBuyingSettings.UpaidStatusId.ToString();

            //if (ddlProcessingStatus.Items.FindByValue(YaMarketBuyingSettings.ProcessingStatusId.ToString()) != null)
            //    ddlProcessingStatus.SelectedValue = YaMarketBuyingSettings.ProcessingStatusId.ToString();

            //if (ddlDeliveryStatus.Items.FindByValue(YaMarketBuyingSettings.DeliveryStatusId.ToString()) != null)
            //    ddlDeliveryStatus.SelectedValue = YaMarketBuyingSettings.DeliveryStatusId.ToString();

            if (ddlDeliveredStatus.Items.FindByValue(YaMarketBuyingSettings.DeliveredStatusId.ToString()) != null)
                ddlDeliveredStatus.SelectedValue = YaMarketBuyingSettings.DeliveredStatusId.ToString();

            if (ddlPickupStatus.Items.FindByValue(YaMarketBuyingSettings.PickupStatusId.ToString()) != null)
                ddlPickupStatus.SelectedValue = YaMarketBuyingSettings.PickupStatusId.ToString();

            if (ddlReservedStatus.Items.FindByValue(YaMarketBuyingSettings.ReservedStatusId.ToString()) != null)
                ddlReservedStatus.SelectedValue = YaMarketBuyingSettings.ReservedStatusId.ToString();

            var cancelled = OrderStatusService.GetOrderStatuses().Where(x => x.IsCanceled).Select(x => x.StatusName).ToList();
            lblCancelledStatus.Text = cancelled.Count > 0 ? String.Join(", <br>", cancelled) : "";

            txtCampaignId.Text = YaMarketBuyingSettings.CampaignId;
            txtAuthTokenToYaMarket.Text = YaMarketBuyingSettings.AuthTokenToMarket;
            txtAuthClientId.Text = YaMarketBuyingSettings.AuthClientId;
            txtLogin.Text = YaMarketBuyingSettings.Login;
            cbDefaultShippingEnabled.Checked = YaMarketBuyingSettings.EnableDefaultShipping;
            txtDefaultShippingName.Text = YaMarketBuyingSettings.DefaultShippingName;
            txtDefaultShippingPrice.Text = YaMarketBuyingSettings.DefaultShippingPrice.ToString();
            txtDefaultShippingMinDate.Text = YaMarketBuyingSettings.DefaultShippingMinDate.ToString();
            txtDefaultShippingMaxDate.Text = YaMarketBuyingSettings.DefaultShippingMaxDate.ToString();
            ddlDefaultShippingType.SelectedValue = YaMarketBuyingSettings.DefaultShippingType;

            lblCashOnDeliveryCountriesCities.Text = String.Join(", ", 
                YaMarketByuingService.GetPaymentCountries("CASH_ON_DELIVERY")
                    .Select(x =>
                            string.Format("{0} <img src='images/remove.jpg' onclick='javascript:DeleteCountry(\"{2}\", {1}); return false;'>",
                                x.Name, x.CountryId, "CASH_ON_DELIVERY")));

            lblCashOnDeliveryCountriesCities.Text += " " + String.Join(", ",
                YaMarketByuingService.GetPaymentCities("CASH_ON_DELIVERY")
                    .Select(x =>
                            string.Format("{0} <img src='images/remove.jpg' onclick='javascript:DeleteCity(\"{2}\", {1}); return false;'>",
                                x.Name, x.CityId, "CASH_ON_DELIVERY")));

            lblCardOnDeliveryCountriesCities.Text = String.Join(", ",
                YaMarketByuingService.GetPaymentCountries("CASH_ON_DELIVERY")
                    .Select(x =>
                            string.Format("{0} <img src='images/remove.jpg' onclick='javascript:DeleteCountry(\"{2}\", {1}); return false;'>",
                                x.Name, x.CountryId, "CASH_ON_DELIVERY")));

            lblCardOnDeliveryCountriesCities.Text += " " + String.Join(", ",
                YaMarketByuingService.GetPaymentCities("CARD_ON_DELIVERY")
                    .Select(x =>
                            string.Format("{0} <img src='images/remove.jpg' onclick='javascript:DeleteCity(\"{2}\", {1}); return false;'>",
                                x.Name, x.CityId, "CARD_ON_DELIVERY")));

            // причины отмены заказа
            ddlCanceled_PROCESSING_EXPIRED.Items.Clear();
            ddlCanceled_PROCESSING_EXPIRED.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_PROCESSING_EXPIRED.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_PROCESSING_EXPIRED.ToString()) != null)
                ddlCanceled_PROCESSING_EXPIRED.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_PROCESSING_EXPIRED.ToString();

            ddlCanceled_REPLACING_ORDER.Items.Clear();
            ddlCanceled_REPLACING_ORDER.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_REPLACING_ORDER.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_REPLACING_ORDER.ToString()) != null)
                ddlCanceled_REPLACING_ORDER.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_REPLACING_ORDER.ToString();

            ddlCanceled_RESERVATION_EXPIRED.Items.Clear();
            ddlCanceled_RESERVATION_EXPIRED.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_RESERVATION_EXPIRED.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_RESERVATION_EXPIRED.ToString()) != null)
                ddlCanceled_RESERVATION_EXPIRED.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_RESERVATION_EXPIRED.ToString();

            ddlCanceled_USER_CHANGED_MIND.Items.Clear();
            ddlCanceled_USER_CHANGED_MIND.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_USER_CHANGED_MIND.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_USER_CHANGED_MIND.ToString()) != null)
                ddlCanceled_USER_CHANGED_MIND.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_USER_CHANGED_MIND.ToString();

            ddlCanceled_USER_NOT_PAID.Items.Clear();
            ddlCanceled_USER_NOT_PAID.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_USER_NOT_PAID.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_USER_NOT_PAID.ToString()) != null)
                ddlCanceled_USER_NOT_PAID.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_USER_NOT_PAID.ToString();

            ddlCanceled_USER_REFUSED_DELIVERY.Items.Clear();
            ddlCanceled_USER_REFUSED_DELIVERY.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_USER_REFUSED_DELIVERY.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_DELIVERY.ToString()) != null)
                ddlCanceled_USER_REFUSED_DELIVERY.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_DELIVERY.ToString();

            ddlCanceled_USER_REFUSED_PRODUCT.Items.Clear();
            ddlCanceled_USER_REFUSED_PRODUCT.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_USER_REFUSED_PRODUCT.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_PRODUCT.ToString()) != null)
                ddlCanceled_USER_REFUSED_PRODUCT.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_PRODUCT.ToString();

            ddlCanceled_USER_REFUSED_QUALITY.Items.Clear();
            ddlCanceled_USER_REFUSED_QUALITY.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_USER_REFUSED_QUALITY.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_QUALITY.ToString()) != null)
                ddlCanceled_USER_REFUSED_QUALITY.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_QUALITY.ToString();

            ddlCanceled_USER_UNREACHABLE.Items.Clear();
            ddlCanceled_USER_UNREACHABLE.Items.AddRange(statusesWithNo.CloneItems());

            if (ddlCanceled_USER_UNREACHABLE.Items.FindByValue(YaMarketBuyingSettings.CanceledStatusId_USER_UNREACHABLE.ToString()) != null)
                ddlCanceled_USER_UNREACHABLE.SelectedValue = YaMarketBuyingSettings.CanceledStatusId_USER_UNREACHABLE.ToString();

            var scheduleDelivery = YaMarketBuyingSettings.ScheduleDelivery;
            chkScheduleDelivery.Items.Clear();
            chkScheduleDelivery.Items.AddRange(YaMarketBuyingSettings.GetListDays());
            for(int i = 0; i < chkScheduleDelivery.Items.Count; i++)
            {
                chkScheduleDelivery.Items[i].Selected = scheduleDelivery.Contains(chkScheduleDelivery.Items[i].Value);
            }
            var timeDelivery = YaMarketBuyingSettings.TimeDeliveryForSchedule.Split(":");
            txtTimeDileveryHour.Text = timeDelivery[0];
            txtTimeDileveryMinutes.Text = timeDelivery[1];

        }

        private void SaveSettings()
        {
            YaMarketByuingService.DeleteShippings();

            var outlets = new List<int>();

            foreach (var dataItem in lvShippings.Items)
            {
                var type = ((DropDownList)dataItem.FindControl("ddlType")).SelectedValue;
                var methodId = ((HiddenField)dataItem.FindControl("hfShippingMethodId")).Value.TryParseInt();
                var minDay = ((TextBox)dataItem.FindControl("txtMinDate")).Text.TryParseInt();
                var maxDay = ((TextBox)dataItem.FindControl("txtMaxDate")).Text.TryParseInt();

                if (type != "")
                {
                    YaMarketByuingService.AddShipping(new YaMarketShipping()
                    {
                        ShippingMethodId = methodId,
                        Type = type,
                        MinDate = minDay,
                        MaxDate = maxDay
                    });
                }

                if (type == "PICKUP")
                    outlets.Add(methodId);
            }

            var payments = new List<string>();

            if (ddlYandex.SelectedValue == "1")
                payments.Add("YANDEX");

            if (ddlShopprepaid.SelectedValue == "1")
                payments.Add("SHOP_PREPAID");

            if (ddlCashOnDelivery.SelectedValue == "1")
                payments.Add("CASH_ON_DELIVERY");

            if (ddlCardOnDelivery.SelectedValue == "1")
                payments.Add("CARD_ON_DELIVERY");

            YaMarketBuyingSettings.Payments = string.Join(";", payments);
            YaMarketBuyingSettings.Outlets = string.Join(";", outlets);
            YaMarketBuyingSettings.AuthToken = txtAuth.Text.Trim();
            YaMarketBuyingSettings.AuthTokenToMarket = txtAuthTokenToYaMarket.Text.Trim();
            YaMarketBuyingSettings.AuthClientId = txtAuthClientId.Text.Trim();
            YaMarketBuyingSettings.Login = txtLogin.Text;

            YaMarketBuyingSettings.UpaidStatusId = ddlUnpaidStatus.SelectedValue.TryParseInt();
            //YaMarketBuyingSettings.ProcessingStatusId = ddlProcessingStatus.SelectedValue.TryParseInt();
            //YaMarketBuyingSettings.DeliveryStatusId = ddlDeliveryStatus.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.DeliveredStatusId = ddlDeliveredStatus.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.PickupStatusId = ddlPickupStatus.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.ReservedStatusId = ddlReservedStatus.SelectedValue.TryParseInt();

            var allStatuses = OrderStatusService.GetOrderStatuses();
            YaMarketBuyingSettings.ProcessingStatusesIds = YaMarketBuyingSettings.ProcessingStatusesIds.Where(x => allStatuses.Find(s => s.StatusID == x) != null).ToList();
            YaMarketBuyingSettings.DeliveryStatusesIds = YaMarketBuyingSettings.DeliveryStatusesIds.Where(x => allStatuses.Find(s => s.StatusID == x) != null).ToList();

            YaMarketBuyingSettings.CampaignId = txtCampaignId.Text.Trim();

            YaMarketBuyingSettings.EnableDefaultShipping = cbDefaultShippingEnabled.Checked;
            YaMarketBuyingSettings.DefaultShippingName = txtDefaultShippingName.Text;
            YaMarketBuyingSettings.DefaultShippingPrice = txtDefaultShippingPrice.Text.TryParseFloat();
            YaMarketBuyingSettings.DefaultShippingMinDate = txtDefaultShippingMinDate.Text.TryParseInt();
            YaMarketBuyingSettings.DefaultShippingMaxDate = txtDefaultShippingMaxDate.Text.TryParseInt();
            YaMarketBuyingSettings.DefaultShippingType = ddlDefaultShippingType.SelectedValue;


            // причины отмены заказа
            YaMarketBuyingSettings.CanceledStatusId_PROCESSING_EXPIRED = ddlCanceled_PROCESSING_EXPIRED.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_REPLACING_ORDER = ddlCanceled_REPLACING_ORDER.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_RESERVATION_EXPIRED = ddlCanceled_RESERVATION_EXPIRED.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_USER_CHANGED_MIND = ddlCanceled_USER_CHANGED_MIND.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_USER_NOT_PAID = ddlCanceled_USER_NOT_PAID.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_DELIVERY = ddlCanceled_USER_REFUSED_DELIVERY.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_PRODUCT = ddlCanceled_USER_REFUSED_PRODUCT.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_USER_REFUSED_QUALITY = ddlCanceled_USER_REFUSED_QUALITY.SelectedValue.TryParseInt();
            YaMarketBuyingSettings.CanceledStatusId_USER_UNREACHABLE = ddlCanceled_USER_UNREACHABLE.SelectedValue.TryParseInt();

            var scheduleDelivery = new StringBuilder();
            foreach(ListItem item in chkScheduleDelivery.Items)
            {
                if (item.Selected)
                    scheduleDelivery.Append(item.Value + ",");
            }
            YaMarketBuyingSettings.ScheduleDelivery = scheduleDelivery.ToString().Trim(',');
            string hourDeluv = "00" + txtTimeDileveryHour.Text, minutesDeluv = "00" + txtTimeDileveryMinutes.Text;
            YaMarketBuyingSettings.TimeDeliveryForSchedule = hourDeluv.Substring(hourDeluv.Length - 2, 2) + ":" + minutesDeluv.Substring(hourDeluv.Length - 2, 2);
        }



        protected void btnAddCashOnDeliveryCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCashOnDeliveryCountry.Text))
            {
                var country = CountryService.GetCountryByName(txtCashOnDeliveryCountry.Text);
                if (country != null)
                {
                    if (!YaMarketByuingService.IsExistPaymentCountry("CASH_ON_DELIVERY", country.CountryId))
                        YaMarketByuingService.AddPaymentCountry("CASH_ON_DELIVERY", country.CountryId);

                    txtCashOnDeliveryCountry.Text = "";
                }
            }
        }
        
        protected void lbDeleteCountry_Click(object sender, EventArgs e)
        {
            var country = countryId.Value;
            var method = methodId.Value;

            if (!string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(method))
            {
                YaMarketByuingService.DeletePaymentCountry(method, country.TryParseInt());
            }

            methodId.Value = countryId.Value = "";
        }

        protected void lbDeleteCity_Click(object sender, EventArgs e)
        {
            var city = cityId.Value;
            var method = methodId.Value;

            if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(method))
            {
                YaMarketByuingService.DeletePaymentCity(method, city.TryParseInt());
            }

            methodId.Value = cityId.Value = "";
        }

        protected void btnCashOnDeliveryAddCity_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCashOnDeliveryCity.Text))
            {
                var city = CityService.GetCityByName(txtCashOnDeliveryCity.Text);
                if (city != null)
                {
                    if (!YaMarketByuingService.IsExistPaymentCity("CASH_ON_DELIVERY", city.CityId))
                        YaMarketByuingService.AddPaymentCity("CASH_ON_DELIVERY", city.CityId);

                    txtCashOnDeliveryCity.Text = "";
                }
            }
        }

        protected void btnAddCardOnDeliveryCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCardOnDeliveryCountry.Text))
            {
                var country = CountryService.GetCountryByName(txtCardOnDeliveryCountry.Text);
                if (country != null)
                {
                    if (!YaMarketByuingService.IsExistPaymentCountry("CARD_ON_DELIVERY", country.CountryId))
                        YaMarketByuingService.AddPaymentCountry("CARD_ON_DELIVERY", country.CountryId);

                    txtCardOnDeliveryCountry.Text = "";
                }
            }
        }
        protected void btnCardOnDeliveryAddCity_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCardOnDeliveryCity.Text))
            {
                var city = CityService.GetCityByName(txtCardOnDeliveryCity.Text);
                if (city != null)
                {
                    if (!YaMarketByuingService.IsExistPaymentCity("CARD_ON_DELIVERY", city.CityId))
                        YaMarketByuingService.AddPaymentCity("CARD_ON_DELIVERY", city.CityId);

                    txtCardOnDeliveryCity.Text = "";
                }
            }
        }

        protected void lvDeliveryStatuses_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteDeliveryStatus")
            {
                var id = Convert.ToInt32(e.CommandArgument);
                var ids = YaMarketBuyingSettings.DeliveryStatusesIds;

                if (ids.Contains(id))
                {
                    ids.Remove(id);
                    YaMarketBuyingSettings.DeliveryStatusesIds = ids;
                }
            }
        }

        protected void btnAddDeliveryStatus_Click(object sender, EventArgs e)
        {
            var id = ddlDeliveryStatus.SelectedValue.TryParseInt();
            var ids = YaMarketBuyingSettings.DeliveryStatusesIds;

            if (id != 0 && !ids.Contains(id))
            {
                ids.Add(id);
                YaMarketBuyingSettings.DeliveryStatusesIds = ids;
            }
        }

        protected void lvProcessingStatuses_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProcessingStatus")
            {
                var id = Convert.ToInt32(e.CommandArgument);
                var ids = YaMarketBuyingSettings.ProcessingStatusesIds;

                if (ids.Contains(id))
                {
                    ids.Remove(id);
                    YaMarketBuyingSettings.ProcessingStatusesIds = ids;
                }
            }
        }

        protected void btnAddProcessingStatus_Click(object sender, EventArgs e)
        {
            var id = ddlProcessingStatus.SelectedValue.TryParseInt();
            var ids = YaMarketBuyingSettings.ProcessingStatusesIds;

            if (id != 0 && !ids.Contains(id))
            {
                ids.Add(id);
                YaMarketBuyingSettings.ProcessingStatusesIds = ids;
            }
        }
    }

    public static class Extensions
    {
        public static ListItem[] CloneItems(this List<ListItem> listToClone)
        {
            return listToClone.Select(x => new ListItem(x.Text, x.Value)).ToArray();
        }

        public static List<ListItem> CloneListItems(this List<ListItem> listToClone)
        {
            return listToClone.Select(x => new ListItem(x.Text, x.Value)).ToList();
        }
    }
}