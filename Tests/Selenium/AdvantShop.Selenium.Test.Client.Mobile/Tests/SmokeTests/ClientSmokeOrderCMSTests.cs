using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Client
{
    [TestFixture]
    public class ClientSmokeOrderCMSTests : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog | ClearType.Customers);
            InitializeService.LoadData(
            "data\\Client\\SmokeTests\\Catalog.Product.csv",
             "data\\Client\\SmokeTests\\Catalog.Photo.csv",
           "data\\Client\\SmokeTests\\Catalog.Offer.csv",
           "data\\Client\\SmokeTests\\Catalog.Category.csv",
           "data\\Client\\SmokeTests\\Catalog.ProductCategories.csv",
           "data\\Client\\SmokeTests\\Catalog.Brand.csv",
           "data\\Client\\SmokeTests\\Catalog.Tag.csv",
             "data\\Client\\SmokeTests\\Catalog.Property.csv",
                 "data\\Client\\SmokeTests\\Catalog.PropertyValue.csv",
                 "data\\Client\\SmokeTests\\Catalog.ProductPropertyValue.csv",
                "data\\Client\\SmokeTests\\Catalog.PropertyGroup.csv",
                "Data\\Client\\SmokeTests\\Catalog.Color.csv",
                "data\\Client\\SmokeTests\\Catalog.Size.csv",
               "data\\Client\\SmokeTests\\CMS.Menu.csv",
            "data\\Client\\SmokeTests\\CMS.StaticBlock.csv",
               "data\\Client\\SmokeTests\\CMS.StaticPage.csv",
                "data\\Client\\SmokeTests\\Settings.News.csv",
                "data\\Client\\SmokeTests\\Settings.NewsCategory.csv",
                "data\\Client\\SmokeTests\\Customers.Customer.csv",
                "data\\Client\\SmokeTests\\Customers.CustomerGroup.csv"
           );

            Init();
        }

         [Test]
        public void ClientProductToCart()
        {
            GoToClient("products/test-product106");

            driver.FindElement(By.CssSelector(".details-spinbox-block")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.CssSelector(".details-spinbox-block")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.CssSelector(".details-spinbox-block")).FindElement(By.TagName("input")).SendKeys("2");
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(3000);

            GoToClient("cart");

            Assert.AreEqual("Корзина", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.PageSource.Contains("TestProduct106"));
            Assert.AreEqual("2", driver.FindElement(By.CssSelector(".cart-full-amount-control input")).GetAttribute("value"));
            driver.FindElement(By.CssSelector(".cart-full-amount-control input")).Click();
            driver.FindElement(By.CssSelector(".cart-full-amount-control input")).Clear();
            driver.FindElement(By.CssSelector(".cart-full-amount-control input")).SendKeys("3");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".cart-full-result-price")).Text.Contains("318"));
        }

        [Test]
        public void ClientProductDeleteFromCart()
        {
            GoToClient("products/test-product107");
            
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(3000);

            GoToClient("cart");
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".cart-full-name-link")).Text.Contains("TestProduct107"));
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".cart-full-empty")).Count);
            driver.FindElement(By.CssSelector(".cart-full-body-item.cart-full-remove")).Click();
            Thread.Sleep(3000);
            GoToClient("cart");
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".cart-full-name-link")).Count);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".cart-full-empty")).Displayed);
        }


        [Test]
        public void ClientProductDeleteAllFromCart()
        {
            GoToClient("products/test-product108");

            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(3000);

            GoToClient("products/test-product109");

            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(3000);

            GoToClient("cart");

            Assert.IsTrue(driver.PageSource.Contains("TestProduct108"));
            Assert.IsTrue(driver.PageSource.Contains("TestProduct109"));
            driver.FindElement(By.CssSelector(".cart-full-header-item.cart-full-remove")).Click();
            Thread.Sleep(3000);
            GoToClient("cart");
            Assert.IsTrue(driver.PageSource.Contains("Ваш заказ не содержит товаров"));
        }

        [Test]
        public void ClientStaticPageOpen()
        {
            GoToClient("pages/about");

            Assert.AreEqual("О магазине", driver.FindElement(By.TagName("h1")).Text);
        }

        [Test]
        public void ClientNewsOpen()
        {
            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("Test News 1 title"));

            Assert.IsTrue(driver.FindElements(By.CssSelector(".news-block-row")).Count == 1);

            driver.FindElement(By.LinkText("Все новости")).Click();
         Thread.Sleep(4000);

            Assert.AreEqual("Новости", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".h3")).Count == 2);

            driver.FindElement(By.LinkText("NewsCategory2")).Click();
         Thread.Sleep(4000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".h3")).Text.Contains("Test News 2 title"));
        }

        [Test]
        public void ClientBrandOpen()
        {
            GoToClient("manufacturers");

            Assert.AreEqual("Бренды", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsFalse(driver.PageSource.Contains("DescripBrand1"));
            Assert.IsTrue(driver.PageSource.Contains("BriefBrand1"));

            driver.FindElement(By.LinkText("BrandName1")).Click();
         Thread.Sleep(4000);

            Assert.AreEqual("BrandName1", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.PageSource.Contains("DescripBrand1"));
            Assert.IsFalse(driver.PageSource.Contains("BriefBrand1"));
        }

        [Test]
        public void ClientSearch()
        {
            GoToClient();

            //check search exist item
            driver.FindElement(By.Name("q")).Click();
            driver.FindElement(By.Name("q")).Clear();
            driver.FindElement(By.Name("q")).SendKeys("TestProduct20");
            driver.FindElement(By.XPath("//span[contains(text(), 'Найти')]")).Click();
         Thread.Sleep(4000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Count == 1);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Text.Contains("TestProduct20"));

            //check search not exist item
            driver.FindElement(By.Name("q")).Click();
            driver.FindElement(By.Name("q")).Clear();
            driver.FindElement(By.Name("q")).SendKeys("TestProduct2000");
            driver.FindElement(By.XPath("//span[contains(text(), 'Найти')]")).Click();
         Thread.Sleep(4000);

            Assert.IsTrue(driver.PageSource.Contains("Найдено 0 по запросу \"TestProduct2000\""));

        }
        
    
        [Test]
        public void ClientFeedbackMessage()
        {
            GoToClient("feedback");

            Assert.AreEqual("Отправить сообщение", driver.FindElement(By.TagName("h1")).Text);

            driver.FindElement(By.LinkText("Вопрос")).Click();
         Thread.Sleep(4000);

            driver.FindElement(By.Id("Message")).Click();
            driver.FindElement(By.Id("Message")).Clear();
            driver.FindElement(By.Id("Message")).SendKeys("Test Message");
            ScrollTo(By.Id("Name"));
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Test Name");
            driver.FindElement(By.Id("Email")).Click();
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("TestEmail@gmail.com");
            driver.FindElement(By.Id("Phone")).Click();
            driver.FindElement(By.Id("Phone")).Clear();
            driver.FindElement(By.Id("Phone")).SendKeys("89345263412");

            ScrollTo(By.CssSelector(".btn.btn-submit.btn-middle"));
            driver.FindElement(By.CssSelector(".btn.btn-submit.btn-middle")).Click();
         Thread.Sleep(4000);

            Assert.IsTrue(driver.PageSource.Contains("Спасибо за сообщение!"));
        }
        
        [Test]
        public void ClientOrder()
        {
            GoToClient("products/test-product41");
            
            ScrollTo(By.CssSelector("[title=\"SizeName1\"]"));
            driver.FindElement(By.CssSelector("[data-product-id=\"41\"]")).Click();
         Thread.Sleep(4000);

            GoToClient("cart");

            driver.FindElement(By.XPath("//a[contains(text(), 'Оформить')]")).Click();
            Thread.Sleep(5000);

            ScrollTo(By.Id("CustomerComment"));
            driver.FindElement(By.Name("checkoutForm")).FindElement(By.CssSelector(".btn.btn-big.btn-submit")).Click();
            Thread.Sleep(5000);
            Assert.IsTrue(driver.Url.Contains("success"));
            Thread.Sleep(1000);
            Assert.AreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text);
        }

        [Test]
        public void ClientOrderOneClick()
        {
            GoToClient("products/test-product42");
            
            ScrollTo(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before"));
            driver.FindElement(By.LinkText("Купить в один клик")).Click();

            driver.FindElement(By.Id("buyOneClickFormName")).Click();
            driver.FindElement(By.Id("buyOneClickFormName")).Clear();
            driver.FindElement(By.Id("buyOneClickFormName")).SendKeys("OneClickName");

            driver.FindElement(By.Id("buyOneClickFormPhone")).Click();
            driver.FindElement(By.Id("buyOneClickFormPhone")).Clear();
            driver.FindElement(By.Id("buyOneClickFormPhone")).SendKeys("5555555555");

            driver.FindElement(By.CssSelector("[value=\"Заказать\"]")).Click();
            Thread.Sleep(3000);

            Assert.IsTrue(driver.PageSource.Contains("Спасибо, ваш заказ оформлен!"));
        }
    }
}
 