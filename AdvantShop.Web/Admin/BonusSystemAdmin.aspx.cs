using System;
using AdvantShop.Core.Controls;
using AdvantShop.Configuration;
using Resources;
using AdvantShop.Core.Services.Bonuses;
using System.Drawing;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Trial;
using AdvantShop.Core.Modules;
using System.Data;
using AdvantShop.Core.Services.Localization;

namespace Admin
{
    public partial class BonusSystemAdmin : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Coupons_Header));

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            cbEnabled.Checked = BonusSystem.IsActive;
            //txtApiKey.Text = BonusSystem.ApiKey;
            ddlBonusType.SelectedValue = ((int)BonusSystem.BonusType).ToString();
            lblBonusFirstPercent.Text = BonusSystem.BonusFirstPercent.ToString();
            txtMaxOrderPercent.Text = BonusSystem.MaxOrderPercent.ToString("F2");
            ckbUseOrderId.Checked = BonusSystem.UseOrderId;
            txtBonusesForNewCard.Text = BonusSystem.BonusesForNewCard.ToString("F2");

            txtBonusTextBlock.Text = BonusSystem.BonusTextBlock;
            txtRightBonusTextBlock.Text = BonusSystem.BonusRightTextBlock;
            chkShowGrades.Checked = BonusSystem.BonusShowGrades;

            hlGetBonusCard.NavigateUrl = UrlService.GetUrl("getbonuscard");

            if (TrialService.IsTrialEnabled)
            {
                txtApiKey.Visible = false;
                divTrial.Visible = true;
            }
            else
            {
                txtApiKey.Visible = true;
                divTrial.Visible = false;
            }
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            BonusSystem.IsActive = cbEnabled.Checked;
            //BonusSystem.ApiKey = txtNewKey.Text.IsNotEmpty() ? txtNewKey.Text : txtApiKey.Text;
            BonusSystem.BonusType = (EBonusType)SQLDataHelper.GetInt(ddlBonusType.SelectedValue);
            BonusSystem.MaxOrderPercent = txtMaxOrderPercent.Text.TryParseFloat(100);
            BonusSystem.UseOrderId = ckbUseOrderId.Checked;
            //BonusSystem.BonusesForNewCard = txtBonusesForNewCard.Text.TryParseFloat();


            BonusSystem.BonusTextBlock = txtBonusTextBlock.Text;
            BonusSystem.BonusRightTextBlock = txtRightBonusTextBlock.Text;
            BonusSystem.BonusShowGrades = chkShowGrades.Checked;


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

            //if (BonusSystemService.IsActive())
            //{
            //    lblMessage.Text = Resource.BonusSystem_Message;
            //    lblMessage.ForeColor = Color.Blue;
            //    lblMessage.Visible = true;
            //}
            //else
            //{
            //    lblMessage.Text = Resources.Resource.BonusSystem_Save_Error;
            //    lblMessage.ForeColor = Color.Red;
            //    lblMessage.Visible = true;
            //}
        }
    }
}