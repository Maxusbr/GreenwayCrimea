using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Brand
{
    [TestFixture]
    public class ProductAddEditMainBrandAddPageViewSort : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Tag.csv",
             "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductPropertyValue.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyGroup.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Color.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Size.csv"
           );

            Init();
        }
        
        [Test]
        public void BrandAddView()
        {
            GoToAdmin("product/edit/1");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName12", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName21", GetGridCell(19, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName49", GetGridCell(49, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName94", GetGridCell(99, "BrandName", "Brands").Text);
        }
     
        [Test]
        public void BrandAddSort()
        {
            GoToAdmin("product/edit/1");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(2000);

            //check sort by brand name
            GetGridCell(-1, "BrandName", "Brands").Click();
            WaitForAjax();
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName12", GetGridCell(9, "BrandName", "Brands").Text);

            GetGridCell(-1, "BrandName", "Brands").Click();
            WaitForAjax();
            Assert.AreEqual("BrandName99", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName90", GetGridCell(9, "BrandName", "Brands").Text);

            //check sort by products count
            GetGridCell(-1, "ProductsCount", "Brands").Click();
            WaitForAjax();
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName40", GetGridCell(9, "BrandName", "Brands").Text);
            Assert.AreEqual("0", GetGridCell(0, "ProductsCount", "Brands").Text);
            Assert.AreEqual("0", GetGridCell(9, "ProductsCount", "Brands").Text);

            GetGridCell(-1, "ProductsCount", "Brands").Click();
            WaitForAjax();
            Assert.AreEqual("BrandName13", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName", "Brands").Text);
            Assert.AreEqual("8", GetGridCell(0, "ProductsCount", "Brands").Text);
            Assert.AreEqual("1", GetGridCell(9, "ProductsCount", "Brands").Text);
        }


        [Test]
        public void BrandAddPage()
        {
            GoToAdmin("product/edit/1");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName12", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName13", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName21", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName22", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName31", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName4", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName40", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName49", GetGridCell(9, "BrandName", "Brands").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName12", GetGridCell(9, "BrandName", "Brands").Text);
            
        }

        [Test]
        public void BrandAddPageToPrevious()
        {
            GoToAdmin("product/edit/1");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);

            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName12", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName13", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName21", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName22", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName30", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName13", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName21", GetGridCell(9, "BrandName", "Brands").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName12", GetGridCell(9, "BrandName", "Brands").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName95", GetGridCell(0, "BrandName", "Brands").Text);
            Assert.AreEqual("BrandName99", GetGridCell(4, "BrandName", "Brands").Text);
        }
    }
}