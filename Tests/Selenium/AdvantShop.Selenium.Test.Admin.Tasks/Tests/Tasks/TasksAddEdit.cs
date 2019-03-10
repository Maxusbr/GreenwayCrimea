using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Tasks
{
    [TestFixture]
    public class AddNewTask : BaseSeleniumTest
    {   
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\Tasks\\Customers.CustomerGroup.csv",
           "data\\Admin\\Tasks\\Customers.Departments.csv",
           "data\\Admin\\Tasks\\Customers.Customer.csv",
           "data\\Admin\\Tasks\\Customers.Managers.csv",
           "data\\Admin\\Tasks\\Customers.TaskGroup.csv",
           "data\\Admin\\Tasks\\Customers.ViewedTask.csv"
           );
             
            Init();
        }
        
        [Test]
        public void AddCorrectTask()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"AddTask\"]")).Click();
            Thread.Sleep(3000);
            //name
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskName\"]")).SendKeys("NewTestTask");
            Thread.Sleep(2000);
            //Assigned
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"newtaskAssigned\"]")))).SelectByText("Admin Ad");
            Thread.Sleep(2000);
            //duedate dueDate
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDuedate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDuedate\"]")).SendKeys("08.12.2016 13:45");
            Thread.Sleep(2000);
            //Despription
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDescription\"]")).SendKeys("Description NewTestTask");
            Thread.Sleep(2000);
            //Group
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"newtaskGroup\"]")))).SelectByText("All");
            Thread.Sleep(2000);
            //Priority
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"newtaskPriority\"]")))).SelectByText("Высокий");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"TaskAdd\"]")).Click();
            Thread.Sleep(2000);
            Refresh();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("NewTestTask"));
        }


        [Test]
        public void EditTask()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            //name
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).SendKeys("EditNewTestTask");
            //Appointed
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAppointed\"]")))).SelectByText("Elena El");
            //duedate dueDate
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskDuedate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskDuedate\"]")).SendKeys("12.12.2020 13:45");
           
            //Group
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]")))).SelectByText("All");
            //Priority
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]")))).SelectByText("Средний");
             //Despription
            SetCkText("Edit Description NewTestTask", "editor1");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskButtonSave\"]")).Click();
            Thread.Sleep(3000);
            Refresh();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("EditNewTestTask"));
            Assert.AreEqual("Средний", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("12.12.2020 13:45", (GetGridCell(0, "DueDateFormatted").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("Admin Ad", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("Elena El", (GetGridCell(0, "AppointedName").FindElement(By.TagName("div")).Text));
             }
        [Test]
        public void EditsTaskCandel()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            //name
            
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).SendKeys("EditCancelNewTestTask");
            Thread.Sleep(2000);
           
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("NewTestTask"));
            Assert.IsFalse(driver.PageSource.Contains("EditCancelNewTestTask"));
        }
    
    }
}
