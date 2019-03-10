using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Settings.BusinessProcess
{
    [TestFixture]
    public class BizProcessNewOrder : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Customers | ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
            "Data\\Admin\\SettingTasks\\Customers.CustomerGroup.csv",
            "Data\\Admin\\SettingTasks\\Customers.Country.csv",
            "Data\\Admin\\SettingTasks\\Customers.Region.csv",
            "Data\\Admin\\SettingTasks\\Customers.City.csv",
            "Data\\Admin\\SettingTasks\\Customers.Customer.csv",
            "Data\\Admin\\SettingTasks\\Customers.Contact.csv",
            "Data\\Admin\\SettingTasks\\Customers.Departments.csv",
            "Data\\Admin\\SettingTasks\\Customers.Managers.csv",
            "Data\\Admin\\SettingTasks\\Customers.ManagerTask.csv",
            "Data\\Admin\\SettingTasks\\Customers.TaskGroup.csv",
            "Data\\Admin\\SettingTasks\\Customers.ViewedTask.csv",

            "Data\\Admin\\SettingTasks\\Catalog.Product.csv",
            "Data\\Admin\\SettingTasks\\Catalog.Offer.csv",
            "Data\\Admin\\SettingTasks\\Catalog.Category.csv",
            "Data\\Admin\\SettingTasks\\Catalog.ProductCategories.csv",

            "Data\\Admin\\SettingTasks\\[Order].OrderContact.csv",
            "Data\\Admin\\SettingTasks\\[Order].OrderSource.csv",
            "Data\\Admin\\SettingTasks\\[Order].OrderCurrency.csv",
            "Data\\Admin\\SettingTasks\\[Order].OrderItems.csv",
            "Data\\Admin\\SettingTasks\\[Order].OrderStatus.csv",
            "Data\\Admin\\SettingTasks\\[Order].PaymentMethod.csv",
            "Data\\Admin\\SettingTasks\\[Order].ShippingMethod.csv",
            "Data\\Admin\\SettingTasks\\[Order].[Order].csv",
            "Data\\Admin\\SettingTasks\\CRM.DealStatus.csv",
            "Data\\Admin\\SettingTasks\\[Order].LeadCurrency.csv",
            "Data\\Admin\\SettingTasks\\[Order].LeadEvent.csv",
            "Data\\Admin\\SettingTasks\\[Order].LeadItem.csv",
            "Data\\Admin\\SettingTasks\\[Order].Lead.csv",
            "Data\\Admin\\SettingTasks\\[Order].OrderCustomer.csv"

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
        public void SettingTaskNewOrderFio()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderFio";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

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

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Отчество");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Patronymic");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order FIO #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("15", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");

            /*positive*/
            Functions.NewFullOrderClient_9000(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order FIO 31"), "name new task in grid");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e-col-index=\"1\"]")).Count == 2, "count new tasks");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            GetGridCell(0, "Name").FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            AssertCkText("New description 31, Ad Admin Patronymic, +74958002001, admin", "editor1");
            VerifyAreEqual("New Order FIO 31", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "name task");
            VerifyAreEqual("Заказ №31", driver.FindElement(By.CssSelector("[data-e2e=\"orderLink\"]")).Text, "order");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Elena El"), "select assigned");

            selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Низкий"), "select priority");

            selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("All"), "select project");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderLink\"]")).Click();
            Thread.Sleep(1000);
            Functions.OpenNewTab(driver, baseURL);
            VerifyIsTrue(driver.Url.EndsWith("orders/edit/31"), "order url");
            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Заказ № 31"), "name order h1");
            GoToAdmin("orders/edit/31#?orderTabs=3");
            ScrollTo(By.TagName("footer"));
            VerifyIsTrue(GetGridCell(0, "Name", "Tasks").FindElement(By.TagName("span")).Text.Contains("New Order FIO 31"), "name new task in order grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted", "Tasks").FindElement(By.TagName("div")).Text), "statys new task in order grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName", "Tasks").FindElement(By.TagName("div")).Text), "assigned new task in order grid");
            GetGridCell(0, "Name", "Tasks").FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-dialog")).Displayed, "open modal window");
            VerifyAreEqual("New Order FIO 31", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "name task modal");
            VerifyAreEqual("Заказ №31", driver.FindElement(By.CssSelector("[data-e2e=\"orderLink\"]")).Text, "order modal");
            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskNewOrderFrom100To10000()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrder";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("14");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Сумма заказа");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("1");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleRange\"]")).Click();
        
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueFrom\"]")).SendKeys("100");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueTo\"]")).SendKeys("1000");
           
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Сумма заказа от 100 до 1000", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");

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

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Admin Ad", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("5 дней", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("14", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");

            /*positive*/
            Functions.NewOrderClient_450(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Sum"), "name new task in grid");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e-col-index=\"1\"]")).Count == 3, "count new tasks");
            VerifyAreEqual("Высокий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text) ,"priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Admin Ad", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");
            /*negative*/
            Functions.NewOrderClient_1350(driver, baseURL);
            GoToAdmin("tasks");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Sum"), "repeat name new task in grid");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e-col-index=\"1\"]")).Count == 3, "count new tasks");


            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskNewOrderGroup()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderGroup";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

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

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("13", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");

            /*positive*/
            Functions.NewOrderClient_450(driver, baseURL);
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
        public void SettingTaskNewOrderPayment()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderPayment";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("12");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Метод оплаты");
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("Наличными курьеру");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Метод оплаты = Наличными курьеру", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");


            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Payment #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("12", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");

            //positive
            Functions.NewFullOrderClient_9000(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Payment"), "name new task in grid");
           VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            VerifyFinally(testname);
        }
    
        [Test]
        public void SettingTaskNewOrderRegion()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderRegion";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("11");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Страна");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Россия");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Страна = Россия", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Регион");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Московская область");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Город");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValue\"]")).SendKeys("Москва");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Adress #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("11", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");

            /*positive*/
            Functions.NewFullOrderClient_9000(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Adress"), "name new task in grid");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskNewOrderShipping()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderShipping";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("10");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Метод доставки");
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("Самовывоз");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Метод доставки = Самовывоз", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");


            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Elena El");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Elena El", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Shipping #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Низкий");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("10", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");

            /*positive*/
            Functions.NewFullOrderClient_9000(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Shipping"), "name new task in grid");
           VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");

            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskNewOrderSource()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderSource";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

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
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueSelect\"]")))).SelectByText("В один клик");


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

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("24 часа", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("9", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");
           
            /*negative*/
            Functions.NewFullOrderClient_9000(driver, baseURL);
            GoToAdmin("tasks");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Source"), "repeat name new task in grid");

            /*positive*/
            Functions.NewOrderClient_450(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Source"), "name new task in grid");
            VerifyAreEqual("Низкий", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("Elena El", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");
          
            VerifyFinally(testname);
        }
       
      
        [Test]
        public void SettingTaskNewOrderTel()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderTel";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

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

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("8", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");

            /*positive*/
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
       
       
       
        [Test]
        public void SettingTaskNewOrderzFromAdmin()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderzFromAdmin";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("7");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilterAdd\"]")).Click();
            Thread.Sleep(1000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParam\"]")))).SelectByText("Создан в части администрирования");
           
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleParamValueOk\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Создан в части администрирования = Да", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleFilter\"]")).Text, "filter rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("Менеджер заказа");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();

            VerifyAreEqual("Менеджер заказа", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text, "name assigned user rule new order");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("New Order Admin #ORDER_ID#");
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("New description Admin  #ORDER_ID#, #NAME#, #PHONE#, #EMAIL#");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Средний");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("All");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);
            //проверка грида

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Менеджер заказа", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("7", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");
           
            /*negative*/
            Functions.NewFullOrderClient_9000(driver, baseURL);
            GoToAdmin("tasks");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Admin"), "repeat name new task in grid");

            /*positive*/
            GoToAdmin("orders/add");
            (new SelectElement(driver.FindElement(By.Id("Order_ManagerId")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector(".edit.link-decoration-none")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".ui-grid-cell-contents a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            WaitForAjax();
          
            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            GetGridCell(1, "selectionRowHeaderCol", "OffersSelectvizr").Click();

            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.Id("header-top"));
            Thread.Sleep(4000);
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(1000);

            GoToAdmin("tasks");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "new task in grid");
            VerifyIsTrue(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Admin"), "name new task in grid");
           VerifyAreEqual("Средний", (GetGridCell(0, "PriorityFormatted").FindElement(By.TagName("div")).Text), "priority new task in grid");
            VerifyAreEqual("В работе", (GetGridCell(0, "StatusFormatted").FindElement(By.TagName("div")).Text), "statys new task in grid");
            VerifyAreEqual("test testov", (GetGridCell(0, "AssignedName").FindElement(By.TagName("div")).Text), "assigned new task in grid");
           
            VerifyFinally(testname);
        }
        [Test]
        public void SettingTaskNewOrderzFromLead()
        {
            GoToAdmin("settingstasks");
            testname = "SettingTaskNewOrderzFromLead";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"OrderCreated\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Новое правило", driver.FindElement(By.TagName("h2")).Text, " new rule title");
            VerifyAreEqual("Новый заказ", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text, " event name");

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

            VerifyAreEqual("Новый заказ", GetGridCell(0, "EventName", "OrderCreatedRules").Text, "name");
            VerifyAreEqual("Elena El", GetGridCell(0, "ManagerFilterHTML", "OrderCreatedRules").Text, "manager name");
            VerifyAreEqual("", GetGridCell(0, "TaskDueDateIntervalFormatted", "OrderCreatedRules").Text, "date");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "OrderCreatedRules").Text, "create");
            VerifyAreEqual("6", GetGridCell(0, "Priority", "OrderCreatedRules").Text, "priority");
            
            /*negative*/
            Functions.NewOrderClient_450(driver, baseURL);
            Thread.Sleep(1000);
            GoToAdmin("tasks");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("span")).Text.Contains("New Order Lead"), "name repeat task in grid");
           
            /*positive*/
            GoToAdmin("leads/edit/120");
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadCreateOrder\"]")).Click();
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
