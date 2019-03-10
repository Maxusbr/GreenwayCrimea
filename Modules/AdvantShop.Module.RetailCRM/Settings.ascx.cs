//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.ExportImport;
using AdvantShop.Modules;
using AdvantShop.Modules.RetailCRM;
using AdvantShop.Modules.RetailCRM.Models;
using AdvantShop.Orders;
using Newtonsoft.Json;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Module.RetailCRM
{
    public partial class Settings : System.Web.UI.UserControl
    {
        private List<Status> crmStatuses = new List<Status>();
        private string _fileName = "";
        private string _filePath = "";


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request["message"] == "true")
            {
                Response.Clear();
                Response.Write(RetailCRMService.Message.ToString());
                Response.End();
                return;
            }

            lblMessage.Visible = false;

            if (!Page.IsPostBack)
            {
                LoadData();
            }

            if (ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId).IsNotEmpty())
            {
                _fileName = "retailcrm_" + ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId).Md5() + ".xml";
                _filePath = HostingEnvironment.MapPath("~/") + _fileName;
            }

        }

        private void LoadData()
        {
            string error;
            var sites = RetailCRMService.GetSites(out error);
            if (sites != null && sites.Count > 0)
            {
                ddlSites.Items.AddRange(sites.Select(s => new ListItem(s.name, s.code)).ToArray());
            }


            crmStatuses = RetailCRMService.GetStatuses(out error);

            lvStatuses.DataSource = OrderStatusService.GetOrderStatuses();
            lvStatuses.DataBind();

            ddlDefaultStatusCategory.DataSource = RetailCRMService.GetGroups(out error);
            ddlDefaultStatusCategory.DataBind();

            lvLogs.DataSource = RetailCRMLog.GetLogFiles();
            lvLogs.DataBind();
            var currency = CurrencyService.GetAllCurrencies();
            var currentCurrencyID = ModuleSettingsProvider.GetSettingValue<int>("CurrencyIDExport", RetailCRMModule.ModuleStringId);
            var selectArtNoType = ModuleSettingsProvider.GetSettingValue<string>("RetailArtNoType", RetailCRMModule.ModuleStringId);
            rblArtNoType.SelectedValue = selectArtNoType != null ? selectArtNoType : "ArtNo";

            ddlCurrency.Items.Clear();
            ddlCurrency.Items.Add(new ListItem("Выберите валюту"));
            
            ddlCurrency.Items.AddRange(CurrencyService.GetAllCurrencies().Select(cur=> new ListItem(cur.Name, cur.CurrencyId.ToString())).ToArray());
            if (ddlCurrency.Items.FindByValue(currentCurrencyID.ToString()) != null)
            {
                ddlCurrency.SelectedValue = currentCurrencyID.ToString();
            }

            rblOrderSendingMode.SelectedValue = ModuleSettingsProvider.GetSettingValue<string>("OrderSendingMode", RetailCRMModule.ModuleStringId) ?? "Always";

        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSubdomain.Text = ModuleSettingsProvider.GetSettingValue<string>("SubDomain", RetailCRMModule.ModuleStringId);
                txtApiKey.Text = ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId);
                txtCollectorKey.Text = ModuleSettingsProvider.GetSettingValue<string>("CollectorKey", RetailCRMModule.ModuleStringId);

                var site = ModuleSettingsProvider.GetSettingValue<string>("Site", RetailCRMModule.ModuleStringId);
                if (ddlSites.Items.FindByValue(site) != null)
                {
                    ddlSites.SelectedValue = site;
                }

                var category = ModuleSettingsProvider.GetSettingValue<string>("DefaultStatusCategory", RetailCRMModule.ModuleStringId);
                if (ddlDefaultStatusCategory.Items.FindByValue(category) != null)
                {
                    ddlDefaultStatusCategory.SelectedValue = category;
                }
            }

            tblSettings.Visible = txtApiKey.Text.IsNotEmpty() && txtSubdomain.Text.IsNotEmpty();
            lblFile.Text = lblFile.NavigateUrl = SettingsMain.SiteUrl + "/" + _fileName;

            if (File.Exists(_filePath))
            {
                var fileInfo = new FileInfo(_filePath);
                lblDate.Text = "обновлен " + fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                lblDate.Text = "не создан";
            }

        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("Site", ddlSites.SelectedValue, RetailCRMModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("DefaultStatusCategory", ddlDefaultStatusCategory.SelectedValue, RetailCRMModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue<int>("CurrencyIDExport", ddlCurrency.SelectedItem.Value == "Выберите валюту" ? 0 : ddlCurrency.SelectedItem.Value.TryParseInt(), RetailCRMModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue<string>("RetailArtNoType", rblArtNoType.SelectedItem.Value, RetailCRMModule.ModuleStringId);

            var statuses = new Dictionary<int, string>();

            foreach (var item in lvStatuses.Items)
            {
                var id = ((HiddenField)item.FindControl("hfStatusID")).Value.TryParseInt();
                var code = ((DropDownList)item.FindControl("ddlStatuses")).SelectedValue;
                statuses.Add(id, code);
            }

            ModuleSettingsProvider.SetSettingValue("Statuses", JsonConvert.SerializeObject(statuses), RetailCRMModule.ModuleStringId);


            ModuleSettingsProvider.SetSettingValue("OrderSendingMode", rblOrderSendingMode.SelectedValue, RetailCRMModule.ModuleStringId);

            lblMessage.Text = "Настройки сохранены";
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("SubDomain", txtSubdomain.Text.Replace("http://", "").Replace("https://", ""), RetailCRMModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("ApiKey", txtApiKey.Text, RetailCRMModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("CollectorKey", txtCollectorKey.Text, RetailCRMModule.ModuleStringId);

            if (RetailCRMService.PingCRM())
            {
                lblMessage.Text = "Настройки сохранены";
                lblMessage.ForeColor = Color.Blue;
                LoadData();
            }
            else
            {
                lblMessage.Text = "Неверные данные. Синхронизация невозможна.";
                lblMessage.ForeColor = Color.Red;
            }
            lblMessage.Visible = true;
        }

        protected void OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var statusId = ((HiddenField)e.Item.FindControl("hfStatusID")).Value.TryParseInt();
            var ddl = ((DropDownList)e.Item.FindControl("ddlStatuses"));
            ddl.DataSource = crmStatuses;
            ddl.DataBind();

            var str = ModuleSettingsProvider.GetSettingValue<string>("Statuses", RetailCRMModule.ModuleStringId);
            if (str != null)
            {
                var statuses = JsonConvert.DeserializeObject<Dictionary<int, string>>(str);
                if (statuses != null && statuses.ContainsKey(statusId) &&
                    ddl.Items.FindByValue(statuses[statusId]) != null)
                {
                    ddl.SelectedValue = statuses[statusId];
                }
            }

        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                string error;
                RetailCRMService.Message.Clear();
                RetailCRMService.Message.Append("<br/>Отправка началась");
                RetailCRMService.UploadOrders(out error);
                RetailCRMService.Message.Append("<br/>" + error + "<br/>" +
                                                "Отправка завершена");
            });

            t.Start();

            Response.Redirect(AdvantShop.Core.UrlRewriter.UrlService.GenerateBaseUrl() + Request.RawUrl + (Request.Url.ToString().Contains("&ping") ? string.Empty : "&ping=true"));

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("ApiKey",
                    RetailCRMModule.ModuleStringId)))
            {
                return;
            }

            var currency = CurrencyService.GetCurrency(ModuleSettingsProvider.GetSettingValue<int>("CurrencyIDExport", RetailCRMModule.ModuleStringId));

            var options = new ExportFeedYandexOptions
            {
                //ExportNotActiveProducts = true,
                //ExportNotAmountProducts = true,
                ExportNotAvailable = true,
                FileName = "retailcrm_" + ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId).Md5(),
                FileExtention = "xml",
                Delivery = true,
                ColorSizeToName = false,
                Currency = currency != null ? currency.Iso3 : "RUB",
                RemoveHtml = true,
                ProductDescriptionType = "full",

            };
            //TODO EXPORTFEED
            var exportFeedModule = new ExportFeedRetailCRM(
                RetailCRMService.GetCategories(),
                RetailCRMService.GetProducts(options),
                options,
                0,
                0
            );
            exportFeedModule.Build();
        }

        protected void Button1_OnClick(object sender, EventArgs e)
        {
            string error = "";
            RetailCRMService.GetOrderHistory(out error, DateTime.Now.AddDays(-1));
        }

    }
}