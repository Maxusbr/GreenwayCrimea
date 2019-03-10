//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Modules;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Repository.Currencies;
using AdvantShop.Configuration;
using AdvantShop.Repository;

namespace AdvantShop.Module.BuyMore.Domain
{
    public class BuyMoreService
    {
        #region Install / Uninstall

        public static bool InstallBuyMoreModule()
        {
            ModulesRepository.ModuleExecuteNonQuery(
            @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Module." + BuyMore.ModuleStringId + @"') AND type in (N'U'))
                Begin
                   CREATE TABLE Module." + BuyMore.ModuleStringId + @"
	                (
	                ID int NOT NULL IDENTITY (1, 1),
	                OrderPriceFrom float(53) NOT NULL,
	                GiftOffersIds nvarchar(max) NULL,
	                FreeShipping bit NULL
	                )  ON [PRIMARY]
                ALTER TABLE Module.BuyMore ADD CONSTRAINT
	                PK_BuyMore PRIMARY KEY CLUSTERED 
	                (
	                ID
	                ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                End",
                CommandType.Text);

            if (string.IsNullOrWhiteSpace(ModuleSettingsProvider.GetSettingValue<string>("ShippingPriceTo", BuyMore.ModuleStringId)))
            {
                ModuleSettingsProvider.SetSettingValue("ShippingPriceTo", "500", BuyMore.ModuleStringId);
            }

            if (string.IsNullOrWhiteSpace(ModuleSettingsProvider.GetSettingValue<string>("MissingDiscount", BuyMore.ModuleStringId)))
            {
                ModuleSettingsProvider.SetSettingValue("MissingDiscount", "20", BuyMore.ModuleStringId);
            }

            if (string.IsNullOrWhiteSpace(ModuleSettingsProvider.GetSettingValue<string>("DisplayAlways", BuyMore.ModuleStringId)))
            {
                ModuleSettingsProvider.SetSettingValue("DisplayAlways", true, BuyMore.ModuleStringId);
            }

            UpdateLocalization();
            return true;
        }

        public static bool UninstallBuyMoreModule()
        {
            return true;
        }

        public static bool UpdateBuyMoreModule()
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BuyMore' AND COLUMN_NAME = 'GiftOffersIds') begin " +
                "Exec('Alter table Module.BuyMore Add GiftOffersIds nvarchar(max) NULL') " +
                "Exec('Update Module.BuyMore Set GiftOffersIds = GiftOfferID') " +
                "Exec('Alter table Module.BuyMore DROP COLUMN GiftOfferID') " +
                "END",
                CommandType.Text);

            UpdateLocalization();
            return true;
        }
        
        public static void UpdateLocalization()
        {
            LocalizationService.AddOrUpdateResource(1, "BuyMore.NextAction.NotHaveEnough", "Вам не хватает еще");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.NextAction.ToReceive", "чтобы получить");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.NextAction.FreeShipping", "бесплатную доставку");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.NextAction.Discount", "скидку");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.NextAction.Gift", "подарок");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.CurrentAction.AmountIsMore", "Сумма вашего заказа более");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.CurrentAction.YouReceive", "поэтому вы получаете:");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.CurrentAction.FreeShipping", "бесплатную доставку");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.CurrentAction.Discount", "скидку");
            LocalizationService.AddOrUpdateResource(1, "BuyMore.CurrentAction.Gift", "подарок");

            LocalizationService.AddOrUpdateResource(2, "BuyMore.NextAction.NotHaveEnough", "You do not have enough");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.NextAction.ToReceive", "to receive");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.NextAction.FreeShipping", "free shipping");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.NextAction.Discount", "discount");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.NextAction.Gift", "gift");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.CurrentAction.AmountIsMore", "The amount of your order is more");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.CurrentAction.YouReceive", "therefore you receive:");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.CurrentAction.FreeShipping", "free shipping");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.CurrentAction.Discount", "discount");
            LocalizationService.AddOrUpdateResource(2, "BuyMore.CurrentAction.Gift", "gift");
        }
        #endregion


        public static List<BuyMoreProductModel> GetAll()
        {
            return ModulesRepository.ModuleExecuteReadList<BuyMoreProductModel>(
                "Select * From [Module].[BuyMore] order by OrderPriceFrom",
                CommandType.Text, GetBuyMoreProductModelFromReader);
        }


        //public static float CalcDiscount(ShoppingCart cart)
        //{
        //    var list = GetAll();
        //    BuyMoreProductModel currentAction = null;

        //    foreach (var action in list)
        //    {
        //        if (cart.TotalPrice >= action.OrderPriceFrom)
        //        {
        //            currentAction = action;
        //        }
        //    }

        //    if (currentAction != null && currentAction.GiftOfferID.HasValue)
        //    {
        //        return currentAction.OrderDiscount;
        //    }

        //    return 0;
        //}

        public static void AddOrRemoveCartItem()
        {
            var cart = ShoppingCartService.CurrentShoppingCart;

            var list = GetAll();
            BuyMoreProductModel currentAction = null;

            foreach (var action in list)
            {
                if (cart.TotalPrice >= action.OrderPriceFrom)
                {
                    currentAction = action;
                }
            }

            foreach (var item in cart.Where(item => item.IsGift && item.ModuleKey == BuyMore.ModuleStringId))
            {
                if (currentAction == null || !currentAction.GiftOffersIdsList.Any(giftOfferId => giftOfferId == item.OfferId)
                    || item.Offer.Amount <= item.Offer.Product.MinAmount || !item.Offer.Product.Enabled
                    || !item.Offer.Product.CategoryEnabled) 
                {
                    ShoppingCartService.DeleteShoppingCartItem(item, false);
                }
            }

            if (currentAction != null)
            {
                foreach (var offerId in currentAction.GiftOffersIdsList)
                {
                    if (cart.All(item => item.OfferId != offerId))
                    {
                        var newItem = new ShoppingCartItem()
                        {
                            OfferId = offerId,
                            Amount = 1,
                            IsGift = true,
                            ModuleKey = BuyMore.ModuleStringId
                        };

                        ShoppingCartService.AddShoppingCartItem(newItem, false);
                        cart.Add(newItem);
                    }
                }
            }
        }

        public static void AddOrRemoveCart(ShoppingCart cart)
        {
            var list = GetAll();
            BuyMoreProductModel currentAction = null;

            foreach (var action in list)
            {
                if (cart.TotalPrice >= action.OrderPriceFrom)
                {
                    currentAction = action;
                }
            }

            foreach (var item in cart.Where(item => item.IsGift && item.ModuleKey == BuyMore.ModuleStringId).ToList())
            {
                if (currentAction == null || !currentAction.GiftOffersIdsList.Any(giftOfferId => giftOfferId == item.OfferId))
                {
                    ShoppingCartService.DeleteShoppingCartItem(item, false);
                    cart.Remove(item);
                }
            }

            if (currentAction != null)
            {
                foreach (var offerId in currentAction.GiftOffersIdsList)
                {
                    var offer = OfferService.GetOffer(offerId);
                    if (cart.All(item => item.OfferId != offerId) && offer != null && ProductService.IsProductEnabled(offer.ProductId))
                    {
                        var newItem = new ShoppingCartItem()
                        {
                            OfferId = offerId,
                            Amount = 1,
                            IsGift = true,
                            ModuleKey = BuyMore.ModuleStringId
                        };

                        ShoppingCartService.AddShoppingCartItem(newItem, false);
                        cart.Add(newItem);
                    }
                }
            }
        }

        public static void ProcessOptions(List<BaseShippingOption> options, List<PreOrderItem> cart)
        {
            var priceTo = ModuleSettingsProvider.GetSettingValue<float>("ShippingPriceTo", BuyMore.ModuleStringId);
            var excludedShippingsIds = ModuleSettingsProvider.GetSettingValue<string>("ExcludedShippingsIds", BuyMore.ModuleStringId) ?? string.Empty;

            var list = GetAll();
            BuyMoreProductModel currentAction = null;

            var totalPrice = cart.Sum(item => item.Amount * item.Price);

            foreach (var action in list)
            {
                if (totalPrice >= action.OrderPriceFrom)
                {
                    currentAction = action;
                }
            }

            if (currentAction != null && currentAction.FreeShipping)
            {
                foreach (var option in options)
                {
                    if (!excludedShippingsIds.Split(',').Contains(option.MethodId.ToString()) && option.Rate <= priceTo)
                        option.Rate = 0;
                }
            }
        }

        public static float GetTotalPrice(ShoppingCart cart)
        {
            return (cart.TotalPrice - cart.TotalPriceIgnoreDiscount).RoundPrice(CurrencyService.CurrentCurrency.Rate ,CurrencyService.GetCurrencyByIso3("RUB"));
        }

        public static float GetTotalItems(ShoppingCart cart)
        {
            return (cart.TotalItems - cart.TotalItemsIgnoreDiscount).RoundPrice(CurrencyService.CurrentCurrency.Rate, CurrencyService.GetCurrencyByIso3("RUB"));
        }

        public static List<ShippingMethod> GetShippingMethodsByGeoMapping(List<ShippingMethod> listMethods)
        {
            var currentZone = IpZoneContext.CurrentZone;
            var items = new List<ShippingMethod>();
            foreach (var shippingMethod in listMethods)
            {
                if (ShippingPaymentGeoMaping.IsExistGeoShipping(shippingMethod.ShippingMethodId))
                {
                    if (ShippingPaymentGeoMaping.CheckShippingEnabledGeo(shippingMethod.ShippingMethodId, currentZone.CountryName, currentZone.City))
                        items.Add(shippingMethod);
                }
                else
                    items.Add(shippingMethod);
            }
            return items;
        }

        #region Get / Add / Update / Delete

        private static BuyMoreProductModel GetBuyMoreProductModelFromReader(SqlDataReader reader)
        {
            return new BuyMoreProductModel
            {
                Id = ModulesRepository.ConvertTo<int>(reader, "ID"),
                OrderPriceFrom = ModulesRepository.ConvertTo<float>(reader, "OrderPriceFrom"),
                FreeShipping = ModulesRepository.ConvertTo<bool>(reader, "FreeShipping"),
                GiftOffersIds = ModulesRepository.ConvertTo<string>(reader, "GiftOffersIds")
            };
        }

        public static BuyMoreProductModel Get(int id)
        {
            return ModulesRepository.ModuleExecuteReadOne("Select * From [Module].[BuyMore] Where Id=@Id",
                CommandType.Text, GetBuyMoreProductModelFromReader,
                new SqlParameter("@Id", id));
        }

        public static void Add(BuyMoreProductModel model)
        {
            model.Id = ModulesRepository.ModuleExecuteScalar<int>(
                "Insert Into [Module].[BuyMore]" +
                " (OrderPriceFrom, FreeShipping, GiftOffersIds) " +
                "Values (@OrderPriceFrom, @FreeShipping, @GiftOffersIds); " +
                "Select scope_identity();",
                CommandType.Text,
                new SqlParameter("@OrderPriceFrom", model.OrderPriceFrom),
                new SqlParameter("@FreeShipping", model.FreeShipping),
                new SqlParameter("@GiftOffersIds", model.GiftOffersIds ?? (object)DBNull.Value)
                );

            CacheManager.RemoveByPattern(CacheNames.ShippingOptions);
        }

        public static void Update(BuyMoreProductModel model)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Update [Module].[BuyMore] " +
                "Set OrderPriceFrom=@OrderPriceFrom, FreeShipping=@FreeShipping, GiftOffersIds=@GiftOffersIds " +
                "Where Id=@Id",
                CommandType.Text,
                new SqlParameter("@ID", model.Id),
                new SqlParameter("@OrderPriceFrom", model.OrderPriceFrom),
                new SqlParameter("@FreeShipping", model.FreeShipping),
                new SqlParameter("@GiftOffersIds", model.GiftOffersIds ?? (object)DBNull.Value)
                );

            CacheManager.RemoveByPattern(CacheNames.ShippingOptions);
        }

        public static void Delete(int id)
        {
            ModulesRepository.ModuleExecuteNonQuery("Delete From [Module].[BuyMore] Where Id=@Id",
                CommandType.Text, new SqlParameter("@Id", id));

            CacheManager.RemoveByPattern(CacheNames.ShippingOptions);
        }

        public static void DeleteGiftFromShoppingCart(string OfferId)
        {
            ModulesRepository.ModuleExecuteNonQuery("Delete From [Catalog].[ShoppingCart] Where OfferId in (@OfferId) And ModuleKey = 'BuyMore'",
                CommandType.Text, new SqlParameter("@OfferId", OfferId));
        }

        #endregion
    }
}
