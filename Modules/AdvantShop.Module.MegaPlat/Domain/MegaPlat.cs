//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Modules
{
    [Description("MegaPlat")]
    public class MegaPlat : IModule
    {
        private string _moduleName;

        public string ModuleName
        {
            get { return string.IsNullOrEmpty(_moduleName) ? _moduleName = GetModuleName() : _moduleName; }
        }

        public string ModuleStringId
        {
            get { return "MegaPlat"; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new MegaPlatSetting()}; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            ModuleSettingsProvider.SetSettingValue("MegaPlatApiKey", Guid.NewGuid(), ModuleStringId);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("MegaPlatApiKey", ModuleStringId);
            return true;
        }

        private string GetModuleName()
        {
            return "MegaPlat";
        }

        private class MegaPlatSetting : IModuleControl
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
                get { return "MegaPlatModule.ascx"; }
            }

            #endregion
        }
    }
}