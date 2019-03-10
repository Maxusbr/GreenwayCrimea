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
    public class BrandFilterPhotoPageTest : BaseSeleniumTest
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
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Photo.csv"
           );

            Init();
        }

        [Test]
        public void BrandFilterPhotoPageBrand()
        {
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName80", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName81", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName90", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName131", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName140", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName141", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName150", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName151", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName160", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterPhotoPageBrandToBegin()
        {
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName80", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName81", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName90", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName131", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName140", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName141", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName150", GetGridCell(9, "BrandName").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName80", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterPhotoPageBrandToEnd()
        {
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName80", GetGridCell(9, "BrandName").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName191", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName200", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterPhotoPageBrandToNext()
        {
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName80", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName81", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName90", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName131", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName140", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName141", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName150", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterPhotoPageBrandToPrevious()
        {
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName80", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName81", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName90", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName81", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName90", GetGridCell(9, "BrandName").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName80", GetGridCell(9, "BrandName").Text);
        }
    }
}