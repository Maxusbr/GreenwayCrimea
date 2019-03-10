using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields
{
    [TestFixture]
    public class SettingsCustomerFieldsSearchTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\CustomerFields\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerFieldValue.csv"

           );

            Init();
            
        }

         


        [Test]
        public void SearchExist()
        {
            testname = "SettingsCustomerFieldsSearchExist";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 111");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();

            VerifyAreEqual("Customer Field 111", GetGridCell(0, "Name", "CustomerFields").Text, "search exist settings");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchNotExist()
        {
            testname = "SettingsCustomerFieldsSearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("555 Field");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchMuchSymbols()
        {
            testname = "SettingsCustomerFieldsSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchInvalidSymbols()
        {
            testname = "SettingsCustomerFieldsSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");

            VerifyFinally(testname);
        }
    }
}