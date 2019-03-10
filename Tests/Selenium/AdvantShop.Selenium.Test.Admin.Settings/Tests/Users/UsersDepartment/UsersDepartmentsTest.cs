using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users.Departments
{
    [TestFixture]
    public class SettingsUsersDepartmentsTest : BaseSeleniumTest
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
        public void SettingsUserDepartmentGrid()
        {
            testname = "SettingsUserDepartmentGrid";
            VerifyBegin(testname);

            VerifyAreEqual("Отделы", driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Text, "h1 settings users departments");

            VerifyAreEqual("Department1", GetGridCell(0, "Name", "Departments").Text, "Name");
            VerifyAreEqual("1", GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).GetAttribute("value"), "Sort Order");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Departments").FindElement(By.TagName("input")).Selected, "Enabled");

            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUserDepartmentInplaceSort()
        {
            testname = "SettingsUserDepartmentInplaceSort";
            VerifyBegin(testname);

            //GetGridFilterTab(1, "Department111");
            GetGridIdFilter("gridDepartments", "Department111");
            XPathContainsText("h1", "Отделы");
            WaitForAjax();

            VerifyAreEqual("111", GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).GetAttribute("value"), "before edit");

            GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).SendKeys("20");

            XPathContainsText("h1", "Отделы");

            Refresh();

            //  GetGridFilterTab(1, "Department111");
            GetGridIdFilter("gridDepartments", "Department111");
            XPathContainsText("h1", "Отделы");
            WaitForAjax();

            VerifyAreEqual("20", GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).GetAttribute("value"), "edited inplace");

            //back
            GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Sort", "Departments").FindElement(By.TagName("input")).SendKeys("111");

            XPathContainsText("h1", "Отделы");
            // GetGridFilterTab(1);
            GetGridIdFilter("gridDepartments");
            WaitForAjax();

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUserDepartmentInplaceEnabled()
        {
            testname = "SettingsUserDepartmentInplaceEnabled";
            VerifyBegin(testname);

            GetGridCell(0, "Enabled", "Departments").Click();

            VerifyIsTrue(!GetGridCell(0, "Enabled", "Departments").FindElement(By.TagName("input")).Selected, "Enabled 1");

            Refresh();

            VerifyIsTrue(!GetGridCell(0, "Enabled", "Departments").FindElement(By.TagName("input")).Selected, "Enabled 2");
            
            VerifyFinally(testname);
        }
        
    }
}