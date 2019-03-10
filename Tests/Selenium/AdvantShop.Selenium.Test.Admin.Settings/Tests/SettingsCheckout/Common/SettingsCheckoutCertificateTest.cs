using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.Selenium.Test.Admin.Settings.Tests.SettingsCheckout.Common
{
    [TestFixture]
    public class SettingsCheckoutCertificateTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog|ClearType.Orders);
            InitializeService.LoadData(
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Product.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Offer.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Category.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.ProductCategories.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\Catalog.Coupon.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\Catalog.CouponCategories.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\Catalog.CouponProducts.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].Certificate.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderContact.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderCurrency.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderItems.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderStatus.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].[Order].csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\[Order].OrderSource.csv"
                );
            Init();
        }
        [Test]
        public void EnableCertificate()
        {
            testname = "EnableCertificate";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            ScrollTo(By.Id("BuyInOneClickDisableInCheckout"));
            if (!driver.FindElement(By.Id("EnableGiftCertificateService")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"EnableCertificate\"]")).Click();
                Thread.Sleep(2000);                
            }
            if (!driver.FindElement(By.Id("DisplayPromoTextbox")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DisplayPromoTextbox\"]")).Click();
                Thread.Sleep(2000);
            }
          
            driver.FindElement(By.Id("MinimalPriceCertificate")).Clear();
            driver.FindElement(By.Id("MinimalPriceCertificate")).SendKeys("100");

            driver.FindElement(By.Id("MaximalPriceCertificate")).Clear();
            driver.FindElement(By.Id("MaximalPriceCertificate")).SendKeys("1000");
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".cart-full-coupon")).Displayed, "certificate block cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-gift-button")).Enabled, "certificate btn cart");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("Certificate2");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("Сертификат:", driver.FindElements(By.CssSelector(".cart-full-summary-name"))[1].Text, "client coupon");
            VerifyAreEqual("2 000 руб.", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[0].Text, "client sum coupon");
            driver.FindElement(By.CssSelector(".cart-full-summary-price a")).Click();
            Thread.Sleep(1000);

            GoToClient("checkout");
            ScrollTo(By.Id("WantBonusCard"));
            VerifyIsTrue(driver.FindElement(By.Name("cardsFormBlock")).Displayed, "certificate block checkout");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-gift-button")).Enabled, "certificate btn checkout");

            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("Certificate2");
            driver.FindElement(By.CssSelector(".btn.btn-small.btn-action.btn-expander")).Click();
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-result")).Text.Contains("0 руб."), "checkout afred cert rezult");
            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();
            Thread.Sleep(4000);

            VerifyFinally(testname);
        }
        [Test]
        public void NotEnableCertificate()
        {
            testname = "NotEnableCertificate";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            ScrollTo(By.Id("BuyInOneClickDisableInCheckout"));
            if (driver.FindElement(By.Id("EnableGiftCertificateService")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"EnableCertificate\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("DisplayPromoTextbox")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DisplayPromoTextbox\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-coupon")).Count == 0, "certificate block cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".checkout-gift-button")).Count == 0, "certificate btn cart");
            GoToClient("checkout");
            VerifyIsTrue(driver.FindElements(By.Name("cardsFormBlock")).Count == 0, "certificate block checkout");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".checkout-gift-button")).Count==0, "certificate btn checkout");


            VerifyFinally(testname);
        }
        [Test]
        public void NotVisibleCertificate()
        {
            testname = "NotVisibleCertificate";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            ScrollTo(By.Id("BuyInOneClickDisableInCheckout"));
            if (!driver.FindElement(By.Id("EnableGiftCertificateService")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"EnableCertificate\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("DisplayPromoTextbox")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DisplayPromoTextbox\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-coupon")).Count == 0, "certificate block cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".checkout-gift-button")).Count == 0, "certificate btn cart");
            GoToClient("checkout");
            VerifyIsTrue(driver.FindElements(By.Name("cardsFormBlock")).Count == 0, "certificate block checkout");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".checkout-gift-button")).Count == 0, "certificate btn checkout");

            GoToAdmin("settingscheckout");

            ScrollTo(By.Id("BuyInOneClickDisableInCheckout"));
            if (driver.FindElement(By.Id("EnableGiftCertificateService")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"EnableCertificate\"]")).Click();
                Thread.Sleep(2000);
            }
            if (!driver.FindElement(By.Id("DisplayPromoTextbox")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DisplayPromoTextbox\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }
            
            GoToClient("cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".cart-full-coupon")).Displayed, "certificate block cart 1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-gift-button")).Enabled, "certificate btn cart 1");

            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("Certificate2");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.Id("toast-container")).Displayed, "certificate disable block cart");

            GoToClient("checkout");
            ScrollTo(By.Id("WantBonusCard"));
            VerifyIsTrue(driver.FindElement(By.Name("cardsFormBlock")).Displayed, "certificate block checkout 1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-gift-button")).Enabled, "certificate btn checkout 1");

            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("Certificate2");
            driver.FindElement(By.CssSelector(".btn.btn-small.btn-action.btn-expander")).Click();
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElement(By.Id("toast-container")).Displayed, "certificate disable block checkout");
            VerifyAreNotEqual("0 руб.", driver.FindElement(By.CssSelector(".checkout-result span")).Text, "checkout afred cert rezult");
            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();
            Thread.Sleep(4000);

            VerifyFinally(testname);
        }
        [Test]
        public void PriceCertificate()
        {
            testname = "MinPriceCertificate";
            VerifyBegin(testname);

            GoToAdmin("design");
            driver.FindElement(By.CssSelector(".other-button button")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.CssSelector("[data-e2e=\"Вы уже смотрели\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"Блок подарочных сертификатов\"] span")).Click();
            Thread.Sleep(2000);
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"Блок подарочных сертификатов\"] input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"Блок подарочных сертификатов\"] span")).Click();
            }
            driver.FindElement(By.CssSelector(".modal-footer button")).Click();
            Thread.Sleep(1000);

            GoToAdmin("settingscheckout");

            ScrollTo(By.Id("BuyInOneClickDisableInCheckout"));

            if (!driver.FindElement(By.Id("EnableGiftCertificateService")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"EnableCertificate\"]")).Click();
                Thread.Sleep(2000);
            }
            driver.FindElement(By.Id("MinimalPriceCertificate")).Clear();
             driver.FindElement(By.Id("MinimalPriceCertificate")).SendKeys("100");

             driver.FindElement(By.Id("MaximalPriceCertificate")).Clear();
             driver.FindElement(By.Id("MaximalPriceCertificate")).SendKeys("1000");
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("giftcertificate");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("NameTo")).SendKeys("TestTo");
            driver.FindElement(By.Id("NameFrom")).SendKeys("TestFrom");
            driver.FindElement(By.Id("Sum")).Clear();
            driver.FindElement(By.Id("Sum")).SendKeys("100000");
            driver.FindElement(By.Id("Message")).SendKeys("test");
            driver.FindElement(By.Id("EmailTo")).SendKeys("test@test.test");
            driver.FindElement(By.Id("EmailFrom")).Clear();
            driver.FindElement(By.Id("EmailFrom")).SendKeys("test1@test.test");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-small.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.Id("toast-container")).Displayed, "max sum");

            driver.FindElement(By.Id("Sum")).Clear();
            driver.FindElement(By.Id("Sum")).SendKeys("10");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-small.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.Id("toast-container")).Displayed, "min sum");

            driver.FindElement(By.Id("Sum")).Clear();
            driver.FindElement(By.Id("Sum")).SendKeys("1000");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-small.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text, "sucsess");

            GoToAdmin("orders");
            
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            VerifyAreEqual("1 000 руб.", GetGridCell(0, "SumFormatted").Text, " Grid orders sum");
            GetGridCell(0, "StatusName").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Сертификат", GetGridCell(0, "CustomName", "OrderCertificates").Text, " Name product at order");
            VerifyAreEqual("1000", GetGridCell(0, "Sum", "OrderCertificates").Text, " Sum at order");


            VerifyFinally(testname);
        }
    }
}
