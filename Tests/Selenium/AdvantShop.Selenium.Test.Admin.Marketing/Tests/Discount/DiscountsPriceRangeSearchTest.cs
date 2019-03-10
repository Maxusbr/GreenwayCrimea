using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange
{
    [TestFixture]
    public class DiscountsPriceRangeSearchTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
        "data\\Admin\\Discount\\Catalog.Product.csv",
        "data\\Admin\\Discount\\Catalog.Offer.csv",
        "data\\Admin\\Discount\\Catalog.Category.csv",
        "data\\Admin\\Discount\\Catalog.ProductCategories.csv",
         "Data\\Admin\\Discount\\[Order].OrderSource.csv",
         "data\\Admin\\Discount\\[Order].OrderStatus.csv",
          "data\\Admin\\Discount\\[Order].OrderPriceDiscount.csv"
          );

            Init();
        }


        [Test]
        public void SearchExist()
        {
            testname = "SearchExist";
            VerifyBegin(testname);

            GoToAdmin("discountspricerange");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("111");

            VerifyAreEqual("111", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "search exist discount price range");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchNotExist()
        {
            testname = "SearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("discountspricerange");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("40000");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchMuchSymbols()
        {
            testname = "SearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("discountspricerange");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchInvalidSymbols()
        {
            testname = "SearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("discountspricerange");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

    }
}
