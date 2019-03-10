using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Catalog.Reviews
{
    [TestFixture]
    public class CatalogReviewsFilterNameTest : BaseSeleniumTest
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
        public void FilterName()
        {
            GoToAdmin("reviews");

            //search by exist name 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnName");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("CustomerName2");
            DropFocus("h1");
            Refresh();

            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("CustomerName2"));
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Найдено записей: 196", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 20", GetGridCell(9, "Text").Text);

            PageSelectItems("20");
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 10", GetGridCell(19, "Text").Text);

            PageSelectItems("50");
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 192", GetGridCell(49, "Text").Text);

            PageSelectItems("100");
            Assert.AreEqual("Текст отзыва 30", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 50", GetGridCell(99, "Text").Text);

            //search by not exist name
            GoToAdmin("reviews");

            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnName");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("CustomerName2456");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnName");
            Assert.AreEqual("Найдено записей: 300", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
        
        [Test]
        public void Page()
        {
            GoToAdmin("reviews");

            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnName");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("CustomerName2");
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
            Assert.AreEqual("Текст отзыва 192", GetGridCell(9, "Text").Text);

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
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnName");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("CustomerName2");
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
            Assert.AreEqual("Текст отзыва 188", GetGridCell(0, "Text").Text);
            Assert.AreEqual("Текст отзыва 5", GetGridCell(5, "Text").Text);
        }

        [Test]
        public void zDelete()
        {
            GoToAdmin("reviews");

            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnName");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("CustomerName2");
            DropFocus("h1");
            Refresh();
            Assert.AreEqual("Найдено записей: 196", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnName");

            Assert.AreEqual("Найдено записей: 104", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            GoToAdmin("reviews");

            Assert.AreEqual("Найдено записей: 104", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
    }
}