using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Tasks
{
    [TestFixture]
    public class DeleteTask : BaseSeleniumTest
    {
         
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Customers);
            InitializeService.LoadData(

          "data\\Admin\\Tasks\\AddTask\\Customers.CustomerGroup.csv",
           "data\\Admin\\Tasks\\AddTask\\Customers.Departments.csv",
           "data\\Admin\\Tasks\\AddTask\\Customers.Customer.csv",
           "data\\Admin\\Tasks\\AddTask\\Customers.Managers.csv",
        //    "data\\Admin\\Tasks\\AddTask\\Customers.ManagerTask.csv",
            "data\\Admin\\Tasks\\AddTask\\Customers.TaskGroup.csv",
            "data\\Admin\\Tasks\\AddTask\\Customers.Task.csv"
           );
             
            Init();
        }
        [Test]
        public void DelSelectedTask()
        {
            GoToAdmin("tasks");
             GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
           driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"0\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test2"));
        
        }
        [Test]
        public void DelTaskiInEditCancel()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskDelTask\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(1000);
              Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test2"));
        }
        [Test]
        public void DelTaskiInEditOk()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskDelTask\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 2);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }

        [Test]
        public void DelTaskiInGridCancel()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }
        [Test]
        public void DelTaskiInGridOk()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test4"));
        }
     /*   [Test]
        public void Attachment()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] span")).Click();
            }*/

    }
}
