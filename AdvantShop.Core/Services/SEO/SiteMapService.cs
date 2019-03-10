//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;

namespace AdvantShop.SEO
{
    public class SiteMapService
    {
        private const string DefaultChangeFreq = "daily";

        private const float DefaultPriority = 0.5f;
        private const int MaxUrlCount = 50000;
        private const int SepFileLength = 30000;

        private static void CreateSiteMapFile(IEnumerable<SiteMapData> data, string fileName)
        {
            var writer = XmlWriter.Create(fileName);

            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (SiteMapData d in data)
            {
                writer.WriteStartElement("url");
                // url -------------

                writer.WriteStartElement("loc");
                writer.WriteString(d.Loc);
                writer.WriteEndElement();

                writer.WriteStartElement("lastmod");
                writer.WriteString(d.Lastmod.ToString("yyyy-MM-dd"));
                writer.WriteEndElement();

                writer.WriteStartElement("changefreq");
                writer.WriteString(d.Changefreq);
                writer.WriteEndElement();

                writer.WriteStartElement("priority");
                writer.WriteString(d.Priority.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();

                // url -------------
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
        }

        public static void GenerateSiteMap(string strFinalFilePath, string strInitFileVirtualPath)
        {
            var data = (List<SiteMapData>)GetSiteMapData();
            if (data.Count > MaxUrlCount)
            {
                var dataLists = new List<List<SiteMapData>>();
                while (data.Count > 0)
                {
                    dataLists.Add(data.Take(SepFileLength).ToList());
                    data.RemoveRange(0, data.Count >= SepFileLength ? SepFileLength : data.Count);
                }
                var writer = XmlWriter.Create(strFinalFilePath);
                writer.WriteStartDocument();
                writer.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");

                int fileIndex = 0;
                foreach (var dataList in dataLists)
                {
                    string filePath = string.Format("{0}_{1}.xml", strFinalFilePath, fileIndex++);
                    CreateSiteMapFile(dataList, filePath);

                    writer.WriteStartElement("sitemap");

                    writer.WriteStartElement("loc");
                    writer.WriteString(SettingsMain.SiteUrl + "/" + strInitFileVirtualPath +
                                       filePath.Split('\\').Last());
                    writer.WriteEndElement();

                    writer.WriteStartElement("lastmod");
                    writer.WriteString(DateTime.Now.ToString("yyyy-MM-dd"));
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            else
            {
                CreateSiteMapFile(data, strFinalFilePath);
            }
        }

        // Extension. No reference at code
        public static string GenerateSiteMapXmlString()
        {

            var sw = new StringWriter();

            var data = (List<SiteMapData>)GetSiteMapData();
            var writer = XmlWriter.Create(sw);

            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (SiteMapData d in data)
            {
                writer.WriteStartElement("url");
                // url -------------

                writer.WriteStartElement("loc");
                writer.WriteString(d.Loc);
                writer.WriteEndElement();

                writer.WriteStartElement("lastmod");
                writer.WriteString(d.Lastmod.ToString("yyyy-MM-dd"));
                writer.WriteEndElement();

                writer.WriteStartElement("changefreq");
                writer.WriteString(d.Changefreq);
                writer.WriteEndElement();

                writer.WriteStartElement("priority");
                writer.WriteString(d.Priority.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();

                // url -------------
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
            return sw.ToString();

        }

        private static SiteMapData GetSiteMapDataFromReader(SqlDataReader reader)
        {
            var prefUrl = SettingsMain.SiteUrl;
            prefUrl = prefUrl.Contains("http://") ? prefUrl : "http://" + prefUrl;

            var siteMapData = new SiteMapData
                                  {
                                      Changefreq = DefaultChangeFreq,
                                      Priority = DefaultPriority
                                  };

            if (reader.FieldCount == 1)
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.Category, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "CategoryId"));
                siteMapData.Lastmod = DateTime.Now;
            }
            else if (reader.GetName(0).ToLower() == "productid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "Productid"));
                siteMapData.Lastmod = SQLDataHelper.GetDateTime(reader, "DateModified");
            }
            else if (reader.GetName(0).ToLower() == "newsid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.News, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "NewsID"));
                siteMapData.Lastmod = SQLDataHelper.GetDateTime(reader, "AddingDate");
            }
            else if (reader.GetName(0).ToLower() == "staticpageid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.StaticPage, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "StaticPageID"));
                siteMapData.Lastmod = SQLDataHelper.GetDateTime(reader, "ModifyDate");
            }
            return siteMapData;
        }

        private static IList<SiteMapData> GetSiteMapData()
        {
            var res = new List<SiteMapData>();
            res.AddRange(SQLDataAccess.ExecuteReadIEnumerable<SiteMapData>("SELECT [CategoryId],[UrlPath] FROM [Catalog].[Category] WHERE [Enabled] = '1'", CommandType.Text, GetSiteMapDataFromReader));
            res.AddRange(SQLDataAccess.ExecuteReadIEnumerable<SiteMapData>(
                @"SELECT [Product].[ProductID], [Product].[DateModified],[UrlPath]
                        FROM [Catalog].[Product] 
                        INNER JOIN [Catalog].[ProductCategories] 
	                        ON [Catalog].[Product].[ProductID] = [Catalog].[ProductCategories].[ProductID]
                        INNER JOIN [Catalog].[Category]
	                        ON [Catalog].[Category].[CategoryID] = [Catalog].[ProductCategories].[CategoryID]
	                        AND [Catalog].[Category].[Enabled] = '1'
                        WHERE [Product].[Enabled] = '1' and (select Count(CategoryID) from Catalog.ProductCategories where ProductID=[Product].[ProductID])<> 0",
                CommandType.Text,
                GetSiteMapDataFromReader));
            res.AddRange(SQLDataAccess.ExecuteReadIEnumerable<SiteMapData>("SELECT [NewsID], [AddingDate],[UrlPath] FROM [Settings].[News]", CommandType.Text, GetSiteMapDataFromReader));
            res.AddRange(SQLDataAccess.ExecuteReadIEnumerable<SiteMapData>("SELECT [StaticPageID],[ModifyDate],[UrlPath] FROM [CMS].[StaticPage] where IndexAtSiteMap=1", CommandType.Text, GetSiteMapDataFromReader));
            return res;
        }
    }
}