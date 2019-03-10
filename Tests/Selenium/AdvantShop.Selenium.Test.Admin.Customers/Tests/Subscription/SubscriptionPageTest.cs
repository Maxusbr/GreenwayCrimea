using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Selenium.Test.Admin.Customers.Tests.Subscription
{
    [TestFixture]
    public class SubscriptionPageTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
               "data\\Admin\\Subscription\\ManySubscription\\Customers.CustomerGroup.csv",
                  "data\\Admin\\Subscription\\ManySubscription\\Customers.Country.csv",
            "data\\Admin\\Subscription\\ManySubscription\\Customers.Region.csv",
            "data\\Admin\\Subscription\\ManySubscription\\Customers.City.csv",
            "data\\Admin\\Subscription\\ManySubscription\\Customers.Customer.csv",
            "data\\Admin\\Subscription\\ManySubscription\\Customers.Contact.csv",
           "data\\Admin\\Subscription\\ManySubscription\\Customers.Departments.csv",
           "data\\Admin\\Subscription\\ManySubscription\\Customers.Managers.csv",
               "data\\Admin\\Subscription\\ManySubscription\\Customers.CustomerField.csv",
               "data\\Admin\\Subscription\\ManySubscription\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\Subscription\\ManySubscription\\Customers.CustomerFieldValuesMap.csv",
               "data\\Admin\\Subscription\\ManySubscription\\Customers.Subscription.csv"

           );

            Init();
            GoToAdmin("subscription");
        }
        [Test]
        public void SubscriptionqPresent10()
        {
            testname = "SubscriptionPresent10";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "line 1");
            VerifyAreEqual("testmail16@test.ru", GetGridCell(9, "Email").Text, "line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SubscriptionPresent20()
        {
            testname = "SubscriptionPresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "line 1");
            VerifyAreEqual("testmail25@test.ru", GetGridCell(19, "Email").Text, "line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void SubscriptionPresent50()
        {
            testname = "SubscriptionPresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "line 1");
            VerifyAreEqual("testmail52@test.ru", GetGridCell(49, "Email").Text, "line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void SubscriptionPresent100()
        {
            testname = "SubscriptionPresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "line 1");
            VerifyAreEqual("testmail98@test.ru", GetGridCell(99, "Email").Text, "line 100");

            VerifyFinally(testname);
        }
        [Test]
        public void SubscriptionsPage()
        {
            testname = "SubscriptionsPage";
            VerifyBegin(testname);

            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "page 1 line 1");
            VerifyAreEqual("testmail16@test.ru", GetGridCell(9, "Email").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("testmail17@test.ru", GetGridCell(0, "Email").Text, "page 2 line 1");
            VerifyAreEqual("testmail25@test.ru", GetGridCell(9, "Email").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("testmail26@test.ru", GetGridCell(0, "Email").Text, "page 3 line 1");
            VerifyAreEqual("testmail34@test.ru", GetGridCell(9, "Email").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("testmail35@test.ru", GetGridCell(0, "Email").Text, "page 4 line 1");
            VerifyAreEqual("testmail43@test.ru", GetGridCell(9, "Email").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("testmail44@test.ru", GetGridCell(0, "Email").Text, "page 5 line 1");
            VerifyAreEqual("testmail52@test.ru", GetGridCell(9, "Email").Text, "page 5 line 10");
            
            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "page 1 line 1");
            VerifyAreEqual("testmail16@test.ru", GetGridCell(9, "Email").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SubscriptionsPageToPrevious()
        {
            testname = "SubscriptionsPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "page 1 line 1");
            VerifyAreEqual("testmail16@test.ru", GetGridCell(9, "Email").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("testmail17@test.ru", GetGridCell(0, "Email").Text, "page 2 line 1");
            VerifyAreEqual("testmail25@test.ru", GetGridCell(9, "Email").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("testmail26@test.ru", GetGridCell(0, "Email").Text, "page 3 line 1");
            VerifyAreEqual("testmail34@test.ru", GetGridCell(9, "Email").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("testmail17@test.ru", GetGridCell(0, "Email").Text, "page 2 line 1");
            VerifyAreEqual("testmail25@test.ru", GetGridCell(9, "Email").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "page 1 line 1");
            VerifyAreEqual("testmail16@test.ru", GetGridCell(9, "Email").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("testmail99@test.ru", GetGridCell(0, "Email").Text, "last page line 1");

            VerifyFinally(testname);
        }
        [Test]
        public void SubscriptionzSelectDelete()
        {
            GoToAdmin("subscription");
            testname = "SubscriptionzSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("testmail11@test.ru", GetGridCell(0, "Email").Text, "selected 2 grid delete");
            VerifyAreEqual("testmail12@test.ru", GetGridCell(1, "Email").Text, "selected 3 grid delete");
            VerifyAreEqual("testmail13@test.ru", GetGridCell(2, "Email").Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("testmail20@test.ru", GetGridCell(0, "Email").Text, "selected all on page 2 grid delete");
            VerifyAreEqual("testmail29@test.ru", GetGridCell(9, "Email").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            VerifyAreEqual("87", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("subscription");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            VerifyFinally(testname);
        }
    }
}
