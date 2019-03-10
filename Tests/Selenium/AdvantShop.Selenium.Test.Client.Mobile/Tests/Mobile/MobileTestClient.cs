using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Mobile
{
    [TestFixture]
    public class MobileTestClient : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\MobileTest\\Catalog.Category.csv",
              "Data\\MobileTest\\Catalog.Product.csv",
               "Data\\MobileTest\\Catalog.ProductCategories.csv",
                "Data\\MobileTest\\Catalog.Offer.csv"
                );

            Init();
            Functions.AdminMobileOn(driver, baseURL);
        }

        [Test]
        public void Carousel()
        {
            testname = "Carousel";
            VerifyBegin(testname);
            GoToClient("/?forcedMobile=true");
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector(".carousel-main-item.js-carousel-item")).Displayed, "carousel");
            ScrollTo(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next"));
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.carousel-main-next")).Click();
            ScrollTo(By.Id("sidebar_trigger"));
            
            GoToAdmin("settings/mobileversion");
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileShowSlider\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("/?forcedMobile=true");
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".carousel-main-item.js-carousel-item")).Count==0, "no carousel");

            VerifyFinally(testname);
        }

        [Test]
        public void ChangeCity()
        {
            testname = "ChangeCity";
            VerifyBegin(testname);
            GoToClient("/?forcedMobile=true ");
            driver.FindElement(By.Id("sidebar_trigger")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("citySelecorList")).Clear();
            driver.FindElement(By.Id("citySelecorList")).SendKeys("Ульяновск");
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Главная")).Click();
            Thread.Sleep(2000);
            Refresh();
            VerifyIsTrue(driver.FindElement(By.LinkText("Ульяновск")).Displayed, "city");

            GoToAdmin("settings/mobileversion");
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileShowCity\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient();
            Thread.Sleep(2000);
            VerifyIsFalse(driver.PageSource.Contains("Ульяновск"), "no city in page source");
            VerifyFinally(testname);
        }

        [Test]
        public void ChangeView()
        {
            testname = "ChangeView";
            VerifyBegin(testname);
            GoToClient("/categories/first/?forcedMobile=true");
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElements(By.CssSelector(".panel.cs-l-1.cs-br-1.inked.ink-dark.catalog-product-item")).Count == 19, "count item panel");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".prod-cell.cs-br-1")).Count == 0, "no item table");

            driver.FindElement(By.CssSelector(".products-view-variants-item.icon-th-large-before.products-view-variants-tile")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".panel.cs-l-1.cs-br-1.inked.ink-dark.catalog-product-item")).Count == 0, "no item panel");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".prod-cell.cs-br-1")).Count == 19, "count item table");

            driver.FindElement(By.CssSelector(".products-view-variants-item.icon-menu-before.products-view-variants-list")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".panel.cs-l-1.cs-br-1.inked.ink-dark.catalog-product-item")).Count == 19, "count item panel");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".prod-cell.cs-br-1")).Count == 0, "no item table");

            VerifyFinally(testname);
        }
       
        [Test]
        public void FilterProduct()
        {
            testname = "FilterProduct";
            VerifyBegin(testname);

            GoToClient("/?forcedMobile=true");
            IWebElement elem = driver.FindElement(By.Id("ddlStatus"));
            SelectElement select = new SelectElement(elem);
            select.SelectByText("first");

            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.EndsWith("categories/first"), "url");

            //при переходе через выбор в селекте не срабатывает WaitAngular и фильтры не прогружаются
           // GoToClient("categories/first");

            driver.FindElement(By.CssSelector(".cs-l-1.cs-br-1.tab-filter.catalog-filter-block-header")).Click();
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".col-xs-4 input")).GetAttribute("value"), "filter value min");
            VerifyAreEqual("20", driver.FindElements(By.CssSelector(".col-xs-4 input"))[1].GetAttribute("value"), "filter value max");
            driver.FindElement(By.CssSelector(".col-xs-4 input")).Clear();
            driver.FindElement(By.CssSelector(".col-xs-4 input")).SendKeys("10");
            driver.FindElements(By.CssSelector(".col-xs-4 input"))[1].Clear();
            driver.FindElements(By.CssSelector(".col-xs-4 input"))[1].SendKeys("15");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("6", driver.FindElements(By.CssSelector(".prod-text")).Count.ToString(), "count find elem");
            VerifyAreEqual("TestProduct8", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, "find elem 1");
            driver.FindElement(By.CssSelector(".cs-l-1.cs-br-1.tab-filter.catalog-filter-block-header")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("19", driver.FindElements(By.CssSelector(".prod-text")).Count.ToString(), "close filter");

            VerifyFinally(testname);
        }
        [Test]
        public void GoToCategory()
        {
            testname = "GoToCategory";
            VerifyBegin(testname);
            GoToClient("/?forcedMobile=true ");
            IWebElement elem = driver.FindElement(By.Id("ddlStatus"));
            SelectElement select = new SelectElement(elem);
            select.SelectByText("first");
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".panel.cs-l-1.cs-br-1.inked.ink-dark.catalog-product-item")).Count == 19, "count item 1");
            VerifyAreEqual("first pr", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, "name product");
            VerifyFinally(testname);
        }

        [Test]
        public void GoToEmptyCart()
        {
            testname = "GoToEmptyCart";
            VerifyBegin(testname);
            GoToClient("/?forcedMobile=true");
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector(".icon-bag-before")).Displayed, "icon");
            driver.FindElement(By.CssSelector(".icon-bag-before")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Корзина", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text, "cart");
            Thread.Sleep(2000);
            VerifyAreEqual("Ваш заказ не содержит товаров", driver.FindElement(By.CssSelector(".cart-full-empty")).Text, "empty cart");
            driver.FindElement(By.CssSelector(".panel.no-borders.back-link.cs-bg-3.cs-l-1")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("/catalog"), "url");
            VerifyAreEqual("Каталог", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text, "h1");

            VerifyFinally(testname);
        }
        [Test]
        public void GoToProduct()
        {
            testname = "GoToProduct";
            VerifyBegin(testname);
            GoToClient("/?forcedMobile=true");
            IWebElement elem = driver.FindElement(By.Id("ddlStatus"));
            SelectElement select = new SelectElement(elem);
            Thread.Sleep(2000);
            select.SelectByText("first");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector(".prod-name.text-floating")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("first pr", driver.FindElement(By.TagName("h1")).Text, "");
            VerifyAreEqual("first", driver.FindElement(By.CssSelector(".panel.no-borders.back-link.cs-bg-3.cs-l-1")).Text, "");
            driver.FindElement(By.CssSelector(".panel.no-borders.back-link.cs-bg-3.cs-l-1")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("/categories/first"), "");
            //Возможно нужно указывать имя категории
            VerifyAreEqual("Каталог", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text, "");

            VerifyFinally(testname);
        }
        [Test]
        public void MobilePhone()
        {
            testname = "MobilePhone";
            VerifyBegin(testname);

            GoToAdmin("settings/mobileversion");
            driver.FindElement(By.Id("MobilePhone")).Click();
            driver.FindElement(By.Id("MobilePhone")).Clear();
            driver.FindElement(By.Id("MobilePhone")).SendKeys("89022222222");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("/?forcedMobile=true ");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-ng-href=\"tel:89022222222\"]")).Displayed, "icon tel");

            GoToAdmin("settings/mobileversion");
            driver.FindElement(By.Id("MobilePhone")).Click();
            driver.FindElement(By.Id("MobilePhone")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);
            GoToClient("/?forcedMobile=true ");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-ng-href=\"tel:\"]")).Displayed, "no icon");

            VerifyFinally(testname);
        }
        
        [Test]
        public void OpenMobile()
        {
            testname = "OpenMobile";
            VerifyBegin(testname);
            GoToClient("/?forcedMobile=true");
            VerifyAreEqual("Главная", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text, "header");
            VerifyIsTrue(driver.FindElement(By.LinkText("Полная версия сайта")).Displayed, "full version");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".bestsellers-section")).Displayed, "bestsellers");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".novelty-section")).Displayed, "news");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".novelty-section .prod-name")).Count == 3, "count news");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".bestsellers-section .prod-name")).Count == 3, "count bestsellers");
            VerifyAreEqual("TestProduct11", driver.FindElements(By.CssSelector(".bestsellers-section .prod-name"))[0].Text, "bestsellers product name 1");
            VerifyAreEqual("TestProduct13", driver.FindElements(By.CssSelector(".bestsellers-section .prod-name"))[2].Text, "bestsellers product name 3");
            VerifyAreEqual("TestProduct6", driver.FindElements(By.CssSelector(".novelty-section .prod-name"))[0].Text, "news product name 1");
            VerifyAreEqual("TestProduct8", driver.FindElements(By.CssSelector(".novelty-section .prod-name"))[2].Text, "news product name 3");

            ScrollTo(By.CssSelector(".panel-add-link.cs-l-1"));
            driver.FindElement(By.CssSelector(".panel-add-link.cs-l-1")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Хиты продаж", driver.FindElement(By.CssSelector("h1")).Text, "h1 bestsellers");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".prod-name")).Count == 5, "count all bestsellers ");
            VerifyAreEqual("TestProduct11", driver.FindElements(By.CssSelector(".prod-name"))[0].Text, "bestsellers prod");
            //Назад по кнопке
            driver.FindElement(By.CssSelector(".panel-arrow.ar-l.icon-left-open-before.cs-l-1")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".bestsellers-section .prod-name"));
            driver.FindElements(By.CssSelector(".panel-add-link.cs-l-1"))[1].Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Новинки", driver.FindElement(By.CssSelector("h1")).Text, "h1 news");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".prod-name")).Count == 5, "count news");
            VerifyAreEqual("TestProduct6", driver.FindElements(By.CssSelector(".prod-name"))[0].Text, "product news");

            VerifyFinally(testname);
        }

        [Test]
        public void OpenProductPage()
        {
            testname = "OpenProductPage";
            VerifyBegin(testname);
            GoToClient("/products/first-pr/?forcedMobile=true");
            VerifyAreEqual("first pr", driver.FindElement(By.TagName("h1")).Text, "h1 product");
            VerifyAreEqual("449", driver.FindElement(By.CssSelector(".details-param-value.inplace-offset")).Text, "atr num");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".price-number")).Text, "price");
            VerifyAreEqual("руб.", driver.FindElement(By.CssSelector(".price-currency")).Text, "currency");//availability  available
            VerifyAreEqual("Есть в наличии", driver.FindElement(By.CssSelector(".availability.available")).Text, "");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Displayed, "icon");
            VerifyIsTrue(driver.FindElement(By.Id("tabDescription")).Displayed, "tabDescription");
            driver.FindElement(By.Id("tabDescription")).Click();
            Thread.Sleep(2000);//.mobile-tabs-content
            VerifyAreEqual("Description1", driver.FindElement(By.CssSelector(".mobile-tabs-content")).Text, "description");//availability  available

            VerifyFinally(testname);
        }

        [Test]
        public void OpenMobileHeader()
        {
            testname = "OpenMobile";
            VerifyBegin(testname);
            GoToClient("/?forcedMobile=true");
            VerifyAreEqual("Главная", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text, "header");

            GoToAdmin("settings/mobileversion");
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileDisplayHeader\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.Id("HeaderCustomTitle")).Clear();
            driver.FindElement(By.Id("HeaderCustomTitle")).SendKeys("NewHeader");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("/?forcedMobile=true");
            VerifyAreEqual("NewHeader", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text, "new header");
            VerifyFinally(testname);
        }

        [Test]
        public void ProductsOnMainPage()
        {
            testname = "ProductsOnMainPage";
            VerifyBegin(testname);

            GoToAdmin("settings/mobileversion");
            driver.FindElement(By.Id("MainPageProductCountMobile")).Clear();
            driver.FindElement(By.Id("MainPageProductCountMobile")).SendKeys("5");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("/?forcedMobile=true");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".bestsellers-section")).Displayed, "bestsellers");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".novelty-section")).Displayed, "news");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".novelty-section .prod-name")).Count == 5, "count news");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".bestsellers-section .prod-name")).Count == 5, "count bestsellers");

            GoToAdmin("settings/mobileversion");
            driver.FindElement(By.Id("MainPageProductCountMobile")).Clear();
            driver.FindElement(By.Id("MainPageProductCountMobile")).SendKeys("3");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("/?forcedMobile=true");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".bestsellers-section")).Displayed, "bestsellers");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".novelty-section")).Displayed, "news");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".novelty-section .prod-name")).Count == 3, "count news");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".bestsellers-section .prod-name")).Count == 3, "count bestsellers");
            VerifyFinally(testname);
        }

        [Test]
        public void ProductToCart()
        {
            testname = "ProductToCart";
            VerifyBegin(testname);
            DelItemFromCart();
            GoToClient("/products/first-pr/?forcedMobile=true");
            VerifyAreEqual("first pr", driver.FindElement(By.TagName("h1")).Text, "h1");
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(2000);
            Refresh();
            VerifyIsTrue(driver.Url.Contains("/cart"), "url");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".cart-count.cs-br-3")).Text, "count prods");
            VerifyAreEqual("first pr", driver.FindElement(By.CssSelector(".cart-full-mobile-name-link.cs-l-d-1")).Text, "name prod");
            VerifyAreEqual("1 руб.", driver.FindElement(By.CssSelector(".cart-full-mobile-item-cost")).Text, "price ");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).GetAttribute("value"), "count");
            driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).SendKeys("00");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".nav-root")).Click();
            Refresh();
            VerifyAreEqual("100", driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).GetAttribute("value"), "change count");

            VerifyAreEqual("100 руб.", driver.FindElement(By.CssSelector(".cart-full-mobile-result-price")).Text, "rezult price");//cart-full-error panel cs-br-1
            driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).Clear();
            driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).SendKeys("1");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).GetAttribute("value"), "change 1 count");
            driver.FindElement(By.CssSelector(".cart-full-mobile-remove.icon-margin-drop.icon-cancel-circled-before.link-text-decoration-none.cs-l-3")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.PageSource.Contains("Ваш заказ не содержит товаров"), "empty cart");

            VerifyFinally(testname);
        }
        [Test]
        public void ProductToCartOrderFullVersion()
        {
            testname = "ProductToCartOrderFullVersion";
            VerifyBegin(testname);
            Functions.AdminMobileCheckoutOff(driver, baseURL);
            DelItemFromCart();
            GoToClient("/products/test-product20/?forcedMobile=true");
            VerifyAreEqual("TestProduct20", driver.FindElement(By.TagName("h1")).Text, "h1");
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(2000);
            Refresh();
            VerifyIsTrue(driver.Url.Contains("/cart"), "url");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".cart-count.cs-br-3")).Text, "count prods");
            VerifyAreEqual("TestProduct20", driver.FindElement(By.CssSelector(".cart-full-mobile-name-link.cs-l-d-1")).Text, "name prod");
            VerifyAreEqual("22 руб.", driver.FindElement(By.CssSelector(".cart-full-mobile-item-cost")).Text, "coast");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).GetAttribute("value"), "count");
            driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).Clear();
            driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).SendKeys("10");
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".nav-root")).Click();
            Refresh();
            VerifyAreEqual("220 руб.", driver.FindElement(By.CssSelector(".cart-full-mobile-result-price")).Text, "new coast");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".btn.btn-middle.btn-submit.btn-disabled")).Count == 0, "disabled btn");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-error.panel.cs-br-1")).Count == 0, "error");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".btn.btn-middle.btn-submit")).Count > 0, "sucsess");
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000);
            VerifyAreEqual("Оформление заказа", driver.FindElement(By.TagName("h1")).Text, "checkout h1");
            //Add adress
            /*  driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action")).Click();
              Thread.Sleep(2000);
              VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-inner.address-dialog.ng-pristine.ng-valid.ng-valid-required")).Count > 0);
              driver.FindElement(By.CssSelector(".col-xs-9")).SendKeys("Customer1");
              //  (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-9"))[1])).SelectByText("Австрия");
              driver.FindElements(By.CssSelector(".col-xs-9"))[2].SendKeys("Ульяновская область");
              driver.FindElements(By.CssSelector(".col-xs-9"))[3].SendKeys("Ульяновск");
              driver.FindElements(By.CssSelector(".col-xs-9"))[4].SendKeys("NewAdress1");
              driver.FindElements(By.CssSelector(".col-xs-9"))[5].SendKeys("111222");
              driver.FindElement(By.CssSelector(".col-xs-8.col-xs-offset-4")).Click();
              Thread.Sleep(2000);

              //Edit Adress
              driver.FindElement(By.CssSelector(".address-controls-item")).Click();
              Thread.Sleep(2000);
              VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-inner.address-dialog.ng-pristine.ng-valid.ng-valid-required")).Count > 0);
              VerifyAreEqual("Customer1", driver.FindElement(By.CssSelector(".col-xs-9")).GetAttribute("value"));
              VerifyAreEqual("Россия", driver.FindElements(By.CssSelector(".col-xs-9"))[1].Text);
              VerifyAreEqual("Ульяновская область", driver.FindElements(By.CssSelector(".col-xs-9"))[2].GetAttribute("value"));
              VerifyAreEqual("Ульяновск", driver.FindElements(By.CssSelector(".col-xs-9"))[3].GetAttribute("value"));
              VerifyAreEqual("NewAdress1", driver.FindElements(By.CssSelector(".col-xs-9"))[4].GetAttribute("value"));
              VerifyAreEqual("111222", driver.FindElements(By.CssSelector(".col-xs-9"))[5].GetAttribute("value"));
              driver.FindElements(By.CssSelector(".col-xs-8.col-xs-offset-4"))[1].Click();
              Thread.Sleep(2000);
              VerifyAreEqual("Россия", driver.FindElement(By.CssSelector("[data-ng-bind=\"item.Country\"]")).Text);
              VerifyAreEqual("Ульяновская область", driver.FindElement(By.CssSelector("[data-ng-if=\"item.Region\"]")).Text);
              VerifyAreEqual("Ульяновск", driver.FindElement(By.CssSelector("[data-ng-if=\"item.City\"]")).Text);
              VerifyAreEqual("NewAdress1", driver.FindElement(By.CssSelector("[data-ng-if=\"item.Address\"]")).Text);
              //Add adress
              driver.FindElement(By.CssSelector(".btn.btn-middle.btn-action")).Click();
              Thread.Sleep(2000);
              driver.FindElement(By.CssSelector(".col-xs-9")).SendKeys("Customer10");
              (new SelectElement(driver.FindElements(By.CssSelector(".col-xs-9"))[1])).SelectByText("Австрия");
              driver.FindElements(By.CssSelector(".col-xs-9"))[2].SendKeys("Reg1");
              driver.FindElements(By.CssSelector(".col-xs-9"))[3].SendKeys("City1");
              driver.FindElements(By.CssSelector(".col-xs-9"))[4].SendKeys("Adress1");
              driver.FindElements(By.CssSelector(".col-xs-9"))[5].SendKeys("111111");
              driver.FindElement(By.CssSelector(".col-xs-8.col-xs-offset-4")).Click();
              Thread.Sleep(2000);
              VerifyIsTrue(driver.FindElements(By.CssSelector("address-list-item")).Count == 2);
              driver.FindElements(By.CssSelector(".address-controls-item.cs-l-5'"))[1].Click();
              VerifyIsTrue(driver.FindElements(By.CssSelector("address-list-item")).Count == 1);
              */
            //shipping method
            driver.FindElements(By.CssSelector(".shipping-item"))[1].Click();
            driver.FindElements(By.CssSelector(".shipping-item"))[2].Click();
            driver.FindElements(By.CssSelector(".shipping-item"))[3].Click();
            driver.FindElements(By.CssSelector(".shipping-item"))[0].Click();
            //payment method
            driver.FindElements(By.CssSelector(".payment-item"))[1].Click();
            driver.FindElements(By.CssSelector(".payment-item"))[2].Click();
            driver.FindElements(By.CssSelector(".payment-item"))[3].Click();
            driver.FindElements(By.CssSelector(".payment-item"))[0].Click();

            //Comment CustomerComment
            driver.FindElement(By.Id("CustomerComment")).SendKeys("Customer Comment 1");
            Thread.Sleep(2000);
            VerifyAreEqual("220 руб.", driver.FindElement(By.CssSelector(".checkout-result-price")).Text, "checkout price");

            //нет надписи количества зачисенных бонусов
            // VerifyAreEqual(" 6,6  руб.", driver.FindElements(By.CssSelector(".checkout-result-price"))[1].Text);
            driver.FindElement(By.CssSelector(".btn.btn-big.btn-submit")).Click();
            Thread.Sleep(3000);
            VerifyAreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text, "h1 sucsess order");
            GoToAdmin("orders");

            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            // VerifyAreEqual("Customer1", GetGridCell(0, "BuyerName").Text, " Grid orders buyer");
            VerifyAreEqual("220 руб.", GetGridCell(0, "SumFormatted").Text, " Grid orders Sum");
            VerifyIsTrue(GetGridCell(0, "OrderDateFormatted").Text.Contains(System.DateTime.Today.ToString("dd.MM.yyyy")), "grid orders CreationDates");
            //check order
            GetGridCell(0, "Number").Click();
            Thread.Sleep(5000);
            VerifyAreEqual("TestProduct20", GetGridCell(0, "Name", "OrderItems").FindElement(By.TagName("a")).Text, " Grid in order CustomName");
            VerifyAreEqual("22", GetGridCell(0, "Price", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), " Grid in order Price");
            VerifyAreEqual("10", GetGridCell(0, "Amount", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), " Grid in order Amount");
            VerifyAreEqual("220 руб.", GetGridCell(0, "Cost", "OrderItems").Text, " Grid in order Cost");

            IWebElement selectElem1 = driver.FindElement(By.Id("Order_OrderSourceId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Мобильная версия"), "select item source");
            VerifyAreEqual("220 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"OrderSum\"]")).Text, "price rezultt");

            VerifyFinally(testname);
        }
        [Test]
        public void ProductToCartOrderMobileVersion()
        {
            testname = "ProductToCartOrderMobileVersion";
            VerifyBegin(testname);
            Functions.AdminMobileCheckoutOn(driver, baseURL);
            DelItemFromCart();
            GoToClient("/products/test-product20/?forcedMobile=true");
            VerifyAreEqual("TestProduct20", driver.FindElement(By.TagName("h1")).Text, "h1");
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-confirm.icon-bag-before")).Click();
            Thread.Sleep(2000);
            Refresh();
            VerifyIsTrue(driver.Url.Contains("/cart"), "url");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".cart-count.cs-br-3")).Text, "count prods");
            VerifyAreEqual("TestProduct20", driver.FindElement(By.CssSelector(".cart-full-mobile-name-link.cs-l-d-1")).Text, "prod name");
            VerifyAreEqual("22 руб.", driver.FindElement(By.CssSelector(".cart-full-mobile-item-cost")).Text, "coast");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).GetAttribute("value"), "count");
            driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).Clear();
            driver.FindElement(By.CssSelector(".col-xs.spinbox-input-wrap input")).SendKeys("10");
            Thread.Sleep(1000);
            Refresh();
            VerifyAreEqual("220 руб.", driver.FindElement(By.CssSelector(".cart-full-mobile-result-price")).Text, "rezult price");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".btn.btn-middle.btn-submit.btn-disabled")).Count == 0, "disabled btn");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-error.panel.cs-br-1")).Count == 0, "error panel");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".btn.btn-middle.btn-submit")).Count > 0, "sucess btn");
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Оформление заказа", driver.FindElement(By.TagName("h1")).Text, "checkout rezult");
            driver.FindElement(By.Name("Name")).SendKeys("Customer1");
            driver.FindElement(By.Name("Phone")).SendKeys("9999999999");
            driver.FindElement(By.Name("Message")).SendKeys("new test order");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-confirm.btn-big")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector(".btn.btn-confirm.btn-small"));
            WaitForElem(By.CssSelector(".checkout-confirm-txt"));
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-confirm-txt")).Text.Contains("Спасибо, ваш заказ оформлен."), "checkout sucess");

            GoToAdmin("orders");

            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            VerifyAreEqual("Customer1", GetGridCell(0, "BuyerName").Text, " Grid orders buyer");
            VerifyAreEqual("220 руб.", GetGridCell(0, "SumFormatted").Text, " Grid orders Sum");
            VerifyIsTrue(GetGridCell(0, "OrderDateFormatted").Text.Contains(System.DateTime.Today.ToString("dd.MM.yyyy")), "grid orders CreationDates");
            //check order
            GetGridCell(0, "Number").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestProduct20", GetGridCell(0, "Name", "OrderItems").FindElement(By.TagName("a")).Text, " Grid in order CustomName");
            VerifyAreEqual("22", GetGridCell(0, "Price", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), " Grid in order Price");
            VerifyAreEqual("10", GetGridCell(0, "Amount", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), " Grid in order Amount");
            VerifyAreEqual("220 руб.", GetGridCell(0, "Cost", "OrderItems").Text, " Grid in order Cost");

            IWebElement selectElem1 = driver.FindElement(By.Id("Order_OrderSourceId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Мобильная версия"), "select item source");
            VerifyAreEqual("220 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"OrderSum\"]")).Text, "price rezultt");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchProduct()
        {
            testname = "SearchProduct";
            VerifyBegin(testname);
            GoToAdmin("settings/mobileversion");
            if (!driver.FindElement(By.Id("DisplayHeaderTitle")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileDisplayHeader\"]")).FindElement(By.TagName("span")).Click();
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
                Thread.Sleep(2000);
            }
            GoToClient("/?forcedMobile=true ");
            driver.FindElement(By.CssSelector(".catalog-search-placeholder input")).SendKeys("first pr");
            driver.FindElement(By.CssSelector(".catalog-search-btn.cs-bg-1")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Поиск", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text);
            VerifyAreEqual("Найдено 1 по запросу \"first pr\"", driver.FindElement(By.CssSelector(".catalog-title.page-title.h2")).Text, " h1 exist");
            VerifyAreEqual("first pr", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, " exist");
            VerifyIsFalse(driver.PageSource.Contains("К сожалению, по вашему запросу ничего не найдено"), "no exist");

            //По кнопке
            driver.FindElement(By.CssSelector(".searchBtn.inked.ink-light")).Click();
            driver.FindElement(By.CssSelector(".js-click-out.search-panel.cs-bg-13 input")).SendKeys("TestProduct1000");
            driver.FindElement(By.CssSelector(".search-disable-i")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Поиск", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text);
            VerifyIsTrue(driver.PageSource.Contains("К сожалению, по вашему запросу ничего не найдено"), "no exist by button");

            VerifyFinally(testname);
        }

        [Test]
        public void SortProduct()
        {
            testname = "SortProduct";
            VerifyBegin(testname);
            GoToClient("/categories/first/?forcedMobile=true ");
            Thread.Sleep(2000);

            VerifyAreEqual("nosorting", driver.FindElement(By.TagName("select")).GetAttribute("value"), "select nosorting");
            VerifyAreEqual("first pr", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, "value nosorting");
            (new SelectElement(driver.FindElement(By.TagName("select")))).SelectByText("Новизне");
            Thread.Sleep(2000);
            VerifyAreEqual("descbyaddingdate", driver.FindElement(By.TagName("select")).GetAttribute("value"), "select addingdate");
            VerifyAreEqual("TestProduct17", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, "addingdate value");

            (new SelectElement(driver.FindElement(By.TagName("select")))).SelectByText("Названию, по возрастанию");
            Thread.Sleep(2000);
            VerifyAreEqual("ascbyname", driver.FindElement(By.TagName("select")).GetAttribute("value"), "select ascbyname");
            VerifyAreEqual("first pr", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, "ascbyname value1");
            VerifyAreEqual("TestProduct16", driver.FindElements(By.CssSelector(".prod-name.text-floating"))[9].Text, "ascbyname value 2");

            (new SelectElement(driver.FindElement(By.TagName("select")))).SelectByText("Названию, по убыванию");
            Thread.Sleep(2000);
            VerifyAreEqual("descbyname", driver.FindElement(By.TagName("select")).GetAttribute("value"), "select descbyname");
            VerifyAreEqual("TestProduct9", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, "descbyname 1");
            VerifyAreEqual("TestProduct8", driver.FindElements(By.CssSelector(".prod-name.text-floating"))[1].Text, "descbyname 2");
            VerifyAreEqual("TestProduct16", driver.FindElements(By.CssSelector(".prod-name.text-floating"))[9].Text, "descbyname 10");

            (new SelectElement(driver.FindElement(By.TagName("select")))).SelectByText("Цене, по возрастанию");
            Thread.Sleep(2000);
            VerifyAreEqual("ascbyprice", driver.FindElement(By.TagName("select")).GetAttribute("value"), "select ascbyprice");
            VerifyAreEqual("first pr", driver.FindElement(By.CssSelector(".prod-name.text-floating")).Text, "ascbyprice 1");
            VerifyAreEqual("TestProduct8", driver.FindElements(By.CssSelector(".prod-name.text-floating"))[9].Text, "ascbyprice 10");
            VerifyFinally(testname);
        }


        protected void DelItemFromCart()
        {
            GoToClient("/cart/?forcedMobile=true");
            if (driver.FindElements(By.CssSelector(".cart-full-mobile-remove")).Count > 0)
                driver.FindElement(By.CssSelector(".cart-full-mobile-remove")).Click();
        }

    }
}

