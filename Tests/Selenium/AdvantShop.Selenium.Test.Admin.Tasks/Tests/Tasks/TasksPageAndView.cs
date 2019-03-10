using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Tasks
{
    [TestFixture]
    public class TasksPageAndView : BaseSeleniumTest
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
            "data\\Admin\\Tasks\\ManyTasks\\Customers.TaskGroup.csv",
            "data\\Admin\\Tasks\\ManyTasksPage\\Customers.Task.csv"
           );
             
            Init();
        }
        
        [Test]
        public void TaskOnPagePresentd10()
        {
            GoToAdmin("tasks");
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));

        }
        [Test]
        public void TaskOnPagePresent20()
        {
            GoToAdmin("tasks");
            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(19, "Name").FindElement(By.TagName("span")).Text.Contains("test20"));

        }
        [Test]
        public void TaskOnPagePresent50()
        {
            GoToAdmin("tasks");
            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(49, "Name").Text.Contains("test50"));

        }
        [Test]
        public void TaskOnPagePresent100()
        {
            GoToAdmin("tasks");
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(99, "Name").Text.Contains("test100"));

        }
        [Test]
        public void SelectTasks()
        {
            GoToAdmin("tasks");
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
          }
        [Test]
        public void SelectAllOnPageTasks()
        {
            GoToAdmin("tasks");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"-1\"] [data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
        }
        [Test]
        public void SelectAllTasks()
        {
            GoToAdmin("tasks");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"-1\"] [data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("114", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);
        }

        [Test]
        public void SelectAllTasksCancel()
        {
            GoToAdmin("tasks");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"-1\"] [data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click(); //снять выделение 
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
        }

        //Паджинация
        [Test]
        public void PageTasks()
        {
            GoToAdmin("tasks");
              Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test20"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test21"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test30"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test31"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test40"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test41"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test50"));


            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            //to begin
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));
            
        }
        
        [Test]
        public void PageTaskToPrev()
        {
            GoToAdmin("tasks");
              Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test20"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test21"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test30"));
            
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test20"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(9, "Name").FindElement(By.TagName("span")).Text.Contains("test10"));

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test111"));
            Assert.IsTrue(GetGridCell(3, "Name").Text.Contains("test114"));
        }
        
    }
}
