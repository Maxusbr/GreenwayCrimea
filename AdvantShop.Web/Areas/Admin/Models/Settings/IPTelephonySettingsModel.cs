using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.CallBack;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class IPTelephonySettingsModel
    {
        public IPTelephonySettingsModel()
        {
            IPTelephonyOperatorTypes = Enum.GetValues(typeof(EOperatorType)).Cast<EOperatorType>()
                .Select(x => new SelectListItem { Text = x.Localize(), Value = x.ToString() })
                .ToList();
            CallBackShowModes = Enum.GetValues(typeof(ECallBackShowMode)).Cast<ECallBackShowMode>()
                .Select(x => new SelectListItem { Text = x.Localize(), Value = x.ToString() })
                .ToList();
        }

        public EOperatorType CurrentIPTelephonyOperatorType { get; set; }
        public bool PhonerLiteActive { get; set; }

        #region CallBack Settings
        public bool CallBackEnabled { get; set; }
        public ECallBackShowMode CallBackShowMode { get; set; }
        public int CallBackTimeInterval { get; set; }
        public string CallBackWorkTimeText { get; set; }
        public string CallBackNotWorkTimeText { get; set; }
        public WorkSchedule Schedule { get; set; }
        public string ScheduleSerialized { get; set; }
        #endregion

        #region Sipuni Settings
        public string SipuniApiKey { get; set; }
        public bool SipuniConsiderInnerCalls { get; set; }
        public string CallBackSipuniAccount { get; set; }
        public string CallBackSipuniShortNumber { get; set; }
        public string CallBackSipuniTree { get; set; }
        public int CallBackSipuniType { get; set; }
        #endregion

        #region Mango Settings
        public string MangoApiUrl { get; set; }
        public string MangoApiKey { get; set; }
        public string MangoSecretKey { get; set; }
        public string CallBackMangoExtension { get; set; }
        #endregion

        #region Telphin Settings
        public string TelphinAppKey { get; set; }
        public string TelphinAppSecret { get; set; }
        public string CallBackTelphinExtension { get; set; }
        public string CallBackTelphinLocalNumber { get; set; }
        public string TelphinExtensions { get; set; }
        #endregion

        #region Zadarma Settings
        public string ZadarmaKey { get; set; }
        public string ZadarmaSecret { get; set; }
        public string CallBackZadarmaPhone { get; set; }
        #endregion

        public List<SelectListItem> IPTelephonyOperatorTypes { get; set; }
        public List<SelectListItem> CallBackShowModes { get; set; }
    }
}
