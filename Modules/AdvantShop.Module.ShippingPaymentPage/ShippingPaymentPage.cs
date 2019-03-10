using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Module.ShippingPaymentPage
{
    public class ShippingPaymentPage : IModule, IModuleBundles
    {
        #region IModule

        public string ModuleStringId
        {
            get { return "ShippingPaymentPage"; }
        }

        public static string ModuleID
        {
            get { return "ShippingPaymentPage"; }
        }

        public string ModuleName
        {
            get { return "Калькулятор Доставки/Оплаты"; }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>() { new Settings() };
            }
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
            ModuleSettingsProvider.SetSettingValue("DefaultWeight", 1, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("DefaultWidth", 10, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("DefaultHeight", 10, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("DefaultLength", 10, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("DefaultPrice", 1000, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("DefaultShippingPrice", 0, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("ShippingTextBlock", "Text can be changed in admin panel of this module", ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("ShippingTextBlockBottom", "Text can be changed in admin panel of this module", ModuleStringId);

            var language = LanguageService.GetLanguage("ru-RU");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PageTitle", "Доставка и оплата");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.EnterYourCity", "Укажите Ваш город");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.Apply", "Применить");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.RequireCart", "С учетом корзины");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingMethods", "Выберите способ доставки:");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingName", "Наименование");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingCost", "Стоимость");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingTime", "Время доставки");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingDescription", "Описание");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentMethods", "Способы оплаты, доступные при выбранной доставке:");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentName", "Наименование");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentCost", "Стоимость");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingTextBlock", "Текстовый блок над таблицей результатов");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingTextBlockBottom", "Текстовый блок под таблицей результатов");

                ModuleSettingsProvider.SetSettingValue("Title", "Доставка и оплата", ModuleStringId);
                ModuleSettingsProvider.SetSettingValue("MetaDescription", "Доставка и оплата", ModuleStringId);
                ModuleSettingsProvider.SetSettingValue("MetaKeywords", "доставка, оплата", ModuleStringId);
            }

            language = LanguageService.GetLanguage("en-US");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PageTitle", "Delivery and payment");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.EnterYourCity", "Enter your city");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.Apply", "Apply");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.RequireCart", "with shopping cart");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingMethods", "Select shipping method:");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingName", "Name");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingCost", "Cost");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingTime", "Time");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingDescription", "Description");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentMethods", "Payment methods, available for selected shipping");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentName", "Name");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentCost", "Cost");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingTextBlock", "Text block");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingTextBlock", "Text block before result table");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingTextBlockBottom", "Text block after result table");

                ModuleSettingsProvider.SetSettingValue("Title", "Delivery and payment", ModuleStringId);
                ModuleSettingsProvider.SetSettingValue("MetaDescription", "Delivery and payment", ModuleStringId);
                ModuleSettingsProvider.SetSettingValue("MetaKeywords", "delivery, payment", ModuleStringId);
            }

            return true;
        }

        public bool UpdateModule()
        {

            var language = LanguageService.GetLanguage("ru-RU");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingMethods", "Выберите способ доставки:");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentMethods", "Способы оплаты, доступные при выбранной доставке:");
            }

            language = LanguageService.GetLanguage("en-US");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.ShippingMethods", "Select shipping method:");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.ShippingPaymentPage.PaymentMethods", "Payment methods, available for selected shipping");
            }

            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("DefaultWeight", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DefaultWidth", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DefaultHeight", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DefaultLength", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DefaultPrice", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DefaultShippingPrice", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ShippingTextBlock", ModuleStringId);
            return true;
        }

        private class Settings : IModuleControl
        {
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
                get { return "Settings.ascx"; }
            }
        }

        #endregion

        #region IModuleBundles

        public List<string> GetCssBundles()
        {
            return new List<string>() { "~/modules/shippingpaymentpage/styles/shipping-payment.css" };
        }

        public List<string> GetJsBundles()
        {
            return null;
        }

        #endregion
    }
}
