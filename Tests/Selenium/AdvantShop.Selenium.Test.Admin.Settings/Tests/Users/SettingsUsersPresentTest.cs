using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users
{
    [TestFixture]
    public class SettingsUsersPresentTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\Users\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Users\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\Users\\Customers.Managers.csv",
                  "data\\Admin\\Settings\\Users\\Customers.Departments.csv",
                  "data\\Admin\\Settings\\Users\\Customers.ManagerRole.csv",
                  "data\\Admin\\Settings\\Users\\Customers.ManagerRolesMap.csv"

           );

            Init();

            GoToAdmin("settings/userssettings");
        }

         

        [Test]
        public void SettingsUsersPresent10()
        {
            testname = "SettingsUsersPresent10";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //  PageSelectItems("10");
            PageSelectItemsTab("10", "gridUsers");
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "present line 1");
            VerifyAreEqual("testfirstname212 testlastname212", GetGridCell(9, "FullName", "Users").Text, "present line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersPresent20()
        {
            testname = "SettingsUsersPresent20";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //   PageSelectItems("20");
            PageSelectItemsTab("20", "gridUsers");
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "present line 1");
            VerifyAreEqual("testfirstname202 testlastname202", GetGridCell(19, "FullName", "Users").Text, "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersPresent50()
        {
            testname = "SettingsUsersPresent50";
            VerifyBegin(testname);

            //ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //Functions.GridPaginationSelect50(driver, baseURL);
            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //  PageSelectItems("50");
            PageSelectItemsTab("50", "gridUsers");
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "present line 1");
            VerifyAreEqual("testfirstname172 testlastname172", GetGridCell(49, "FullName", "Users").Text, "present line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersPresent100()
        {
            testname = "SettingsUsersPresent100";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //  PageSelectItems("100");
            PageSelectItemsTab("100", "gridUsers");
            Thread.Sleep(2000);
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "present line 1");
            VerifyAreEqual("testfirstname122 testlastname122", GetGridCell(99, "FullName", "Users").Text, "present line 100");

            VerifyFinally(testname);
        }
    }
}