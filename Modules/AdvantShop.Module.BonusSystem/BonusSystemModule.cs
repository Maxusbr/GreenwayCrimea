//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System;
using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Customers;
using AdvantShop.Module.BonusSystemModule.Services;

namespace AdvantShop.Module.BonusSystemModule
{
    partial class BonusSystemModule : IBonusSystem, IRenderModuleByKey, IModuleChangeActive, IModule //, IAdminSearch, IModuleTask,
    {
        #region Module methods

        public static string ModuleID
        {
            get { return "BonusSystemModule"; }
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
                        return "Бонусная система";

                    case "en":
                        return "Bonus system";

                    default:
                        return "Bonus system";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new BonusSystemSetting()}; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleID);
        }

        public bool InstallModule()
        {
            BonusSystemModuleService.Install();
            BonusSystemModuleService.Update();
            BonusSystemModuleService.Localize();
            return true;
        }

        public bool UpdateModule()
        {
            BonusSystemModuleService.Update();
            BonusSystemModuleService.Localize();
            return true;
        }

        public bool UninstallModule()
        {
            BonusSystemModuleService.ClearLocalize();
            return true;
        }

        private class BonusSystemSetting : IModuleControl
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
                get { return "BonusSystemModule.ascx"; }
            }

            #endregion
        }

        public Customer GetCustomerByCode(string code, Guid userId)
        {
            return null; 
        }

        #endregion

        #region IModuleChangeActive

        public void ModuleChangeActive(bool active)
        {

        }

        #endregion
        
        #region IRenderModuleByKey

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>();
        }

        #endregion

    }
}