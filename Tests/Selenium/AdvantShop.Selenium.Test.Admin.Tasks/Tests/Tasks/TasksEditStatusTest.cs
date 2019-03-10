using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Tasks
{
    [TestFixture]
    public class EditStatusTaskTest : BaseSeleniumTest
    {
         
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Customers);
            InitializeService.LoadData(

          "data\\Admin\\Tasks\\EditTasks\\Customers.CustomerGroup.csv",
           "data\\Admin\\Tasks\\EditTasks\\Customers.Departments.csv",
           "data\\Admin\\Tasks\\EditTasks\\Customers.Customer.csv",
           "data\\Admin\\Tasks\\EditTasks\\Customers.Managers.csv",
            "data\\Admin\\Tasks\\EditTasks\\Customers.TaskGroup.csv",
            "data\\Admin\\Tasks\\EditTasks\\Customers.Task.csv"

           );
             
            Init();
        }
        

        [Test]
        public void EditTaskStatusInProgress()
        {
            GoToAdmin("tasks");
            Assert.AreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskStatusInprogress\"]")).Click();
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("В работе (выполняется)", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
        }
        [Test]
        public void EditTaskStatusStoped()
        {
            GoToAdmin("tasks");
            Assert.AreEqual("В работе (выполняется)", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskStatusStop\"]")).Click();
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
        }
        [Test]
        public void EditTaskStatustComplitedOk()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskStatusCompleted\"]")).Click();
            driver.FindElement(By.CssSelector(".form-group input")).SendKeys("Rezult");
            driver.FindElement(By.CssSelector(".ladda-label")).Click();
            Thread.Sleep(1000);
           GoToAdmin("tasks?filterby=completed");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.AreEqual("Завершена", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
            //edittaskRezult
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            //rezult
            Assert.AreEqual("Rezult", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskRezult\"]")).Text);
            Thread.Sleep(2000);
        }
        [Test]
        public void EditTaskStatustComplitedCandel()
        {
            GoToAdmin("tasks");
            string str = GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text;
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskStatusCompleted\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"taskResult\"]")).SendKeys("Rezult");
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual(str, (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
        }
        [Test]
        public void EditTaskStatuzAccepted()
        {
            GoToAdmin("tasks?filterby=completed");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskStatusAccepted\"]")).Click();
            Thread.Sleep(1000);
            GoToAdmin("tasks?filterby=accepted");
              Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.AreEqual("Принято", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
           }
    }
}
