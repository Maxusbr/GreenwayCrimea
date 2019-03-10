//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class OfferService
    {
        public static int AddOffer(Offer offer)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddOffer]", CommandType.StoredProcedure,
                                          new SqlParameter("@ProductID", offer.ProductId),
                                          new SqlParameter("@ArtNo", offer.ArtNo),
                                          new SqlParameter("@Amount", offer.Amount),
                                          new SqlParameter("@Price", offer.BasePrice),
                                          new SqlParameter("@SupplyPrice", offer.SupplyPrice),
                                          new SqlParameter("@ColorID", offer.ColorID ?? (object)DBNull.Value),
                                          new SqlParameter("@SizeID", offer.SizeID ?? (object)DBNull.Value),
                                          new SqlParameter("@Main", offer.Main));
        }

        public static void UpdateOffer(Offer offer)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateOffer]", CommandType.StoredProcedure,
                                            new SqlParameter("@OfferId", offer.OfferId),
                                            new SqlParameter("@ProductID", offer.ProductId),
                                            new SqlParameter("@ArtNo", offer.ArtNo),
                                            new SqlParameter("@Amount", offer.Amount),
                                            new SqlParameter("@Price", offer.BasePrice),
                                            new SqlParameter("@SupplyPrice", offer.SupplyPrice),
                                            new SqlParameter("@ColorID", offer.ColorID ?? (object)DBNull.Value),
                                            new SqlParameter("@SizeID", offer.SizeID ?? (object)DBNull.Value),
                                            new SqlParameter("@Main", offer.Main));
        }

        public static void DeleteOffer(int offerId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete FROM [Catalog].[Offer] WHERE [offerID] = @offerID",
                                          CommandType.Text,
                                          new SqlParameter("@offerID", offerId));
        }


        public static void DeleteOldOffers(int productId, List<Offer> newOffers)
        {
            string query = newOffers != null && newOffers.Any()
                               ? string.Format("Delete FROM [Catalog].[Offer] WHERE [productId] = @productId and offerID not in ({0})",
                                    newOffers.Select(offer => offer.OfferId).AggregateString(","))
                               : "Delete FROM [Catalog].[Offer] WHERE [productId] = @productId";

            SQLDataAccess.ExecuteNonQuery(query, CommandType.Text,
                new SqlParameter("@productId", productId));
        }

        public static List<Offer> GetProductOffers(int productId)
        {
            return SQLDataAccess.ExecuteReadList<Offer>(
                     "SELECT OfferID, Offer.ProductID, Offer.ArtNo, Price, Amount, SupplyPrice, ColorID, SizeID, Main, CurrencyValue " +
                     "FROM Catalog.Offer " +
                     "Inner Join Catalog.Product on Offer.ProductID = Product.ProductID " +
                     "Inner Join Catalog.Currency on Currency.CurrencyID = Product.CurrencyID " +
                     "WHERE Offer.ProductID = @ProductID",
                     CommandType.Text,
                     GetOfferFromReader,
                     new SqlParameter("@ProductID", productId));
        }

        public static Offer GetOffer(int offerId)
        {
            return SQLDataAccess.ExecuteReadOne<Offer>(
                     "SELECT OfferID, Offer.ProductID, Offer.ArtNo, Price, Amount, SupplyPrice, ColorID, SizeID, Main, CurrencyValue " +
                     "FROM Catalog.Offer " +
                     "Inner Join Catalog.Product on Offer.ProductID = Product.ProductID " +
                     "Inner Join Catalog.Currency on Currency.CurrencyID = Product.CurrencyID " +
                     "WHERE [offerID] = @offerID",
                     CommandType.Text,
                     GetOfferFromReader,
                     new SqlParameter("@offerID", offerId));
        }

        public static Offer GetOffer(string artNo)
        {
            return SQLDataAccess.ExecuteReadOne<Offer>(
                     "SELECT OfferID, Offer.ProductID, Offer.ArtNo, Price, Amount, SupplyPrice, ColorID, SizeID, Main, CurrencyValue " +
                     "FROM Catalog.Offer " +
                     "Inner Join Catalog.Product on Offer.ProductID = Product.ProductID " +
                     "Inner Join Catalog.Currency on Currency.CurrencyID = Product.CurrencyID " +
                     "WHERE Offer.ArtNo = @ArtNo",
                     CommandType.Text,
                     GetOfferFromReader,
                     new SqlParameter("@ArtNo", artNo));
        }

        public static Offer GetMainOffer(List<Offer> offers, bool allowPreOrder, int? colorid = null, int? sizeId = null)
        {
            if (offers == null || !offers.Any())
                return null;
            // для соответствия фото в каталоге и карточке товара
            var mainOffer = SettingsCatalog.ShowOnlyAvalible && offers.Count(x => x.BasePrice > 0 && x.Amount > 0) > 0 ?
                offers.Where(x => x.BasePrice > 0 && x.Amount > 0).OrderByDescending(o => o.Main).FirstOrDefault() :
                offers.OrderByDescending(o => o.Main).FirstOrDefault();
            if (!colorid.HasValue && mainOffer != null && mainOffer.ColorID.HasValue)
                colorid = mainOffer.ColorID;

            Offer CurrentOffer = null;

            // сначала доступные к покупке или оформлению под заказ
            if (colorid.HasValue && sizeId.HasValue)
            {
                CurrentOffer = GetMainOffer(offers, o => o.ColorID == colorid && o.SizeID == sizeId && IsAvailableOffer(o, allowPreOrder));
            }
            else if (colorid.HasValue)
            {
                CurrentOffer = GetMainOffer(offers, o => o.ColorID == colorid && IsAvailableOffer(o, allowPreOrder));
            }
            else if (sizeId.HasValue)
            {
                CurrentOffer = GetMainOffer(offers, o => o.SizeID == sizeId && IsAvailableOffer(o, allowPreOrder));
            }

            if (CurrentOffer == null)
            {
                CurrentOffer = GetMainOffer(offers, o => IsAvailableOffer(o, allowPreOrder));
            }

            // если нет доступных оферов
            if (CurrentOffer == null && (colorid.HasValue || sizeId.HasValue))
            {
                if (colorid.HasValue && sizeId.HasValue)
                {
                    CurrentOffer = GetMainOffer(offers, o => o.ColorID == colorid && o.SizeID == sizeId);
                }
                else if (colorid.HasValue)
                {
                    CurrentOffer = GetMainOffer(offers, o => o.ColorID == colorid);
                }
                else if (sizeId.HasValue)
                {
                    CurrentOffer = GetMainOffer(offers, o => o.SizeID == sizeId);
                }
            }

            return CurrentOffer ?? offers.FirstOrDefault();
        }

        private static Offer GetMainOffer(List<Offer> offers, Func<Offer, bool> condition)
        {
            return offers.OrderByDescending(o => o.Main)
                .ThenBy(o => o.Color != null ? o.Color.SortOrder : 0)
                .ThenBy(o => o.Size != null ? o.Size.SortOrder : 0)
                .FirstOrDefault(condition);
        }

        private static bool IsAvailableOffer(Offer o, bool allowPreOrder)
        {
            return (o.RoundedPrice > 0 && o.Amount > 0) || allowPreOrder;
        }

        private static Offer GetOfferFromReader(SqlDataReader reader)
        {
            var offer = new Offer
            {
                ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                BasePrice = SQLDataHelper.GetFloat(reader, "Price"),
                Amount = SQLDataHelper.GetFloat(reader, "Amount"),
                SupplyPrice = SQLDataHelper.GetFloat(reader, "SupplyPrice"),
                ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                OfferId = SQLDataHelper.GetInt(reader, "OfferID"),
                ColorID = SQLDataHelper.GetNullableInt(reader, "ColorID"),
                SizeID = SQLDataHelper.GetNullableInt(reader, "SizeID"),
                Main = SQLDataHelper.GetBoolean(reader, "Main")
            };

            return offer;
        }

        public static bool IsArtNoExist(string artNo, int offerId)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "Select Count(OfferID) from Catalog.Offer Where ArtNo=@ArtNo and OfferID<>@OfferID",
                    CommandType.Text, new SqlParameter("@ArtNo", artNo),
                    new SqlParameter("@offerID", offerId)
                    ) > 0;
        }

        public static string OffersToString(List<Offer> offers, string columSeparator, string propertySeparator)
        {
            return offers.OrderByDescending(o => o.Main).Select(offer =>
                                 offer.ArtNo + propertySeparator + (offer.Size != null ? offer.Size.SizeName : "null") + propertySeparator +
                                 (offer.Color != null ? offer.Color.ColorName : "null") + propertySeparator + offer.BasePrice +
                                 propertySeparator + offer.SupplyPrice + propertySeparator + offer.Amount).AggregateString(columSeparator);
        }

        public static void OffersFromString(Product product, string offersString, string columSeparator,string propertySeparator)
        {
            if (string.IsNullOrEmpty(columSeparator) || string.IsNullOrEmpty(propertySeparator))
                _OffersFromString(product, offersString);
            else
                _OffersFromString(product, offersString, columSeparator, propertySeparator);
        }

        private static void _OffersFromString(Product product, string offersString)
        {
            product.HasMultiOffer = true;

            var oldOffers = new List<Offer>(product.Offers);
            product.Offers.Clear();

            var mainOffer = true;

            foreach (string[] fields in offersString.Split(';').Select(str => str.Replace("[", "").Replace("]", "").Split(':')))
            {
                if (fields.Count() == 6)
                {
                    var multiOffer = oldOffers.FirstOrDefault(offer => offer.ArtNo == fields[0]) ?? new Offer();
                    multiOffer.ProductId = product.ProductId;
                    multiOffer.Main = mainOffer;

                    multiOffer.ArtNo = fields[0]; // ArtNo

                    if (fields[1] != "null") // Size
                    {
                        Size size = SizeService.GetSize(fields[1]);
                        if (size == null)
                        {
                            size = new Size { SizeName = fields[1] };
                            size.SizeId = SizeService.AddSize(size);
                        }

                        multiOffer.SizeID = size.SizeId;
                    }
                    else
                    {
                        multiOffer.SizeID = null;
                    }

                    if (fields[2] != "null") // Color
                    {
                        Color color = ColorService.GetColor(fields[2]);
                        if (color == null)
                        {
                            color = new Color { ColorName = fields[2], ColorCode = "#000000" };
                            color.ColorId = ColorService.AddColor(color);
                        }

                        multiOffer.ColorID = color.ColorId;
                    }
                    else
                    {
                        multiOffer.ColorID = null;
                    }


                    multiOffer.BasePrice = fields[3].TryParseFloat(); // Price
                    multiOffer.SupplyPrice = fields[4].TryParseFloat(); // SupplyPrice
                    multiOffer.Amount = fields[5].TryParseFloat(); //Amount

                    product.Offers.Add(multiOffer);
                    mainOffer = false;
                }
            }
        }

        private static void _OffersFromString(Product product, string offersString, string columSeparator, string propertySeparator)
        {
            product.HasMultiOffer = true;

            var oldOffers = new List<Offer>(product.Offers);
            product.Offers.Clear();

            var mainOffer = true;

            foreach (string[] fields in offersString.Replace("[", "").Replace("]", "").Split(columSeparator).Select(str => str.Split(propertySeparator)))
            {
                if (fields.Count() == 6)
                {
                    var multiOffer = oldOffers.FirstOrDefault(offer => offer.ArtNo == fields[0]) ?? new Offer();
                    multiOffer.ProductId = product.ProductId;
                    multiOffer.Main = mainOffer;

                    multiOffer.ArtNo = fields[0]; // ArtNo

                    if (fields[1] != "null") // Size
                    {
                        Size size = SizeService.GetSize(fields[1]);
                        if (size == null)
                        {
                            size = new Size { SizeName = fields[1] };
                            size.SizeId = SizeService.AddSize(size);
                        }

                        multiOffer.SizeID = size.SizeId;
                    }
                    else
                    {
                        multiOffer.SizeID = null;
                    }

                    if (fields[2] != "null") // Color
                    {
                        Color color = ColorService.GetColor(fields[2]);
                        if (color == null)
                        {
                            color = new Color { ColorName = fields[2], ColorCode = "#000000" };
                            color.ColorId = ColorService.AddColor(color);
                        }

                        multiOffer.ColorID = color.ColorId;
                    }
                    else
                    {
                        multiOffer.ColorID = null;
                    }

                    multiOffer.BasePrice = fields[3].TryParseFloat(); // Price
                    multiOffer.SupplyPrice = fields[4].TryParseFloat(); // SupplyPrice
                    multiOffer.Amount = fields[5].TryParseFloat(); //Amount

                    product.Offers.Add(multiOffer);
                    mainOffer = false;
                }
            }
        }

        public static void OfferFromFields(Product product, float? price, float? purchase, float? amount)
        {
            
            if (price == null && purchase == null && amount == null)
                return;

            product.HasMultiOffer = false;

            var singleOffer = product.Offers.FirstOrDefault() ?? new Offer();
            product.Offers.Clear();

            singleOffer.ArtNo = product.ArtNo;
            singleOffer.Main = true;
            singleOffer.ProductId = product.ProductId;
            singleOffer.BasePrice = price ?? singleOffer.BasePrice;
            singleOffer.SupplyPrice = purchase ?? singleOffer.SupplyPrice;
            singleOffer.Amount = amount ?? singleOffer.Amount;

            product.Offers.Add(singleOffer);
        }


        #region Product Gifts

        public static List<Offer> GetProductGifts(int productId)
        {
            return SQLDataAccess.ExecuteReadList<Offer>(
                     "SELECT OfferID, Offer.ProductID, Offer.ArtNo, Price, Amount, SupplyPrice, ColorID, SizeID, Main, CurrencyValue " +
                     "FROM Catalog.Offer " +
                     "Inner Join Catalog.ProductGifts on ProductGifts.GiftOfferId = Offer.OfferId " +
                     "Inner Join Catalog.Product on Offer.ProductID = Product.ProductID " +
                     "Inner Join Catalog.Currency on Currency.CurrencyID = Product.CurrencyID " +
                     "WHERE ProductGifts.ProductId = @ProductId",
                     CommandType.Text,
                     GetOfferFromReader,
                     new SqlParameter("@ProductId", productId));
        }

        public static void AddProductGift(int productId, int giftOfferId)
        {
            if (IsExistProductGift(productId, giftOfferId))
                return;
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO Catalog.ProductGifts (ProductId, GiftOfferId) VALUES (@ProductId, @GiftOfferId)",
                CommandType.Text, new SqlParameter("@ProductId", productId),
                new SqlParameter("@GiftOfferId", giftOfferId));
        }

        public static void DeleteProductGift(int productId, int giftOfferId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Catalog.ProductGifts WHERE ProductId = @ProductId AND GiftOfferId = @GiftOfferId",
                CommandType.Text, new SqlParameter("@ProductId", productId),
                new SqlParameter("@GiftOfferId", giftOfferId));
        }

        public static void ClearProductGifts(int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Catalog.ProductGifts WHERE ProductId = @ProductId",
                CommandType.Text, new SqlParameter("@ProductId", productId));
        }

        public static bool IsExistProductGift(int productId, int giftOfferId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM Catalog.ProductGifts WHERE ProductId = @ProductId AND GiftOfferId = @GiftOfferId",
                CommandType.Text, new SqlParameter("@ProductId", productId),
                new SqlParameter("@GiftOfferId", giftOfferId)) > 0;
        }

        public static List<int> GetProductIdsByGift(int giftOfferId)
        {
            return SQLDataAccess.ExecuteReadColumn<int>(
                "SELECT ProductId FROM Catalog.ProductGifts WHERE GiftOfferId = @GiftOfferId",
                CommandType.Text, "ProductId",
                new SqlParameter("@GiftOfferId", giftOfferId));
        }

        public static string ProductGiftsToString(int productId, string columnSeparator)
        {
            var items = SQLDataAccess.ExecuteReadColumn<string>(
                "Select ArtNo from Catalog.Offer inner join Catalog.ProductGifts on ProductGifts.GiftOfferId = Offer.OfferId where ProductGifts.ProductId = @productId",
                CommandType.Text, "ArtNo", new SqlParameter("@productId", productId));
            return items.AggregateString(columnSeparator);
        }

        public static bool ProductGiftsFromString(int productId, string value, string columnSeparator)
        {
            ClearProductGifts(productId);

            if (string.IsNullOrEmpty(value))
                return true;

            var arrArt = value.Split(new[] { columnSeparator }, StringSplitOptions.None);
            foreach (string t in arrArt)
            {
                var artNo = t.Trim();
                if (string.IsNullOrWhiteSpace(artNo))
                    continue;
                var offer = GetOffer(artNo);
                if (offer != null)
                    AddProductGift(productId, offer.OfferId);
            }
            return true;
        }

        #endregion
    }
}