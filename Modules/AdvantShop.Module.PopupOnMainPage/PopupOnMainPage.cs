using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.PopupOnMainPage
{
    public class PopupOnMainPage : IModule, IRenderModuleByKey
    {
        #region IModule

        public static string ModuleID
        {
            get { return "popuponmainpage"; }
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
                        return "Рекламный Popup";

                    case "en":
                        return "Comercial Popup";

                    default:
                        return "Comercial Popup";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new PopupOnMainPageSetting() }; }
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
            string defaultTitle = "<div class='popup-header' style='color: #4b4f58; font-size: 30px; padding: 20px 15px; text-align: center;'>Ваш первый подарок прямо сейчас - <br> 500 рублей на бонусную карту<div class='popup-pre-header' style='color: #4b4f58; font-size: 12px; margin-top: 15px;'>Оформите карту Много.ру бесплатно и получайте бонусы за покупки!</div></div>";
            string defaultContent = "<div class='popup-content' style='background: linear-gradient(to bottom, #379aed 0%, #379aed 32%, #3899ea 37%, #3090dd 57%, #2d89d4 66%, #2c88d5 69%, #2a86d1 71%, #2882ce 78%, #2782cb 79%, #267cc5 90%, #267cc5 100%); padding: 60px; text-align: center;'><div><img src='../../userfiles/image/gift.png' alt=''></div><div><a href='#' class='popup-btn' style='background: #d9df40; border-radius: 6px; color: white; display: inline-block; font-size: 25px; font-weight: bold; line-height: 47px; padding: 0 20px; text-decoration: none;'>Оформить карту</a></div></div>";

            ModuleSettingsProvider.SetSettingValue("PopupOnMainPageHtml", defaultContent, ModuleID);
            ModuleSettingsProvider.SetSettingValue("PopupOnMainPageTimeSpan", 10, ModuleID);
            ModuleSettingsProvider.SetSettingValue("PopupOnMainPageTitle", defaultTitle, ModuleID);

            ModuleSettingsProvider.SetSettingValue("ShowOnMain", true, ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShowInDetails", false, ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShowInOtherPages", false, ModuleID);
            ModuleSettingsProvider.SetSettingValue("DelayShowPopup", 2, ModuleID);

            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("PopupOnMainPageHtml", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("PopupOnMainPageTitle", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("PopupOnMainPageTimeSpan", ModuleID);

            ModuleSettingsProvider.RemoveSqlSetting("ShowOnMain", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowInDetails", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowInOtherPages", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("DelayShowPopup", ModuleID);

            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        private class PopupOnMainPageSetting : IModuleControl
        {
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
                get { return "PopupOnMainPageSetting.ascx"; }
            }
        }

        #endregion

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_end",
                    IsSimpleText = true,
                    Content = "<link rel=\"stylesheet\" href=\"modules/popuponmainpage/styles/styles.css\" /> "
                },
                new ModuleRoute()
                {
                    Key = "body_end",
                    ActionName = "PopupOnMainPageScript",
                    ControllerName = "PopupOnMainPage"
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_end",
                    IsSimpleText = true,
                    Content = "<link rel=\"stylesheet\" href=\"modules/popuponmainpage/styles/styles.css\" /> "
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_end",
                    ActionName = "PopupOnMainPageScript",
                    ControllerName = "PopupOnMainPage"
                }
            };
        }
    }
}