using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users
{
    [TestFixture]
    public class SettingsUsersSearchTest : BaseSeleniumTest
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
        }

         

        [Test]
        public void SettingsUsersSearchExistName()
        {
            testname = "SettingsUsersSearchExistName";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");

            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            //GetGridFilter().SendKeys("testfirstname111");
            GetGridIdFilter("gridUsers", "testfirstname111");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyAreEqual("testfirstname111 testlastname111", GetGridCell(0, "FullName", "Users").Text, "search exist name");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersSearchExistEmail()
        {
            testname = "SettingsUsersSearchExistEmail";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");

            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            //GetGridFilter().SendKeys("testmail@mail.ru135");
            GetGridIdFilter("gridUsers", "testmail@mail.ru135");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyAreEqual("testmail@mail.ru135", GetGridCell(0, "Email", "Users").Text, "search exist email");
            VerifyAreEqual("testfirstname135 testlastname135", GetGridCell(0, "FullName", "Users").Text, "search exist email's name");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersSearchNotExist()
        {
            testname = "SettingsUsersSearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");

            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            //GetGridFilter().SendKeys("testfirstname111222222222222");
            GetGridIdFilter("gridUsers", "testfirstname111222222222222");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist name");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersSearchMuchSymbols()
        {
            testname = "SettingsUsersSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");

            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            //GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            GetGridIdFilter("gridUsers", "1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersSearchInvalidSymbols()
        {
            testname = "SettingsUsersSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("settings/userssettings");

            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            //GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            GetGridIdFilter("gridUsers", "########@@@@@@@@&&&&&&&******,,,,..");
            XPathContainsText("h1", "Сотрудники");
            WaitForAjax();
            Thread.Sleep(2000);
      
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
    }
}