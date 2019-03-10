using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.SmsHistory
{
    [TestFixture]
    public class BonusSmsLogPageView : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Product.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Offer.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Category.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.ProductCategories.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.Grade.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.Card.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.SmsTemplate.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.SmsLog.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Customers.CustomerGroup.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Customers.Customer.csv"
                );
            Init();

            GoToAdmin("smstemplates/smslog");
        }

        [Test]
        public void Page()
        {
            testname = "LogPage";
            VerifyBegin(testname);
            VerifyAreEqual("999999", GetGridCell(0, "Phone", "log").Text, "page 1 line 1");
            VerifyAreEqual("888888", GetGridCell(9, "Phone", "log").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("888888", GetGridCell(0, "Phone", "log").Text, "page 2 line 1");
            VerifyAreEqual("777777", GetGridCell(9, "Phone", "log").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("777777", GetGridCell(0, "Phone", "log").Text, "page 3 line 1");
            VerifyAreEqual("666666", GetGridCell(9, "Phone", "log").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("666666", GetGridCell(0, "Phone", "log").Text, "page 4 line 1");
            VerifyAreEqual("444444", GetGridCell(9, "Phone", "log").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("333333", GetGridCell(0, "Phone", "log").Text, "page 5 line 1");
            VerifyAreEqual("456789", GetGridCell(9, "Phone", "log").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("456790", GetGridCell(0, "Phone", "log").Text, "page 6 line 1");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("999999", GetGridCell(0, "Phone", "log").Text, "page 1 line 1");
            VerifyAreEqual("888888", GetGridCell(9, "Phone", "log").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "LogPageToPrevious";
            VerifyBegin(testname);
            VerifyAreEqual("999999", GetGridCell(0, "Phone", "log").Text, "page 1 line 1");
            VerifyAreEqual("888888", GetGridCell(9, "Phone", "log").Text, "page 1 line 10");

           
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("888888", GetGridCell(0, "Phone", "log").Text, "page 2 line 1");
            VerifyAreEqual("777777", GetGridCell(9, "Phone", "log").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("777777", GetGridCell(0, "Phone", "log").Text, "page 3 line 1");
            VerifyAreEqual("666666", GetGridCell(9, "Phone", "log").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("888888", GetGridCell(0, "Phone", "log").Text, "page 2 line 1");
            VerifyAreEqual("777777", GetGridCell(9, "Phone", "log").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("999999", GetGridCell(0, "Phone", "log").Text, "page 1 line 1");
            VerifyAreEqual("888888", GetGridCell(9, "Phone", "log").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("456790", GetGridCell(0, "Phone", "log").Text, "page last line 1");

            VerifyFinally(testname);
        }

        [Test]
        public void LogPresentz10()
        {
            testname = "LogPresent10";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("Test Message1", GetGridCell(0, "Body", "log").Text, "present Body line 1");
            VerifyAreEqual("Test Message10", GetGridCell(9, "Body", "log").Text, "present Body line 1");

            VerifyFinally(testname);
        }

        [Test]
        public void LogPresent20()
        {
            testname = "LogPresent20";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("Test Message1", GetGridCell(0, "Body", "log").Text, "present Body line 1");
            VerifyAreEqual("Test Message20", GetGridCell(19, "Body", "log").Text, "present Body line 1");

            VerifyFinally(testname);
        }

        [Test]
        public void LogPresent50()
        {
            testname = "LogPresent50";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("Test Message1", GetGridCell(0, "Body", "log").Text, "present Body line 1");
            VerifyAreEqual("Test Message50", GetGridCell(49, "Body", "log").Text, "present Body line 1");

            VerifyFinally(testname);
        }

        [Test]
        public void LogPresent100()
        {
            testname = "LogPresent100";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("Test Message1", GetGridCell(0, "Body", "log").Text, "present Body line 1");
            VerifyAreEqual("Test Message51", GetGridCell(50, "Body", "log").Text, "present Body line 1"); 

            VerifyFinally(testname);
        }

    }
}
