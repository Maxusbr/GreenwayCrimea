using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Brand
{
    [TestFixture]
    public class BrandFilterActivityPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.ProductCategories.csv"
           );
            Init();
        }
    
        [Test]
        public void BrandFilterActivityOffPage()
        {
            GoToAdmin("brands");
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h1");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName40", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName41", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName51", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName60", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterActivityOffPageToBegin()
        {
            GoToAdmin("brands");
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h1");

            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName40", GetGridCell(9, "BrandName").Text);


            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName41", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(9, "BrandName").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

        }

        [Test]
        public void BrandFilterActivityOffPageToEnd()
        {
            GoToAdmin("brands");
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h1");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterActivityOffPageToNext()
        {
            GoToAdmin("brands");
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h1");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName40", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName41", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterActivityOffPageToPrevious()
        {
            GoToAdmin("brands");
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h1");

            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
        }

    }
}