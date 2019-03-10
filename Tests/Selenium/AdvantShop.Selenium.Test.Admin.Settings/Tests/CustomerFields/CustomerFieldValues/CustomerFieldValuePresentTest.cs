using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields.Values
{
    [TestFixture]
    public class SettingsCustomerFieldValuePresentTest : BaseSeleniumTest
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
        public void CustomerFieldValuePresent10()
        {
            testname = "CustomerFieldValuePresent10";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "present line 1");
            VerifyAreEqual("Value 10", GetGridCell(9, "Value", "CustomerFieldValues").Text, "present line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValuePresent20()
        {
            testname = "CustomerFieldValuePresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "present line 1");
            VerifyAreEqual("Value 20", GetGridCell(19, "Value", "CustomerFieldValues").Text, "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValuePresent50()
        {
            testname = "CustomerFieldValuePresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "present line 1");
            VerifyAreEqual("Value 50", GetGridCell(49, "Value", "CustomerFieldValues").Text, "present line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValuePresent100()
        {
            testname = "CustomerFieldValuePresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "present line 1");
            VerifyAreEqual("Value 100", GetGridCell(99, "Value", "CustomerFieldValues").Text, "present line 100");

            VerifyFinally(testname);
        }
    }
}