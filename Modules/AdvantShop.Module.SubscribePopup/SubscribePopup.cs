using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.SubscribePopup
{
    public class SubscribePopup : IModule, IRenderModuleByKey
    {
        #region Module methods

        public static string ModuleID
        {
            get { return "SubscribePopup"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Всплывающее окно Подписка на новости";

                    case "en":
                        return "Subscribe Popup";

                    default:
                        return "Subscribe Popup";
                }
            }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            ModuleSettingsProvider.SetSettingValue("SubscribePopupHtml",
                                                   "<div class='subscribe'>" +
                                                   "<div class='stripes top'>&nbsp;</div>" +
                                                   "<div class='subscribe-space'>" +
                                                   "<h4>Подписка на новости</h4>" +
                                                   "<p>Подпишитесь на нашу рассылку, и станьте одним из первых, кто будет в курсе всех новостей!</p>" +
                                                   "<hr />" +
                                                   "<div>" +
                                                   "<label>Ваш e-mail адрес</label><input data-subscribe='email' data-type='email' placeholder='Введите ваш e-mail адрес' required='required' type='email'/>" +
                                                   "<a class='submit' data-subscribe='button' href='javascript:void(0);'>Подписаться</a></div>" +
                                                   "</div>" +
                                                   "<div class='error' data-subscribe='error'>&nbsp;</div>" +
                                                   "<div class='stripes bottom'>&nbsp;</div>" +
                                                   "</div>", ModuleID);
            ModuleSettingsProvider.SetSettingValue("SubscribePopupTimeSpan", 0, ModuleID);

            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("SubscribePopupHtml", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("SubscribePopupTimeSpan", ModuleID);

            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new SubscribePopupSetting() }; }
        }

        private class SubscribePopupSetting : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки";

                        case "en":
                            return "Settings";

                        default:
                            return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "SubscribePopupSetting.ascx"; }
            }

            #endregion
        }

        #endregion

        #region Module Settings

        public static string SettingStaticHtml
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("SubscribePopupHtml", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("SubscribePopupHtml", value, ModuleID); }
        }

        public static int SettingTimeSpan
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("SubscribePopupTimeSpan", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("SubscribePopupTimeSpan", value, ModuleID); }
        }

        public static bool SettingShowOnMain
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowOnMain", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowOnMain", value, ModuleID); }
        }

        public static bool SettingShowInDetails
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowInDetails", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowInDetails", value, ModuleID); }
        }

        public static bool SettingShowInOtherPages
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowInOtherPages", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowInOtherPages", value, ModuleID); }
        }

        public static bool SettingNotifyAdmin
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NotifyAdmin", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("NotifyAdmin", value, ModuleID); }
        }

        public static int SettingDelayShowPopup
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("DelayShowPopup", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("DelayShowPopup", value, ModuleID); }
        }

        public static string SettingPopupTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PopupTitle", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("PopupTitle", value, ModuleID); }
        }
        public static string SettingPopupTopHtml
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PopupTopHtml", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("PopupTopHtml", value, ModuleID); }
        }
        public static string SettingPopupBottomHtml
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PopupBottomHtml", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("PopupBottomHtml", value, ModuleID); }
        }
        public static string SettingPopupFinalHtml
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PopupFinalHtml", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("PopupFinalHtml", value, ModuleID); }
        }
        #endregion

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_end",
                    ActionName = "SubscribePopupScript",
                    ControllerName = "SubscribePopup"
                }
            };
        }

    }
}