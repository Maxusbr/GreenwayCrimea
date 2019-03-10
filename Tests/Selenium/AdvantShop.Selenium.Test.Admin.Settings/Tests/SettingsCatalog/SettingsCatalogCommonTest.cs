using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCatalog.Common
{
    [TestFixture]
    public class SettingsCatalogCommonProductsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
            reindexSearch();
        }
        
        [Test]
        public void ShowQuickViewOn()
        {
            testname = "SettingsCatalogCommonProductsShowQuickViewOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("ShowQuickView");

            GoToClient("categories/apple-phones");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElements(By.CssSelector(".products-view-picture-link"))[0]);
            a.Perform();

            driver.FindElement(By.LinkText("Быстрый просмотр")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyIsTrue(driver.FindElement(By.Name("form")).Enabled, "quick show pop up");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowQuickViewOff()
        {
            testname = "SettingsCatalogCommonProductsShowQuickViewOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("ShowQuickView");

            GoToClient("categories/apple-phones");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElements(By.CssSelector(".products-view-picture-link"))[0]);
            a.Perform();
            
            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-picture-link"))[0].Text.Contains("Быстрый просмотр"), "quick show pop up");

            VerifyFinally(testname);
        }
        
        [Test]
        public void ProductsPerPage()
        {
            testname = "SettingsCatalogCommonProductsPerPage";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            
            driver.FindElement(By.Id("ProductsPerPage")).Click();
            driver.FindElement(By.Id("ProductsPerPage")).Clear();
            driver.FindElement(By.Id("ProductsPerPage")).SendKeys("2");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("2", driver.FindElement(By.Id("ProductsPerPage")).GetAttribute("value"), "products per page admin value");

            //check client
            GoToClient("categories/apple-phones");
            
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block")).Count == 2, "products per page client count");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowProductsCountOn()
        {
            testname = "SettingsCatalogCommonShowProductsCountOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("ShowProductsCount");
            
            GoToClient();

            VerifyIsTrue(driver.FindElements(By.CssSelector(".menu-dropdown-link-wrap.cs-bg-i-7.icon-right-open-after-abs"))[1].Text.Contains("Техника (181)"), "products count in menu");

            GoToClient("categories/tech");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".product-categories-item-slim"))[1].Text.Contains("Игровые приставки (36)"), "products count in category");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowProductsCountOff()
        {
            testname = "SettingsCatalogCommonShowProductsCountOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("ShowProductsCount");

            GoToClient();

            VerifyIsFalse(driver.FindElements(By.CssSelector(".menu-dropdown-link-wrap.cs-bg-i-7.icon-right-open-after-abs"))[1].Text.Contains("Техника (181)"), "products count in menu");

            GoToClient("categories/tech");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".product-categories-item-slim"))[1].Text.Contains("Игровые приставки (36)"), "products count in category");

            VerifyFinally(testname);
        }

        [Test]
        public void DisplayCategoriesInBottomMenuOn()
        {
            testname = "SettingsCatalogCommonDisplayCategoriesInBottomMenuOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("DisplayCategoriesInBottomMenu");

            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Категории"), "display categories in bottom menu");
            
            VerifyFinally(testname);
        }

        [Test]
        public void DisplayCategoriesInBottomMenuOff()
        {
            testname = "SettingsCatalogCommonDisplayCategoriesInBottomMenuOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("DisplayCategoriesInBottomMenu");

            GoToClient();

            VerifyIsFalse(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Категории"), "display categories in bottom menu");
            
            VerifyFinally(testname);
        }

        [Test]
        public void ShowProductArtNoOn()
        {
            testname = "SettingsCatalogCommonShowProductArtNoOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("ShowProductArtNo");

            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].Text.Contains("966"), "show product art number in catalog");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowProductArtNoOff()
        {
            testname = "SettingsCatalogCommonShowProductArtNoOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("ShowProductArtNo");

            GoToClient("categories/apple-phones");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].Text.Contains("app939_4s"), "show product art number in catalog");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableProductRatingOn()
        {
            testname = "SettingsCatalogCommonEnableProductRatingOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("EnableProductRating");

            GoToClient("categories/apple-phones");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".rating")).Enabled, "enable product rating");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableProductRatingOff()
        {
            testname = "SettingsCatalogCommonEnableProductRatingOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("EnableProductRating");

            GoToClient("categories/apple-phones");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElements(By.CssSelector(".rating")).Count > 0, "enable product rating");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableCompareProductsOn()
        {
            testname = "SettingsCatalogCommonEnableCompareProductsOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("EnableCompareProducts");

            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".toolbar-bottom")).Text.Contains("Сравнение товаров"), "enable compare products");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableCompareProductsOff()
        {
            testname = "SettingsCatalogCommonEnableCompareProductsOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("EnableCompareProducts");

            GoToClient();

            VerifyIsFalse(driver.FindElement(By.CssSelector(".toolbar-bottom")).Text.Contains("Сравнение товаров"), "enable compare products");

            VerifyFinally(testname);
        }

        [Test]
        public void EnablePhotoPreviewsOn()
        {
            testname = "SettingsCatalogCommonEnablePhotoPreviewsOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("EnablePhotoPreviews");

            GoToClient("categories/apple-phones");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElements(By.CssSelector(".products-view-picture-link"))[0]);
            a.Perform();

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElements(By.CssSelector(".products-view-photos-item"))[0].Enabled, "enable photo previews");

            VerifyFinally(testname);
        }

        [Test]
        public void EnablePhotoPreviewsOff()
        {
            testname = "SettingsCatalogCommonEnablePhotoPreviewsOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("EnablePhotoPreviews");

            GoToClient("categories/apple-phones");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElements(By.CssSelector(".products-view-picture-link"))[0]);
            a.Perform();

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElements(By.CssSelector(".products-view-photos-item")).Count > 0, "enable photo previews");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowCountPhotoOn()
        {
            testname = "SettingsCatalogCommonShowCountPhotoOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("ShowCountPhoto");

            GoToClient("categories/apple-phones");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElements(By.CssSelector(".products-view-picture-link"))[0]);
            a.Perform();

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-photos-count.cs-bg-1.cs-t-8")).Text.Contains("3"), "show count photo");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowCountPhotoOff()
        {
            testname = "SettingsCatalogCommonShowCountPhotoOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("ShowCountPhoto");

            GoToClient("categories/apple-phones");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElements(By.CssSelector(".products-view-picture-link"))[0]);
            a.Perform();

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElements(By.CssSelector(".products-view-photos-count.cs-bg-1.cs-t-8")).Count > 0, "show count photo");

            VerifyFinally(testname);
        }


        [Test]
        public void ShowOnlyAvailableOn()
        {
            testname = "SettingsCatalogCommonShowOnlyAvailableOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("ShowOnlyAvalible");

            GoToClient("categories/igrushki");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("7"), "show only available products");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowOnlyAvailableOff()
        {
            testname = "SettingsCatalogCommonShowOnlyAvailableOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("ShowOnlyAvalible");

            GoToClient("categories/igrushki");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("8"), "show only available products");

            VerifyFinally(testname);
        }

        [Test]
        public void MoveNotAvailableToEndOn()
        {
            testname = "SettingsCatalogCommonMoveNotAvailableToEndOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkSelected("MoveNotAvaliableToEnd");

            GoToClient("categories/igrushki");

            var lastElem = driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block")).Count;
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[lastElem - 1].Text.Contains("Бабочка"), "move not available products to end");

            VerifyFinally(testname);
        }

        [Test]
        public void MoveNotAvailableToEndOff()
        {
            testname = "SettingsCatalogCommonMoveNotAvailableToEndOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");

            checkNotSelected("MoveNotAvaliableToEnd");

            GoToClient("categories/igrushki");
            
            var lastElem = driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block")).Count;
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[lastElem - 1].Text.Contains("Lego - Creator Highway Speedster (от 7 до 12 лет)"), "move not available products to end");
            
            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class SettingsCatalogCommonFiltersTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
        }

        [Test]
        public void FilterVisibilityOn()
        {
            testname = "SettingsCatalogCommonFilterVisibilityOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkSelected("FilterVisibility");

            GoToClient("categories/igrushki");
           
            VerifyIsTrue(driver.FindElements(By.Name("catalogFilterForm")).Count > 0, "filter visibility");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterVisibilityOff()
        {
            testname = "SettingsCatalogCommonFilterVisibilityOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkNotSelected("FilterVisibility");

            GoToClient("categories/igrushki");
            
            VerifyIsFalse(driver.FindElements(By.Name("catalogFilterForm")).Count > 0, "filter visibility");
            
            VerifyFinally(testname);
        }

        [Test]
        public void ShowPriceFilterOn()
        {
            testname = "SettingsCatalogCommonShowPriceFilterOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkSelected("ShowPriceFilter");

            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Цена"), "show price filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowPriceFilterOff()
        {
            testname = "SettingsCatalogCommonShowPriceFilterOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkNotSelected("ShowPriceFilter");
            
            GoToClient("categories/platia");

            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Цена"), "show price filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowProducerFilterOn()
        {
            testname = "SettingsCatalogCommonShowProducerFilterOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkSelected("ShowProducerFilter");

            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Производители"), "show producer filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowProducerFilterOff()
        {
            testname = "SettingsCatalogCommonShowProducerFilterOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkNotSelected("ShowProducerFilter");

            GoToClient("categories/platia");

            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Производители"), "show producer filter");

            VerifyFinally(testname);
        }
        
        [Test]
        public void ShowSizeFilterOn()
        {
            testname = "SettingsCatalogCommonShowSizeFilterOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkSelected("ShowSizeFilter");

            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Размер"), "show size filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowSizeFilterOff()
        {
            testname = "SettingsCatalogCommonShowSizeFilterOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkNotSelected("ShowSizeFilter");

            GoToClient("categories/platia");

            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Размер"), "show size filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowColorFilterOn()
        {
            testname = "SettingsCatalogCommonShowColorFilterOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkSelected("ShowColorFilter");

            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Цвет"), "show color filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowColorFilterOff()
        {
            testname = "SettingsCatalogCommonShowColorFilterOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkNotSelected("ShowColorFilter");

            GoToClient("categories/platia");

            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Цвет"), "show color filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ExcludingFiltersOn()
        {
            testname = "SettingsCatalogCommonExcludingFiltersOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkSelected("ShowProducerFilter");
            checkSelected("ExcludingFilters");

            GoToClient("categories/platia");

            var textColorWithoutFilter = driver.FindElement(By.Name("catalogFilterForm")).FindElement(By.XPath("//span[contains(text(), 'Armani')]")).GetCssValue("color");
            
            GoToClient("categories/platia?pricefrom=2500&priceto=4600");

            var textColorWithFilter = driver.FindElement(By.Name("catalogFilterForm")).FindElement(By.XPath("//span[contains(text(), 'Armani')]")).GetCssValue("color");

            VerifyIsFalse(textColorWithoutFilter.Equals(textColorWithFilter), "excluding filters");

            VerifyFinally(testname);
        }

        [Test]
        public void ExcludingFiltersOff()
        {
            testname = "SettingsCatalogCommonExcludingFiltersOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowOnlyAvalible"));

            checkSelected("ShowProducerFilter");
            checkNotSelected("ExcludingFilters");

            GoToClient("categories/platia");

            var textColorWithoutFilter = driver.FindElement(By.Name("catalogFilterForm")).FindElement(By.XPath("//span[contains(text(), 'Armani')]")).GetCssValue("color");

            GoToClient("categories/platia?pricefrom=2500&priceto=4600");

            var textColorWithFilter = driver.FindElement(By.Name("catalogFilterForm")).FindElement(By.XPath("//span[contains(text(), 'Armani')]")).GetCssValue("color");

            VerifyIsTrue(textColorWithoutFilter.Equals(textColorWithFilter), "excluding filters");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class SettingsCatalogCommonSizeColorTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
        }

        [Test]
        public void SizesHeader()
        {
            testname = "SettingsCatalogCommonSizesHeader";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowColorFilter"));

            driver.FindElement(By.Id("SizesHeader")).Click();
            driver.FindElement(By.Id("SizesHeader")).Clear();
            driver.FindElement(By.Id("SizesHeader")).SendKeys("SizesHeader Test");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("SizesHeader Test", driver.FindElement(By.Id("SizesHeader")).GetAttribute("value"), "size header admin value");

            //check client
            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("SizesHeader Test"), "size header client");
            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Размер"), "no previous size header client");

            VerifyFinally(testname);
        }

        [Test]
        public void ColorsHeader()
        {
            testname = "SettingsCatalogCommonColorsHeader";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowColorFilter"));

            driver.FindElement(By.Id("ColorsHeader")).Click();
            driver.FindElement(By.Id("ColorsHeader")).Clear();
            driver.FindElement(By.Id("ColorsHeader")).SendKeys("ColorsHeader Test");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("ColorsHeader Test", driver.FindElement(By.Id("ColorsHeader")).GetAttribute("value"), "color header admin value");

            //check client
            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("ColorsHeader Test"), "color header client");
            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).Text.Contains("Цвет"), "no previous color header client");

            VerifyFinally(testname);
        }

        [Test]
        public void ColorIconWidthCatalog()
        {
            testname = "SettingsCatalogCommonColorIconWidthCatalog";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowColorFilter"));

            driver.FindElement(By.Id("ColorIconWidthCatalog")).Click();
            driver.FindElement(By.Id("ColorIconWidthCatalog")).Clear();
            driver.FindElement(By.Id("ColorIconWidthCatalog")).SendKeys("50");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("50", driver.FindElement(By.Id("ColorIconWidthCatalog")).GetAttribute("value"), "color icon catalog admin value");

            //check client
            GoToClient("categories/platia");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("width").Contains("50px"), "color icon catalog width client");
            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).FindElements(By.CssSelector(".color-viewer-inner.cs-br-1"))[0].GetCssValue("width").Contains("50px"), "color icon catalog width filter client");
            GoToClient("products/dress1");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".details-row.details-colors")).FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("width").Contains("50px"), "color icon product cart width client");

            VerifyFinally(testname);
        }

        [Test]
        public void ColorIconWidthDetails()
        {
            testname = "SettingsCatalogCommonColorIconWidthDetails";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowColorFilter"));

            driver.FindElement(By.Id("ColorIconWidthDetails")).Click();
            driver.FindElement(By.Id("ColorIconWidthDetails")).Clear();
            driver.FindElement(By.Id("ColorIconWidthDetails")).SendKeys("35");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("35", driver.FindElement(By.Id("ColorIconWidthDetails")).GetAttribute("value"), "color icon catalog admin value");

            //check client
            GoToClient("categories/platia");
            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("width").Contains("35px"), "color icon catalog width client");
            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).FindElements(By.CssSelector(".color-viewer-inner.cs-br-1"))[0].GetCssValue("width").Contains("35px"), "color icon catalog width filter client");
            GoToClient("products/dress1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-row.details-colors")).FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("width").Contains("35px"), "color icon product cart width client");

            VerifyFinally(testname);
        }

        [Test]
        public void ColorIconHeightCatalog()
        {
            testname = "SettingsCatalogCommonColorIconHeightCatalog";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowColorFilter"));

            driver.FindElement(By.Id("ColorIconHeightCatalog")).Click();
            driver.FindElement(By.Id("ColorIconHeightCatalog")).Clear();
            driver.FindElement(By.Id("ColorIconHeightCatalog")).SendKeys("40");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("40", driver.FindElement(By.Id("ColorIconHeightCatalog")).GetAttribute("value"), "color icon catalog admin value");

            //check client
            GoToClient("categories/platia");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("height").Contains("40px"), "color icon catalog height client");
            VerifyIsTrue(driver.FindElement(By.Name("catalogFilterForm")).FindElements(By.CssSelector(".color-viewer-inner.cs-br-1"))[0].GetCssValue("height").Contains("40px"), "color icon catalog height filter client");
            GoToClient("products/dress1");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".details-row.details-colors")).FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("height").Contains("40px"), "color icon product cart height client");

            VerifyFinally(testname);
        }

        [Test]
        public void ColorIconHeightDetails()
        {
            testname = "SettingsCatalogCommonColorIconHeightDetails";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ShowColorFilter"));

            driver.FindElement(By.Id("ColorIconHeightDetails")).Click();
            driver.FindElement(By.Id("ColorIconHeightDetails")).Clear();
            driver.FindElement(By.Id("ColorIconHeightDetails")).SendKeys("20");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("20", driver.FindElement(By.Id("ColorIconHeightDetails")).GetAttribute("value"), "color icon catalog admin value");

            //check client
            GoToClient("categories/platia");
            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("height").Contains("20px"), "color icon catalog height client");
            VerifyIsFalse(driver.FindElement(By.Name("catalogFilterForm")).FindElements(By.CssSelector(".color-viewer-inner.cs-br-1"))[0].GetCssValue("height").Contains("20px"), "color icon catalog height filter client");
            GoToClient("products/dress1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-row.details-colors")).FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).GetCssValue("height").Contains("20px"), "color icon product cart height client");

            VerifyFinally(testname);
        }

        [Test]
        public void ComplexFilterOn()
        {
            testname = "SettingsCatalogCommonComplexFilterOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("SizesHeader"));

            checkSelected("ComplexFilter");

            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).Enabled, "complex filter");

            VerifyFinally(testname);
        }

        [Test]
        public void ComplexFilterOff()
        {
            testname = "SettingsCatalogCommonComplexFilterOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("SizesHeader"));

            checkNotSelected("ComplexFilter");

            GoToClient("categories/platia");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElements(By.CssSelector(".color-viewer-inner.cs-br-1")).Count > 0, "complex filter");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class SettingsCatalogCommonProductButtonsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
        }

        [Test]
        public void BuyButtonText()
        {
            testname = "SettingsCatalogCommonBuyButtonText";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ColorIconHeightDetails"));
            checkSelected("DisplayBuyButton");

            ScrollTo(By.Id("ColorIconHeightDetails"));
            driver.FindElement(By.Id("BuyButtonText")).Click();
            driver.FindElement(By.Id("BuyButtonText")).Clear();
            driver.FindElement(By.Id("BuyButtonText")).SendKeys("BuyButtonText Test");
            
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("BuyButtonText Test", driver.FindElement(By.Id("BuyButtonText")).GetAttribute("value"), "buy button text admin value");

            //check client
            GoToClient("categories/platia");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-buttons")).Text.Contains("BuyButtonText Test"), "buy button text catalog client");
            GoToClient("products/dress1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Text.Contains("BuyButtonText Test"), "buy button text product cart client");

            VerifyFinally(testname);
        }

        [Test]
        public void PreOrderButtonText()
        {
            testname = "SettingsCatalogCommonPreOrderButtonText";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ColorIconHeightDetails"));
            checkSelected("DisplayPreOrderButton");

            ScrollTo(By.Id("ColorIconHeightDetails"));
            driver.FindElement(By.Id("PreOrderButtonText")).Click();
            driver.FindElement(By.Id("PreOrderButtonText")).Clear();
            driver.FindElement(By.Id("PreOrderButtonText")).SendKeys("PreOrderButtonText Test");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            VerifyAreEqual("PreOrderButtonText Test", driver.FindElement(By.Id("PreOrderButtonText")).GetAttribute("value"), "preorder button text admin value");

            //check client
            GoToClient("categories/apple-phones");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-buttons")).Text.Contains("PreOrderButtonText Test"), "preorder button text catalog client");
            GoToClient("products/apple_iphone_4s_64gb_chernyi");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-payment-block")).Text.Contains("PreOrderButtonText Test"), "preorder button text product cart client");

            VerifyFinally(testname);
        }

        [Test]
        public void DisplayBuyButtonOn()
        {
            testname = "SettingsCatalogCommonDisplayBuyButtonOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ColorIconHeightDetails"));

            checkSelected("DisplayBuyButton");

            GoToClient("categories/platia");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-buttons")).Enabled, "display buy button catalog");
            
            VerifyFinally(testname);
        }

        [Test]
        public void DisplayBuyButtonOff()
        {
            testname = "SettingsCatalogCommonDisplayBuyButtonOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ColorIconHeightDetails"));

            checkNotSelected("DisplayBuyButton");

            GoToClient("categories/platia");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElements(By.CssSelector(".products-view-buttons")).Count > 0, "display buy button");
            
            VerifyFinally(testname);
        }
        
        [Test]
        public void DisplayPreOrderButtonOn()
        {
            testname = "SettingsCatalogCommonDisplayPreOrderButtonOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ColorIconHeightDetails"));

            checkSelected("DisplayPreOrderButton");

            GoToClient("categories/apple-phones");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-buttons")).Enabled, "display preorder button");

            VerifyFinally(testname);
        }

        [Test]
        public void DisplayPreOrderButtonOff()
        {
            testname = "SettingsCatalogCommonDisplayPreOrderButtonOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("ColorIconHeightDetails"));

            checkNotSelected("DisplayPreOrderButton");

            GoToClient("categories/apple-phones");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElements(By.CssSelector(".products-view-buttons")).Count > 0, "display preorder button");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class SettingsCatalogCommonProductsViewTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
        }

        [Test]
        public void EnableCatalogViewChangeOn()
        {
            testname = "SettingsCatalogCommonEnableCatalogViewChangeOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("BuyButtonText"));

            checkSelected("EnableCatalogViewChange");
            
            GoToClient("categories/apple-phones");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-variants")).Displayed, "enable catalog view");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableCatalogViewChangeOff()
        {
            testname = "SettingsCatalogCommonEnableCatalogViewChangeOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("BuyButtonText"));

            checkNotSelected("EnableCatalogViewChange");

            GoToClient("categories/apple-phones");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-variants")).Count > 0, "enable catalog view");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableSearchViewChangeOn()
        {
            testname = "SettingsCatalogCommonEnableSearchViewChangeOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("BuyButtonText"));

            checkSelected("EnableSearchViewChange");

            GoToClient("search?q=apple");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-variants")).Displayed, "enable search view");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableSearchViewChangeOff()
        {
            testname = "SettingsCatalogCommonEnableSearchViewChangeOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("BuyButtonText"));

            checkNotSelected("EnableSearchViewChange");

            GoToClient("search?q=apple");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".products-view-variants")).Count > 0, "enable search view");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultCatalogView()
        {
            testname = "SettingsCatalogCommonDefaultCatalogView";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("BuyButtonText"));

            (new SelectElement(driver.FindElement(By.Id("DefaultCatalogView")))).SelectByText("Таблица");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");
            
            IWebElement selectCatalogView = driver.FindElement(By.Id("DefaultCatalogView"));
            SelectElement select = new SelectElement(selectCatalogView);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Таблица"), "default catalog view admin");

            //check client
            GoToClient("categories/platia");
            
            VerifyIsTrue(driver.FindElement(By.CssSelector(".row.products-view.products-view-table")).Displayed, "default catalog view client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSearchView()
        {
            testname = "SettingsCatalogCommonDefaultSearchView";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=common");
            ScrollTo(By.Id("BuyButtonText"));

            (new SelectElement(driver.FindElement(By.Id("DefaultSearchView")))).SelectByText("Список");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=common");

            IWebElement selectCatalogView = driver.FindElement(By.Id("DefaultSearchView"));
            SelectElement select = new SelectElement(selectCatalogView);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Список"), "default search view admin");

            //check clients
            GoToClient("search?q=apple");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view.products-view-container.products-view-list")).Displayed, "default search view client");

            VerifyFinally(testname);
        }
    }
}