using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadAddEditEventTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Catalog | ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\CRM\\Lead\\LeadEvent\\Catalog.Product.csv",
           "data\\Admin\\CRM\\Lead\\LeadEvent\\Catalog.Offer.csv",
           "data\\Admin\\CRM\\Lead\\LeadEvent\\Catalog.Category.csv",
           "data\\Admin\\CRM\\Lead\\LeadEvent\\Catalog.ProductCategories.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\CRM.DealStatus.csv",
           "data\\Admin\\CRM\\Lead\\LeadEvent\\[Order].OrderSource.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\[Order].LeadCurrency.csv",
           "data\\Admin\\CRM\\Lead\\LeadEvent\\[Order].LeadEvent.csv",
          "data\\Admin\\CRM\\Lead\\LeadEvent\\[Order].LeadItem.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\[Order].Lead.csv",
          "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.CustomerGroup.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.Country.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.Region.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.City.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.Customer.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.Contact.csv",
            "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.Departments.csv",
           "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.Managers.csv",
           "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.TaskGroup.csv",
         "data\\Admin\\CRM\\Lead\\LeadEvent\\Customers.Task.csv"

          );

            Init();
        }
        
        [Test]
        public void AddTask()
        {
            testname = "LeadAddEventTask";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/6");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTasks\"]")).Text.Contains("Ни одной записи не найдено"), "lead no tasks at first");
            VerifyIsTrue(driver.FindElement(By.TagName("tasks-grid")).Text.Contains("Найдено задач: 0"), "lead no tasks count at first");
            
            ScrollTo(By.XPath("//h1[contains(text(), 'Задачи')]"));
            driver.FindElement(By.TagName("tasks-grid")).FindElement(By.CssSelector(".btn.btn-sm.btn-success")).Click();
            Thread.Sleep(4000);

            VerifyAreEqual("Новая задача", driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.TagName("h2")).Text, "add task pop up");

            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskName\"]")).SendKeys("Lead Task Test");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"newtaskAssigned\"]")))).SelectByText("ManagerName2 ManagerLastName2");

            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDescription\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDescription\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDescription\"]")).SendKeys("Lead Task Description Test");

            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDuedate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDuedate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"newtaskDuedate\"]")).SendKeys("25.02.2030 19:55");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"newtaskPriority\"]")))).SelectByText("Высокий");

            DropFocus("h2");
            XPathContainsText("span", "Добавить");

            GoToAdmin("leads/edit/6");

            //check lead details
            VerifyAreEqual("6", GetGridCell(0, "Id", "Tasks").Text, "task Id");
            VerifyAreEqual("Lead Task Test", GetGridCell(0, "Name", "Tasks").Text, "task Name");
            VerifyAreEqual("25.02.2030 19:55", GetGridCell(0, "DueDateFormatted", "Tasks").Text, "task DueDate");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "task Status");
            VerifyAreEqual("ManagerName2 ManagerLastName2", GetGridCell(0, "AssignedName", "Tasks").Text, "task AssignedName");
            VerifyAreEqual("Admin Ad", GetGridCell(0, "AppointedName", "Tasks").Text, "task AppointedName");

            VerifyIsTrue(driver.FindElement(By.TagName("tasks-grid")).Text.Contains("Найдено задач: 1"), "task added count");

            VerifyIsTrue(driver.FindElement(By.TagName("lead-events")).Text.Contains("Задача добавлена"), "task added saved as event");

            //check tasks grid 
            GoToAdmin("tasks");

            VerifyIsTrue(driver.PageSource.Contains("Lead Task Test"), "grid task");

            VerifyFinally(testname);
        }

        [Test]
        public void AddComment()
        {
            testname = "LeadAddEventComment";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/11");

            ScrollTo(By.TagName("lead-events"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEvent\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEvent\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEvent\"]")).SendKeys("new comment test");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEventAdd\"]")).Click();
            Thread.Sleep(3000);

            GoToAdmin("leads/edit/11");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LeadEventTitle\"]")).Text.Contains("Комментарий"), "event added title");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LeadEventType\"]")).Text.Contains("new comment test"), "event added comment text");

            VerifyFinally(testname);
        }

        [Test]
        public void TaskComplete()
        {
            testname = "LeadAddEventTaskComplete";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/5");

            IWebElement selectElem = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select = new SelectElement(selectElem);
            string dealStatusBegin = select.AllSelectedOptions[0].Text;

            // ScrollTo(By.TagName("tasks-grid"));
            ScrollTo(By.XPath("//h1[contains(text(), 'Задачи')]"));
            VerifyAreEqual("Task5", GetGridCell(0, "Name", "Tasks").Text, "pre check task Name");

            GetGridCell(0, "Name", "Tasks").Click();
            Thread.Sleep(3000);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskStatusCompleted\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"taskResult\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taskResult\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taskResult\"]")).SendKeys("test Result lead's task");

            XPathContainsText("span", "Завершить");
            Thread.Sleep(2000);

            //check lead details
            GoToAdmin("leads/edit/5");

            VerifyAreEqual("Завершена", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead task completed Status");
            VerifyIsTrue(driver.FindElement(By.TagName("lead-events")).Text.Contains("Задача завершена"), "lead task completed event");
            VerifyIsTrue(driver.FindElement(By.TagName("lead-events")).Text.Contains("Задача \"Task5\" завершена. Результат выполнения задачи: test Result lead's task"), "lead task completed description event");

            IWebElement selectElem2 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select2 = new SelectElement(selectElem2);
            string dealStatusEnd = select2.AllSelectedOptions[0].Text;

            VerifyIsTrue(dealStatusBegin.Equals(dealStatusEnd), "lead deal status not changed");

            //ScrollTo(By.TagName("tasks-grid"));
            ScrollTo(By.XPath("//h1[contains(text(), 'Задачи')]"));

            // GetGridCell(0, "Name", "Tasks").Click();
            GetGridCell(0, "_serviceColumn", "Tasks").FindElement(By.TagName("ui-modal-trigger")).Click();
            WaitForElem(By.XPath("//h2[contains(text(), 'Задача')]"));
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"edittaskRezult\"]")).Text.Contains("test Result lead's task"), "Result lead's task saved");

            //check tasks grid 
            GoToAdmin("tasks?filterby=completed");
            VerifyAreEqual("Завершена", GetGridCell(0, "StatusFormatted").Text, "grid task completed Status");

            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Task5"), "grid task no");

            VerifyFinally(testname);
        }

        [Test]
        public void TaskCompleteChangeDealStatus()
        {
            testname = "TaskCompleteChangeDealStatus";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/4");

            IWebElement selectElem = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select = new SelectElement(selectElem);
            VerifyIsTrue(select.AllSelectedOptions[0].Text.Contains("Новый"), "lead deal status default");

            // ScrollTo(By.TagName("tasks-grid"));
            ScrollTo(By.XPath("//h1[contains(text(), 'Задачи')]"));

            GetGridCell(0, "Name", "Tasks").Click();
            Thread.Sleep(3000);

            driver.FindElement(By.CssSelector("[data-e2e=\"edittaskStatusCompleted\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"taskResult\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taskResult\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taskResult\"]")).SendKeys("Result");
            DropFocus("h2");

            (new SelectElement(driver.FindElement(By.TagName("select")))).SelectByText("Ожидание решения клиента");

            XPathContainsText("span", "Завершить");
            Thread.Sleep(5000);
            
            //check lead details
            GoToAdmin("leads/edit/4");

            VerifyAreEqual("Завершена", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead task completed Status");

            IWebElement selectElem1 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.AllSelectedOptions[0].Text.Contains("Ожидание решения клиента"), "lead deal status changed");

            VerifyFinally(testname);
        }
        

        [Test]
        public void LeadEditDealStatus()
        {
            testname = "LeadEditDealStatus";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/80");

            IWebElement selectElem1 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.AllSelectedOptions[0].Text.Contains("Выставление КП"), "pre check lead deal status");

            // (new SelectElement(driver.FindElement(By.Id("Lead_DealStatusId")))).SelectByText("Ожидание решения клиента");
            (new SelectElement(driver.FindElement(By.Id("Lead_DealStatusId")))).SelectByValue("4");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(3000);

            GoToAdmin("leads/edit/80");

            IWebElement selectElem2 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("Ожидание решения клиента"), "lead deal status edited");

            VerifyIsTrue(driver.FindElement(By.TagName("lead-events")).Text.Contains("Этап сделки изменен на \"Ожидание решения клиента\""), "lead deal status event");

            VerifyFinally(testname);
        }
    }
}