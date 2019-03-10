using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange
{
    [TestFixture]
    public class DiscountsPriceRangeAddEditTest : BaseSeleniumTest
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
        public void DiscountsPriceRangeAdd()
        {
            testname = "DiscountsPriceRangeAdd";
            VerifyBegin(testname);

            GoToAdmin("discountspricerange");

            driver.FindElement(By.CssSelector("[data-e2e=\"bntAdd\"]")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Скидка из стоимости заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");

            driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).SendKeys("5000");

            driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).SendKeys("97");

            driver.FindElement(By.CssSelector("[data-e2e=\"bntSave\"]")).Click();
            Thread.Sleep(3000);

            GoToAdmin("discountspricerange");

            VerifyAreEqual("Найдено записей: 171", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("5000");

            VerifyAreEqual("5000", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "discount price range added");
            VerifyAreEqual("97", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "discount percent discount added");
            
            VerifyFinally(testname);
        }

        [Test]
        public void DiscountsPriceRangeEdit()
        {
            testname = "DiscountsPriceRangeEdit";
            VerifyBegin(testname);

            GoToAdmin("discountspricerange");

            //pre check admin
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("100");

            VerifyAreEqual("100", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "pre check discount price range");
            VerifyAreEqual("90", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "pre check percent discount");

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            //pre check pop up
            VerifyAreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).GetAttribute("value"), "pre check pop up discount price range");
            VerifyAreEqual("90", driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).GetAttribute("value"), "pre check pop up discount price range");

            driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).SendKeys("20000");

            driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).SendKeys("4");

            driver.FindElement(By.CssSelector("[data-e2e=\"bntSave\"]")).Click();
            Thread.Sleep(3000);
            
            GoToAdmin("discountspricerange");
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("100");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "edited prev price range");

            //check admin
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("20000");

            VerifyAreEqual("20000", GetGridCell(0, "PriceRange").FindElement(By.TagName("input")).GetAttribute("value"), "discount price range edited");
            VerifyAreEqual("4", GetGridCell(0, "PercentDiscount").FindElement(By.TagName("input")).GetAttribute("value"), "discount percent discount edited");

            //check pop up
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("20000", driver.FindElement(By.CssSelector("[data-e2e=\"priceRange\"]")).GetAttribute("value"), "pop up discount price range edited");
            VerifyAreEqual("4", driver.FindElement(By.CssSelector("[data-e2e=\"percentDiscount\"]")).GetAttribute("value"), "pop up discount price range edited");

            VerifyFinally(testname);
        }
        
    }
}
