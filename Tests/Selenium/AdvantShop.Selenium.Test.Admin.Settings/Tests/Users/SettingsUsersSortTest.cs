using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users
{
    [TestFixture]
    public class SettingsUsersSortTest : BaseSeleniumTest
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
        public void SettingsUsersSortFullName()
        {
            testname = "SettingsUsersSortFullName";
            VerifyBegin(testname);

            GetGridCell(-1, "FullName", "Users").Click();
            WaitForAjax();
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "sort FullName 1 asc");
            VerifyAreEqual("testfirstname106 testlastname106", GetGridCell(9, "FullName", "Users").Text, "sort FullName 10 asc");

            GetGridCell(-1, "FullName", "Users").Click();
            WaitForAjax();
            VerifyAreEqual("testfirstname99 testlastname99", GetGridCell(0, "FullName", "Users").Text, "sort FullName 1 desc");
            VerifyAreEqual("testfirstname90 testlastname90", GetGridCell(9, "FullName", "Users").Text, "sort FullName 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersSortEmail()
        {
            testname = "SettingsUsersSortEmail";
            VerifyBegin(testname);

            GetGridCell(-1, "Email", "Users").Click();
            WaitForAjax();
            VerifyAreEqual("admin", GetGridCell(0, "Email", "Users").Text, "sort Email 1 asc");
            VerifyAreEqual("testmail@mail.ru106", GetGridCell(9, "Email", "Users").Text, "sort Email 10 asc");

            GetGridCell(-1, "Email", "Users").Click();
            WaitForAjax();
            VerifyAreEqual("testmail@mail.ru99", GetGridCell(0, "Email", "Users").Text, "sort Email 1 desc");
            VerifyAreEqual("testmail@mail.ru90", GetGridCell(9, "Email", "Users").Text, "sort Email 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersSortDepartmentName()
        {
            testname = "SettingsUsersSortDepartmentName";
            VerifyBegin(testname);

            GetGridCell(-1, "DepartmentName", "Users").Click();
            WaitForAjax();
            VerifyAreEqual("", GetGridCell(0, "DepartmentName", "Users").Text, "sort DepartmentName 1 asc");
            VerifyAreEqual("", GetGridCell(9, "DepartmentName", "Users").Text, "sort DepartmentName 10 asc");

            GetGridCell(-1, "DepartmentName", "Users").Click();
            WaitForAjax();
            VerifyAreEqual("Department6", GetGridCell(0, "DepartmentName", "Users").Text, "sort DepartmentName 1 desc");
            VerifyAreEqual("Department5", GetGridCell(1, "DepartmentName", "Users").Text, "sort DepartmentName 2 desc");
            VerifyAreEqual("", GetGridCell(9, "DepartmentName", "Users").Text, "sort DepartmentName 10 asc");

            VerifyFinally(testname);
        }

  
        [Test]
        public void SettingsUsersSortSortEnabled()
        {
            testname = "SettingsUsersSortSortEnabled";
            VerifyBegin(testname);

            GetGridCell(-1, "Enabled", "Users").Click();
            WaitForAjax();
            VerifyIsTrue(!GetGridCell(0, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "sort Enabled 1 asc");
            VerifyIsTrue(!GetGridCell(9, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "sort Enabled 10 asc");

            GetGridCell(-1, "Enabled", "Users").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "sort Enabled 1 desc");
            VerifyIsTrue(GetGridCell(9, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "sort Enabled 10 desc");

            VerifyFinally(testname);
        }
    }
}