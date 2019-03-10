//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Payment
{
    public enum PageWithPaymentButton
    {
        myaccount,
        orderconfirmation
    }

    public class PaymentService
    {
        private const string PaymentCacheKey = "PaymentMethods_";
        private const string PaymentCreditCacheKey = PaymentCacheKey + "Credit";

        public static List<PaymentMethod> GetAllPaymentMethods(bool onlyEnabled)
        {
            var cacheKey = PaymentCacheKey + (onlyEnabled ? "Active" : "All");

            return CacheManager.Get(cacheKey, () => SQLDataAccess.ExecuteReadList(
                onlyEnabled
                    ? "SELECT * FROM [Order].[PaymentMethod] left join Catalog.Photo on Photo.ObjId=PaymentMethod.PaymentMethodID and Type=@Type where Enabled=1 ORDER BY [SortOrder]"
                    : "SELECT * FROM [Order].[PaymentMethod] left join Catalog.Photo on Photo.ObjId=PaymentMethod.PaymentMethodID and Type=@Type ORDER BY [SortOrder]",
                CommandType.Text,
                reader => GetPaymentMethodFromReader(reader, true),
                new SqlParameter("@Type", PhotoType.Payment.ToString())));
        }

        public static IEnumerable<int> GetAllPaymentMethodIDs()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("SELECT [PaymentMethodID] FROM [Order].[PaymentMethod]", CommandType.Text, "PaymentMethodID");
        }

        public static PaymentMethod GetPaymentMethod(int paymentMethodId)
        {
            var payment =
                SQLDataAccess.ExecuteReadOne(
                    "SELECT * FROM [Order].[PaymentMethod] WHERE [PaymentMethodID] = @PaymentMethodID", CommandType.Text,
                    reader => GetPaymentMethodFromReader(reader),
                    new SqlParameter("@PaymentMethodID", paymentMethodId));

            return payment;
        }

        public static PaymentMethod GetPaymentMethodByName(string name)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT top(1) * FROM [Order].[PaymentMethod] WHERE [Name] = @Name",
                CommandType.Text, reader => GetPaymentMethodFromReader(reader), new SqlParameter("@Name", name));
        }

        public static PaymentMethodAdminModel GetPaymentMethodAdminModel(int methodId)
        {
            var method = GetPaymentMethod(methodId);
            if (method == null)
                return null;

            var type = GetPaymentMethodAdminModelType(method.PaymentKey);

            var model = (PaymentMethodAdminModel)Activator.CreateInstance(type);

            model.PaymentMethodId = method.PaymentMethodId;
            model.PaymentKey = method.PaymentKey;
            model.Name = method.Name;
            model.Description = method.Description;
            model.Enabled = method.Enabled;
            model.SortOrder = method.SortOrder;
            model.Extracharge = method.Extracharge;
            model.ExtrachargeType = method.ExtrachargeType;
            model.CurrencyId = method.CurrencyId;

            var icon = method.IconFileName;
            model.Icon = icon != null ? FoldersHelper.GetPath(FolderType.PaymentLogo, icon.PhotoName, false) : "";
            model.Parameters = method.Parameters;

            model.ProcessType = method.ProcessType;
            model.NotificationType = method.NotificationType;
            model.ShowUrls = method.ShowUrls;
            model.SuccessUrl = method.SuccessUrl;
            model.CancelUrl = method.CancelUrl;
            model.FailUrl = method.FailUrl;
            model.NotificationUrl = method.NotificationUrl;

            model.ShowCurrency = !(method is IPaymentCurrencyHide);

            return model;
        }

        private static Type GetPaymentMethodAdminModelType(string paymentType)
        {
            paymentType = paymentType.ToLower();

            return CacheManager.Get("GetPaymentMethodAdminModelType_" + paymentType, 2, () =>
            {
                var derivedType = typeof(PaymentMethodAdminModel);

                try
                {
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.GlobalAssemblyCache && x.FullName.StartsWith("AdvantShop")))
                    {
                        var type = assembly.GetTypes().FirstOrDefault(t => t.Name.ToLower().Replace("paymentmodel", "") == paymentType && derivedType.IsAssignableFrom(t));
                        if (type != null)
                            return type;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error("PaymentService.GetPaymentMethodAdminModelType: Can't find type " + paymentType, ex);
                }

                return derivedType;
            });
        }


        public static List<ICreditPaymentMethod> GetCreditPaymentMethods()
        {
            if (CacheManager.Contains(PaymentCreditCacheKey))
                return CacheManager.Get<List<ICreditPaymentMethod>>(PaymentCreditCacheKey);
            var list = GetAllPaymentMethods(true).OfType<ICreditPaymentMethod>().ToList();
            CacheManager.Insert(PaymentCreditCacheKey, list);
            return list;
        }

        public static PaymentMethod GetPaymentMethodFromReader(SqlDataReader reader, bool loadPic = false)
        {
            var pay = PaymentMethod.Create(SQLDataHelper.GetString(reader, "PaymentType"));
            if (pay == null)
                return null;

            pay.PaymentMethodId = SQLDataHelper.GetInt(reader, "PaymentMethodID");
            pay.Name = SQLDataHelper.GetString(reader, "Name");
            pay.Enabled = SQLDataHelper.GetBoolean(reader, "Enabled");
            pay.Description = SQLDataHelper.GetString(reader, "Description");
            pay.SortOrder = SQLDataHelper.GetInt(reader, "SortOrder");
            pay.ExtrachargeType = (ExtrachargeType)SQLDataHelper.GetInt(reader, "ExtrachargeType");
            pay.Extracharge = SQLDataHelper.GetFloat(reader, "Extracharge");
            pay.IconFileName = loadPic
                ? new Photo(SQLDataHelper.GetInt(reader, "PhotoId"), SQLDataHelper.GetInt(reader, "ObjId"),
                    PhotoType.Payment) { PhotoName = SQLDataHelper.GetString(reader, "PhotoName") }
                : null;
            pay.CurrencyId = SQLDataHelper.GetInt(reader, "CurrencyId");

            pay.Parameters = GetPaymentMethodParameters(pay.PaymentMethodId);

            return pay;
        }

        public static Dictionary<string, string> GetPaymentMethodParameters(int paymentMethodId)
        {
            return
                SQLDataAccess.ExecuteReadDictionary<string, string>(
                    "SELECT Name, Value FROM [Order].[PaymentParam] WHERE [PaymentMethodID] = @PaymentMethodID",
                    CommandType.Text, "Name", "Value", new SqlParameter("@PaymentMethodID", paymentMethodId));
        }

        public static int AddPaymentMethod(PaymentMethod method)
        {
            var id = SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO [Order].[PaymentMethod] ([PaymentType],[Name], [Enabled], [Description], [SortOrder],Extracharge,ExtrachargeType,CurrencyId) " +
                "VALUES (@PaymentType,@Name, @Enabled, @Description, @SortOrder,@Extracharge,@ExtrachargeType,@CurrencyId); SELECT scope_identity();",
                CommandType.Text,
                new SqlParameter("@PaymentType", method.PaymentKey),
                new SqlParameter("@Name", method.Name ?? string.Empty),
                new SqlParameter("@Enabled", method.Enabled),
                new SqlParameter("@Description", method.Description ?? string.Empty),
                new SqlParameter("@SortOrder", method.SortOrder),
                new SqlParameter("@Extracharge", method.Extracharge),
                new SqlParameter("@ExtrachargeType", (int)method.ExtrachargeType),
                new SqlParameter("@CurrencyId", method.CurrencyId));

            AddPaymentMethodParameters(id, method.Parameters);
            CacheManager.RemoveByPattern(PaymentCacheKey);

            return id;
        }

        private static void AddPaymentMethodParameters(int paymentMethodId, Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters.Where(parameter => parameter.Value.IsNotEmpty()))
            {
                SQLDataAccess.ExecuteNonQuery(
                    "INSERT INTO [Order].[PaymentParam] (PaymentMethodID, Name, Value) VALUES (@PaymentMethodID, @Name, @Value)",
                    CommandType.Text,
                    new SqlParameter("@PaymentMethodID", paymentMethodId),
                    new SqlParameter("@Name", parameter.Key),
                    new SqlParameter("@Value", parameter.Value));
            }
        }

        public static void UpdatePaymentMethod(PaymentMethod paymentMethod, bool updateParams = true)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[PaymentMethod] SET [Name] = @Name,[Enabled] = @Enabled,[SortOrder] = @SortOrder,[Description] = @Description,[PaymentType] = @PaymentType, " +
                "Extracharge=@Extracharge, ExtrachargeType=@ExtrachargeType, CurrencyId=@CurrencyId WHERE [PaymentMethodID] = @PaymentMethodID",
                CommandType.Text,
                new SqlParameter("@PaymentMethodID", paymentMethod.PaymentMethodId),
                new SqlParameter("@Name", paymentMethod.Name),
                new SqlParameter("@Enabled", paymentMethod.Enabled),
                new SqlParameter("@SortOrder", paymentMethod.SortOrder),
                new SqlParameter("@Description", paymentMethod.Description),
                new SqlParameter("@PaymentType", paymentMethod.PaymentKey),
                new SqlParameter("@Extracharge", paymentMethod.Extracharge),
                new SqlParameter("@ExtrachargeType", (int)paymentMethod.ExtrachargeType),
                new SqlParameter("@CurrencyId", paymentMethod.CurrencyId));

            if (updateParams)
                UpdatePaymentParams(paymentMethod.PaymentMethodId, paymentMethod.Parameters);

            CacheManager.RemoveByPattern(CacheNames.PaymentOptions);
        }

        public static void UpdatePaymentParams(int paymentMethodId, Dictionary<string, string> parameters)
        {
            foreach (var kvp in parameters)
            {
                SQLDataAccess.ExecuteNonQuery("[Order].[sp_UpdatePaymentParam]", CommandType.StoredProcedure,
                    new SqlParameter("@PaymentMethodID", paymentMethodId),
                    new SqlParameter("@Name", kvp.Key),
                    new SqlParameter("@Value", kvp.Value ?? ""));
            }
            CacheManager.RemoveByPattern(PaymentCacheKey);
        }
        
        public static void DeletePaymentMethod(int paymentMethodId)
        {
            PhotoService.DeletePhotos(paymentMethodId, PhotoType.Payment);
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Order].[PaymentMethod] WHERE [PaymentMethodID] = @PaymentMethodID", CommandType.Text, new SqlParameter("@PaymentMethodID", paymentMethodId));
            CacheManager.RemoveByPattern(PaymentCacheKey);
        }

        public static List<PaymentMethod> GetCertificatePaymentMethods()
        {
            return SQLDataAccess.ExecuteReadList(
                "SELECT * FROM [Settings].[GiftCertificatePayments] INNER JOIN [Order].[PaymentMethod] ON [PaymentMethod].[PaymentMethodID] = [GiftCertificatePayments].[PaymentID] WHERE [Enabled] = 1 ORDER BY [SortOrder]",
                CommandType.Text, reader => GetPaymentMethodFromReader(reader));
        }

        public static void SaveOrderpaymentInfo(int orderId, int paymentId, string name, string value)
        {
            SQLDataAccess.ExecuteNonQuery(
                    "INSERT INTO [Order].[OrderPaymentInfo] (OrderID, PaymentMethodID, Name, Value) VALUES (@OrderID, @PaymentMethodID, @Name, @Value)", // DELETE FROM [Order].[OrderPaymentInfo] WHERE OrderID=@OrderID and PaymentMethodID=@PaymentMethodID and Name=@Name;
                    CommandType.Text,
                    new SqlParameter("@OrderID", orderId),
                    new SqlParameter("@PaymentMethodID", paymentId),
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Value", value));
        }

        public static PaymentAdditionalInfo GetOrderIdByPaymentIdAndCode(int paymentMethodId, string responseCode)
        {
            return SQLDataAccess.ExecuteReadOne(
                "Select * From [Order].[OrderPaymentInfo] Where PaymentMethodID = @PaymentMethodID AND Value = @Code",
                CommandType.Text,
                reader => new PaymentAdditionalInfo
                {
                    PaymentMethodId = SQLDataHelper.GetInt(reader, "PaymentMethodID"),
                    OrderId = SQLDataHelper.GetInt(reader, "OrderID"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    Value = SQLDataHelper.GetString(reader, "Value")
                },
                new SqlParameter("@PaymentMethodID", paymentMethodId),
                new SqlParameter("@Code", responseCode));
        }


        public static List<PaymentMethod> UseGeoMapping(List<PaymentMethod> listMethods, string country, string city)
        {
            var items = new List<PaymentMethod>();
            foreach (var elem in listMethods)
            {
                if (ShippingPaymentGeoMaping.IsExistGeoPayment(elem.PaymentMethodId))
                {
                    if (ShippingPaymentGeoMaping.CheckPaymentEnabledGeo(elem.PaymentMethodId, country, city))
                        items.Add(elem);
                }
                else
                {
                    items.Add(elem);
                }
            }
            return items;
        }
    }
}