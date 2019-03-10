using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Tasks
{
    [TestFixture]
    public class TasksFilterTest : BaseSeleniumTest
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
            "data\\Admin\\Tasks\\Customers.ManagerTask.csv",
            "data\\Admin\\Tasks\\Customers.Task.csv",
            "data\\Admin\\Tasks\\Customers.TaskGroup.csv",
          "data\\Admin\\Tasks\\Customers.ViewedTask.csv"
           );
             
            Init();
        }
        
        [Test]
        public void FilterByCreate()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnDateCreated");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("08.08.2016 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("15.08.2016 21:00");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").Text.Contains("test13"));

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnDateCreated");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));

        }

        [Test]
        public void FilterByPriority()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnPriorities");
           
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Низкий");
            
              Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Средний");
            
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test2"));
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Высокий");
           
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("test3"));
            Assert.IsTrue(GetGridCell(1, "Name").Text.Contains("test12"));

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnPriorities");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));

        }
            
        [Test]
        public void FilterByDueDate()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnDueDateFormatted");
           
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("08.08.2016 00:00");
           
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("15.08.2016 21:00");
           
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnDueDateFormatted");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }

        [Test]
        public void FilterByAssigned()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnAssigned");
                     
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Admin Ad");
          
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnAssigned");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }

        [Test]
        public void FilterByAppointed()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnAppointed");
                      
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Admin Ad");

            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnAppointed");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }
        //убрали функционал
        /*
        [Test]
        public void FilterByStatus()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnStatuses");
                        
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("В работе");
           
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(3, "Name").FindElement(By.TagName("span")).Text.Contains("test4"));
            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnStatuses");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }

        [Test]
        public void FilterByPriorityandStatus()
        {
            GoToAdmin("tasks");
           Functions.GridFilterSet(driver, baseURL, "_noopColumnPriorities");
                      
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Низкий");
          
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));

            Functions.GridFilterSet(driver, baseURL, "_noopColumnStatuses");
                       
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e-grid-filter-block-name=\"_noopColumnStatuses\"] [data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("В работе (выполняется)");
           
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));
            //close priority
            Functions.GridFilterClose(driver, baseURL, "_noopColumnPriorities");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));

            //close status
            Functions.GridFilterClose(driver, baseURL, "_noopColumnStatuses");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }
        */
        [Test]
        public void FilterByAssignedandAppointed()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnAppointed");
                       
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Admin Ad");
           
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));
            
            Functions.GridFilterSet(driver, baseURL, "_noopColumnAssigned");
                       
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e-grid-filter-block-name=\"_noopColumnAssigned\"] [data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Admin Ad");
           
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));

            //close appointed
            Functions.GridFilterClose(driver, baseURL, "_noopColumnAppointed");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));

            //close assigned
            Functions.GridFilterClose(driver, baseURL, "_noopColumnAssigned");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }

        [Test]
        public void FilterByDueDateandAssigned()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnDueDateFormatted");
           
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("07.11.2016 00:00");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("11.11.2016 21:00");
           
                        
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test2"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test4"));

            Functions.GridFilterSet(driver, baseURL, "_noopColumnAssigned");
           
            
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("test testov");
           
              Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test2"));
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));

            //close duedate
            Functions.GridFilterClose(driver, baseURL, "_noopColumnDueDateFormatted");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));

            //close assign
            Functions.GridFilterClose(driver, baseURL, "_noopColumnAssigned");
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
        }

        [Test]
        public void FilterByViewed()
        {
            GoToAdmin("tasks");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnViewed");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".ng-tab.nav-item.active")).Text.Contains("В работе"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            WaitForAjax();
            Assert.AreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).Text.Contains("test2"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
            Assert.IsTrue(GetGridCell(3, "Name").FindElement(By.TagName("span")).Text.Contains("test4"));
            Assert.IsTrue(GetGridCell(4, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            Assert.IsTrue(GetGridCell(0, "StatusFormatted").Text.Contains("В работе"));
            Assert.IsTrue(GetGridCell(1, "StatusFormatted").Text.Contains("В работе"));
            Assert.IsTrue(GetGridCell(2, "StatusFormatted").Text.Contains("В работе"));
            Assert.IsTrue(GetGridCell(3, "StatusFormatted").Text.Contains("В работе"));
            Assert.IsTrue(GetGridCell(4, "StatusFormatted").Text.Contains("В работе (выполняется)"));
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("400")); //400 means normal text
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("400"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("400"));
            Assert.IsTrue(GetGridCell(3, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("400"));
            Assert.IsTrue(GetGridCell(4, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("400"));

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            WaitForAjax();
            Assert.AreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test14"));
            Assert.IsTrue(GetGridCell(0, "StatusFormatted").Text.Contains("В работе (выполняется)"));
            Assert.IsTrue(GetGridCell(1, "StatusFormatted").Text.Contains("В работе (выполняется)"));
            Assert.IsTrue(GetGridCell(2, "StatusFormatted").Text.Contains("В работе (выполняется)"));
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("700")); //700 means bold text
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("700"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).GetCssValue("font-weight").Contains("700"));

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnViewed");
            WaitForAjax();
            Assert.AreEqual("Найдено записей: 8", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.IsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("test1"));
            Assert.IsTrue(GetGridCell(1, "Name").FindElement(By.TagName("span")).Text.Contains("test2"));
            Assert.IsTrue(GetGridCell(2, "Name").FindElement(By.TagName("span")).Text.Contains("test3"));
            Assert.IsTrue(GetGridCell(4, "Name").FindElement(By.TagName("span")).Text.Contains("test11"));
            Assert.IsTrue(GetGridCell(5, "Name").FindElement(By.TagName("span")).Text.Contains("test12"));
            Assert.IsTrue(GetGridCell(6, "Name").FindElement(By.TagName("span")).Text.Contains("test13"));
            Assert.IsTrue(GetGridCell(7, "Name").FindElement(By.TagName("span")).Text.Contains("test14"));

        }
    }
}
