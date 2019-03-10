using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Settings.BizProcesses
{
    [TestFixture]
    public class SettingsBizProcessLeadStatusRuleTest : BaseMultiSeleniumTest
    {

        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Catalog | ClearType.Customers);
            InitializeService.LoadData(
                    "data\\Admin\\Settings\\BizProcessLead\\Catalog.Product.csv",
        "data\\Admin\\Settings\\BizProcessLead\\Catalog.Offer.csv",
        "data\\Admin\\Settings\\BizProcessLead\\Catalog.Category.csv",
        "data\\Admin\\Settings\\BizProcessLead\\Catalog.ProductCategories.csv",
        "data\\Admin\\Settings\\BizProcessLead\\Customers.Customer.csv",
        "data\\Admin\\Settings\\BizProcessLead\\Customers.CustomerGroup.csv",
           "data\\Admin\\Settings\\BizProcessLead\\Customers.Country.csv",
            "data\\Admin\\Settings\\BizProcessLead\\Customers.Region.csv",
            "data\\Admin\\Settings\\BizProcessLead\\Customers.City.csv",
            "data\\Admin\\Settings\\BizProcessLead\\Customers.Contact.csv",
                "data\\Admin\\Settings\\BizProcessLead\\Customers.Departments.csv",
            "data\\Admin\\Settings\\BizProcessLead\\Customers.Managers.csv",
                "data\\Admin\\Settings\\BizProcessLead\\CRM.DealStatus.csv",
            "data\\Admin\\Settings\\BizProcessLead\\[Order].OrderSource.csv",
                "data\\Admin\\Settings\\BizProcessLead\\[Order].LeadCurrency.csv",
                "data\\Admin\\Settings\\BizProcessLead\\[Order].LeadEvent.csv",
                    "data\\Admin\\Settings\\BizProcessLead\\[Order].LeadItem.csv",
                "data\\Admin\\Settings\\BizProcessLead\\[Order].Lead.csv",
        "data\\Admin\\Settings\\BizProcessLead\\Customers.TaskGroup.csv",
        "data\\Admin\\Settings\\BizProcessLead\\Customers.Task.csv"

           );

            Init();
        }



        [Test]
        public void LeadStatusFilterSum()
        {
            testname = "BizProcessLeadStatusFilterSum";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).GetAttribute("innerText").Contains("Смена этапа лида"), "biz rule type");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Созвон с клиентом");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Бюджет");

            driver.FindElement(By.LinkText("Указать диапазон")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueFrom\"]")).SendKeys("50");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueTo\"]")).SendKeys("100");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name sum");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text sum");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("6");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В днях");
            
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Высокий");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);

            //check rules grid
            GoToAdmin("settingstasks");

            VerifyAreEqual("Смена этапа лида на Созвон с клиентом", GetGridCell(0, "EventName", "LeadStatusChangedRules").Text, "biz rule grid type");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "LeadStatusChangedRules").Text, "biz rule grid manager");
            VerifyAreEqual("6 дней", GetGridCell(0, "TaskDueDateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task due time");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task interval");
            VerifyAreEqual("0", GetGridCell(0, "Priority", "LeadStatusChangedRules").Text, "biz rule grid priority");

            setLeadBuyInOneClick();

            //client
            ReInitClient();
            leadBuyOneClick("63", "Task filter sum", "1231231122");

            ReInit();

            //check task not added yet 
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name sum");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created yet");
            
            changeLeadStatus("121", "Созвон с клиентом");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name sum");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name sum", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Высокий", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name sum", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text sum", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Высокий"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 121"), "lead h1");
            
            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct63"), "lead product");
            VerifyAreEqual("Task name sum", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Созвон с клиентом"), "lead status");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadStatusFilterFIO()
        {
            testname = "BizProcessLeadStatusFilterFIO";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Созвон с клиентом");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Имя");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("FirstName");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Фамилия");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("LastName");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Отчество");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Patronymic");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name test FIO filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text test FIO filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("20");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В минутах");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            
            changeLeadStatus("77", "Созвон с клиентом");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name test FIO filter");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name test FIO filter", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name test FIO filter", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text test FIO filter", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 77"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Созвон с клиентом"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct77"), "lead product");
            VerifyAreEqual("Task name test FIO filter", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadStatusFilterEmail()
        {
            testname = "BizProcessLeadStatusFilterEmail";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Выставление КП");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Email");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("mail@mail.com");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name test Email filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text test Email filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            
            changeLeadStatus("1", "Выставление КП");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name test Email filter");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name test Email filter", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name test Email filter", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text test Email filter", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 1"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Выставление КП"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct1"), "lead product");
            VerifyAreEqual("Task name test Email filter", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadStatusFilterPhone()
        {
            testname = "BizProcessLeadStatusFilterPhone";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Выставление КП");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Телефон");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("+7(999)999-99-99");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name test Phone filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text test Phone filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);

            /* create lead from client */
            setLeadBuyInOneClick();

            //client
            ReInitClient();
            leadBuyOneClick("25", "Task filter Phone", "9999999999");

            //check task not added yet 
            ReInit();
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name test Phone filter");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created yet");

            changeLeadStatus("121", "Выставление КП");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name test Phone filter");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name test Phone filter", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name test Phone filter", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text test Phone filter", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 121"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Выставление КП"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct25"), "lead product");
            VerifyAreEqual("Task name test Phone filter", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadStatusFilterCustomerGroup()
        {
            testname = "BizProcessLeadStatusFilterCustomerGroup";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Выставление КП");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Группа покупателя");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("CustomerGroup2");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name test CustomerGroup filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text test CustomerGroup filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            
            changeLeadStatus("2", "Выставление КП");
            
            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name test CustomerGroup filter");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name test CustomerGroup filter", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name test CustomerGroup filter", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text test CustomerGroup filter", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 2"), "lead h1");
            
            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Выставление КП"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct2"), "lead product");
            VerifyAreEqual("Task name test CustomerGroup filter", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadStatusFilterSource()
        {
            testname = "BizProcessLeadStatusFilterSource";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Выставление КП");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Источник");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("Оффлайн");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name test Source filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text test Source filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            
            changeLeadStatus("100", "Выставление КП");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name test Source filter");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name test Source filter", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name test Source filter", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text test Source filter", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 100"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Выставление КП"), "lead status");

            IWebElement selectElemLeadSource = driver.FindElement(By.Id("Lead_OrderSourceId"));
            SelectElement select5 = new SelectElement(selectElemLeadSource);
            VerifyIsTrue(select5.SelectedOption.Text.Contains("Оффлайн"), "lead source");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct100"), "lead product");
            VerifyAreEqual("Task name test Source filter", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadStatusFilterCountryRegionCity()
        {
            testname = "BizProcessLeadStatusFilterCountryRegionCity";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Ожидание решения клиента");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Страна");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Россия");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Регион");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Московская область");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Город");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Москва");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name test CountryRegionCity filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text test CountryRegionCity filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            
            changeLeadStatus("3", "Ожидание решения клиента");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name test CountryRegionCity filter");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name test CountryRegionCity filter", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name test CountryRegionCity filter", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text test CountryRegionCity filter", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 3"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Ожидание решения клиента"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct3"), "lead product");
            VerifyAreEqual("Task name test CountryRegionCity filter", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }


        [Test]
        public void LeadStatusFilterCreateFromAdmin()
        {
            testname = "BizProcessLeadStatusFilterCreateFromAdmin";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Ожидание решения клиента");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Создан в части администрирования");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name lead from admin");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text lead from admin");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("1");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В часах");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);

            //check rules grid
            GoToAdmin("settingstasks");

            VerifyAreEqual("Смена этапа лида на Ожидание решения клиента", GetGridCell(0, "EventName", "LeadStatusChangedRules").Text, "biz rule grid type");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerFilterHTML", "LeadStatusChangedRules").Text, "biz rule grid manager");
            VerifyAreEqual("1 час", GetGridCell(0, "TaskDueDateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task due time");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task interval");
            VerifyAreEqual("0", GetGridCell(0, "Priority", "LeadStatusChangedRules").Text, "biz rule grid priority");

            //create lead from admin
            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("лид из администрированной части");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+79729272728");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");

            GetGridCell(4, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            XPathContainsText("button", "Выбрать");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(4000);

            //check task not added yet 
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name lead from admin");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created yet");

            changeLeadStatus("121", "Ожидание решения клиента");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name lead from admin");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name lead from admin", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Низкий", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name lead from admin", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text lead from admin", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("test testov"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Низкий"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 121"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Ожидание решения клиента"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct5"), "lead product");
            VerifyAreEqual("Task name lead from admin", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }


        [Test]
        public void LeadStatusFilterDesc()
        {
            testname = "BizProcessLeadStatusFilterDesc";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Ожидание решения клиента");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Описание");

            driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.TagName("input")).SendKeys("Desc33");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name lead with description");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text lead with description");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("1");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В часах");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);

            //check rules grid
            GoToAdmin("settingstasks");

            VerifyAreEqual("Смена этапа лида на Ожидание решения клиента", GetGridCell(0, "EventName", "LeadStatusChangedRules").Text, "biz rule grid type");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerFilterHTML", "LeadStatusChangedRules").Text, "biz rule grid manager");
            VerifyAreEqual("1 час", GetGridCell(0, "TaskDueDateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task due time");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task interval");
            VerifyAreEqual("0", GetGridCell(0, "Priority", "LeadStatusChangedRules").Text, "biz rule grid priority");
            
            changeLeadStatus("33", "Ожидание решения клиента");

            //check task added 
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name lead with description");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Task name lead with description", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Низкий", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Task name lead with description", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Task text lead with description", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("test testov"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Низкий"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 33"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select4 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Ожидание решения клиента"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct33"), "lead product");
            VerifyAreEqual("Task name lead with description", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            VerifyAreEqual("Desc33", driver.FindElement(By.Id("Lead_Description")).GetAttribute("value"), "lead's task description");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadStatusFilterCreateFromAdminValueNot()
        {
            testname = "BizProcessLeadStatusFilterCreateFromAdminNoValueNot";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Ожидание решения клиента");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Создан в части администрирования");

            driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.CssSelector(".adv-checkbox-label")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("Task name lead from admin - not");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("Task text lead from admin - not");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("1");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В часах");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);

            //check rules grid
            GoToAdmin("settingstasks");

            VerifyAreEqual("Смена этапа лида на Ожидание решения клиента", GetGridCell(0, "EventName", "LeadStatusChangedRules").Text, "biz rule grid type");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerFilterHTML", "LeadStatusChangedRules").Text, "biz rule grid manager");
            VerifyAreEqual("1 час", GetGridCell(0, "TaskDueDateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task due time");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "LeadStatusChangedRules").Text, "biz rule grid task interval");
            VerifyAreEqual("0", GetGridCell(0, "Priority", "LeadStatusChangedRules").Text, "biz rule grid priority");

            //create lead
            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("лид из администрированной части");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+79729272729");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");

            GetGridCell(4, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            XPathContainsText("button", "Выбрать");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(4000);

            //check task not added yet 
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name lead from admin - not");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created yet");

            changeLeadStatus("121", "Ожидание решения клиента");

            //check task not added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Task name lead from admin - not");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created");

            VerifyFinally(testname);
        }


        [Test]
        public void LeadStatusFilterVariables()
        {
            testname = "BizProcessLeadStatusFilterVariables";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"LeadCreated\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"LeadStatusChanged\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Ожидание решения клиента");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Группа покупателя");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("CustomerGroup2");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("название задачи - #LEAD_ID#, #NAME#, #PHONE#, #EMAIL#");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("текст задачи - #LEAD_ID#, #NAME#, #PHONE#, #EMAIL#");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("1");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В часах");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);

            //check rule pop up
            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"OrderStatusChanged\"]"));
            GetGridCell(0, "_serviceColumn", "LeadStatusChangedRules").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.CssSelector(".modal-body"));

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text.Contains("Смена этапа лида на Ожидание решения клиента"), "rule pop up type");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text.Contains("Группа покупателя = CustomerGroup2"), "rule pop up filter");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text.Contains("test testov"), "rule pop up manager");
            VerifyAreEqual("название задачи - #LEAD_ID#, #NAME#, #PHONE#, #EMAIL#", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).GetAttribute("value"), "rule pop up task name");
            VerifyAreEqual("текст задачи - #LEAD_ID#, #NAME#, #PHONE#, #EMAIL#", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).GetAttribute("value"), "rule pop up task text");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateInput\"]")).Selected, "rule pop up task due time in use");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).GetAttribute("value"), "rule pop up task due time");

            IWebElement selectElemDueDateSelect = driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]"));
            SelectElement select = new SelectElement(selectElemDueDateSelect);
            VerifyIsTrue(select.SelectedOption.Text.Contains("В часах"), "rule pop up task due time value");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskCreateDateInput\"]")).Selected, "rule pop up task create time not in use");

            IWebElement selectElemTaskPriority = driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]"));
            SelectElement select1 = new SelectElement(selectElemTaskPriority);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("Низкий"), "rule pop up task priority");

            IWebElement selectElemTaskProj = driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]"));
            SelectElement select2 = new SelectElement(selectElemTaskProj);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("По умолчанию"), "rule pop up task group");

            changeLeadStatus("10", "Ожидание решения клиента");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("название задачи - 10, LastName FirstName Patronymic, +7 495 800 200 01, mail@mail.com");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("название задачи - 10, LastName FirstName Patronymic, +7 495 800 200 01, mail@mail.com", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Низкий", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("название задачи - 10, LastName FirstName Patronymic, +7 495 800 200 01, mail@mail.com", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("текст задачи - 10, LastName FirstName Patronymic, +7 495 800 200 01, mail@mail.com", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select3 = new SelectElement(selectElemUser);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("test testov"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select4 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Низкий"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select5 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select5.SelectedOption.Text.Contains("TaskGroup1"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 10"), "lead h1");

            IWebElement selectElemDealStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select6 = new SelectElement(selectElemDealStatus);
            VerifyIsTrue(select6.SelectedOption.Text.Contains("Ожидание решения клиента"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct10"), "lead product");
            VerifyAreEqual("название задачи - 10, LastName FirstName Patronymic, +7 495 800 200 01, mail@mail.com", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        public void changeLeadStatus(string leadId, string dealStatus)
        {
            GoToAdmin("leads/edit/" + leadId);
            (new SelectElement(driver.FindElement(By.Id("Lead_DealStatusId")))).SelectByText(dealStatus);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
        }
    }
}