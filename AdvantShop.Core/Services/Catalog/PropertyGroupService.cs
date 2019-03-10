using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class PropertyGroupService
    {
        public static List<PropertyGroupView> GetGroupsByCategory(int categoryId)
        {
            return SQLDataAccess.ExecuteReadList(
               @"SELECT [PropertyGroup].[PropertyGroupId],[PropertyGroup].[GroupName], [PropertyId], [Property].[Name] as PropertyName, Type
                FROM [Catalog].[PropertyGroup] 
                Join [Catalog].[PropertyGroupCategory] On [PropertyGroup].[PropertyGroupId] = [PropertyGroupCategory].[PropertyGroupId] 
                Left Join [Catalog].[Property] On [Property].[GroupId] = [PropertyGroup].[PropertyGroupId] 
                Where [PropertyGroupCategory].[CategoryId] = @CategoryId 
                Order By PropertyGroup.GroupSortOrder, Property.SortOrder",
                CommandType.Text,
                reader => new PropertyGroupView()
                {
                    PropertyId = SQLDataHelper.GetInt(reader, "PropertyId"),
                    PropertyGroupId = SQLDataHelper.GetInt(reader, "PropertyGroupId"),
                    GroupName = SQLDataHelper.GetString(reader, "GroupName"),
                    PropertyName = SQLDataHelper.GetString(reader, "PropertyName"),
                    Type = SQLDataHelper.GetInt(reader, "Type"),
                },
                new SqlParameter("@CategoryId", categoryId));
        }


        public static List<PropertyGroupView> GetGroupsByProduct(int productID)
        {
            return SQLDataAccess.ExecuteReadList(
               @"SELECT [PropertyGroup].[PropertyGroupId]
	            ,[PropertyGroup].[GroupName] AS GroupName
	            ,Property.[PropertyId]
	            ,[Property].[Name] AS PropertyName
	            ,Type
                FROM [catalog].[ProductPropertyValue]
                    INNER JOIN [Catalog].[PropertyValue] on [ProductPropertyValue].PropertyValueID = [PropertyValue].PropertyValueID
                    INNER JOIN [Catalog].[Property] ON [Property].[PropertyID] = [PropertyValue].[PropertyID]
                    LEFT JOIN [Catalog].[PropertyGroup] on [PropertyGroup].PropertyGroupId = [Property].GroupId
                WHERE [ProductPropertyValue].ProductID = productID
                ORDER BY case when PropertyGroup.GroupSortOrder is null then 1 else 0 end, PropertyGroup.GroupSortOrder, Property.SortOrder",
                CommandType.Text,
                reader => new PropertyGroupView()
                {
                    PropertyId = SQLDataHelper.GetInt(reader, "PropertyId"),
                    PropertyGroupId = SQLDataHelper.GetInt(reader, "PropertyGroupId"),
                    GroupName = SQLDataHelper.GetString(reader, "GroupName"),
                    PropertyName = SQLDataHelper.GetString(reader, "PropertyName"),
                    Type = SQLDataHelper.GetInt(reader, "Type"),
                },
                new SqlParameter("@productID", productID));
        }
        



        #region Get/Add/Update/Delete group

        private static PropertyGroup GetPropertyGroupFromReader(SqlDataReader reader)
        {
            return new PropertyGroup
            {
                PropertyGroupId = SQLDataHelper.GetInt(reader, "PropertyGroupId"),
                Name = SQLDataHelper.GetString(reader, "GroupName"),
                NameDisplayed = SQLDataHelper.GetString(reader, "GroupNameDisplayed"),
                SortOrder = SQLDataHelper.GetInt(reader, "GroupSortOrder")
            };
        }

        public static PropertyGroup Get(int propertyGroupId)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Catalog].[PropertyGroup] WHERE PropertyGroupId = @PropertyGroupId",
                                                   CommandType.Text, GetPropertyGroupFromReader, new SqlParameter("@PropertyGroupId", propertyGroupId));
        }

        public static PropertyGroup Get(string propertyGroupName)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Catalog].[PropertyGroup] WHERE GroupName = @GroupName",
                                                   CommandType.Text, GetPropertyGroupFromReader, new SqlParameter("@GroupName", propertyGroupName));
        }

        public static List<PropertyGroup> GetList()
        {
            return SQLDataAccess.ExecuteReadList("SELECT * FROM [Catalog].[PropertyGroup] Order By GroupSortOrder, GroupName", CommandType.Text, GetPropertyGroupFromReader);
        }

        public static List<PropertyGroup> GetListByCategory(int categoryId)
        {
            return SQLDataAccess.ExecuteReadList(
                "SELECT [PropertyGroup].[PropertyGroupId],[GroupName],[GroupSortOrder] FROM [Catalog].[PropertyGroup] " +
                "Join [Catalog].[PropertyGroupCategory] On [PropertyGroup].[PropertyGroupId] = [PropertyGroupCategory].[PropertyGroupId] " +
                "Where [PropertyGroupCategory].[CategoryId] = @CategoryId " +
                "Order By GroupSortOrder",
                CommandType.Text,
                reader => new PropertyGroup()
                {
                    PropertyGroupId = SQLDataHelper.GetInt(reader, "PropertyGroupId"),
                    Name = SQLDataHelper.GetString(reader, "GroupName"),
                    SortOrder = SQLDataHelper.GetInt(reader, "GroupSortOrder"),
                }, 
                new SqlParameter("@categoryId", categoryId));
        }


        public static int Add(PropertyGroup propertyGroup)
        {
            propertyGroup.PropertyGroupId =
                SQLDataHelper.GetInt(
                    SQLDataAccess.ExecuteScalar(
                        "Insert Into Catalog.PropertyGroup (GroupName, GroupNameDisplayed, GroupSortOrder) Values (@GroupName, @GroupNameDisplayed, @GroupSortOrder) Select Scope_Identity()",
                        CommandType.Text, 
                        new SqlParameter("@GroupName", propertyGroup.Name),
                        new SqlParameter("@GroupNameDisplayed", string.IsNullOrEmpty(propertyGroup.NameDisplayed) ? propertyGroup.Name : propertyGroup.NameDisplayed),
                        new SqlParameter("@GroupSortOrder", propertyGroup.SortOrder)));

            return propertyGroup.PropertyGroupId;
        }

        public static void Update(PropertyGroup propertyGroup)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update Catalog.PropertyGroup Set GroupName=@GroupName, GroupNameDisplayed=@GroupNameDisplayed, GroupSortOrder=@GroupSortOrder Where PropertyGroupId=@PropertyGroupId",
                CommandType.Text,
                new SqlParameter("@PropertyGroupId", propertyGroup.PropertyGroupId),
                new SqlParameter("@GroupName", propertyGroup.Name),
                new SqlParameter("@GroupNameDisplayed", string.IsNullOrEmpty(propertyGroup.NameDisplayed) ? propertyGroup.Name : propertyGroup.NameDisplayed),
                new SqlParameter("@GroupSortOrder", propertyGroup.SortOrder));
        }

        public static void UpdateSortOrder(int groupId, int sortOrder)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update Catalog.PropertyGroup Set GroupSortOrder=@GroupSortOrder Where PropertyGroupId=@PropertyGroupId",
                CommandType.Text,
                new SqlParameter("@PropertyGroupId", groupId),
                new SqlParameter("@GroupSortOrder", sortOrder));
        }
        
        public static void Delete(int propertyGroupId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From Catalog.PropertyGroup Where PropertyGroupId=@PropertyGroupId; " +
                "Update Catalog.Property Set GroupId = Null Where GroupId=@PropertyGroupId", 
                CommandType.Text,
                new SqlParameter("@PropertyGroupId", propertyGroupId));
        }

        #endregion


        #region Get/Add/Update/Delete category of group

        public static List<string> GetGroupCategories(int propertyGroupId)
        {
            return SQLDataAccess.ExecuteReadList(
                "Select Category.Name From Catalog.PropertyGroupCategory " +
                "Left Join Catalog.Category on Category.CategoryId = PropertyGroupCategory.CategoryId " +
                "Where PropertyGroupId=@PropertyGroupId", 
                CommandType.Text,
                reader => SQLDataHelper.GetString(reader, "Name"),
                new SqlParameter("@PropertyGroupId", propertyGroupId));
        }

        public static void AddGroupToCategory(int propertyGroupId, int categoryId)
        {
            SQLDataAccess.ExecuteNonQuery("Insert Into Catalog.PropertyGroupCategory (PropertyGroupId, CategoryId) Values(@PropertyGroupId, @CategoryId)", CommandType.Text,
                                          new SqlParameter("@PropertyGroupId", propertyGroupId),
                                          new SqlParameter("@CategoryId", categoryId));
        }

        public static void DeleteGroupFromCategory(int propertyGroupId, int categoryId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete From Catalog.PropertyGroupCategory Where PropertyGroupId=@PropertyGroupId And CategoryId=@CategoryId", CommandType.Text,
                                          new SqlParameter("@PropertyGroupId", propertyGroupId),
                                          new SqlParameter("@CategoryId", categoryId));
        }

        public static void DeleteGroupCategories(int propertyGroupId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete From Catalog.PropertyGroupCategory Where PropertyGroupId=@PropertyGroupId", CommandType.Text,
                                          new SqlParameter("@PropertyGroupId", propertyGroupId));
        }

        public static void DeleteGroupCategoriesByCategoryId(int categoryId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete From Catalog.PropertyGroupCategory Where CategoryId=@CategoryId", CommandType.Text,
                                          new SqlParameter("@CategoryId", categoryId));
        }


        #endregion
    }
}