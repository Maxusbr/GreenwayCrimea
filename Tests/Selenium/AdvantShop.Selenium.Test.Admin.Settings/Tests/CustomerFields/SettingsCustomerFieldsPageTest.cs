using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields
{
    [TestFixture]
    public class SettingsCustomerFieldsPageTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\CustomerFields\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerFieldValue.csv"

           );

            Init();

            GoToAdmin("settingscustomers#?tab=customerFields");
        }

         


        [Test]
        public void Page()
        {
            testname = "SettingsCustomerFieldsPage";
            VerifyBegin(testname);

            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "page 1 line 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 11", GetGridCell(0, "Name", "CustomerFields").Text, "page 2 line 1");
            VerifyAreEqual("Customer Field 20", GetGridCell(9, "Name", "CustomerFields").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 21", GetGridCell(0, "Name", "CustomerFields").Text, "page 3 line 1");
            VerifyAreEqual("Customer Field 30", GetGridCell(9, "Name", "CustomerFields").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 31", GetGridCell(0, "Name", "CustomerFields").Text, "page 4 line 1");
            VerifyAreEqual("Customer Field 40", GetGridCell(9, "Name", "CustomerFields").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 41", GetGridCell(0, "Name", "CustomerFields").Text, "page 5 line 1");
            VerifyAreEqual("Customer Field 50", GetGridCell(9, "Name", "CustomerFields").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 51", GetGridCell(0, "Name", "CustomerFields").Text, "page 6 line 1");
            VerifyAreEqual("Customer Field 60", GetGridCell(9, "Name", "CustomerFields").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "page 1 line 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "SettingsCustomerFieldsPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "page 1 line 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 11", GetGridCell(0, "Name", "CustomerFields").Text, "page 2 line 1");
            VerifyAreEqual("Customer Field 20", GetGridCell(9, "Name", "CustomerFields").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 21", GetGridCell(0, "Name", "CustomerFields").Text, "page 3 line 1");
            VerifyAreEqual("Customer Field 30", GetGridCell(9, "Name", "CustomerFields").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 11", GetGridCell(0, "Name", "CustomerFields").Text, "page 2 line 1");
            VerifyAreEqual("Customer Field 20", GetGridCell(9, "Name", "CustomerFields").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "page 1 line 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 141", GetGridCell(0, "Name", "CustomerFields").Text, "last page line 1");
            VerifyAreEqual("Customer Field 150", GetGridCell(9, "Name", "CustomerFields").Text, "last page line 10");

            VerifyFinally(testname);
        }
    }
}