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
    public class BrandTest : BaseSeleniumTest
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
        public void ABrandSort()
        {
            GoToAdmin("brands");

            //SortByName
            GetGridCell(-1, "BrandName").Click();
            WaitForAjax();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName12", GetGridCell(9, "BrandName").Text);
            GetGridCell(-1, "BrandName").Click();
            WaitForAjax();
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName99", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName90", GetGridCell(9, "BrandName").Text);

            //SortByCountry
            GetGridCell(-1, "CountryName").Click();
            WaitForAjax();
            Thread.Sleep(1000);
            Assert.AreEqual("Реюньон", GetGridCell(0, "CountryName").Text);
            GetGridCell(-1, "CountryName").Click();
            WaitForAjax();
            Thread.Sleep(1000);
            Assert.AreEqual("Япония", GetGridCell(0, "CountryName").Text);

            //SortByOrder
            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName105", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("105", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //SortByActivity
            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName6", GetGridCell(0, "BrandName").Text);
        }

        [Test]
        public void BrandPresent()
        {
            GoToAdmin("brands");
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("Найдено записей: 105", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(19, "BrandName").Text);

            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(49, "BrandName").Text);

            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(99, "BrandName").Text);
        }
      
        [Test]
        public void BrandSearchAndDoToEdit()
        {
            GoToAdmin("brands");

            //search exist product
            GetGridFilter().SendKeys("BrandName5");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("BrandName5", GetGridCell(0, "BrandName").Text);

            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Brand5");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            
            //go to edit
            GoToAdmin("brands");
            GetGridCell(0, "BrandName").Click();
            Thread.Sleep(4000);
            Assert.AreEqual("Производитель \"BrandName1\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("BrandName1", driver.FindElement(By.Id("BrandName")).GetAttribute("value"));
            Assert.IsTrue(driver.Url.Contains("brands/edit/1"));

        }
      
        [Test]
        public void BrandSelectAndDelete()
        {
            GoToAdmin("brands");
            Functions.GridPaginationSelect10(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            //check delete cancel
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("BrandName1", GetGridCell(0, "BrandName").Text);

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
            Assert.AreNotEqual("BrandName2", GetGridCell(0, "BrandName").Text);
            Assert.AreNotEqual("BrandName3", GetGridCell(1, "BrandName").Text);
            Assert.AreNotEqual("BrandName4", GetGridCell(2, "BrandName").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("BrandName15", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName24", GetGridCell(9, "BrandName").Text);

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("91", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

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
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            GoToAdmin("brands");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
      
        [Test]
        public void ABrandCheckSort()
        {
            GoToAdmin("brands");
            Assert.AreEqual("BrandName7", GetGridCell(6, "BrandName").Text);
            Assert.AreEqual("7", GetGridCell(6, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("manufacturers");

            var element = driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[0];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);

            Assert.AreEqual("BrandName7", driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[1].FindElement(By.ClassName("brand-name")).Text);

            //check inplace sort order
            GoToAdmin("brands");
            GetGridCell(6, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(6, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(6, "SortOrder").FindElement(By.TagName("input")).SendKeys("1");
            DropFocus("h1");
            Assert.AreEqual("1", GetGridCell(6, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("manufacturers");
            var element2 = driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[0];
            IJavaScriptExecutor jse2 = (IJavaScriptExecutor)driver;
            jse2.ExecuteScript("arguments[0].scrollIntoView(true)", element2);

            Assert.AreEqual("BrandName7", driver.FindElements(By.CssSelector(".brand-item.col-xs-12.col-sm-6.col-md-4.col-lg-3"))[0].FindElement(By.ClassName("brand-name")).Text);

            //return 
            GoToAdmin("brands");
            GetGridCell(1, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(1, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(1, "SortOrder").FindElement(By.TagName("input")).SendKeys("7");
            DropFocus("h1");
        }
        
        [Test]
        public void BrandDoActive()
        {
            GoToAdmin("brands");
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click(); 
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);

            //do selected items active 
            GoToAdmin("brands");
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            Thread.Sleep(1000);
            WaitForAjax();
            Assert.AreEqual("BrandName2", GetGridCell(1, "BrandName").Text);
            Assert.IsTrue(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName3", GetGridCell(2, "BrandName").Text);
            Assert.IsTrue(GetGridCell(2, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }

        [Test]
        public void BrandDoNotActive()
        {
            GoToAdmin("brands");
            GetGridCell(5, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();  
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(5, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName6", GetGridCell(5, "BrandName").Text);

            //do selected items not active 
            GoToAdmin("brands");
            GetGridCell(6, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            GetGridCell(7, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"2\"]")).Click();
            Thread.Sleep(1000);
            WaitForAjax();
            Assert.AreEqual("BrandName7", GetGridCell(6, "BrandName").Text);
            Assert.IsFalse(GetGridCell(6, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName8", GetGridCell(7, "BrandName").Text);
            Assert.IsFalse(GetGridCell(7, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //do all items on page not active
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"2\"]")).Click();
            Thread.Sleep(1000);
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }
        
    }
}