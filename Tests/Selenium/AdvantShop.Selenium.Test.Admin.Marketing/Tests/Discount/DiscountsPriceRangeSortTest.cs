using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange
{
    [TestFixture]
    public class DiscountsPriceRangeSortTest : BaseSeleniumTest
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
        public void ByPriceRange()
        {
            testname = "ByPriceRange";
            VerifyBegin(testname);

            GetGridCell(-1, "PriceRange").Click();
            WaitForAjax();
            VerifyAreEqual("11", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "Price Range 1 asc");
            VerifyAreEqual("20", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "Price Range 10 asc");

            GetGridCell(-1, "PriceRange").Click();
            WaitForAjax();
            VerifyAreEqual("180", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "Price Range 1 desc");
            VerifyAreEqual("171", GetGridCell(9, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "Price Range 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void ByPercentDiscount()
        {
            testname = "ByPercentDiscount";
            VerifyBegin(testname);

            GetGridCell(-1, "PercentDiscount").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "Percent Discount 1 asc");
            VerifyAreEqual("5", GetGridCell(9, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "Percent Discount 10 asc");

            GetGridCell(-1, "PercentDiscount").Click();
            WaitForAjax();
            VerifyAreEqual("90", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "Percent Discount 1 desc");
            VerifyAreEqual("81", GetGridCell(9, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "Percent Discount 10 desc");

            VerifyFinally(testname);
        }
        
    }
}
