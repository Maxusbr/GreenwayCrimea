using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields
{
    [TestFixture]
    public class SettingsCustomerFieldsPresentTest : BaseSeleniumTest
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
        public void Present10()
        {
            testname = "SettingsCustomerFieldsPresent10";
            VerifyBegin(testname);

            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "present line 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "present line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void Present20()
        {
            testname = "SettingsCustomerFieldsPresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "present line 1");
            VerifyAreEqual("Customer Field 20", GetGridCell(19, "Name", "CustomerFields").Text, "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void Present50()
        {
            testname = "SettingsCustomerFieldsPresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "present line 1");
            VerifyAreEqual("Customer Field 50", GetGridCell(49, "Name", "CustomerFields").Text, "present line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void Present100()
        {
            testname = "SettingsCustomerFieldsPresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "present line 1");
            VerifyAreEqual("Customer Field 100", GetGridCell(99, "Name", "CustomerFields").Text, "present line 100");

            VerifyFinally(testname);
        }
    }
}