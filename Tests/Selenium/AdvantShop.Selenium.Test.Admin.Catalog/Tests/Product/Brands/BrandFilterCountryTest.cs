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
    public class BrandFilterCountryTest : BaseSeleniumTest
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
        public void BrandAFilterCountryPresent()
        {
            GoToAdmin("brands");
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "CountryName", filterItem: "Южный Судан", tag: "h1");
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("Южный Судан", GetGridCell(0, "CountryName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(9, "BrandName").Text);
            Assert.AreEqual("Южный Судан", GetGridCell(9, "CountryName").Text);

            PageSelectItems("20");
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName110", GetGridCell(19, "BrandName").Text);

           PageSelectItems("50");
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName140", GetGridCell(49, "BrandName").Text);

           PageSelectItems("100");
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName190", GetGridCell(99, "BrandName").Text);

            //close
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "CountryName");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName100", GetGridCell(99, "BrandName").Text);
        }

        [Test]
        public void BrandBFilterCountryNotExistPresent10()
        {
            GoToAdmin("brands");
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "CountryName", filterItem: "Германия", tag: "h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //close
            Functions.GridFilterClose(driver, baseURL, name: "CountryName");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterCountrySelectAndDelete()
        {
            GoToAdmin("brands");
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "CountryName", filterItem: "Южный Судан", tag: "h1");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("BrandName91", GetGridCell(0, "BrandName").Text);

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
            Assert.AreNotEqual("BrandName92", GetGridCell(0, "BrandName").Text);
            Assert.AreNotEqual("BrandName93", GetGridCell(1, "BrandName").Text);
            Assert.AreNotEqual("BrandName94", GetGridCell(2, "BrandName").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("BrandName105", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName114", GetGridCell(9, "BrandName").Text);

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("89", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

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

            //close
            Functions.GridFilterClose(driver, baseURL, name: "CountryName");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterCountryDoNotActive()
        {
            GoToAdmin("brands");
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "CountryName", filterItem: "Южный Судан", tag: "h1");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
           Thread.Sleep(1000);
            DropFocus("h1");
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName101", GetGridCell(0, "BrandName").Text);

            //do selected items not active 
            Refresh();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"2\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName102", GetGridCell(1, "BrandName").Text);
            Assert.IsFalse(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName103", GetGridCell(2, "BrandName").Text);
            Assert.IsFalse(GetGridCell(2, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //close
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "CountryName");
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
        }

        [Test]
        public void BrandFilterCountryDoActive()
        {
            GoToAdmin("brands");
            PageSelectItems("20");
            ScrollTo(By.Id("header-top"));
             Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "CountryName", filterItem: "Южный Судан", tag: "h1");
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            DropFocus("h1");
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);

            //do selected items active 
            Refresh();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("BrandName92", GetGridCell(1, "BrandName").Text);
            Assert.IsTrue(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName93", GetGridCell(2, "BrandName").Text);
            Assert.IsTrue(GetGridCell(2, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }
       
        [Test]
        public void BrandCFilterCountryGoToEditAndBack()
        {
            GoToAdmin("brands");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "CountryName", filterItem: "Россия", tag: "h1");

            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName9", GetGridCell(8, "BrandName").Text);

            ScrollTo(By.Id("header-top"));
            GetGridCell(0, "BrandName").Click();
           Thread.Sleep(2000);

            Assert.AreEqual("Производитель \"BrandName1\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("BrandName1", driver.FindElement(By.Id("BrandName")).GetAttribute("value"));
            Assert.IsTrue(driver.Url.Contains("brands/edit/1"));
            GoBack();
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName9", GetGridCell(8, "BrandName").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"CountryName\"]")).Displayed);
        }

        /* more than 1 filters */
        [Test]
        public void ABrandTwoFiltersActivityAndCountry()
        {
            GoToAdmin("brands");

           PageSelectItems("50");
            ScrollTo(By.Id("header-top"));

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"CountryName\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Южный Судан");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"Enabled\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Enabled\"] [data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");

            DropFocus("h1");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Assert.AreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            Assert.AreEqual("BrandName100", GetGridCell(9, "BrandName").Text);
            ScrollTo(By.Id("header-top"));
            Assert.AreEqual("BrandName91", GetGridCell(0, "BrandName").Text);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Functions.GridFilterClose(driver, baseURL, name: "CountryName");
            Assert.AreEqual("Найдено записей: 200", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
    }
}