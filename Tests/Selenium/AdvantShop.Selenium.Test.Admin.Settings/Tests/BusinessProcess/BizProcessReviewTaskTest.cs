using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Settings.BizProcessesReviewAddTasks
{
    [TestFixture]
    public class SettingsBizProcessReviewTaskTest : BaseMultiSeleniumTest
    {

        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Catalog | ClearType.Customers);
            InitializeService.LoadData(
                       "data\\Admin\\Settings\\BizProcessReview\\Catalog.Product.csv",
           "data\\Admin\\Settings\\BizProcessReview\\Catalog.Offer.csv",
           "data\\Admin\\Settings\\BizProcessReview\\Catalog.Category.csv",
           "data\\Admin\\Settings\\BizProcessReview\\Catalog.ProductCategories.csv",
         "data\\Admin\\Settings\\BizProcessReview\\Customers.Customer.csv",
           "data\\Admin\\Settings\\BizProcessReview\\Customers.CustomerGroup.csv",
                   "data\\Admin\\Settings\\BizProcessReview\\Customers.Departments.csv",
               "data\\Admin\\Settings\\BizProcessReview\\Customers.Managers.csv",
                "data\\Admin\\Settings\\BizProcessReview\\CRM.DealStatus.csv",
                 "data\\Admin\\Settings\\BizProcessReview\\CRM.BizProcessRule.csv",
                        "data\\Admin\\Settings\\BizProcessReview\\[Order].OrderSource.csv",
                "data\\Admin\\Settings\\BizProcessReview\\[Order].LeadCurrency.csv",
                    "data\\Admin\\Settings\\BizProcessReview\\[Order].LeadEvent.csv",
                    "data\\Admin\\Settings\\BizProcessReview\\[Order].LeadItem.csv",
                "data\\Admin\\Settings\\BizProcessReview\\[Order].Lead.csv",
                      "data\\Admin\\Settings\\BizProcessReview\\Customers.TaskGroup.csv",
         "data\\Admin\\Settings\\BizProcessReview\\Customers.Task.csv"

           );

            Init();
            Functions.AdminSettingsReviewsOn(driver, baseURL);
        }
        
        [Test]
        public void ReviewAddFromClient()
        {
            testname = "ReviewAddFromClient";
            VerifyBegin(testname);
            
            GoToClient("products/test-product41");
            
            ScrollTo(By.Id("tabReviews"));
            driver.FindElement(By.Id("tabReviews")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("reviewsFormName")).Click();
            driver.FindElement(By.Name("reviewsFormName")).Clear();
            driver.FindElement(By.Name("reviewsFormName")).SendKeys("ReviewAuthor");
            
            driver.FindElement(By.Name("reviewsFormEmail")).Click();
            driver.FindElement(By.Name("reviewsFormEmail")).Clear();
            driver.FindElement(By.Name("reviewsFormEmail")).SendKeys("ReviewAuthor@mail.ru");

            driver.FindElement(By.Name("reviewFormText")).Click();
            driver.FindElement(By.Name("reviewFormText")).Clear();
            driver.FindElement(By.Name("reviewFormText")).SendKeys("Review Test Test");

            ScrollTo(By.Name("reviewsFormName"));
            driver.FindElement(By.Name("reviewSubmit")).Click();
            Thread.Sleep(2000);

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Новый отзыв");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Новый отзыв", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Новый отзыв", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Текст задачи", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.AllSelectedOptions[0].Text.Contains("test testov"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("TaskGroup1"), "task details group");

            //check review
            driver.FindElement(By.LinkText("Открыть отзыв")).Click();
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-header")).FindElement(By.TagName("h2")).Text.Contains("Отзыв"), "review h2");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("TestProduct41"), "review's product");
            VerifyAreEqual("ReviewAuthor", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).GetAttribute("value"), "review's author");
            VerifyAreEqual("ReviewAuthor@mail.ru", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).GetAttribute("value"), "review's mail");
            VerifyAreEqual("Review Test Test", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).GetAttribute("value"), "review's text");

            VerifyFinally(testname);
        }


        [Test]
        public void ReviewAddFromAdmin()
        {
            testname = "ReviewAddFromAdmin";
            VerifyBegin(testname);

            GoToAdmin("reviews");

            driver.FindElement(By.CssSelector("[data-e2e=\"reviewAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewArtNo\"]")).SendKeys("42");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).SendKeys("ReviewAuthor from Admin");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).SendKeys("ReviewAuthorFromAdmin@mail.ru");
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).SendKeys("текст отзыва");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Новый отзыв");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Новый отзыв", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Средний", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");

            VerifyIsTrue(driver.PageSource.Contains("TaskGroup1"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("Новый отзыв", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("Текст задачи", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select = new SelectElement(selectElemUser);
            VerifyIsTrue(select.AllSelectedOptions[0].Text.Contains("test testov"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select2 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("Средний"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select3 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("TaskGroup1"), "task details group");

            //check review
            driver.FindElement(By.LinkText("Открыть отзыв")).Click();
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-header")).FindElement(By.TagName("h2")).Text.Contains("Отзыв"), "review h2");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("TestProduct42"), "review's product");
            VerifyAreEqual("ReviewAuthor from Admin", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).GetAttribute("value"), "review's author");
            VerifyAreEqual("ReviewAuthorFromAdmin@mail.ru", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).GetAttribute("value"), "review's mail");
            VerifyAreEqual("текст отзыва", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).GetAttribute("value"), "review's text");

            VerifyFinally(testname);
        }

        [Test]
        public void ReviewAddVariables()
        {
            testname = "BizProcessReviewAddVariables";
            VerifyBegin(testname);

            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"CallMissed\"]"));
            if (!driver.FindElement(By.CssSelector("[grid-unique-id=\"gridReviewAddedRules\"]")).Text.Contains("Правила не заданы"))

            {
                GetGridCell(0, "_serviceColumn", "ReviewAddedRules").FindElement(By.TagName("ui-grid-custom-delete")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.ClassName("swal2-confirm")).Click();
                Thread.Sleep(2000);
            }

            driver.FindElement(By.CssSelector("[data-e2e-settings-task-rule=\"ReviewAdded\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).GetAttribute("innerText").Contains("Новый отзыв"), "biz rule type");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerAdd\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManager\"]")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerOk\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).SendKeys("название задачи - #NAME#, #EMAIL#, #TEXT#");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).SendKeys("текст задачи - #NAME#, #EMAIL#, #TEXT#");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateEmul\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).SendKeys("10");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).SendKeys("2");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]")))).SelectByText("В днях");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]")))).SelectByText("Высокий");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]")))).SelectByText("TaskGroup2");

            driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleOk\"]")).Click();
            Thread.Sleep(2000);

            //check rule grid
            GoToAdmin("settingstasks");

            ScrollTo(By.CssSelector("[data-e2e-settings-task-rule=\"CallMissed\"]"));

            VerifyAreEqual("Новый отзыв", GetGridCell(0, "EventName", "ReviewAddedRules").Text, "biz rule grid type");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerFilterHTML", "ReviewAddedRules").Text, "biz rule grid manager");
            VerifyAreEqual("10 дней", GetGridCell(0, "TaskDueDateIntervalFormatted", "ReviewAddedRules").Text, "biz rule grid task due time");
            VerifyAreEqual("сразу", GetGridCell(0, "TaskCreateIntervalFormatted", "ReviewAddedRules").Text, "biz rule grid task interval");
            VerifyAreEqual("2", GetGridCell(0, "Priority", "ReviewAddedRules").Text, "biz rule grid priority");
            
            GetGridCell(0, "_serviceColumn", "ReviewAddedRules").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.CssSelector(".modal-body"));

            //check rule pop up
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleType\"]")).Text.Contains("Новый отзыв"), "rule pop up type");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"BizRulePriority\"]")).GetAttribute("value"), "rule pop up task priority");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleManagerName\"]")).Text.Contains("test testov"), "rule pop up manager");
            VerifyAreEqual("название задачи - #NAME#, #EMAIL#, #TEXT#", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskName\"]")).GetAttribute("value"), "rule pop up task name");
            VerifyAreEqual("текст задачи - #NAME#, #EMAIL#, #TEXT#", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskText\"]")).GetAttribute("value"), "rule pop up task text");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateInput\"]")).Selected, "rule pop up task due time in use");
            VerifyAreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDate\"]")).GetAttribute("value"), "rule pop up task due time");

            IWebElement selectElemDueDateSelect = driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskDueDateSelect\"]"));
            SelectElement select = new SelectElement(selectElemDueDateSelect);
            VerifyIsTrue(select.SelectedOption.Text.Contains("В днях"), "rule pop up task due time value");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskCreateDateInput\"]")).Selected, "rule pop up task create time not in use");

            IWebElement selectElemTaskPriority = driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskPriority\"]"));
            SelectElement select1 = new SelectElement(selectElemTaskPriority);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("Высокий"), "rule pop up task priority");

            IWebElement selectElemTaskProj = driver.FindElement(By.CssSelector("[data-e2e=\"BizRuleTaskGroup\"]"));
            SelectElement select2 = new SelectElement(selectElemTaskProj);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("TaskGroup2"), "rule pop up task group");

            //add review
            GoToClient("products/test-product43");

            ScrollTo(By.Id("tabReviews"));
            driver.FindElement(By.Id("tabReviews")).FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("reviewsFormName")).Click();
            driver.FindElement(By.Name("reviewsFormName")).Clear();
            driver.FindElement(By.Name("reviewsFormName")).SendKeys("TestReviewAuthorName");

            driver.FindElement(By.Name("reviewsFormEmail")).Click();
            driver.FindElement(By.Name("reviewsFormEmail")).Clear();
            driver.FindElement(By.Name("reviewsFormEmail")).SendKeys("TestReviewAuthor@mail.ru");

            driver.FindElement(By.Name("reviewFormText")).Click();
            driver.FindElement(By.Name("reviewFormText")).Clear();
            driver.FindElement(By.Name("reviewFormText")).SendKeys("Test Review Text");

            ScrollTo(By.Name("reviewsFormName"));
            driver.FindElement(By.Name("reviewSubmit")).Click();
            Thread.Sleep(2000);

            //check task added
            GoToAdmin("tasks");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("название задачи - TestReviewAuthorName, TestReviewAuthor@mail.ru, Test Review Text");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("название задачи - TestReviewAuthorName, TestReviewAuthor@mail.ru, Test Review Text", GetGridCell(0, "Name").Text, "task added name");
            VerifyAreEqual("Высокий", GetGridCell(0, "PriorityFormatted").Text, "task added priority");
            VerifyAreEqual("В работе", GetGridCell(0, "StatusFormatted").Text, "task added status");
            VerifyAreEqual("test testov", GetGridCell(0, "AssignedName").Text, "task added assigned name");
            
            VerifyIsTrue(driver.PageSource.Contains("TaskGroup2"), "task added group");

            //check task details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.Id("cke_editor1"));

            VerifyAreEqual("название задачи - TestReviewAuthorName, TestReviewAuthor@mail.ru, Test Review Text", driver.FindElement(By.CssSelector("[data-e2e=\"edittaskName\"]")).GetAttribute("value"), "task details name");
            AssertCkText("текст задачи - TestReviewAuthorName, TestReviewAuthor@mail.ru, Test Review Text", "editor1");

            IWebElement selectElemUser = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskAssigned\"]"));
            SelectElement select3 = new SelectElement(selectElemUser);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("test testov"), "task details user assigned to");

            IWebElement selectElemPriority = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskPriopity\"]"));
            SelectElement select4 = new SelectElement(selectElemPriority);
            VerifyIsTrue(select4.SelectedOption.Text.Contains("Высокий"), "task details priority");

            IWebElement selectElemGroup = driver.FindElement(By.CssSelector("[data-e2e=\"edittaskGroup\"]"));
            SelectElement select5 = new SelectElement(selectElemGroup);
            VerifyIsTrue(select5.SelectedOption.Text.Contains("TaskGroup2"), "task details group");

            //check review
            driver.FindElement(By.LinkText("Открыть отзыв")).Click();
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-header")).FindElement(By.TagName("h2")).Text.Contains("Отзыв"), "review h2");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("TestProduct43"), "review's product");
            VerifyAreEqual("TestReviewAuthorName", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewName\"]")).GetAttribute("value"), "review's author");
            VerifyAreEqual("TestReviewAuthor@mail.ru", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewEmail\"]")).GetAttribute("value"), "review's mail");
            VerifyAreEqual("Test Review Text", driver.FindElement(By.CssSelector("[data-e2e=\"ReviewText\"]")).GetAttribute("value"), "review's text");

            VerifyFinally(testname);
        }
    }
}