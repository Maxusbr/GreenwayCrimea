using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CustomersGroup
{
    [TestFixture]
    public class CustomersGroupTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.Product.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.Offer.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.Category.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.ProductCategories.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.Customer.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.CustomerGroup.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.Departments.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.Managers.csv",
            "data\\Admin\\Customers\\CustomersGroup\\Customers.ManagerTask.csv"
           );
             
            Init();
        }
        
        [Test]
        public void CustomersGroupPresent()
        {
            GoToAdmin("customergroups");
            Assert.AreEqual("Группы покупателей", driver.FindElement(By.TagName("h1")).Text);
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup110", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup120", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup20", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup70", GetGridCell(99, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check go to customers
            ScrollTo(By.Id("header-top"));
            GetGridCell(0, "_customersColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Покупатели, Группа CustomerGroup101", driver.FindElement(By.TagName("h1")).Text);
        }

        [Test]
        public void CustomersGroupSelectAndDelete()
        {
            GoToAdmin("customergroups");
            gridReturnDefaultView10();
            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("200", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check deselect all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            
            Refresh();

            //check delete cancel
            GetGridCell(1, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup102", GetGridCell(1, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            Assert.AreNotEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(3000);
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(3000);
            GetGridCell(3, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(3000);
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("4", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(3, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreNotEqual("CustomerGroup102", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreNotEqual("CustomerGroup103", GetGridCell(1, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreNotEqual("CustomerGroup104", GetGridCell(2, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreNotEqual("CustomerGroup105", GetGridCell(3, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all on page 10
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check delete selected items all on page 10
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("CustomerGroup116", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup195", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all on page 20
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check delete selected items all on page 20
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("CustomerGroup1", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup24", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all on page 50
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("50", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check delete selected items all on page 50
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("CustomerGroup1", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup123", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all on page 100
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(99, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check delete selected items all on page 100
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("CustomerGroup1", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup190", GetGridCell(17, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check delete all selected items
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("customergroups");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text.Contains("Найдено записей: 1"));
        }

        [Test]
        public void CustomersGroupSearchExist()
        {
            GoToAdmin("customergroups");

            //check search exist item
            GetGridFilter().SendKeys("CustomerGroup126");
            DropFocus("h1");
            Assert.AreEqual("CustomerGroup126", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check search not exist item
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("CustomerGroup556");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
       
    }
}