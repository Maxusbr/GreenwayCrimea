using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsAddProductTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
             "data\\Admin\\CMS\\News\\Settings.News.csv",
             "data\\Admin\\CMS\\News\\Settings.NewsCategory.csv",
             "data\\Admin\\CMS\\News\\Catalog.Product.csv",
            "data\\Admin\\CMS\\News\\Catalog.Offer.csv",
            "data\\Admin\\CMS\\News\\Catalog.Category.csv",
            "data\\Admin\\CMS\\News\\Catalog.ProductCategories.csv"
           );

            Init();
        }

        [Test]
        public void NewsAddProduct()
        {
            testname = "CMSNewsAddProduct";
            VerifyBegin(testname);

            GoToAdmin("news/edit/1");

            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            XPathContainsText("span", "TestCategory1");
            GetGridCell(0, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            XPathContainsText("button", "Выбрать");

            //check admin
            GoToAdmin("news/edit/1");
            VerifyIsTrue(driver.FindElement(By.TagName("news-products")).Text.Contains("TestProduct1"), "product added to news admin");

            //check client
            GoToClient("news/test_news_1");
            VerifyIsTrue(driver.PageSource.Contains("Товары к этой новости"), "product added to news client h1");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct1"), "product added to news client");

            //check delete product from news
            GoToAdmin("news/edit/1");

            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.TagName("news-products")).FindElement(By.CssSelector(".link-invert.link-decoration-none.link-grey.fa.fa-remove.categories-block-icon")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            
            //check admin delete
            GoToAdmin("news/edit/1");
            VerifyIsFalse(driver.FindElement(By.TagName("news-products")).Text.Contains("TestProduct1"), "product added to news admin delete");

            //check client delete
            GoToClient("news/test_news_1");
            VerifyIsFalse(driver.PageSource.Contains("Товары к этой новости"), "product added to news client h1 delete");
            VerifyIsFalse(driver.PageSource.Contains("TestProduct1"), "product added to news client delete");

            VerifyFinally(testname);
        }

        [Test]
        public void NewsAddProductSelect()
        {
            testname = "CMSNewsAddProductSelect";
            VerifyBegin(testname);

            GoToAdmin("news/edit/2");

            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            XPathContainsText("span", "TestCategory1");
            GetGridCell(1, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            XPathContainsText("button", "Выбрать");

            //check admin
            GoToAdmin("news/edit/2");
            VerifyIsTrue(driver.FindElement(By.TagName("news-products")).Text.Contains("TestProduct2"), "product 1 added to news admin");
            VerifyIsTrue(driver.FindElement(By.TagName("news-products")).Text.Contains("TestProduct3"), "product 2 added to news admin");

            //check client
            GoToClient("news/test_news_2");
            VerifyIsTrue(driver.PageSource.Contains("Товары к этой новости"), "product added to news client h1");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct2"), "product 1 added to news client");
            VerifyIsTrue(driver.PageSource.Contains("TestProduct3"), "product 2 added to news client");

            VerifyFinally(testname);
        }

        [Test]
        public void NewsAddProductSelectAllOnPage()
        {
            testname = "CMSNewsAddProductSelectAllOnPage";
            VerifyBegin(testname);

            GoToAdmin("news/edit/3");

            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            XPathContainsText("span", "TestCategory1");
            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            XPathContainsText("button", "Выбрать");

            //check admin
            GoToAdmin("news/edit/3");
            VerifyIsTrue(driver.FindElement(By.TagName("news-products")).FindElements(By.CssSelector(".item-block")).Count == 10, "selected all on page products added to news admin");

            //check client
            GoToClient("news/test_news_3");
            VerifyIsTrue(driver.PageSource.Contains("Товары к этой новости"), "product added to news client h1");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 10, "selected all on page products added to news client");

            VerifyFinally(testname);
        }

        [Test]
        public void NewsAddProductSelectAll()
        {
            testname = "CMSNewsAddProductSelectAll";
            VerifyBegin(testname);

            GoToAdmin("news/edit/4");

            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-body"));
            XPathContainsText("span", "TestCategory1");
            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            XPathContainsText("button", "Выбрать");

            //check admin
            GoToAdmin("news/edit/4");
            VerifyIsTrue(driver.FindElement(By.TagName("news-products")).FindElements(By.CssSelector(".item-block")).Count == 11, "selected all products added to news admin");

            //check product link
            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.TagName("news-products")).FindElement(By.LinkText("5 - TestProduct5")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Товар \"TestProduct5\""), "product link from news h1");
            VerifyIsTrue(driver.Url.Contains("product/edit"), "product link from news url");

            //check client
            GoToClient("news/test_news_4");
            VerifyIsTrue(driver.PageSource.Contains("Товары к этой новости"), "product added to news client h1");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 11, "selected all products added to news client");

            VerifyFinally(testname);
        }
    }
}
