//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Module.ProductSets.Domain
{
    public class ProductSetsService
    {
        #region Module

        public static bool InstallProductSetsModule()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "ProductSet"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[ProductSet](
	                    [ProductId] [int] NOT NULL,
	                    [LinkedOfferId] [int] NOT NULL,
                        CONSTRAINT [PK_ProductSet] PRIMARY KEY CLUSTERED 
                    (
	                    [ProductId] ASC,
	                    [LinkedOfferId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
                    ) ON [PRIMARY]", CommandType.Text);
            }

            ModuleSettingsProvider.SetSettingValue("Title", "Вместе с этим товаром покупают", ProductSets.ModuleId);

            return ModulesRepository.IsExistsModuleTable("Module", "ProductSet");
        }

        public static bool UpdateProductSetsModule()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "ProductSetDiscount"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[ProductSetDiscount](
	                    [ProductId] [int] NOT NULL,
	                    [Discount] [float] NOT NULL,
                        CONSTRAINT [PK_ProductSetDiscount] PRIMARY KEY CLUSTERED 
                    (
	                    [ProductId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
                    ) ON [PRIMARY]", CommandType.Text);
            }
            return true;
        }

        public static bool UninstallProductSetsModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("Title", ProductSets.ModuleId);
            return true;
        }

        #endregion

        #region Product Set

        public static bool CanBuyOffer(Offer offer)
        {
            return offer.Product.Enabled && offer.Product.CategoryEnabled &&
                   offer.Amount > 0 && offer.RoundedPrice > 0;
        }

        public static bool CanAddOfferToCart(Offer offer)
        {
            return !CustomOptionsService.DoesProductHaveRequiredCustomOptions(offer.ProductId);
        }

        public static List<Offer> GetLinkedOffers(int productId, bool onlyCanBuy = false)
        {
            var offers = GetLinkedOfferIds(productId).Select(OfferService.GetOffer).Where(offer => offer != null).ToList();
            if (onlyCanBuy)
                offers.RemoveAll(o => !CanBuyOffer(o) || !CanAddOfferToCart(o));
            return offers;
        }

        private static List<int> GetLinkedOfferIds(int productId)
        {
            return ModulesRepository.ModuleExecuteReadColumn<int>(
                "SELECT LinkedOfferId FROM [Module].[ProductSet] WHERE ProductId = @ProductId", CommandType.Text,
                "LinkedOfferId", new SqlParameter("@ProductId", productId));
        }

        public static void AddProductLink(int productId, int linkedOfferId)
        {
            if (IsExistSet(productId, linkedOfferId))
                return;
            ModulesRepository.ModuleExecuteNonQuery(
                "INSERT INTO [Module].[ProductSet] (ProductId, LinkedOfferId) VALUES (@ProductId, @LinkedOfferId)",
                CommandType.Text, new SqlParameter("@ProductId", productId),
                new SqlParameter("@LinkedOfferId", linkedOfferId));
        }

        public static void DeleteProductLink(int productId, int linkedOfferId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[ProductSet] WHERE ProductId = @ProductId AND LinkedOfferId = @LinkedOfferId",
                CommandType.Text, new SqlParameter("@ProductId", productId),
                new SqlParameter("@LinkedOfferId", linkedOfferId));
        }

        public static void ClearProductLinks(int productId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[ProductSet] WHERE ProductId = @ProductId",
                CommandType.Text, new SqlParameter("@ProductId", productId));
        }

        public static bool IsExistSet(int productId, int linkedOfferId)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "SELECT COUNT(*) FROM [Module].[ProductSet] WHERE ProductId = @ProductId AND LinkedOfferId = @LinkedOfferId",
                CommandType.Text, new SqlParameter("@ProductId", productId),
                new SqlParameter("@LinkedOfferId", linkedOfferId)) > 0;
        }

        private static bool HasProductSet(int productId)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "SELECT COUNT(*) FROM [Module].[ProductSet] WHERE ProductId = @ProductId",
                CommandType.Text, new SqlParameter("@ProductId", productId)) > 0;
        }

        private static int? GetProductIdByOfferInSet(int offerId)
        {
            return ModulesRepository.ModuleExecuteReadOne<int?>(
                "SELECT ProductId FROM [Module].[ProductSet] WHERE LinkedOfferId = @LinkedOfferId",
                CommandType.Text, reader => ModulesRepository.ConvertTo<int?>(reader, "ProductId"),
                new SqlParameter("@LinkedOfferId", offerId));
        }

        public static void SetDiscount(int productId, float discount)
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "ProductSetDiscount"))
            {
                UpdateProductSetsModule();
            }

            ModulesRepository.ModuleExecuteNonQuery(
                "IF ((SELECT COUNT(*) FROM Module.ProductSetDiscount WHERE ProductId = @ProductId) > 0) " +
                "UPDATE Module.ProductSetDiscount SET Discount = @Discount WHERE ProductId = @ProductId " +
                "ELSE " +
                "INSERT INTO Module.ProductSetDiscount (ProductId, Discount) VALUES (@ProductId, @Discount) ",
                CommandType.Text, new SqlParameter("@ProductId", productId), new SqlParameter("@Discount", discount));
        }

        public static float GetDiscount(int productId)
        {

            if (!ModulesRepository.IsExistsModuleTable("Module", "ProductSetDiscount"))
            {
                UpdateProductSetsModule();
            }

            return ModulesRepository.ModuleExecuteScalar<float>(
            "SELECT Discount FROM Module.ProductSetDiscount WHERE ProductId = @ProductId",
            CommandType.Text, new SqlParameter("@ProductId", productId));
        }

        #region Export/Import

        public static string LinkedOffersToString(int productId, string columnSeparator)
        {
            var items = ModulesRepository.ModuleExecuteReadColumn<string>(
                "Select ArtNo from Catalog.Offer inner join [Module].[ProductSet] on ProductSet.LinkedOfferId = Offer.OfferId where ProductSet.ProductId = @ProductId",
                CommandType.Text, "ArtNo", new SqlParameter("@ProductId", productId));
            return items.Aggregate(new StringBuilder(), (current, next) => current.AppendFormat("{0}{1}", next, columnSeparator),
                    current => current.ToString().TrimEnd(columnSeparator.ToCharArray()));
        }

        public static bool LinkedOffersFromString(int productId, string value, string columnSeparator)
        {
            ClearProductLinks(productId);

            if (string.IsNullOrEmpty(value))
                return true;

            var arrArt = value.Split(new[] { columnSeparator }, StringSplitOptions.None);
            foreach (string t in arrArt)
            {
                var artNo = t.Trim();
                if (string.IsNullOrWhiteSpace(artNo))
                    continue;
                var offer = OfferService.GetOffer(artNo);
                if (offer != null)
                    AddProductLink(productId, offer.OfferId);
            }
            return true;
        }

        public static bool SetDiscountFromString(int productId, string value)
        {
            var discount = value.TryParseFloat();
            SetDiscount(productId, discount);
            return true;
        }

        #endregion

        public static void CopyProductSets(Product sourceProduct, Product newProduct)
        {
            foreach (var offerId in GetLinkedOfferIds(sourceProduct.ProductId))
            {
                AddProductLink(newProduct.ProductId, offerId);
            }
        }

        public static Dictionary<int, List<ShoppingCartItem>> GetCartProductSets(ShoppingCart cart)
        {
            var result = new Dictionary<int, List<ShoppingCartItem>>();
            var items = cart.Where(x => x.ModuleKey == ProductSets.ModuleId);
            var processedItems = new List<int>();

            foreach (var item in items)
            {
                if (result.ContainsKey(item.ShoppingCartItemId) || processedItems.Contains(item.ShoppingCartItemId) || !HasProductSet(item.Offer.ProductId) || GetDiscount(item.Offer.ProductId) == 0)
                    continue;
                var offers = GetLinkedOffers(item.Offer.ProductId, true);
                if (!offers.Any() || !offers.All(x => items.Where(y => !processedItems.Contains(y.ShoppingCartItemId)).Select(y => y.OfferId).Contains(x.OfferId)))
                    continue;

                var itemsInSet = new List<ShoppingCartItem>();
                foreach (var offer in offers)
                {
                    var itemInSet = items.FirstOrDefault(x => x.OfferId == offer.OfferId && x.Amount == item.Amount && !processedItems.Contains(x.ShoppingCartItemId));
                    if (itemInSet != null)
                        itemsInSet.Add(itemInSet);
                }
                if (!itemsInSet.Any())
                    continue;
                itemsInSet.Insert(0, item);
                result.Add(item.ShoppingCartItemId, itemsInSet);
                processedItems.AddRange(itemsInSet.Select(x => x.ShoppingCartItemId));
            }

            return result;
        }

        public static bool CartHasProductSet(ShoppingCart cart, int productId, out List<ShoppingCartItem> itemsInSet)
        {
            itemsInSet = new List<ShoppingCartItem>();

            ShoppingCartItem item = cart.FirstOrDefault(x => x.Offer.ProductId == productId && x.ModuleKey == ProductSets.ModuleId);
            if (item == null || !HasProductSet(productId) || GetDiscount(productId) == 0)
                return false;

            var offers = GetLinkedOffers(item.Offer.ProductId, true);
            if (!offers.Any())
                return false;

            var itemsToSearch = cart.Where(x => x.ModuleKey == ProductSets.ModuleId && x.Amount == item.Amount && !x.IsGift && x.AttributesXml.IsNullOrEmpty()).ToList();
            if (offers.All(x => itemsToSearch.Select(y => y.OfferId).Contains(x.OfferId)))
            {
                itemsInSet.Add(item);
                itemsInSet.AddRange(itemsToSearch.Where(x => offers.Select(y => y.OfferId).Contains(x.OfferId)));
                return true;
            }
            return false;
        }

        public static bool CartHasProductSet(ShoppingCart cart, int productId)
        {
            List<ShoppingCartItem> itemsInSet;
            return CartHasProductSet(cart, productId, out itemsInSet);
        }

        public static void ProcessCart()
        {
            var cart = ShoppingCartService.CurrentShoppingCart;

            var processedItems = new List<int>();
            var itemsToDelete = new List<int>();

            for (int i = 0; i < cart.Count; i++)
            {
                var curItem = cart[i];
                if (processedItems.Contains(curItem.ShoppingCartItemId) || // уже обработан
                    itemsToDelete.Contains(curItem.ShoppingCartItemId) || // будет удален
                    (curItem.ModuleKey.IsNotEmpty() && curItem.ModuleKey != ProductSets.ModuleId) || // обработан другим модулем
                    !HasProductSet(curItem.Offer.ProductId) || // нет комплекта
                    GetDiscount(curItem.Offer.ProductId) == 0) // не задана скидка
                    continue;

                var offers = GetLinkedOffers(curItem.Offer.ProductId, true);

                var notProcessedCart = cart.Where(x => !processedItems.Contains(x.ShoppingCartItemId) && !itemsToDelete.Contains(x.ShoppingCartItemId)).ToList();
                if (!offers.Any() || !offers.All(x => notProcessedCart.Select(y => y.OfferId).Contains(x.OfferId))) // комплект не полный
                    continue;

                processedItems.Add(curItem.ShoppingCartItemId);

                float minAmount = 0; // наименьшее кол-во товара в комплекте
                var itemsInSet = new List<ShoppingCartItem>();
                foreach (var offer in offers)
                {
                    var items = cart.Where(x => !processedItems.Contains(x.ShoppingCartItemId) && !itemsToDelete.Contains(x.ShoppingCartItemId) &&
                                                !x.IsGift && x.AttributesXml.IsNullOrEmpty() && x.OfferId == offer.OfferId &&
                                                (x.ModuleKey.IsNullOrEmpty() || x.ModuleKey == ProductSets.ModuleId)).ToList();
                    var itemsAmount = items.Sum(x => x.Amount);
                    if (minAmount == 0 || itemsAmount < minAmount)
                        minAmount = itemsAmount;
                    itemsInSet.Add(items[0]);
                }

                if (minAmount == 0)
                    continue;

                itemsInSet.Insert(0, curItem);

                foreach (var item in itemsInSet)
                {
                    if (item.ModuleKey != ProductSets.ModuleId)
                    {
                        item.ModuleKey = ProductSets.ModuleId;
                        ShoppingCartService.UpdateShoppingCartItem(item, false);
                    }

                    var sameItems = cart.Where(x => x.OfferId == item.OfferId &&
                        !processedItems.Contains(x.ShoppingCartItemId) && !itemsToDelete.Contains(x.ShoppingCartItemId) &&
                        !x.IsGift && x.AttributesXml == item.AttributesXml && x.ShoppingCartItemId != item.ShoppingCartItemId &&
                        (x.ModuleKey.IsNullOrEmpty() || x.ModuleKey == ProductSets.ModuleId)).ToList();
                    if (item.Amount < minAmount)
                    {
                        if (!sameItems.Any())
                        {
                            minAmount = item.Amount;
                        }
                        else
                        {
                            var totalOffersAmount = item.Amount + sameItems.Sum(x => x.Amount);
                            item.Amount = totalOffersAmount < minAmount ? totalOffersAmount : minAmount;
                            ShoppingCartService.UpdateShoppingCartItem(item, false);

                            if (totalOffersAmount > item.Amount)
                            {
                                sameItems[0].Amount = totalOffersAmount - item.Amount;
                                sameItems[0].ModuleKey = null;
                                ShoppingCartService.UpdateShoppingCartItem(sameItems[0], false);
                                itemsToDelete.AddRange(sameItems.Where(x => x.ShoppingCartItemId != sameItems[0].ShoppingCartItemId).Select(x => x.ShoppingCartItemId));
                            }
                            else
                            {
                                itemsToDelete.AddRange(sameItems.Select(x => x.ShoppingCartItemId));
                            }
                        }
                    }
                    else if (item.Amount > minAmount)
                    {
                        if (!sameItems.Any())
                        {
                            var newItem = (ShoppingCartItem)item.Clone();
                            newItem.Amount = item.Amount - minAmount;
                            newItem.ModuleKey = null;
                            newItem.ShoppingCartItemId = ShoppingCartService.AddShoppingCartItem(newItem, false);
                            cart.Add(newItem);
                        }
                        else
                        {
                            sameItems[0].Amount += item.Amount - minAmount;
                            sameItems[0].ModuleKey = null;
                            ShoppingCartService.UpdateShoppingCartItem(sameItems[0], false);
                            itemsToDelete.AddRange(sameItems.Where(x => x.ShoppingCartItemId != sameItems[0].ShoppingCartItemId).Select(x => x.ShoppingCartItemId));
                        }
                        item.Amount = minAmount;
                        ShoppingCartService.UpdateShoppingCartItem(item, false);
                    }
                    processedItems.Add(item.ShoppingCartItemId);
                }
            }
            foreach (var item in cart.Where(x => !processedItems.Contains(x.ShoppingCartItemId) && !itemsToDelete.Contains(x.ShoppingCartItemId) && x.ModuleKey == ProductSets.ModuleId))
            {
                var sameItems = cart.Where(x => x.OfferId == item.OfferId && x.ShoppingCartItemId != item.ShoppingCartItemId &&
                    !processedItems.Contains(x.ShoppingCartItemId) && !itemsToDelete.Contains(x.ShoppingCartItemId) &&
                    !x.IsGift && x.AttributesXml == item.AttributesXml && x.ModuleKey.IsNullOrEmpty()).ToList();
                if (sameItems.Any())
                {
                    sameItems[0].Amount += item.Amount;
                    ShoppingCartService.UpdateShoppingCartItem(sameItems[0], false);
                    itemsToDelete.Add(item.ShoppingCartItemId);
                }
                else
                {
                    item.ModuleKey = null;
                    ShoppingCartService.UpdateShoppingCartItem(item, false);
                }
            }
            foreach (var item in cart.Where(x => itemsToDelete.Contains(x.ShoppingCartItemId)))
            {
                ShoppingCartService.DeleteShoppingCartItem(item, false);
            }
        }

        public static Discount GetCartItemDiscount(int cartItemId)
        {
            var cart = ShoppingCartService.GetShoppingCart(ShoppingCartType.ShoppingCart, CustomerContext.CustomerId); //ShoppingCartService.CurrentShoppingCart;
            var sets = GetCartProductSets(cart);

            ShoppingCartItem mainItem = null;
            foreach (var kvp in sets)
            {
                if (kvp.Key != cartItemId && !kvp.Value.Select(x => x.ShoppingCartItemId).Contains(cartItemId))
                    continue;
                mainItem = cart.FirstOrDefault(x => x.ShoppingCartItemId == kvp.Key);
                break;
            }
            if (mainItem == null)
                return null;

            return new Discount(GetDiscount(mainItem.Offer.ProductId), 0);
        }

        #endregion
    }
}
