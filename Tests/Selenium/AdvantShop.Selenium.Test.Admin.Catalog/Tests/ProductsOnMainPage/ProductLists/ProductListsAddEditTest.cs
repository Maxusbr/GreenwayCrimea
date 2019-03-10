using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class ProductListsAddEditTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Product_ProductList.csv"
           );
             
            Init();
        }
        
        [Test]
        public void ProductListAddActive()
        {
            GoToAdmin("productlists");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Добавление списка", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).SendKeys("NewProductList");
            driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).SendKeys("5");
            SetCkText("Product List Description Test", "editor1");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check admin grid
            GetGridFilter().SendKeys("NewProductList");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("NewProductList", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled", "ProductLists").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.AreEqual("5", GetGridCell(0, "SortOrder", "ProductLists").Text);

            //check details
            GetGridCell(0, "_serviceColumn", "ProductLists").FindElement(By.TagName("ui-modal-trigger")).Click();
            WaitForElem(By.CssSelector(".modal-body"));
            Assert.IsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Редактирование списка"));
            Assert.AreEqual("NewProductList", driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).GetAttribute("value"));
            Assert.AreEqual("5", driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"SwitchOnOffProductList\"]")).FindElement(By.TagName("input")).Selected);
            AssertCkText("Product List Description Test", "editor1");
        }

        [Test]
        public void ProductListEdit()
        {
            GoToAdmin("productlists");
            GetGridFilter().SendKeys("ProductList22");
            DropFocus("h2");
            WaitForAjax();
            GetGridCell(0, "_serviceColumn", "ProductLists").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).SendKeys("ChangedName");
            driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).SendKeys("20");
            SetCkText("Product List Description Edited", "editor1");
            driver.FindElement(By.CssSelector("[data-e2e=\"SwitchOnOffProductList\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("productlists");

            GetGridFilter().SendKeys("ProductList22");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check admin grid
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("ChangedName");
            DropFocus("h2");
            WaitForAjax();

            Assert.AreEqual("ChangedName", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.IsFalse(GetGridCell(0, "Enabled", "ProductLists").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);
            Assert.AreEqual("20", GetGridCell(0, "SortOrder", "ProductLists").Text);

            //check details
            GetGridCell(0, "_serviceColumn", "ProductLists").FindElement(By.TagName("ui-modal-trigger")).Click();
            WaitForElem(By.CssSelector(".modal-body"));
            Assert.AreEqual("ChangedName", driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).GetAttribute("value"));
            Assert.AreEqual("20", driver.FindElement(By.CssSelector("[data-e2e=\"SortProductList\"]")).GetAttribute("value"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"SwitchOnOffProductList\"]")).FindElement(By.TagName("input")).Selected);
            AssertCkText("Product List Description Edited", "editor1");
        }
        
        [Test]
        public void ProductListAddNotActive()
        {
            GoToAdmin("productlists");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).SendKeys("NewProductListNotActive");
            driver.FindElement(By.CssSelector("[data-e2e=\"SwitchOnOffProductList\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check admin grid
            GetGridFilter().SendKeys("NewProductListNotActive");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("NewProductListNotActive", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.IsFalse(GetGridCell(0, "Enabled", "ProductLists").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected);

            //check details
            GetGridCell(0, "_serviceColumn", "ProductLists").FindElement(By.TagName("ui-modal-trigger")).Click();
            WaitForElem(By.CssSelector(".modal-body"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"SwitchOnOffProductList\"]")).FindElement(By.TagName("input")).Selected);
        }


        [Test]
        public void ProductListAddCancel()
        {
            GoToAdmin("productlists");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"NameProductList\"]")).SendKeys("NewProductListCancel");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Отмена')]")).Click();
            Thread.Sleep(2000);

            Refresh();

            GetGridFilter().SendKeys("NewProductListCancel");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}