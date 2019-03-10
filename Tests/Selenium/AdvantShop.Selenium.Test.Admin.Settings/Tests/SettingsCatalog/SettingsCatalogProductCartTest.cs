using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCatalog.ProductCart
{
    [TestFixture]
    public class SettingsCatalogProductCartTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
        }

        [Test]
        public void DisplayWeightOn()
        {
            testname = "SettingsCatalogProductCartDisplayWeightOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkSelected("DisplayWeight");

            GoToClient("products/apple_iphone_4s_64gb_chernyi");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-row.details-weight")).Displayed, "display weight");

            VerifyFinally(testname);
        }

        [Test]
        public void DisplayWeightOff()
        {
            testname = "SettingsCatalogProductCartDisplayWeightOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkNotSelected("DisplayWeight");

            GoToClient("products/apple_iphone_4s_64gb_chernyi");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".details-row.details-weight")).Count > 0, "display weight");

            VerifyFinally(testname);
        }

        [Test]
        public void DisplayDimensionsOn()
        {
            testname = "SettingsCatalogProductCartDisplayDimensionsOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkSelected("DisplayDimensions");

            GoToClient("products/smartfon-apple-iphone-7-32gb-chernyi");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-row.details-dimensions")).Displayed, "display dimensions");

            VerifyFinally(testname);
        }

        [Test]
        public void DisplayDimensionsOff()
        {
            testname = "SettingsCatalogProductCartDisplayDimensionsOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkNotSelected("DisplayDimensions");

            GoToClient("products/smartfon-apple-iphone-7-32gb-chernyi");

            VerifyIsFalse(driver.FindElements(By.CssSelector(".details-row.details-dimensions")).Count > 0, "display dimensions");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowStockAvailabilityOn()
        {
            testname = "SettingsCatalogProductCartShowStockAvailabilityOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkSelected("ShowStockAvailability");

            GoToClient("products/ipd35");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".availability.available")).Text.Contains("Есть в наличии (8 шт.)"), "show stock availability");

            VerifyFinally(testname);
        }

        [Test]
        public void ShowStockAvailabilityOff()
        {
            testname = "SettingsCatalogProductCartShowStockAvailabilityOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkNotSelected("ShowStockAvailability");

            GoToClient("products/ipd35");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".availability.available")).Text.Contains("Есть в наличии (8 шт.)"), "show stock availability");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class SettingsCatalogProductCartPhotoTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
            uploadImg();
        }

        [Test]
        public void EnableZoomOn()
        {
            testname = "SettingsCatalogProductCartEnableZoomOn";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkSelected("EnableZoom");

            GoToClient("products/ipd35");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector(".gallery-block")).FindElement(By.TagName("img")));
            a.Perform();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".zoomer-window")).Displayed, "enable zoom");

            VerifyFinally(testname);
        }

        [Test]
        public void EnableZoomOff()
        {
            testname = "SettingsCatalogProductCartEnableZoomOff";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            checkNotSelected("EnableZoom");

            GoToClient("products/ipd35");

            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector(".gallery-block")).FindElement(By.TagName("img")));
            a.Perform();

            VerifyIsFalse(driver.FindElements(By.CssSelector(".zoomer-window")).Count > 0, "enable zoom");

            VerifyFinally(testname);
        }

        public void uploadImg()
        {
            GoToAdmin("product/edit/35");
            driver.FindElement(By.XPath("//div[contains(text(), 'Фотографии')]")).Click();
            WaitForElem(By.CssSelector("[data-e2e=\"PhotoImg\"]"));
            Thread.Sleep(2000);
            Actions a = new Actions(driver);
            a.Build();
            a.MoveToElement(driver.FindElement(By.CssSelector("[data-e2e=\"PhotoImg\"]")));
            a.Perform();
            driver.FindElement(By.CssSelector(".product-block-state.clearfix")).FindElement(By.CssSelector("[data-e2e=\"PhotoItemDelete\"]")).Click();
            WaitForElem(By.ClassName("swal2-confirm"));
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);
        }

    }
    [TestFixture]
    public class SettingsCatalogProductCartReviewsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
            Functions.AdminSettingsReviewsImgUploadingOn(driver, baseURL);
            if
                   (!driver.FindElement(By.CssSelector("[data-e2e=\"displayReviewsImage\"]")).FindElement(By.Id("DisplayReviewsImage")).Selected)
            {
                var element = driver.FindElement(By.CssSelector("[data-e2e=\"EnableZoom\"]"));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"displayReviewsImage\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
                element = driver.FindElement(By.Id("header-top"));
                IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
                jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element);
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(4000);
            }
        }

        [Test]
        public void ReviewImageWidthHeight()
        {
            testname = "SettingsCatalogProductCartReviewImageWidthHeight";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");
            ScrollTo(By.Id("AllowReviews"));

            driver.FindElement(By.Id("ReviewImageWidth")).Click();
            driver.FindElement(By.Id("ReviewImageWidth")).Clear();
            driver.FindElement(By.Id("ReviewImageWidth")).SendKeys("300");

            driver.FindElement(By.Id("ReviewImageHeight")).Click();
            driver.FindElement(By.Id("ReviewImageHeight")).Clear();
            driver.FindElement(By.Id("ReviewImageHeight")).SendKeys("200");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");

            VerifyAreEqual("300", driver.FindElement(By.Id("ReviewImageWidth")).GetAttribute("value"), "review image width admin value");
            VerifyAreEqual("200", driver.FindElement(By.Id("ReviewImageHeight")).GetAttribute("value"), "review image height admin value");

            //upload img
            GoToClient("products/dress1#?tab=tabReviews");
            ScrollTo(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before"));

            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);

            driver.FindElement(By.Name("reviewsFormName")).Click();
            driver.FindElement(By.Name("reviewsFormName")).Clear();
            driver.FindElement(By.Name("reviewsFormName")).SendKeys("Review Name");

            driver.FindElement(By.Name("reviewsFormEmail")).Click();
            driver.FindElement(By.Name("reviewsFormEmail")).Clear();
            driver.FindElement(By.Name("reviewsFormEmail")).SendKeys("review@mail.test");

            driver.FindElement(By.Name("reviewFormText")).Click();
            driver.FindElement(By.Name("reviewFormText")).Clear();
            driver.FindElement(By.Name("reviewFormText")).SendKeys("Review Text");

            driver.FindElement(By.Name("reviewSubmit")).Click();
            Thread.Sleep(2000);

            //check client
            GoToClient("products/dress1#?tab=tabReviews");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".reviews")).FindElement(By.CssSelector(".review-item-image")).GetAttribute("style").Contains("min-width: 300px"), "uploaded img width");

            VerifyFinally(testname);
        }
    }
    [TestFixture]
    public class SettingsCatalogProductCartShippingsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();

            Init();
        }

        [Test]
        public void ShippingsInProductCartShowNever()
        {
            testname = "SettingsCatalogProductCartShippingsInDetailsShowNever";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            ScrollTo(By.Id("ReviewImageWidth"));
            (new SelectElement(driver.FindElement(By.Id("ShowShippingsMethodsInDetails")))).SelectByText("Никогда");
            ScrollTo(By.Id("header-top"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Enabled)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }
            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");
            IWebElement selectShippingsInDetails = driver.FindElement(By.Id("ShowShippingsMethodsInDetails"));
            SelectElement select = new SelectElement(selectShippingsInDetails);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Никогда"), "show shippings in product cart admin");

            //check client
            GoToClient("products/ipd35");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Доставка в") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Самовывоз") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Курьером"), "show shippings in product cart client");

            VerifyFinally(testname);
        }
        [Test]
        public void ShippingsInProductCartShowAlways()
        {
            testname = "SettingsCatalogProductCartShippingsInDetailsShowAlways";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            ScrollTo(By.Id("ReviewImageWidth"));
            (new SelectElement(driver.FindElement(By.Id("ShowShippingsMethodsInDetails")))).SelectByText("Всегда");
            ScrollTo(By.Id("header-top"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Enabled)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }
            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");
            IWebElement selectShippingsInDetails = driver.FindElement(By.Id("ShowShippingsMethodsInDetails"));
            SelectElement select = new SelectElement(selectShippingsInDetails);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Всегда"), "show shippings in product cart admin");

            //check client
            GoToClient("products/ipd35");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Доставка в") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Самовывоз") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Курьером"), "show shippings in product cart client");

            VerifyFinally(testname);
        }

        [Test]
        public void ShippingsInProductCartShowByClick()
        {
            testname = "SettingsCatalogProductCartShippingsInDetailsShowByClick";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            ScrollTo(By.Id("ReviewImageWidth"));
            (new SelectElement(driver.FindElement(By.Id("ShowShippingsMethodsInDetails")))).SelectByText("По клику");
            ScrollTo(By.Id("header-top"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Enabled)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }
            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");
            IWebElement selectShippingsInDetails = driver.FindElement(By.Id("ShowShippingsMethodsInDetails"));
            SelectElement select = new SelectElement(selectShippingsInDetails);
            VerifyIsTrue(select.SelectedOption.Text.Contains("По клику"), "show shippings in product cart admin");

            //check client
            GoToClient("products/ipd35");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Доставка в") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Самовывоз") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Курьером"), "show shippings in product cart client by click");
            driver.FindElement(By.LinkText("Рассчитать стоимость доставки")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Доставка в") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Самовывоз") && driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).Text.Contains("Курьером"), "show shippings in product cart client");

            VerifyFinally(testname);
        }

        [Test]
        public void ShippingzInProductCartCount()
        {
            testname = "SettingsCatalogProductCartShippingsInDetailsCount";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");
            ScrollTo(By.Id("ReviewImageWidth"));
            (new SelectElement(driver.FindElement(By.Id("ShowShippingsMethodsInDetails")))).SelectByText("Всегда");
            driver.FindElement(By.Id("ShippingsMethodsInDetailsCount")).Click();
            driver.FindElement(By.Id("ShippingsMethodsInDetailsCount")).Clear();
            driver.FindElement(By.Id("ShippingsMethodsInDetailsCount")).SendKeys("1");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admins
            GoToAdmin("settingscatalog#?catalogTab=product");

            VerifyAreEqual("1", driver.FindElement(By.Id("ShippingsMethodsInDetailsCount")).GetAttribute("value"), "shippings count in product cart admin value");

            //check client
            GoToClient("products/ipd35");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".cs-bg-4.block-exuding")).FindElements(By.CssSelector(".shipping-variants-row")).Count == 1, "shippings count in product cart client");

            VerifyFinally(testname);
        }

    }
    [TestFixture]
    public class SettingsCatalogProductCartMarketingTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(

            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.Product.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.Offer.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.Category.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.ProductCategories.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.PropertyGroup.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.Property.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.PropertyValue.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.ProductPropertyValue.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.PropertyGroupCategory.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.RelatedCategories.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.RelatedProducts.csv",
            "data\\Admin\\Settings\\SettingsCatalog\\ProductCart\\Catalog.RelatedPropertyValues.csv"

           );

            Init();
        }

        [Test]
        public void RelatedProductName()
        {
            testname = "SettingsCatalogProductCartRelatedProductName";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            ScrollTo(By.Id("ReviewImageHeight"));
            driver.FindElement(By.Id("RelatedProductName")).Click();
            driver.FindElement(By.Id("RelatedProductName")).Clear();
            driver.FindElement(By.Id("RelatedProductName")).SendKeys("RelatedProductName Test");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");

            VerifyAreEqual("RelatedProductName Test", driver.FindElement(By.Id("RelatedProductName")).GetAttribute("value"), "related product name admin value");

            //check client
            GoToClient("products/test-product1");

            VerifyIsTrue(driver.PageSource.Contains("RelatedProductName Test"), "related product name client");

            VerifyFinally(testname);
        }

        [Test]
        public void AlternativeProductName()
        {
            testname = "SettingsCatalogProductCartAlternativeProductName";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            ScrollTo(By.Id("ReviewImageHeight"));
            driver.FindElement(By.Id("AlternativeProductName")).Click();
            driver.FindElement(By.Id("AlternativeProductName")).Clear();
            driver.FindElement(By.Id("AlternativeProductName")).SendKeys("AlternativeProductName Test");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");

            VerifyAreEqual("AlternativeProductName Test", driver.FindElement(By.Id("AlternativeProductName")).GetAttribute("value"), "alternative product name admin value");

            //check client
            GoToClient("products/test-product1");

            VerifyIsTrue(driver.PageSource.Contains("AlternativeProductName Test"), "alternative product name client");

            VerifyFinally(testname);
        }

        [Test]
        public void RelatedProductSourceTypeFromProduct()
        {
            testname = "SettingsCatalogRelatedProductSourceFromProduct";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            string relatedProductName = driver.FindElement(By.Id("RelatedProductName")).GetAttribute("value");
            string alternativeProductName = driver.FindElement(By.Id("AlternativeProductName")).GetAttribute("value");

            ScrollTo(By.Id("ReviewImageHeight"));
            (new SelectElement(driver.FindElement(By.Id("RelatedProductSourceType")))).SelectByText("Из списка назначенных товаров");
            ScrollTo(By.Id("header-top"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Enabled)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");
            IWebElement selectRelatedProductSourceType = driver.FindElement(By.Id("RelatedProductSourceType"));
            SelectElement select = new SelectElement(selectRelatedProductSourceType);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Из списка назначенных товаров"), "related products source type admin");

            //check client
            GoToClient("products/test-product2");

            VerifyIsFalse(driver.PageSource.Contains(relatedProductName) && driver.PageSource.Contains(alternativeProductName), "related products source type from product not show");

            GoToClient("products/test-product1");

            VerifyIsTrue(driver.PageSource.Contains(relatedProductName) && driver.PageSource.Contains(alternativeProductName), "related products source type from product show");

            VerifyFinally(testname);
        }

        [Test]
        public void RelatedProductSourceTypeFromCategory()
        {
            testname = "SettingsCatalogRelatedProductSourceFromCategory";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            string relatedProductName = driver.FindElement(By.Id("RelatedProductName")).GetAttribute("value");
            string alternativeProductName = driver.FindElement(By.Id("AlternativeProductName")).GetAttribute("value");

            ScrollTo(By.Id("ReviewImageHeight"));
            (new SelectElement(driver.FindElement(By.Id("RelatedProductSourceType")))).SelectByText("Из назначенной категории");
            ScrollTo(By.Id("header-top"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Enabled)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
                Thread.Sleep(2000);
            }

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");
            IWebElement selectRelatedProductSourceType = driver.FindElement(By.Id("RelatedProductSourceType"));
            SelectElement select = new SelectElement(selectRelatedProductSourceType);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Из назначенной категории"), "related products source type admin");

            //check client
            GoToClient("products/test-product2");

            VerifyIsTrue(driver.PageSource.Contains(relatedProductName) && driver.PageSource.Contains(alternativeProductName), "related products source type from category show");

            GoToClient("products/test-product3");

            VerifyIsFalse(driver.PageSource.Contains(relatedProductName) && driver.PageSource.Contains(alternativeProductName), "related products source type from category not show");

            VerifyFinally(testname);
        }

        [Test]
        public void RelatedProductsMaxCount()
        {
            testname = "SettingsCatalogProductCartRelatedProductsMaxCount";
            VerifyBegin(testname);

            GoToAdmin("settingscatalog#?catalogTab=product");

            ScrollTo(By.Id("ReviewImageHeight"));
            driver.FindElement(By.Id("RelatedProductsMaxCount")).Click();
            driver.FindElement(By.Id("RelatedProductsMaxCount")).Clear();
            driver.FindElement(By.Id("RelatedProductsMaxCount")).SendKeys("1");

            (new SelectElement(driver.FindElement(By.Id("RelatedProductSourceType")))).SelectByText("Из назначенной категории");
 
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCatalogSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingscatalog#?catalogTab=product");

            VerifyAreEqual("1", driver.FindElement(By.Id("RelatedProductsMaxCount")).GetAttribute("value"), "related products max count admin value");

            //check client
            GoToClient("products/test-product2");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view.products-view-tile"))[0].FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Count == 1, "related products max count client");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view.products-view-tile"))[1].FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Count == 1, "alternative products max count client");

            VerifyFinally(testname);
        }
    }
}