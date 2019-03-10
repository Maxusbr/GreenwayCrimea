using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Selenium.Test.Admin.Settings.Tests.BonusSystem
{
    [TestFixture]
    public class BonusSystemSettingsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses| ClearType.Shipping);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\Settings\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Settings\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Settings\\Catalog.Category.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Settings\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Settings\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Settings\\Customers.Customer.csv",
            "data\\Admin\\Settings\\BonusSystem\\Settings\\[Order].ShippingMethod.csv",
            "data\\Admin\\Settings\\BonusSystem\\Settings\\[Order].ShippingParam.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Settings\\Bonus.Grade.csv",
            "Data\\Admin\\Settings\\BonusSystem\\Settings\\Bonus.Card.csv"

                );
            Init();
        }

      
        [Test]
        public void CheckBonusType()
        {
            GoToAdmin("settingsbonus");
            testname = "CheckzBonusType";
            VerifyBegin(testname);

            (new SelectElement(driver.FindElement(By.Id("BonusType")))).SelectByText("Стоимость товаров и доставки");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);
            IWebElement selectElem1 = driver.FindElement(By.Id("BonusType"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Стоимость товаров и доставки"), "select item shipping");

            ProductToCard("+5 руб. на бонусную карту");
            GoToClient("checkout");
            ScrollTo(By.CssSelector(".footer-menu-head"));
            VerifyAreEqual("200 руб.", driver.FindElement(By.CssSelector(".checkout-result-price")).Text, " checkout rezult with shipping");
            VerifyAreEqual("10 руб.", driver.FindElement(By.CssSelector(".checkout-bonus-result")).FindElement(By.CssSelector(".checkout-result-price")).Text, "bonus count in checkout with shipping");

            GoToAdmin("settingsbonus");
           
            (new SelectElement(driver.FindElement(By.Id("BonusType")))).SelectByText("Стоимость товаров");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);           

            GoToClient("checkout");
            ScrollTo(By.CssSelector(".footer-menu-head"));
            VerifyAreEqual("200 руб.", driver.FindElement(By.CssSelector(".checkout-result-price")).Text, " checkout rezult without shipping");
            VerifyAreEqual("5 руб.", driver.FindElement(By.CssSelector(".checkout-bonus-result")).FindElement(By.CssSelector(".checkout-result-price")).Text, "bonus count in checkout rezult without shipping");

            VerifyFinally(testname);
        }

        [Test]
        public void CheckDescriptionBonus()
        {
            GoToAdmin("settingsbonus");
            testname = "CheckDescriptionBonus";
            VerifyBegin(testname);

            SetCkText("Description BonusTextBlock", "BonusTextBlock");
            SetCkText("Description BonusRightTextBlock", "BonusRightTextBlock");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);
            AssertCkText("Description BonusTextBlock", "BonusTextBlock");
            AssertCkText("Description BonusRightTextBlock", "BonusRightTextBlock");

            GoToClient("getbonuscard");
            VerifyAreEqual("Description BonusTextBlock", driver.FindElement(By.CssSelector(".loyalty-txt")).Text, "main description ");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".site-body-cell-no-right")).Text.Contains("Description BonusRightTextBlock"), "right block description");

            VerifyFinally(testname);
        }
        [Test]
        public void CheckPaymentBonus()
        {
            GoToAdmin("settingsbonus");
            testname = "CheckPaymentBonus";
            VerifyBegin(testname);
            driver.FindElement(By.Id("MaxOrderPercent")).Clear();
            driver.FindElement(By.Id("MaxOrderPercent")).SendKeys("50");
            (new SelectElement(driver.FindElement(By.Id("BonusType")))).SelectByText("Стоимость товаров");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("checkout");

            ScrollTo(By.Id("isBonusApply"));
            VerifyAreEqual("Бонусами по карте (у вас 1000,0 бонусов)", driver.FindElement(By.CssSelector(".custom-input-text")).Text, "bonus count in checkout checkbox");
            driver.FindElement(By.Id("isBonusApply")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.CssSelector(".footer-menu-head"));
            VerifyAreEqual("150 руб.", driver.FindElement(By.CssSelector(".checkout-result-price")).Text, " checkout rezult isBonusApply");
            VerifyAreEqual("2,50 руб.", driver.FindElement(By.CssSelector(".checkout-bonus-result")).FindElement(By.CssSelector(".checkout-result-price")).Text, "bonus count in checkout rezult isBonusApply");
            
            GoToAdmin("settingsbonus");
            (new SelectElement(driver.FindElement(By.Id("BonusType")))).SelectByText("Стоимость товаров и доставки");
            driver.FindElement(By.Id("MaxOrderPercent")).Clear();
            driver.FindElement(By.Id("MaxOrderPercent")).SendKeys("100");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("checkout");
            ScrollTo(By.CssSelector(".footer-menu-head"));
            VerifyAreEqual("0 руб.", driver.FindElement(By.CssSelector(".checkout-result-price")).Text, " checkout zero rezult");
           
            VerifyFinally(testname);
        }

        public void ProductToCard(string bonus)
        {
            GoToClient("products/test-product1");
            if (driver.FindElement(By.CssSelector(".cart-mini span")).Text.Contains("пусто"))
            {
                ScrollTo(By.CssSelector(".rating"));
                VerifyAreEqual(bonus, driver.FindElement(By.CssSelector(".bonus-string-sum")).Text, "Count bonus in product cart");
                driver.FindElement(By.CssSelector(".details-payment-inline a")).Click();
                Thread.Sleep(2000);
            }
        }
    }
}
