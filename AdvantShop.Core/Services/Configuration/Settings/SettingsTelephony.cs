using System;
using System.Collections.Generic;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.CallBack;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;

namespace AdvantShop.Configuration
{
    public class SettingsTelephony
    {
        public static EOperatorType CurrentIPTelephonyOperatorType
        {
            get { return SettingProvider.Items["IPTelephonyOperatorType"].TryParseEnum<EOperatorType>(); }
            set { SettingProvider.Items["IPTelephonyOperatorType"] = value.ToString(); }
        }

        public static bool PhonerLiteActive
        {
            get { return Convert.ToBoolean(SettingProvider.Items["PhonerLiteActive"]); }
            set { SettingProvider.Items["PhonerLiteActive"] = value.ToString(); }
        }

        #region Sipuni
        
        public static string SipuniApiKey
        {
            get { return SettingProvider.Items["SipuniApiKey"]; }
            set { SettingProvider.Items["SipuniApiKey"] = value; }
        }

        public static bool SipuniConsiderInnerCalls
        {
            get { return Convert.ToBoolean(SettingProvider.Items["Telephony.Sipuni.ConsiderInnerCalls"]); }
            set { SettingProvider.Items["Telephony.Sipuni.ConsiderInnerCalls"] = value.ToString(); }
        }
        
        public static bool SipuniWebPhoneEnabled
        {
            get { return SettingProvider.Items["SipuniWebPhoneEnabled"].TryParseBool(); }
            set { SettingProvider.Items["SipuniWebPhoneEnabled"] = value.ToString(); }
        }
        
        public static string SipuniWebPhoneWidget
        {
            get { return SettingProvider.Items["SipuniWebPhoneWidget"]; }
            set { SettingProvider.Items["SipuniWebPhoneWidget"] = value; }
        }
        

        public static string CallBackSipuniAccount
        {
            get { return SettingProvider.Items["Telephony.CallBack.SipuniAccount"]; }
            set { SettingProvider.Items["Telephony.CallBack.SipuniAccount"] = value; }
        }

        public static string CallBackSipuniShortNumber
        {
            get { return SettingProvider.Items["Telephony.CallBack.ShortNumber"]; }
            set { SettingProvider.Items["Telephony.CallBack.ShortNumber"] = value; }
        }

        public static string CallBackSipuniTree
        {
            get { return SettingProvider.Items["Telephony.CallBack.Tree"]; }
            set { SettingProvider.Items["Telephony.CallBack.Tree"] = value; }
        }

        /// <summary>
        /// Тип звонка 
        /// 0 - Звонок с внутреннего номера на внешний номер (мобильный или городской) call_number;
        /// 1 - Звонок на внешний номер через схему - call_tree;
        /// 2 - Звонок с внешнего номера на другой внешний номер - call_external;
        /// </summary>
        public static int CallBackSipuniType
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["Telephony.CallBack.Type"]); }
            set { SettingProvider.Items["Telephony.CallBack.Type"] = value.ToString(); }
        }

        #endregion


        #region Telphin

        public static string TelphinAppKey
        {
            get { return SettingProvider.Items["Telphin.AppKey"]; }
            set { SettingProvider.Items["Telphin.AppKey"] = value; }
        }

        public static string TelphinAppSecret
        {
            get { return SettingProvider.Items["Telphin.AppSecret"]; }
            set { SettingProvider.Items["Telphin.AppSecret"] = value; }
        }

        public static string TelphinExtensions
        {
            get { return SettingProvider.Items["Telphin.Extensions"]; }
            set { SettingProvider.Items["Telphin.Extensions"] = value; }
        }

        public static string CallBackTelphinExtension
        {
            get { return SettingProvider.Items["CallBack.Telphin.Extension"]; }
            set { SettingProvider.Items["CallBack.Telphin.Extension"] = value; }
        }

        public static string CallBackTelphinExtensionId
        {
            get { return SettingProvider.Items["CallBack.Telphin.ExtensionId"]; }
            set { SettingProvider.Items["CallBack.Telphin.ExtensionId"] = value; }
        }

        public static string CallBackTelphinLocalNumber
        {
            get { return SettingProvider.Items["CallBack.Telphin.LocalNumber"]; }
            set { SettingProvider.Items["CallBack.Telphin.LocalNumber"] = value; }
        }

        #endregion


        #region Mango

        public static string MangoApiKey
        {
            get { return SettingProvider.Items["Mango.ApiKey"]; }
            set { SettingProvider.Items["Mango.ApiKey"] = value; }
        }

        public static string MangoSecretKey
        {
            get { return SettingProvider.Items["Mango.SecretKey"]; }
            set { SettingProvider.Items["Mango.SecretKey"] = value; }
        }

        public static string MangoApiUrl
        {
            get { return SettingProvider.Items["Mango.ApiUrl"]; }
            set { SettingProvider.Items["Mango.ApiUrl"] = value; }
        }

        public static string CallBackMangoExtension
        {
            get { return SettingProvider.Items["CallBack.Mango.Extension"]; }
            set { SettingProvider.Items["CallBack.Mango.Extension"] = value; }
        }

        #endregion


        #region Zadarma

        public static string ZadarmaKey
        {
            get { return SettingProvider.Items["Zadarma.Key"]; }
            set { SettingProvider.Items["Zadarma.Key"] = value; }
        }

        public static string ZadarmaSecret
        {
            get { return SettingProvider.Items["Zadarma.Secret"]; }
            set { SettingProvider.Items["Zadarma.Secret"] = value; }
        }

        public static string CallBackZadarmaPhone
        {
            get { return SettingProvider.Items["CallBack.Zadarma.Phone"]; }
            set { SettingProvider.Items["CallBack.Zadarma.Phone"] = value; }
        }

        #endregion


        #region Callback

        public static bool CallBackEnabled
        {
            get { return SettingProvider.Items["Telephony.CallBack.Enabled"].TryParseBool(); }
            set { SettingProvider.Items["Telephony.CallBack.Enabled"] = value.ToString(); }
        }

        public static int CallBackTimeInterval
        {
            get { return SettingProvider.Items["Telephony.CallBack.TimeInterval"].TryParseInt(); }
            set { SettingProvider.Items["Telephony.CallBack.TimeInterval"] = value.ToString(); }
        }

        public static string CallBackWorkSchedule
        {
            get { return SettingProvider.Items["Telephony.CallBack.WorkSchedule"]; }
            set { SettingProvider.Items["Telephony.CallBack.WorkSchedule"] = value; }
        }

        public static ECallBackShowMode CallBackShowMode
        {
            get { return SettingProvider.Items["CallBack.ShowMode"].TryParseEnum<ECallBackShowMode>(); }
            set { SettingProvider.Items["CallBack.ShowMode"] = value.ToString(); }
        }

        public static string CallBackWorkTimeText
        {
            get { return SettingProvider.Items["CallBack.WorkTimeText"]; }
            set { SettingProvider.Items["CallBack.WorkTimeText"] = value; }
        }

        public static string CallBackWorkTimeTextFormatted
        {
            get
            {
                return GlobalStringVariableService.TranslateExpression(CallBackWorkTimeText,
                    new List<SeoToken>()
                    {
                        new SeoToken("#SECONDS#",
                            string.Format("{0} {1}", CallBackTimeInterval, Strings.Numerals(CallBackTimeInterval,
                                LocalizationService.GetResource("Core.Configuration.SettingsTelephony.Seconds0"),
                                LocalizationService.GetResource("Core.Configuration.SettingsTelephony.Seconds1"),
                                LocalizationService.GetResource("Core.Configuration.SettingsTelephony.Seconds2"),
                                LocalizationService.GetResource("Core.Configuration.SettingsTelephony.Seconds5"))))

                    });
            }
        }

        public static string CallBackNotWorkTimeText
        {
            get { return SettingProvider.Items["CallBack.NotWorkTimeText"]; }
            set { SettingProvider.Items["CallBack.NotWorkTimeText"] = value; }
        }

        #endregion
    }


}
