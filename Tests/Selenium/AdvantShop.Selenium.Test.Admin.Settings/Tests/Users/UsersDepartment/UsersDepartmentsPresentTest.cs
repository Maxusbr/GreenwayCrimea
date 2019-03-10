using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users.Departments
{
    [TestFixture]
    public class SettingsUsersDepartmentsPresentTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\Users\\UsersDepartment\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Users\\UsersDepartment\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\Users\\UsersDepartment\\Customers.Managers.csv",
                  "data\\Admin\\Settings\\Users\\UsersDepartment\\Customers.Departments.csv",
                  "data\\Admin\\Settings\\Users\\UsersDepartment\\Customers.ManagerRole.csv",
                  "data\\Admin\\Settings\\Users\\UsersDepartment\\Customers.ManagerRolesMap.csv"

           );

            Init();

            GoToAdmin("settings/userssettings#?tab=Departments");
        }

         

        [Test]
        public void SettingsUsersDepartmentPresent10()
        {
            testname = "SettingsUsersDepartmentPresent10";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //  PageSelectItems("10", 1);
            PageSelectItemsTab("10", "gridDepartments");
            VerifyAreEqual("Department1", GetGridCell(0, "Name", "Departments").Text, "present line 1");
            VerifyAreEqual("Department10", GetGridCell(9, "Name", "Departments").Text, "present line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersDepartmentPresent20()
        {
            testname = "SettingsUsersDepartmentPresent20";
            VerifyBegin(testname);

           ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //PageSelectItems("20", 1);
            PageSelectItemsTab("20", "gridDepartments");
            VerifyAreEqual("Department1", GetGridCell(0, "Name", "Departments").Text, "present line 1");
            VerifyAreEqual("Department20", GetGridCell(19, "Name", "Departments").Text, "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersDepartmentPresent50()
        {
            testname = "SettingsUsersDepartmentPresent50";
            VerifyBegin(testname);

           ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //  PageSelectItems("50", 1);
            PageSelectItemsTab("50", "gridDepartments");
            VerifyAreEqual("Department1", GetGridCell(0, "Name", "Departments").Text, "present line 1");
            VerifyAreEqual("Department50", GetGridCell(49, "Name", "Departments").Text, "present line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersDepartmentPresent100()
        {
            testname = "SettingsUsersDepartmentPresent100";
            VerifyBegin(testname);

           ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //  PageSelectItems("100", 1);
            PageSelectItemsTab("100", "gridDepartments");
            VerifyAreEqual("Department1", GetGridCell(0, "Name", "Departments").Text, "present line 1");
            VerifyAreEqual("Department100", GetGridCell(99, "Name", "Departments").Text, "present line 100");

            VerifyFinally(testname);
        }
    }
}