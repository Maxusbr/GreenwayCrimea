using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.Users
{
    [TestFixture]
    public class SettingsUsersPageTest : BaseSeleniumTest
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
        public void SettingsUsersPage()
        {
            testname = "SettingsUsersPage";
            VerifyBegin(testname);

            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "page 1 line 1");
            VerifyAreEqual("testfirstname212 testlastname212", GetGridCell(9, "FullName", "Users").Text, "page 1 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname211 testlastname211", GetGridCell(0, "FullName", "Users").Text, "page 2 line 1");
            VerifyAreEqual("testfirstname202 testlastname202", GetGridCell(9, "FullName", "Users").Text, "page 2 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname201 testlastname201", GetGridCell(0, "FullName", "Users").Text, "page 3 line 1");
            VerifyAreEqual("testfirstname192 testlastname192", GetGridCell(9, "FullName", "Users").Text, "page 3 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname191 testlastname191", GetGridCell(0, "FullName", "Users").Text, "page 4 line 1");
            VerifyAreEqual("testfirstname182 testlastname182", GetGridCell(9, "FullName", "Users").Text, "page 4 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname181 testlastname181", GetGridCell(0, "FullName", "Users").Text, "page 5 line 1");
            VerifyAreEqual("testfirstname172 testlastname172", GetGridCell(9, "FullName", "Users").Text, "page 5 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname171 testlastname171", GetGridCell(0, "FullName", "Users").Text, "page 6 line 1");
            VerifyAreEqual("testfirstname162 testlastname162", GetGridCell(9, "FullName", "Users").Text, "page 6 line 10");

            //to begin
            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "page 1 line 1");
            VerifyAreEqual("testfirstname212 testlastname212", GetGridCell(9, "FullName", "Users").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsUsersPageToPrevious()
        {
            testname = "SettingsUsersPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "page 1 line 1");
            VerifyAreEqual("testfirstname212 testlastname212", GetGridCell(9, "FullName", "Users").Text, "page 1 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname211 testlastname211", GetGridCell(0, "FullName", "Users").Text, "page 2 line 1");
            VerifyAreEqual("testfirstname202 testlastname202", GetGridCell(9, "FullName", "Users").Text, "page 2 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname201 testlastname201", GetGridCell(0, "FullName", "Users").Text, "page 3 line 1");
            VerifyAreEqual("testfirstname192 testlastname192", GetGridCell(9, "FullName", "Users").Text, "page 3 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname211 testlastname211", GetGridCell(0, "FullName", "Users").Text, "page 2 line 1");
            VerifyAreEqual("testfirstname202 testlastname202", GetGridCell(9, "FullName", "Users").Text, "page 2 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("admin testlastname221", GetGridCell(0, "FullName", "Users").Text, "page 1 line 1");
            VerifyAreEqual("testfirstname212 testlastname212", GetGridCell(9, "FullName", "Users").Text, "page 1 line 10");

            //to end
            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridUsers\"]")).FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testfirstname1 testlastname1", GetGridCell(0, "FullName", "Users").Text, "last page line 1");

            VerifyFinally(testname);
        }
    }
}