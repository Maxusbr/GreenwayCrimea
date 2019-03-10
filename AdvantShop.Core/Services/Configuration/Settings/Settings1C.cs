//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System;
using AdvantShop.Configuration;

namespace AdvantShop.Core.Services.Configuration.Settings
{
    public class Settings1C
    {
        public static bool Enabled
        {
            get { return Convert.ToBoolean(SettingProvider.Items["1c_Enabled"]); }
            set { SettingProvider.Items["1c_Enabled"] = value.ToString(); }
        }

        public static bool DisableProductsDecremention
        {
            get { return Convert.ToBoolean(SettingProvider.Items["1c_DisableProductsDecremention"]); }
            set { SettingProvider.Items["1c_DisableProductsDecremention"] = value.ToString(); }
        }


        public static bool OnlyUseIn1COrders
        {
            get { return Convert.ToBoolean(SettingProvider.Items["1c_OnlyUseIn1COrders"]); }
            set { SettingProvider.Items["1c_OnlyUseIn1COrders"] = value.ToString(); }
        }

        public static bool UpdateStatuses
        {
            get { return Convert.ToBoolean(SettingProvider.Items["1c_UpdateStatuses"]); }
            set { SettingProvider.Items["1c_UpdateStatuses"] = value.ToString(); }
        }


        public static bool UpdateProducts
        {
            get { return Convert.ToBoolean(SettingProvider.Items["1c_UpdateProducts"]); }
            set { SettingProvider.Items["1c_UpdateProducts"] = value.ToString(); }
        }

        public static bool SendAllProducts
        {
            get { return Convert.ToBoolean(SettingProvider.Items["1c_SendAllProducts"]); }
            set { SettingProvider.Items["1c_SendAllProducts"] = value.ToString(); }
        }
        

    }
}
