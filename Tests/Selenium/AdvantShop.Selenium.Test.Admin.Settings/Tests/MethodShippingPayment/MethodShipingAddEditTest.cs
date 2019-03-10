using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.ShippingMethod
{
    [TestFixture]
    public class ShipingTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Shipping | ClearType.Payment|ClearType.Catalog);
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
        public void SettingsMethodShipingEdittCourier()
        {
            testname = "EditCourier";
            VerifyBegin(testname);

            GoToAdmin("settings/shippingmethods");

            VerifyAreEqual("FixedRateMailMen Фиксированная стоимость доставки", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingName\"]")).Text, " Shiping Name 1 Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnabled\"] input")).GetAttribute("value"), " Shiping Enabled 1 Method in Setting");
            VerifyAreEqual("FreeShipping Бесплатная доставка", driver.FindElements(By.CssSelector("[data-e2e=\"ShippingName\"]"))[1].Text, " Shiping Name 2 Method in Setting");
            VerifyAreEqual("false", driver.FindElements(By.CssSelector("[data-e2e=\"ShippingEnabled\"] input"))[1].GetAttribute("value"), " Shiping Enabled 2 Method in Setting");

            GoToAdmin("shippingmethods/edit/1");
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("FixedRateMailMen"), " h1 teg edit shipping");
            VerifyAreEqual("FixedRateMailMen", driver.FindElement(By.Id("Name")).GetAttribute("value"), " Shiping Name 1 Method in edit ");
            VerifyIsTrue(driver.FindElement(By.Name("Enabled")).Selected, " Shiping Enabled 1 Method in edit");

            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("FixedRateCourier"); //.col-xs-9 a
     
            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("brand_logo.jpg"));
            //country
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountryName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountryName\"]")).SendKeys("Россия");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountryAdd\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountryName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountryName\"]")).SendKeys("Беларусь");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountryAdd\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountryDel\"]")).Click();
            Thread.Sleep(1000);
            //city
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCityName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCityName\"]")).SendKeys("Анапа");
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCityAdd\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCityName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCityName\"]")).SendKeys("Москва");
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCityAdd\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCityDel\"]")).Click();
            Thread.Sleep(1000);
           // VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCity\"]")).Text.Contains("Москва"), " Shiping 1 city in edit ");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"ShippingCountry\"]")).Text.Contains("Беларусь"), " Shiping country 1 Method in edit ");

            //city
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnCity\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnCity\"]")).SendKeys("Казань");
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnCityAdd\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnCityList\"]")).Text.Contains("Казань"), " Shiping enabled city in edit ");
            //description
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("New Description here");
            driver.FindElement(By.Id("ZeroPriceMessage")).Clear();
            driver.FindElement(By.Id("ZeroPriceMessage")).SendKeys("New ZeroPriceMessage here");
            driver.FindElement(By.Id("SortOrder")).Clear();
            driver.FindElement(By.Id("SortOrder")).SendKeys("0");

            ScrollTo(By.CssSelector(".btn.btn-sm.btn-action"));

            VerifyIsTrue(driver.FindElement(By.Name("Enabled")).Selected, " Shiping Enabled 1 Method in edit");
            VerifyIsTrue(driver.FindElement(By.Name("DisplayCustomFields")).Selected, " Shiping DisplayCustomFields 1 Method in edit");
            VerifyIsTrue(driver.FindElement(By.Name("DisplayIndex")).Selected, " Shiping DisplayIndex 1 Method in edit");
            VerifyIsTrue(!driver.FindElement(By.Name("ShowInDetails")).Selected, " Shiping ShowInDetails 1 Method in edit");
        
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingAddittional\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingIndex\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingDetais\"]")).Click();
            driver.FindElements(By.CssSelector("[data-e2e=\"PaymentMethods\"]"))[0].Click();
            driver.FindElements(By.CssSelector("[data-e2e=\"PaymentMethods\"]"))[1].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("ShippingPrice")).Clear();
            driver.FindElement(By.Id("ShippingPrice")).SendKeys("100");
            driver.FindElement(By.Id("DeliveryTime")).Clear();
            driver.FindElement(By.Id("DeliveryTime")).SendKeys("30");
            Thread.Sleep(2000);
                        
            ScrollTo(By.ClassName("search-input-wrap"));

            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingAdd\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingReturn\"] a")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("settings/shippingmethods"), " return from edit shipping");

            VerifyAreEqual("FixedRateCourier Фиксированная стоимость доставки", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingName\"]")).Text, " edited Shiping Name 1 Method in Setting");
            VerifyAreEqual("true", driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnabled\"] input")).GetAttribute("value"), "edited  Shiping Enabled 1 Method in Setting");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".shipping-icon img")).GetAttribute("src").Contains("nophoto"), "image method in Setting");

            ProductToCard();

            GoToClient("checkout");
            Refresh();
            Thread.Sleep(7000);
            VerifyAreEqual("1", driver.FindElements(By.CssSelector(".checkout-block"))[1].FindElements(By.CssSelector(".custom-input-radio")).Count.ToString(), "change shipping Method in cart");
            VerifyAreEqual("FixedRateCourier", driver.FindElement(By.CssSelector(".shipping-item-title")).Text, " Name edited shipping Method in cart");
           // В тестах не поазывают цену и описание

            // VerifyAreEqual("New Description here", driver.FindElement(By.CssSelector(".readmore-content")).Text, " Description edited shipping Method in cart");
           // VerifyIsTrue(driver.FindElement(By.CssSelector(".shipping-item-description.cs-t-3")).Text.Contains("100 руб."), " price edited shipping Method in cart");
          //  VerifyIsTrue(driver.FindElement(By.CssSelector(".shipping-item-description.cs-t-3")).Text.Contains("30"), " time edited shipping Method in cart");
            VerifyAreEqual("100 руб.", driver.FindElement(By.CssSelector("[data-ng-bind=\"checkout.Cart.Delivery\"]")).Text, " cost edited shipping Method in cart");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".shipping-item-icon img")).GetAttribute("src").Contains("nophoto"), "image method in cart");

            VerifyAreEqual("Нет доступных методов оплаты", driver.FindElement(By.Id("checkoutpayment")).Text, " no method payment in cart");
            driver.FindElement(By.CssSelector(".link-dotted-invert")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//a[contains(text(), 'Казань')]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Нет доступных методов доставки", driver.FindElement(By.Id("checkoutshippings")).Text, " no method shipping for city in cart");
            VerifyAreEqual("Нет доступных методов оплаты", driver.FindElement(By.Id("checkoutpayment")).Text, " no method payment in cart");

            VerifyFinally(testname);
        }

      
        [Test]
        public void SettingsMethodShippinggAdd()
        {
            testname = "SettingsMethodShippinggAdd";
            VerifyBegin(testname);
            GoToAdmin("settings/shippingmethods");

            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingAdd\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Метод доставки", driver.FindElement(By.TagName("h2")).Text, " h2 add shipping method");
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingAddName\"]")).SendKeys("New Shipping Method");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"ShippingAddSelect\"]")))).SelectByText("Бесплатная доставка");
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingAddDecs\"]")).SendKeys("New Description Shipping Method");
            driver.FindElement(By.CssSelector("[data-e2e=\"ShippingAdd\"]")).Click();
            Thread.Sleep(2000);
            if(!driver.FindElement(By.Name("Enabled")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ShippingEnabled\"]")).Click();
            }

            GoToAdmin("settings/shippingmethods");
            VerifyIsTrue(driver.PageSource.Contains("New Shipping Method"), "Show on page shipping method in list");
           
            ProductToCard();
            //отображение методов в корзине
            GoToClient("checkout");
            VerifyIsTrue(driver.PageSource.Contains("New Shipping Method"), "Show on page shipping method in cart");
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
