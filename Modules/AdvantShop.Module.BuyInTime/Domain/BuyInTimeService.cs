//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.BuyInTime.Domain
{



    public class BuyInTimeService
    {
        #region Fields

        public const string ModuleName = "BuyInTime";
        public const string CacheKey = "BuyInTimeCacheKey";
        private const string tpl = "<div class=\"buy-in-time-inner\">" + 
                                           "<h3 class=\"buy-in-time-header\">#ActionTitle#</h3>" +
                                           "<div class=\"buy-in-time-content\">" +
                                               "<div class=\"buy-in-time-countdown-block\"> " +
                                                   "<div class=\"buy-in-time-text\">До конца распродажи:</div>" +
                                                   "#Countdown#" +
                                               "</div>" +
                                               "<figure class=\"buy-in-time-picture-block\">" +
                                                   "<a href=\"#ProductLink#\"><img alt=\"#ProductName#\" class=\"buy-in-time-picture\" src=\"#ProductPictureSrc#\" /></a>" +
                                                   "<div class=\"buy-in-time-discount sticker-main\">" +
                                                       "<div class=\"buy-in-time-discount-number\">#DiscountPercent#%</div>" +
                                                       "<div class=\"buy-in-time-discount-text\">скидка</div>" +
                                                   "</div>" +
                                               "</figure>" +
                                               "<div class=\"buy-in-time-price-block\">" +
                                                   "<div class=\"buy-in-time-name\"><a class=\"buy-in-time-name-link\" href=\"#ProductLink#\">#ProductName#</a></div>" +
                                                   "<div class=\"buy-in-time-price-default\">Цена: <span class=\"price\"><span class=\"price-current\"><span class=\"price-number\">#OldPrice#</span><span class=\"price-currency\">р.</span></span></span></div>" +
                                                   "<div class=\"buy-in-time-price-today\">Цена: <span class=\"price\"><span class=\"price-current\"><span class=\"price-number\">#NewPrice#</span><span class=\"price-currency\">р.</span></span></span></div>" +
                                                   "<div class=\"buy-in-time-button-block\"><a class=\"btn btn-small btn-action btn-buy-in-time\" href=\"#ProductLink#\">Экономия: #DiscountPrice# р.</a></div>" +
                                               "</div>" +
                                           "</div>" +
                                       "</div>";

        public static string PicturePath
        {
            get { return SettingsGeneral.AbsolutePath + "pictures/modules/BuyInTime/"; }
        }

        public enum eShowMode
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2
        }

        #endregion

        #region Install / Uninstall

        public static bool InstallBuyInTimeModule()
        {
            ModulesRepository.ModuleExecuteNonQuery(
            @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Module." + ModuleName + @"') AND type in (N'U'))
                Begin
                    CREATE TABLE Module." + ModuleName + @"
	                    (
	                    Id int NOT NULL IDENTITY (1, 1),
	                    ProductId int NOT NULL,
                        DateStart datetime NOT NULL,
	                    DateExpired datetime NOT NULL,
	                    DiscountInTime float(53) NOT NULL,
                        ActionText nvarchar(MAX) NOT NULL,
	                    ShowMode tinyint NOT NULL,
                        IsRepeat bit NOT NULL,
                        DaysRepeat int NOT NULL,
                        Picture nvarchar(50) NULL
	                    )  ON [PRIMARY]
                    
                    ALTER TABLE Module." + ModuleName + @" ADD CONSTRAINT
	                    PK_BuyInTime PRIMARY KEY CLUSTERED 
	                    (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    
                    ALTER TABLE Module." + ModuleName + @" ADD CONSTRAINT
	                    FK_BuyInTime_Product FOREIGN KEY
	                    (ProductId) REFERENCES Catalog.Product (ProductId) ON UPDATE  NO ACTION ON DELETE  CASCADE
                End",
                CommandType.Text);

            ModuleSettingsProvider.SetSettingValue("BuyInTimeLabel", "Успей купить", ModuleName);

            ModuleSettingsProvider.SetSettingValue("BuyInTimeActionTitle", "Успей купить!", ModuleName);

            ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultActionText", tpl, ModuleName);

            //ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultActionTextMode1", String.Format(tpl, "buy-in-time-block-horizontal"), ModuleName);

            //ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultActionTextMode2", String.Format(tpl, "buy-in-time-block-vertical"), ModuleName);

            //ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultMobileActionText", String.Format(tpl, "buy-in-time-block-mobile"), ModuleName);

            return true;
        }

        public static bool UninstallBuyInTimeModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("BuyInTimeActionTitle", ModuleName);
            ModuleSettingsProvider.RemoveSqlSetting("BuyInTimeLabel", ModuleName);
            ModuleSettingsProvider.RemoveSqlSetting("BuyInTimeDefaultActionText", ModuleName);
            ModuleSettingsProvider.RemoveSqlSetting("BuyInTimeDefaultActionTextMode1", ModuleName);
            ModuleSettingsProvider.RemoveSqlSetting("BuyInTimeDefaultActionTextMode2", ModuleName);
            ModuleSettingsProvider.RemoveSqlSetting("BuyInTimeDefaultMobileActionText", ModuleName);
            return true;
        }

        public static bool UpdateBuyInTimeModule()
        {
			ModulesRepository.ModuleExecuteNonQuery(@"Update [Module].[BuyInTime] set ActionText = Replace(ActionText,'р.','#CurrencyCode#'), 
				MobileActionText = Replace(MobileActionText,'р.','#CurrencyCode#')", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'SortOrder' AND [object_id] = OBJECT_ID(N'Module.BuyInTime'))
                BEGIN
	                ALTER TABLE Module.BuyInTime ADD SortOrder int NOT NULL DEFAULT(0)
                END", CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                @"IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ShowInMobile' AND [object_id] = OBJECT_ID(N'Module.BuyInTime'))
                BEGIN
	                ALTER TABLE Module.BuyInTime ADD ShowInMobile bit NULL,	MobileActionText nvarchar(MAX) NULL
                END", CommandType.Text);

            ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultActionText", tpl, ModuleName);

            //ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultActionTextMode1", String.Format(tpl, "buy-in-time-block-horizontal"), ModuleName);

            //ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultActionTextMode2", String.Format(tpl, "buy-in-time-block-vertical"), ModuleName);

            //ModuleSettingsProvider.SetSettingValue("BuyInTimeDefaultMobileActionText", String.Format(tpl, "buy-in-time-block-mobile"), ModuleName);

            return true;
        }

        #endregion

        public static DateTime GetExpireDateTime(DateTime expired, int daysRepeat)
        {
            var now = DateTime.Now;
            var days = Math.Ceiling((double)(now - expired).Days / daysRepeat) * daysRepeat;
            var expiredDateTime = expired.AddDays(days > 0 ? days : daysRepeat);
            if (expiredDateTime <= now)
                expiredDateTime = expiredDateTime.AddDays(daysRepeat);

            return expiredDateTime;
        }

        public static ProductDiscount GetByProduct(int productId, DateTime dateTime)
        {
            var action = ModulesRepository.ModuleExecuteReadOne(
                "Select Top(1) * From [Module].[BuyInTime] " +
                "Where ProductId=@ProductId And ((@dateTime between DateStart and DateExpired) Or (IsRepeat = 1)) Order by SortOrder",
                CommandType.Text, GetBuyInTimeProductModelFromReader,
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@dateTime", dateTime));

            if (action == null)
                return null;

            if (action.IsRepeat && action.DateExpired < DateTime.Now)
            {
                action.DateExpired = GetExpireDateTime(action.DateExpired, action.DaysRepeat);
            }

            return new ProductDiscount()
            {
                ProductId = productId,
                Discount = action.DiscountInTime,
                DateExpired = action.DateExpired
            };
        }

        public static BuyInTimeProductModel GetByShowMode(int showMode, DateTime dateTime)
        {
            var action = ModulesRepository.ModuleExecuteReadOne(
                "Select Top(1) * From [Module].[BuyInTime] " +
                "Where ShowMode=@ShowMode And ((@dateTime between DateStart and DateExpired) Or (IsRepeat = 1)) Order by SortOrder",
                CommandType.Text, GetBuyInTimeProductModelFromReader,
                new SqlParameter("@ShowMode", showMode),
                new SqlParameter("@dateTime", dateTime));

            if (action != null)
            {
                var cssClass = showMode == 1 ? "buy-in-time-block-horizontal" : (showMode == 2 ? "buy-in-time-block-vertical" : string.Empty);
                action.ActionText = String.Format("<article class=\"buy-in-time-block {0}\">{1}</article>", cssClass, action.ActionText);
            }

            return action;
        }

        public static BuyInTimeProductModel GetInMobile(DateTime dateTime)
        {
            var action = ModulesRepository.ModuleExecuteReadOne(
                "Select Top(1) * From [Module].[BuyInTime] " +
                "Where ShowInMobile = 1 And ((@dateTime between DateStart and DateExpired) Or (IsRepeat = 1)) Order by SortOrder",
                CommandType.Text, GetBuyInTimeProductModelFromReader,
                new SqlParameter("@dateTime", dateTime));

            if (action != null)
            {
                action.MobileActionText = String.Format("<article class=\"buy-in-time-block {0}\">{1}</article>", "buy-in-time-block-mobile", action.MobileActionText);
            }

            return action;
        }

        public static List<ProductDiscount> GetProductDiscountsList(DateTime dateTime)
        {
            return CacheManager.Get(CacheKey + "_ProductDiscountsList", 1,
                () =>
                    ModulesRepository.ModuleExecuteReadList(
                        "Select * From [Module].[BuyInTime] Where ((@dateTime between DateStart and DateExpired) Or (IsRepeat = 1))",
                        CommandType.Text,
                        reader => new ProductDiscount()
                        {
                            ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                            Discount = ModulesRepository.ConvertTo<float>(reader, "DiscountInTime"),
                            DateExpired = ModulesRepository.ConvertTo<DateTime>(reader, "DateExpired")
                        },
                        new SqlParameter("@dateTime", dateTime)));
        }

        public static DataTable GetProductsTable()
        {
            return ModulesRepository.ModuleExecuteTable(
                "Select Id, [BuyInTime].[ProductId], DateStart, DateExpired, DiscountInTime, Product.Name, SortOrder From [Module].[BuyInTime] " +
                "Left Join Catalog.Product On Product.ProductId = [BuyInTime].[ProductId] Order By SortOrder",
                CommandType.Text);
        }

        #region Get / Add / Update / Delete

        private static BuyInTimeProductModel GetBuyInTimeProductModelFromReader(SqlDataReader reader)
        {
            return new BuyInTimeProductModel
            {
                Id = ModulesRepository.ConvertTo<int>(reader, "Id"),
                ProductId = ModulesRepository.ConvertTo<int>(reader, "ProductId"),
                DateStart = ModulesRepository.ConvertTo<DateTime>(reader, "DateStart"),
                DateExpired = ModulesRepository.ConvertTo<DateTime>(reader, "DateExpired"),
                DiscountInTime = ModulesRepository.ConvertTo<float>(reader, "DiscountInTime"),
                ActionText = ModulesRepository.ConvertTo<string>(reader, "ActionText"),
                ShowMode = ModulesRepository.ConvertTo<int>(reader, "ShowMode"),
                IsRepeat = ModulesRepository.ConvertTo<bool>(reader, "IsRepeat"),
                DaysRepeat = ModulesRepository.ConvertTo<int>(reader, "DaysRepeat"),
                Picture = ModulesRepository.ConvertTo<string>(reader, "Picture"),
                SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                ShowInMobile = ModulesRepository.ConvertTo<bool>(reader, "ShowInMobile"),
                MobileActionText = ModulesRepository.ConvertTo<string>(reader, "MobileActionText"),
            };
        }

        public static BuyInTimeProductModel Get(int id)
        {
            return ModulesRepository.ModuleExecuteReadOne("Select * From [Module].[BuyInTime] Where Id=@Id",
                CommandType.Text, GetBuyInTimeProductModelFromReader,
                new SqlParameter("@Id", id));
        }

        public static void Add(BuyInTimeProductModel action)
        {
            action.Id = ModulesRepository.ModuleExecuteScalar<int>(
                "Insert Into [Module].[BuyInTime]" +
                " (ProductId,DateStart,DateExpired,DiscountInTime,ActionText,ShowMode,IsRepeat,DaysRepeat,Picture,SortOrder,ShowInMobile,MobileActionText) " +
                "Values (@ProductId,@DateStart,@DateExpired,@DiscountInTime,@ActionText,@ShowMode,@IsRepeat,@DaysRepeat,@Picture,@SortOrder,@ShowInMobile,@MobileActionText); " +
                "Select scope_identity();",
                CommandType.Text,
                new SqlParameter("@ProductId", action.ProductId),
                new SqlParameter("@DateStart", action.DateStart),
                new SqlParameter("@DateExpired", action.DateExpired),
                new SqlParameter("@DiscountInTime", action.DiscountInTime),
                new SqlParameter("@ActionText", action.ActionText),
                new SqlParameter("@ShowMode", action.ShowMode),
                new SqlParameter("@IsRepeat", action.IsRepeat),
                new SqlParameter("@DaysRepeat", action.DaysRepeat),
                new SqlParameter("@Picture", action.Picture ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", action.SortOrder),
                new SqlParameter("@ShowInMobile", action.ShowInMobile),
                new SqlParameter("@MobileActionText", action.MobileActionText));

            CacheManager.RemoveByPattern(CacheKey);
        }

        public static void Update(BuyInTimeProductModel action)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Update [Module].[BuyInTime] " +
                "Set ProductId=@ProductId, DateStart=@DateStart, DateExpired=@DateExpired, DiscountInTime=@DiscountInTime, ActionText=@ActionText, ShowMode=@ShowMode, " +
                    "IsRepeat=@IsRepeat, DaysRepeat=@DaysRepeat, Picture=@Picture, SortOrder=@SortOrder, ShowInMobile=@ShowInMobile, MobileActionText=@MobileActionText " +
                "Where Id=@Id",
                CommandType.Text,
                new SqlParameter("@Id", action.Id),
                new SqlParameter("@ProductId", action.ProductId),
                new SqlParameter("@DateStart", action.DateStart),
                new SqlParameter("@DateExpired", action.DateExpired),
                new SqlParameter("@DiscountInTime", action.DiscountInTime),
                new SqlParameter("@ActionText", action.ActionText),
                new SqlParameter("@ShowMode", action.ShowMode),
                new SqlParameter("@IsRepeat", action.IsRepeat),
                new SqlParameter("@DaysRepeat", action.DaysRepeat),
                new SqlParameter("@Picture", action.Picture ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", action.SortOrder),
                new SqlParameter("@ShowInMobile", action.ShowInMobile),
                new SqlParameter("@MobileActionText", action.MobileActionText));

            CacheManager.RemoveByPattern(CacheKey);
        }

        public static void UpdatePicture(int actionId, string picture)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Update [Module].[BuyInTime] Set Picture=@Picture Where Id=@Id",
                CommandType.Text,
                new SqlParameter("@Id", actionId),
                new SqlParameter("@Picture", picture ?? (object)DBNull.Value));

            CacheManager.RemoveByPattern(CacheKey);
        }

        public static void Delete(int id)
        {
            var model = Get(id);
            if (model == null)
                return;
            if (!string.IsNullOrEmpty(model.Picture) && File.Exists(PicturePath + model.Picture))
                File.Delete(PicturePath + model.Picture);

            ModulesRepository.ModuleExecuteNonQuery("Delete From [Module].[BuyInTime] Where Id=@Id",
                CommandType.Text, new SqlParameter("@Id", id));

            CacheManager.RemoveByPattern(CacheKey);
        }

        #endregion
    }
}
