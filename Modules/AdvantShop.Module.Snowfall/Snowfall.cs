//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.Snowfall
{
    public class Snowfall : IModule, IRenderModuleByKey
    {
        #region IModule

        //private string _moduleName;

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Снегопад";

                    case "en":
                        return "Snowfall";

                    default:
                        return "Snowfall";
                }
                //return string.IsNullOrEmpty(_moduleName) ? _moduleName = GetModuleName() : _moduleName;
            }
        }

        public string ModuleStringId
        {
            get { return "Snowfall"; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new SnowfallSetting()}; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId) &&
                   File.Exists(HttpContext.Current.Server.MapPath("~/Modules/Snowfall/snowfall.js"));
        }

        public bool InstallModule()
        {
            //ModuleSettingsProvider.SetSettingValue("SnowfallEnabled", false, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("SnowfallColor", "#56aaff", ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("SnowfallMaxSize", 20, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("SnowfallMinSize", 10, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("SnowfallNewOn", 500, ModuleStringId);
            return true;
        }

        public bool UninstallModule()
        {
            //ModuleSettingsProvider.RemoveSqlSetting("SnowfallEnabled", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("SnowfallColor", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("SnowfallMaxSize", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("SnowfallMinSize", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("SnowfallNewOn", ModuleStringId);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        private class SnowfallSetting : IModuleControl
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
                get { return "SnowfallModule.ascx"; }
            }

            #endregion
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
                    Content = DoRenderBeforeBodyEnd()
                }
            };
        }

        public string DoRenderBeforeBodyEnd()
        {
            var optsString =
                String.Format("{{flakeColor:'{0}', maxSize:{1}, minSize:{2}, newOn:{3}}}",
                    ModuleSettingsProvider.GetSettingValue<string>("SnowfallColor", ModuleStringId),
                    ModuleSettingsProvider.GetSettingValue<int>("SnowfallMaxSize", ModuleStringId),
                    ModuleSettingsProvider.GetSettingValue<int>("SnowfallMinSize", ModuleStringId),
                    ModuleSettingsProvider.GetSettingValue<int>("SnowfallNewOn", ModuleStringId));

            return
                String.Format("<script data-module=\"snowfall\" data-module-snowfall-options=\"{0}\" src=\"modules/snowfall/snowfall.js\"></script>",
                              optsString);
        }
    }
}