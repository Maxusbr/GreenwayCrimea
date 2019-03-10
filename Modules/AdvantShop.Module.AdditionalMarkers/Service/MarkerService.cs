using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Module.AdditionalMarkers.Models;
using System.Data.SqlClient;
using AdvantShop.Core.Modules;
using System.Data;
using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Module.AdditionalMarkers.Service
{
    public class MarkerService
    {
        public static int InsertOrUpdateMarker(Marker marker, bool identityOff = false)
        {
            try
            {
                var pars = new SqlParameter[]
                {
                    new SqlParameter("@markerId",marker.MarkerId),
                    new SqlParameter("@name",marker.Name),
                    new SqlParameter("@color",marker.Color.Replace("#","")),
                    new SqlParameter("@colorName",marker.ColorName.Replace("#","")),
                    new SqlParameter("@url",marker.Url ?? (object)DBNull.Value),
                    new SqlParameter("@description",marker.Description ?? (object)DBNull.Value),
                    new SqlParameter("@openNewTab",marker.OpenNewTab),
                    new SqlParameter("@sortOrder",marker.SortOrder),
                };

                var insert = identityOff ? @"SET IDENTITY_INSERT Module.Marker ON
                                        INSERT INTO Module.Marker (MarkerId, Name, Color, ColorName, Url, Description, OpenNewTab, SortOrder) VALUES (@markerId, @name, @color, @colorName, @url, @description, @openNewTab, @sortOrder)
                                        SET IDENTITY_INSERT Module.Marker OFF
                                        SELECT @markerId" :
                                    @"INSERT INTO Module.Marker VALUES (@name, @color, @colorName, @url, @description, @openNewTab, @sortOrder)
                                    SELECT SCOPE_IDENTITY()";

                var query = @"IF ((SELECT COUNT(MarkerId) FROM Module.Marker WHERE MarkerId=@markerId) > 0)
                                BEGIN
                                    UPDATE Module.Marker SET Name=@name, Color=@color, ColorName=@colorName, Url=@url, Description=@description, OpenNewTab=@openNewTab,SortOrder=@sortOrder 
                                            WHERE MarkerId = @markerId
                                    SELECT @markerId
                                END
                              ELSE
                                BEGIN
                                    "+ insert + @"
                                END";

                return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text, pars);
            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(ex);
                return -1;
            }
        }

        public static List<int> GetKeys()
        {
            var query = @"SELECT MarkerId FROM Module.Marker";
            return ModulesRepository.Query<int>(query, CommandType.Text).ToList();
        }

        public static bool DeleteMarker(int markerId)
        {
            var query = @"DELETE FROM Module.Marker Where MarkerId=" + markerId;
            return ModuleService.NonQuery(query);
        }

        public static List<Marker> GetMarkers()
        {
            var query = @"SELECT * FROM Module.Marker ORDER BY SortOrder";

            return ModulesRepository.Query<Marker>(query,CommandType.Text).ToList();
        }

        public static List<Marker> GetMarkers(int productId)
        {
            var query = @"SELECT Marker.MarkerId,
		                            Marker.Name,
		                            Color,
		                            ColorName,
		                            Url,
		                            Marker.[Description],
		                            OpenNewTab,
		                            SortOrder
                            FROM Module.Marker
                            INNER JOIN Module.ProductMarker ON ProductMarker.MarkerId = Marker.MarkerId
                            WHERE ProductMarker.ProductId =" + productId + " ORDER BY SortOrder";

            return ModulesRepository.Query<Marker>(query, CommandType.Text).ToList();
        }

        public static Marker GetMarker(int markerId)
        {
            var query = @"SELECT * FROM Module.Marker WHERE MarkerId = " + markerId;
            return ModulesRepository.Query<Marker>(query, commandType: CommandType.Text).FirstOrDefault();
        }

        public static int CurrentSortOrder()
        {
        var query = @"IF ((SELECT COUNT(SortOrder) FROM Module.Marker) > 0)
		                    SELECT MAX(SortOrder) FROM [Module].[Marker]
                        ELSE
	                        SELECT 0";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text) + 10;
        }

        public static int Link(int productId, int markerId)
        {
            var pars = new SqlParameter[]
            {
                new SqlParameter("@productId", productId),
                new SqlParameter("@markerId", markerId)
            };

            var query = @"IF ((SELECT COUNT (*) FROM Module.ProductMarker WHERE ProductId=@productId AND MarkerId=@markerId) > 0)
                            BEGIN
                                DELETE FROM Module.ProductMarker WHERE ProductId=@productId AND MarkerId=@markerId
                                SELECT 0
                            END
                          ELSE
                            BEGIN
                                INSERT INTO Module.ProductMarker VALUES (@productId, @markerId)
                                SELECT 1
                            END";

            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text, pars);
        }

        public static ProductViewMarker GetLinks(int productId)
        {
            var pvm = new ProductViewMarker();
            var product = Catalog.ProductService.GetProduct(productId);

            pvm.ProductId = productId;
            pvm.ProductUrl = product.UrlPath;

            var query = @"SELECT Marker.MarkerId,
		                        Name,
		                        Color,
		                        ColorName,
		                        Url,
		                        [Description],
		                        OpenNewTab,
		                        SortOrder
                        FROM Module.Marker
                        INNER JOIN Module.ProductMarker ON ProductMarker.MarkerId = Marker.MarkerId
                        WHERE ProductMarker.ProductId = " + productId + @" ORDER BY SortOrder";

            pvm.Markers = ModulesRepository.Query<Marker>(query, CommandType.Text).ToList();

            return pvm;
        }

        public static ProductViewMarker GetLinks(string productUrl)
        {
            var pvm = new ProductViewMarker();
            var product = Catalog.ProductService.GetProductByUrl(productUrl);

            pvm.ProductId = product.ProductId;
            pvm.ProductUrl = productUrl;

            var query = @"SELECT Marker.MarkerId,
		                            Marker.Name,
		                            Color,
		                            ColorName,
		                            Url,
		                            Marker.[Description],
		                            OpenNewTab,
		                            SortOrder
                            FROM Module.Marker
                            INNER JOIN Module.ProductMarker ON ProductMarker.MarkerId = Marker.MarkerId
                            INNER JOIN [Catalog].Product ON Product.ProductId = ProductMarker.ProductId
                            WHERE Product.UrlPath = '" + productUrl + @"'
                            ORDER BY SortOrder";

            pvm.Markers = ModulesRepository.Query<Marker>(query, CommandType.Text).ToList();

            return pvm;
        }

        public static List<int> GetLinksByMarkerId(int markerId)
        {
            var query = @"SELECT ProductId FROM Module.ProductMarker
                            WHERE MarkerId = " + markerId;

            return ModulesRepository.Query<int>(query, CommandType.Text).ToList();
        }


        public static string PrepareCSVField(CSVField field, int productId)
        {
            var productMarkers = GetLinks(productId).Markers;

            return string.Join(",", productMarkers.Select(x => "cl" + x.MarkerId));
        }

        public static bool ProcessCSVField(CSVField field, int productId, string value)
        {
            var markersToLink = value.Replace("cl", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (ClearProduct(productId))
            {
                foreach (var markerId in markersToLink)
                    Link(productId, markerId.TryParseInt());
                return true;
            }
            return false;
        }

        public static bool ClearProduct(int productId)
        {
            var query = @"DELETE FROM Module.ProductMarker WHERE ProductId=" + productId;
            return ModuleService.NonQuery(query);
        }
    }
}
