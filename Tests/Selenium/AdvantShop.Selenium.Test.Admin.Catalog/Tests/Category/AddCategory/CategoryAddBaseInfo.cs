using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.AddCategory
{
    [TestFixture]
    public class CategoryAddBaseInfo : BaseSeleniumTest
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
        public void AddCategory()
        {
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Категория \"Новая категория\"", driver.FindElement(By.TagName("h1")).Text);
            //имя
            driver.FindElement(By.Id("Name")).SendKeys("New_Category");
            //описание
            SetCkText("New_Category_Description_here", "BriefDescription");
            SetCkText("New_Category_Brief_Description_here", "Description");
            Thread.Sleep(2000);
            //урл
            ScrollTo(By.Name("DefaultMeta"));
            driver.FindElement(By.Id("UrlPath")).Click();
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");
            DropFocus("h1");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //в админке 
            GoToAdmin("catalog");
            Assert.AreEqual("5", driver.FindElement(By.CssSelector("[data-e2e-select=\"CategoryTop\"] [data-e2e-select=\"CategoryTopRightAll\"] [data-e2e-quantity=\"CategoryAllQuantity\"]")).Text);
            Assert.IsTrue(driver.PageSource.Contains("New_Category"));
            //в клиентке
            GoToClient("catalog");
            Assert.IsTrue(driver.PageSource.Contains("New_Category"));
            driver.FindElement(By.CssSelector(".product-categories-header-slim")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("New_Category_Description_here"));
            Assert.IsTrue(driver.Url.Contains("categories/newcategory"));
            Assert.IsTrue(driver.PageSource.Contains("New_Category_Brief_Description_here"));

            Assert.AreEqual("Мой магазин - New_Category", driver.Title);
            Assert.AreEqual("New_Category", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("Мой магазин - New_Category", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("Мой магазин - New_Category", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
            
         
        }
           
        [Test]
        public void CategoryCheckEnabled()
        {
           GoToAdmin("category/add?parentId=0");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Disabled");
            DropFocus("h1");
            driver.FindElement(By.CssSelector(".adv-checkbox-emul")).Click();
           driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory_disabled");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            GoToClient("catalog");
            Assert.IsFalse(driver.PageSource.Contains("New_Category_Disabled"));
            Assert.IsTrue(Is404Page("categories/newcategory_disabled"));
        }
        
        [Test]
        public void SaveParent()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Save_Parent1");
             driver.FindElement(By.ClassName("edit")).Click();
            driver.FindElement(By.Id("2")).Click();
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategorysvp");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

           GoToAdmin("catalog");
            Assert.IsFalse(driver.PageSource.Contains("New_Category_Save_Parent1"));
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItem\"][data-e2e-categories-block-item-id=\"2\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("New_Category_Save_Parent1"));
            GoToClient("catalog");
            driver.FindElements(By.CssSelector(".product-categories-header-slim"))[2].Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("New_Category_Save_Parent1"));
        }
       
    }
}
