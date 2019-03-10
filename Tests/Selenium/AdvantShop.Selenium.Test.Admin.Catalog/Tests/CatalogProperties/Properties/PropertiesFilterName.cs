using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Properties
{
    [TestFixture]
    public class PropertiesFilterName : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Properties\\Catalog.Category.csv",
                "data\\Admin\\Properties\\Catalog.Brand.csv",
                "Data\\Admin\\Properties\\Catalog.Property.csv",
                 "Data\\Admin\\Properties\\Catalog.PropertyValue.csv",
                 "Data\\Admin\\Properties\\Catalog.ProductPropertyValue.csv",
                "Data\\Admin\\Properties\\Catalog.Product.csv",
                "Data\\Admin\\Properties\\Catalog.Offer.csv",
                "Data\\Admin\\Properties\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Properties\\Catalog.PropertyGroup.csv"
                );

             
            Init();

        }
        
        [Test]
        public void ByName()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property10");
            DropFocus("h1");
           
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(2, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "Name");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void ByNamePresent20()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property10");
            DropFocus("h1");

            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(2, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "Name");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void ByNamePresent50()
        {
            GoToAdmin("properties");

            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property10");
            DropFocus("h1");

            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(2, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "Name");
            Thread.Sleep(2000);
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property50", GetGridCell(49, "Name").Text);
        }
        [Test]
        public void ByNameAndFilter()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property2");
            DropFocus("h1");

            Assert.AreEqual("Property2", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property28", GetGridCell(9, "Name").Text);

            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");           
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");

            Assert.AreEqual("Property2", GetGridCell(0, "Name").Text);

            //close name
            Functions.GridFilterClose(driver, baseURL, "Name");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property5", GetGridCell(4, "Name").Text);
            //close filter
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void ByNameAndSort()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property1");
            DropFocus("h1");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property18", GetGridCell(9, "Name").Text);

            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");        
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("10");
           DropFocus("h1");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(1, "Name").Text);

            //close name
            Functions.GridFilterClose(driver, baseURL, "Name");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            //close sort
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
   
        }
        [Test]
        public void ByFilterAndDetails()
        {
             GoToAdmin("properties");
            Thread.Sleep(1000);
            Functions.GridFilterSet(driver, baseURL, "UseInFilter");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");
           
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);

            Functions.GridFilterSet(driver, baseURL, "UseInDetails");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"UseInDetails\"] [data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");

            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close details
            Functions.GridFilterClose(driver, baseURL, "UseInDetails");
            Assert.AreEqual("Property6", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property15", GetGridCell(9, "Name").Text);
            //close brief
            Functions.GridFilterClose(driver, baseURL, "UseInFilter");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void ByNameGoToEditAndBack()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property10");
            DropFocus("h1");
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(2, "Name").Text);


            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] a")).Click();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(2, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"]")).Displayed);
        }
        /*
        [Test]
        public void ByNameGoToDetailsAndBack()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property10");
            DropFocus("h1");
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(2, "Name").Text);

            driver.FindElement(By.CssSelector(".fa.fa-list")).Click();
            Thread.Sleep(3000);
            Refresh();
            Assert.AreEqual("Значения свойства - \"Property10\"", driver.FindElement(By.TagName("h1")).Text);
            GoBack();
            GoBack();
            Thread.Sleep(3000);
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property101", GetGridCell(2, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"]")).Displayed);
        }*/
        [Test]
        public void ByNamezDelEl()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            Functions.GridFilterSet(driver, baseURL, "Name");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Property2");
            DropFocus("h1");
            Assert.AreEqual("Property2", GetGridCell(0, "Name").Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Property29", GetGridCell(0, "Name").Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"] [data-e2e=\"gridFilterItemClose\"]")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property3", GetGridCell(1, "Name").Text);
            Assert.AreEqual("Property11", GetGridCell(9, "Name").Text);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"]")).Count > 0);
        }
    }

}
