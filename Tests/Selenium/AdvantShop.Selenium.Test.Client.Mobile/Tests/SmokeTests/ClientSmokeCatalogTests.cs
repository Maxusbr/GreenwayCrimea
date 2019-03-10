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
    public class ClientSmokeCatalogTests : BaseSeleniumTest
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
        public void ClientCategoryOpen()
        {
            GoToClient();

            Assert.IsTrue(driver.PageSource.Contains("TestCategory1"));
            Assert.IsTrue(driver.PageSource.Contains("TestCategory2"));

            driver.FindElement(By.LinkText("TestCategory1")).Click();
            Thread.Sleep(4000);

            //check product in category
            Assert.AreEqual("TestCategory1", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.PageSource.Contains("TestProduct1"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"1\"]")).FindElement(By.CssSelector(".products-view-picture-link img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"1\"]")).FindElement(By.CssSelector(".products-view-price")).Text.Contains("1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"1\"]")).FindElement(By.CssSelector(".products-view-buttons a")).Enabled);

            GoToClient();

            driver.FindElement(By.LinkText("TestCategory6")).Click();
            Thread.Sleep(4000);

            Assert.AreEqual("TestCategory6", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("TestCategory7", driver.FindElement(By.CssSelector(".product-categories-header-thin.h2")).Text);
            Assert.IsTrue(driver.PageSource.Contains("TestProduct101"));
        }

        [Test]
        public void ClientCategoryView()
        {
            GoToClient();

            driver.FindElement(By.LinkText("TestCategory1")).Click();
            Thread.Sleep(4000);

            //check view tile
            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.products-view.products-view-tile")).Displayed);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".row.products-view.products-view-list")).Count);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".row.products-view.products-view-table")).Count);

            //check view list
            driver.FindElement(By.CssSelector("[title=\"Список\"]")).Click();
            Thread.Sleep(4000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.products-view.products-view-list")).Displayed);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".row.products-view.products-view-tile")).Count);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".row.products-view.products-view-table")).Count);

            //check view table
            driver.FindElement(By.CssSelector("[title=\"Таблица\"]")).Click();
            Thread.Sleep(4000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.products-view.products-view-table")).Displayed);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".row.products-view.products-view-tile")).Count);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".row.products-view.products-view-list")).Count);
        }


        [Test]
        public void ClientProductOpen()
        {
            Functions.AdminSettingsProductCart(driver, baseURL);

            GoToClient("categories/test-category1");

            ScrollTo(By.CssSelector("[data-product-id=\"1\"]"));
            driver.FindElement(By.LinkText("TestProduct1")).Click();
            Thread.Sleep(4000);

            Assert.IsTrue(driver.Url.Contains("test-product1"));
            Assert.AreEqual("TestProduct1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Скидка 50%", driver.FindElement(By.CssSelector(".products-view-label-inner.products-view-label-discount")).Text);
            Assert.IsFalse(driver.FindElement(By.CssSelector(".gallery-picture.text-static img")).GetAttribute("src").Contains("nophoto"));

            //check brief
            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-meta-list")).Text.Contains("BrandName1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-sku")).FindElement(By.CssSelector(".details-param-value.inplace-offset")).Text.Contains("1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-dimensions")).FindElement(By.CssSelector(".details-param-value")).Text.Contains("1 x 1 x 1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-weight")).FindElement(By.CssSelector(".details-param-value")).Text.Contains("1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-weight")).FindElement(By.CssSelector(".details-param-name")).Text.Contains("Вес:"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-unit")).FindElement(By.CssSelector(".details-param-value")).Text.Contains("unit"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-unit")).FindElement(By.CssSelector(".details-param-name")).Text.Contains("Ед. измерения:"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-briefproperties")).FindElement(By.CssSelector(".details-param-value")).Text.Contains("PropertyValue1"));

            //check cart
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-payment.cs-br-1")).Text.Contains("1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-payment.cs-br-1")).Text.Contains("1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-payment.cs-br-1")).FindElement(By.CssSelector(".details-payment-cell a")).Enabled);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".color-viewer-inner.cs-br-1")).Displayed);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".sizes-viewer-inner.center-aligner.cs-l-1")).Displayed);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".sizes-viewer-inner.center-aligner.cs-l-1")).Text.Contains("SizeName1"));
            Assert.IsTrue(driver.PageSource.Contains("Desc1"));
        }

        [Test]
        public void ClientProductColorAmount()
        {
            GoToAdmin("product/edit/4");
            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(text(), 'Добавить цену')]")).Click();
            Thread.Sleep(3000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddSize\"]")))).SelectByText("SizeName1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"AddColor\"]")))).SelectByText("Color1");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddPrice\"]")).SendKeys("44");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"AddAmount\"]")).SendKeys("44");
            
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(4000);

            GoToClient("products/test-product4");
            
            //check client product cart
            Assert.IsFalse(driver.FindElement(By.CssSelector("[title=\"Color1\"]")).GetAttribute("class").Contains("selected"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[title=\"Color4\"]")).GetAttribute("class").Contains("selected"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[title=\"SizeName1\"] input")).Enabled);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".price")).Text.Contains("2"));

            ScrollTo(By.CssSelector("[title=\"Color1\"]"));
            driver.FindElement(By.CssSelector("[title=\"Color1\"]")).Click();
            Thread.Sleep(4000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[title=\"Color1\"]")).GetAttribute("class").Contains("selected"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[title=\"Color4\"]")).GetAttribute("class").Contains("selected"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[title=\"SizeName4\"] input")).Enabled);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".price")).Text.Contains("22"));
        }
        
        [Test]
        public void ClientProductsOnMainPage()
        {
            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-best.cs-br-1")).FindElement(By.LinkText("TestProduct7")).Displayed);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-new.cs-br-1")).FindElement(By.LinkText("TestProduct4")).Displayed);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-discount.cs-br-1")).FindElement(By.LinkText("TestProduct5")).Displayed);
        }

      
        [Test]
        public void ClientCategorySort()
        {
            GoToClient("categories/test-category4");

            //check sort by name
            driver.FindElement(By.XPath("//span[contains(text(), 'По названию')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct61"));

            driver.FindElement(By.XPath("//span[contains(text(), 'По названию')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct80"));
            
            //check sort by price
            driver.FindElement(By.XPath("//span[contains(text(), 'По цене')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct80"));
            
            driver.FindElement(By.XPath("//span[contains(text(), 'По цене')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct61"));
            
            //check sort by new
            driver.FindElement(By.XPath("//span[contains(text(), 'По новизне')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct80"));

            driver.FindElement(By.XPath("//span[contains(text(), 'По новизне')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct61"));

            //check sort by rating
            driver.FindElement(By.XPath("//span[contains(text(), 'По рейтингу')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct70"));

            driver.FindElement(By.XPath("//span[contains(text(), 'По рейтингу')]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block"))[0].FindElement(By.CssSelector(".products-view-info")).FindElement(By.TagName("a")).Text.Contains("TestProduct61"));

        }

        [Test]
        public void ClientCategoryFilterPrice()
        {
            GoToClient("categories/test-category3");
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("20"));

            ScrollTo(By.Name("catalogFilterForm"));
            IWebElement elem = driver.FindElement(By.CssSelector(".ngrs-handle.ngrs-handle-min"));
            Actions move = new Actions(driver);
            move.DragAndDropToOffset(elem, 140, 0).Perform();
            Thread.Sleep(3000);
           // WaitForAjax();
            //Assert.IsTrue(driver.FindElement(By.CssSelector("[data-ng-bind='catalogFilter.foundCount']")).Text.Contains("10"));
            driver.FindElement(By.CssSelector("[data-ng-click='catalogFilter.submit()']")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("10"));
            Assert.IsTrue(driver.Url.Contains("pricefrom"));
            Assert.IsTrue(driver.Url.Contains("priceto"));
        }

        [Test]
        public void ClientCategoryFilterBrand()
        {
            GoToClient("categories/test-category3");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).GetAttribute("innerText").Contains("20"));

            ScrollToElements(By.CssSelector(".catalog-filter-block"), 1);
            driver.FindElements(By.CssSelector(".catalog-filter-block"))[1].FindElements(By.CssSelector(".catalog-filter-row span"))[0].Click();
            Thread.Sleep(1000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-ng-bind='catalogFilter.foundCount']")).GetAttribute("innerText").Contains("1"));
            MouseFocus(driver, By.CssSelector(".catalog-filter-popover-text span"));
            Assert.AreEqual("1", driver.FindElement(By.CssSelector(".catalog-filter-popover-text span")).GetAttribute("innerText"));

            Assert.IsTrue(driver.FindElements(By.CssSelector(".adv-popover-content"))[0].FindElement(By.CssSelector(".catalog-filter-popover-text")).FindElement(By.TagName("span")).GetAttribute("innerText").Contains("1"));
        }

        [Test]
        public void ClientCategoryFilterSize()
        {
            GoToClient("categories/test-category3");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("20"));
            
            ScrollTo(By.CssSelector("[title=\"Color1\"]"));
            driver.FindElements(By.CssSelector(".catalog-filter-block"))[3].FindElements(By.CssSelector(".catalog-filter-row span"))[0].Click();
            Thread.Sleep(3000);
            //Assert.IsTrue(driver.FindElement(By.CssSelector(".catalog-filter-popover-text span")).Text.Contains("1"));
            driver.FindElement(By.CssSelector("[data-ng-click='catalogFilter.submit()']")).Click();
            Thread.Sleep(4000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("1"));
        }

        [Test]
        public void ClientCategoryFilterColor()
        {
            GoToClient("categories/test-category3");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("20"));

            ScrollTo(By.CssSelector("[title=\"Color1\"]"));
            driver.FindElement(By.CssSelector("[title=\"Color1\"]")).Click();
            driver.FindElement(By.CssSelector("[title=\"Color2\"]")).Click();
            //Assert.IsTrue(driver.FindElement(By.CssSelector(".catalog-filter-popover-text span")).Text.Contains("2"));
            driver.FindElement(By.CssSelector("[data-ng-click='catalogFilter.submit()']")).Click();
            Thread.Sleep(4000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text.Contains("2"));
        }
        
    }
}
