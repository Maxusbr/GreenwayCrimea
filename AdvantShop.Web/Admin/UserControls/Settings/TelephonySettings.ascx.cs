using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.CallBack;
using Resources;

namespace Admin.UserControls.Settings
{
    public partial class TelephonySettings : UserControl
    {
        public string ErrMessage = Resource.Admin_CommonSettings_InvalidNotify;

        protected void Page_Init(object sender, EventArgs e)
        {
            tdWorkSchedule.Controls.Add(new LiteralControl
            {
                Text = string.Format("<div><table style=\"margin-bottom: 3px;\">" +
                                     "<tr style=\"border-bottom: 1px solid gray;\">" +
                                     "<td style=\"width: 50px;\">{0}</td>" +
                                     "<td style=\"width: 72px;\">{1}</td>" +
                                     "<td style=\"width: 55px;\">{2}</td>" +
                                     "</tr></table></div>",
                                     Resource.Admin_CommonSettings_CallBack_WorkSchedule_Day,
                                     Resource.Admin_CommonSettings_CallBack_WorkSchedule_From,
                                     Resource.Admin_CommonSettings_CallBack_WorkSchedule_To)
            });
            var culture = AdvantShop.Localization.Culture.CurrentCulture();
            var offset = (int)culture.DateTimeFormat.FirstDayOfWeek;

            var hours = new List<int>();
            for (int  i = 0;  i < 24;  i++)
            {
                hours.Add(i);
            }
            for (int i = 0; i < 7; i++)
            {
                var day = (DayOfWeek) (i + offset < 7 ? i + offset : i + offset - 7);
                
                var div = new Panel();
                div.Controls.Add(new CheckBox {ID = "chkEnabled" + day});
                div.Controls.Add(new Label
                {
                    ID = "lbl" + day,
                    AssociatedControlID = "chkEnabled" + day,
                    Text = culture.DateTimeFormat.GetShortestDayName(day) + ": ",
                    CssClass = "shortDayName"
                });
                
                var ddlHoursFrom = new DropDownList {ID = "ddlHoursFrom" + day};
                ddlHoursFrom.Items.AddRange(
                    hours.Select(h => new ListItem(h.ToString().PadLeft(2, '0'), h.ToString())).ToArray());
                div.Controls.Add(ddlHoursFrom);
                div.Controls.Add(new Label {Text = ":00"});

                div.Controls.Add(new Label {Text = " - "});

                var ddlHoursTo = new DropDownList {ID = "ddlHoursTo" + day};
                ddlHoursTo.Items.AddRange(
                    hours.Select(h => new ListItem(h.ToString().PadLeft(2, '0'), h.ToString())).ToArray());
                div.Controls.Add(ddlHoursTo);
                div.Controls.Add(new Label {Text = ":00"});

                tdWorkSchedule.Controls.Add(div);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            ddlOperatorType.SelectedValue = SettingsTelephony.CurrentIPTelephonyOperatorType.ToString();
            chkPhonerLiteActive.Checked = SettingsTelephony.PhonerLiteActive;

            txtTelphinAppKey.Text = SettingsTelephony.TelphinAppKey;
            txtTelphinAppSecret.Text = SettingsTelephony.TelphinAppSecret;
            txtCallBackTelphinExtension.Text = SettingsTelephony.CallBackTelphinExtension;
            txtCallBackTelphinLocalNumber.Text = SettingsTelephony.CallBackTelphinLocalNumber;

            txtMangoApiKey.Text = SettingsTelephony.MangoApiKey;
            txtMangoSecretKey.Text = SettingsTelephony.MangoSecretKey;
            txtMangoApiUrl.Text = SettingsTelephony.MangoApiUrl;
            txtCallBackMangoExtension.Text = SettingsTelephony.CallBackMangoExtension;

            txtSipuniApiKey.Text = SettingsTelephony.SipuniApiKey;
            chkSipuniConsiderInnerCalls.Checked = SettingsTelephony.SipuniConsiderInnerCalls;
            chkSipuniWebPhoneEnabled.Checked = SettingsTelephony.SipuniWebPhoneEnabled;
            txtSipuniWebPhoneWidget.Text = SettingsTelephony.SipuniWebPhoneWidget;
            txtCallBackSipuniAccount.Text = SettingsTelephony.CallBackSipuniAccount;
            txtCallBackSipuniShortNumber.Text = SettingsTelephony.CallBackSipuniShortNumber;
            txtCallBackSipuniTree.Text = SettingsTelephony.CallBackSipuniTree;
            ddlCallbackSipuniType.SelectedValue = SettingsTelephony.CallBackSipuniType.ToString();

            chkEnableCallBack.Checked = SettingsTelephony.CallBackEnabled;
            txtCallBackTimeInterval.Text = SettingsTelephony.CallBackTimeInterval.ToString();
            ddlCallBackShowMode.SelectedValue = SettingsTelephony.CallBackShowMode.ToString();
            txtCallBackWorkTimeText.Text = SettingsTelephony.CallBackWorkTimeText;
            txtCallBackNotWorkTimeText.Text = SettingsTelephony.CallBackNotWorkTimeText;
            
            var schedule = WorkSchedule.Schedule;
            foreach (DayOfWeek day in Enum.GetValues(typeof (DayOfWeek)))
            {
                var workTime = schedule.Get(day);
                ((CheckBox) tdWorkSchedule.FindControl("chkEnabled" + day)).Checked = workTime.Enabled;
                ((DropDownList) tdWorkSchedule.FindControl("ddlHoursFrom" + day)).SelectedValue =
                    workTime.From.Hours.ToString();
                ((DropDownList) tdWorkSchedule.FindControl("ddlHoursTo" + day)).SelectedValue =
                    workTime.To.Hours.ToString();
            }
        }

        public bool SaveData()
        {
            if (!ValidateData())
                return false;

            SettingsTelephony.CurrentIPTelephonyOperatorType = ddlOperatorType.SelectedValue.TryParseEnum<EOperatorType>();
            SettingsTelephony.PhonerLiteActive = chkPhonerLiteActive.Checked;

            SettingsTelephony.TelphinAppKey = txtTelphinAppKey.Text.Trim();
            SettingsTelephony.TelphinAppSecret = txtTelphinAppSecret.Text.Trim();
            SettingsTelephony.CallBackTelphinExtension = txtCallBackTelphinExtension.Text.Trim();
            SettingsTelephony.CallBackTelphinLocalNumber = txtCallBackTelphinLocalNumber.Text.Trim();

            SettingsTelephony.MangoApiKey = txtMangoApiKey.Text.Trim();
            SettingsTelephony.MangoSecretKey = txtMangoSecretKey.Text.Trim();
            SettingsTelephony.MangoApiUrl = txtMangoApiUrl.Text.Trim();
            SettingsTelephony.CallBackMangoExtension = txtCallBackMangoExtension.Text.Trim();

            SettingsTelephony.SipuniApiKey = txtSipuniApiKey.Text.Trim();
            SettingsTelephony.SipuniConsiderInnerCalls = chkSipuniConsiderInnerCalls.Checked;
            SettingsTelephony.SipuniWebPhoneEnabled = chkSipuniWebPhoneEnabled.Checked;
            SettingsTelephony.SipuniWebPhoneWidget = txtSipuniWebPhoneWidget.Text;
            SettingsTelephony.CallBackSipuniAccount = txtCallBackSipuniAccount.Text;
            SettingsTelephony.CallBackSipuniTree = txtCallBackSipuniTree.Text;
            SettingsTelephony.CallBackSipuniType = ddlCallbackSipuniType.SelectedValue.TryParseInt();

            SettingsTelephony.CallBackEnabled = chkEnableCallBack.Checked;
            SettingsTelephony.CallBackTimeInterval = txtCallBackTimeInterval.Text.TryParseInt();
            SettingsTelephony.CallBackShowMode = ddlCallBackShowMode.SelectedValue.TryParseEnum<ECallBackShowMode>();
            SettingsTelephony.CallBackWorkTimeText = txtCallBackWorkTimeText.Text;
            SettingsTelephony.CallBackNotWorkTimeText = txtCallBackNotWorkTimeText.Text;

            var shortNumber = txtCallBackSipuniShortNumber.Text.TryParseInt();
            SettingsTelephony.CallBackSipuniShortNumber = shortNumber >= 100 && shortNumber <= 999
                ? shortNumber.ToString()
                : string.Empty;

            var schedule = WorkSchedule.Schedule;
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var chkEnabled = (CheckBox)tdWorkSchedule.FindControl("chkEnabled" + day);
                var ddlHoursFrom = (DropDownList)tdWorkSchedule.FindControl("ddlHoursFrom" + day);
                var ddlHoursTo = (DropDownList)tdWorkSchedule.FindControl("ddlHoursTo" + day);

                schedule.Set(day, new WorkTime(chkEnabled.Checked, ddlHoursFrom.SelectedValue.TryParseInt(),
                    ddlHoursTo.SelectedValue.TryParseInt()));
            }
            WorkSchedule.Schedule = schedule;

            LoadData();

            return true;
        }

        private bool ValidateData()
        {
            return true;
        }
    }
}