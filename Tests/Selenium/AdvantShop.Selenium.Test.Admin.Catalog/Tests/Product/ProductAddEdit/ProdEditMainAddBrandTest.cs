using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.BrandAddEdit
{
    [TestFixture]
    public class ProductAddEditMainAddBrandTest : BaseSeleniumTest
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
        public void ProductEditAddFirstBrand()
        {
          GoToAdmin("product/edit/1");
            Assert.AreEqual("Не выбран", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count > 0);
            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Производители", driver.FindElement(By.TagName("h2")).Text);
            GetGridCell(0, "BrandName", "Brands").FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check admin
            Assert.AreEqual("BrandName1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //check client
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.PageSource.Contains("BrandName1"));
        }

        [Test]
        public void ProductEditAddBrandviaSearch()
        {
            GoToAdmin("product/edit/4");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);
            GetGridFilter().Click();
            GetGridFilter().SendKeys("BrandName10");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("BrandName10", GetGridCell(0, "BrandName", "Brands").Text);
            GetGridCell(0, "BrandName", "Brands").FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check admin
            Assert.AreEqual("BrandName10", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);

            //check client
            GoToClient("products/test-product4");
            Assert.IsTrue(driver.PageSource.Contains("BrandName10"));
        }

        [Test]
        public void ProductEditAddBrandUsingPage()
        {
            GoToAdmin("product/edit/5");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            GetGridCell(0, "BrandName", "Brands").FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check admin
            Assert.AreEqual("BrandName22", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);

            //check client
            GoToClient("products/test-product5");
            Assert.IsTrue(driver.PageSource.Contains("BrandName22"));
        }

        [Test]
        public void ProductEditAddDisabledBrand()
        {
            GoToAdmin("product/edit/6");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);
            GetGridFilter().Click();
            GetGridFilter().SendKeys("BrandName3");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("BrandName3", GetGridCell(0, "BrandName", "Brands").Text);
            GetGridCell(0, "BrandName", "Brands").FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check admin
            Assert.AreEqual("BrandName3", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);

            //check client
            GoToClient("products/test-product6");
            Assert.IsFalse(driver.PageSource.Contains("BrandName3"));
        }

        [Test]
        public void ProductEditAddBrandElse()
        {
            GoToAdmin("product/edit/7");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);
            GetGridCell(0, "BrandName", "Brands").FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check admin
            Assert.AreEqual("BrandName1", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //choose another brand
            driver.FindElement(By.CssSelector("[data-e2e=\"AddBrand\"]")).Click();
            Thread.Sleep(1000);
            GetGridFilter().Click();
            GetGridFilter().SendKeys("BrandName60");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("BrandName60", GetGridCell(0, "BrandName", "Brands").Text);
            GetGridCell(0, "BrandName", "Brands").FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            
            //check admin
            GoToAdmin("product/edit/7");
            Assert.AreEqual("BrandName60", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //check client
            GoToClient("products/test-product7");
            Assert.IsTrue(driver.PageSource.Contains("BrandName60"));
        }

        [Test]
        public void ProductEditAddBrandDelete()
        {
            GoToAdmin("product/edit/53");

            //check admin
            Assert.AreEqual("BrandName12", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //check delete brand
            driver.FindElement(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("product/edit/53");
            Assert.AreEqual("Не выбран", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count > 0);

            //check brand grid
            GoToAdmin("brands");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("BrandName12");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName12", GetGridCell(0, "BrandName").Text);

            //check client
            GoToClient("products/test-product53");
            Assert.IsFalse(driver.PageSource.Contains("BrandName12"));
        }

        [Test]
        public void ProductzEditAddBrandDeleteFromGrid()
        {
            GoToAdmin("product/edit/50");

            Assert.AreEqual("BrandName10", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //delete brand from grid
            GoToAdmin("brands");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("BrandName10");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName10", GetGridCell(0, "BrandName").Text);
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/50");
            Assert.AreEqual("Не выбран", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count > 0);

            //check client
            GoToClient("products/test-product50");
            Assert.IsFalse(driver.PageSource.Contains("BrandName10"));
        }

        [Test]
        public void ProductEditAddBrandToDisabled()
        {
            GoToAdmin("product/edit/51");

            Assert.AreEqual("BrandName11", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //brand do disabled
            GoToAdmin("brands");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("BrandName11");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName11", GetGridCell(0, "BrandName").Text);
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("product/edit/51");
            Assert.AreEqual("BrandName11", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //check client
            GoToClient("products/test-product51");
            Assert.IsFalse(driver.PageSource.Contains("BrandName11"));
        }

        [Test]
        public void ProductEditAddBrandToEnabled()
        {
            GoToAdmin("product/edit/52");

            Assert.AreEqual("BrandName2", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //brand do disabled
            GoToAdmin("brands");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("BrandName2");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("BrandName2", GetGridCell(0, "BrandName").Text);
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("product/edit/52");
            Assert.AreEqual("BrandName2", driver.FindElement(By.CssSelector("[data-e2e=\"ProductBrandName\"]")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"DeleteBrand\"]")).Count == 1);

            //check client
            GoToClient("products/test-product52");
            Assert.IsTrue(driver.PageSource.Contains("BrandName2"));
        }
    }
}