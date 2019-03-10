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
    public class BrandPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\Brands\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Brands\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Brands\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Brands\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\Brands\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }
        
        [Test]
        public void PageBrand()
        {
            GoToAdmin("brands");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4)")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5)")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6)")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName40", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7)")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName41", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7)")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName51", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName60", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void PageBrandToBegin()
        {
            GoToAdmin("brands");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName40", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName41", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(9, "BrandName").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void PageBrandToEnd()
        {
            GoToAdmin("brands");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName105", GetGridCell(4, "BrandName").Text);
        }

        [Test]
        public void PageBrandToNext()
        {
            GoToAdmin("brands");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName40", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName41", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void PageBrandToPrevious()
        {
            GoToAdmin("brands");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName21", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
        }
  
    }
}