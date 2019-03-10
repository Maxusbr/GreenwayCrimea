using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Admin.Catalog
{
    [TestFixture]
    public class AdminCatalogTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\CatalogTest\\Catalog.Tag.csv",
           "data\\Admin\\Catalog\\CatalogTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\CatalogTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\CatalogTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\CatalogTest\\Catalog.ProductCategories.csv");
             
           Init();
        }

        [Test]
        public void CatalogCategoryOpenAndEdit()
        {
            GoToAdmin("catalog");

            //check correct count
            Assert.AreEqual("6", driver.FindElement(By.CssSelector("[data-e2e-select=\"CategoryTop\"] [data-e2e-select=\"CategoryTopRightAll\"] [data-e2e-quantity=\"CategoryAllQuantity\"]")).Text);

            //check catalog open
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItem\"][data-e2e-categories-block-item-id=\"1\"]")).Text);

            //check catalog go to edit by top
            driver.FindElement(By.CssSelector("[data-e2e=\"EditCategory\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Категория \"Каталог\"", driver.FindElement(By.TagName("h1")).Text);

            //check category open
            GoBack();
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.TagName("h2")).Text);

            //check category go to edit
            GoBack();
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"1\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Категория \"TestCategory1\"", driver.FindElement(By.TagName("h1")).Text);

            //check category go to edit by top
            GoBack();
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItem\"][data-e2e-categories-block-item-id=\"1\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"EditCategory\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Категория \"TestCategory1\"", driver.FindElement(By.TagName("h1")).Text);
        }
        
        [Test]
        public void CategorySelectAndDelete()
        {
            GoToAdmin("catalog");

            //check select item
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemSelect\"][data-e2e-categories-block-item-select-id=\"1\"]")).Click();
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemSelect\"][data-e2e-categories-block-item-select-id=\"1\"] [data-e2e-select=\"CategorySelectTrue\"]")).Selected);

            //check cancel delete item
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemDelete\"][data-e2e-categories-block-item-delete-id=\"2\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual(6, driver.FindElements(By.CssSelector(".categories-block-wrap.as-sortable-item")).Count);

            //check delete item
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemDelete\"][data-e2e-categories-block-item-delete-id=\"2\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual(5, driver.FindElements(By.CssSelector(".categories-block-wrap.as-sortable-item")).Count);

            //check selected items
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemSelect\"][data-e2e-categories-block-item-select-id=\"1\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemSelect\"][data-e2e-categories-block-item-select-id=\"3\"]")).Click();
            Assert.IsTrue(driver.FindElement(By.CssSelector(".btn.btn-sm.btn-default")).Text.Contains("2 категории выбрано"));

            //check delete selected items
            driver.FindElement(By.CssSelector("[data-e2e-select=\"CategorySelectedDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);
            Assert.AreNotEqual("TestCategory1", driver.FindElement(By.CssSelector("[data-e2e-block=\"Category\"]")).Text);
            Assert.AreNotEqual("TestCategory3", driver.FindElement(By.CssSelector("[data-e2e-block=\"Category\"]")).Text);

            //check item delete from edit
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"7\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Категория \"TestCategory7\"", driver.FindElement(By.TagName("h1")).Text);
            driver.FindElement(By.XPath("//a[contains(text(), 'Удалить')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            GoToAdmin("catalog");
            Assert.AreEqual(2, driver.FindElements(By.CssSelector(".categories-block-wrap.as-sortable-item")).Count);

            //check all items selected 
            driver.FindElement(By.CssSelector("[data-e2e-select=\"CategorySelect\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemSelect\"][data-e2e-categories-block-item-select-id=\"5\"] [data-e2e-select=\"CategorySelectTrue\"]")).Selected);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemSelect\"][data-e2e-categories-block-item-select-id=\"6\"] [data-e2e-select=\"CategorySelectTrue\"]")).Selected);

            //check all items selected delete
            driver.FindElement(By.CssSelector("[data-e2e-select=\"CategorySelectedDelete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector("[data-e2e=\"categoriesBlockItem\"]")).Count);

            GoToAdmin("catalog");
            //Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            Assert.AreEqual(0, driver.FindElements(By.CssSelector("[data-e2e=\"categoriesBlockItem\"]")).Count);
        }
    }
}