using AdvantShop.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Module.SimaLand.Models;
using AdvantShop.Catalog;
using System.Drawing;
using AdvantShop.Helpers;
using AdvantShop.Configuration;
using System.Net;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;

namespace AdvantShop.Module.SimaLand.Service
{
    public class AdvBrandService
    {
        private static int? GetBrandIdByName(string brandName)
        {
            var query = @"SELECT BrandID  FROM [Catalog].[Brand] where BrandName = @brandName";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text, new SqlParameter("@brandName", brandName));
        }

        public static int InsertOrGetBrand(SimalandTrademark trademark)
        {

            var brandId = GetBrandIdByName(trademark.name);
            if (brandId.HasValue && brandId != 0)
            {
                return (int)brandId;
            }

            var query = @"Insert into Catalog.Brand (BrandName, BrandDescription, Enabled, SortOrder, UrlPath)
	                  values (@name, @description, @enabled, @sortorder, @urlpath) Select Scope_Identity()";

            brandId = ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text,
                        new SqlParameter("@name", trademark.name),
                        new SqlParameter("@description", trademark.description),
                        new SqlParameter("@enabled", true),
                        new SqlParameter("@sortorder", 10),
                        new SqlParameter("@urlpath", trademark.slug));
            try
            {
                if (trademark.photo != "")
                {
                    downloadimage(trademark.photo, (int)brandId);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("at AdvBrandService.downloadimage path = " + trademark.photo, ex);
            }
            return (int)brandId;

        }

        private static bool downloadimage(string photoName, int objId)
        {
            try
            {
                var link = string.Format("https://cdn.sima-land.ru/trademark/100/{0}", photoName);
                WebRequest getImage = WebRequest.Create(link);
                var imageResponse = getImage.GetResponse();
                var slImage = imageResponse.GetResponseStream();

                var tempName =
                    PhotoService.AddPhoto(new Photo(0, objId, PhotoType.Brand)
                    {
                        OriginName = "brandlogo_simaland.jpg"
                    });
                using (Image image = Image.FromStream(slImage, true))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, tempName),
                                    SettingsPictureSize.BrandLogoWidth, SettingsPictureSize.BrandLogoHeight, image);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return true;
        }
    }
}
