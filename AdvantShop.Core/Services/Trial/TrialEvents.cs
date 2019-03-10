//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Trial
{
    public enum TrialEvents
    {
        //tech events
        RightCity,
        WrongCity,
        // lvl 0

        VisitClientSide,
        VisitAdminSide,

        // lvl 1
        CreateShop,
        ChangeShopName,
        ChangePhoneNumber,
        ChangeTheme,
        ChangeColorScheme,
        ChangeBackGround,
        ChangeMainPageMode,
        ChangeLogo,
        AddCarousel,
        BuyTheProduct,
        ChangeOrderStatus,
        ShareInSocialNetwork,

        // lvl 2
        DeleteTestData,
        AddCategory,
        AddProduct,
        AddProductPhoto,
        AddProductProperty,
        AddShippingMethod,
        AddPaymentMethod,
        ChangeContactPage,
        ActivateModule,
        DeactivateModule,

        // lvl 3
        ChangeDomain,
        SendTestEmail,
        SetUpGoogleAnalytics,
        SetUpYandexMentrika,
        GetFirstThouthandVisitors,
        CheckoutOrder,
        ExportProductsToFeed,

        // lvl 2.5
        MakeCSVExport,
        MakeCSVImport,
        GenerateLogo
        
    }
}