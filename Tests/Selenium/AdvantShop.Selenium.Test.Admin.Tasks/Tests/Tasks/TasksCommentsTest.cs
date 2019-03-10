using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Tasks
{
    [TestFixture]
    public class TasksCommentsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\TasksCommentTest\\Customers.CustomerGroup.csv",
           "data\\Admin\\TasksCommentTest\\Customers.Departments.csv",
           "data\\Admin\\TasksCommentTest\\Customers.Customer.csv",
           "data\\Admin\\TasksCommentTest\\Customers.Managers.csv",

            "data\\Admin\\TasksCommentTest\\Customers.TaskGroup.csv",
           "data\\Admin\\TasksCommentTest\\Customers.Task.csv"
           );
             
            Init();
        }

        
        [Test]
        public void AddCommentTasksADD()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("editTaskForm"));

            ScrollTo(By.CssSelector("[data-e2e=\"edittaskAttachment\"]"));

            driver.FindElement(By.Name("addAdminCommentForm")).FindElement(By.TagName("textarea")).SendKeys("TestComment1");
            driver.FindElement(By.CssSelector("[data-e2e=\"commentAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            GoToAdmin("tasks");
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click(); 
            Thread.Sleep(3000);
            WaitForElem(By.Name("editTaskForm"));
            
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"commentName\"]")).Text.Contains("Admin Ad"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"commentText\"]")).Text.Contains("TestComment1"));
        }

        [Test]
        public void AddCommentTasksANSWER()
        {
            GoToAdmin("tasks");
            GetGridCell(1, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("editTaskForm"));

            ScrollTo(By.CssSelector("[data-e2e=\"edittaskAttachment\"]"));

            driver.FindElement(By.Name("addAdminCommentForm")).FindElement(By.TagName("textarea")).SendKeys("TestComment2");
            driver.FindElement(By.CssSelector("[data-e2e=\"commentAdd\"]")).Click();
            Thread.Sleep(2000);
             driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            GoToAdmin("tasks");
            GetGridCell(1, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Name("editTaskForm"));

            ScrollTo(By.CssSelector("[data-e2e=\"edittaskAttachment\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"commentAnswer\"]")).Click();
            driver.FindElement(By.Id("adminCommentsFormText")).SendKeys(" TestComment2 Answer");
            driver.FindElement(By.CssSelector("[data-e2e=\"commentAdd\"]")).Click();
            Thread.Sleep(2000);
             driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            GoToAdmin("tasks");
            GetGridCell(1, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Name("editTaskForm"));
            
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"commentName\"]"))[1].Text.Contains("Admin Ad"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"commentText\"]"))[1].Text.Contains("TestComment2 Answer"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"commentParentName\"]")).Text.Contains("Admin Ad"));
        }

        [Test]
        public void AddCommentTasksDELETE()
        {
            GoToAdmin("tasks");
            GetGridCell(2, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("editTaskForm"));

            ScrollTo(By.CssSelector("[data-e2e=\"edittaskAttachment\"]"));

            driver.FindElement(By.Name("addAdminCommentForm")).FindElement(By.TagName("textarea")).SendKeys("TestComment3");
            driver.FindElement(By.CssSelector("[data-e2e=\"commentAdd\"]")).Click();
            Thread.Sleep(2000);
             driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            GoToAdmin("tasks");
            GetGridCell(2, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Name("editTaskForm"));

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"commentText\"]")).Text.Contains("TestComment3"));

            ScrollTo(By.CssSelector("[data-e2e=\"edittaskAttachment\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"commentDelete\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
             driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            GoToAdmin("tasks");
            GetGridCell(2, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Name("editTaskForm"));
            
            Assert.IsFalse(driver.PageSource.Contains("TestComment3"));
          }

        [Test]
        public void AddCommentTasksEDIT()
        {
            GoToAdmin("tasks");
            GetGridCell(3, "_serviceColumn").FindElement(By.TagName("a")).Click();  
            Thread.Sleep(2000);
            WaitForElem(By.Name("editTaskForm"));
            ScrollTo(By.CssSelector("[data-e2e=\"edittaskAttachment\"]"));
            driver.FindElement(By.Name("addAdminCommentForm")).FindElement(By.TagName("textarea")).SendKeys("TestComment4");
            driver.FindElement(By.CssSelector("[data-e2e=\"commentAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);
            GoToAdmin("tasks");
            GetGridCell(3, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("editTaskForm"));
            ScrollTo(By.CssSelector("[data-e2e=\"edittaskAttachment\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"commentEdit\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"commentEditText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"commentEditText\"]")).SendKeys("changed");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"commentSave\"]")).Click();
            Thread.Sleep(2000);
             driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
           Thread.Sleep(2000);
            GoToAdmin("tasks");
            GetGridCell(3, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Name("editTaskForm"));
            
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"commentText\"]")).Text.Contains("changed"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"commentText\"]")).Text.Contains("TestComment4"));
        }

    }
}
