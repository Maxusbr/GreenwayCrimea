using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Catalog.Reviews
{
    [TestFixture]
    public class CatalogReviewsFilterAddDatePageTest : BaseSeleniumTest
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

            GoToAdmin("reviews");

            Functions.GridFilterSet(driver, baseURL, name: "AddDateFormatted");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2013 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.01.2013 00:00");
            DropFocus("h1");
            WaitForAjax();
        }
        
        [Test]
        public void Page()
        {
            Assert.AreEqual("Текст отзыва 258", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 249", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("Текст отзыва 248", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 239", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("Текст отзыва 238", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 229", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Assert.AreEqual("Текст отзыва 228", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 219", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Assert.AreEqual("Текст отзыва 218", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 209", GetGridCell(9, "Text").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Текст отзыва 258", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 249", GetGridCell(9, "Text").Text);

        }

        [Test]
        public void PageToPrevious()
        {
            Assert.AreEqual("Текст отзыва 258", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 249", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Текст отзыва 248", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 239", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Текст отзыва 238", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 229", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Текст отзыва 248", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 239", GetGridCell(9, "Text").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Текст отзыва 258", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 249", GetGridCell(9, "Text").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("Текст отзыва 181", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 190", GetGridCell(9, "Text").Text);
        }
    }
}