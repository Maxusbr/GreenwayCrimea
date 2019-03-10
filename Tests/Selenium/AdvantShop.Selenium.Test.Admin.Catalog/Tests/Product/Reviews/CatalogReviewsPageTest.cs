using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Catalog.Reviews
{
    [TestFixture]
    public class CatalogReviewsPageTest : BaseSeleniumTest
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
        }
     
        [Test]
        public void Page()
        {
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
    }
}