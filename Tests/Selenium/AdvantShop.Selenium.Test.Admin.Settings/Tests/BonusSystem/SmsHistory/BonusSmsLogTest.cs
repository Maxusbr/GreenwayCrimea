using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.SmsHistory
{
    [TestFixture]
    public class BonusSmsLogTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Category.csv",
          "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.ProductCategories.csv",
               "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.Card.csv",
             "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.SmsTemplate.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.SmsLog.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Customers.Customer.csv"


                );
            Init();

            GoToAdmin("smstemplates/smslog");
        }

        [Test]
        public void SmsLogGrid()
        {
            testname = "SmsLogGrid";
            VerifyBegin(testname);

            VerifyAreEqual("999999", GetGridCell(0, "Phone", "log").Text, "sms number line 1");
            VerifyAreEqual("Test Message1", GetGridCell(0, "Body", "log").Text, "sms Body line 1");
            VerifyAreEqual("Send", GetGridCell(0, "State", "log").Text, "sms State line 1");
            VerifyAreEqual("20.04.2017 03:40:00", GetGridCell(0, "Created_Str", "log").Text, "sms CreatedStr line 1");

            VerifyAreEqual("999999", GetGridCell(1, "Phone", "log").Text, "sms number line 2");
            VerifyAreEqual("Test Message2", GetGridCell(1, "Body", "log").Text, "sms Body line 2");
            VerifyAreEqual("Send", GetGridCell(1, "State", "log").Text, "sms State line 1");
            VerifyAreEqual("19.04.2017 03:40:00", GetGridCell(1, "Created_Str", "log").Text, "sms CreatedStr line 2");
            VerifyFinally(testname);   
        }
        

        [Test]
        public void SmsSearchExistByText()
        {
            testname = "SearchExistByCustomer";
            VerifyBegin(testname);

            GoToAdmin("smstemplates/smslog");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Test Message10");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("888888", GetGridCell(0, "Phone", "log").Text, "sms number line 1");
            VerifyAreEqual("Test Message10", GetGridCell(0, "Body", "log").Text, "sms Body line 1");
            VerifyAreEqual("Send", GetGridCell(0, "State", "log").Text, "sms State line 1");
            VerifyAreEqual("11.04.2017 03:40:00", GetGridCell(0, "Created_Str", "log").Text, "sms CreatedStr line 1");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SmsSearchMuchSymbols()
        {
            testname = "SearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("smstemplates/smslog");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SmsSearchInvalidSymbols()
        {
            testname = "SearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("smstemplates/smslog");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
    }
}
