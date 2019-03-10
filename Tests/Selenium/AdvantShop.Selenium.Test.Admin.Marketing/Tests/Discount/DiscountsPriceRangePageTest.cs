using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange
{
    [TestFixture]
    public class DiscountsPriceRangePageTest : BaseSeleniumTest
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
            GoToAdmin("discountspricerange");
        }

        [Test]
        public void Page()
        {
            testname = "DiscountsPriceRangePage";
            VerifyBegin(testname);

            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("20", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("21", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("30", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("31", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("40", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("41", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 1");
            VerifyAreEqual("50", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("51", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 1");
            VerifyAreEqual("60", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("61", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 1");
            VerifyAreEqual("70", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("20", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "DiscountsPriceRangePageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("20", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("21", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("30", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("31", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("40", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("21", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("30", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("20", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("171", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "last page line 1");
            VerifyAreEqual("180", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "last page line 10");

            VerifyFinally(testname);
        }


    }
}
