using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using System.Web.UI.WebControls;

namespace AdvantShop.Module.YaBuying.Domain
{
    public class YaMarketBuyingSettings
    {
        private const string ModuleStringId = "YaMarketBuying";

        public static string AuthToken
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AuthToken", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AuthToken", value, ModuleStringId); }
        }

        public static string Payments
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("Payments", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("Payments", value, ModuleStringId); }
        }

        public static string Outlets
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("Outlets", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("Outlets", value, ModuleStringId); }
        }

        public static string CampaignId
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CampaignId", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CampaignId", value, ModuleStringId); }
        }

        public static string AuthTokenToMarket
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AuthTokenToMarket", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AuthTokenToMarket", value, ModuleStringId); }
        }

        public static string Login
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("Login", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("Login", value, ModuleStringId); }
        }

        public static string AuthClientId
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AuthClientId", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AuthClientId", value, ModuleStringId); }
        }

        public static bool EnableDefaultShipping
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("EnableDefaultShipping", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("EnableDefaultShipping", value, ModuleStringId); }
        }

        public static string DefaultShippingName
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("DefaultShippingName", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DefaultShippingName", value, ModuleStringId); }
        }
        
        public static float DefaultShippingPrice
        {
            get { return ModuleSettingsProvider.GetSettingValue<float>("DefaultShippingPrice", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DefaultShippingPrice", value, ModuleStringId); }
        }

        public static int DefaultShippingMinDate
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("DefaultShippingMinDate", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DefaultShippingMinDate", value, ModuleStringId); }
        }

        public static int DefaultShippingMaxDate
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("DefaultShippingMaxDate", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DefaultShippingMaxDate", value, ModuleStringId); }
        }

        public static string DefaultShippingType
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("DefaultShippingType", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DefaultShippingType", value, ModuleStringId); }
        }

        public static string ScheduleDelivery
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ScheduleDelivery", ModuleStringId) ?? " "; }
            set { ModuleSettingsProvider.SetSettingValue("ScheduleDelivery", value, ModuleStringId); }
        }

        public static string TimeDeliveryForSchedule
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("TimeDeliveryForSchedule", ModuleStringId) ?? "23:59"; }
            set { ModuleSettingsProvider.SetSettingValue("TimeDeliveryForSchedule", value, ModuleStringId); }
        }

        public static ListItem[] GetListDays()
        {
            var result = new ListItem[] {
                new ListItem() { Text = "Понедельник", Value = "monday" },
                new ListItem() { Text = "Вторник", Value = "tuesday" },
                new ListItem() { Text = "Среда", Value = "wednesday" },
                new ListItem() { Text = "Четверг", Value = "thursday" },
                new ListItem() { Text = "Пятница", Value = "friday" },
                new ListItem() { Text = "Суббота", Value = "saturday" },
                new ListItem() { Text = "Воскресенье", Value = "sunday" }};

            return result;
        }


        #region Statuses

        public static int UpaidStatusId
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("UpaidStatusId", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("UpaidStatusId", value, ModuleStringId); }
        }

        //public static int ProcessingStatusId
        //{
        //    get { return ModuleSettingsProvider.GetSettingValue<int>("ProcessingStatusId", ModuleStringId); }
        //    set { ModuleSettingsProvider.SetSettingValue("ProcessingStatusId", value, ModuleStringId); }
        //}

        public static List<int> ProcessingStatusesIds
        {
            get
            {
                var ids = ModuleSettingsProvider.GetSettingValue<string>("ProcessingStatusesIds", ModuleStringId);
                if (string.IsNullOrWhiteSpace(ids))
                    return new List<int>();

                return
                    ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.TryParseInt())
                        .Where(x => x != 0)
                        .ToList();
            }
            set
            {
                ModuleSettingsProvider.SetSettingValue("ProcessingStatusesIds", String.Join(",", value), ModuleStringId);
            }
        }


        //public static int DeliveryStatusId
        //{
        //    get { return ModuleSettingsProvider.GetSettingValue<int>("DeliveryStatusId", ModuleStringId); }
        //    set { ModuleSettingsProvider.SetSettingValue("DeliveryStatusId", value, ModuleStringId); }
        //}

        public static List<int> DeliveryStatusesIds
        {
            get
            {
                var ids = ModuleSettingsProvider.GetSettingValue<string>("DeliveryStatusesIds", ModuleStringId);
                if (string.IsNullOrWhiteSpace(ids))
                    return new List<int>();

                return
                    ids.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.TryParseInt())
                        .Where(x => x != 0)
                        .ToList();
            }
            set
            {
                ModuleSettingsProvider.SetSettingValue("DeliveryStatusesIds", String.Join(",", value), ModuleStringId);
            }
        }

        public static int DeliveredStatusId
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("DeliveredStatusId", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DeliveredStatusId", value, ModuleStringId); }
        }

        public static int PickupStatusId
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("PickupStatusId", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("PickupStatusId", value, ModuleStringId); }
        }

        public static int ReservedStatusId
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("ReservedStatusId", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ReservedStatusId", value, ModuleStringId); }
        }

        /// <summary>
        /// PROCESSING_EXPIRED — магазин не обработал заказ вовремя;
        /// </summary>
        public static int CanceledStatusId_PROCESSING_EXPIRED
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_PROCESSING_EXPIRED", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_PROCESSING_EXPIRED", value, ModuleStringId); }
        }

        /// <summary>
        /// REPLACING_ORDER — покупатель изменяет состав заказа;
        /// </summary>
        public static int CanceledStatusId_REPLACING_ORDER
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_REPLACING_ORDER", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_REPLACING_ORDER", value, ModuleStringId); }
        }

        /// <summary>
        /// RESERVATION_EXPIRED — покупатель не завершил оформление зарезервированного заказа вовремя;
        /// </summary>
        public static int CanceledStatusId_RESERVATION_EXPIRED
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_RESERVATION_EXPIRED", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_RESERVATION_EXPIRED", value, ModuleStringId); }
        }

        /// <summary>
        /// SHOP_FAILED — магазин не может выполнить заказ;
        /// </summary>
        public static int CanceledStatusId_SHOP_FAILED
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_SHOP_FAILED", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_SHOP_FAILED", value, ModuleStringId); }
        }

        /// <summary>
        /// USER_CHANGED_MIND — покупатель отменил заказ по собственным причинам;
        /// </summary>
        public static int CanceledStatusId_USER_CHANGED_MIND
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_USER_CHANGED_MIND", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_USER_CHANGED_MIND", value, ModuleStringId); }
        }

        /// <summary>
        /// USER_NOT_PAID — покупатель не оплатил заказ (для типа оплаты PREPAID);
        /// </summary>
        public static int CanceledStatusId_USER_NOT_PAID
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_USER_NOT_PAID", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_USER_NOT_PAID", value, ModuleStringId); }
        }

        /// <summary>
        /// USER_REFUSED_DELIVERY — покупателя не устраивают условия доставки;
        /// </summary>
        public static int CanceledStatusId_USER_REFUSED_DELIVERY
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_USER_REFUSED_DELIVERY", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_USER_REFUSED_DELIVERY", value, ModuleStringId); }
        }

        /// <summary>
        /// USER_REFUSED_PRODUCT — покупателю не подошел товар;
        /// </summary>
        public static int CanceledStatusId_USER_REFUSED_PRODUCT
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_USER_REFUSED_PRODUCT", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_USER_REFUSED_PRODUCT", value, ModuleStringId); }
        }

        /// <summary>
        /// USER_REFUSED_QUALITY — покупателя не устраивает качество товара;
        /// </summary>
        public static int CanceledStatusId_USER_REFUSED_QUALITY
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_USER_REFUSED_QUALITY", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_USER_REFUSED_QUALITY", value, ModuleStringId); }
        }

        /// <summary>
        /// USER_UNREACHABLE — не удалось связаться с покупателем.
        /// </summary>
        public static int CanceledStatusId_USER_UNREACHABLE
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CanceledStatusId_USER_UNREACHABLE", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CanceledStatusId_USER_UNREACHABLE", value, ModuleStringId); }
        }
        
        #endregion

    }
}