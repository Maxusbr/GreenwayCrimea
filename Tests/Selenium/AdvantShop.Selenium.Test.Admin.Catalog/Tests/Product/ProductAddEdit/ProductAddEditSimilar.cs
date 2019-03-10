using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Similar
{
    [TestFixture]
    public class ProductAddEditSimilar : BaseSeleniumTest
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

        protected void AddSimilarProduct(string name = null)
        {
            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(2000);
            GetGridFilter().SendKeys(name);
            DropFocus("h2");
            Assert.AreEqual(name, GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            GetGridCell(0, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
        }

        [Test]
        public void AddSimilarBySearch()
        {
            GoToAdmin("product/edit/2#?tabsInProduct=2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);
          
            AddSimilarProduct("TestProduct12");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct12"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 1);

            AddSimilarProduct("TestProduct13");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct13"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 2);

            AddSimilarProduct("TestProduct14");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct14"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 3);

            AddSimilarProduct("TestProduct15");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct15"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product2");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //Похожие товары
            Assert.AreEqual("Похожие товары", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 4);

            Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct14", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct13", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct15", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //pre
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct14", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
           /* GoToAdmin("product/edit/2#?tabsInProduct=2");
            Functions.DelElement(driver);*/
        }
        [Test]
        public void AddSimilarByPage()
        {
            GoToAdmin("product/edit/3#?tabsInProduct=2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct21", GetGridCell(3, "Name", "ProductsSelectvizr").Text);
            ScrollTo(By.CssSelector("[data-e2e-grid-cell=\"gridProductsSelectvizr[2][\'Name\']\"]"));
            GetGridCell(3, "Name", "ProductsSelectvizr").Click();
            GetGridCell(4, "Name", "ProductsSelectvizr").Click();
            GetGridCell(5, "Name", "ProductsSelectvizr").Click();
            GetGridCell(6, "Name", "ProductsSelectvizr").Click();
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
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("Похожие товары", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 4);

            Assert.AreEqual("TestProduct21", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct22", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //pre
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct21", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
         /*   GoToAdmin("product/edit/3#?tabsInProduct=2");
            Functions.DelElement(driver);*/
         }
        [Test]
        public void AddSimilarByFilter()
        {
            GoToAdmin("product/edit/4#?tabsInProduct=2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);

            Functions.AddProductToListByFilter(driver, linkText: "Похожие товары", filter: "ProductArtNo", item: "12", tabIndex: 0, gridCell: "ProductArtNo");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct12"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 1);

            Functions.AddProductToListByFilter(driver, linkText: "Похожие товары", filter: "ProductArtNo", item: "13", tabIndex: 0, gridCell: "ProductArtNo");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct13"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 2);

            Functions.AddProductToListByFilter(driver, linkText: "Похожие товары", filter: "ProductArtNo", item: "14", tabIndex: 0, gridCell: "ProductArtNo");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct14"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 3);

           
            GoToClient("products/test-product4");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("Похожие товары", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 3);

            Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct14", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
/*
            GoToAdmin("product/edit/4#?tabsInProduct=2");
            Functions.DelElement(driver);*/

        }
        [Test]
        public void AddSimilarOnPage()
        {
            GoToAdmin("product/edit/5#?tabsInProduct=2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct10"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct11"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct12"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 10);

            GoToClient("products/test-product5");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("Похожие товары", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 10);

            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct11", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct10", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct11", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct13", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[4].FindElement(By.CssSelector(".products-view-name-link")).Text);
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct14", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[5].FindElement(By.CssSelector(".products-view-name-link")).Text);
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct13", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[4].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct15", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[6].FindElement(By.CssSelector(".products-view-name-link")).Text);
         /*   GoToAdmin("product/edit/5#?tabsInProduct=2");
            Functions.DelElement(driver);*/

        }
        [Test]
        public void AddSimilarzAll()
        {
            GoToAdmin("product/edit/6#?tabsInProduct=2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(2000);
            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("28 - TestProduct28"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("29 - TestProduct29"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("30 - TestProduct30"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("31 - TestProduct31"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product6");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //Похожие товары
            Assert.AreEqual("Похожие товары", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 4);

            Assert.AreEqual("TestProduct28", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct30", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct29", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct31", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
           /* driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[4].FindElement(By.CssSelector(".products-view-name-link")).Text);
          /*  driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct12", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct14", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[5].FindElement(By.CssSelector(".products-view-name-link")).Text);
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct13", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[4].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct15", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[6].FindElement(By.CssSelector(".products-view-name-link")).Text);
            GoToAdmin("product/edit/6#?tabsInProduct=2");
            Functions.DelElement(driver);*/
        }
        [Test]
        public void AddSimilarByPageDel()
        {
            GoToAdmin("product/edit/7#?tabsInProduct=2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);

            driver.FindElements(By.CssSelector(".header-subtext .btn.btn-sm.btn-action"))[0].Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct21", GetGridCell(3, "Name", "ProductsSelectvizr").Text);
            ScrollTo(By.CssSelector("[data-e2e-grid-cell=\"gridProductsSelectvizr[2][\'Name\']\"]"));
            GetGridCell(3, "Name", "ProductsSelectvizr").Click();
            GetGridCell(5, "Name", "ProductsSelectvizr").Click();
            GetGridCell(6, "Name", "ProductsSelectvizr").Click();
            GetGridCell(7, "Name", "ProductsSelectvizr").Click();
            GetGridCell(8, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct21"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct24"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct25"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 5);
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
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("Похожие товары", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 4);

            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct25", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct26", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //pre
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct25", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            GoToAdmin("product/edit/7#?tabsInProduct=2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);
            Functions.DelElement(driver);

            GoToClient("products/test-product7");
            Thread.Sleep(2000);
            Assert.IsFalse(driver.PageSource.Contains("Похожие товары"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 0);



        }

    }
}
