using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardSearch : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.Category.csv",
          "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.ProductCategories.csv",
            "Data\\Admin\\Settings\\BonusSystem\\Grid\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Bonus.Card.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Customers.Customer.csv"


                );
            Init();

            GoToAdmin("cards");
        }

         
        [Test]
        public void CardSearchExistByCardNum()
        {
            testname = "CardSearchExistByCardNum";
            VerifyBegin(testname);

            GoToAdmin("Cards");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("530874");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("530874", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "search exist Card id");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }


        [Test]
        public void CardSearchExistByCustomer()
        {
            testname = "CardSearchExistByCustomer";
            VerifyBegin(testname);

            GoToAdmin("Cards");
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("FirstName3");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("LastName3 FirstName3", GetGridCell(0, "FIO").Text, "search exist fullname");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
        
        [Test]
        public void CardSearchMuchSymbols()
        {
            testname = "CardSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("Cards");

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
        public void CardSearchInvalidSymbols()
        {
            testname = "CardSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("Cards");

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
