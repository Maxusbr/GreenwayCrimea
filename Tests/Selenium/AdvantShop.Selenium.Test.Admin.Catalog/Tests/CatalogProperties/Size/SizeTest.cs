using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Catalog.Sizes
{
    [TestFixture]
    public class SizeCatalogAdminTest : BaseSeleniumTest
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
        public void SizeAPresent()
        {
            GoToAdmin("sizes");
            Assert.AreEqual("Размеры", driver.FindElement(By.TagName("h1")).Text);
            GetGridFilter().SendKeys("SizeName197");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("4", GetGridCell(0, "ProductsCount").Text);

            GoToAdmin("sizes");

            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName20", GetGridCell(19, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName50", GetGridCell(49, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName100", GetGridCell(99, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void SizeSearch()
        {
            GoToAdmin("sizes");

            //check search exist size
            GetGridFilter().SendKeys("SizeName26");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("SizeName26", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check search not exist size
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("SizeName365");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        
        [Test]
        public void SizeSelectAndDelete()
        {
            GoToAdmin("sizes");

            //check delete size in use
            gridReturnDefaultView10();
            GetGridFilter().SendKeys("SizeName101");
            driver.FindElement(By.CssSelector(".link-disabled.ui-grid-custom-service-icon.fa.fa-times")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Удаление невозможно", driver.FindElement(By.TagName("h2")).Text);
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("sizes");
            GetGridFilter().SendKeys("SizeName101");
            Assert.AreEqual("SizeName101", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            GoToAdmin("sizes");

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("3", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreNotEqual("SizeName2", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreNotEqual("SizeName3", GetGridCell(1, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreNotEqual("SizeName4", GetGridCell(2, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("SizeName15", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName24", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("186", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("sizes");
            Assert.AreEqual("SizeName101", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        
        [Test]
        public void SizeInplaceEditAndFilter()
        {
            GoToAdmin("sizes");

            //inplace
            GetGridFilter().SendKeys("SizeName99");
            DropFocus("h1");
            WaitForAjax();

            GetGridCell(0, "SizeName").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SizeName").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SizeName").FindElement(By.TagName("input")).SendKeys("SizeX");

            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).SendKeys("500");
            DropFocus("h1");

            //check
            GoToAdmin("sizes");
            GetGridFilter().SendKeys("SizeX");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("SizeX", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("500", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //name filter 
            GoToAdmin("sizes");
            Functions.GridFilterSet(driver, baseURL, "SizeName");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("SizeName10");
            DropFocus("h1");
            Assert.AreEqual("SizeName10", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName100", GetGridCell(1, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName108", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));

            //close filter
            Functions.GridFilterClose(driver, baseURL, "SizeName");
            Assert.AreEqual("SizeName1", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("SizeName10", GetGridCell(9, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
         }
    }
}