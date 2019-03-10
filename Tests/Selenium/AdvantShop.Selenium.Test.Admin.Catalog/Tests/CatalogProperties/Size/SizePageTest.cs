using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Catalog.Sizes
{
    [TestFixture]
    public class SizePageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Size.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }

        [Test]
        public void Page()
        {
            GoToAdmin("sizes");
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("SizeName11", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName20", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("SizeName21", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName30", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Assert.AreEqual("SizeName31", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName40", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Assert.AreEqual("SizeName41", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName50", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Assert.AreEqual("SizeName51", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName60", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void PageToBegin()
        {
            GoToAdmin("sizes");
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName11", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName20", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName21", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName30", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName31", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName40", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName41", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName50", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

        }

        [Test]
        public void PageToEnd()
        {
            GoToAdmin("sizes");
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("SizeName191", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName200", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void PageToNext()
        {
            GoToAdmin("sizes");
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName11", GetGridCell(0, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName20", GetGridCell(9, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName21", GetGridCell(0, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName30", GetGridCell(9, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName31", GetGridCell(0, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName40", GetGridCell(9, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName41", GetGridCell(0, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName50", GetGridCell(9, "SizeName").FindElement(By.Name("inputForm")).FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void PageToPrevious()
        {
            GoToAdmin("sizes");
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName11", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName20", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("SizeName21", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName30", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("SizeName11", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName20", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
        }

    }
}