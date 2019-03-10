using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.CallBack;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Admin.Models.Settings;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Handlers.Settings.TelephonySettings
{
    public class SaveIPTelephonySettings
    {
        private IPTelephonySettingsModel _model;

        public SaveIPTelephonySettings(IPTelephonySettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            SettingsTelephony.CurrentIPTelephonyOperatorType = _model.CurrentIPTelephonyOperatorType;
            SettingsTelephony.PhonerLiteActive = _model.PhonerLiteActive;

            SettingsTelephony.CallBackEnabled = _model.CallBackEnabled;
            SettingsTelephony.CallBackTimeInterval = _model.CallBackTimeInterval;
            SettingsTelephony.CallBackShowMode = _model.CallBackShowMode;
            SettingsTelephony.CallBackWorkTimeText = _model.CallBackWorkTimeText;
            SettingsTelephony.CallBackNotWorkTimeText = _model.CallBackNotWorkTimeText;
            WorkSchedule.Schedule = JsonConvert.DeserializeObject<WorkSchedule>(_model.ScheduleSerialized);

            SettingsTelephony.SipuniApiKey = _model.SipuniApiKey;
            SettingsTelephony.SipuniConsiderInnerCalls = _model.SipuniConsiderInnerCalls;
            SettingsTelephony.CallBackSipuniAccount = _model.CallBackSipuniAccount;
            var shortNumber = _model.CallBackSipuniShortNumber.TryParseInt();
            SettingsTelephony.CallBackSipuniShortNumber = shortNumber >= 100 && shortNumber <= 999 ? shortNumber.ToString() : string.Empty;
            SettingsTelephony.CallBackSipuniTree = _model.CallBackSipuniTree;
            SettingsTelephony.CallBackSipuniType = _model.CallBackSipuniType;

            SettingsTelephony.MangoApiKey = _model.MangoApiKey;
            SettingsTelephony.MangoSecretKey = _model.MangoSecretKey;
            SettingsTelephony.MangoApiUrl = _model.MangoApiUrl;
            SettingsTelephony.CallBackMangoExtension = _model.CallBackMangoExtension;

            SettingsTelephony.TelphinAppKey = _model.TelphinAppKey;
            SettingsTelephony.TelphinAppSecret = _model.TelphinAppSecret;
            SettingsTelephony.CallBackTelphinExtension = _model.CallBackTelphinExtension;
            SettingsTelephony.CallBackTelphinExtensionId = string.Empty;
            SettingsTelephony.CallBackTelphinLocalNumber = _model.CallBackTelphinLocalNumber;
            if (_model.CurrentIPTelephonyOperatorType == EOperatorType.Telphin && _model.CallBackTelphinExtension.IsNotEmpty())
            {
                var ext = new TelphinHandler().GetExtension(_model.CallBackTelphinExtension);
                if (ext == null)
                    throw new BlException(LocalizationService.GetResource("Admin.SettingsTelephony.Errors.CallbackTelphinExt.NotFound"), "CallBackTelphinExtension");
                if (ext.Type != "phone")
                    throw new BlException(LocalizationService.GetResource("Admin.SettingsTelephony.Errors.CallbackTelphinExt.WrongType"), "CallBackTelphinExtension");
                SettingsTelephony.CallBackTelphinExtensionId = ext.Id;
            }

            SettingsTelephony.ZadarmaKey = _model.ZadarmaKey;
            SettingsTelephony.ZadarmaSecret = _model.ZadarmaSecret;
            SettingsTelephony.CallBackZadarmaPhone = _model.CallBackZadarmaPhone;
        }
    }
}
