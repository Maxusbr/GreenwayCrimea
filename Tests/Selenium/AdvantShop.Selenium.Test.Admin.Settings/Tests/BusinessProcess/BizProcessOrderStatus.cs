using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Selenium.Test.Admin.Settings.Tests.BusinessProcess
{
    [TestFixture]
    public class BizProcessOrderStatus : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Customers | ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.CustomerGroup.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.Country.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.Region.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.City.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.Customer.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.Contact.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.Departments.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.Managers.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.ManagerTask.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.TaskGroup.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Customers.ViewedTask.csv",

            "Data\\Admin\\SettingTasks\\OrderStatus\\Catalog.Product.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Catalog.Offer.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Catalog.Category.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\Catalog.ProductCategories.csv",

            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].OrderContact.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].OrderSource.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].OrderCurrency.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].OrderItems.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].OrderStatus.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].PaymentMethod.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].ShippingMethod.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].[Order].csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\CRM.DealStatus.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].LeadCurrency.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].LeadEvent.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].LeadItem.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].Lead.csv",
            "Data\\Admin\\SettingTasks\\OrderStatus\\[Order].OrderCustomer.csv"

           );

            Init();
        }
        [Test]
        public void OpenPageSettingTask()
        {

            GoToAdmin("settingstasks");
            testname = "OpenPageSettingTask";
            VerifyBegin(testname);

            VerifyAreEqual("Общие", driver.FindElement(By.TagName("h3")).Text, " open page h3 - common");
            VerifyAreEqual("Бизнес-процессы", driver.FindElements(By.TagName("h3"))[1].Text, " open page h3 -business process");

            IWebElement selectElem1 = driver.FindElement(By.Id("DefaultTaskGroupId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("All"), "select default task group");
            VerifyIsTrue(driver.FindElements(By.TagName("h4")).Count > 0, "count business process");

            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskNewStatusNew()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewStatusNew";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderStatusChanged\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Смена статуса заказа", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Новый");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("15");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Фамилия");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Ad");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Фамилия = Ad", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Имя");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Admin");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
                    

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order FIO #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида
            /*
            VerifyAreEqual("Смена статуса заказа на Новый", GetGridCell(0, "EventName", "OrderStatusChangedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderStatusChangedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderStatusChangedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderStatusChangedRules").Text, "create");
            VerifyAreEqual("15", GetGridCell(0, "Priority", "OrderStatusChangedRules").Text, "priority");
          */
            /*negative*/
            GoToAdmin("orders");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("Отменён");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);

            GoToAdmin("tasks");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
          
            /*positive*/
            GoToAdmin("orders");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("Новый");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);
           
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order FIO 30"), "name new task in grid");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e-col-index=\"1\"]")).Count == 2, "count new tasks");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            GetGridCell(0, "Name").FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            AssertCkText("New description 30, Ad Admin, admin", "editor1");
            VerifyAreEqual("New Order FIO 30", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "name task");
            VerifyAreEqual("Заказ №30", driver.FindElement(By.CssSelector("[data-e2e=\"orderLink\"]")).Text, "order");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Elena El"), "select assigned");

            selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Низкий"), "select priority");

            selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("All"), "select project");

            VerifyFinally(testname);

        }
        [Test]
        public void SettingTaskStatusObr()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskStatusObr";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderStatusChanged\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Смена статуса заказа", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("В обработке");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("14");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Сумма заказа");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("30");
            

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Сумма заказа = 30", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Admin Ad");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Admin Ad", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Sum #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Высокий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("5");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В днях");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида
            /*
            VerifyAreEqual("Смена статуса заказа на В обработке", GetGridCell(0, "EventName", "OrderStatusChangedRules").Text, "name");
            VerifyAreEqual("Admin Ad", GetGridCell(0, "ManagerFilterHTML", "OrderStatusChangedRules").Text, "manager name");
            VerifyAreEqual("5 дней", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderStatusChangedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderStatusChangedRules").Text, "create");
            VerifyAreEqual("14", GetGridCell(0, "Priority", "OrderStatusChangedRules").Text, "priority");
            */
            /*positive*/
            GoToAdmin("orders");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("В обработке");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);

            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Sum"), "name new task in grid");
            VerifyAreEqual("Высокий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Admin Ad", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");
         

            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskStatusShipping()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskStatusShipping";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderStatusChanged\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Смена статуса заказа", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Отправлен");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("13");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Группа покупателя");
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("CustomerGroup1");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Группа покупателя = CustomerGroup1", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");


            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Group #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида
            /*
            VerifyAreEqual("Смена статуса заказа на Отправлен", GetGridCell(0, "EventName", "OrderStatusChangedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderStatusChangedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderStatusChangedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderStatusChangedRules").Text, "create");
            VerifyAreEqual("13", GetGridCell(0, "Priority", "OrderStatusChangedRules").Text, "priority");
            */
            /*positive*/
            GoToAdmin("orders");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("Отправлен");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);

            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Group"), "name new task in grid");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskStatuszDeliver()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderSource";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderStatusChanged\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Смена статуса заказа", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Доставлен");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("9");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Email");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("admin");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Email = admin", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Источник заказа");
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("Мобильная версия");


            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Source #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("24");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В часах");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида
            /*
            VerifyAreEqual("Смена статуса заказа на Доставлен", GetGridCell(0, "EventName", "OrderStatusChangedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderStatusChangedRules").Text, "manager name");
            VerifyAreEqual("24 часа", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderStatusChangedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderStatusChangedRules").Text, "create");
            VerifyAreEqual("9", GetGridCell(0, "Priority", "OrderStatusChangedRules").Text, "priority");
          */
            /*negative*/
            GoToAdmin("orders");
            GetGridCell(1, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("Доставлен");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);

            GoToAdmin("tasks");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Source"), "name new task in grid");

            /*positive*/
            GoToAdmin("orders");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("Доставлен");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);

            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Source"), "name new task in grid");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            VerifyFinally(testname);
        }

        /*
        [Test]
        public void SettingTaskStatuzCancel()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderTel";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderStatusChanged\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Смена статуса заказа", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Отменён");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("8");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Телефон");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("+74958002001");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Телефон = +74958002001", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");


            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Phone #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида
            
            VerifyAreEqual("Смена статуса заказа на Отменён", GetGridCell(0, "EventName", "OrderStatusChangedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderStatusChangedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderStatusChangedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderStatusChangedRules").Text, "create");
            VerifyAreEqual("8", GetGridCell(0, "Priority", "OrderStatusChangedRules").Text, "priority");
            
            //positive
            Functions.NewOrderClient_450(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Phone"), "name new task in grid");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            VerifyFinally(testname);
        }
    */
        [Test]
        public void SettingTaskStatuzCancelAll()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderzFromLead";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderStatusChanged\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Смена статуса заказа", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTypeStatus\"]")))).SelectByText("Отменен навсегда");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("6");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Создан из лида");
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Создан из лида = Да", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");


            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Lead #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида
/*
            VerifyAreEqual("Смена статуса заказа на Отменен навсегда", GetGridCell(0, "EventName", "OrderStatusChangedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderStatusChangedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderStatusChangedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderStatusChangedRules").Text, "create");
            VerifyAreEqual("6", GetGridCell(0, "Priority", "OrderStatusChangedRules").Text, "priority");
            */
            /*negative*/
            GoToAdmin("orders");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("Отменен навсегда");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);

            GoToAdmin("tasks");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Lead"), "name new task in grid");
                     
            /*positive*/
            GoToAdmin("leads/edit/120");
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadCreateOrder\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.Id("Order_OrderStatusId")))).SelectByText("Отменен навсегда");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Lead"), "name new task in grid");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            VerifyFinally(testname);
        }
    }
}
