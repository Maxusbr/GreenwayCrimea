using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users.Departments
{
    [TestFixture]
    public class SettingsUsersDepartmentsDelTest : BaseSeleniumTest
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
        public void SelectDelete()
        {
            testname = "SettingsUsersDepartmentSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "Departments").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Department1", GetGridCell(0, "Name", "Departments").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "Departments").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Department2", GetGridCell(0, "Name", "Departments").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 1 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            // FilterTabDelete(1);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridDepartments");
            VerifyAreEqual("Department5", GetGridCell(0, "Name", "Departments").Text, "selected 1 grid delete");
            VerifyAreEqual("Department6", GetGridCell(1, "Name", "Departments").Text, "selected 2 grid delete");
            VerifyAreEqual("Department7", GetGridCell(2, "Name", "Departments").Text, "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            // FilterTabDelete(1);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridDepartments");
            VerifyAreEqual("Department15", GetGridCell(0, "Name", "Departments").Text, "selected all on page 1 grid delete");
            VerifyAreEqual("Department24", GetGridCell(9, "Name", "Departments").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("106", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "Departments").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            //FilterTabDelete(1);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridDepartments");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            GoToAdmin("settings/userssettings#?tab=Departments");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all 2");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridDepartments\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting 2");

            VerifyFinally(testname);
        }
    }
}