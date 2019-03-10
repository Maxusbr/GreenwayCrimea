using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users.Departments
{
    [TestFixture]
    public class SettingsUsersDepartmentsSearchTest : BaseSeleniumTest
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
        }

         


        [Test]
        public void SettingsUsersDepartmentSearchExist()
        {
            testname = "SettingsUsersDepartmentSearchExist";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings#?tab=Departments");

            //GetGridFilterTab(1, "Department111");
            GetGridIdFilter("gridDepartments", "Department111");
            XPathContainsText("h1", "Отделы");
            WaitForAjax();

            VerifyAreEqual("Department111", GetGridCell(0, "Name", "Departments").Text, "search exist item");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersDepartmentSearchNotExist()
        {
            testname = "SettingsUsersDepartmentSearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings#?tab=Departments");

            //GetGridFilterTab(1, "555 Department");
            GetGridIdFilter("gridDepartments", "555 Department");
            XPathContainsText("h1", "Отделы");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersDepartmentSearchMuchSymbols()
        {
            testname = "SettingsUsersDepartmentSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings#?tab=Departments");

            // GetGridFilterTab(1, "1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            GetGridIdFilter("gridDepartments", "1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            XPathContainsText("h1", "Отделы");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersDepartmentSearchInvalidSymbols()
        {
            testname = "SettingsUsersDepartmentSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings#?tab=Departments");

            //GetGridFilterTab(1, "########@@@@@@@@&&&&&&&******,,,,..");
            GetGridIdFilter("gridDepartments", "########@@@@@@@@&&&&&&&******,,,,..");
            XPathContainsText("h1", "Отделы");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");

            VerifyFinally(testname);
        }
    }
}