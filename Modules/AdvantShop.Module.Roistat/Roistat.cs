using System.Collections.Generic;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Module.Roistat.Domain;
using AdvantShop.Orders;

namespace AdvantShop.Module.Roistat
{
    public class Roistat : IModule, IAdminModuleSettings, IRenderModuleByKey, IOrderChanged, ILeadChanged
    {
        public const string ModuleId = "Roistat";

        #region IModule

        public string ModuleStringId { get { return ModuleId; } }
        public string ModuleName { get { return "Roistat"; } }
        public List<IModuleControl> ModuleControls { get { return null; } }
        public bool HasSettings { get { return false; } }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            return RoistatService.Install();
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        #endregion

        #region IAdminModuleSettings

        public List<ModuleSettingTab> AdminSettings
        {
            get
            {
                return new List<ModuleSettingTab>()
                {
                    new ModuleSettingTab()
                    {
                        Title = "Настройки",
                        Action = "Settings",
                        Controller = "RoistatSettings"
                    }
                };
            }
        }

        #endregion
        
        #region IRenderModuleByKey

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_end",
                    ActionName = "RoistatScript",
                    ControllerName = "Roistat"
                },
                new ModuleRoute()
                {
                    Key = "admin_order_orderinfo",
                    ActionName = "AdminOrderInfo",
                    ControllerName = "RoistatSettings"
                },
                new ModuleRoute()
                {
                    Key = "admin_lead_description",
                    ActionName = "AdminLeadDescription",
                    ControllerName = "RoistatSettings"
                }
            };
        }

        #endregion

        #region IOrderChanged

        public void DoOrderAdded(IOrder order)
        {
            RoistatService.OnOrderAdding(order);
        }

        public void DoOrderChangeStatus(IOrder order)
        {
        }

        public void DoOrderUpdated(IOrder order)
        {
        }

        public void DoOrderDeleted(int orderId)
        {
        }

        public void PayOrder(int orderId, bool payed)
        {
        }

        #endregion

        #region ILeadChanged

        public void LeadAdded(Lead lead)
        {
            RoistatService.OnLeadAdding(lead);
        }

        public void LeadUpdated(Lead lead)
        {
        }

        public void LeadDeleted(int leadId)
        {
        }
        
        #endregion
    }
}
