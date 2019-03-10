
using System.Data;

using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Repository;

namespace AdvantShop.Module.BonusSystemModule.Services
{
    public class BonusSystemModuleService
    {
        #region Install / Localize

        public static bool Install()
        {
            ModuleSettingsProvider.SetSettingValue("BonusTextBlock", "<h2>Возможности больше потребностей!</h2><span style=\"font-weight: bold;\">В чем выгода</span><br />Вы можете накапливать бонусы за покупку товаров в розничных магазинах и в интернет-магазине. <br />Как получить карту?<br /><br />Вы можете получить карту бесплатно при совершении покупки на любую сумму в магазинах.", BonusSystemModule.ModuleID);
            ModuleSettingsProvider.SetSettingValue("BonusRightTextBlock", "", BonusSystemModule.ModuleID);

            return true;
        }

        public static bool Update()
        {
            ModuleSettingsProvider.SetSettingValue("BonusShowGrades", "True", BonusSystemModule.ModuleID);

            if (!ModulesRepository.IsExistsModuleTable("Module", "BonusClientCode"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
@"CREATE TABLE [Module].[BonusClientCode](
	[UserId] [uniqueidentifier] NOT NULL,
	[Code] [int] NOT NULL,
    [CreatedDate] [date] NOT NULL,
 CONSTRAINT [PK_BonusClientCode] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]",
                    CommandType.Text);
            }

            return true;
        }

        public static void Localize()
        {
            var language = LanguageService.GetLanguage("ru-RU");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.GetBonusCardTitle", "Бонусная программа");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.ClientIdText", "ИД посетителя");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.GetBonusCardHeader", "Получить бонусную карту");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.CardGrades", "Номинал карты");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.CardGradesFormat", "{0} - сумма заказов до {1} руб - {2}");
            }

            language = LanguageService.GetLanguage("en-US");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.GetBonusCardTitle", "Bonus card");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.ClientIdText", "Client Id");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.GetBonusCardHeader", "Get bonus card");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.CardGrades", "Grades");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.BonusSystem.CardGradesFormat", "{0} - order sum {1} - {2}");
            }
        }

        public static void ClearLocalize()
        {
            LocalizationService.RemoveByPattern("Module.BonusSystem");
        }

        #endregion
    }
}
