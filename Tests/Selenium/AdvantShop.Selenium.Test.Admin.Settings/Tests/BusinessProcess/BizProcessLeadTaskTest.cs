using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System;

namespace AdvantShop.SeleniumTest.Admin.Settings.BizProcesses.LeadAddTasks
{
    [TestFixture]
    public class SettingsBizProcessLeadTaskTest : BaseMultiSeleniumTest
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
                   "data\\Admin\\Settings\\BizProcessLead\\Customers.Departments.csv",
               "data\\Admin\\Settings\\BizProcessLead\\Customers.Managers.csv",
                "data\\Admin\\Settings\\BizProcessLead\\CRM.DealStatus.csv",
                 "data\\Admin\\Settings\\BizProcessLead\\CRM.BizProcessRule.csv",
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
        
        /* positive tests */
        [Test]
        public void LeadAddFromAdmin()
        {
            testname = "BizProcessLeadAddFromAdmin";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("lead from admin");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+723177712923");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(2000);

            XPathContainsText("span", "TestCategory2");

            VerifyAreEqual("21 руб.", GetGridCell(0, "PriceFormatted", "OffersSelectvizr").Text, "sum according to biz rule");
            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            XPathContainsText("button", "Выбрать");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(2000);

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Новый лид");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Новый лид", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Новый лид", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Текст задачи", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("test testov"), "task details user assigned to");

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

            IWebElement selectElemManager = driver.FindElement(By.Id("Lead_ManagerId"));
            SelectElement select4 = new SelectElement(selectElemManager);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("test testov"), "lead manager");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct21"), "lead product");
            VerifyAreEqual("Новый лид", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadAddFromClient()
        {
            testname = "BizProcessLeadAddFromClient";
            VerifyBegin(testname);

            //set create lead when buy in one click
            GoToAdmin("settingscheckout#?checkoutTab=common");

            IWebElement selectElem1 = driver.FindElement(By.Name("BuyInOneClickAction"));
            SelectElement select1 = new SelectElement(selectElem1);

            if (!select1.SelectedOption.Text.Contains("Создавать лид"))

            {
                ScrollTo(By.Name("BuyInOneClickButtonText"));
                (new SelectElement(driver.FindElement(By.Name("BuyInOneClickAction")))).SelectByText("Создавать лид");

                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
                Thread.Sleep(2000);
            }

            //client
            GoToClient("products/test-product22");
            
            ScrollTo(By.CssSelector("[data-product-id=\"22\"]"));
            driver.FindElement(By.LinkText("Купить в один клик")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[value=\"Заказать\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Закрыть")).Click();
            Thread.Sleep(2000);

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Новый лид");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Новый лид", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");
            
            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Новый лид", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Текст задачи", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.SelectedOption.Text.Contains("test testov"), "task details user assigned to");

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

            IWebElement selectElemManager = driver.FindElement(By.Id("Lead_ManagerId"));
            SelectElement select4 = new SelectElement(selectElemManager);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("test testov"), "lead manager");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct22"), "lead product");
            VerifyAreEqual("Новый лид", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadChangeDealStatus()
        {
            testname = "BizProcessLeadChangeDealStatus";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/100");

            IWebElement selectElem1 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("Сделка заключена"), "pre check lead deal status");

            (new SelectElement(driver.FindElement(By.Id("Lead_DealStatusId")))).SelectByText("Ожидание решения клиента");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("leads/edit/100");

            IWebElement selectElem2 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Ожидание решения клиента"), "lead new deal status saved");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Смена этапа лида");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Смена этапа лида", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Низкий", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup2"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Смена этапа лида", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Текст задачи", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select3 = new SelectElement(selectElemUser);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("Elena El"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select4 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Низкий"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select5 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select5.SelectedOption.Text.Contains("TaskGroup2"), "task details group");

            //check lead
            driver.FindElement(By.CssSelector("[data-e2e=\"leadLink\"]")).Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            VerifyIsTrue(windowHandles.Count == 2, "2 tabs - tasks and lead");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 100"), "lead h1");
            
            IWebElement selectElemLeadStatus = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select6 = new SelectElement(selectElemLeadStatus);
            VerifyIsTrue(select6.SelectedOption.Text.Contains("Ожидание решения клиента"), "lead status");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct100"), "lead product");
            VerifyAreEqual("Смена этапа лида", GetGridCell(0, "Name", "Tasks").Text, "lead's task name");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted", "Tasks").Text, "lead's task status");
            VerifyAreEqual("Elena El", GetGridCell(0, "AssignedName", "Tasks").Text, "lead's task assigned name");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        /* negative tests */
        [Test]
        public void LeadAddFromAdminInappropriateRuleConditions()
        {
            testname = "LeadAddFromAdminInappropriateRuleConditions";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("lead from admin Inappropriate");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+723177745623");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(2000);

            XPathContainsText("span", "TestCategory1");

            VerifyAreEqual("4 руб.", GetGridCell(3, "PriceFormatted", "OffersSelectvizr").Text, "sum not according to biz rule");
            GetGridCell(3, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            XPathContainsText("button", "Выбрать");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(2000);

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Новый лид");
            DropFocus("h1");
            Blur();
            
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadAddFromClientInappropriateRuleConditions()
        {
            testname = "LeadAddFromClientInappropriateRuleConditions";
            VerifyBegin(testname);

            //set create lead when buy in one click
            GoToAdmin("settingscheckout#?checkoutTab=common");

            IWebElement selectElem1 = driver.FindElement(By.Name("BuyInOneClickAction"));
            SelectElement select1 = new SelectElement(selectElem1);

            if (!select1.SelectedOption.Text.Contains("Создавать лид"))

            {
                ScrollTo(By.Name("BuyInOneClickButtonText"));
                (new SelectElement(driver.FindElement(By.Name("BuyInOneClickAction")))).SelectByText("Создавать лид");

                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
                Thread.Sleep(2000);
            }

            //client
            GoToClient("products/test-product5");

            ScrollTo(By.CssSelector("[data-product-id=\"5\"]"));
            driver.FindElement(By.LinkText("Купить в один клик")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[value=\"Заказать\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Закрыть")).Click();
            Thread.Sleep(2000);

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Новый лид");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadChangeDealStatusInappropriateRuleConditions()
        {
            testname = "LeadChangeDealStatusInappropriateRuleConditions";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/101");

            IWebElement selectElem1 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("Сделка заключена"), "pre check lead deal status");

            (new SelectElement(driver.FindElement(By.Id("Lead_DealStatusId")))).SelectByText("Созвон с клиентом");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("leads/edit/101");

            IWebElement selectElem2 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Созвон с клиентом"), "lead new deal status saved");

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Смена этапа лида");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no task created");

            VerifyFinally(testname);
        }
    }
}