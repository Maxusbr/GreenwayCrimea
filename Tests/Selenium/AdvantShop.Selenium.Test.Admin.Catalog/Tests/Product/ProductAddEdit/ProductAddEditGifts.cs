using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Gifts
{
    [TestFixture]
    public class ProductAddEditGifts : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Category.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Brand.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Property.csv",
                  "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.PropertyValue.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.ProductPropertyValue.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Product.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Offer.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.ProductCategories.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.PropertyGroup.csv"
                );
            Init();
        }

        protected void AddGiftsProduct(string name = null)
        {
            driver.FindElements(By.CssSelector(".header-subtext a"))[0].Click();
            Thread.Sleep(2000);
            GetGridFilter().SendKeys(name);
            DropFocus("h2");
            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
        }
        [Test]
        public void AddGiftBySearch()
        {
            GoToAdmin("product/edit/2#?tabsInProduct=3");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            AddGiftsProduct("11");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct11"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 1);

            AddGiftsProduct("13");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct13"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 2);

            AddGiftsProduct("14");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct14"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 3);

            AddGiftsProduct("15");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct15"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product2");

            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 4);

            MouseFocus(driver, By.CssSelector(".product-gift-image"));
            Assert.AreEqual("TestProduct11", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text);
            Assert.AreEqual("TestProduct13", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[1].Text);
            Assert.AreEqual("TestProduct14", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[2].Text);
            Assert.AreEqual("TestProduct15", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[3].Text);

         /*   GoToAdmin("product/edit/2#?tabsInProduct=3");
            Functions.DelElement(driver);*/
        }
        [Test]
        public void AddGiftByPage()
        {
            GoToAdmin("product/edit/3#?tabsInProduct=3");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e-grid-cell=\"gridOffersSelectvizr[2][\'Name\']\"]"));
            GetGridCell(3, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(4, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(5, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(6, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct21"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct22"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct24"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product3");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 4);

            //  driver.FindElement(By.CssSelector(".product-gift-image")).Click();
            // Thread.Sleep(500);
            MouseFocus(driver, By.CssSelector(".product-gift-image"));
            Assert.AreEqual("TestProduct21", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text);
            Assert.AreEqual("TestProduct22", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[1].Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[2].Text);
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[3].Text);

            GoToAdmin("product/edit/3#?tabsInProduct=3");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);
            Functions.DelElement(driver);
            GoToClient("products/test-product3");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 0);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 0);
        }
        
        //[Test]
        //public void AddGiftByFilter()
        //{
        //    GoToAdmin("product/edit/4#?tabsInProduct=3");
        //     ScrollTo(By.XPath("//h2[contains(text(), 'Списки')]"));

        //    Functions.AddProductToListByFilter(driver, linkText: "Подарки", filter: "ProductArtNo", item: "12", tabIndex: 2);
        //    Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct12"));
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 1);

        //    Functions.AddProductToListByFilter(driver, linkText: "Подарки", filter: "ProductArtNo", item: "13", tabIndex: 2);
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct13"));
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 2);

        //    Functions.AddProductToListByFilter(driver, linkText: "Подарки", filter: "ProductArtNo", item: "14", tabIndex: 2);
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct14"));
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 3);

        //    Functions.AddProductToListByFilter(driver, linkText: "Подарки", filter: "ProductArtNo", item: "15", tabIndex: 2);
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct15"));
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

        //    GoToClient("products/test-product4");
        //    Thread.Sleep(2000);
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1);
        //    Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 4);

        //    driver.FindElement(By.CssSelector(".product-gift-image")).Click();
        //    Thread.Sleep(2000);
        //    Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text);
        //    Assert.AreEqual("TestProduct13", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[1].Text);
        //    Assert.AreEqual("TestProduct14", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[2].Text);
        //    Assert.AreEqual("TestProduct15", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[3].Text);

        //    GoToAdmin("product/edit/4#?tabsInProduct=3");
        //    Functions.DelElement(driver);
        //}
        
        [Test]
        public void AddGiftOnPage()
        {
            GoToAdmin("product/edit/5#?tabsInProduct=3");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
           GetGridCell(-1, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct10"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct11"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct12"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 10);

            GoToClient("products/test-product5");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 10);

            MouseFocus(driver, By.CssSelector(".product-gift-image"));
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text);
            Assert.AreEqual("TestProduct10", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[1].Text);
            Assert.AreEqual("TestProduct11", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[2].Text);
            Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[3].Text);
          /*  GoToAdmin("product/edit/5#?tabsInProduct=3");
            Functions.DelElement(driver);*/
        }
        [Test]
        public void AddGiftzAll()
        {
            GoToAdmin("product/edit/6#?tabsInProduct=3");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(2000);
            GetGridCell(-1, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct28"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct29"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct30"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct31"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product6");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 4);

            MouseFocus(driver, By.CssSelector(".product-gift-image"));
            Assert.AreEqual("TestProduct28", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text);
            Assert.AreEqual("TestProduct29", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[1].Text);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[2].Text);
            Assert.AreEqual("TestProduct31", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[3].Text);

          /*  GoToAdmin("product/edit/6#?tabsInProduct=3");
            Functions.DelElement(driver);*/
        }
        [Test]
        public void AddGiftByPageDel()
        {
            GoToAdmin("product/edit/7#?tabsInProduct=3");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e-grid-cell=\"gridOffersSelectvizr[2][\'Name\']\"]"));
            GetGridCell(3, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(4, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(5, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(6, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(7, "selectionRowHeaderCol", "OffersSelectvizr").Click();
           GetGridCell(8, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct21"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct22"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct24"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 6);
            driver.FindElement(By.CssSelector(".pull-right a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(4000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct22"));
            driver.FindElement(By.CssSelector(".pull-right a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct24"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct25"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct26"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product7");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 4);

            MouseFocus(driver, By.CssSelector(".product-gift-image"));
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text);
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[1].Text);
            Assert.AreEqual("TestProduct25", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[2].Text);
            Assert.AreEqual("TestProduct26", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[3].Text);
/*
            GoToAdmin("product/edit/7#?tabsInProduct=3");
            Functions.DelElement(driver);*/
          

        }
    }
}
