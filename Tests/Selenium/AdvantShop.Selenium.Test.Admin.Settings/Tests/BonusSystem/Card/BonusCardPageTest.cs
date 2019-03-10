using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardPage : BaseSeleniumTest
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
        public void CardPage()
        {
            testname = "CardPage";
            VerifyBegin(testname);

            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("530811", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("530820", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("530821", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 3 line 1");
            VerifyAreEqual("530830", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("530831", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 4 line 1");
            VerifyAreEqual("530840", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("530841", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 5 line 1");
            VerifyAreEqual("530850", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("530851", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 6 line 1");
            VerifyAreEqual("530860", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void CardPageToPrevious()
        {
            testname = "CardPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("530811", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("530820", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("530821", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 3 line 1");
            VerifyAreEqual("530830", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("530811", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("530820", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("530911", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "last page line 1");
            VerifyAreEqual("530920", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "last page line 10");

            VerifyFinally(testname);
        }
    }
}
