using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users
{
    [TestFixture]
    public class SettingsUsersFilterTest : BaseMultiSeleniumTest
    {

        [SetUp]
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
        public void SettingsUsersFilterImg()
        {
            testname = "SettingsUsersFilterImg";
            VerifyBegin(testname);

            //check filter img with
            Functions.GridFilterTabSet(driver, baseURL, name: "PhotoSrc", gridId: "gridUsers");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фото");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 151", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter img with");

            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "FullName filter img with");
            VerifyIsFalse(GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "avatar line 1");
            VerifyIsFalse(GetGridCell(9, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "avatar line 9");

            //check filter img without
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фото");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 70", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter img without");

            VerifyAreEqual("testfirstname220 testlastname220", GetGridCell(0, "FullName", "Users").Text, "FullName filter img without");
            VerifyIsTrue(GetGridCell(0, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "no avatar line 1");
            VerifyIsTrue(GetGridCell(9, "PhotoSrc", "Users").FindElement(By.TagName("img")).GetAttribute("src").Contains("no-avatar"), "no avatar line 9");

            //check delete with filter
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            
            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, "PhotoSrc");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 151", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter img after deleting 1");

            GoToAdmin("settings/userssettings");
            VerifyAreEqual("Найдено записей: 151", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter img after deleting 2");

            VerifyFinally(testname);

           
        }

        [Test]
        public void SettingsUsersFilterName()
        {
            testname = "SettingsUsersFilterName";
            VerifyBegin(testname);

            //check filter full name
            Functions.GridFilterTabSet(driver, baseURL, name: "FullName", gridId: "gridUsers");

            //search by not exist name
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 name test 3");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist name
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("testlastname2");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();

            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "FullName");

            WaitForAjax();

            VerifyAreEqual("Найдено записей: 33", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter full name");

            //check delete with filter
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");

            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "delete filtered items except admin");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter full name");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, "FullName");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 189", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter full name deleting 1");

            GoToAdmin("settings/userssettings");
            VerifyAreEqual("Найдено записей: 189", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter full name deleting 2");

            VerifyFinally(testname);

           
        }


        [Test]
        public void SettingsUsersFilterEmail()
        {
            testname = "SettingsUsersFilterEmail";
            VerifyBegin(testname);

            //check filter email
            Functions.GridFilterTabSet(driver, baseURL, name: "Email", gridId: "gridUsers");

            //search by not exist email
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 name test 3");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist email
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("testmail@mail.ru2");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();

            VerifyAreEqual("testfirstname220 testlastname220", GetGridCell(0, "FullName", "Users").Text, "FullName");
            VerifyAreEqual("testmail@mail.ru220", GetGridCell(0, "Email", "Users").Text, "Email");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 32", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter email");

            //check delete with filter
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, "Email");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 189", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter email deleting 1");

            GoToAdmin("settings/userssettings");
            VerifyAreEqual("Найдено записей: 189", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter email deleting 2");

            VerifyFinally(testname);

           
        }

        [Test]
        public void SettingsUsersFilterDepart()
        {
            testname = "SettingsUsersFilterDepart";
            VerifyBegin(testname);

            //check filter department no
            Functions.GridFilterTabSet(driver, baseURL, name: "_noopColumnDepartments", gridId: "gridUsers");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Не указан");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 214", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter department no");

            //check filter department
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Department1");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter department");

            VerifyAreEqual("testfirstname220 testlastname220", GetGridCell(0, "FullName", "Users").Text, "FullName line 1");
            VerifyAreEqual("testfirstname219 testlastname219", GetGridCell(1, "FullName", "Users").Text, "FullName line 2");
            
            //check all departments
            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 12, "count departments");  //10 departments + null option + no department option

            //check delete with filter
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
          //  ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL,"_noopColumnDepartments");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 219", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter department after deleting 1");

            GoToAdmin("settings/userssettings");
            VerifyAreEqual("Найдено записей: 219", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter department after deleting 2");

            VerifyFinally(testname);

           
        }

        [Test]
        public void SettingsUsersFilterEnabled()
        {
            testname = "SettingsUsersFilterEnabled";
            VerifyBegin(testname);

            //check filter enabled
            Functions.GridFilterTabSet(driver, baseURL, name: "Enabled", gridId: "gridUsers");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Активные");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 201", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled");

            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "FullName filter enabled 1 ");
            VerifyAreEqual("testfirstname212 testlastname212", GetGridCell(9, "FullName", "Users").Text, "FullName filter enabled 10");

            //check filter disabled
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 20", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter disabled");

            VerifyAreEqual("testfirstname21 testlastname21", GetGridCell(0, "FullName", "Users").Text, "FullName filter disabled 1");
            VerifyAreEqual("testfirstname12 testlastname12", GetGridCell(9, "FullName", "Users").Text, "FullName filter disabled 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridUsers");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 201", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled after deleting 1");

            GoToAdmin("settings/userssettings");
            VerifyAreEqual("Найдено записей: 201", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled after deleting 2");

            VerifyFinally(testname);

           
        }
    }
}