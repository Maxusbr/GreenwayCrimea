using System;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Modules;
using AdvantShop.Module.SmsNotifications;
using AdvantShop.Orders;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL;
using System.Data;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Module.SmsNotifications.Status;
using AdvantShop.Module.SmsNotifications.Domain.SmsServices;

namespace Advantshop.Modules.UserControls
{
    public partial class Modules_SmsNotifications_SmsNotifications : UserControl
    {
        private static readonly string _moduleName = SmsNotifications.ModuleId;
        private InSetFieldFilter _selectionFilter;
        private bool _inverseSelection;
        protected List<OrderStatus> _statusList;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (IsPostBack)
            {
                string strIds = Request.Form["SelectedIds"];
                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    var arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };

                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        string t = arrids[idx];
                        var idParts = t.Split('_');
                        switch (idParts[0])
                        {
                            case "Product":
                                if (idParts[1] != "-1")
                                {
                                    ids[idx] = idParts[1];
                                }
                                else
                                {
                                    _selectionFilter.IncludeValues = false;
                                    _inverseSelection = true;
                                }
                                break;
                            default:
                                _inverseSelection = true;
                                break;
                        }
                    }
                    _selectionFilter.Values = ids.Distinct().Where(item => item != null).ToArray();
                }
            }
            else
            {
                ErrorsGrid.Visible = false;
            }
            _statusList = OrderStatusService.GetOrderStatuses();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadSettings();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void LoadSettings()
        {

            txtNumberPhoneAdmin.Text = ModuleSettingsProvider.GetSettingValue<string>("NumberPhoneAdmin", _moduleName);

            ckbSendSmsNewOrder.Checked = ModuleSettingsProvider.GetSettingValue<bool>("SendSmsNewOrder", _moduleName);
            txtTextSmsNewOrder.Text = ModuleSettingsProvider.GetSettingValue<string>("TextSmsNewOrder", _moduleName);
            ckbSendNewOrderAdmin.Checked = ModuleSettingsProvider.GetSettingValue<bool>("SendNewOrderAdmin", _moduleName);

            ckbSendSmsChangeStatus.Checked = ModuleSettingsProvider.GetSettingValue<bool>("SendSmsChangeStatus", _moduleName);
            ckbSendChangeStatusAdmin.Checked = ModuleSettingsProvider.GetSettingValue<bool>("SendChangeStatusAdmin", _moduleName);

            ckbSendSmsOrderPhone.Checked = ModuleSettingsProvider.GetSettingValue<bool>("SendSmsOrderPhone", _moduleName);

            var smsservice = ModuleSettingsProvider.GetSettingValue<string>("SmsService", _moduleName);
            if (ddlSmsService.Items.FindByValue(smsservice) != null)
                ddlSmsService.SelectedValue = smsservice;
            else
                ddlSmsService.Items[0].Selected = true;

            txtWwwSms4BRuLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSms4BRuLogin", _moduleName);
            txtWwwSms4BRuPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSms4BRuPassword", _moduleName);
            txtWwwSms4BRuSender.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSms4BRuSender", _moduleName);


            txtStreamTelecomLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("StreamTelecomLogin", _moduleName);
            txtStreamTelecomPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("StreamTelecomPassword", _moduleName);
            txtStreamTelecomSender.Text = ModuleSettingsProvider.GetSettingValue<string>("StreamTelecomSender", _moduleName);

            txtSmslabRuLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("SmslabRuLogin", _moduleName);
            txtSmslabRuPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("SmslabRuPassword", _moduleName);
            txtSmslabRuSender.Text = ModuleSettingsProvider.GetSettingValue<string>("SmslabRuSender", _moduleName);

            txtWwwSmsimpleRuLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSmsimpleRuLogin", _moduleName);
            txtWwwSmsimpleRuPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSmsimpleRuPassword", _moduleName);
            txtWwwSmsimpleRuOriginId.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSmsimpleRuOriginId", _moduleName);

            txtGsmInformRuLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("GsmInformRuLogin", _moduleName);
            txtGsmInformRuPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("GsmInformRuPassword", _moduleName);
            txtGsmInformRuSender.Text = ModuleSettingsProvider.GetSettingValue<string>("GsmInformRuSender", _moduleName);

            txtWwwIqsmsRuLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwIqsmsRuLogin", _moduleName);
            txtWwwIqsmsRuPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwIqsmsRuPassword", _moduleName);
            txtWwwIqsmsRuSender.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwIqsmsRuSender", _moduleName);

            txtLeninsmsRuLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("LeninsmsRuLogin", _moduleName);
            txtLeninsmsRuApiKey.Text = ModuleSettingsProvider.GetSettingValue<string>("LeninsmsRuApiKey", _moduleName);
            txtLeninsmsRuSender.Text = ModuleSettingsProvider.GetSettingValue<string>("LeninsmsRuSender", _moduleName);

            txtWwwSmspilotRuLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSmspilotRuLogin", _moduleName);
            txtWwwSmspilotRuPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSmspilotRuPassword", _moduleName);
            txtWwwSmspilotRuSender.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwSmspilotRuSender", _moduleName);

            txtRuSmsOnlineComLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("RuSmsOnlineComLogin", _moduleName);
            txtRuSmsOnlineComSecretKey.Text = ModuleSettingsProvider.GetSettingValue<string>("RuSmsOnlineComSecretKey", _moduleName);
            txtRuSmsOnlineComSender.Text = ModuleSettingsProvider.GetSettingValue<string>("RuSmsOnlineComSender", _moduleName);

            txtWwwUnisenderComApiKey.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwUnisenderComApiKey", _moduleName);
            txtWwwUnisenderComSender.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwUnisenderComSender", _moduleName);

            txtWwwEpochtaRuApiKey.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwEpochtaRuApiKey", _moduleName);
            txtWwwEpochtaRuPrivatKey.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwEpochtaRuPrivatKey", _moduleName);
            txtWwwEpochtaRuSender.Text = ModuleSettingsProvider.GetSettingValue<string>("WwwEpochtaRuSender", _moduleName);

            var table = SmsNotificationsStatus.GetDataStatus();
            DataColumn col = new DataColumn("IsSelected", typeof(bool));
            table.Columns.Add(col);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (var i = 0; i <= table.Rows.Count - 1; i++)
                {
                    var intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == table.Rows[intIndex]["ID"].ToString()))
                    {
                        table.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }
            grid.DataSource = table;
            grid.DataBind();
            lblProducts.Text = table.Rows.Count.ToString();
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ErrorsGrid.Visible = false;
            if (e.CommandName == "deleteStatus")
            {
                SmsNotificationsStatus.DeleteStatusById(Convert.ToInt32(e.CommandArgument));
            }
            else if (e.CommandName == "AddStatus")
            {
                var id = ((DropDownList)grid.FooterRow.FindControl("ddlStatus")).SelectedValue;
                if (SmsNotificationsStatus.GetStatus(id.TryParseInt()) != null)
                {
                    ErrorsGrid.Text = "Шаблон с таким статусом уже существует, обновите страницу";
                    ErrorsGrid.Visible = true;
                    return;
                }
                if (id.TryParseInt() < 1)
                {
                    ErrorsGrid.Text = "Неверный статус, обновите страницу";
                    ErrorsGrid.Visible = true;
                    return;
                }
                var status = new StatusItem() { Status = id.TryParseInt() };
                status.Content = ((TextBox)grid.FooterRow.FindControl("fckContent")).Text;
                status.Enabled = ((CheckBox)grid.FooterRow.FindControl("chkEnabled")).Checked;
                SmsNotificationsStatus.AddStatus(status);
                grid.ShowFooter = false;
            }
            else if (e.CommandName == "CancelAdd")
            {
                grid.ShowFooter = false;
                grid.FooterStyle.BackColor = Color.FromName("#e1e1e1");
            }
            else if (e.CommandName == "updateStatus")
            {
                var status = SmsNotificationsStatus.GetStatus(e.CommandArgument.ToString().TryParseInt());
                if (status == null)
                {
                    ErrorsGrid.Text = "Шаблон с таким статусом не существует, обновите страницу";
                    ErrorsGrid.Visible = true;
                    return;
                }
                var row = ((GridView)sender).Rows;
                for (int i = 0; i < row.Count; i++)
                {
                    if (((Label)row[i].FindControl("Label0")).Text == e.CommandArgument.ToString())
                    {
                        status.Content = ((TextBox)row[i].FindControl("fckContentEdit")).Text;
                        status.Enabled = ((CheckBox)row[i].FindControl("chkEnabledEdit")).Checked;
                    }
                }
                SmsNotificationsStatus.UpdateStatus(status);
            }
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null) return;
            var t = (DataRowView)e.Row.DataItem;

            ((LinkButton)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).CommandName =
                "deleteStatus";

            var tr = new AsyncPostBackTrigger
            {
                ControlID = ((e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).UniqueID,
                EventName = "Click"
            };

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    ((DropDownList)e.Row.FindControl("ddlStatus")).SelectedValue =
            //        ((DataRowView)e.Row.DataItem)[0].ToString();

            UpdatePanel1.Triggers.Add(tr);
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            string[] str = Request.Form["SelectedIds"].Split(' ');
            foreach (string item in str)
            {
                if (!string.IsNullOrEmpty(item) && Request.Form["SelectedIds"] != "-1")
                    SmsNotificationsStatus.DeleteStatusById(Convert.ToInt32(item));
            }
            if (Equals(Request.Form["SelectedIds"], "-1"))
            {
                DataTable table = SmsNotificationsStatus.GetDataStatus();
                foreach (DataRow item in table.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString()))
                    {
                        SmsNotificationsStatus.DeleteStatusById(Convert.ToInt32(item[0].ToString()));
                    }
                }
            }
        }

        protected void lbActivateSelected_Click(object sender, EventArgs e)
        {
            string[] str = Request.Form["SelectedIds"].Split(' ');
            foreach (string item in str)
            {
                if (!string.IsNullOrEmpty(item) && !Equals(item, "-1"))
                {
                    StatusItem status = SmsNotificationsStatus.GetStatus(Convert.ToInt32(item));
                    status.Enabled = true;
                    SmsNotificationsStatus.UpdateStatus(status);
                }
            }
            if (Equals(Request.Form["SelectedIds"], "-1"))
            {
                DataTable table = SmsNotificationsStatus.GetDataStatus();
                foreach (DataRow item in table.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString()))
                    {
                        StatusItem status = SmsNotificationsStatus.GetStatus(Convert.ToInt32(item[0].ToString()));
                        status.Enabled = true;
                        SmsNotificationsStatus.UpdateStatus(status);
                    }
                }
            }
        }

        protected void lbDectivateSelected_Click(object sender, EventArgs e)
        {
            string[] str = Request.Form["SelectedIds"].Split(' ');
            foreach (string item in str)
            {
                if (!string.IsNullOrEmpty(item) && !Equals(item, "-1"))
                {
                    StatusItem status = SmsNotificationsStatus.GetStatus(Convert.ToInt32(item));
                    status.Enabled = false;
                    SmsNotificationsStatus.UpdateStatus(status);
                }
            }
            if (Equals(Request.Form["SelectedIds"], "-1"))
            {
                DataTable table = SmsNotificationsStatus.GetDataStatus();
                foreach (DataRow item in table.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString()))
                    {
                        StatusItem status = SmsNotificationsStatus.GetStatus(Convert.ToInt32(item[0].ToString()));
                        status.Enabled = false;
                        SmsNotificationsStatus.UpdateStatus(status);
                    }
                }
            }
        }

        protected void ddlStatus_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Clear();
            var statusList = OrderStatusService.GetOrderStatuses();
            if (statusList != null && statusList.Count > 0)
            {
                var temp = statusList.Select(x => new ListItem() { Text = x.StatusName, Value = x.StatusID.ToString() }).ToList();
                ((DropDownList)sender).Items.AddRange(temp.ToArray());
            }
        }

        protected void btnAddResource_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = Color.FromName("#ccffcc");
        }

        protected void Save()
        {
            var errMessage = new StringBuilder();
            const string tpl = "<li>{0}</li>";
            bool isExistErr = false;

            errMessage.Append("<ul style=\"padding:0; margin:0;\">");


            if (!string.IsNullOrEmpty(txtNumberPhoneAdmin.Text))
            {
                foreach (var phone in txtNumberPhoneAdmin.Text.Split(','))
                {
                    long temp;
                    if (!long.TryParse(phone, out temp) || phone.Length != 11)
                    {
                        isExistErr = true;
                        errMessage.AppendFormat(tpl, GetLocalResourceObject("SmsNotificationsValid_NumberPhoneAdmin"));
                    }
                }
            }

            if (ckbSendNewOrderAdmin.Checked || ckbSendChangeStatusAdmin.Checked)
            {
                if (string.IsNullOrEmpty(txtNumberPhoneAdmin.Text))
                {
                    isExistErr = true;
                    errMessage.AppendFormat(tpl, GetLocalResourceObject("SmsNotificationsValid_PhoneAdmin"));
                }
            }

            if (ckbSendSmsNewOrder.Checked && string.IsNullOrEmpty(txtTextSmsNewOrder.Text))
            {
                isExistErr = true;
                errMessage.AppendFormat(tpl, GetLocalResourceObject("SmsNotificationsValid_TextSmsNewOrder"));
            }

            errMessage.Append("</ul>");

            if (isExistErr)
            {
                lblErr.Text = errMessage.ToString();
                lblErr.ForeColor = Color.Red;
                lblErr.Visible = true;
                return;
            }

            ModuleSettingsProvider.SetSettingValue("NumberPhoneAdmin", txtNumberPhoneAdmin.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("SendSmsNewOrder", ckbSendSmsNewOrder.Checked, _moduleName);
            ModuleSettingsProvider.SetSettingValue("TextSmsNewOrder", txtTextSmsNewOrder.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SendNewOrderAdmin", ckbSendNewOrderAdmin.Checked, _moduleName);

            ModuleSettingsProvider.SetSettingValue("SendSmsChangeStatus", ckbSendSmsChangeStatus.Checked, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SendChangeStatusAdmin", ckbSendChangeStatusAdmin.Checked,
                                                   _moduleName);

            ModuleSettingsProvider.SetSettingValue("SendSmsOrderPhone", ckbSendSmsOrderPhone.Checked, _moduleName);

            ModuleSettingsProvider.SetSettingValue("SmsService", ddlSmsService.SelectedValue, _moduleName);

            ModuleSettingsProvider.SetSettingValue("WwwSms4BRuLogin", txtWwwSms4BRuLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwSms4BRuPassword", txtWwwSms4BRuPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwSms4BRuSender", txtWwwSms4BRuSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("SmslabRuLogin", txtSmslabRuLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SmslabRuPassword", txtSmslabRuPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SmslabRuSender", txtSmslabRuSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("StreamTelecomLogin", txtStreamTelecomLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("StreamTelecomPassword", txtStreamTelecomPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("StreamTelecomSender", txtStreamTelecomSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("WwwSmsimpleRuLogin", txtWwwSmsimpleRuLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwSmsimpleRuPassword", txtWwwSmsimpleRuPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwSmsimpleRuOriginId", txtWwwSmsimpleRuOriginId.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("GsmInformRuLogin", txtGsmInformRuLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("GsmInformRuPassword", txtGsmInformRuPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("GsmInformRuSender", txtGsmInformRuSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("WwwIqsmsRuLogin", txtWwwIqsmsRuLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwIqsmsRuPassword", txtWwwIqsmsRuPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwIqsmsRuSender", txtWwwIqsmsRuSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("LeninsmsRuLogin", txtLeninsmsRuLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("LeninsmsRuApiKey", txtLeninsmsRuApiKey.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("LeninsmsRuSender", txtLeninsmsRuSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("WwwSmspilotRuLogin", txtWwwSmspilotRuLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwSmspilotRuPassword", txtWwwSmspilotRuPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwSmspilotRuSender", txtWwwSmspilotRuSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("RuSmsOnlineComLogin", txtRuSmsOnlineComLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("RuSmsOnlineComSecretKey", txtRuSmsOnlineComSecretKey.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("RuSmsOnlineComSender", txtRuSmsOnlineComSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("WwwUnisenderComApiKey", txtWwwUnisenderComApiKey.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwUnisenderComSender", txtWwwUnisenderComSender.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("WwwEpochtaRuApiKey", txtWwwEpochtaRuApiKey.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwEpochtaRuPrivatKey", txtWwwEpochtaRuPrivatKey.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("WwwEpochtaRuSender", txtWwwEpochtaRuSender.Text, _moduleName);

            lblMessage.Text = (String)GetLocalResourceObject("SmsNotifications_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;

            LoadSettings();
        }

        protected void btnUnisenderReg_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("SmsService", ddlSmsService.SelectedValue, _moduleName);

            var apiKey = new UnisenderCom().Register(txtUniSenderEmail.Text, txtUnisenderLogin.Text, txtUnisenderPassword.Text);
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                ModuleSettingsProvider.SetSettingValue("WwwUnisenderComApiKey", apiKey, _moduleName);
                txtWwwUnisenderComApiKey.Text = apiKey;

                lblMessage.Text = "Регистрация прошла успешно. <a href='https://cp.unisender.com' target='_blank'>Перейти в кабинет</a>";
                lblMessage.ForeColor = Color.Blue;
                lblMessage.Visible = true;
                lblErr.Visible = false;

                txtUniSenderEmail.Text = "";
                txtUnisenderLogin.Text = "";
                txtUnisenderPassword.Text = "";

            }
            else
            {
                lblErr.Text = "Ошибка при регистрации";
                lblErr.Visible = true;
                lblMessage.Visible = false;
            }

        }
    }
}