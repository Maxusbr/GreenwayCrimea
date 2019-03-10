using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Region
{
    [TestFixture]
    public class SettingsRegionSearchTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Region\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Region\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Region\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Region\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Region\\Customers.CustomerGroup.csv"

           );

            Init();
            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
        }

        [Test]
        public void RegionSearchNotexist()
        {
            testname = "RegionSearchNotexist";
            VerifyBegin(testname);
            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestRegion000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);

        }

        [Test]
        public void RegionSearchMuch()
        {
            testname = "RegionSearchMuch";
            VerifyBegin(testname);
            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222223333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void RegionSearchInvalid()
        {
            testname = "RegionSearchInvalid";
            VerifyBegin(testname);

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void RegionSearchExist()
        {
            testname = "RegionSearchExist";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestRegion10");
            WaitForAjax();
            VerifyAreEqual("TestRegion10", GetGridCell(0, "Name", "Region").Text, "find value");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            VerifyFinally(testname);
        }
    }
}
