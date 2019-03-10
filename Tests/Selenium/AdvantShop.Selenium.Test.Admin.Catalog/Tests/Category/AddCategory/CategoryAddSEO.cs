using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.AddCategory
{
    [TestFixture]
    public class CategoryAddSEO : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Product.csv",
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Offer.csv",
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Category.csv",
                "data\\Admin\\Catalog\\AddCategory\\Catalog.Brand.csv",
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.ProductCategories.csv");

             
            Init();

        }
        
        [Test]
        public void AddNewCategoryCheckSEO()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
           Thread.Sleep(2000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_SEO_Title");
            DropFocus("h1");
            ScrollTo(By.Name("DefaultMeta"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected); 

            Thread.Sleep(3000);
            driver.FindElement(By.Id("UrlPath")).Click();
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("new_seo_title");           
            driver.FindElement(By.Id("SeoTitle")).SendKeys("New_Category_Title");
            driver.FindElement(By.Id("SeoH1")).SendKeys("New_Category_H1");
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("New_Category_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).SendKeys("New_Category_SeoDescription");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            
           GoToClient("categories/new_seo_title");
            Assert.AreEqual("New_Category_Title", driver.Title);
            Assert.AreEqual("New_Category_H1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("New_Category_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("New_Category_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }

    }
}
