using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange
{
    [TestFixture]
    public class DiscountsPriceRangePresentTest : BaseSeleniumTest
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
        public void Present10()
        {
            testname = "Present10";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("20", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void Present20()
        {
            testname = "Present20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("30", GetGridCell(19, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void Present50()
        {
            testname = "Present50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("60", GetGridCell(49, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void Present100()
        {
            testname = "Present100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("110", GetGridCell(99, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "line 100");

            VerifyFinally(testname);
        }

    }
}
