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
    public class PropertiesSelect : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Properties\\Catalog.Category.csv",
                "Data\\Admin\\Properties\\Catalog.Brand.csv",
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
        public void SelectItemPresent10()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'selectionRowHeaderCol\']\"] [data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
        }
        [Test]
        public void SelectAllItemPresent10()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("101", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllItemPresent20()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("101", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllItemPresent50()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("101", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllItemPresent100()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(99, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("101", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent10()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent20()
        {
             GoToAdmin("properties");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent50()
        {
             GoToAdmin("properties");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("50", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent100()
        {
             GoToAdmin("properties");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(99, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void DeSelectAllItemPresent10()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("101", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        [Test]
        public void DeSelectAllItemPresent20()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("101", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        [Test]
        public void DeSelectAllItemPresent50()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        [Test]
        public void DeSelectAllItemPresent100()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        //delet
        [Test]
        public void SelectItemzDeletCancel()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void SelectItemzDeletOk()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Property2", GetGridCell(0, "Name").Text);
        }      
        [Test]
        public void SelectItemzDeletWithoutSelectCancel()
        {
             GoToAdmin("properties");                                 
            gridReturnDefaultView10();
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Property2", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void SelectItemzDeletWithoutSelectOk()
        {
             GoToAdmin("properties");                              
            gridReturnDefaultView10();
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.AreNotEqual("Property2", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property3", GetGridCell(0, "Name").Text);
        }        
        [Test]
        public void SelectItemzDelOnPageCancel()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Property3", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void SelectItemzDelOnPageOk()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Property13", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void SelectItemzDelzAllCancel()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000); driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void SelectItemzDelzAllOk()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000); driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void SelectItemChangeGroup()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'selectionRowHeaderCol\']\"] [data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("Group11");
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);                                                             
            Assert.AreEqual("Group11", GetGridCell(0, "GroupName").Text); 
            Assert.AreNotEqual("Group11", GetGridCell(1, "GroupName").Text); 
          GoToAdmin("properties?groupId=11#?grid=%7B%22paginationCurrentPage%22:1,%22paginationPageSize%22:10,%22Page%22:1,%22ItemsPerPage%22:100%7D");
            Assert.AreEqual(1, driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count);
        }
        [Test]
        public void SelectItemChangeGroupOnPage()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("Group3");  
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);                                                                         
            Assert.AreEqual("Group3", GetGridCell(0, "GroupName").Text);
            Assert.AreEqual("Group3", GetGridCell(9, "GroupName").Text);
            GoToAdmin("properties?groupId=3");
            Assert.AreEqual(10, driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count);
        }
        [Test]
        public void SelectItemChangeGroupAll()
        {
             GoToAdmin("properties");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("Group2");
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);                                                         
            Assert.AreEqual("Group2", GetGridCell(0, "GroupName").Text);
            Assert.AreEqual("Group2", GetGridCell(9, "GroupName").Text);
            GoToAdmin("properties?groupId=2");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Assert.AreEqual("101", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
    }
}
