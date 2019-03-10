using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.ProductsOnMainClient
{
    [TestFixture]
    public class ProductOnMainClientTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductOnMainClient\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductOnMainClient\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductOnMainClient\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductOnMainClient\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductOnMainClient\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductOnMainClient\\Catalog.Product_ProductList.csv"
           );

            Init();
        }

        [Test]
        public void BestSellerClient()
        {
            testname = "BestSellerClient";
            VerifyBegin(testname);

            GoToAdmin("mainpageproducts?type=best");

            //check admin
            VerifyIsTrue(driver.FindElements(By.CssSelector(".aside-menu-count-inner"))[2].Text.Contains("4/5"), "count list products admin");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Хиты продаж"), "h1 page admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct4"), "product 1 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct5"), "product 2 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct6"), "product 3 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct7"), "product 4 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct8"), "product 5 in list admin");

            VerifyAreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all in grid");

            //check client
            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-best.cs-br-1")).Text.Contains("Хиты продаж"), "list h2 client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-best.cs-br-1")).Text.Contains("TestProduct7"), "product 1 in list client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-best.cs-br-1")).Text.Contains("TestProduct8"), "product 2 in list client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-best.cs-br-1")).Text.Contains("TestProduct4"), "product 3 in list client main");

            GoToClient("productlist/best");

            VerifyAreEqual("Хиты продаж", driver.FindElement(By.TagName("h1")).Text, "h1 page client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result")).Text.Contains("Всего найдено: 4"), "count list products client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct4"), "product 1 in list client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct5"), "product 2 in list client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct7"), "product 3 in list client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct8"), "product 4 in list client");
            VerifyIsFalse(driver.PageSource.Contains("TestProduct6"), "product 5 disabled in list client");

            VerifyFinally(testname);
        }

        [Test]
        public void NewClient()
        {
            testname = "NewClient";
            VerifyBegin(testname);

            GoToAdmin("mainpageproducts?type=new");

            //check admin
            VerifyIsTrue(driver.FindElements(By.CssSelector(".aside-menu-count-inner"))[3].Text.Contains("3/4"), "count list products admin");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новинки"), "h1 page admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct1"), "product 1 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct2"), "product 2 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct3"), "product 3 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct4"), "product 4 in list admin");

            VerifyAreEqual("Найдено записей: 4", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all in grid");

            //check client
            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-new.cs-br-1")).Text.Contains("Новинки"), "list h2 client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-new.cs-br-1")).Text.Contains("TestProduct3"), "product 1 in list client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-new.cs-br-1")).Text.Contains("TestProduct4"), "product 2 in list client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-new.cs-br-1")).Text.Contains("TestProduct2"), "product 3 in list client main");

            GoToClient("productlist/new");

            VerifyAreEqual("Новинки", driver.FindElement(By.TagName("h1")).Text, "h1 page client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result")).Text.Contains("Всего найдено: 3"), "count list products client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct2"), "product 1 in list client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct3"), "product 2 in list client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct4"), "product 3 in list client");
            VerifyIsFalse(driver.PageSource.Contains("TestProduct1"), "product 5 disabled in list client");

            VerifyFinally(testname);
        }

        [Test]
        public void SaleClient()
        {
            testname = "SaleClient";
            VerifyBegin(testname);

            GoToAdmin("mainpageproducts?type=sale");

            //check admin
            VerifyIsTrue(driver.FindElements(By.CssSelector(".aside-menu-count-inner"))[4].Text.Contains("2/2"), "count list products admin");

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Товары со скидкой"), "h1 page admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct13"), "product 1 in list admin");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-contents-wrapper")).Text.Contains("TestProduct14"), "product 2 in list admin");

            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all in grid");

            //check client
            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-discount.cs-br-1")).Text.Contains("Скидка!"), "list h2 client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-discount.cs-br-1")).Text.Contains("TestProduct13"), "product 1 in list client main");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-discount.cs-br-1")).Text.Contains("TestProduct14"), "product 2 in list client main");

            GoToClient("productlist/sale");

            VerifyAreEqual("Товары со скидкой", driver.FindElement(By.TagName("h1")).Text, "h1 page client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".products-view-sort-result")).Text.Contains("Всего найдено: 2"), "count list products client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct13"), "product 1 in list client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct14"), "product 2 in list client");

            VerifyFinally(testname);
        }

        [Test]
        public void BestSellerClientAddDescription()
        {
            testname = "BestSellerClientAddDescription";
            VerifyBegin(testname);

            GoToAdmin("mainpageproducts?type=best");

            driver.FindElement(By.CssSelector("[data-e2e=\"editMainPageList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            VerifyIsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Редактирование списка"), "pop up header");

            SetCkText("Description Added Best Sellers", "editor1");
            XPathContainsText("button", "Сохранить");
            
            //check admin
            GoToAdmin("mainpageproducts?type=best");

            driver.FindElement(By.CssSelector("[data-e2e=\"editMainPageList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            AssertCkText("Description Added Best Sellers", "editor1");

            //check client
            GoToClient();
            VerifyIsFalse(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-best.cs-br-1")).Text.Contains("Description Added Best Sellers"), "list's description added client no on main");
            
            GoToClient("productlist/best");

            VerifyIsTrue(driver.PageSource.Contains("Description Added Best Sellers"), "list's description added client");

            VerifyFinally(testname);
        }

        [Test]
        public void NewClientAddDescription()
        {
            testname = "NewClientAddDescription";
            VerifyBegin(testname);

            GoToAdmin("mainpageproducts?type=new");

            driver.FindElement(By.CssSelector("[data-e2e=\"editMainPageList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            VerifyIsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Редактирование списка"), "pop up header");

            SetCkText("Description Added New Sellers", "editor1");
            XPathContainsText("button", "Сохранить");

            //check admin
            GoToAdmin("mainpageproducts?type=new");

            driver.FindElement(By.CssSelector("[data-e2e=\"editMainPageList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            AssertCkText("Description Added New Sellers", "editor1");

            //check client
            GoToClient();
            VerifyIsFalse(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-new.cs-br-1")).Text.Contains("Description Added New Sellers"), "list's description added client no on main");

            GoToClient("productlist/new");

            VerifyIsTrue(driver.PageSource.Contains("Description Added New Sellers"), "list's description added client");

            VerifyFinally(testname);
        }

        [Test]
        public void SaleClientAddDescription()
        {
            testname = "SaleClientAddDescription";
            VerifyBegin(testname);

            GoToAdmin("mainpageproducts?type=sale");

            driver.FindElement(By.CssSelector("[data-e2e=\"editMainPageList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            VerifyIsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Редактирование списка"), "pop up header");

            SetCkText("Discount description", "editor1");
            XPathContainsText("button", "Сохранить");

            //check admin
            GoToAdmin("mainpageproducts?type=sale");

            driver.FindElement(By.CssSelector("[data-e2e=\"editMainPageList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            AssertCkText("Discount description", "editor1");

            //check client
            GoToClient();
            VerifyIsFalse(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-discount.cs-br-1")).Text.Contains("Discount description"), "list's description added client no on main");

            GoToClient("productlist/sale");

            VerifyIsTrue(driver.PageSource.Contains("Discount description"), "list's description added client");

            VerifyFinally(testname);
        }

        [Test]
        public void ProdListClientEditDescription()
        {
            testname = "ProdListClientEditDescription";
            VerifyBegin(testname);

            //pre check client
            GoToClient("productlist/list/1");

            VerifyIsTrue(driver.PageSource.Contains("Description1"), "list's description pre check client");
            
            GoToAdmin("productlists/products/1");

            driver.FindElement(By.CssSelector("[data-e2e=\"editProductList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            VerifyIsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Редактирование списка"), "pop up header");

            AssertCkText("Description1", "editor1");
            SetCkText("edited text test", "editor1");
            XPathContainsText("button", "Сохранить");

            //check admin
            GoToAdmin("productlists/products/1");

            driver.FindElement(By.CssSelector("[data-e2e=\"editProductList\"]")).Click();
            WaitForElem(By.Id("cke_editor1"));

            AssertCkText("edited text test", "editor1");

            //check client
            GoToClient();
            VerifyIsFalse(driver.FindElement(By.CssSelector(".products-specials-block.products-specials-list.cs-br-1")).Text.Contains("edited text test"), "list's description added client no on main");

            GoToClient("productlist/list/1");

            VerifyIsTrue(driver.PageSource.Contains("edited text test"), "list's description added client");

            VerifyFinally(testname);
        }
    }
}