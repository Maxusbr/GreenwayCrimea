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
    public class SettingsCheckoutPrintTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            Init();
        }
        [Test]
        public void CheckField()
        {
            testname = "CheckField";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");
            ScrollTo(By.CssSelector("[data-e2e=\"NextOrderNumber\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"ShowStatusInfo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShowMap\"]")).Click();
            (new SelectElement(driver.FindElement(By.Id("PrintOrder_MapType")))).SelectByText("Google maps");
            ScrollTo(By.Id("header-top"));
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            }
            catch { }
            Refresh();
            VerifyIsFalse(driver.FindElement(By.Id("PrintOrder_ShowStatusInfo")).Selected, "check status info");
            VerifyIsTrue(driver.FindElement(By.Id("PrintOrder_ShowMap")).Selected, "check show map");
            IWebElement selectElem1 = driver.FindElement(By.Id("PrintOrder_MapType"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Google maps"), "check type map");

            VerifyFinally(testname);
        }
    }
    [TestFixture]
    public class SettingsCheckoutGiftTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Product.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.ProductGifts.csv",
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
        }
        [Test]
        public void CheckMultiplyGifts()
        {
            testname = "CheckMultiplyGifts";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");
            ScrollTo(By.CssSelector("[data-e2e=\"NextOrderNumber\"]"));

            if (!driver.FindElement(By.Id("MultiplyGiftsCount")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"MultiplyGiftsCount\"]")).Click();
                Thread.Sleep(2000);
            }            
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            }
            catch { }

            GoToClient("products/test-product21");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1, "count gift");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 1, "count product gift");

            MouseFocus(driver, By.CssSelector(".product-gift-image"));
            VerifyAreEqual("TestProduct10", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text, "name gift");
            driver.FindElement(By.CssSelector(".input-small")).SendKeys("0");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("cart");
            VerifyAreEqual("TestProduct21", driver.FindElement(By.CssSelector(".cart-full-name-link")).Text, "name cart");
            VerifyAreEqual("TestProduct10", driver.FindElements(By.CssSelector(".cart-full-name-link"))[1].Text, "name 2 cart");

            VerifyAreEqual("10", driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).GetAttribute("value"), "Amount cart");
            VerifyAreEqual("10", driver.FindElement(By.CssSelector("[data-ng-bind=\"item.Amount\"]")).Text, "Amount 2 cart");

            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).SendKeys("5");
            DropFocus("h1");
            Refresh();
            VerifyAreEqual("5", driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).GetAttribute("value"), "Amount change cart");
            VerifyAreEqual("5", driver.FindElement(By.CssSelector("[data-ng-bind=\"item.Amount\"]")).Text, "Amount change 2 cart");
            
            driver.FindElement(By.CssSelector(".cart-full-buttons a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(1000);

            VerifyFinally(testname);
        }
        [Test]
        public void CheckSingleGifts()
        {
            testname = "CheckSingleGifts";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");
            ScrollTo(By.CssSelector("[data-e2e=\"NextOrderNumber\"]"));

            if (driver.FindElement(By.Id("MultiplyGiftsCount")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"MultiplyGiftsCount\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            }
            catch { }

            GoToClient("products/test-product21");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1, "count gift");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 1, "count product gift");

            MouseFocus(driver, By.CssSelector(".product-gift-image"));
            VerifyAreEqual("TestProduct10", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text, "name gift");
            driver.FindElement(By.CssSelector(".input-small")).SendKeys("0");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("cart");
            VerifyAreEqual("TestProduct21", driver.FindElement(By.CssSelector(".cart-full-name-link")).Text, "name cart");
            VerifyAreEqual("TestProduct10", driver.FindElements(By.CssSelector(".cart-full-name-link"))[1].Text, "name 2 cart");

            VerifyAreEqual("10", driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).GetAttribute("value"), "Amount cart");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-ng-bind=\"item.Amount\"]")).Text, "Amount 2 cart");

            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).SendKeys("5");
            DropFocus("h1");
            Refresh();
            VerifyAreEqual("5", driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).GetAttribute("value"), "Amount change cart");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-ng-bind=\"item.Amount\"]")).Text, "Amount change 2 cart");

            driver.FindElement(By.CssSelector(".cart-full-buttons a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".buy-one-click-buttons input")).Click();
            Thread.Sleep(1000);

            VerifyFinally(testname);
        }
    }
}
