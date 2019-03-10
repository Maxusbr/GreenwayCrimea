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
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Payment;
using AdvantShop.Taxes;

namespace AdvantShop.Shipping
{
    public class ShippingMethodService
    {
        public static ShippingMethod GetShippingMethodFromReader(SqlDataReader reader, bool loadPic = false)
        {
            var model = new ShippingMethod
            {
                ShippingMethodId = SQLDataHelper.GetInt(reader, "ShippingMethodID"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                ShippingType = SQLDataHelper.GetString(reader, "ShippingType"),
                Description = SQLDataHelper.GetString(reader, "Description"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                DisplayCustomFields = SQLDataHelper.GetBoolean(reader, "DisplayCustomFields"),
                DisplayIndex = SQLDataHelper.GetBoolean(reader, "DisplayIndex"),
                ShowInDetails = SQLDataHelper.GetBoolean(reader, "ShowInDetails"),
                IconFileName = loadPic
                    ? new Photo(SQLDataHelper.GetInt(reader, "PhotoId"), SQLDataHelper.GetInt(reader, "ObjId"),
                        PhotoType.Shipping) { PhotoName = SQLDataHelper.GetString(reader, "PhotoName") }
                    : null,
                ZeroPriceMessage = SQLDataHelper.GetString(reader, "ZeroPriceMessage"),
                TaxType = (TaxType)SQLDataHelper.GetInt(reader, "TaxType"),
            };
            model.Params = GetShippingParams(model.ShippingMethodId);
            return model;
        }

        /// <summary>
        /// return shipping service by his id
        /// </summary>
        /// <param name="shippingMethodId"></param>
        /// <returns>ShippingMethod</returns>
        public static DataTable GetShippingPayments(int shippingMethodId)
        {
            return
                SQLDataAccess.ExecuteTable(
                    "SELECT [PaymentMethod].[PaymentMethodID], [PaymentMethod].[Name], (Select Count(PaymentMethodID) From [Order].[ShippingPayments] Where PaymentMethodID = [PaymentMethod].[PaymentMethodID] AND ShippingMethodID = @ShippingMethodID) as [Use] FROM [Order].[PaymentMethod]",
                    CommandType.Text,
                    new SqlParameter("@ShippingMethodID", shippingMethodId));
        }

        public static List<PaymentShippingAdminModel> GetPayments(int shippingMethodId)
        {
            return
                SQLDataAccess.Query<PaymentShippingAdminModel>(
                    "SELECT [PaymentMethod].[PaymentMethodID], [PaymentMethod].[Name], " +
                    "(Select Count(PaymentMethodID) From [Order].[ShippingPayments] Where PaymentMethodID = [PaymentMethod].[PaymentMethodID] AND ShippingMethodID = @id) as [MethodsCount] " +
                    "FROM [Order].[PaymentMethod]", new {id = shippingMethodId})
                    .ToList();
        }

        /// <summary>
        /// return shipping service by his id
        /// </summary>
        /// <param name="shippingMethodId"></param>
        /// <returns>ShippingMethod</returns>
        public static ShippingMethod GetShippingMethod(int shippingMethodId)
        {
            return
                SQLDataAccess.ExecuteReadOne(
                    "SELECT * FROM [Order].[ShippingMethod] WHERE ShippingMethodID = @ShippingMethodID",
                    CommandType.Text,
                    reader => GetShippingMethodFromReader(reader),
                    new SqlParameter("@ShippingMethodID", shippingMethodId));
        }

        public static ShippingMethodAdminModel GetShippingMethodAdminModel(int methodId)
        {
            var method = GetShippingMethod(methodId);
            if (method == null)
                return null;

            var type = GetShippingMethodAdminModelType(method.ShippingType);

            var model = (ShippingMethodAdminModel) Activator.CreateInstance(type);

            model.ShippingMethodId = method.ShippingMethodId;
            model.ShippingType = method.ShippingType;
            model.Name = method.Name;
            model.Description = method.Description;
            model.Enabled = method.Enabled;
            model.SortOrder = method.SortOrder;
            model.ShowInDetails = method.ShowInDetails;
            model.DisplayCustomFields = method.DisplayCustomFields;
            model.DisplayIndex = method.DisplayIndex;
            model.ZeroPriceMessage = method.ZeroPriceMessage;
            model.TaxType = method.TaxType;

            var icon = method.IconFileName;
            model.Icon = icon != null ? FoldersHelper.GetPath(FolderType.ShippingLogo, icon.PhotoName, false) : "";
            model.Params = method.Params;

            return model;
        }

        private static Type GetShippingMethodAdminModelType(string shippingType)
        {
            var derivedType = typeof(ShippingMethodAdminModel);

            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.GlobalAssemblyCache && x.FullName.StartsWith("AdvantShop")))
                {
                    var type = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains(shippingType) && derivedType.IsAssignableFrom(t));
                    if (type != null)
                        return type;
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return derivedType;
        }


        public static bool IsPaymentNotUsed(int shippingMethodId, int paymentMethodId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT Count(PaymentMethodID) FROM [Order].[ShippingPayments] WHERE ShippingMethodID = @ShippingMethodID AND PaymentMethodID = @PaymentMethodID",
                CommandType.Text, new SqlParameter("@ShippingMethodID", shippingMethodId),
                new SqlParameter("@PaymentMethodID", paymentMethodId)) > 0;
        }
        public static List<int> NotAvailablePayments(int shippingMethodId)
        {
            return SQLDataAccess.ExecuteReadList<int>(
                "SELECT PaymentMethodID FROM [Order].[ShippingPayments] WHERE ShippingMethodID = @ShippingMethodID",
                CommandType.Text, reader => SQLDataHelper.GetInt(reader, "PaymentMethodID"), new SqlParameter("@ShippingMethodID", shippingMethodId));
        }

        public static ShippingMethod GetShippingMethodByName(string name)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Order].[ShippingMethod] WHERE Name = @Name",
                CommandType.Text,
                reader => GetShippingMethodFromReader(reader),
                new SqlParameter("@Name", name));
        }

        /// <summary>
        /// get all enabled shipping services
        /// </summary>
        /// <returns>List of ShippingMethod</returns>
        public static List<ShippingMethod> GetAllShippingMethods(bool enabled)
        {
            return
                SQLDataAccess.ExecuteReadList(
                    "SELECT * FROM [Order].[ShippingMethod] " +
                    "Left Join Catalog.Photo on Photo.objId=ShippingMethod.ShippingMethodID and Type=@Type " +
                    "WHERE Enabled = @Enabled " +
                    "Order by sortOrder",
                    CommandType.Text, 
                    reader => GetShippingMethodFromReader(reader, true),
                    new SqlParameter("@Enabled", enabled), 
                    new SqlParameter("@Type", PhotoType.Shipping.ToString()));
        }

        /// <summary>
        /// get all enabled shipping services
        /// </summary>
        /// <returns>List of ShippingMethod</returns>
        public static List<ShippingMethod> GetAllShippingMethods()
        {
            return
                SQLDataAccess.ExecuteReadList(
                    "SELECT * FROM [Order].[ShippingMethod] " +
                    "Left Join Catalog.Photo on Photo.objId=ShippingMethod.ShippingMethodID and Type=@Type " +
                    "Order by sortOrder",
                    CommandType.Text, 
                    reader => GetShippingMethodFromReader(reader, true),
                    new SqlParameter("@Type", PhotoType.Shipping.ToString()));
        }

        public static IEnumerable<int> GetAllShippingMethodIds()
        {
            return SQLDataAccess.ExecuteReadColumn<int>("SELECT ShippingMethodID  FROM [Order].[ShippingMethod]", CommandType.Text, "ShippingMethodID");
        }

        public static int InsertShippingMethod(ShippingMethod item)
        {
            item.ShippingMethodId =
                SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO [Order].[ShippingMethod] ([ShippingType],[Name],[Description],[Enabled],[SortOrder],[DisplayCustomFields],[DisplayIndex],[ShowInDetails], ZeroPriceMessage, TaxType) " +
                    "VALUES (@ShippingType,@Name,@Description,@Enabled,@SortOrder,@DisplayCustomFields,@DisplayIndex,@ShowInDetails, @ZeroPriceMessage, @TaxType); SELECT scope_identity();",
                    CommandType.Text,
                    new SqlParameter("@ShippingType", item.ShippingType),
                    new SqlParameter("@Name", item.Name),
                    new SqlParameter("@Description", item.Description),
                    new SqlParameter("@Enabled", item.Enabled),
                    new SqlParameter("@SortOrder", item.SortOrder),
                    new SqlParameter("@DisplayCustomFields", item.DisplayCustomFields),
                    new SqlParameter("@DisplayIndex", item.DisplayIndex),
                    new SqlParameter("@ShowInDetails", item.ShowInDetails),
                    new SqlParameter("@ZeroPriceMessage", item.ZeroPriceMessage ?? ""),
                    new SqlParameter("@TaxType", (int)item.TaxType));

            InsertShippingParams(item.ShippingMethodId, item.Params);

            RemoveCache(item.ShippingMethodId);

            return item.ShippingMethodId;
        }

        public static bool UpdateShippingPayments(int shippingMethodId, List<int> payments)
        {
            var deleteCmd = "Delete From [Order].[ShippingPayments] Where [ShippingMethodID] = @shippingMethodId;";
            var insertCmd = payments.Aggregate(string.Empty,
                (current, paymentId) => current + string.Format("INSERT INTO [Order].[ShippingPayments] ([ShippingMethodID], [PaymentMethodID]) VALUES ({0}, {1});", shippingMethodId, paymentId));
            SQLDataAccess.ExecuteNonQuery(deleteCmd + insertCmd, CommandType.Text, new SqlParameter("shippingMethodId", shippingMethodId));

            RemoveCache(shippingMethodId);

            return true;
        }

        public static bool UpdateShippingMethod(ShippingMethod item, bool updateParams = true)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[ShippingMethod] SET [ShippingType] = @ShippingType,[Name] = @Name,[Description] = @Description,[Enabled] = @Enabled,[SortOrder] = @SortOrder, DisplayCustomFields=@DisplayCustomFields, DisplayIndex=@DisplayIndex, ShowInDetails=@ShowInDetails, ZeroPriceMessage=@ZeroPriceMessage, TaxType=@TaxType WHERE ShippingMethodID=@ShippingMethodID",
                CommandType.Text,
                new SqlParameter("@ShippingType", item.ShippingType),
                new SqlParameter("@Name", item.Name),
                new SqlParameter("@Description", item.Description),
                new SqlParameter("@Enabled", item.Enabled),
                new SqlParameter("@SortOrder", item.SortOrder),
                new SqlParameter("@ShippingMethodID", item.ShippingMethodId),
                new SqlParameter("@DisplayCustomFields", item.DisplayCustomFields),
                new SqlParameter("@DisplayIndex", item.DisplayIndex),
                new SqlParameter("@ShowInDetails", item.ShowInDetails),
                new SqlParameter("@ZeroPriceMessage", item.ZeroPriceMessage ?? ""),
                new SqlParameter("@TaxType", (int) item.TaxType)
            );

            if (updateParams)
                UpdateShippingParams(item.ShippingMethodId, item.Params);

            RemoveCache(item.ShippingMethodId);

            return true;
        }

        public static void DeleteShippingMethod(int shippingId)
        {
            PhotoService.DeletePhotos(shippingId, PhotoType.Shipping);
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Order].[ShippingMethod] WHERE ShippingMethodID = @shippingId",
                CommandType.Text, new SqlParameter("@shippingId", shippingId));

            RemoveCache(shippingId);
        }

        /// <summary>
        /// gets list of shippingMethod by type and enabled
        /// </summary>
        /// <param name="type"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static List<ShippingMethod> GetShippingMethodByType(string type, bool active = true)
        {
            return
                SQLDataAccess.ExecuteReadList(
                    "SELECT * FROM [Order].[ShippingMethod] WHERE ShippingType = @ShippingType" + (active ? " and enabled=1" : ""),
                    CommandType.Text, reader => GetShippingMethodFromReader(reader),
                    new SqlParameter("@ShippingType", type));
        }

        public static Dictionary<string, string> GetShippingParams(int shippingMethodId)
        {
            return
                SQLDataAccess.ExecuteReadDictionary<string, string>(
                    "SELECT ParamName,ParamValue FROM [Order].[ShippingParam] WHERE ShippingMethodID = @ShippingMethodID",
                    CommandType.Text, "ParamName", "ParamValue", new SqlParameter("@ShippingMethodID", shippingMethodId));
        }

        public static void InsertShippingParams(int shippingMethodId, Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                SQLDataAccess.ExecuteNonQuery(
                    "INSERT INTO [Order].[ShippingParam] ([ShippingMethodID],[ParamName],[ParamValue]) VALUES (@ShippingMethodID,@ParamName,@ParamValue)",
                    CommandType.Text,
                    new SqlParameter("@ShippingMethodID", shippingMethodId),
                    new SqlParameter("@ParamName", parameter.Key),
                    new SqlParameter("@ParamValue", parameter.Value));
            }
        }

        public static bool UpdateShippingParams(int shippingMethodId, Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                SQLDataAccess.ExecuteNonQuery(
                    @"if (SELECT COUNT(*) FROM [Order].[ShippingParam] WHERE [ShippingMethodID] = @ShippingMethodID AND [ParamName] = @ParamName) = 0
		                INSERT INTO [Order].[ShippingParam] ([ShippingMethodID], [ParamName], [ParamValue]) VALUES (@ShippingMethodID, @ParamName, @ParamValue)
	                else
		                UPDATE [Order].[ShippingParam] SET [ParamValue] = @ParamValue WHERE [ShippingMethodID] = @ShippingMethodID AND [ParamName] = @ParamName",
                    CommandType.Text,
                    new SqlParameter("@ShippingMethodID", shippingMethodId),
                    new SqlParameter("@ParamName", parameter.Key),
                    new SqlParameter("@ParamValue", parameter.Value));
            }
            return true;
        }

        public static void RemoveCache(int shippingMethodId)
        {
            ShippingCacheRepositiry.Delete(shippingMethodId);

            CacheManager.RemoveByPattern(CacheNames.ShippingOptions);
            CacheManager.RemoveByPattern(CacheNames.PaymentOptions);
        }
    }
}