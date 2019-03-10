using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Country
{
    [TestFixture]
    public class CountriesSearchTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Customers.CustomerGroup.csv"

           );

            Init();
            GoToAdmin("settingssystem#?systemTab=countries");
        }

        [Test]
        public void CountrySearchNotexist()
        {
            testname = "CountrySearchNotexist";
            VerifyBegin(testname);
            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestCountry000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);

        }

        [Test]
        public void CountrySearchMuch()
        {
            testname = "CountrySearchMuch";
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
        public void CountrySearchInvalid()
        {
            testname = "CountrySearchInvalid";
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
        public void CountrySearchExist()
        {
            testname = "CountrySearchExist";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestCountry10");
            WaitForAjax();
            VerifyAreEqual("TestCountry10", GetGridCell(0, "Name", "Country").Text, "find value");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            VerifyFinally(testname);
        }
    }
}
