using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Tasks
{
    [TestFixture]
    public class TasksSort : BaseSeleniumTest
    {
         
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Customers);
            InitializeService.LoadData(

          "data\\Admin\\Tasks\\ManyTasks\\Customers.CustomerGroup.csv",
           "data\\Admin\\Tasks\\ManyTasks\\Customers.Departments.csv",
           "data\\Admin\\Tasks\\ManyTasks\\Customers.Customer.csv",
           "data\\Admin\\Tasks\\ManyTasks\\Customers.Managers.csv",
          //  "data\\Admin\\Tasks\\ManyTasks\\Customers.ManagerTask.csv",
            "data\\Admin\\Tasks\\ManyTasks\\Customers.TaskGroup.csv",
            "data\\Admin\\Tasks\\ManyTasks\\Customers.Task.csv"
           );
             
            Init();
        }
        
        [Test]
        public void SortByNumber()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            GetGridCell(-1, "Id").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));
            GetGridCell(-1, "Id").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test114"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test105"));
            GetGridCell(-1, "Id").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));
        }

        [Test]
        public void SortByName()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            GetGridCell(-1, "Name").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test107"));
            GetGridCell(-1, "Name").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test99"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test90"));
        }
        [Test]
        public void SortByPriority()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            GetGridCell(-1, "PriorityFormatted").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test28"));
            Assert.AreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("Низкий", (GetGridCell(9, "PriorityFormatted").FindElement(By.TagName("div")).Text));

            GetGridCell(-1, "PriorityFormatted").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test35"));
            Assert.AreEqual("Высокий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("Высокий", (GetGridCell(9, "PriorityFormatted").FindElement(By.TagName("div")).Text));
        }
        [Test]
        public void SortByDuedate()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            GetGridCell(-1, "DueDateFormatted").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test114"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test105"));
            GetGridCell(-1, "DueDateFormatted").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));
        }
        [Test]
        public void SortByStatus()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            GetGridCell(-1, "StatusFormatted").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));
            Assert.AreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("В работе", (GetGridCell(9, "StatusFormatted").FindElement(By.TagName("div")).Text));

            GetGridCell(-1, "StatusFormatted").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test50"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test59"));
            Assert.AreEqual("В работе (выполняется)", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("В работе (выполняется)", (GetGridCell(9, "StatusFormatted").FindElement(By.TagName("div")).Text));
        }
        [Test]
        public void SortByAssigned()
        {
            GoToAdmin("tasks");
            Thread.Sleep(2000);
            GetGridCell(-1, "AssignedName").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test7"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test27"));
            Assert.AreEqual("Admin Ad", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("Admin Ad", (GetGridCell(9, "AssignedName").FindElement(By.TagName("div")).Text));

            GetGridCell(-1, "AssignedName").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test31"));
            Assert.AreEqual("test testov", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("test testov", (GetGridCell(9, "AssignedName").FindElement(By.TagName("div")).Text));
        }
        [Test]
        public void SortByAppointed()
        {
            GoToAdmin("tasks");
            Thread.Sleep(3000);

            GetGridCell(-1, "AppointedName").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
            Assert.AreEqual("Admin Ad", (GetGridCell(0, "AppointedName").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("Admin Ad", (GetGridCell(9, "AppointedName").FindElement(By.TagName("div")).Text));

            GetGridCell(-1, "AppointedName").Click(); 
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.AreEqual("test testov", (GetGridCell(0, "AppointedName").FindElement(By.TagName("div")).Text));
            Assert.AreEqual("test testov", (GetGridCell(9, "AppointedName").FindElement(By.TagName("div")).Text));
        }
    }
    }
