//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.SEO;

namespace AdvantShop.CMS
{
    public class StaticPageService
    {
        public static StaticPage GetStaticPage(int pageId)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM [CMS].[StaticPage] WHERE [StaticPageID] = @StaticPageID", CommandType.Text,
                GetStaticPageFromReader, new SqlParameter("@StaticPageID", pageId));
        }

        public static StaticPage GetStaticPage(string urlPath)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM [CMS].[StaticPage] WHERE [urlPath] = @urlPath", CommandType.Text,
                GetStaticPageFromReader, new SqlParameter("@urlPath", urlPath));
        }

        public static StaticPage GetFirstPage()
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT top (1) * FROM [CMS].[StaticPage] WHERE enabled=1", CommandType.Text,
                GetStaticPageFromReader);
        }
               

        private static StaticPage GetStaticPageFromReader(SqlDataReader reader)
        {
            return new StaticPage
            {
                StaticPageId = SQLDataHelper.GetInt(reader, "StaticPageID"),
                PageName = SQLDataHelper.GetString(reader, "PageName"),
                PageText = SQLDataHelper.GetString(reader, "PageText"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                AddDate = SQLDataHelper.GetDateTime(reader, "AddDate"),
                ModifyDate = SQLDataHelper.GetDateTime(reader, "ModifyDate"),
                IndexAtSiteMap = SQLDataHelper.GetBoolean(reader, "IndexAtSiteMap"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                ParentId = SQLDataHelper.GetInt(reader, "ParentID", 0),
                UrlPath = SQLDataHelper.GetString(reader, "UrlPath")
            };
        }

        public static void DeleteStaticPage(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [CMS].[StaticPage] WHERE [StaticPageID] = @ID", CommandType.Text, new SqlParameter("@ID", id));
            foreach (var childId in GetChildStaticPages(id, true).Select(page => page.StaticPageId))
                DeleteStaticPage(childId);
        }

        public static IEnumerable<int> GetStaticPagesIDs()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("SELECT [StaticPageID] FROM [CMS].[StaticPage] Order by SortOrder", CommandType.Text, "StaticPageID");
        }

        public static void UpdateStaticPage(StaticPage page)
        {
            SQLDataAccess.ExecuteNonQuery(
                @"UPDATE [CMS].[StaticPage] SET [PageName] = @PageName, [PageText] = @PageText, [SortOrder] = @SortOrder, [ModifyDate] = GETDATE(), [IndexAtSiteMap] = @IndexAtSiteMap, 
                    [ParentID] = @ParentID, [Enabled] = @Enabled, UrlPath=@UrlPath WHERE [StaticPageID] = @StaticPageID",
                CommandType.Text,
                new[]
                {
                    new SqlParameter("@PageName", page.PageName),
                    new SqlParameter("@PageText", page.PageText ?? string.Empty),
                    new SqlParameter("@SortOrder", page.SortOrder),
                    new SqlParameter("@IndexAtSiteMap", page.IndexAtSiteMap),
                    new SqlParameter("@ParentID", page.ParentId == 0 ? DBNull.Value : (object) page.ParentId),
                    new SqlParameter("@StaticPageID", page.StaticPageId),
                    new SqlParameter("@Enabled", page.Enabled),
                    new SqlParameter("@UrlPath", page.UrlPath)
                });

            // ---- Meta
            if (page.Meta != null)
            {
                if (page.Meta.Title.IsNullOrEmpty() && page.Meta.MetaKeywords.IsNullOrEmpty() &&
                    page.Meta.MetaDescription.IsNullOrEmpty() && page.Meta.H1.IsNullOrEmpty())
                {
                    if (MetaInfoService.IsMetaExist(page.ID, MetaType.StaticPage))
                        MetaInfoService.DeleteMetaInfo(page.ID, MetaType.StaticPage);
                }
                else
                    MetaInfoService.SetMeta(page.Meta);
            }
        }

        public static IEnumerable<StaticPage> GetAllStaticPages()
        {
            return SQLDataAccess.ExecuteReadIEnumerable<StaticPage>("SELECT * FROM [CMS].[StaticPage] Order by SortOrder", CommandType.Text,
                                          GetStaticPageFromReader);
        }

        public static IEnumerable<StaticPage> GetRootStaticPages()
        {
            return SQLDataAccess.ExecuteReadIEnumerable<StaticPage>("SELECT * FROM [CMS].[StaticPage] Where ParentID is NULL OR [ParentID] = 0 Order by SortOrder", CommandType.Text,
                                          GetStaticPageFromReader);
        }

        public static int AddStaticPage(StaticPage page)
        {
            page.StaticPageId =
                SQLDataAccess.ExecuteScalar<int>(
                    @"INSERT INTO [CMS].[StaticPage] ([PageName],[PageText],[SortOrder],[AddDate],[ModifyDate],[IndexAtSiteMap],[ParentID],[Enabled],[UrlPath]) VALUES " +
                    " (@PageName,@PageText,@SortOrder,GETDATE(),GETDATE(),@IndexAtSiteMap,@ParentID,@Enabled, @UrlPath); SELECT scope_identity();",
                    CommandType.Text,
                    new[]
                    {
                        new SqlParameter("@PageName", page.PageName ?? ""),
                        new SqlParameter("@PageText", page.PageText ?? ""),
                        new SqlParameter("@SortOrder", page.SortOrder),
                        new SqlParameter("@IndexAtSiteMap", page.IndexAtSiteMap),
                        new SqlParameter("@Enabled", page.Enabled),
                        page.ParentId == 0
                            ? new SqlParameter("@ParentID", DBNull.Value)
                            : new SqlParameter("@ParentID", page.ParentId),
                        new SqlParameter("@UrlPath", page.UrlPath)
                    });

	        if (page.Meta != null)
	        {
                if (!page.Meta.Title.IsNullOrEmpty() || !page.Meta.MetaKeywords.IsNullOrEmpty() || !page.Meta.MetaDescription.IsNullOrEmpty() || !page.Meta.H1.IsNullOrEmpty())
		        {
			        page.Meta.ObjId = page.StaticPageId;
                    MetaInfoService.SetMeta(page.Meta);
		        }
	        }
            return page.StaticPageId;
        }

        public static List<StaticPage> GetChildStaticPages(int parentId, bool enabledOnly)
        {
            var command = enabledOnly
                            ? "SELECT * FROM [CMS].[StaticPage] WHERE [ParentID] = @ParentID and enabled=1 Order by SortOrder"
                            : "SELECT * FROM [CMS].[StaticPage] WHERE [ParentID] = @ParentID Order by SortOrder";

            return SQLDataAccess.ExecuteReadList(command, CommandType.Text, GetStaticPageFromReader, new SqlParameter("@ParentID", parentId));
        }

        public static List<int> GetParentStaticPages(int pageId)
        {
            return SQLDataAccess.ExecuteReadColumn<int>("[CMS].[sp_GetParentStaticPages]", CommandType.StoredProcedure, "StaticPageID", new SqlParameter("@ChildPageId", pageId));
        }


        public static bool CheckChilds(int parentId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM [CMS].[StaticPage] WHERE [ParentID] = @ParentID",
                CommandType.Text,
                new SqlParameter("@ParentID", parentId)) > 0;
        }

        public static List<int> GetAllChildIdByParent(int parent)
        {
            return SQLDataAccess.ExecuteReadColumn<int>("[CMS].[sp_GetChildStaticPagesByParent]", CommandType.StoredProcedure, "StaticPageID", new SqlParameter("@ParentId", parent));
        }

        public static void SetStaticPageActivity(int staticPageId, bool active)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [CMS].[StaticPage] Set Enabled = @Enabled Where [StaticPageID] = @StaticPageID",
                CommandType.Text,
                new SqlParameter("@StaticPageID", staticPageId),
                new SqlParameter("@Enabled", active));
        }

        public static void ChangeParentPage(int staticPageId, int parentId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [CMS].[StaticPage] Set ParentID = @ParentID Where [StaticPageID] = @StaticPageID",
                CommandType.Text,
                new SqlParameter("@StaticPageID", staticPageId),
                new SqlParameter("@ParentID", parentId != 0 ? (object) parentId : DBNull.Value));
        }
    }
}