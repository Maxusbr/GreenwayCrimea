using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.EditCategory
{
    [TestFixture]
    public class CategoryEditAdditional : BaseSeleniumTest
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
        public void BrandInCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[0].Click();

            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
           
           GoToClient("catalog");

            Assert.IsTrue(driver.PageSource.Contains("Производители"));
            Assert.IsTrue(driver.PageSource.Contains("BrandName3"));
        }
        [Test]
        public void BrandInCategoryNo()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[1].Click();
           
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            
           GoToClient("catalog");

            Assert.IsFalse(driver.PageSource.Contains("Производители"));
            Assert.IsFalse(driver.PageSource.Contains("BrandName3"));
        }
        [Test]
        public void TwoLevelInCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[2].Click();

            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            
           GoToClient("catalog");

            Assert.IsTrue(driver.PageSource.Contains("TestCategory4"));
            Assert.IsTrue(driver.PageSource.Contains("TestCategory6"));
        }
       
        [Test]
        public void TwoLevelInCategoryNo()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElements(By.CssSelector(".adv-radio-label input"))[3].Click();

            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
           
           GoToClient("catalog");

            Assert.IsTrue(driver.PageSource.Contains("TestCategory4"));
            Assert.IsFalse(driver.PageSource.Contains("TestCategory6"));
        }
        [Test]
        public void HiddenCategoryInmenu()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".adv-radio-label"));
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[4].Click();
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
           
           GoToClient("catalog");
            Assert.IsFalse(driver.PageSource.Contains("TestCategory3"));
        }
        [Test]
        public void HiddenCategoryInmenuNo()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector(".adv-radio-label"));
          
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[5].Click();
            
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
           
            GoToClient("catalog");
            Assert.IsTrue(driver.PageSource.Contains("TestCategory3"));
        }
        [Test]
        public void DisplayStyleListCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);         
           
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("DisplayStyle")))).SelectByText("Список");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();

            GoToClient("categories/newcategory");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories-thin")).Count > 0);
        }
        [Test]
        public void DisplayStyleTileCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);
         
           
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("DisplayStyle")))).SelectByText("Плитка");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();

            GoToClient("categories/newcategory");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories-item-slim")).Count > 0);
        }
        [Test]
        public void DisplayStyleNoPreCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"][data-e2e-categories-block-item-edit-id=\"3\"]")).Click();
            Thread.Sleep(2000);         
           
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newcategory");

            (new SelectElement(driver.FindElement(By.Id("DisplayStyle")))).SelectByText("Не показывать");
             ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();

            GoToClient("categories/newcategory");
           Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories-item-slim")).Count == 0);
          Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories-thin")).Count == 0);
        }
    }
}
