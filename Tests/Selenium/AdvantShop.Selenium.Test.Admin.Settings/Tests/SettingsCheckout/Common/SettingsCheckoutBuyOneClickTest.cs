using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.Selenium.Test.Admin.Settings.Tests.SettingsCheckout
{
    [TestFixture]
    public class SettingsCheckoutBuyOneClickTest : BaseSeleniumTest
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
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].Certificate.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderContact.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderCurrency.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderItems.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderStatus.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].[Order].csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\[Order].OrderSource.csv"
                );
            Init();
            
            GoToClient("products/test-product5");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();
        }
        [Test]
        public void EnableBuyInOneClick()
        {
            testname = "EnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
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
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-buy-one-click")).Displayed, "btn buy inOneClick");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-buy-one-click")).Enabled, "Enabled btn buy inOneClick");
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Displayed, "btn buy cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Enabled, "Enabled btn buy cart");
            
            GoToClient("checkout");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-oneclick")).Displayed, " btn buy  checkout");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-oneclick a")).Enabled, "Enabled btn buy checkout");
           
            VerifyFinally(testname);
        }

        [Test]
        public void NotEnableBuyInOneClick()
        {
            testname = "NotEnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (!driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
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
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-buy-one-click")).Displayed, "btn buy inOneClick");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-buy-one-click")).Enabled, "Enabled btn buy inOneClick");
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Displayed, "btn buy cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Enabled, "Enabled btn buy cart");

            GoToClient("checkout");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".checkout-cart-oneclick")).Count==0, " btn buy no checkout");

            VerifyFinally(testname);
        }
        [Test]
        public void NotVisibleBuyInOneClick()
        {
            testname = "NotVisibleBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
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
            VerifyIsTrue(driver.FindElements(By.CssSelector(".details-buy-one-click")).Count == 0, "btn buy inOneClick");
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".btn.btn-middle.btn-action span")).Count == 0, "btn buy cart");

            GoToClient("checkout");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".checkout-cart-oneclick")).Count == 0, " btn buy no checkout");

            VerifyFinally(testname);
        }
        [Test]
        public void NotVisibleEnableBuyInOneClick()
        {
            testname = "NotVisibleEnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (!driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
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
            VerifyIsTrue(driver.FindElements(By.CssSelector(".details-buy-one-click")).Count == 0, "btn buy inOneClick");
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".btn.btn-middle.btn-action span")).Count == 0, "btn buy cart");

            GoToClient("checkout");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".checkout-cart-oneclick")).Count == 0, " btn buy no checkout");

            VerifyFinally(testname);
        }

        [Test]
        public void SelectLeadBuyInOneClick()
        {
            testname = "EnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
                Thread.Sleep(2000);
            }
            ScrollTo(By.Id("BuyInOneClick"));
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickAction")))).SelectByText("Создавать лид");
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");

            driver.FindElement(By.CssSelector(".details-buy-one-click")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("buyOneClickFormName")).Click();
            driver.FindElement(By.Name("buyOneClickFormName")).Clear();
            driver.FindElement(By.Name("buyOneClickFormName")).SendKeys("NewLead");
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-inner")).Displayed, "display windows");
            VerifyAreEqual("Оформление заказа", driver.FindElement(By.CssSelector(".modal-header")).Text, "windows");
            VerifyAreEqual("Ваш телефон внесен в очередь, скоро мы с Вами свяжемся.", driver.FindElement(By.CssSelector(".buy-one-click-final-text")).Text, "final text");

            driver.FindElement(By.CssSelector(".buy-one-click-buttons a")).Click();

            GoToAdmin("leads");

            VerifyAreEqual("Новый", GetGridCell(0, "DealStatusName").Text, " Grid lead StatusName");
            VerifyAreEqual("500", GetGridCell(0, "Sum").Text, " Grid lead sum");



            VerifyFinally(testname);
        }
        [Test]
        public void SelectOrderBuyInOneClick()
        {
            testname = "EnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
                Thread.Sleep(2000);
            }
            ScrollTo(By.Id("BuyInOneClick"));
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickAction")))).SelectByText("Создавать заказ");
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");
          
            driver.FindElement(By.CssSelector(".details-buy-one-click")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("buyOneClickFormName")).Click();
            driver.FindElement(By.Name("buyOneClickFormName")).Clear();
            driver.FindElement(By.Name("buyOneClickFormName")).SendKeys("NewOrder");
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text, "sucsess");

            GoToAdmin("orders");

            VerifyAreEqual("NewOrder", GetGridCell(0, "BuyerName").Text, " Grid orders StatusName");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            VerifyAreEqual("500 руб.", GetGridCell(0, "SumFormatted").Text, " Grid orders sum");                    

            VerifyFinally(testname);
        }


        [Test]
        public void SelectPaymentBuyInOneClick()
        {
            testname = "EnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
                Thread.Sleep(2000);
            }
            ScrollTo(By.Id("BuyInOneClick"));
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickAction")))).SelectByText("Создавать заказ");
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickDefaultPaymentMethod")))).SelectByText("Наложенный платеж");
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");

            driver.FindElement(By.CssSelector(".details-buy-one-click")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("buyOneClickFormName")).Click();
            driver.FindElement(By.Name("buyOneClickFormName")).Clear();
            driver.FindElement(By.Name("buyOneClickFormName")).SendKeys("NewOrderPay");
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text, "sucsess");

            GoToAdmin("orders");

            VerifyAreEqual("NewOrderPay", GetGridCell(0, "BuyerName").Text, " Grid orders StatusName");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");

            GetGridCell(0, "StatusName").Click();
            VerifyAreEqual("Наложенный платеж", driver.FindElement(By.CssSelector("[ng-bind=\"$ctrl.Summary.PaymentName\"]")).Text, " method");

            GoToAdmin("settingscheckout");
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickDefaultPaymentMethod")))).SelectByText("----");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            GoToClient("products/test-product5");

            driver.FindElement(By.CssSelector(".details-buy-one-click")).Click();
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);

            GoToAdmin("orders");

            GetGridCell(0, "StatusName").Click();
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[ng-bind=\"$ctrl.Summary.ShippingName\"]")).Text, " no method");


            VerifyFinally(testname);
        }
        [Test]
        public void SelectShippingBuyInOneClick()
        {
            testname = "EnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
                Thread.Sleep(2000);
            }
            ScrollTo(By.Id("BuyInOneClick"));
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickAction")))).SelectByText("Создавать заказ");
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickDefaultShippingMethod")))).SelectByText("Курьером");
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");

            driver.FindElement(By.CssSelector(".details-buy-one-click")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Name("buyOneClickFormName")).Click();
            driver.FindElement(By.Name("buyOneClickFormName")).Clear();
            driver.FindElement(By.Name("buyOneClickFormName")).SendKeys("NewOrderShipping");
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text, "sucsess");

            GoToAdmin("orders");

            VerifyAreEqual("NewOrderShipping", GetGridCell(0, "BuyerName").Text, " Grid orders StatusName");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");

            GetGridCell(0, "StatusName").Click();
            VerifyAreEqual("Курьером", driver.FindElement(By.CssSelector("[ng-bind=\"$ctrl.Summary.ShippingName\"]")).Text, " method");


            GoToAdmin("settingscheckout");
            (new SelectElement(driver.FindElement(By.Id("BuyInOneClickDefaultShippingMethod")))).SelectByText("----");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            GoToClient("products/test-product5");

            driver.FindElement(By.CssSelector(".details-buy-one-click")).Click();
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(2000);

            GoToAdmin("orders");
            
            GetGridCell(0, "StatusName").Click();
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[ng-bind=\"$ctrl.Summary.ShippingName\"]")).Text, " no method");


            VerifyFinally(testname);
        }
        [Test]
        public void TextBuyInOneClick()
        {
            testname = "EnableBuyInOneClick";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("BuyInOneClick")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"BuyInOneClick\"]")).Click();
                Thread.Sleep(2000);
            }
            if (driver.FindElement(By.Id("BuyInOneClickDisableInCheckout")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"OneClickDisableInCheckout\"]")).Click();
                Thread.Sleep(2000);
            }
            driver.FindElement(By.Id("BuyInOneClickLinkText")).Clear();
            driver.FindElement(By.Id("BuyInOneClickLinkText")).SendKeys("Test link text");

            driver.FindElement(By.Id("BuyInOneClickFirstText")).Clear();
            driver.FindElement(By.Id("BuyInOneClickFirstText")).SendKeys("Test big text in window");

            driver.FindElement(By.Id("BuyInOneClickButtonText")).Clear();
            driver.FindElement(By.Id("BuyInOneClickButtonText")).SendKeys("TestOrder");

            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product5");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-buy-one-click")).Displayed, "btn buy inOneClick");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".details-buy-one-click")).Enabled, "Enabled btn buy inOneClick");
            VerifyAreEqual("Test link text", driver.FindElement(By.CssSelector(".details-buy-one-click")).Text, "text btn buy inOneClick prod");
            driver.FindElement(By.CssSelector(".details-buy-one-click")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Test big text in window", driver.FindElement(By.CssSelector(".buy-one-click-text")).Text, " big text btn buy inOneClick");
            VerifyAreEqual("TestOrder", driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).GetAttribute("value"), "btn text btn buy inOneClick");

            GoToClient("cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Displayed, "btn buy cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Enabled, "Enabled btn buy cart");
            VerifyAreEqual("Test link text", driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Text, "text btn buy inOneClick cart");
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action span")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Test big text in window", driver.FindElement(By.CssSelector(".buy-one-click-text")).Text, " big text btn buy inOneClick cart");
            VerifyAreEqual("TestOrder", driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).GetAttribute("value"), "btn text btn buy inOneClick cart");

            GoToClient("checkout");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-oneclick")).Displayed, " btn buy  checkout");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-oneclick a")).Enabled, "Enabled btn buy checkout");
            VerifyAreEqual("Test link text", driver.FindElement(By.CssSelector(".checkout-cart-oneclick a")).Text, "text btn buy inOneClick checkout");
            driver.FindElement(By.CssSelector(".checkout-cart-oneclick a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Test big text in window", driver.FindElement(By.CssSelector(".buy-one-click-text")).Text, " big text btn buy inOneClick checkout");
            VerifyAreEqual("TestOrder", driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).GetAttribute("value"), "btn text btn buy inOneClick checkout");

            VerifyFinally(testname);
        }
    }
}
