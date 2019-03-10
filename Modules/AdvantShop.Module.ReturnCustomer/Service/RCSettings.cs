using AdvantShop.Core.Modules;

namespace AdvantShop.Module.ReturnCustomer.Service
{
    public class RCSettings
    {
        public static readonly int Version = 0;

        public static string ModuleID = ReturnCustomer.ModuleStringId;

        public static string MessageSubject
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MessageSubject", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("MessageSubject", value, ModuleID); }
        }

        public static string MessageText
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MessageText", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("MessageText", value, ModuleID); }
        }

        public static string AlternativeMessageSubject
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AlternativeMessageSubject", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AlternativeMessageSubject", value, ModuleID); }
        }

        public static string AlternativeMessageText
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AlternativeMessageText", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AlternativeMessageText", value, ModuleID); }
        }

        public static string DisabledMailsList
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("DisabledMailsList", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("DisabledMailsList", value, ModuleID); }
        }

        public static int DaysInterval
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("DaysInterval", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("DaysInterval", value, ModuleID); }
        }

        public static bool AutoSending
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AutoSending", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AutoSending", value, ModuleID); }
        }

        public static bool SetDefaultSettings()
        {
            DaysInterval = 7;
            MessageSubject = "Мы были рады Вас видеть!";
            MessageText = @"<div class='wrapper' style='color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;'>
                            <div class='header' style='border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;'>
                            <div class='logo' style='display: table-cell; text-align: left; vertical-align: middle;'>#LOGO#</div>
                            <div class='phone' style='display: table-cell; text-align: right; vertical-align: middle;'>
                            <div class='tel' style='font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;'>#MAIN_PHONE#</div>
                            </div>
                            </div>
                            <div style='padding:0 0 10px 0;font-weight:bold'>#USERNAME#, БЫЛИ РАДЫ ВАС ВИДЕТЬ!</div>
                            <div style='padding: 0px 0px 10px; font-weight: bold; text-align: center;'><span style='font-size:28px;'>МЫ БЕРЕЖНО ОТОБРАЛИ ТОВАРЫ, </span></div>
                            <div style='padding: 0px 0px 10px; font-weight: bold; text-align: center;'><span style='font-size:28px;'>КОТОРЫЕ ВЫ СМОТРЕЛИ</span></div>
                            <div>
                            <p dir='ltr'>На всякий случай мы сохранили список товаров, которые вы смотрели, чтобы быстро и удобно перейти на нужную вам страницу товара.</p>
                            <p dir='ltr'>Отличный выбор, хотим вам сказать! ;)</p>
                            <p dir='ltr' style='text-align: center;'>#VIEWEDPRODUCTS#</p><br />
                            <p dir='ltr'>Как вы уже знаете, спрос на популярные товары достаточно высокий, поэтому мы не можем обещать, что они будут в наличии...</p>
                            <p dir='ltr'>То, что Вам понравилось, может кто-то купить...</p>
                            </div>
                            <div>
                            <hr />
                            <div class='comment' style='margin-top: 15px;'>
                            <p dir='ltr'>Служба заботы о покупателях<br />
                            #MAIN_PHONE#</p>
                            <p>&nbsp;</p>
                            </div></div></div>";

            AlternativeMessageSubject = "Мы были рады Вас видеть!";

            AlternativeMessageText = @"<div class='wrapper' style='color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;'>
                            <div class='header' style='border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;'>
                            <div class='logo' style='display: table-cell; text-align: left; vertical-align: middle;'>#LOGO#</div>
                            <div class='phone' style='display: table-cell; text-align: right; vertical-align: middle;'>
                            <div class='tel' style='font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;'>#MAIN_PHONE#</div>
                            </div>
                            </div>
                            <div style='padding:0 0 10px 0;font-weight:bold'>#USERNAME#, БЫЛИ РАДЫ ВАС ВИДЕТЬ!</div>
                            <p dir='ltr'>Как вы уже знаете, спрос на популярные товары достаточно высокий, поэтому мы не можем обещать, что они будут в наличии...</p>
                            <p dir='ltr'>То, что Вам понравилось, может кто-то купить...</p>
                            </div>
                            <div>
                            <hr />
                            <div class='comment' style='margin-top: 15px;'>
                            <p dir='ltr'>Служба заботы о покупателях<br />
                            #MAIN_PHONE#</p>
                            <p>&nbsp;</p>
                            </div></div></div>";

            AutoSending = false;

            DisabledMailsList = string.Empty;

            return true;
        }
    }
}
