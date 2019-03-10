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
    public class CategoryEditBaseInfo : BaseSeleniumTest
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
        public void ChangeNameCategory()
        {
            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItemEdit\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Категория \"TestCategory1\"", driver.FindElement(By.TagName("h1")).Text);
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Save_Name");
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(1000);
            //Проверка
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("New_Category_Save_Name"));

             GoToAdmin("catalog");
            Assert.AreEqual("New_Category_Save_Name", driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItem\"][data-e2e-categories-block-item-id=\"1\"]")).Text);
            GoToClient();
             Assert.AreEqual("New_Category_Save_Name", driver.FindElement(By.CssSelector(".menu-dropdown-link-text.text-floating")).Text);
        }

        [Test]
        public void ChangerURLCategory()
        {
           GoToAdmin("category/edit/2");
           driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New_Category");
            DropFocus("h1");
            driver.FindElement(By.Name("UrlPath")).Clear();
            driver.FindElement(By.Name("UrlPath")).SendKeys("newurl");
           ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            Refresh();
            String Actualtext = driver.FindElement(By.CssSelector("[data-e2e=\"brandLinkLook\"]")).GetAttribute("href");
            Assert.IsTrue(Actualtext.Contains("/categories/newurl"));
            GoToClient();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("New_Category")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.Url.Contains("/categories/newurl"));

        }
        [Test]
        public void ChangeDescription()
        {
           GoToAdmin("category/edit/3");

            SetCkText("New_Description_here", "BriefDescription");
            
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("ChangeDescription");
           
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            GoToClient();
            driver.FindElement(By.LinkText("ChangeDescription")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("New_Description_here"));
        }

        [Test]
        public void ChangeBriefDescription()
        {
           GoToAdmin("category/edit/7");
            SetCkText("New_Brief_Description_here", "Description");
           
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("ChangeBriefDescription");
            
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            GoToClient();
            driver.FindElement(By.LinkText("ChangeBriefDescription")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.TagName("footer"));
            Assert.IsTrue(driver.PageSource.Contains("New_Brief_Description_here"));
        }
        [Test]
        public void ChangeEnabledCategory()
        {
            GoToAdmin("category/edit/8");
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("ChangeEnabledCategory");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".adv-checkbox-emul")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

           GoToClient("catalog");
            Assert.IsFalse(driver.PageSource.Contains("ChangeEnabledCategory"));

            GoToAdmin("category/edit/8");
            driver.FindElement(By.CssSelector(".adv-checkbox-emul")).Click();
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            
           GoToClient("catalog");
            Assert.IsTrue(driver.PageSource.Contains("ChangeEnabledCategory"));
        }
        [Test]
        public void ChangeSaveParent()
        {
            GoToAdmin("category/edit/9");
            driver.FindElement(By.ClassName("edit")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("11")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

           GoToAdmin("catalog");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"categoriesBlockItem\"][data-e2e-categories-block-item-id=\"9\"]")).Count == 0);
            driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItem\"][data-e2e-categories-block-item-id=\"11\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"categoriesBlockItem\"][data-e2e-categories-block-item-id=\"9\"]")).Displayed);
            GoToClient();
            driver.FindElement(By.LinkText("TestCategory11")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("TestCategory9"));
        }
        [Test]
        public void LinkLook()
        {
            GoToAdmin("category/edit/10");
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("LinkLook");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("newnew");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);
            String Actualtext = driver.FindElement(By.CssSelector("[data-e2e=\"brandLinkLook\"]")).GetAttribute("href");
            Assert.IsTrue(Actualtext.Contains("/categories/newnew"));
            driver.FindElement(By.CssSelector("[data-e2e=\"brandLinkLook\"]")).Click();
            Thread.Sleep(4000);

            Functions.OpenNewTab(driver, baseURL);

            Assert.AreEqual("LinkLook", driver.FindElement(By.TagName("h1")).Text);

            Functions.CloseTab(driver, baseURL);
        }
    }
}
