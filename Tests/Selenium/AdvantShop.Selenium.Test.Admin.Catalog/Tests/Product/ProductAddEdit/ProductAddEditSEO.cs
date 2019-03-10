using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.SEO
{
    [TestFixture]
    public class ProductAddEditSEO : BaseSeleniumTest
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
             "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductPropertyValue.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyGroup.csv",
                "Data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Color.csv",
                "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Size.csv"
           );

            Init();
        }

        [Test]
        public void ProductEditCheckMeta()
        {
            GoToAdmin("product/edit/1");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            if (driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }
          
            driver.FindElement(By.Id("SeoTitle")).SendKeys("New_Product_Title");
            driver.FindElement(By.Id("SeoH1")).SendKeys("New_Product_H1");
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("New_Product_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).SendKeys("New_Product_SeoDescription");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(2000);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected);
            Assert.AreEqual("New_Product_Title", driver.FindElement(By.Id("SeoTitle")).GetAttribute("value"));
            Assert.AreEqual("New_Product_H1", driver.FindElement(By.Id("SeoH1")).GetAttribute("value"));
            Assert.AreEqual("New_Product_SeoKeywords", driver.FindElement(By.Id("SeoKeywords")).GetAttribute("value"));
            Assert.AreEqual("New_Product_SeoDescription", driver.FindElement(By.Id("SeoDescription")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product1");
            Assert.AreEqual("New_Product_Title", driver.Title);
            Assert.AreEqual("New_Product_H1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("New_Product_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("New_Product_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

        }
        [Test]
        public void ProductEditCheckMetaParams()
        {
            GoToAdmin("product/edit/1");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            if (driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("#STORE_NAME#");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("#PRODUCT_NAME#");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("#CATEGORY_NAME#");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("#CATEGORY_NAME# #PRODUCT_NAME#");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(2000);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected);
            Assert.AreEqual("#STORE_NAME#", driver.FindElement(By.Id("SeoTitle")).GetAttribute("value"));
            Assert.AreEqual("#PRODUCT_NAME#", driver.FindElement(By.Id("SeoH1")).GetAttribute("value"));
            Assert.AreEqual("#CATEGORY_NAME#", driver.FindElement(By.Id("SeoKeywords")).GetAttribute("value"));
            Assert.AreEqual("#CATEGORY_NAME# #PRODUCT_NAME#", driver.FindElement(By.Id("SeoDescription")).GetAttribute("value"));

            //check client
            GoToClient("products/test-product1");
            Assert.AreEqual("Мой магазин", driver.FindElement(By.CssSelector("[property=\"og:title\"]")).GetAttribute("content"));
            Assert.AreEqual("TestProduct1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("TestCategory1 TestProduct1", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

        }       
        [Test]
        public void ProductEditMetaURL()
        {
            GoToAdmin("product/edit/1");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new_path");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("product/edit/1");
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected);
            Assert.AreEqual("new_path", driver.FindElement(By.Id("UrlPath")).GetAttribute("value"));

            Assert.IsTrue(Is404Page("products/test-product1"));

            GoToClient("products/new_path");
            Assert.AreEqual("Мой магазин - TestProduct1", driver.Title);
            Assert.AreEqual("TestProduct1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Мой магазин - TestProduct1", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
            Assert.AreEqual("Мой магазин - TestProduct1", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));

            GoToAdmin("product/edit/1");
            ScrollTo(By.Id("UrlPath"));
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("test-product1");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
        }

        [Test]
        public void ProductEditMetazInstruction()
        {
            GoToAdmin("product/edit/2");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'SEO')]")).Click();
            Thread.Sleep(3000);
            
            if (driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"productDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }
            Thread.Sleep(2000);

            ScrollTo(By.Id("SeoDescription"));

            XPathContainsText("a", "Инструкция. Настройка мета по умолчанию для магазина");
            Thread.Sleep(4000);
            
            Functions.OpenNewTab(driver, baseURL);
            Assert.IsTrue(driver.Url.Contains("help") && driver.Url.Contains("seo-module"));
            Functions.CloseTab(driver, baseURL);
        }

    }
}
