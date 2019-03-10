//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class PropertyService
    {
        #region Property

        private static Property GetPropertyFromReader(SqlDataReader reader)
        {
            return new Property
            {
                PropertyId = SQLDataHelper.GetInt(reader, "PropertyId"),

                Name = SQLDataHelper.GetString(reader, "Name"),
                NameDisplayed = SQLDataHelper.GetString(reader, "NameDisplayed"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                UseInFilter = SQLDataHelper.GetBoolean(reader, "UseInFilter"),
                UseInDetails = SQLDataHelper.GetBoolean(reader, "UseInDetails"),
                UseInBrief = SQLDataHelper.GetBoolean(reader, "UseInBrief"),

                Expanded = SQLDataHelper.GetBoolean(reader, "Expanded"),

                Description = SQLDataHelper.GetString(reader, "Description"),
                Unit = SQLDataHelper.GetString(reader, "Unit"),
                Type = SQLDataHelper.GetInt(reader, "Type"),
                GroupId = SQLDataHelper.GetNullableInt(reader, "GroupId"),
            };
        }

        /// <summary>
        /// returns property of product by it's ID
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public static Property GetPropertyById(int propertyId)
        {
            return SQLDataAccess.ExecuteReadOne("[Catalog].[sp_GetPropertyByID]", CommandType.StoredProcedure,
                                                    GetPropertyFromReader, new SqlParameter("@PropertyID", propertyId));
        }

        public static Property GetPropertyByName(string name)
        {
            return SQLDataAccess.ExecuteReadOne("Select TOP 1 * From Catalog.Property Where Name=@name", CommandType.Text, GetPropertyFromReader, new SqlParameter("@name", name));
        }


        public static List<Property> GetAllProperties()
        {
            return SQLDataAccess.ExecuteReadList("[Catalog].[sp_GetAllProperties]", CommandType.StoredProcedure,
                                                 GetPropertyFromReader);
        }

        public static int GetProductsCountByProperty(int propId)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_GetCOUNTProductsByProperty]", CommandType.StoredProcedure, new SqlParameter("@PropertyID", propId));
        }

        /// <summary>
        /// add's new property into DB
        /// </summary>
        /// <param name="property"></param>
        public static int AddProperty(Property property)
        {
            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);

            return SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddProperty]", CommandType.StoredProcedure,
                                                    new SqlParameter("@Name", property.Name),
                                                    new SqlParameter("@NameDisplayed", string.IsNullOrEmpty(property.NameDisplayed) ? property.Name : property.NameDisplayed),
                                                    new SqlParameter("@UseInFilter", property.UseInFilter),
                                                    new SqlParameter("@UseInDetails", property.UseInDetails),
                                                    new SqlParameter("@UseInBrief", property.UseInBrief),
                                                    new SqlParameter("@Expanded", property.Expanded),
                                                    new SqlParameter("@SortOrder", property.SortOrder),
                                                    new SqlParameter("@Description", property.Description ?? (object)DBNull.Value),
                                                    new SqlParameter("@Unit", property.Unit ?? (object)DBNull.Value),
                                                    new SqlParameter("@Type", property.Type),
                                                    new SqlParameter("@GroupId", property.GroupId ?? (object)DBNull.Value));
        }

        /// <summary>
        /// Deletes property from DB
        /// </summary>
        /// <param name="propertyId"></param>
        public static void DeleteProperty(int propertyId)
        {
            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);

            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProperty]", CommandType.StoredProcedure, new SqlParameter() { ParameterName = "@PropertyID", Value = propertyId });
        }

        /// <summary>
        /// updates property in DB
        /// </summary>
        /// <param name="property"></param>
        public static void UpdateProperty(Property property)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProperty]", CommandType.StoredProcedure,
                                            new SqlParameter("@PropertyID", property.PropertyId),
                                            new SqlParameter("@Name", property.Name),
                                            new SqlParameter("NameDisplayed", string.IsNullOrEmpty(property.NameDisplayed) ? property.Name : property.NameDisplayed),
                                            new SqlParameter("@UseInFilter", property.UseInFilter),
                                            new SqlParameter("@UseInDetails", property.UseInDetails),
                                            new SqlParameter("@Expanded", property.Expanded),
                                            new SqlParameter("@SortOrder", property.SortOrder),
                                            new SqlParameter("@Description", property.Description ?? (object) DBNull.Value),
                                            new SqlParameter("@Unit", property.Unit ?? (object) DBNull.Value),
                                            new SqlParameter("@Type", property.Type),
                                            new SqlParameter("@GroupId", property.GroupId ?? (object) DBNull.Value),
                                            new SqlParameter("@UseInBrief", property.UseInBrief));

            SQLDataAccess.ExecuteNonQuery("Update [Catalog].[PropertyValue] Set [UseInFilter]=@UseInFilter, [UseInDetails]=@UseInDetails, UseInBrief=@UseInBrief Where PropertyId=@PropertyId ",
                                            CommandType.Text,
                                            new SqlParameter("@PropertyId", property.PropertyId),
                                            new SqlParameter("@UseInFilter", property.UseInFilter),
                                            new SqlParameter("@UseInDetails", property.UseInDetails),
                                            new SqlParameter("@UseInBrief", property.UseInBrief));

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        #endregion

        #region Property Values

        public static Dictionary<int, string> GetPropertiesValuesByNameEndProductId(string text, int productId, int propertyId = 0)
        {

            var command = "Select DISTINCT PropertyValueID, Value From Catalog.PropertyValue WHERE Value like @name + '%' AND PropertyValueID NOT IN (Select PropertyValueID From Catalog.ProductPropertyValue Where ProductID = @productId)";

            var sqlParameters = new List<SqlParameter>(){
             new SqlParameter("@name", text),
             new SqlParameter("@productId", productId)
            };

            if (propertyId != 0)
            {
                command += " AND PropertyID = @PropertyID";

                sqlParameters.Add(new SqlParameter("@PropertyID", propertyId));
            }

            return SQLDataAccess.ExecuteReadDictionary<int, string>(command, CommandType.Text, "PropertyValueID", "Value", sqlParameters.ToArray());
        }

        public static Dictionary<int, string> GetPropertiesByName(string name)
        {
            return SQLDataAccess.ExecuteReadDictionary<int, string>("Select DISTINCT PropertyID, Name From Catalog.Property WHERE Name like @name + '%'",
                                                              CommandType.Text, "PropertyID", "Name", new SqlParameter("@name", name));
        }

        public static bool IsExistPropertyValueInProduct(int productId, int propertyValueId)
        {
            return (int)SQLDataAccess.ExecuteScalar("Select Count(PropertyValueID) From Catalog.ProductPropertyValue Where ProductID = @productId And PropertyValueID = @propertyValueID", CommandType.Text, new SqlParameter("@productId", productId), new SqlParameter("@propertyValueID", propertyValueId)) > 0;
        }

        public static bool IsExistPropertyValueInProduct(int productId, int propertyId, string value)
        {
            return (int)SQLDataAccess.ExecuteScalar("Select Count(PropertyValueID) From Catalog.ProductPropertyValue Where ProductID = @productId And PropertyValueID In (Select PropertyValueID From Catalog.PropertyValue Where PropertyId = @propertyId And Value = @value )",
                CommandType.Text,
                new SqlParameter("@productId", productId),
                new SqlParameter("@propertyId", propertyId),
                new SqlParameter("@value", value)) > 0;
        }

        /// <summary>
        /// returns all values that includes in property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public static List<PropertyValue> GetValuesByPropertyId(int propertyId)
        {
            return SQLDataAccess.ExecuteReadList("[Catalog].[sp_GetPropertyValuesByPropertyID]", CommandType.StoredProcedure,
                                                    GetPropertyValueFromReader, new SqlParameter("@PropertyID", propertyId));
        }

        /// <summary>
        /// returns all values of propepties belonging to product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<PropertyValue> GetPropertyValuesByProductId(int productId)
        {
            return SQLDataAccess.ExecuteReadList("[Catalog].[sp_GetPropertyValuesByProductID]", CommandType.StoredProcedure,
                                                    GetPropertyValueFromReader, new SqlParameter("@ProductID", productId));
        }

        public static PropertyValue GetPropertyValueById(int propertyValueId)
        {
            return SQLDataAccess.ExecuteReadOne("[Catalog].[sp_GetPropertyValueByID]", CommandType.StoredProcedure,
                                                    GetPropertyValueFromReader, new SqlParameter("@PropertyValueId", propertyValueId));
        }

        public static PropertyValue GetPropertyValueByName(int propertyId, string value)
        {
            return SQLDataAccess.ExecuteReadOne("Select Top 1 PropertyValueID, Value From Catalog.PropertyValue Where PropertyID=@propertyID And Value=@value", CommandType.Text,
                                                    reader => new PropertyValue
                                                    {
                                                        PropertyValueId = SQLDataHelper.GetInt(reader, "PropertyValueID"),
                                                        Value = SQLDataHelper.GetString(reader, "Value"),
                                                        PropertyId = propertyId
                                                    }, new SqlParameter("@propertyID", propertyId), new SqlParameter("@value", value));
        }


        private static PropertyValue GetPropertyValueFromReader(SqlDataReader reader)
        {
            return new PropertyValue
            {
                PropertyValueId = SQLDataHelper.GetInt(reader, "PropertyValueID"),
                PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                Value = SQLDataHelper.GetString(reader, "Value"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                Property = new Property
                {
                    GroupId = SQLDataHelper.GetNullableInt(reader, "GroupId"),
                    PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                    Name = SQLDataHelper.GetString(reader, "PropertyName"),
                    NameDisplayed = SQLDataHelper.GetString(reader, "PropertyNameDisplayed"),
                    SortOrder = SQLDataHelper.GetInt(reader, "PropertySortOrder"),
                    Expanded = SQLDataHelper.GetBoolean(reader, "Expanded"),
                    UseInDetails = SQLDataHelper.GetBoolean(reader, "UseInDetails"),
                    UseInFilter = SQLDataHelper.GetBoolean(reader, "UseInFilter"),
                    UseInBrief = SQLDataHelper.GetBoolean(reader, "UseInBrief"),
                    Type = SQLDataHelper.GetInt(reader, "Type"),
                    Description = SQLDataHelper.GetString(reader, "Description"),
                    Unit = SQLDataHelper.GetString(reader, "Unit"),
                    Group =
                        SQLDataHelper.GetNullableInt(reader, "GroupId") != null
                            ? new PropertyGroup()
                            {
                                PropertyGroupId = SQLDataHelper.GetInt(reader, "GroupId"),
                                Name = SQLDataHelper.GetString(reader, "GroupName"),
                                NameDisplayed = SQLDataHelper.GetString(reader, "GroupNameDisplayed"),
                                SortOrder = SQLDataHelper.GetInt(reader, "GroupSortOrder")
                            }
                            : null
                }
            };
        }

        /// <summary>
        /// returns property that include curent value
        /// </summary>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public static Property GetPropertyByValueId(int valueId)
        {
            return SQLDataAccess.ExecuteReadOne("[Catalog].[sp_GetPropertyByValueID]", CommandType.StoredProcedure,
                                                GetPropertyFromReader, new SqlParameter("@PropertyValueId", valueId));
        }


        public static void UpdateOrInsertProductProperty(int productId, string name, string value, int sortOrder)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateOrInsertProductProperty]", CommandType.StoredProcedure,
                                                 new SqlParameter("@ProductID", productId),
                                                 new SqlParameter("@Name", name),
                                                 new SqlParameter("@Value", value),
                                                 new SqlParameter("@RangeValue", value.RemoveChars()),
                                                 new SqlParameter("@SortOrder", sortOrder)
                                                 );
        }

        /// <summary>
        /// adds new value for some property
        /// </summary>
        /// <param name="propVal"></param>
        public static int AddPropertyValue(PropertyValue propVal)
        {
            if (propVal == null)
                throw new ArgumentNullException("propVal");
            if (propVal.PropertyId == 0)
                throw new ArgumentException(@"PropertyId cannot be zero", "propVal");

            var propValId = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddPropertyValue]", CommandType.StoredProcedure,
                                                            new SqlParameter("@Value", propVal.Value),
                                                            new SqlParameter("@PropertyID", propVal.PropertyId),
                                                            new SqlParameter("@SortOrder", propVal.SortOrder),
                                                            new SqlParameter("@RangeValue", propVal.Value.RemoveChars()));

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);

            return propValId;
        }

        /// <summary>
        /// Deletes value from DB
        /// </summary>
        /// <param name="propertyValueId"></param>
        public static void DeletePropertyValueById(int propertyValueId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeletePropertyValue]", CommandType.StoredProcedure, new SqlParameter("@PropertyValueID", propertyValueId));

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        /// <summary>
        /// updates value in DB
        /// </summary>
        /// <param name="value"></param>
        public static void UpdatePropertyValue(PropertyValue value)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdatePropertyValue]", CommandType.StoredProcedure,
                                                 new SqlParameter("@Value", value.Value),
                                                 new SqlParameter("@SortOrder", value.SortOrder),
                                                 new SqlParameter("@PropertyValueId", value.PropertyValueId),
                                                 new SqlParameter("@RangeValue", value.Value.RemoveChars()));

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        /// <summary>
        /// returns all products that includes this value
        /// </summary>
        /// <param name="propVal"></param>
        /// <returns></returns>
        public static List<int> GetProductsIDsByPropertyValue(PropertyValue propVal)
        {
            List<int> productIDs = SQLDataAccess.ExecuteReadList<int>("select ProductID from [Catalog].[ProductPropertyValue] Where PropertyValueID = @ValueID",
                                                                 CommandType.Text,
                                                                 reader => SQLDataHelper.GetInt(reader, "ProductID"),
                                                                 new SqlParameter("@ValueID", propVal.PropertyValueId));
            return productIDs;
        }

        #endregion


        public static IList<PropertyValue> GetPropertyValuesByCategories(int categoryId, bool useDepth)
        {
            return SQLDataAccess.ExecuteReadList<PropertyValue>(
                "[Catalog].[sp_GetPropertyInFilter]",
                CommandType.StoredProcedure,
                reader => new PropertyValue
                {
                    PropertyValueId = SQLDataHelper.GetInt(reader, "PropertyValueID"),
                    PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                    Value = SQLDataHelper.GetString(reader, "Value"),
                    RangeValue = SQLDataHelper.GetFloat(reader, "RangeValue"),
                    Property = new Property
                    {
                        PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                        Name = SQLDataHelper.GetString(reader, "PropertyName"),
                        NameDisplayed = SQLDataHelper.GetString(reader, "PropertyNameDisplayed"),
                        SortOrder = SQLDataHelper.GetInt(reader, "PropertySortOrder"),
                        Expanded = SQLDataHelper.GetBoolean(reader, "PropertyExpanded"),
                        Type = SQLDataHelper.GetInt(reader, "PropertyType"),
                        Unit = SQLDataHelper.GetString(reader, "PropertyUnit"),
						Description = SQLDataHelper.GetString(reader, "PropertyDescription")
                    }
                },
                new SqlParameter("@categoryId", categoryId),
                new SqlParameter("@useDepth", useDepth));
        }

        public static void DeleteProductPropertyValue(int productId, int propertyValueId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProductPropertyValue]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@PropertyValueID", propertyValueId));

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        public static void DeleteProductProperties(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProductProperties]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId));

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        public static void AddProductProperyValue(int propValId, int productId)
        {
            if (propValId == 0)
                throw new ArgumentException(@"Value cannot be zero", "propValId");

            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Catalog].[ProductPropertyValue] ([ProductID],[PropertyValueID]) VALUES (@ProductID,@PropertyValueID)",
                                            CommandType.Text,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@PropertyValueID", propValId)
                                            );

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        public static void UpdateProductPropertyValue(int productId, int oldPropertyValueId, int newPropertyValueId)
        {
            if (oldPropertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "oldPropertyValueId");
            if (newPropertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "newPropertyValueId");

            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProductProperty]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@OldPropertyValueID", oldPropertyValueId),
                                            new SqlParameter("@NewPropertyValueID", newPropertyValueId)
                                            );

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        public static void UpdateProductPropertyValue(int productId, int propertyValueId, string value)
        {
            if (propertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "propertyValueId");
            //I was drunk
            int propertyId = GetPropertyByValueId(propertyValueId).PropertyId;
            DeleteProductPropertyValue(productId, propertyValueId);
            AddProductProperyValue(AddPropertyValue(new PropertyValue { PropertyId = propertyId, Value = value, SortOrder = 0 }), productId);
        }

        public static void ShowInFilter(int propertyId, bool show)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Catalog].[Property] SET UseInFilter = @UseInFilter WHERE [PropertyID] = @PropertyID",
                CommandType.Text, new SqlParameter("@PropertyID", propertyId),
                new SqlParameter("@UseInFilter", show));

            CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);
        }

        public static void ShowInDetails(int propertyId, bool show)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Catalog].[Property] SET UseInDetails = @UseInDetails WHERE [PropertyID] = @PropertyID",
                CommandType.Text, new SqlParameter("@PropertyID", propertyId),
                new SqlParameter("@UseInDetails", show));
        }

        public static void ShowInBrief(int propertyId, bool show)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Catalog].[Property] SET UseInBrief = @UseInBrief WHERE [PropertyID] = @PropertyID",
                CommandType.Text, new SqlParameter("@PropertyID", propertyId),
                new SqlParameter("@UseInBrief", show));
        }

        public static void UpdateGroup(int propertyId, int? groupId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Catalog].[Property] SET GroupId = @GroupId WHERE [PropertyId] = @PropertyId",
                CommandType.Text,
                new SqlParameter("@PropertyId", propertyId),
                new SqlParameter("@GroupId", groupId ?? (object)DBNull.Value));
        }

        public static List<Property> GetPropertyNamesByCompareCart()
        {
            return SQLDataAccess.ExecuteReadList<Property>(
                "select distinct Property.PropertyId, Property.Name, Property.NameDisplayed, Property.UseInFilter, Property.UseInDetails, Property.UseInBrief, Expanded, Description, Unit, Type, GroupId, Property.SortOrder " +
                "from catalog.Shoppingcart inner join catalog.Offer on Shoppingcart.OfferID = Offer.OfferId and Shoppingcart.ShoppingCartType = 3" +
                "inner join catalog.ProductPropertyValue on Offer.ProductID = ProductPropertyValue.Productid " +
                "inner join catalog.PropertyValue on PropertyValue.PropertyValueID = ProductPropertyValue.PropertyValueID " +
                "inner join catalog.Property on Property.PropertyID = PropertyValue.PropertyID " +
                "where CustomerId=@CustomerId " +
                "order by Property.SortOrder",
                CommandType.Text, GetPropertyFromReader,
                new SqlParameter("@CustomerId", Customers.CustomerContext.CustomerId));
        }

        public static string PropertiesToString(List<PropertyValue> productPropertyValues, string columSeparator, string propertySeparator)
        {
            var res = new StringBuilder();
            for (int i = 0; i < productPropertyValues.Count; i++)
            {
                if (i == 0)
                    res.Append(productPropertyValues[i].Property.Name + propertySeparator + productPropertyValues[i].Value);
                else res.Append(columSeparator + productPropertyValues[i].Property.Name + propertySeparator + productPropertyValues[i].Value);
            }
            return res.ToString();
        }

        public static void PropertiesFromString(int productId, string properties, string columSeparator, string propertySeparator)
        {
            if (string.IsNullOrWhiteSpace(columSeparator) || string.IsNullOrWhiteSpace(propertySeparator))
                _PropertiesFromString(productId, properties);
            else
                _PropertiesFromString(productId, properties, columSeparator, propertySeparator);
        }

        private static void _PropertiesFromString(int productId, string properties)
        {
            try
            {
                DeleteProductProperties(productId);
                if (string.IsNullOrEmpty(properties)) return;
                //type:value,type:value,...
                var items = properties.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var stepSort = 0;
                foreach (string s in items)
                {
                    var temp = s.Trim().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length != 2)
                        continue;
                    var tempType = temp[0].Trim();
                    var tempValue = temp[1].Trim();
                    if (!string.IsNullOrWhiteSpace(tempType) && !string.IsNullOrWhiteSpace(tempValue))
                    {
                        // inside stored procedure not thread save/ do save mode by logic 
                        SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_ParseProductProperty]", CommandType.StoredProcedure,
                                                      new SqlParameter("@nameProperty", tempType),
                                                      new SqlParameter("@propertyValue", tempValue),
                                                      new SqlParameter("@rangeValue", tempValue.TryParseFloat()),
                                                      new SqlParameter("@productId", productId),
                                                      new SqlParameter("@sort", stepSort));
                        stepSort += 10;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private static void _PropertiesFromString(int productId, string properties, string columSeparator, string propertySeparator)
        {
            try
            {
                DeleteProductProperties(productId);
                if (string.IsNullOrEmpty(properties)) return;
                //type:value,type:value,...
                var items = properties.Split(new[] { columSeparator }, StringSplitOptions.RemoveEmptyEntries);
                var stepSort = 0;
                foreach (string s in items)
                {
                    var temp = s.Trim().Split(new[] { propertySeparator }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length != 2)
                        continue;
                    var tempType = temp[0].Trim();
                    var tempValue = temp[1].Trim();
                    if (!string.IsNullOrWhiteSpace(tempType) && !string.IsNullOrWhiteSpace(tempValue))
                    {
                        // inside stored procedure not thread save/ do save mode by logic 
                        SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_ParseProductProperty]", CommandType.StoredProcedure,
                                                      new SqlParameter("@nameProperty", tempType),
                                                      new SqlParameter("@propertyValue", tempValue),
                                                      new SqlParameter("@rangeValue", tempValue.TryParseFloat()),
                                                      new SqlParameter("@productId", productId),
                                                      new SqlParameter("@sort", stepSort));
                        stepSort += 10;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

    }
}