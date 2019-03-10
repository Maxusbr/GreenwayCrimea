using AdvantShop.Core.SQL;
using AdvantShop.SEO;

namespace AdvantShop.Handlers.Inplace
{
    public class GetMetaHandler
    {
        public object Execute(int id, MetaType type)
        {
            var metaInfo = MetaInfoService.GetMetaInfo(id, type);

            string globalVaribles = string.Empty;
            string nameFieldTitle = string.Empty;
            string queryName = string.Empty;

            switch (type)
            {
                case MetaType.Brand:
                    globalVaribles = Resources.Resource.Admin_m_Brand_UseGlobalVariables;
                    nameFieldTitle = Resources.Resource.Admin_m_Brand_Name;
                    queryName = "SELECT BrandName FROM Catalog.Brand Where BrandID=@objId";
                    break;
                case MetaType.Category:
                    globalVaribles = Resources.Resource.Admin_m_Category_UseGlobalVariables;
                    nameFieldTitle = Resources.Resource.Admin_m_Category_Name;
                    queryName = "SELECT [Name] FROM [Catalog].[Category] Where CategoryID=@objId";
                    break;
                case MetaType.News:
                    globalVaribles = Resources.Resource.Admin_m_News_UseGlobalVariables;
                    nameFieldTitle = Resources.Resource.Admin_m_News_Title;
                    queryName = "SELECT [Title] FROM [Settings].[News] Where NewsID=@objId";
                    break;
                case MetaType.Product:
                    globalVaribles = Resources.Resource.Admin_m_Product_UseGlobalVariables;
                    nameFieldTitle = Resources.Resource.Admin_Product_Name;
                    queryName = "SELECT [Name] FROM [Catalog].[Product] Where [ProductID]=@objId";
                    break;
				case MetaType.Tag:
					globalVaribles = Resources.Resource.Admin_Tag_UseGlobalVariables;
					nameFieldTitle = Resources.Resource.Admin_Tag_Name;
					queryName = "SELECT [Name] FROM [Catalog].[Tag] Where [Id]=@objId";
					break;
                case MetaType.StaticPage:
                    globalVaribles = Resources.Resource.Admin_StaticPage_UseGlobalVariables;
                    nameFieldTitle = Resources.Resource.Admin_StaticPage_PageName;
                    queryName = "SELECT [PageName] FROM [CMS].[StaticPage] Where [StaticPageID]=@objId";
                    break;
            }

            return metaInfo != null
                ? new
                {
                    metaInfo.H1,
                    metaInfo.MetaDescription,
                    metaInfo.MetaKeywords,
                    metaInfo.Title,
                    globalVaribles = globalVaribles,
                    Name =
                        SQLDataAccess.ExecuteScalar(queryName, System.Data.CommandType.Text,
                            new System.Data.SqlClient.SqlParameter("@objId", id)),
                    NameFieldTitle = nameFieldTitle
                }
                : new
                {
                    H1 = string.Empty,
                    MetaDescription = string.Empty,
                    MetaKeywords = string.Empty,
                    Title = string.Empty,
                    globalVaribles = globalVaribles,
                    Name =
                        SQLDataAccess.ExecuteScalar(queryName, System.Data.CommandType.Text,
                            new System.Data.SqlClient.SqlParameter("@objId", id)),
                    NameFieldTitle = nameFieldTitle
                };
        }
    }
}