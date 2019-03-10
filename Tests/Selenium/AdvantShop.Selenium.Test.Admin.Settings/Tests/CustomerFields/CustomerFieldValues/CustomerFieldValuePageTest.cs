using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields.Values
{
    [TestFixture]
    public class SettingsCustomerFieldValuePageTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\CustomerFieldValues\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerFieldValue.csv"

           );

            Init();

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridCell(0, "HasValues", "CustomerFields").Click();
        }

         

        [Test]
        public void CustomerFieldValuePage()
        {
            testname = "CustomerFieldValuePage";
            VerifyBegin(testname);

            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 1 line 1");
            VerifyAreEqual("Value 10", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 11", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 2 line 1");
            VerifyAreEqual("Value 20", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 21", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 3 line 1");
            VerifyAreEqual("Value 30", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 31", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 4 line 1");
            VerifyAreEqual("Value 40", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 41", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 5 line 1");
            VerifyAreEqual("Value 50", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 51", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 6 line 1");
            VerifyAreEqual("Value 60", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 1 line 1");
            VerifyAreEqual("Value 10", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValuePageToPrevious()
        {
            testname = "CustomerFieldValuePageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 1 line 1");
            VerifyAreEqual("Value 10", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 11", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 2 line 1");
            VerifyAreEqual("Value 20", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 21", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 3 line 1");
            VerifyAreEqual("Value 30", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 11", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 2 line 1");
            VerifyAreEqual("Value 20", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "page 1 line 1");
            VerifyAreEqual("Value 10", GetGridCell(9, "Value", "CustomerFieldValues").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 131", GetGridCell(0, "Value", "CustomerFieldValues").Text, "last page line 1");
            VerifyAreEqual("Value 140", GetGridCell(9, "Value", "CustomerFieldValues").Text, "last page line 10");

            VerifyFinally(testname);
        }
    }
}