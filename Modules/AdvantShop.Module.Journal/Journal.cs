using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.Journal
{
    public class Journal: IModule
    {
        #region IModule methods

        public static string ModuleStringId
        {
            get { return "Journal"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public string ModuleName { get { return "Печатный каталог"; } }
        
        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new JournalSetting() }; }
        }

        public bool HasSettings { get { return true; } }

        private class JournalSetting : IModuleControl
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
                get { return "JournalSettings.ascx"; }
            }

            #endregion
        }

        #endregion
    }
}
