//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;

namespace AdvantShop.CMS
{
    public class MenuService
    {
        private static void ClearMenuCache()
        {
            CacheManager.RemoveByPattern(CacheNames.MenuPrefix);
        }

        public static int AddMenuItem(AdvMenuItem mItem)
        {
            ClearMenuCache();
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO [CMS].[Menu] (MenuItemParentID, MenuItemName, MenuItemIcon, MenuItemUrlPath, MenuItemUrlType, SortOrder, ShowMode, Enabled, Blank, NoFollow, MenuType) " +
                "VALUES (@MenuItemParentID, @MenuItemName, @MenuItemIcon, @MenuItemUrlPath, @MenuItemUrlType, @SortOrder, @ShowMode, @Enabled, @Blank, @NoFollow, @MenuType); " +
                "SELECT scope_identity();",
                CommandType.Text,
                new SqlParameter("@MenuItemParentID", mItem.MenuItemParentID == 0 ? DBNull.Value : (object) mItem.MenuItemParentID),
                new SqlParameter("@MenuItemName", mItem.MenuItemName),
                new SqlParameter("@MenuItemIcon", string.IsNullOrEmpty(mItem.MenuItemIcon) ? DBNull.Value : (object) mItem.MenuItemIcon),
                new SqlParameter("@MenuItemUrlPath", mItem.MenuItemUrlPath),
                new SqlParameter("@MenuItemUrlType", mItem.MenuItemUrlType),
                new SqlParameter("@SortOrder", mItem.SortOrder),
                new SqlParameter("@Blank", mItem.Blank),
                new SqlParameter("@ShowMode", mItem.ShowMode),
                new SqlParameter("@Enabled", mItem.Enabled),
                new SqlParameter("@NoFollow", mItem.NoFollow),
                new SqlParameter("@MenuType", mItem.MenuType));
        }

        private static AdvMenuItem GetMenuItemFromReader(SqlDataReader reader)
        {
            return new AdvMenuItem
            {
                MenuItemID = SQLDataHelper.GetInt(reader, "MenuItemID"),
                MenuItemParentID = SQLDataHelper.GetInt(reader, "MenuItemParentID"),
                MenuItemName = SQLDataHelper.GetString(reader, "MenuItemName"),
                MenuItemIcon = SQLDataHelper.GetString(reader, "MenuItemIcon"),
                MenuItemUrlPath = SQLDataHelper.GetString(reader, "MenuItemUrlPath"),
                MenuItemUrlType = (EMenuItemUrlType)SQLDataHelper.GetInt(reader, "MenuItemUrlType"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                ShowMode = (EMenuItemShowMode)SQLDataHelper.GetInt(reader, "ShowMode"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                Blank = SQLDataHelper.GetBoolean(reader, "Blank"),
                NoFollow = SQLDataHelper.GetBoolean(reader, "NoFollow"),
                MenuType = (EMenuType)SQLDataHelper.GetInt(reader, "MenuType")
            };
        }

        public static AdvMenuItem GetMenuItemById(int mItemId)
        {
            return SQLDataAccess.ExecuteReadOne<AdvMenuItem>(
                "SELECT * FROM [CMS].[Menu] WHERE MenuItemID = @MenuItemID",
                CommandType.Text, GetMenuItemFromReader,
                new SqlParameter("@MenuItemID", mItemId));
        }

        public static List<int> GetParentMenuItems(int mItemId)
        {
            return SQLDataAccess.ExecuteReadColumn<int>(
                "[CMS].[sp_GetParentMenuItemsByItemId]",
                CommandType.StoredProcedure, "MenuItemID",
                new SqlParameter("@MenuItemID", mItemId));
        }

        public static void UpdateMenuItem(AdvMenuItem mItem)
        {
            ClearMenuCache();
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [CMS].[Menu] SET MenuItemParentID=@MenuItemParentID, MenuItemName=@MenuItemName, MenuItemIcon=@MenuItemIcon, MenuItemUrlPath=@MenuItemUrlPath, " +
                "MenuItemUrlType=@MenuItemUrlType, SortOrder=@SortOrder, ShowMode=@ShowMode, Enabled=@Enabled, Blank=@Blank, Nofollow=@NoFollow " +
                "WHERE MenuItemID = @MenuItemID",
                CommandType.Text,
                new SqlParameter("@MenuItemID", mItem.MenuItemID),
                new SqlParameter("@MenuItemParentID", mItem.MenuItemParentID == 0 ? DBNull.Value : (object)mItem.MenuItemParentID),
                new SqlParameter("@MenuItemName", mItem.MenuItemName),
                new SqlParameter("@MenuItemIcon", string.IsNullOrEmpty(mItem.MenuItemIcon) ? DBNull.Value : (object)mItem.MenuItemIcon),
                new SqlParameter("@MenuItemUrlPath", mItem.MenuItemUrlPath),
                new SqlParameter("@MenuItemUrlType", mItem.MenuItemUrlType),
                new SqlParameter("@SortOrder", mItem.SortOrder),
                new SqlParameter("@ShowMode", mItem.ShowMode),
                new SqlParameter("@Enabled", mItem.Enabled),
                new SqlParameter("@Blank", mItem.Blank),
                new SqlParameter("@NoFollow", mItem.NoFollow));
        }

        public static void DeleteMenuItemById(int mItemId)
        {
            ClearMenuCache();
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [CMS].[Menu] WHERE MenuItemID = @MenuItemID",
                CommandType.Text, new SqlParameter("@MenuItemID", mItemId));
        }

        public static void DeleteMenuItemIconById(int mItemId, string filePath)
        {
            ClearMenuCache();
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [CMS].[Menu] SET MenuItemIcon = NULL WHERE MenuItemID = @MenuItemID",
                CommandType.Text, new SqlParameter("@MenuItemID", mItemId));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Возвращает сам parentId и child ids
        /// </summary>
        /// <returns></returns>
        public static List<int> GetAllChildIdByParent(int parentId, EMenuType type)
        {
            return SQLDataAccess.ExecuteReadColumn<int>(
                "[CMS].[sp_GetChildMenuItemByParent]",
                CommandType.StoredProcedure,
                "MenuItemID",
                new SqlParameter("@ParentId", parentId),
                new SqlParameter("@MenuType", (int)type));
        }

        public static List<AdvMenuItem> GetChildMenuItemsByParentId(int mItemParentId, EMenuType type)
        {
            return SQLDataAccess.ExecuteReadList<AdvMenuItem>(
                "SELECT p.*, (SELECT COUNT(MenuItemID) FROM [CMS].[Menu] AS c WHERE c.MenuItemParentID = p.MenuItemID and c.Enabled=1) as Child_Count " +
                "FROM [CMS].[Menu] as p WHERE MenuType = @MenuType AND " + 
                (mItemParentId == 0 ? "[MenuItemParentID] IS NULL " : "[MenuItemParentID] = " + mItemParentId) +
                " ORDER BY [SortOrder]",
                CommandType.Text,
                (reader) =>
                {
                    var mItem = GetMenuItemFromReader(reader);
                    if (mItem.MenuItemUrlType != EMenuItemUrlType.Custom)
                    {
                        mItem.MenuItemUrlPath = UrlService.GetUrl() + mItem.MenuItemUrlPath;
                    }
                    mItem.HasChild = SQLDataHelper.GetInt(reader, "Child_Count") > 0;
                    return mItem;
                },
                new SqlParameter("@MenuType", type));
        }

        public static List<MenuItemModel> GetMenuItems(int parentId, EMenuType type, EMenuItemShowMode showMode)
        {
            return SQLDataAccess.ExecuteReadList(

                "SELECT p.*, (SELECT Count(MenuItemID) FROM [CMS].[Menu] AS c WHERE c.MenuItemParentID = p.MenuItemID and c.Enabled=1) as Child_Count " +
                "FROM [CMS].[Menu] as p WHERE MenuType = @MenuType AND " +
                (parentId == 0 ? "[MenuItemParentID] IS NULL" : "[MenuItemParentID] = " + parentId) +
                " AND (ShowMode = 0 OR ShowMode = @ShowMode) AND Enabled = 1 ORDER BY [SortOrder]",
                CommandType.Text,
                reader =>
                {
                    var item = new MenuItemModel
                    {
                        ItemId = SQLDataHelper.GetInt(reader, "MenuItemID"),
                        ItemParentId = SQLDataHelper.GetInt(reader, "MenuItemParentID"),
                        Name = SQLDataHelper.GetString(reader, "MenuItemName"),
                        UrlPath = SQLDataHelper.GetString(reader, "MenuItemUrlPath"),
                        IconPath = SQLDataHelper.GetString(reader, "MenuItemIcon"),
                        Blank = SQLDataHelper.GetBoolean(reader, "Blank"),
                        NoFollow = SQLDataHelper.GetBoolean(reader, "NoFollow"),
                        MenuType = (EMenuType) SQLDataHelper.GetInt(reader, "MenuType"),
                        HasChild = SQLDataHelper.GetInt(reader, "Child_Count") > 0,
                        DisplaySubItems = false
                    };

                    return item;
                },
                new SqlParameter("@showMode", (int) showMode), 
                new SqlParameter("@MenuType", (int) type));
        }

        public static List<MenuItemModel> GetAllMenuItems(int parentId, EMenuType type)
        {
            return SQLDataAccess.ExecuteReadList(

                "SELECT p.*, (SELECT Count(MenuItemID) FROM [CMS].[Menu] AS c WHERE c.MenuItemParentID = p.MenuItemID) as Child_Count " +
                "FROM [CMS].[Menu] as p WHERE MenuType = @MenuType AND " +
                (parentId == 0 ? "[MenuItemParentID] IS NULL" : "[MenuItemParentID] = " + parentId) +
                " ORDER BY [SortOrder]",
                CommandType.Text,
                reader =>
                {
                    var item = new MenuItemModel
                    {
                        ItemId = SQLDataHelper.GetInt(reader, "MenuItemID"),
                        ItemParentId = SQLDataHelper.GetInt(reader, "MenuItemParentID"),
                        Name = SQLDataHelper.GetString(reader, "MenuItemName"),
                        UrlPath = SQLDataHelper.GetString(reader, "MenuItemUrlPath"),
                        IconPath = SQLDataHelper.GetString(reader, "MenuItemIcon"),
                        Blank = SQLDataHelper.GetBoolean(reader, "Blank"),
                        NoFollow = SQLDataHelper.GetBoolean(reader, "NoFollow"),
                        MenuType = (EMenuType)SQLDataHelper.GetInt(reader, "MenuType"),
                        HasChild = SQLDataHelper.GetInt(reader, "Child_Count") > 0,
                        DisplaySubItems = false
                    };

                    return item;
                },
                new SqlParameter("@MenuType", (int)type));
        }

        public static List<string> GetMenuIcons()
        {
            return SQLDataAccess.ExecuteReadColumn<string>("select MenuItemIcon FROM [CMS].[Menu] where MenuItemIcon<>'' and MenuItemIcon is not null", CommandType.Text, "MenuItemIcon");
        }

    }
}