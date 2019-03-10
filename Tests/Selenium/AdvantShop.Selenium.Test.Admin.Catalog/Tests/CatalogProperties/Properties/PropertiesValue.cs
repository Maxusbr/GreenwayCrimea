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
    public class PropertiesValue : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Properties\\PropertiesValue\\Catalog.Category.csv",
                "Data\\Admin\\Properties\\PropertiesValue\\Catalog.Brand.csv",
                "Data\\Admin\\Properties\\PropertiesValue\\Catalog.Property.csv",
                 "Data\\Admin\\Properties\\PropertiesValue\\Catalog.PropertyValue.csv",
                 "Data\\Admin\\Properties\\PropertiesValue\\Catalog.ProductPropertyValue.csv",
                "Data\\Admin\\Properties\\PropertiesValue\\Catalog.Product.csv",
                "Data\\Admin\\Properties\\PropertiesValue\\Catalog.Offer.csv",
                "Data\\Admin\\Properties\\PropertiesValue\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Properties\\PropertiesValue\\Catalog.PropertyGroup.csv"
                );

             
            Init();

        }
        
        [Test]
        public void OpenValueWindows()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("2", GetGridCell(0, "ProductsCount").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
            driver.FindElement(By.LinkText("Вернуться назад")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
        }       
       
        [Test]
        public void SearchValue()
        {
             GoToAdmin("propertyValues?propertyId=2");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            
            GetGridFilter().SendKeys("4");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count ==2);
            Assert.AreEqual("4", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
           /* GetGridFilter().Clear();
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 14);*/
       }
        [Test]
        public void FilterValue()
        {
            GoToAdmin("propertyValues?propertyId=2");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"]")).Click();   
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Value\"]")).Displayed);
            driver.FindElement(By.CssSelector("[data-e2e-grid-filter-block-name=\"Value\"] [data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e-grid-filter-block-name=\"Value\"] [data-e2e=\"gridFilterItemInput\"]")).SendKeys("3");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 2);
            Assert.AreEqual("3", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Value\"] [data-e2e=\"gridFilterItemClose\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 14);
         }
        [Test]
        public void Present20Value()
        {
            GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10);
            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 14);
        }
       [Test]
        public void SelectItemValue()
        {
             GoToAdmin("propertyValues?propertyId=2");
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllItemValue()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("14", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
     
        }
        [Test]
        public void SelectAllOnPageItemValue()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void ValuesDel()
        {
             GoToAdmin("propertyValues?propertyId=2");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 13);
            Assert.AreEqual("2", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void ValueSelectitemDelcencel()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("2", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void ValueSelectitemDelok()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("3", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void ValueSelectzAllOnPageItemDelcancel()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("3", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void ValueSelectzAllOnPageItemDelok()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("13", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void ValueSelectzAllzItemDelcancel()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void ValueSelectzAllzItemDelok()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void Present10Page()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10",GetGridCell(9, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("11", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("14",GetGridCell(3, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void Present10PageToNext()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10",GetGridCell(9, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("11", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("14",GetGridCell(3, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            }
        [Test]
        public void Present10PageToPrevious()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10",GetGridCell(9, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("11", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("14",GetGridCell(3, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10",GetGridCell(9, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void Present10PageToEnd()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10",GetGridCell(9, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("11", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("14",GetGridCell(3, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void Present10PageToBegin()
        {
             GoToAdmin("propertyValues?propertyId=2");
            gridReturnDefaultView10();
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10",GetGridCell(9, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("11", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("14",GetGridCell(3, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Thread.Sleep(2000);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10",GetGridCell(9, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        
        [Test]
        public void AddPropertyValuesCanсel()
        {
            GoToAdmin("propertyValues?propertyId=1");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("New_proprtyvalue");
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[1].Clear();
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[1].SendKeys("2");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

        }
        [Test]
        public void AddPropertyValuesOk()
        {
            GoToAdmin("propertyValues?propertyId=1");
            Refresh();
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("New_proprtyvalue");
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[1].Clear();
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[1].SendKeys("2");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("New_proprtyvalue", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        [Test]
        public void AddPropertyValuesoknew()
        {
            GoToAdmin("propertyValues?propertyId=1");
            Refresh();
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed);
            driver.FindElement(By.CssSelector(".col-xs-9 input")).SendKeys("New_proprtyvalue1");
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[1].Clear();
            driver.FindElements(By.CssSelector(".col-xs-9 input"))[1].SendKeys("1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("New_proprtyvalue1", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("New_proprtyvalue",GetGridCell(1, "Value").FindElement(By.TagName("input")).GetAttribute("value"));
        }
    }
}
