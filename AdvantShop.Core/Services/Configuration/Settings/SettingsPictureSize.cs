//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Helpers;

namespace AdvantShop.Configuration
{
    public class SettingsPictureSize
    {
        #region Brand

        public static int BrandLogoWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["BrandLogoWidth"]); }
            set { TemplateSettingsProvider.Items["BrandLogoWidth"] = value.ToString(); }
        }

        public static int BrandLogoHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["BrandLogoHeight"]); }
            set { TemplateSettingsProvider.Items["BrandLogoHeight"] = value.ToString(); }
        }

        #endregion

        #region Product

        public static int BigProductImageWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["BigProductImageWidth"]); }
            set { TemplateSettingsProvider.Items["BigProductImageWidth"] = value.ToString(); }
        }

        public static int BigProductImageHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["BigProductImageHeight"]); }
            set { TemplateSettingsProvider.Items["BigProductImageHeight"] = value.ToString(); }
        }

        public static int MiddleProductImageWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["MiddleProductImageWidth"]); }
            set { TemplateSettingsProvider.Items["MiddleProductImageWidth"] = value.ToString(); }
        }

        public static int MiddleProductImageHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["MiddleProductImageHeight"]); }
            set { TemplateSettingsProvider.Items["MiddleProductImageHeight"] = value.ToString(); }
        }

        public static int SmallProductImageWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["SmallProductImageWidth"]); }
            set { TemplateSettingsProvider.Items["SmallProductImageWidth"] = value.ToString(); }
        }

        public static int SmallProductImageHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["SmallProductImageHeight"]); }
            set { TemplateSettingsProvider.Items["SmallProductImageHeight"] = value.ToString(); }
        }

        public static int XSmallProductImageWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["XSmallProductImageWidth"]); }
            set { TemplateSettingsProvider.Items["XSmallProductImageWidth"] = value.ToString(); }
        }

        public static int XSmallProductImageHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["XSmallProductImageHeight"]); }
            set { TemplateSettingsProvider.Items["XSmallProductImageHeight"] = value.ToString(); }
        }

        #endregion

        #region Category

        public static int BigCategoryImageWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["BigCategoryImageWidth"]); }
            set { TemplateSettingsProvider.Items["BigCategoryImageWidth"] = value.ToString(); }
        }

        public static int BigCategoryImageHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["BigCategoryImageHeight"]); }
            set { TemplateSettingsProvider.Items["BigCategoryImageHeight"] = value.ToString(); }
        }

        public static int SmallCategoryImageWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["SmallCategoryImageWidth"]); }
            set { TemplateSettingsProvider.Items["SmallCategoryImageWidth"] = value.ToString(); }
        }

        public static int SmallCategoryImageHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["SmallCategoryImageHeight"]); }
            set { TemplateSettingsProvider.Items["SmallCategoryImageHeight"] = value.ToString(); }
        }


        public static int IconCategoryImageWidth
        {
            get { return 30; }
        }

        public static int IconCategoryImageHeight
        {
            get { return 30; }
        }


        #endregion

        #region News

        public static int NewsImageWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["NewsImageWidth"]); }
            set { TemplateSettingsProvider.Items["NewsImageWidth"] = value.ToString(); }
        }

        public static int NewsImageHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["NewsImageHeight"]); }
            set { TemplateSettingsProvider.Items["NewsImageHeight"] = value.ToString(); }
        }

        #endregion

        #region Carousel

        public static int CarouselBigWidth
        {
            get { return 1920; }
        }

        public static int CarouselBigHeight
        {
            get { return 1000; }
        }

        #endregion
        
        #region Payment Icons

        public static int PaymentIconWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["PaymentIconWidth"]); }
            set { TemplateSettingsProvider.Items["PaymentIconWidth"] = value.ToString(); }
        }

        public static int PaymentIconHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["PaymentIconHeight"]); }
            set { TemplateSettingsProvider.Items["PaymentIconHeight"] = value.ToString(); }
        }

        #endregion

        #region Shipping Icons

        public static int ShippingIconWidth
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["ShippingIconWidth"]); }
            set { TemplateSettingsProvider.Items["ShippingIconWidth"] = value.ToString(); }
        }

        public static int ShippingIconHeight
        {
            get { return SQLDataHelper.GetInt(TemplateSettingsProvider.Items["ShippingIconHeight"]); }
            set { TemplateSettingsProvider.Items["ShippingIconHeight"] = value.ToString(); }
        }

        #endregion


        #region Color Icons

        public static int ColorIconWidthCatalog
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["ColorIconWidthCatalog"]); }
            set { SettingProvider.Items["ColorIconWidthCatalog"] = value.ToString(); }
        }

        public static int ColorIconHeightCatalog
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["ColorIconHeightCatalog"]); }
            set { SettingProvider.Items["ColorIconHeightCatalog"] = value.ToString(); }
        }

        public static int ColorIconWidthDetails
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["ColorIconWidthDetails"]); }
            set { SettingProvider.Items["ColorIconWidthDetails"] = value.ToString(); }
        }

        public static int ColorIconHeightDetails
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["ColorIconHeightDetails"]); }
            set { SettingProvider.Items["ColorIconHeightDetails"] = value.ToString(); }
        }

        #endregion

        #region Manager

        public static int ManagerFotoWidth
        {
            get { return 110; }
        }

        public static int ManagerFotoHeight
        {
            get { return 140; }
        }
        #endregion

        #region Review Image

        public static int ReviewImageWidth
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["ReviewImageWidth"]); }
            set { SettingProvider.Items["ReviewImageWidth"] = value.ToString(); }
        }

        public static int ReviewImageHeight
        {
            get { return SQLDataHelper.GetInt(SettingProvider.Items["ReviewImageHeight"]); }
            set { SettingProvider.Items["ReviewImageHeight"] = value.ToString(); }
        }

        #endregion
    }
}