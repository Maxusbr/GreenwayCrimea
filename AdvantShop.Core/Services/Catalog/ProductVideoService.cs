//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class ProductVideoService
    {
        public static List<ProductVideo> GetProductVideos(int productId)
        {
            List<ProductVideo> list = SQLDataAccess.ExecuteReadList<ProductVideo>(
                "SELECT ProductVideoID, ProductID, Name, PlayerCode, Description, VideoSortOrder FROM [Catalog].[ProductVideo] WHERE [ProductID]=@ProductID ORDER BY [VideoSortOrder]",
                CommandType.Text, GetProductVideoFromReader, new SqlParameter { ParameterName = "@ProductID", Value = productId });

            return list;
        }

        public static ProductVideo GetProductVideoFromReader(SqlDataReader reader)
        {
            return new ProductVideo
                       {
                           ProductVideoId = SQLDataHelper.GetInt(reader, "ProductVideoID"),
                           ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                           Name = SQLDataHelper.GetString(reader, "Name"),
                           PlayerCode = SQLDataHelper.GetString(reader, "PlayerCode"),
                           Description = SQLDataHelper.GetString(reader, "Description"),
                           VideoSortOrder = SQLDataHelper.GetInt(reader, "VideoSortOrder")
                       };
        }

        public static void AddProductVideo(ProductVideo pv)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Catalog].[ProductVideo] ([ProductID], [Name], [PlayerCode], [Description], [VideoSortOrder]) VALUES (@ProductId, @Name, @PlayerCode, @Description, @VideoSortOrder)",
                            CommandType.Text, new[]
                                                        {
                                                         new SqlParameter("@ProductID", pv.ProductId),
                                                         new SqlParameter("@Name", pv.Name),
                                                         new SqlParameter("@PlayerCode", pv.PlayerCode),
                                                         new SqlParameter("@Description", pv.Description),
                                                         new SqlParameter("@VideoSortOrder", pv.VideoSortOrder)
                                                        }
                            );
        }

        public static void UpdateProductVideo(ProductVideo pv)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE Catalog.ProductVideo SET Name=@Name, PlayerCode=@PlayerCode, Description=@Description, VideoSortOrder=@VideoSortOrder WHERE ProductVideoID = @ProductVideoID",
                            CommandType.Text, new[]
                                                        {
                                                         new SqlParameter("@ProductVideoId", pv.ProductVideoId),
                                                         new SqlParameter("@Name", pv.Name),
                                                         new SqlParameter("@PlayerCode", pv.PlayerCode),
                                                         new SqlParameter("@Description", pv.Description),
                                                         new SqlParameter("@VideoSortOrder", pv.VideoSortOrder)
                                                        }
                            );
        }

        public static void UpdateProductVideo(int productVideoId, string name, int videoSortOrder)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE Catalog.ProductVideo SET Name=@Name, VideoSortOrder=@VideoSortOrder WHERE ProductVideoID=@ProductVideoID",
                                                CommandType.Text,
                                                new SqlParameter { ParameterName = "@Name", Value = name },
                                                new SqlParameter { ParameterName = "@VideoSortOrder", Value = videoSortOrder },
                                                new SqlParameter { ParameterName = "@ProductVideoId", Value = productVideoId }
                                                );
        }

        public static void DeleteProductVideo(int productVideoId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[ProductVideo] WHERE ProductVideoId=@ProductVideoId", CommandType.Text,
                                                new SqlParameter { ParameterName = "@ProductVideoId", Value = productVideoId });
        }

        public static void DeleteProductVideos(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[ProductVideo] WHERE ProductId=@ProductId", CommandType.Text,
                                                new SqlParameter { ParameterName = "@ProductId", Value = productId });
        }

        public static ProductVideo GetProductVideo(int productVideoId)
        {
            return
                SQLDataAccess.ExecuteReadOne<ProductVideo>(
                    "SELECT * FROM [Catalog].[ProductVideo] WHERE [ProductVideoID] = @ProductVideoID", CommandType.Text,
                    GetProductVideoFromReader, new SqlParameter("@ProductVideoID", productVideoId));
        }

        public static bool HasVideo(int productId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                 "SELECT Count(ProductVideoID) FROM [Catalog].[ProductVideo] WHERE [ProductID] = @ProductID",
                 CommandType.Text, new SqlParameter("@ProductID", productId)) > 0;

        }

        public static string VideoToString(List<ProductVideo> productVideos, string columSeparator)
        {
            var sb = new StringBuilder();

            foreach (var video in productVideos.OrderBy(v => v.VideoSortOrder))
            {
                sb.AppendFormat("{0}" + columSeparator, video.PlayerCode);
            }

            return sb.ToString().Trim((columSeparator ?? "").ToCharArray());
        }


        public static void VideoFromString(int productId, string videos, string columSeparator)
        {
            if (string.IsNullOrWhiteSpace(columSeparator))
                _VideoFromString(productId, videos);
            else
                _VideoFromString(productId, videos, columSeparator);

        }

        private static void _VideoFromString(int productId, string videos)
        {
            DeleteProductVideos(productId);
            if (string.IsNullOrWhiteSpace(videos))
                return;
            var arrVideos = videos.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            int count = 0;
            foreach (var videoStr in arrVideos)
            {
                if (videoStr == ",")
                    continue;
                if (videoStr.StartsWith("http://") || videoStr.StartsWith("https://"))
                {
                    string error;
                    var playerCode = GetPlayerCodeFromLink(videoStr, out error);
                    if (string.IsNullOrEmpty(error))
                    {
                        AddProductVideo(new ProductVideo
                        {
                            Description = string.Empty,
                            Name = count == 0 ? productId.ToString() : productId + "-" + count,
                            PlayerCode = playerCode,
                            ProductId = productId,
                            VideoSortOrder = 0
                        });
                    }
                }
                else
                {
                    AddProductVideo(new ProductVideo
                    {
                        Description = string.Empty,
                        Name = count == 0 ? productId.ToString() : productId + "-" + count,
                        PlayerCode = videoStr,
                        ProductId = productId,
                        VideoSortOrder = 0
                    });
                }
                count++;
            }
        }

        private static void _VideoFromString(int productId, string videos, string columSeparator)
        {
            DeleteProductVideos(productId);
            if (string.IsNullOrWhiteSpace(videos))
                return;
            var arrVideos = videos.Split(columSeparator);
            var count = 0;
            foreach (var videoStr in arrVideos)
            {
                if (videoStr == columSeparator)
                    continue;
                if (videoStr.StartsWith("http://") || videoStr.StartsWith("https://"))
                {
                    string error;
                    var playerCode = GetPlayerCodeFromLink(videoStr, out error);
                    if (string.IsNullOrEmpty(error))
                    {
                        AddProductVideo(new ProductVideo
                        {
                            Description = string.Empty,
                            Name = productId + "-" + count,
                            PlayerCode = playerCode,
                            ProductId = productId,
                            VideoSortOrder = 0
                        });
                    }
                }
                else
                {
                    AddProductVideo(new ProductVideo
                    {
                        Description = string.Empty,
                        Name = productId + "-" + count,
                        PlayerCode = videoStr,
                        ProductId = productId,
                        VideoSortOrder = 0
                    });
                }
                count++;
            }
        }

        public static string GetPlayerCodeFromLink(string videoLink, out string errorMessage)
        {
            videoLink = videoLink.Trim();
            errorMessage = string.Empty;
            string playercode;

            try
            {
                if (!String.IsNullOrEmpty(videoLink))
                {
                    if (videoLink.Contains("youtu.be"))
                    {
                        playercode =
                            String.Format(
                                "<iframe width=\"560\" height=\"315\" src=\"//www.youtube.com/embed/{0}\" frameborder=\"0\" allowfullscreen></iframe>",
                                videoLink.Split(new[] { "youtu.be/" }, StringSplitOptions.None).Last());
                    }
                    else if (videoLink.Contains("youtube.com"))
                    {
                        videoLink = videoLink.StartsWith("https://") ? videoLink : "https://" + videoLink.Replace("http://", "");

                        if (!Uri.IsWellFormedUriString(videoLink, UriKind.Absolute))
                        {
                            errorMessage = LocalizationService.GetResource("Core.Catalog.ProductVideo.WrongLink");
                            return string.Empty;
                        }
                        if (videoLink.Contains("v="))
                        {
                            var url = new Uri(videoLink);
                            string param = System.Web.HttpUtility.ParseQueryString(url.Query).Get("v");
                            playercode =
                            String.Format(
                                "<iframe width=\"560\" height=\"315\" src=\"//www.youtube.com/embed/{0}\" frameborder=\"0\" allowfullscreen></iframe>",
                                param);
                        }
                        else
                        {
                            playercode =
                            String.Format(
                                "<iframe width=\"560\" height=\"315\" src=\"{0}\" frameborder=\"0\" allowfullscreen></iframe>",
                                videoLink);
                        }
                        
                    }
                    else if (videoLink.Contains("vimeo.com"))
                    {
                        playercode =
                            String.Format(
                                "<iframe src=\"//player.vimeo.com/video/{0}?title=0&amp;byline=0&amp;portrait=0\" width=\"560\" height=\"315\" frameborder=\"0\" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>",
                                videoLink.Split(new[] { "vimeo.com/" }, StringSplitOptions.None).Last());
                    }
                    else
                    {
                        errorMessage = LocalizationService.GetResource("Core.Catalog.ProductVideo.WrongLink");
                        return string.Empty;
                    }
                }
                else
                {
                    errorMessage = LocalizationService.GetResource("Core.Catalog.ProductVideo.NoPlayerCode");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                errorMessage = LocalizationService.GetResource("Core.Catalog.ProductVideo.WrongLink");
                Diagnostics.Debug.Log.Error(ex);
                return string.Empty;
            }

            return playercode;
        }
    }
}

