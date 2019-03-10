using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users
{
    [TestFixture]
    public class SettingsUsersTest : BaseSeleniumTest
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
        public void SettingsUsersGrid()
        {
            testname = "SettingsUsersGrid";
            VerifyBegin(testname);

            VerifyAreEqual("Сотрудники", driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Text, "h1 settings users page");

            VerifyIsTrue(!GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "avatar");

            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "FullName");
            VerifyAreEqual("admin", GetGridCell(0, "Email", "Users").Text, "Email");
            VerifyAreEqual("Department6", GetGridCell(0, "DepartmentName", "Users").Text, "DepartmentName");
            VerifyAreEqual("Администратор", GetGridCell(0, "Roles", "Users").Text, "Roles");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "Enabled");

            VerifyAreEqual("Найдено записей: 221", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersInplaceEnabled()
        {
            testname = "SettingsUsersInplaceEnabled";
            VerifyBegin(testname);

            GetGridCell(1, "Enabled", "Users").Click();
            VerifyIsTrue(!GetGridCell(1, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "Enabled 1");

            GoToAdmin("settings/userssettings");

            VerifyIsTrue(!GetGridCell(1, "Enabled", "Users").FindElement(By.TagName("input")).Selected, "Enabled 2");

         //   GetGridCell(1, "Enabled", "Users").Click();
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersLinkEmail()
        {
            testname = "SettingsUsersLinkEmail";
            VerifyBegin(testname);

            GetGridCell(0, "Email", "Users").FindElement(By.TagName("span")).Click();
            WaitForElem(By.CssSelector(".modal-header"));

            VerifyAreEqual("Редактирование сотрудника", driver.FindElement(By.TagName("h2")).Text, "open pop edit up");
            
            driver.FindElement(By.XPath("//button[contains(text(), 'Отмена')]")).Click();

            VerifyFinally(testname);
        }


        [Test]
        public void SettingsUsersLinkName()
        {
            testname = "SettingsUsersLinkName";
            VerifyBegin(testname);

            GetGridCell(0, "FullName", "Users").Click();
            WaitForElem(By.CssSelector(".modal-header"));

            VerifyAreEqual("Редактирование сотрудника", driver.FindElement(By.TagName("h2")).Text, "open pop edit up");

            driver.FindElement(By.XPath("//button[contains(text(), 'Отмена')]")).Click();

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersOpenManagerPage()
        {
            testname = "SettingsUsersOpenManagerPage";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");
            if
               (!driver.FindElement(By.CssSelector("[data-e2e=\"showManagers\"]")).FindElement(By.TagName("input")).Selected)
         
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"showManagers\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(1000);
            }
            
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"showManagers\"]")).FindElement(By.TagName("input")).Selected, "checkbox show managers selected");
            
            driver.FindElement(By.LinkText("Перейти на страницу менеджеров")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h1[contains(text(), 'Менеджеры')]"));

            //check client
            VerifyIsTrue(driver.Url.Contains("managers"), "check managers url");
            VerifyAreEqual("Менеджеры", driver.FindElement(By.TagName("h1")).Text, "h1 managers page");

            GoToAdmin("settings/userssettings");

            //check admin
            driver.FindElement(By.CssSelector("[data-e2e=\"showManagers\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);

            VerifyIsFalse(driver.FindElements(By.LinkText("Перейти на страницу менеджеров")).Count > 0, "no link - checkbox show managers not selected");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"showManagers\"]")).FindElement(By.TagName("input")).Selected, "checkbox show managers not selected");

            VerifyIsTrue(Is404Page("managers"), "404 page if not show managers");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUserzSelectDelete()
        {
            testname = "SettingsUsersSelectDelete";
            VerifyBegin(testname);
            gridReturnDefaultView10();
            //check delete cancel 
            GetGridCell(1, "_serviceColumn", "Users").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname220 testlastname220", GetGridCell(1, "FullName", "Users").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(1, "_serviceColumn", "Users").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname219 testlastname219", GetGridCell(1, "FullName", "Users").Text, "1 grid delete");

            //check select 
            GetGridCell(1, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(3, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid"); //1 admin
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(3, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");
            VerifyAreEqual("testfirstname216 testlastname216", GetGridCell(1, "FullName", "Users").Text, "selected 2 grid delete");
            VerifyAreEqual("testfirstname215 testlastname215", GetGridCell(2, "FullName", "Users").Text, "selected 3 grid delete");
            VerifyAreEqual("testfirstname214 testlastname214", GetGridCell(3, "FullName", "Users").Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");
            VerifyAreEqual("testfirstname207 testlastname207", GetGridCell(1, "FullName", "Users").Text, "selected all on page 2 grid delete");
            VerifyAreEqual("testfirstname199 testlastname199", GetGridCell(9, "FullName", "Users").Text, "selected all on page 10 grid delete");
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "delete all on page except admin");

            //check select all
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("208", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "Users").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");

            GoToAdmin("settings/userssettings");
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "delete all except admin");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            VerifyFinally(testname);
        }
    }
}