using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.MethodShippingPayment
{
    [TestFixture]
    public class MethodTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Shipping | ClearType.Payment | ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\Shipping\\[Order].PaymentMethod.csv",
            "data\\Admin\\Settings\\Shipping\\[Order].ShippingMethod.csv",
              "data\\Admin\\Settings\\Shipping\\Catalog.Category.csv",
                "data\\Admin\\Settings\\Shipping\\Catalog.Brand.csv",
                 "data\\Admin\\Settings\\Shipping\\Catalog.Product.csv",
                 "data\\Admin\\Settings\\Shipping\\Catalog.Offer.csv",
                 "data\\Admin\\Settings\\Shipping\\Catalog.ProductCategories.csv"

           );

            Init();
        }
         

        [Test]
        public void SettingsMethod()
        {
            testname = "SettingsMethod";
            VerifyBegin(testname);

            GoToAdmin("settings/shippingmethods");
            VerifyAreEqual("FixedRateMailMen Фиксированная стоимость доставки", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingName\"]")).Text, " Shiping Name Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnabled\"] input")).GetAttribute("value"), " Shiping Enabled Method in Setting");
            VerifyAreEqual("FreeShipping Бесплатная доставка", driver.FindElements(By.CssSelector("[data-e2e=\"ShippingName\"]"))[1].Text, " Shiping Name Method in Setting");
            VerifyAreEqual("false", driver.FindElements(By.CssSelector("[data-e2e=\"ShippingEnabled\"] input"))[1].GetAttribute("value"), " Shiping Enabled Method in Setting");
            GoToAdmin("settings/paymentmethods");
            VerifyAreEqual("Cash Наличные", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentName\"]")).Text, " payment Name Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentEnabled\"] input")).GetAttribute("value"), " payment Enabled Method in Setting");
            VerifyAreEqual("WalletOne Единая Касса (Wallet One)", driver.FindElements(By.CssSelector("[data-e2e=\"PaymentName\"]"))[1].Text, " payment Name Method in Setting");
            VerifyAreEqual("false", driver.FindElements(By.CssSelector("[data-e2e=\"PaymentEnabled\"] input"))[1].GetAttribute("value"), " payment Enabled Method in Setting");

            ProductToCard();

            //отображение методов в корзине
            GoToClient("checkout");
            VerifyAreEqual("1", driver.FindElements(By.CssSelector(".checkout-block"))[1].FindElements(By.CssSelector(".custom-input-radio")).Count.ToString(), " Count shipping Method in cart");
            VerifyAreEqual("1", driver.FindElements(By.CssSelector(".checkout-block"))[3].FindElements(By.CssSelector(".payment-item-radio")).Count.ToString(), " Count payment Method in cart");

            VerifyAreEqual("FixedRateMailMen", driver.FindElement(By.CssSelector(".shipping-item-title")).Text, " Name shipping Method in cart");
            VerifyAreEqual("Cash", driver.FindElement(By.CssSelector(".payment-item-title")).Text, " Name payment Method in cart");

            VerifyFinally(testname);
        }
        [Test]
        public void SettingsMethodEditInplace()
        {
            testname = "SettingsMethodEditInplace";
            VerifyBegin(testname);

            GoToAdmin("settings/shippingmethods");
            Thread.Sleep(1000);
            VerifyAreEqual("FixedRateMailMen Фиксированная стоимость доставки", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingName\"]")).Text, " Shiping Name 1 Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnabled\"] input")).GetAttribute("value"), " Shiping Enabled 1 Method in Setting");
            VerifyAreEqual("FreeShipping Бесплатная доставка", driver.FindElements(By.CssSelector("[data-e2e=\"ShippingName\"]"))[1].Text, " Shiping Name 2 Method in Setting");
            VerifyAreEqual("false", driver.FindElements(By.CssSelector("[data-e2e=\"ShippingEnabled\"] input"))[1].GetAttribute("value"), " Shiping Enabled 2 Method in Setting");

            driver.FindElements(By.CssSelector(".switcher-state-trigger"))[2].Click();
            WaitForAjax();
            VerifyAreEqual("true", driver.FindElements(By.CssSelector(".switcher-state-label input"))[1].GetAttribute("value"), " change Shiping Enabled Method in Setting");

            GoToAdmin("settings/paymentmethods");
            VerifyAreEqual("Cash Наличные", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentName\"]")).Text, " payment Name 1 Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentEnabled\"] input")).GetAttribute("value"), " payment Enabled 1 Method in Setting");
            VerifyAreEqual("WalletOne Единая Касса (Wallet One)", driver.FindElements(By.CssSelector("[data-e2e=\"PaymentName\"]"))[1].Text, " payment Name 2 Method in Setting");
            VerifyAreEqual("false", driver.FindElements(By.CssSelector("[data-e2e=\"PaymentEnabled\"] input"))[1].GetAttribute("value"), " payment Enabled 2 Method in Setting");

            driver.FindElements(By.CssSelector(".switcher-state-trigger"))[2].Click();
            WaitForAjax();
            VerifyAreEqual("true", driver.FindElements(By.CssSelector(".switcher-state-label input"))[1].GetAttribute("value"), " change payment Enabled Method in Setting");

            ProductToCard();

            //отображение методов в корзине
            GoToClient("checkout");
            VerifyAreEqual("2", driver.FindElements(By.CssSelector(".checkout-block"))[1].FindElements(By.CssSelector(".custom-input-radio")).Count.ToString(), "change shipping Method in cart");
            VerifyAreEqual("2", driver.FindElements(By.CssSelector(".checkout-block"))[3].FindElements(By.CssSelector(".payment-item-radio")).Count.ToString(), "change payment Method in cart");
            VerifyAreEqual("FixedRateMailMen", driver.FindElement(By.CssSelector(".shipping-item-title")).Text, " Name shipping 1 Method in cart");
            VerifyAreEqual("Cash", driver.FindElement(By.CssSelector(".payment-item-title")).Text, " Name payment 1 Method in cart");
            VerifyAreEqual("FreeShipping", driver.FindElements(By.CssSelector(".shipping-item-title"))[1].Text, " Name shipping 2 Method in cart");
            VerifyAreEqual("WalletOne", driver.FindElements(By.CssSelector(".payment-item-title"))[1].Text, " Name payment 2 Method in cart");

            VerifyFinally(testname);
        }
        [Test]
        public void SettingsMethodsDel()
        {
            testname = "SettingsMethodDel";
            VerifyBegin(testname);
            GoToAdmin("settings/shippingmethods");
            driver.FindElement(By.CssSelector(".fa.fa-times")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".fa.fa-times")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".shipping-text")).Count == 0, "del ship method in setting");

            GoToAdmin("settings/paymentmethods");
            driver.FindElement(By.CssSelector(".fa.fa-times")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".fa.fa-times")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".payment-text")).Count == 0, "del pay method in setting");

            ProductToCard();

            GoToClient("checkout");
            VerifyAreEqual("Нет доступных методов доставки", driver.FindElement(By.Id("checkoutshippings")).Text, " no method shipping for city in cart");
            VerifyAreEqual("Нет доступных методов оплаты", driver.FindElement(By.Id("checkoutpayment")).Text, " no method payment in cart");

            VerifyFinally(testname);
        }
        public void ProductToCard()
        {
            GoToClient();
            if (driver.FindElement(By.CssSelector(".cart-mini span")).Text.Contains("пусто"))
            {
                ScrollTo(By.CssSelector(".products-specials-more"));
                driver.FindElement(By.CssSelector(".products-view-buttons")).Click();
                Thread.Sleep(2000);
            }
        }
    }
}