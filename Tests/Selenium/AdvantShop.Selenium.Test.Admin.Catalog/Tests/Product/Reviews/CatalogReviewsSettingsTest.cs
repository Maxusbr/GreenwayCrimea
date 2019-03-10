using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Catalog.Reviews
{
    [TestFixture]
    public class CatalogReviewsSettingsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.CMS);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\Reviews\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Reviews\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Reviews\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Reviews\\Catalog.ProductCategories.csv",
                    "data\\Admin\\Catalog\\Reviews\\Customers.Customer.csv",
           "data\\Admin\\Catalog\\Reviews\\Customers.CustomerGroup.csv",
            "data\\Admin\\Catalog\\Reviews\\CMS.Review.csv"
           );

            Init();
        }

        [Test]
        public void AddCheckedWithImg()
        {
            Functions.AdminSettingsReviewsShowImgOn(driver, baseURL);

            GoToAdmin("reviews");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Отзыв", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).SendKeys("50");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("CustomerName");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("ReviewEmail@gmail.com");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("Хороший товар и не дорогой, спасибо");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).SendKeys("18.11.2016 11:03:23");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ReviewCheckedSelected\"]")).Selected);
            //  Assert.IsTrue(driver.PageSource.Contains("Файл не выбран"));
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).Clear();
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("icon.jpg"));
            // Assert.IsTrue(driver.PageSource.Contains("icon.jpg"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Хороший товар и не дорогой, спасибо");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("ReviewEmail@gmail.com"));
            Assert.AreEqual("Хороший товар и не дорогой, спасибо", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("18.11.2016 11:03:23", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsTrue(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product50");
            Assert.IsTrue(driver.PageSource.Contains("CustomerName"));
            Assert.IsTrue(driver.PageSource.Contains("Отзывы (1)"));
            Assert.IsTrue(driver.PageSource.Contains("Хороший товар и не дорогой, спасибо"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".reviews")).FindElements(By.CssSelector(".review-item-image")).Count == 1);
            Assert.IsFalse(driver.FindElement(By.CssSelector(".reviews")).FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
        }


        [Test]
        public void AddNotCheckedWithoutImg()
        {
            Functions.AdminSettingsReviewsShowImgOn(driver, baseURL);

            GoToAdmin("reviews");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).SendKeys("11");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("Customer");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("ReviewEmail1@gmail.com");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("Все понравилось");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).SendKeys("18.11.2016 11:03:23");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewCheckedClick\"]")).Click();
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Все понравилось");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("Customer"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("ReviewEmail1@gmail.com"));
            Assert.AreEqual("Все понравилось", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct11", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("18.11.2016 11:03:23", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product11");
            Assert.IsTrue(driver.PageSource.Contains("Customer"));
            Assert.IsTrue(driver.PageSource.Contains("Отзывы (1)"));
            Assert.IsTrue(driver.PageSource.Contains("Все понравилось"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".reviews")).FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(driver.PageSource.Contains("18 ноября 2016"));
        }


        [Test]
        public void AddCheckedWithImgShowOff()
        {
            Functions.AdminSettingsReviewsShowImgOff(driver, baseURL);

            GoToAdmin("reviews");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).SendKeys("12");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("CustomerName");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("ReviewEmail@gmail.com");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("ура");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).SendKeys("18.11.2016 11:03:23");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ReviewCheckedSelected\"]")).Selected);
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).Clear();
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("icon.jpg"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("ура");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("ReviewEmail@gmail.com"));
            Assert.AreEqual("ура", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct12", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("18.11.2016 11:03:23", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsTrue(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product12");
            Assert.IsTrue(driver.PageSource.Contains("CustomerName"));
            Assert.IsTrue(driver.PageSource.Contains("Отзывы (1)"));
            Assert.IsTrue(driver.PageSource.Contains("ура"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".reviews")).FindElements(By.CssSelector(".review-item-image")).Count > 0);
        }

        [Test]
        public void AddNotCheckedModerateOn()
        {
            Functions.AdminSettingsReviewsModerateOn(driver, baseURL);

            GoToAdmin("reviews");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).SendKeys("13");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("CustomerName");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("ReviewEmail@gmail.com");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("тестовый отзыв");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).SendKeys("18.11.2016 11:03:23");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewCheckedClick\"]")).Click();

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("тестовый отзыв");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("ReviewEmail@gmail.com"));
            Assert.AreEqual("тестовый отзыв", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct13", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("18.11.2016 11:03:23", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product13");
            Assert.IsFalse(driver.PageSource.Contains("CustomerName"));
            Assert.IsFalse(driver.PageSource.Contains("Отзывы (1)"));
            Assert.IsFalse(driver.PageSource.Contains("тестовый отзыв"));
        }

        [Test]
        public void AddCheckedModerateOn()
        {
            Functions.AdminSettingsReviewsModerateOn(driver, baseURL);

            GoToAdmin("reviews");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).SendKeys("14");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("CustomerName");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("ReviewEmail@gmail.com");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("закажем еще");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).SendKeys("18.11.2016 11:03:23");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("закажем еще");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("ReviewEmail@gmail.com"));
            Assert.AreEqual("закажем еще", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct14", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("18.11.2016 11:03:23", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsTrue(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product14");
            Assert.IsTrue(driver.PageSource.Contains("CustomerName"));
            Assert.IsTrue(driver.PageSource.Contains("Отзывы (1)"));
            Assert.IsTrue(driver.PageSource.Contains("закажем еще"));
        }

        [Test]
        public void AddNotCheckedModerateOff()
        {
            Functions.AdminSettingsReviewsModerateOff(driver, baseURL);

            GoToAdmin("reviews");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).SendKeys("15");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("CustomerName");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("ReviewEmail@gmail.com");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("супер отзыв");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewDateAdd\"]")).SendKeys("18.11.2016 11:03:23");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewCheckedClick\"]")).Click();

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("reviews");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("супер отзыв");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName"));
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("ReviewEmail@gmail.com"));
            Assert.AreEqual("супер отзыв", GetGridCell(0, "Text").Text);
            Assert.AreEqual("TestProduct15", GetGridCell(0, "ProductName").Text);
            Assert.AreEqual("18.11.2016 11:03:23", GetGridCell(0, "AddDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "Checked").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product15");
            Assert.IsTrue(driver.PageSource.Contains("CustomerName"));
            Assert.IsTrue(driver.PageSource.Contains("Отзывы (1)"));
            Assert.IsTrue(driver.PageSource.Contains("супер отзыв"));
        }

        [Test]
        public void AddClientImgUpLoadimgOn()
        {
            Functions.AdminSettingsReviewsImgUploadingOn(driver, baseURL);

            GoToClient("products/test-product1#?tab=tabReviews");

            ScrollTo(By.CssSelector(".review-form-block"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".review-form-block")).Text.Contains("Выберите файл"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".review-form-block")).Text.Contains("Изображение"));
        }

        [Test]
        public void AddClientImgUpLoadimgOff()
        {
            Functions.AdminSettingsReviewsImgUploadingOff(driver, baseURL);

            GoToClient("products/test-product1#?tab=tabReviews");

            ScrollTo(By.CssSelector(".review-form-block"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".review-form-block")).Text.Contains("Выберите файл"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".review-form-block")).Text.Contains("Изображение"));
        }

        [Test]
        public void AllowReviewsOn()
        {
            Functions.AdminSettingsReviewsOn(driver, baseURL);

            GoToClient("products/test-product1");
            
            Assert.IsTrue(driver.PageSource.Contains("Отзывы"));
        }

        [Test]
        public void AllowReviewsOff()
        {
            Functions.AdminSettingsReviewsOff(driver, baseURL);

            GoToClient("products/test-product1");

            Assert.IsFalse(driver.PageSource.Contains("Отзывы"));
        }
    }
}