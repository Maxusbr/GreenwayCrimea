using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.CallBack;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.TelephonySettings
{
    public class GetIPTelephonySettings
    {
        public IPTelephonySettingsModel Execute()
        {
            var model = new IPTelephonySettingsModel()
            {
                CurrentIPTelephonyOperatorType = SettingsTelephony.CurrentIPTelephonyOperatorType,
                PhonerLiteActive = SettingsTelephony.PhonerLiteActive,

                CallBackEnabled = SettingsTelephony.CallBackEnabled,
                CallBackTimeInterval = SettingsTelephony.CallBackTimeInterval,
                CallBackShowMode = SettingsTelephony.CallBackShowMode,
                CallBackWorkTimeText = SettingsTelephony.CallBackWorkTimeText,
                CallBackNotWorkTimeText = SettingsTelephony.CallBackNotWorkTimeText,
                Schedule = WorkSchedule.Schedule,
                ScheduleSerialized = WorkSchedule.Schedule.ToString(),

                SipuniApiKey = SettingsTelephony.SipuniApiKey,
                SipuniConsiderInnerCalls = SettingsTelephony.SipuniConsiderInnerCalls,
                CallBackSipuniAccount = SettingsTelephony.CallBackSipuniAccount,
                CallBackSipuniShortNumber = SettingsTelephony.CallBackSipuniShortNumber,
                CallBackSipuniTree = SettingsTelephony.CallBackSipuniTree,
                CallBackSipuniType = SettingsTelephony.CallBackSipuniType,

                MangoApiKey = SettingsTelephony.MangoApiKey,
                MangoSecretKey = SettingsTelephony.MangoSecretKey,
                MangoApiUrl = SettingsTelephony.MangoApiUrl,
                CallBackMangoExtension = SettingsTelephony.CallBackMangoExtension,

                TelphinAppKey = SettingsTelephony.TelphinAppKey,
                TelphinAppSecret = SettingsTelephony.TelphinAppSecret,
                TelphinExtensions = SettingsTelephony.TelphinExtensions ?? "[]",
                CallBackTelphinExtension = SettingsTelephony.CallBackTelphinExtension,
                CallBackTelphinLocalNumber = SettingsTelephony.CallBackTelphinLocalNumber,

                ZadarmaKey = SettingsTelephony.ZadarmaKey,
                ZadarmaSecret = SettingsTelephony.ZadarmaSecret,
                CallBackZadarmaPhone = SettingsTelephony.CallBackZadarmaPhone
            };

            model.IPTelephonyOperatorTypes = Enum.GetValues(typeof(EOperatorType)).Cast<EOperatorType>()
                .Select(x => new SelectListItem
                {
                    Text = x.Localize(),
                    Value = x.ToString(),
                    Selected = x == model.CurrentIPTelephonyOperatorType
                }).ToList();

            model.CallBackShowModes = Enum.GetValues(typeof(ECallBackShowMode)).Cast<ECallBackShowMode>()
                .Select(x => new SelectListItem
                {
                    Text = x.Localize(),
                    Value = x.ToString(),
                    Selected = x == model.CallBackShowMode
                }).ToList();

            return model;
        }
    }
}
