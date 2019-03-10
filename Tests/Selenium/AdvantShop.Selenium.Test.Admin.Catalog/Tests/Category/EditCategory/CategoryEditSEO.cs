using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.EditCategory
{
    [TestFixture]
    public class CategoryEditSEO : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
               "Data\\Admin\\EditCategory\\Catalog.Category.csv",
               "Data\\Admin\\EditCategory\\Catalog.Brand.csv",
               "Data\\Admin\\EditCategory\\Catalog.Property.csv",
                "Data\\Admin\\EditCategory\\Catalog.PropertyValue.csv",
                "Data\\Admin\\EditCategory\\Catalog.ProductPropertyValue.csv",
               "Data\\Admin\\EditCategory\\Catalog.Product.csv",
               "Data\\Admin\\EditCategory\\Catalog.Offer.csv",
               "Data\\Admin\\EditCategory\\Catalog.ProductCategories.csv",
               "Data\\Admin\\EditCategory\\Catalog.PropertyGroup.csv"
                );

             
            Init();

        }
        
        [Test]
        public void ChangeCheckSEOnew()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);
            var element = driver.FindElements(By.TagName("figure"))[2];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);

            if (driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected);

            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new");
           
            driver.FindElement(By.Id("SeoTitle")).SendKeys("New_Category_Title");
            driver.FindElement(By.Id("SeoH1")).SendKeys("New_Category_H1");
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("New_Category_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).SendKeys("New_Category_SeoDescription");
            
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

             Assert.AreEqual("New_Category_Title", driver.FindElement(By.Id("SeoTitle")).GetAttribute("value"));
            Assert.AreEqual("New_Category_SeoDescription", driver.FindElement(By.Id("SeoDescription")).GetAttribute("value"));
            Assert.AreEqual("New_Category_H1", driver.FindElement(By.Id("SeoH1")).GetAttribute("value"));
            Assert.AreEqual("New_Category_SeoKeywords", driver.FindElement(By.Id("SeoKeywords")).GetAttribute("value"));

            GoToClient("categories/new");
            Assert.AreEqual("New_Category_Title", driver.Title);
            Assert.AreEqual("New_Category_H1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("New_Category_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("New_Category_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }

        [Test]
        public void ChangeCheckzMetaold()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);            
             
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new");
            var element = driver.FindElements(By.TagName("figure"))[2];
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);
   
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected);   
            Assert.IsTrue(driver.FindElements(By.Id("SeoTitle")).Count == 0);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

          GoToClient("categories/new");
            Assert.AreEqual("Мой магазин - TestCategory1", driver.Title);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Мой магазин - TestCategory1", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
            Assert.AreEqual("Мой магазин - TestCategory1", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
        }
    }
}
