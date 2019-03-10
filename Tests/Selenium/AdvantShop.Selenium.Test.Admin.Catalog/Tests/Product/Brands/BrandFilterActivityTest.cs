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
    public class BrandFilterActivityTest : BaseSeleniumTest
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
        public void BrandFilterActivityAOnPresent()
        {
            GoToAdmin("brands");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Активные", tag: "h1");
            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName110", GetGridCell(9, "BrandName").Text);

            PageSelectItems("20");
            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName120", GetGridCell(19, "BrandName").Text);

            PageSelectItems("50");
            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName150", GetGridCell(49, "BrandName").Text);

            PageSelectItems("100");
            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName200", GetGridCell(99, "BrandName").Text);

            //close
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(99, "BrandName").Text);
        }

        [Test]
        public void BrandFilterActivityBOffPresent()
        {
            GoToAdmin("brands");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h1");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            PageSelectItems("20");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(19, "BrandName").Text);

            PageSelectItems("50");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(49, "BrandName").Text);

            PageSelectItems("100");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(99, "BrandName").Text);
        }
        
        [Test]
        public void BrandFilterActivityOnSelectAndDelete()
        {
            GoToAdmin("brands");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
              Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Активные", tag: "h1");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("BrandName101", GetGridCell(0, "BrandName").Text);

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
            Assert.AreNotEqual("BrandName102", GetGridCell(0, "BrandName").Text);
            Assert.AreNotEqual("BrandName103", GetGridCell(1, "BrandName").Text);
            Assert.AreNotEqual("BrandName104", GetGridCell(2, "BrandName").Text);

            //check select all on page
            PageSelectItems("50");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("BrandName155", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName200", GetGridCell(45, "BrandName").Text);
            Assert.AreEqual("Найдено записей: 46", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check select all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("46", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(45, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //close filter
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
        }
 
        [Test]
        public void BrandFilterActivityOnDoNotActive()
        {
            GoToAdmin("brands");
              Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Активные", tag: "h1");
            PageSelectItems("20");
            ScrollTo(By.Id("header-top"));
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            DropFocus("h1");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);

            //do selected items not active 
            Refresh();
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"2\"]")).Click();
            Thread.Sleep(1000);
           Assert.AreNotEqual("BrandName2", GetGridCell(0, "BrandName").Text);
           Assert.AreNotEqual("BrandName6", GetGridCell(1, "BrandName").Text);

            //close
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(19, "BrandName").Text);
        }

        [Test]
        public void BrandFilterActivityOffDoActive()
        {
            GoToAdmin("brands");
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h1");
            PageSelectItems("20");
            ScrollTo(By.Id("header-top"));
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            DropFocus("h1");
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //do selected items active 
            Refresh();
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(4, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            Thread.Sleep(1000);
           Assert.AreNotEqual("BrandName2", GetGridCell(0, "BrandName").Text);
           Assert.AreNotEqual("BrandName6", GetGridCell(4, "BrandName").Text);
        }
       
        [Test]
        public void BrandFilterActivityAOnGoToEditAndBack()
        {
            GoToAdmin("brands");
              Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Активные", tag: "h1");
            PageSelectItems("10");

            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName110", GetGridCell(9, "BrandName").Text);

            GetGridCell(0, "BrandName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Производитель \"BrandName101\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("BrandName101", driver.FindElement(By.Id("BrandName")).GetAttribute("value"));
            Assert.IsTrue(driver.Url.Contains("brands/edit/101"));

            GoBack();
            Thread.Sleep(3000);
            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName110", GetGridCell(9, "BrandName").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Enabled\"]")).Displayed);
        }
    }
}