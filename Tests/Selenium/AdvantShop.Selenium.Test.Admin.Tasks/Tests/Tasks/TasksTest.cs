using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Tasks
{
    [TestFixture]
    public class TasksTest : BaseSeleniumTest
    {
         
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders| ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\Tasks\\Customers.CustomerGroup.csv",
           "data\\Admin\\Tasks\\Customers.Departments.csv",
           "data\\Admin\\Tasks\\Customers.Customer.csv",
           "data\\Admin\\Tasks\\Customers.Managers.csv",
            "data\\Admin\\Tasks\\Customers.TaskGroup.csv",
            "data\\Admin\\Tasks\\Customers.Task.csv"
           );
             
            Init();
        }
        
       
        [Test]
        public void OpenTasks()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
        }
        [Test]
        public void SearchTasks()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            GetGridFilter().SendKeys("test1");
            DropFocus("h1");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Thread.Sleep(2000);
        }

        [Test]
        public void AssignedToMeTasks()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Мои задачи")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));
        }

        [Test]
        public void AppointedByMeTasks()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Поручил")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }
        [Test]
        public void CompletedTasks()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Завершенные")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test5"));
        }
        [Test]
        public void AcceptedTasks()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Принятые")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test15"));
        }
       
    }
}
