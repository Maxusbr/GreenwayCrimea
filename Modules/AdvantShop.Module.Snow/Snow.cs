using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using System.Globalization;

namespace AdvantShop.Module.Snow
{
    public class Snow: IModule, IRenderModuleByKey
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
                        return "Снегопад 2";

                    case "en":
                        return "Snowfall 2";

                    default:
                        return "Snowfall 2";
                }
                //return string.IsNullOrEmpty(_moduleName) ? _moduleName = GetModuleName() : _moduleName;
            }
        }

        public string ModuleStringId
        {
            get { return "Snow"; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new SnowfallSetting() }; }
        }

        public bool HasSettings
        {
            get { return false; }
        }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            ////ModuleSettingsProvider.SetSettingValue("SnowfallEnabled", false, ModuleStringId);
            //ModuleSettingsProvider.SetSettingValue("SnowfallColor", "#56aaff", ModuleStringId);
            //ModuleSettingsProvider.SetSettingValue("SnowfallMaxSize", 20, ModuleStringId);
            //ModuleSettingsProvider.SetSettingValue("SnowfallMinSize", 10, ModuleStringId);
            //ModuleSettingsProvider.SetSettingValue("SnowfallNewOn", 500, ModuleStringId);
            return true;
        }

        public bool UninstallModule()
        {
            ////ModuleSettingsProvider.RemoveSqlSetting("SnowfallEnabled", ModuleStringId);
            //ModuleSettingsProvider.RemoveSqlSetting("SnowfallColor", ModuleStringId);
            //ModuleSettingsProvider.RemoveSqlSetting("SnowfallMaxSize", ModuleStringId);
            //ModuleSettingsProvider.RemoveSqlSetting("SnowfallMinSize", ModuleStringId);
            //ModuleSettingsProvider.RemoveSqlSetting("SnowfallNewOn", ModuleStringId);
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
                get { return ""; }
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
                    Content = "<div id=\"snow\"></div><link rel=\"stylesheet\" href=\"modules/snow/styles/snow.css\">"
                }
            };
        }


    }

}
