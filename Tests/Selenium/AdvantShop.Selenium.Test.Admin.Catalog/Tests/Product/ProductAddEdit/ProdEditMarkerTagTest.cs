using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.TagsAndMarkers
{
    [TestFixture]
    public class ProductAddEditMainMarkersTagsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Tag.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.TagMap.csv",
             "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductPropertyValue.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyGroup.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Color.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Size.csv"
           );

            Init();
        }
        /* markers tests */
        [Test]
        public void ProductEditMarkerAdd()
        {
            GoToClient("products/test-product106");
            Assert.IsFalse(driver.PageSource.Contains("Хит"));
            Assert.IsFalse(driver.PageSource.Contains("Новинка"));
            Assert.IsFalse(driver.PageSource.Contains("Рекомендуем"));
            Assert.IsFalse(driver.PageSource.Contains("Распродажа"));

            GoToAdmin("product/edit/106");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxRecomendedClick\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxSalesClick\"]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/106");

            //check admin
            Assert.IsTrue(driver.FindElement(By.Id("Recomended")).Selected);
            Assert.IsTrue(driver.FindElement(By.Id("Sales")).Selected);
            Assert.IsFalse(driver.FindElement(By.Id("BestSeller")).Selected);
            Assert.IsFalse(driver.FindElement(By.Id("New")).Selected);

            //check client product card
            GoToClient("products/test-product106");
            
            Assert.IsTrue(driver.PageSource.Contains("Рекомендуем"));
            Assert.IsTrue(driver.PageSource.Contains("Распродажа"));
            Assert.IsFalse(driver.PageSource.Contains("Хит"));
            Assert.IsFalse(driver.PageSource.Contains("Новинка"));

            //check client category
            GoToClient("categories/test-category7");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Рекомендуем"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Распродажа"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Хит"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Новинка"));

            //check client subcategory
            GoToClient("categories/test-category6");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Рекомендуем"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Распродажа"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Хит"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"106\"]")).Text.Contains("Новинка"));
        }

        [Test]
        public void ProductEditMarkerAddAdmin()
        {
            GoToClient("products/test-product107");
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Хит"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Новинка"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Рекомендуем"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Распродажа"));

            GoToAdmin("product/edit/107");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxBestSellerClick\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxNewClick\"]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/107");

            //check admin product card
            Assert.IsTrue(driver.FindElement(By.Id("BestSeller")).Selected);
            Assert.IsTrue(driver.FindElement(By.Id("New")).Selected);
            Assert.IsFalse(driver.FindElement(By.Id("Recomended")).Selected);
            Assert.IsFalse(driver.FindElement(By.Id("Sales")).Selected);

            //check admin catalog
            GoToAdmin("catalog");
            Assert.AreEqual("1/1", driver.FindElements(By.CssSelector(".aside-menu-inner"))[2].FindElement(By.CssSelector(".aside-menu-count-inner")).Text);
            Assert.AreEqual("1/1", driver.FindElements(By.CssSelector(".aside-menu-inner"))[3].FindElement(By.CssSelector(".aside-menu-count-inner")).Text);

            driver.FindElement(By.XPath("//div[contains(text(), 'Хиты продаж')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct107", GetGridCell(0, "Name").Text);

            driver.FindElement(By.XPath("//div[contains(text(), 'Новинки')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct107", GetGridCell(0, "Name").Text);

            //check client product card
            GoToClient("products/test-product107");
            Assert.IsTrue(driver.PageSource.Contains("Хит"));
            Assert.IsTrue(driver.PageSource.Contains("Новинка"));
            Assert.IsFalse(driver.PageSource.Contains("Рекомендуем"));
            Assert.IsFalse(driver.PageSource.Contains("Распродажа"));

            //check client category
            GoToClient("categories/test-category7");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Хит"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Новинка"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Рекомендуем"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Распродажа"));

            //check client subcategory
            GoToClient("categories/test-category6");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Хит"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Новинка"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Рекомендуем"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"107\"]")).Text.Contains("Распродажа"));
        }

        [Test]
        public void ProductEditMarkerAddAll()
        {
            GoToClient("products/test-product108");
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Хит"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Новинка"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Рекомендуем"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Распродажа"));

            GoToAdmin("product/edit/108");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxBestSellerClick\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxNewClick\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxRecomendedClick\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxSalesClick\"]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/108");

            //check admin product card
            Assert.IsTrue(driver.FindElement(By.Id("BestSeller")).Selected);
            Assert.IsTrue(driver.FindElement(By.Id("New")).Selected);
            Assert.IsTrue(driver.FindElement(By.Id("Recomended")).Selected);
            Assert.IsTrue(driver.FindElement(By.Id("Sales")).Selected);

            //check admin catalog
            GoToAdmin("catalog");

            driver.FindElement(By.XPath("//div[contains(text(), 'Хиты продаж')]")).Click();
            Thread.Sleep(2000);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct108");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("TestProduct108", GetGridCell(0, "Name").Text);

            driver.FindElement(By.XPath("//div[contains(text(), 'Новинки')]")).Click();
            Thread.Sleep(2000);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct108");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("TestProduct108", GetGridCell(0, "Name").Text);

            //check client product card
            GoToClient("products/test-product108");
            Assert.IsTrue(driver.PageSource.Contains("Хит"));
            Assert.IsTrue(driver.PageSource.Contains("Новинка"));
            Assert.IsTrue(driver.PageSource.Contains("Рекомендуем"));
            Assert.IsTrue(driver.PageSource.Contains("Распродажа"));

            //check client category
            GoToClient("categories/test-category7");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Хит"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Новинка"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Рекомендуем"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Распродажа"));

            //check client subcategory
            GoToClient("categories/test-category6");
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Хит"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Новинка"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Рекомендуем"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-product-id=\"108\"]")).Text.Contains("Распродажа"));
        }

        /* tags tests */
        [Test]
        public void ProductEditTagAddFromSelect()
        {
            GoToAdmin("product/edit/1");

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"SelectTag\"]")).Text.Contains("TagName2"));

            driver.FindElement(By.CssSelector("[data-e2e=\"SelectTag\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TagName3')]")).Click();
            Thread.Sleep(1000);
            DropFocus("h1");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"SelectTag\"]")).Text.Contains("TagName2"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"SelectTag\"]")).Text.Contains("TagName3"));

            //check client
            GoToClient("categories/test-category1");
            Assert.AreEqual("TagName2", driver.FindElement(By.CssSelector(".tags")).FindElements(By.TagName("a"))[1].Text);
            Assert.AreEqual("TagName3", driver.FindElement(By.CssSelector(".tags")).FindElements(By.TagName("a"))[2].Text);

            Assert.IsTrue(driver.PageSource.Contains("TestProduct1"));
            Assert.AreEqual("18", driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text);

            driver.FindElement(By.LinkText("TagName3")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.PageSource.Contains("TagName3"));
            Assert.IsTrue(driver.PageSource.Contains("TestProduct1"));

            Assert.AreEqual("1", driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text);
        }
        
        [Test]
        public void ProductEditTagDelete()
        {
            GoToAdmin("product/edit/6");

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"SelectTag\"]")).Text.Contains("TagName1"));
            
            //check client
            GoToClient("categories/test-category1");
            Assert.AreEqual("TagName1", driver.FindElement(By.CssSelector(".tags")).FindElements(By.TagName("a"))[0].Text);
            
            Assert.IsTrue(driver.PageSource.Contains("TestProduct6"));
            Assert.AreEqual("18", driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text);

            driver.FindElement(By.LinkText("TagName1")).Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.PageSource.Contains("TagName1"));
            Assert.IsTrue(driver.PageSource.Contains("TestProduct6"));

            Assert.AreEqual("1", driver.FindElement(By.CssSelector(".products-view-sort-result-number")).Text);

            //check tag delete
            GoToAdmin("product/edit/6");

            driver.FindElement(By.CssSelector(".close.ui-select-match-close")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check client
            GoToClient("categories/test-category1/tag/tag-name1");
            
            Assert.IsFalse(driver.PageSource.Contains("TestProduct6"));
        }
    }
}