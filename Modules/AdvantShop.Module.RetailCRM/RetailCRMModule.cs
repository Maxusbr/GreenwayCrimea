//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Modules.RetailCRM;
using AdvantShop.Orders;
using AdvantShop.Core.Modules;
using System.Threading;

namespace AdvantShop.Modules
{
    [Description("RetailCRMModule")]
    public class RetailCRMModule : IModule, IOrderChanged, IModuleTask, IRenderModuleByKey
    {
        public const string ModuleStringId = "RetailCRMModule";

        #region IModule
        
        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public string ModuleName
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new RetailCRMSetting() }; }
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
            RetailCRMService.InstallModule();
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        public bool UpdateModule()
        {
            InstallModule();
            return true;
        }

        private class RetailCRMSetting : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    return "Настройки";
                }
            }

            public string File
            {
                get { return "Settings.ascx"; }
            }
        }

        #endregion

        #region IModuleTask

        public List<TaskSetting> GetTasks()
        {
            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (ExportFeedJob).FullName + "," + typeof (ExportFeedJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 6
                },
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (OrderHistoryJob).FullName + "," + typeof (OrderHistoryJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Minutes,
                    TimeInterval = 5
                }
            };
        }

        #endregion

        #region IOrderChanged

        public void DoOrderAdded(IOrder order)
        {
            if (!ModuleInited || order.IsDraft)
                return;

            var ctx = HttpContext.Current;

            Task.Run(() =>
            {
                HttpContext.Current = ctx;

                RetailCRMService.SaveRoistat(order.OrderID);

                var log = new RetailCRMLog();
                log.Write("DoOrderAdded " + order.OrderID);

                string error;
                RetailCRMService.UploadOrder(OrderService.GetOrder(order.OrderID), true, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    log.Write("error " + error);
                }

            });
        }

        public void DoOrderChangeStatus(IOrder order)
        {

            if (!ModuleInited || order.IsDraft || ModuleSettingsProvider.GetSettingValue<string>("OrderSendingMode", RetailCRMModule.ModuleStringId) == "OnCreating")
                return;

            var ctx = HttpContext.Current;

            Task.Run(() =>
            {
                Thread.Sleep(10 * 1000);

                HttpContext.Current = ctx;

                var log = new RetailCRMLog();
                log.Write("DoOrderChangeStatus " + order.OrderID);

                string error;
                RetailCRMService.UploadOrderStatus(OrderService.GetOrder(order.OrderID), false, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    log.Write("error " + error);
                }
            });
        }

        public void DoOrderUpdated(IOrder order)
        {

            if (!ModuleInited || order.IsDraft || ModuleSettingsProvider.GetSettingValue<string>("OrderSendingMode", RetailCRMModule.ModuleStringId) == "OnCreating")
                return;

            var ctx = HttpContext.Current;

            Task.Run(() =>
            {
                Thread.Sleep(5 * 1000);

                HttpContext.Current = ctx;

                var log = new RetailCRMLog();
                log.Write("DoOrderUpdated " + order.OrderID);

                string error;
                RetailCRMService.UploadOrder(OrderService.GetOrder(order.OrderID), true, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    log.Write("error " + error);
                }
            });
        }

        public void DoOrderDeleted(int orderId)
        {
            if (!ModuleInited || ModuleSettingsProvider.GetSettingValue<string>("OrderSendingMode", RetailCRMModule.ModuleStringId) == "OnCreating")
                return;

            // TODO: Заказ уже удален. Что нужно отслать в retailCRM?

            var order = OrderService.GetOrder(orderId);
            if (order == null || order.IsDraft)
                return;

            var log = new RetailCRMLog();

            string error;
            RetailCRMService.UploadOrderStatus(order, false, out error);
            if (!string.IsNullOrEmpty(error))
            {
                log.Write("error " + error);
            }
        }

        public void PayOrder(int orderId, bool payed)
        {
            if (!ModuleInited || ModuleSettingsProvider.GetSettingValue<string>("OrderSendingMode", RetailCRMModule.ModuleStringId) == "OnCreating")
                return;

            var ctx = HttpContext.Current;

            Task.Run(() =>
            {
                HttpContext.Current = ctx;

                var log = new RetailCRMLog();
                log.Write("PayOrder " + orderId);

                string error;
                RetailCRMService.UploadOrderPaid(OrderService.GetOrder(orderId), payed, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    log.Write("error " + error);
                }
            });
        }

        public void PayOrder(int orderId)
        {
            if (!ModuleInited || ModuleSettingsProvider.GetSettingValue<string>("OrderSendingMode", RetailCRMModule.ModuleStringId) == "OnCreating")
                return;

            var ctx = HttpContext.Current;

            Task.Run(() =>
            {
                HttpContext.Current = ctx;

                var order = OrderService.GetOrder(orderId);
                if (order.IsDraft)
                    return;

                var log = new RetailCRMLog();
                log.Write("PayOrder " + orderId);

                string error;
                RetailCRMService.UploadOrder(order, true, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    log.Write("error " + error);
                }
            });
        }

        #endregion


        private bool ModuleInited
        {
            get
            {
                var isAlive = true;
                isAlive &= !string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("ApiKey", ModuleStringId));
                isAlive &= !string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("Statuses", ModuleStringId));
                return isAlive;
            }
        }

        #region IRenderModuleByKey

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "head",
                    ActionName = "GetScript",
                    ControllerName = "RetailCRM",
                }
            };
        }

        #endregion

    }

}