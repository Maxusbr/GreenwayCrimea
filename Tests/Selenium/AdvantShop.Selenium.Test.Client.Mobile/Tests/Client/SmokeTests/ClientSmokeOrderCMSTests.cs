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
            Thread.Sleep(1000);

            GoToClient("cart");

            Assert.AreEqual("Корзина", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.PageSource.Contains("TestProduct106"));
            Assert.AreEqual("2", driver.FindElement(By.CssSelector(".cart-full-amount-control input")).GetAttribute("value"));
            driver.FindElement(By.CssSelector(".cart-full-amount-control input")).Click();
            driver.FindElement(By.CssSelector(".cart-full-amount-control input")).Clear();
            driver.FindElement(By.CssSelector(".cart-full-amount-control input")).SendKeys("3");
            DropFocusH1();
            Assert.IsTrue(driver.FindElement(By.CssSelector(".cart-full-result-price")).Text.Contains("318"));
        }

        [Test]
        public void ClientProductDeleteFromCart()
        {
            GoToClient("products/test-product107");
            
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(1000);

            GoToClient("cart");
            
            Assert.IsTrue(driver.PageSource.Contains("TestProduct107"));
            driver.FindElement(By.CssSelector(".cart-full-body-item.cart-full-remove")).Click();
            Thread.Sleep(1000);
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

            Assert.AreEqual("Новости", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".h3")).Count == 2);

            driver.FindElement(By.LinkText("NewsCategory2")).Click();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".h3")).Text.Contains("Test News 2 title"));
        }

        [Test]
        public void ClientBrandOpen()
        {
            GoToClient("manufacturers");

            Assert.IsTrue(driver.PageSource.Contains("BrandName1"));

            driver.FindElement(By.LinkText("BrandName1")).Click();
            
            Assert.AreEqual("BrandName1", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.PageSource.Contains("DescripBrand1"));
        }

        [Test]
        public void ClientSearch()
        {
          //  Functions.RecalculateProducts(driver, baseURL);
           // Functions.RecalculateSearch(driver, baseURL);

            GoToClient();

            //check search exist item
            driver.FindElement(By.Name("q")).Click();
            driver.FindElement(By.Name("q")).Clear();
            driver.FindElement(By.Name("q")).SendKeys("TestProduct20");
            driver.FindElement(By.XPath("//span[contains(text(), 'Найти')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Count == 1);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-item.text-static.cs-br-1.js-products-view-item")).Text.Contains("TestProduct20"));

            //check search not exist item
            driver.FindElement(By.Name("q")).Click();
            driver.FindElement(By.Name("q")).Clear();
            driver.FindElement(By.Name("q")).SendKeys("TestProduct2000");
            driver.FindElement(By.XPath("//span[contains(text(), 'Найти')]")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Найдено 0 по запросу \"TestProduct2000\""));

        }
        
    
        [Test]
        public void ClientFeedbackMessage()
        {
            GoToClient();

            driver.FindElement(By.LinkText("Отправить сообщение")).Click();

            Assert.AreEqual("Отправить сообщение", driver.FindElement(By.TagName("h1")).Text);

            driver.FindElement(By.LinkText("Вопрос")).Click();

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

            Assert.IsTrue(driver.PageSource.Contains("Спасибо за сообщение!"));
        }
        
        [Test]
        public void ClientOrder()
        {
            GoToClient("products/test-product41");

            Refresh();

            ScrollTo(By.CssSelector("[title=\"SizeName1\"]"));
            driver.FindElement(By.CssSelector("[data-product-id=\"41\"]")).Click();

            GoToClient("cart");

            driver.FindElement(By.XPath("//a[contains(text(), 'Оформить')]")).Click();
            Thread.Sleep(3000);

            ScrollTo(By.Id("CustomerComment"));
            driver.FindElement(By.CssSelector("[value=\"Подтвердить заказ\"]")).Click();
            Thread.Sleep(3000);

            Assert.IsTrue(driver.PageSource.Contains("Спасибо, ваш заказ оформлен!"));
        }

        [Test]
        public void ClientOrderOneClick()
        {
            GoToClient("products/test-product42");

            Refresh();

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
 