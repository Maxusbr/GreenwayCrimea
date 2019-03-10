using System.Data;
using System.Data.SqlClient;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.SEO;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceMetaHandler
    {
        public bool Execute(int id, string name, MetaType metaType, string title, string h1, string metaKeywords,
            string metaDescription)
        {
            MetaInfo metaInfo;

            if (MetaInfoService.IsMetaExist(id, metaType))
            {
                metaInfo = MetaInfoService.GetMetaInfo(id, metaType);
                metaInfo.Title = title;
                metaInfo.H1 = h1;
                metaInfo.MetaKeywords = metaKeywords;
                metaInfo.MetaDescription = metaDescription;
            }
            else
            {
                metaInfo = new MetaInfo()
                {
                    ObjId = id,
                    Title = title,
                    H1 = h1,
                    MetaKeywords = metaKeywords,
                    MetaDescription = metaDescription,
                    Type = metaType
                };
            }

            var query = string.Empty;

            switch (metaInfo.Type)
            {
                case MetaType.Brand:
                    query = "UPDATE Catalog.Brand SET BrandName=@name Where BrandID=@objId";
                    break;
                case MetaType.Category:
                    query = "UPDATE [Catalog].[Category] SET [Name] = @name WHERE CategoryID = @objId";
                    break;
                case MetaType.News:
                    query = "UPDATE [Settings].[News] SET [Title] = @name WHERE NewsID = @objId";
                    break;
                case MetaType.Product:
                    query = "UPDATE [Catalog].[Product] SET [Name] = @name WHERE [ProductID] = @objId";
                    break;
				case MetaType.Tag:
					query = "UPDATE [Catalog].[Tag] SET [Name] = @name WHERE [Id] = @objId";
					break;
                case MetaType.StaticPage:
                    query = "UPDATE [CMS].[StaticPage] SET [PageName] = @name WHERE [StaticPageID] = @objId";
                    break;
            }

            SQLDataAccess.ExecuteNonQuery(query, CommandType.Text, new SqlParameter("@name", name),
                new SqlParameter("@objId", metaInfo.ObjId));

            if (MetaType.Category == metaInfo.Type)
            {
                CategoryService.ClearCategoryCache();
            }

            MetaInfoService.SetMeta(metaInfo);

            return true;
        }
    }
}