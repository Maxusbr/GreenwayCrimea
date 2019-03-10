using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.DiscountsPriceRange.Client
{
    [TestFixture]
    public class DiscountsPriceRangeClientTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
        "data\\Admin\\Discount\\DiscountClient\\Catalog.Product.csv",
        "data\\Admin\\Discount\\DiscountClient\\Catalog.Offer.csv",
        "data\\Admin\\Discount\\DiscountClient\\Catalog.Category.csv",
        "data\\Admin\\Discount\\DiscountClient\\Catalog.ProductCategories.csv",
         "Data\\Admin\\Discount\\DiscountClient\\[Order].OrderSource.csv",
         "data\\Admin\\Discount\\DiscountClient\\[Order].OrderStatus.csv",
          "data\\Admin\\Discount\\DiscountClient\\[Order].OrderPriceDiscount.csv"
          );

            Init();

        }

        [Test]
        public void DiscountApplied()
        {
            testname = "DiscountAppliedClient";
            VerifyBegin(testname);
            Functions.DelProductCart(driver, baseURL);

            GoToClient("products/test-product102");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).Text.Contains("1 102 руб."), "product offer");

            ScrollTo(By.CssSelector(".rating"));
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(4000);

            GoToClient("cart");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".cart-full-result-block")).Text.Contains("- 551 руб. (50%)"), "offer with discount in cart");

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(3000);
            GoToClient("checkout");
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            VerifyIsTrue(driver.FindElement(By.Id("rightCell")).Text.Contains("-50%"), "offer PercentDiscount in checkout");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-result-price.cs-t-4")).Text.Contains("551 руб."), "offer with discount in checkout");

            VerifyFinally(testname);
        }

        [Test]
        public void DiscountNotApplied()
        {
            testname = "DiscountNotAppliedClient";
            VerifyBegin(testname);
            Functions.DelProductCart(driver, baseURL);

            GoToClient("products/test-product60");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).Text.Contains("1 060 руб."), "product offer");

            ScrollTo(By.CssSelector(".rating"));
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(4000);

            GoToClient("cart");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".cart-full-result-block")).Text.Contains("(50%)"), "no discount in cart");

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            VerifyIsFalse(driver.FindElement(By.Id("rightCell")).Text.Contains("-50%"), "offer PercentDiscount in checkout no");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-result-price.cs-t-4")).Text.Contains("1 060 руб."), "no offer in checkout");

            VerifyFinally(testname);
        }

        [Test]
        public void zDiscountsDisabled()
        {
            testname = "DiscountsDisabledClient";
            VerifyBegin(testname);
            Functions.DelProductCart(driver, baseURL);

            GoToAdmin("discountspricerange");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"enableDiscounts\"]")).FindElement(By.TagName("input")).Selected, "pre check discounts enabled");

            driver.FindElement(By.CssSelector("[data-e2e=\"enableDiscounts\"]")).FindElement(By.CssSelector(".adv-checkbox-emul")).Click();
            Thread.Sleep(1000);

            //check admin
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"enableDiscounts\"]")).FindElement(By.TagName("input")).Selected, "discounts disabled");

            //check client
            GoToClient("products/test-product102");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-payment-cell.details-payment-price")).Text.Contains("1 102 руб."), "product offer");

            ScrollTo(By.CssSelector(".rating"));
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(4000);

            GoToClient("cart");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".cart-full-result-block")).Text.Contains("(50%)"), "cart discounts disabled");

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            VerifyIsFalse(driver.FindElement(By.Id("rightCell")).Text.Contains("-50%"), "offer PercentDiscount in checkout no");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-result-price.cs-t-4")).Text.Contains("1 102 руб."), "checkout discounts disabled");

            VerifyFinally(testname);
        }
        
    }
}
