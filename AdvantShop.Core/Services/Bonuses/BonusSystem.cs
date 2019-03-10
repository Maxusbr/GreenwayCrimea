using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Model.Rules;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Helpers;
using AdvantShop.Saas;

namespace AdvantShop.Core.Services.Bonuses
{
    public enum EBonusType
    {
        [Localize("Стоимость товаров и доставки")]
        ByProductsCostWithShipping = 0,

        [Localize("Стоимость товаров")]
        ByProductsCost = 1
    }

    public class BonusSystem
    {
        public static int DefaultGrade
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["BonusSystem.DefaultGrade"]); }
            set { SettingProvider.Items["BonusSystem.DefaultGrade"] = value.ToString(); }
        }

        public static long CardFrom
        {
            get { return SQLDataHelper.GetLong(SettingProvider.Items["BonusSystem.CardFrom"]); }
            set { SettingProvider.Items["BonusSystem.CardFrom"] = value.ToString(); }
        }

        public static long CardTo
        {
            get { return SQLDataHelper.GetLong(SettingProvider.Items["BonusSystem.CardTo"]); }
            set { SettingProvider.Items["BonusSystem.CardTo"] = value.ToString(); }
        }

        public static bool IsActive
        {
            get
            {
                var context = HttpContext.Current;
                if (context != null && context.Items["BonusSystem.IsActive"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Items["BonusSystem.IsActive"]);

                var isActive =
                    SQLDataHelper.GetBoolean(SettingProvider.Items["BonusSystem.IsActive"]) &&
                    (!SaasDataService.IsSaasEnabled ||
                     (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.BonusSystem));
                
                if (context != null)
                    context.Items["BonusSystem.IsActive"] = isActive;

                return isActive;
            }
            set { SettingProvider.Items["BonusSystem.IsActive"] = value.ToString(); }
        }


        public static EBonusType BonusType
        {
            get { return (EBonusType)Convert.ToInt32(SettingProvider.Items["BonusSystem.BonusType"]); }
            set { SettingProvider.Items["BonusSystem.BonusType"] = ((int)value).ToString(); }
        }

        public static decimal BonusFirstPercent
        {
            get
            {
                var grade = GradeService.Get(DefaultGrade);
                if (grade != null)
                    return grade.BonusPercent;
                return 0;
            }
            //get { return BonusSystemService.GetBonusDefaultPercent(); }
        }

        public static float MaxOrderPercent
        {
            get { return SQLDataHelper.GetFloat(SettingProvider.Items["BonusSystem.MaxOrderPercent"]); }
            set { SettingProvider.Items["BonusSystem.MaxOrderPercent"] = value.ToString(); }
        }

        public static float BonusesForNewCard
        {
            get
            {
                var bdrule = CustomRuleService.Get(ERule.NewCard);
                if (bdrule == null || !bdrule.Enabled)
                    return 0;

                var rule = BaseRule.Get(bdrule) as NewCardRule;
                if (rule == null)
                    return 0;

                return (float)rule.GiftBonus;
            }
        }

        public static bool UseOrderId
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["BonusSystem.UseOrderId"]); }
            set { SettingProvider.Items["BonusSystem.UseOrderId"] = value.ToString(); }
        }

        public static string BonusTextBlock
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.BonusTextBlock"]) ?? ModuleSettingsProvider.GetSettingValue<string>("BonusTextBlock", "BonusSystemModule"); }
            set { SettingProvider.Items["BonusSystem.BonusTextBlock"] = value; }
        }

        public static string BonusRightTextBlock
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.BonusRightTextBlock"]) ?? ModuleSettingsProvider.GetSettingValue<string>("BonusRightTextBlock", "BonusSystemModule"); }
            set { SettingProvider.Items["BonusSystem.BonusRightTextBlock"] = value; }
        }

        public static bool BonusShowGrades
        {
            get { return SQLDataHelper.GetNullableBoolean(SettingProvider.Items["BonusSystem.BonusShowGrades"]) ?? ModuleSettingsProvider.GetSettingValue<bool>("BonusShowGrades", "BonusSystemModule"); }
            set { SettingProvider.Items["BonusSystem.BonusShowGrades"] = value.ToString(); }
        }

        public static bool ForbidOnCoupon
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["BonusSystem.ForbidOnCoupon"]); }
            set { SettingProvider.Items["BonusSystem.ForbidOnCoupon"] = value.ToString(); }
        }

        #region SMS

        public static bool SmsEnabled
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["BonusSystem.SmsEnabled"]); }
            set { SettingProvider.Items["BonusSystem.SmsEnabled"] = value.ToString(); }
        }

        public static string SmsTitle
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.SmsTitle"]); }
            set { SettingProvider.Items["BonusSystem.SmsTitle"] = value; }
        }

        //public static string SmsLogin
        //{
        //    get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.SmsLogin"]); }
        //    set { SettingProvider.Items["BonusSystem.SmsLogin"] = value; }
        //}

        //public static string SmsPassword
        //{
        //    get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.SmsPassword"]); }
        //    set { SettingProvider.Items["BonusSystem.SmsPassword"] = value; }
        //}

        public static SmsProviderType SmsProviderType
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.SmsProviderType"]).TryParseEnum<SmsProviderType>(); }
            set { SettingProvider.Items["BonusSystem.SmsProviderType"] = value.ToString(); }
        }

        #region Sms4B

        public static string Sms4BLogin
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.Sms4BLogin"]); }
            set { SettingProvider.Items["BonusSystem.Sms4BLogin"] = value; }
        }

        public static string Sms4BPassword
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.Sms4BPassword"]); }
            set { SettingProvider.Items["BonusSystem.Sms4BPassword"] = value; }
        }

        #endregion

        #region stream-telecom.ru

        public static string StreamSmsLogin
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.StreamSmsLogin"]); }
            set { SettingProvider.Items["BonusSystem.StreamSmsLogin"] = value; }
        }

        public static string StreamSmsPassword
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.StreamSmsPassword"]); }
            set { SettingProvider.Items["BonusSystem.StreamSmsPassword"] = value; }
        }

        #endregion

        #region ePochta

        public static string EPochtaPublicKey
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.EPochtaPublicKey"]); }
            set { SettingProvider.Items["BonusSystem.EPochtaPublicKey"] = value; }
        }

        public static string EPochtaPrivateKey
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.EPochtaPrivateKey"]); }
            set { SettingProvider.Items["BonusSystem.EPochtaPrivateKey"] = value; }
        }

        #endregion

        #region UniSender

        public static string UniSenderApiKey
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BonusSystem.UniSenderApiKey"]); }
            set { SettingProvider.Items["BonusSystem.UniSenderApiKey"] = value; }
        }        

        #endregion

        #endregion
    }
}