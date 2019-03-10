using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Colors
{
    [TestFixture]
    public class ColorSelect : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Color\\Catalog.Color.csv"
                );

             
            Init();

        }
        
     
        [Test]
        public void SelectItemPresent10()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'selectionRowHeaderCol\']\"] [data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
        }
        [Test]
        public void SelectAllItemPresent10()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllItemPresent20()
        {
           GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllItemPresent50()
        {
           GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllItemPresent100()
        {
           GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(99, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent10()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent20()
        {
           GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent50()
        {
           GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("50", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void SelectAllOnPageItemPresent100()
        {
           GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(99, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
        }
        [Test]
        public void DeSelectAllItemPresent10()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        [Test]
        public void DeSelectAllItemPresent20()
        {
           GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        [Test]
        public void DeSelectAllItemPresent50()
        {
            GoToAdmin("colors");
            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        [Test]
        public void DeSelectAllItemPresent100()
        {
           GoToAdmin("colors");

            ScrollTo(By.CssSelector(".version"));
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Selected);
        }
        [Test]
        public void SelectItemzDeletCancel()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
           driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'selectionRowHeaderCol\']\"] [data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color1", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void SelectItemzDeletOk()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'selectionRowHeaderCol\']\"] [data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("Color1", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color2", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void SelectItemzDeletWithoutSelectCancel()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            //driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"3\"] a"))[1].Click();
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color2", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void SelectItemzDeletWithoutSelectOk()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("Color2", GetGridCell(0, "ColorName").Text);
            Assert.AreEqual("Color3", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void SelectItemzDelOnPageCancel()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color3", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void SelectItemzDelOnPageOk()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Color13", GetGridCell(0, "ColorName").Text);
        }
        [Test]
        public void SelectItemzDelzAllCancel()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void SelectItemzDelzAllOk()
        {
           GoToAdmin("colors");
            gridReturnDefaultView10();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
           driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        
    }
}
