using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardPresent : BaseSeleniumTest
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
        public void CardPresent10()
        {
            testname = "CardPresent10";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "present line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void CardPresent20()
        {
            testname = "CardPresent20";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("530820", GetGridCell(19, "CardNumber").FindElement(By.TagName("a")).Text, "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void CardPresent50()
        {
            testname = "CardPresent50";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("530850", GetGridCell(49, "CardNumber").FindElement(By.TagName("a")).Text, "present line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void CardPresent100()
        {
            testname = "CardPresent100";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("530900", GetGridCell(99, "CardNumber").FindElement(By.TagName("a")).Text, "present line 100");

            VerifyFinally(testname);
        }
    }
}
