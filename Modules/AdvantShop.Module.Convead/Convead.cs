using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Orders;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Modules.Convead;

namespace AdvantShop.Module.Convead
{
    public class Convead : IModule, IRenderModuleByKey, IOrderChanged, IModuleTask
    {
        public bool HasSettings
        {
            get { return true; }
        }

        public string ModuleName
        {
            get { return "Convead"; }
        }

        public static string ModuleID
        {
            get { return "Convead"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleID);
        }

        public bool InstallModule()
        {
            ModuleSettingsProvider.SetSettingValue("APP_KEY", string.Empty, ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("APP_KEY", ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new ConveadSetting() }; }
        }

        private class ConveadSetting : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru": return "Настройки";
                        case "en": return "Settings";
                        default: return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "ConveadSettings.ascx"; }
            }

            #endregion
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "head",
                    ActionName = "ConveadScript",
                    ControllerName = "Convead"
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_start",
                    ActionName = "ConveadScript",
                    ControllerName = "Convead"
                },
                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "CheckoutFinalStep",
                    ControllerName = "Convead"
                },
                new ModuleRoute()
                {
                    Key = "product_info",
                    ActionName = "ProductScript",
                    ControllerName = "Convead"
                }
            };
        }

        public List<TaskSetting> GetTasks()
        {
            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (ConveadOrderHistoryJob).FullName + "," + typeof (ConveadOrderHistoryJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Minutes,
                    TimeInterval = 5
                }
            };
        }

        #region Implementation of IOrderChanged

        public void DoOrderAdded(IOrder order)
        {
            return;
        }

        public void DoOrderChangeStatus(IOrder order)
        {
            var appkey = ModuleSettingsProvider.GetSettingValue<string>("APP_KEY", ModuleID);
            if (string.IsNullOrEmpty(appkey))
                return;

            if (order == null) return;
            var status = order.GetOrderStatus();
            var statusName = status != null ? status.StatusName.Replace('ё','е') : string.Empty;
            switch (statusName)
            {
                case "Новый":
                    statusName = "new"; break;
                case "Оплачен":
                    statusName = "paid"; break;
                case "Отправлен":
                    statusName = "shipped"; break;
                case "Отменен":
                    statusName = "cancelled"; break;
                case "Доставлен":
                    statusName = "Доставлен"; break;
                default: break;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://tracker.convead.io/integration/common/webhook");
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("X-Webhook-Topic", "events/order_update");
            request.Headers.Add("X-App-Key", appkey);
            string postData = new JavaScriptSerializer().Serialize(new
            {
                order_id = order.OrderID.ToString(),
                state = statusName,
                revenue = order.Sum.ToString().Replace(",", "."),
                items = order.OrderItems.Select(
                            oi => new
                            {
                                //product_id = oi.ProductID != null ? oi.ProductID.ToString() : "",
                                product_id = oi.ArtNo != null ? OfferService.GetOffer(oi.ArtNo.ToString()).OfferId.ToString() : "",
                                qnt = oi.Amount.ToString().Replace(",", "."),
                                price = oi.Price.ToString().Replace(",", ".")
                            })
            });
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            try
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public void DoOrderUpdated(IOrder order)
        {
            return;
        }

        public void PayOrder(int orderId, bool payed)
        {
            return;
        }

        public void DoOrderDeleted(int orderId)
        {
            return;
        }

        #endregion

    }
}