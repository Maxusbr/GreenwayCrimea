using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Catalog.Reviews
{
    [TestFixture]
    public class CatalogReviewsFilterImgTest : BaseSeleniumTest
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
           "data\\Admin\\Catalog\\Reviews\\Catalog.Photo.csv",
                    "data\\Admin\\Catalog\\Reviews\\Customers.Customer.csv",
           "data\\Admin\\Catalog\\Reviews\\Customers.CustomerGroup.csv",
            "data\\Admin\\Catalog\\Reviews\\CMS.Review.csv"
           );

            Init();
        }

        [Test]
        public void FilterImg()
        {
            GoToAdmin("reviews");

            //check without img 
            Functions.GridFilterSet(driver, baseURL, name: "PhotoName");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            Refresh();
            Assert.AreEqual("Найдено записей: 200", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.IsTrue(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(9, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));

            //check with img 
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фотографией");
            DropFocus("h1");
            Refresh();
            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            Assert.AreEqual("Текст отзыва 200", GetGridCell(0, "Text").Text);
            Assert.IsFalse(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(GetGridCell(9, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));

            PageSelectItems("20");
            Assert.IsFalse(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(GetGridCell(19, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));

            PageSelectItems("50");
            Assert.IsFalse(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(GetGridCell(49, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));

            PageSelectItems("100");
            Assert.IsFalse(GetGridCell(0, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(GetGridCell(99, "PhotoName").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "PhotoName");
            Assert.AreEqual("Найдено записей: 300", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

        }
        
        [Test]
        public void Page()
        {
            GoToAdmin("reviews");

            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, name: "PhotoName");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            Blur();

            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 20", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("Текст отзыва 21", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 10", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("Текст отзыва 11", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 45", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Assert.AreEqual("Текст отзыва 40", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 35", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Assert.AreEqual("Текст отзыва 32", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 250", GetGridCell(9, "Text").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 20", GetGridCell(9, "Text").Text);

        }

        [Test]
        public void PageToPrevious()
        {
            GoToAdmin("reviews");

            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, name: "PhotoName");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            Blur();

            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 20", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Текст отзыва 21", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 10", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Текст отзыва 11", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 45", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Текст отзыва 21", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 10", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 20", GetGridCell(9, "Text").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("Текст отзыва 291", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 300", GetGridCell(9, "Text").Text);
        }
        
        [Test]
        public void zDelete()
        {
            GoToAdmin("reviews");

            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, name: "PhotoName");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            Refresh();

            Assert.AreEqual("Найдено записей: 200", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Functions.GridFilterClose(driver, baseURL, name: "PhotoName");

            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            GoToAdmin("reviews");

            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
    }
}