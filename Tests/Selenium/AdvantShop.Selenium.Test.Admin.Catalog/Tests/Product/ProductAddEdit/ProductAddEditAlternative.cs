using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Alternative
{
    [TestFixture]
    public class ProductAddEditAlternative : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Category.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Brand.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Property.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.PropertyValue.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.ProductPropertyValue.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Product.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.Offer.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Lists\\Catalog.PropertyGroup.csv"
                );
            Init();
            //Functions.RecalculateProducts(driver, baseURL);
            //Functions.RecalculateSearch(driver, baseURL);
        }


        protected void AddAlternativeProduct(string name = null)
        {
            driver.FindElement(By.CssSelector(".header-subtext .btn.btn-sm.btn-action")).Click();
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
        public void AddAlternativeBySearch()
        {
            GoToAdmin("product/edit/2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);

            AddAlternativeProduct("TestProduct12");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct12"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 1);
            
            AddAlternativeProduct("TestProduct13");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct13"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 2);
            
            AddAlternativeProduct("TestProduct14");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct14"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 3);
            
            AddAlternativeProduct("TestProduct15");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct15"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);
            GoToClient("products/test-product2");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));
           
            //С этим товаром покупают
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
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
                 
        }
        [Test]
        public void AddAlternativeByPage()
        {
            GoToAdmin("product/edit/25");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);


            driver.FindElement(By.CssSelector(".header-subtext .btn.btn-sm.btn-action")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);            
            Assert.AreEqual("TestProduct21", GetGridCell(3, "Name", "ProductsSelectvizr").Text);
            GetGridCell(2, "Name", "ProductsSelectvizr").Click();
            GetGridCell(3, "Name", "ProductsSelectvizr").Click();
            GetGridCell(4, "Name", "ProductsSelectvizr").Click();
            GetGridCell(5, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct21"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct22"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct20"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product25");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 4);

            Assert.AreEqual("TestProduct21", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct22", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct20", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //pre
            driver.FindElement(By.CssSelector(".carousel-nav-prev.icon-left-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct21", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //GoToAdmin("product/edit/25");
            //Functions.DelElement(driver);
        }
        [Test]
        public void AddAlternativeByFilter()
        {
            GoToAdmin("product/edit/4");

            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);
            
            Functions.AddProductToListByFilter(driver, linkText: "С этим товаром покупают", filter: "Name", item: "TestProduct12", gridCell: "Name");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct12"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 1);
            Thread.Sleep(1000);
            Functions.AddProductToListByFilter(driver, linkText: "С этим товаром покупают", filter: "Name", item: "TestProduct13", gridCell: "Name");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct13"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 2);
            Thread.Sleep(1000);
            Functions.AddProductToListByFilter(driver, linkText: "С этим товаром покупают", filter: "Name", item: "TestProduct14", gridCell: "Name");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct14"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 3);
            Thread.Sleep(1000);
            Functions.AddProductToListByFilter(driver, linkText: "С этим товаром покупают", filter: "Name", item: "TestProduct15", gridCell: "Name");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct15"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 4);

            GoToClient("products/test-product4");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
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
            //GoToAdmin("product/edit/4");
            //Functions.DelElement(driver);
        }
        [Test]
        public void AddAlternativeOnPage()
        {
            GoToAdmin("product/edit/5");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);


            driver.FindElement(By.CssSelector(".header-subtext .btn.btn-sm.btn-action")).Click();
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
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
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
            //GoToAdmin("product/edit/5");
            //Functions.DelElement(driver);
        }
        [Test]
        public void AddAlternativeAll()
        {
            GoToAdmin("product/edit/26");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);


            driver.FindElement(By.CssSelector(".header-subtext .btn.btn-sm.btn-action")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 24);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct21"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct22"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct24"));

            GoToClient("products/test-product26");
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 24);

            Assert.AreEqual("TestProduct21", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //next 
            driver.FindElement(By.CssSelector(".carousel-nav-next.icon-right-open-after.cs-l-1-interactive")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct22", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[1].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct24", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            //GoToAdmin("product/edit/31");
            //Functions.DelElement(driver);
        }
        [Test]
        public void AddAlternativeByPageDel()
        {
            GoToAdmin("product/edit/7");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);


            driver.FindElement(By.CssSelector(".header-subtext .btn.btn-sm.btn-action")).Click();
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
            GetGridCell(7, "Name", "ProductsSelectvizr").Click();
            GetGridCell(8, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct21"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct22"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[3].Text.Contains("TestProduct24"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 6);
            driver.FindElement(By.CssSelector(".pull-right a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
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
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));

            //С этим товаром покупают
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
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

            GoToAdmin("product/edit/7");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);

            Functions.DelElement(driver);

            GoToClient("products/test-product7");
            Thread.Sleep(2000);
            Assert.IsFalse(driver.PageSource.Contains("С этим товаром покупают"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 0);
           }

        [Test]
        public void AddAlternativeSimilarGiftzByPage()
        {
            GoToAdmin("product/edit/3");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Списки')]")).Click();
            Thread.Sleep(3000);


            driver.FindElement(By.CssSelector(".header-subtext .btn.btn-sm.btn-action")).Click();
            
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            
            Assert.AreEqual("TestProduct21", GetGridCell(3, "Name", "ProductsSelectvizr").Text);
            GetGridCell(3, "Name", "ProductsSelectvizr").Click();
            GetGridCell(4, "Name", "ProductsSelectvizr").Click();
            GetGridCell(5, "Name", "ProductsSelectvizr").Click();
            
            
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".item-block__name-link")).Text.Contains("TestProduct21"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct22"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[2].Text.Contains("TestProduct23"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 3);

            ScrollTo(By.Id("lists"));
            driver.FindElements(By.CssSelector(".uib-tab.nav-item.ng-tab"))[1].Click();
            
            driver.FindElement(By.CssSelector("related-products[data-type='Alternative'] .btn")).Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "ProductsSelectvizr").Click();
            GetGridCell(1, "Name", "ProductsSelectvizr").Click();
            
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[0].Text.Contains("TestProduct1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[1].Text.Contains("TestProduct10"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 2);

            driver.FindElements(By.CssSelector(".uib-tab.nav-item.ng-tab"))[2].Click();
            
            driver.FindElement(By.CssSelector("product-gifts .btn")).Click();
            Thread.Sleep(2000);
            GetGridCell(2, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link"))[0].Text.Contains("TestProduct11"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".item-block__name-link")).Count == 1);

            GoToClient("products/test-product3");
            

            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-gift-image")).Count == 1);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".col-xs-3.col-p-v.gift-img")).Count == 1);

            driver.FindElement(By.CssSelector(".product-gift-image")).Click();
            MouseFocus(driver, By.CssSelector(".product-gift-image"));
           Assert.AreEqual("TestProduct11", driver.FindElements(By.CssSelector(".col-xs-9.col-p-v.gift-txt a"))[0].Text);

            ScrollTo(By.CssSelector(".tabs.tabs-horizontal.details-tabs"));
            Assert.AreEqual("С этим товаром покупают", driver.FindElement(By.CssSelector(".h2")).Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item")).Count == 5);

            Assert.AreEqual("TestProduct21", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[0].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct23", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[2].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("Похожие товары", driver.FindElements(By.CssSelector(".h2"))[1].Text);
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[3].FindElement(By.CssSelector(".products-view-name-link")).Text);
            Assert.AreEqual("TestProduct10", driver.FindElements(By.CssSelector(".products-view-block.cs-br-1.js-products-view-block.js-carousel-item"))[4].FindElement(By.CssSelector(".products-view-name-link")).Text);

        }
    }
}
