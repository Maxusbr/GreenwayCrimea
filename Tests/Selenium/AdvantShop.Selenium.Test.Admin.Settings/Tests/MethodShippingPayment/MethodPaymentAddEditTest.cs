using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.MethodShippingPayment
{
    [TestFixture]
    public class MethodPaymentAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Shipping | ClearType.Payment| ClearType.Catalog);
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
        public void SettingsMethodPaymentEdittCash()
        {
            testname = "PaymentEdit";
            VerifyBegin(testname);
            GoToAdmin("settings/paymentmethods");
            VerifyAreEqual("Cash Наличные", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentName\"]")).Text, " payment Name 1 Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentEnabled\"] input")).GetAttribute("value"), " payment Enabled 1 Method in Setting");
            
            GoToAdmin("paymentmethods/edit/1");
            Thread.Sleep(2000);
            VerifyAreEqual("Метод оплаты - \"Cash\"", driver.FindElement(By.TagName("h1")).Text, " h1 teg edit payment");
            VerifyAreEqual("Cash", driver.FindElement(By.Name("Name")).GetAttribute("value"), " Shiping Name 1 Method in edit ");
            VerifyIsTrue(driver.FindElement(By.Name("Enabled")).Selected, " payment Enabled 1 Method in edit");
           // VerifyAreEqual("Наличные", driver.FindElements(By.Name(".row.middle-xs .col-xs-6.relative"))[2].Text, " payment type 1 Method in edit");
           
            driver.FindElement(By.Name("Name")).Clear();
            driver.FindElement(By.Name("Name")).SendKeys("TestNameCash");
            //country
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCountry\"]")).SendKeys("Россия");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCountryAdd\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCountry\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCountry\"]")).SendKeys("Беларусь");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCountryAdd\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Беларусь", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentNameCountry\"]")).Text, " payment 1 country in edit ");
            VerifyAreEqual(", Россия", driver.FindElements(By.CssSelector("[data-e2e=\"PaymentNameCountry\"]"))[1].Text, " payment 2 country in edit ");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCountryDel\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"PaymentNameCountry\"]")).Count==2, " Shiping country 1 Method in edit ");
            
            //city
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCity\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCity\"]")).SendKeys("Москва");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCityAdd\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCity\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCity\"]")).SendKeys("Самара");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentCityAdd\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Москва", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentNameCity\"]")).Text, " payment 1 city in edit ");
            VerifyAreEqual("Самара", driver.FindElements(By.CssSelector("[data-e2e=\"PaymentNameCity\"]"))[1].Text, " payment 2  city in edit ");
            Thread.Sleep(2000);
            driver.FindElements(By.CssSelector("[data-e2e=\"PaymentCityDel\"]"))[1].Click();

            //description
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("New Description here");
            driver.FindElement(By.Name("SortOrder")).Clear();
            driver.FindElement(By.Name("SortOrder")).SendKeys("0");

            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("brand_logo.jpg"));
            ScrollTo(By.Name("ExtrachargeType"));

            (new SelectElement(driver.FindElement(By.Name("ExtrachargeType")))).SelectByText("Фиксированная");
            driver.FindElement(By.Name("Extracharge")).Clear();
            driver.FindElement(By.Name("Extracharge")).SendKeys("300");

            Thread.Sleep(1000);
            ScrollTo(By.ClassName("search-input-wrap"));
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentAdd\"]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentReturn\"] a")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("settings/paymentmethods"), " return from edit paymentm");
            VerifyAreEqual("TestNameCash Наличные", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentName\"]")).Text, " payment Name 1 Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"PaymentEnabled\"] input")).GetAttribute("value"), " payment Enabled 1 Method in Setting");

            ProductToCard();

            //отображение методов в корзине
            GoToClient("checkout");

            VerifyAreEqual("TestNameCash", driver.FindElement(By.CssSelector(".payment-item-title")).Text, " Name edited payment Method in cart");
            VerifyAreEqual("New Description here", driver.FindElement(By.CssSelector(".payment-item-description.cs-t-3")).Text, "Description edited payment Method in cart");
            VerifyAreEqual("300 руб.", driver.FindElement(By.CssSelector("[data-ng-bind=\"checkout.Cart.Payment.Value\"]")).Text, " cost edited payment Method in cart");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsMethodPaymenttAdd()
        {
            testname = "SettingsMethodPaymenttAdd";
            VerifyBegin(testname);
            GoToAdmin("settings/paymentmethods");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentAdd\"]")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("Метод оплаты", driver.FindElement(By.TagName("h2")).Text, " h2 add payment method");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentAddName\"]")).SendKeys("New payment Method");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PaymentAddSelect\"]")))).SelectByText("Единая Касса (Wallet One)");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentAddDesc\"]")).SendKeys("New Description payment Method");
            driver.FindElement(By.CssSelector("[data-e2e=\"PaymentAdd\"]")).Click();
            Thread.Sleep(1000);

            GoToAdmin("settings/paymentmethods");
            VerifyIsTrue(driver.PageSource.Contains("New payment Method"), "Show on page method in list");
            driver.FindElements(By.CssSelector("[data-e2e=\"PaymentEnabled\"] span"))[4].Click();
            Thread.Sleep(1000);
            ProductToCard();
            //отображение методов в корзине
            GoToClient("checkout");
            Thread.Sleep(5000);
            VerifyIsTrue(driver.PageSource.Contains("New payment Method"), "Show on page method in cart");

            VerifyFinally(testname);
        }
        //.products-view-buttons
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
