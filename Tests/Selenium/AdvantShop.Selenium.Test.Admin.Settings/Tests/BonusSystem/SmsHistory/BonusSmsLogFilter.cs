using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.SmsHistory
{
    [TestFixture]
    public class BonusSmsLogFilter : BaseSeleniumTest
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
        public void FilterPhone()
        {
            testname = "BonusSmsLogFilterPhone";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "Phone");

            //search by not exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("53624351");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("####@@@@@@&&&&&&");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111");
            Blur();

            VerifyAreEqual("111111", GetGridCell(0, "Phone", "log").Text, "sms number line 1");
            VerifyAreEqual("Test Message46", GetGridCell(0, "Body", "log").Text, "sms Body line 1");
            VerifyAreEqual("Get", GetGridCell(0, "State", "log").Text, "sms State line 1");
            VerifyAreEqual("06.03.2017 03:40:00", GetGridCell(0, "Created_Str", "log").Text, "sms CreatedStr line 1");
                      
            Functions.GridFilterClose(driver, baseURL, name: "Phone");
            VerifyAreEqual("Найдено записей: 51", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter card num deleting 1");
            Refresh();
            VerifyAreEqual("Найдено записей: 51", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter card num deleting 1");
                       
            VerifyFinally(testname);
        }

        [Test]
        public void FilterText()
        {
            testname = "BonusSmsLogFilterText";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "Body");

            //search by not exist Contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 name contact 3");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist FIO
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Test Message10");

            VerifyAreEqual("888888", GetGridCell(0, "Phone", "log").Text, "sms number line 1");
            VerifyAreEqual("Test Message10", GetGridCell(0, "Body", "log").Text, "sms Body line 1");
            VerifyAreEqual("Send", GetGridCell(0, "State", "log").Text, "sms State line 1");
            VerifyAreEqual("11.04.2017 03:40:00", GetGridCell(0, "Created_Str", "log").Text, "sms CreatedStr line 1");
                      
            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Body");
            VerifyAreEqual("Найдено записей: 51", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FIO deleting 1");
                      
            VerifyFinally(testname);
        }
    }
}
