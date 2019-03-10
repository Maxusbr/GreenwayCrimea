using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Main
{
    [TestFixture]
    public class ProductAddEditMainTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Tag.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.TagMap.csv",
             "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.ProductPropertyValue.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.PropertyGroup.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Color.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Main\\Catalog.Size.csv"
           );

            Init();
        }

        [Test]
        public void ProductEditName()
        {
            //pre check client
            GoToClient("products/test-product1");

            Assert.AreEqual("TestProduct1", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("TestProduct1"));

            //edit name
            GoToAdmin("product/edit/1");
            
            Assert.AreEqual("TestProduct1", driver.FindElement(By.Name("Name")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Товар \"TestProduct1\""));

            driver.FindElement(By.Name("Name")).Click();
            driver.FindElement(By.Name("Name")).Clear();
            driver.FindElement(By.Name("Name")).SendKeys("Edited name 1");
            XPathContainsText("h2", "Основное");
            ScrollTo(By.Id("header-top"));

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");

            Assert.AreEqual("Edited name 1", driver.FindElement(By.Name("Name")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Edited name 1"));

            //check client
            GoToClient("products/test-product1");

            Assert.AreEqual("Edited name 1", driver.FindElement(By.TagName("h1")).Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("Edited name 1"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".breads")).Text.Contains("TestProduct1"));
        }

        [Test]
        public void ProductEditArtNo()
        {
            //pre check client
            GoToClient("products/test-product4");
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-sku")).Text.Contains("4"));

            //edit name
            GoToAdmin("product/edit/4");

            Assert.AreEqual("4", driver.FindElement(By.Name("ArtNo")).GetAttribute("value"));

            driver.FindElement(By.Name("ArtNo")).Click();
            driver.FindElement(By.Name("ArtNo")).Clear();
            driver.FindElement(By.Name("ArtNo")).SendKeys("11111");
            XPathContainsText("h2", "Основное");
            ScrollTo(By.Id("header-top"));

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("product/edit/4");
            ScrollTo(By.XPath("//h2[contains(text(), 'Цена и наличие')]"));
            GetGridCell(0, "_serviceColumn", "Offers").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/4");

            Assert.AreEqual("11111", driver.FindElement(By.Name("ArtNo")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product4");
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-row.details-sku")).Text.Contains("11111"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".details-row.details-sku")).Text.Contains("4"));
        }

        [Test]
        public void ProductEditBarcode()
        {
            GoToAdmin("product/edit/5");

            Assert.AreEqual("BarCodeTest5", driver.FindElement(By.Name("BarCode")).GetAttribute("value"));

            ScrollTo(By.Name("BarCode"));
            driver.FindElement(By.Name("BarCode")).Click();
            driver.FindElement(By.Name("BarCode")).Clear();
            driver.FindElement(By.Name("BarCode")).SendKeys("55555");
            XPathContainsText("label", "Штрих код");
            ScrollTo(By.Id("header-top"));

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/5");

            Assert.AreEqual("55555", driver.FindElement(By.Name("BarCode")).GetAttribute("value"));
        }

        [Test]
        public void ProductEditBarcodeAdd()
        {
            GoToAdmin("product/edit/101");

            Assert.AreEqual("", driver.FindElement(By.Name("BarCode")).GetAttribute("value"));

            ScrollTo(By.Name("BarCode"));
            driver.FindElement(By.Name("BarCode")).Click();
            driver.FindElement(By.Name("BarCode")).Clear();
            driver.FindElement(By.Name("BarCode")).SendKeys("123 test");
            XPathContainsText("label", "Штрих код");
            ScrollTo(By.Id("header-top"));

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/101");

            Assert.AreEqual("123 test", driver.FindElement(By.Name("BarCode")).GetAttribute("value"));
        }

        [Test]
        public void ProductEditDoEnabled()
        {
            //pre check client
            Assert.IsTrue(Is404Page("products/test-product2"));

            //edit enabled
            GoToAdmin("product/edit/2");
            Thread.Sleep(2000);
            Assert.IsFalse(driver.FindElement(By.Id("Enabled")).Selected);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxEnabledClick\"]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/2");

            Assert.IsTrue(driver.FindElement(By.Id("Enabled")).Selected);

            //check client
            Assert.IsFalse(Is404Page("products/test-product2"));
        }

        [Test]
        public void ProductEditDoDisabled()
        {
            //pre check client
            Assert.IsFalse(Is404Page("products/test-product6"));

            //edit enabled
            GoToAdmin("product/edit/6");
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.Id("Enabled")).Selected);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"CheckBoxEnabledClick\"]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/6");

            Assert.IsFalse(driver.FindElement(By.Id("Enabled")).Selected);

            //check client
            Assert.IsTrue(Is404Page("products/test-product6"));
        }
    }
}