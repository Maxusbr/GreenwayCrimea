//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;

namespace AdvantShop.Core.Services.CMS
{
    public static class CarouselService
    {
        private const string CarouselCacheKey = "Carousel";

        public static Carousel GetCarousel(int carouselId)
        {
            return SQLDataAccess.ExecuteReadOne<Carousel>("Select * From CMS.Carousel Where CarouselID=@CarouselID",
                CommandType.Text, GetCarouselFromReader, new SqlParameter("@CarouselID", carouselId));
        }

        public static void SetActive(int carouselId, bool active)
        {
            SQLDataAccess.ExecuteNonQuery("Update CMS.Carousel set Enabled=@Enabled where CarouselID=@CarouselID", CommandType.Text,
                                        new SqlParameter("@CarouselID", carouselId),
                                        new SqlParameter("@Enabled", active));

            CacheManager.RemoveByPattern(CarouselCacheKey);
        }

        public static void DeleteCarousel(int id)
        {
            SQLDataAccess.ExecuteNonQuery("[CMS].[sp_DeleteCarousel]", CommandType.StoredProcedure, new SqlParameter("@CarouselID", id));
            PhotoService.DeletePhotos(id, PhotoType.Carousel);

            CacheManager.RemoveByPattern(CarouselCacheKey);

            SettingsDesign.IsDefaultSlides = false;
        }

        public static int AddCarousel(Carousel carousel)
        {
            CacheManager.RemoveByPattern(CarouselCacheKey);

            SettingsDesign.IsDefaultSlides = false;

            carousel.CarouselId = SQLDataAccess.ExecuteScalar<int>("[CMS].[sp_InsertCarousel]", CommandType.StoredProcedure,
                    new SqlParameter("@URL", carousel.Url),
                    new SqlParameter("@Enabled", carousel.Enabled),
                    new SqlParameter("@SortOrder", carousel.SortOrder),
                    new SqlParameter("@DisplayInOneColumn", carousel.DisplayInOneColumn),
                    new SqlParameter("@DisplayInTwoColumns", carousel.DisplayInTwoColumns),
                    new SqlParameter("@DisplayInMobile", carousel.DisplayInMobile),
                    new SqlParameter("@Blank", carousel.Blank));

            return carousel.CarouselId;
        }

        public static int GetMaxSortOrder()
        {
            return SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("Select max(sortorder) from [CMS].[Carousel]", CommandType.Text)) + 10;
        }


        public static void UpdateCarousel(Carousel carousel)
        {
            CacheManager.RemoveByPattern(CarouselCacheKey);

            SQLDataAccess.ExecuteNonQuery("[CMS].[sp_UpdateCarousel]", CommandType.StoredProcedure,
                new SqlParameter("@CarouselID", carousel.CarouselId),
                new SqlParameter("@URL", carousel.Url),
                new SqlParameter("@SortOrder", carousel.SortOrder),
                new SqlParameter("@Enabled", carousel.Enabled),
                new SqlParameter("@DisplayInOneColumn", carousel.DisplayInOneColumn),
                new SqlParameter("@DisplayInTwoColumns", carousel.DisplayInTwoColumns),
                new SqlParameter("@DisplayInMobile", carousel.DisplayInMobile),
                new SqlParameter("@Blank", carousel.Blank));
        }

        private static Carousel GetCarouselFromReader(IDataReader reader)
        {
            return new Carousel
            {
                CarouselId = SQLDataHelper.GetInt(reader, "CarouselID"),
                Url = SQLDataHelper.GetString(reader, "URL"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                DisplayInOneColumn = SQLDataHelper.GetBoolean(reader, "DisplayInOneColumn"),
                DisplayInTwoColumns = SQLDataHelper.GetBoolean(reader, "DisplayInTwoColumns"),
                DisplayInMobile = SQLDataHelper.GetBoolean(reader, "DisplayInMobile"),
                Blank = SQLDataHelper.GetBoolean(reader, "Blank"),
            };
        }

        public static List<Carousel> GetAllCarousels()
        {
            return SQLDataAccess.ExecuteReadList<Carousel>("Select * From CMS.Carousel", CommandType.Text, GetCarouselFromReader);
        }

        public static List<Carousel> GetAllCarouselsMainPage(ECarouselPageMode mainPageMode)
        {
            string where = "";

            switch (mainPageMode)
            {
                case ECarouselPageMode.OneColumn:
                    where = " and DisplayInOneColumn=1";
                    break;
                case ECarouselPageMode.TwoColumns:
                    where = " and DisplayInTwoColumns=1";
                    break;
                case ECarouselPageMode.Mobile:
                    where = " and DisplayInMobile=1";
                    break;
                default: throw new NotImplementedException(mainPageMode.ToString() + " not implemented");
            }

            return
                CacheManager.Get(CarouselCacheKey + mainPageMode.ToString(), () =>
                    SQLDataAccess.ExecuteReadList(
                        "Select CarouselID, PhotoId, ObjId, URL, PhotoName, Description, DisplayInOneColumn, DisplayInTwoColumns, DisplayInMobile, Blank " +
                        "From CMS.Carousel Left Join Catalog.Photo on Photo.ObjId = Carousel.CarouselID and Type = @Type " +
                        "Where Enabled = 1 " + where +
                        "Order by SortOrder",
                        CommandType.Text,
                        reader => new Carousel
                        {
                            CarouselId = SQLDataHelper.GetInt(reader, "CarouselID"),
                            Url = SQLDataHelper.GetString(reader, "URL"),
                            Picture =
                                new CarouselPhoto(SQLDataHelper.GetInt(reader, "PhotoId"),
                                    SQLDataHelper.GetInt(reader, "ObjId"))
                                {
                                    PhotoName = SQLDataHelper.GetString(reader, "PhotoName"),
                                    Description = SQLDataHelper.GetString(reader, "Description")
                                },
                            DisplayInOneColumn = SQLDataHelper.GetBoolean(reader, "DisplayInOneColumn"),
                            DisplayInTwoColumns = SQLDataHelper.GetBoolean(reader, "DisplayInTwoColumns"),
                            DisplayInMobile = SQLDataHelper.GetBoolean(reader, "DisplayInMobile"),
                            Blank = SQLDataHelper.GetBoolean(reader, "Blank"),
                        },
                        new SqlParameter("@Type", PhotoType.Carousel.ToString())));
        }
    }
}