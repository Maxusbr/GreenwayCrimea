using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CustomersGroup
{
    [TestFixture]
    public class CustomersGroupViewCustomerTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.ClearData(ClearType.Orders);
            InitializeService.LoadData(
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.Customer.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.Contact.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.CustomerGroup.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.Departments.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.Managers.csv",
          //  "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.ManagerTask.csv",
           // "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.TaskGroup.csv",
           // "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.Task.csv",
           // "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Customers.ViewedTask.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Catalog.Product.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Catalog.Offer.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Catalog.Category.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\Catalog.ProductCategories.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].OrderContact.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].OrderCurrency.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].OrderItems.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].OrderSource.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].OrderStatus.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].[Order].csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].OrderCustomer.csv"
         //   "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].LeadItem.csv",
          //  "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].LeadCurrency.csv",
           // "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupView\\[Order].Lead.csv"
           );
             
            Init();
        }

        /*
     [Test]
     public void CustomersGroupViewOpenPage()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(3000);
         driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_customersColumn\']\"] a")).Click();
         Thread.Sleep(3000);
         Assert.AreEqual("Покупатели, Группа CustomerGroup101", driver.FindElement(By.TagName("h1")).Text);
         //Assert.AreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridRow\"][data-e2e-row-index=\"0\"] [data-e2e=\"gridCell\"][data-e2e-grid-cell=\"grid[0][\'_customersColumn\']\"] a")).Text);
         Thread.Sleep(1000);
     }

     [Test]
     public void CustomersGroupSearchExist()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("CustomerGroup57");
         Thread.Sleep(5000);
         driver.FindElement(By.TagName("body")).Click();
         Thread.Sleep(5000);
         Assert.AreEqual("CustomerGroup57", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'GroupName\']\"] input")).GetAttribute("value"));
         Thread.Sleep(1000);
     }

     [Test]
     public void CustomersGroupSearchNotExist()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("CustomerGroup9900");
         Thread.Sleep(5000);
         driver.FindElement(By.TagName("body")).Click();
         Thread.Sleep(5000);
         Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
         Thread.Sleep(1000);
     }

     [Test]
     public void CustomersGroupSearchTooMuchSymbols()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("111111111122222qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq2222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
         Thread.Sleep(5000);
         driver.FindElement(By.TagName("body")).Click();
         Thread.Sleep(5000);
         Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
         Thread.Sleep(1000);
     }

     [Test]
     public void CustomersGroupSearchInvalidSymbols()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
         Thread.Sleep(5000);
         driver.FindElement(By.TagName("body")).Click();
         Thread.Sleep(5000);
         Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
         Thread.Sleep(1000);
     }

     [Test]
     public void CustomersGroupWDelete()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         driver.FindElement(By.CssSelector("[data-e2e=\"gridRow\"][data-e2e-row-index=\"0\"] [data-e2e=\"gridCell\"][data-e2e-col-index=\"4\"] a")).Click();
         Thread.Sleep(2000);
         driver.FindElement(By.ClassName("swal2-confirm")).Click();
         Thread.Sleep(2000);
         Assert.AreNotEqual("CustomerGroup101", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'GroupName\']\"] input")).GetAttribute("value"));
         Thread.Sleep(1000);
     }

     [Test]
     public void CustomersGroupDeleteCancel()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         driver.FindElement(By.CssSelector("[data-e2e=\"gridRow\"][data-e2e-row-index=\"1\"] [data-e2e=\"gridCell\"][data-e2e-col-index=\"4\"] a")).Click();
         Thread.Sleep(2000);
         driver.FindElement(By.ClassName("swal2-cancel")).Click();
         Thread.Sleep(2000);
         Assert.AreEqual("CustomerGroup102", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'GroupName\']\"] input")).GetAttribute("value"));
         Thread.Sleep(1000);
     }

     [Test]
     public void CustomersGroupGoToCustomers()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         driver.FindElement(By.CssSelector("[data-e2e=\"gridRow\"][data-e2e-row-index=\"0\"] [data-e2e=\"gridCell\"][data-e2e-grid-cell=\"grid[0][\'_customersColumn\']\"] a")).Click();
         Thread.Sleep(3000);
         Assert.AreEqual("Покупатели, Группа CustomerGroup101", driver.FindElement(By.TagName("h1")).Text);
         Thread.Sleep(1000);
     }

     /*
     [Test]
     public void CustomersGroupCorrectQuantityAll()
     {
         GoToAdmin("customergroups");
         Thread.Sleep(2000);
         Assert.AreEqual("200", driver.FindElements(By.CssSelector(".link-invert.aside-menu-row"))[5].FindElement(By.CssSelector(".aside-menu-count-inner")).Text);
         Thread.Sleep(1000);
     } */
        /*

        [Test]
        public void CustomersGroupXSelectDelete()
        {
            GoToAdmin("customergroups");
            Thread.Sleep(2000);
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(2000);
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(2000);
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("CustomerGroup102", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'GroupName\']\"] input")).GetAttribute("value"));
            Thread.Sleep(2000);
            Assert.AreNotEqual("CustomerGroup103", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'GroupName\']\"] input")).GetAttribute("value"));
            Thread.Sleep(2000);
            Assert.AreNotEqual("CustomerGroup104", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[2][\'GroupName\']\"] input")).GetAttribute("value"));
            Thread.Sleep(2000);
        }

        [Test]
        public void CustomersGroupYSelectAllOnPage20Delete()
        {
            GoToAdmin("customergroups");
            Thread.Sleep(2000);
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            //Assert.AreEqual("CustomerGroup135", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'GroupName\']\"] input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup123", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'GroupName\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
        }

        [Test]
        public void CustomersGroupYSelectAllOnPage10Delete()
        {
            GoToAdmin("customergroups");
            Thread.Sleep(2000);
            Functions.GridPaginationSelect10(driver, baseURL);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            // Assert.AreEqual("CustomerGroup115", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'GroupName\']\"] input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup84", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'GroupName\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
        }

        [Test]
        public void CustomersGroupYSelectAllOnPage50Delete()
        {
            GoToAdmin("customergroups");
            Thread.Sleep(2000);
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            //Assert.AreEqual("CustomerGroup185", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'GroupName\']\"] input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup172", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'GroupName\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
        }

        [Test]
        public void CustomersGroupYSelectAllOnPage100Delete()
        {
            GoToAdmin("customergroups");
            Thread.Sleep(2000);
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            //Assert.AreEqual("CustomerGroup105", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'GroupName\']\"] input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup75", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'GroupName\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
        }

        [Test]
        public void CustomersGroupZAllSelectDelete()
        {
            GoToAdmin("customergroups");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(4000);
            //Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            Assert.AreEqual("CustomerGroup1", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Thread.Sleep(1000);
        }  */

    }
}