using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.Menu
{
    [TestFixture]
    public class CMSMenuAdd : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\CMS\\Menu\\Catalog.Product.csv",
           "data\\Admin\\CMS\\Menu\\Catalog.Offer.csv",
           "data\\Admin\\CMS\\Menu\\Catalog.Category.csv",
           "data\\Admin\\CMS\\Menu\\Catalog.ProductCategories.csv",
               "data\\Admin\\CMS\\Menu\\Catalog.Brand.csv",
            "data\\Admin\\CMS\\Menu\\CMS.Menu.csv",
            "data\\Admin\\CMS\\Menu\\CMS.StaticBlock.csv",
               "data\\Admin\\CMS\\Menu\\CMS.StaticPage.csv",
                  "data\\Admin\\CMS\\Menu\\Settings.News.csv",
                "data\\Admin\\CMS\\Menu\\Settings.NewsCategory.csv"
               );

            Init();
        }

        [Test]
        public void CMSMenuMainAddNewPage()
        {
            GoToAdmin("menus");

            driver.FindElements(By.XPath("//a[contains(text(), 'Добавить пункт меню')]"))[0].Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("Добавление элемента меню", driver.FindElement(By.TagName("h2")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).SendKeys("MenuItemNewPage");

            Assert.IsTrue(driver.FindElement(By.XPath("//input[@value='-1']")).Selected);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNamePage\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNamePage\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNamePage\"]")).SendKeys("MenuItemNewStatPage");

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Добавить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(5000);

            //check new static page details
            WaitForElem(By.Id("PageName"));
            Assert.IsTrue(driver.Url.Contains("edit"));
            
            Assert.IsTrue(driver.FindElement(By.Id("Enabled")).Selected);
            Assert.IsTrue(driver.FindElement(By.Id("IndexAtSiteMap")).Selected);

            Assert.AreEqual("MenuItemNewStatPage", driver.FindElement(By.Name("PageName")).GetAttribute("value"));
            Assert.AreEqual("menuitemnewstatpage", driver.FindElement(By.Name("UrlPath")).GetAttribute("value"));

            Assert.IsTrue(driver.FindElement(By.Id("DefaultMeta")).Selected);

            //check admin
            GoToAdmin("menus");
            
            Assert.IsTrue(driver.FindElements(By.CssSelector(".ibox-content.category-content.border_none"))[0].Text.Contains("MenuItemNewPage"));

            driver.FindElement(By.XPath("//span[contains(text(), 'MenuItemNewPage')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).GetAttribute("value").Contains("/pages/menuitemnewstatpage"));
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@value='2']")).Selected);
            
            //check client
            GoToClient();
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("MenuItemNewPage"));
            
            Assert.IsFalse(Is404Page("pages/menuitemnewstatpage"));
        }
        
        [Test]
        public void CMSMenuMainAddExistPage()
        {
            GoToAdmin("menus");

            driver.FindElements(By.XPath("//a[contains(text(), 'Добавить пункт меню')]"))[0].Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).SendKeys("MenuItemStatPage");

            driver.FindElement(By.CssSelector("[data-e2e=\"StatPage\"] span")).Click();

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            WaitForElem(By.XPath("//h2[contains(text(), 'Статические страницы')]"));

            Assert.AreEqual("Статические страницы", driver.FindElement(By.TagName("h2")).Text);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Страница для нового элемента меню");
            DropFocus("h2");

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("pages/stat_page_for_menu_new", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).GetAttribute("value"));

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Добавить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            Assert.IsTrue(driver.FindElements(By.CssSelector(".ibox-content.category-content.border_none"))[0].Text.Contains("MenuItemStatPage"));

            driver.FindElement(By.XPath("//span[contains(text(), 'MenuItemStatPage')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.XPath("//h2[contains(text(), 'Редактирование элемента меню')]"));
            Thread.Sleep(2000);
            Assert.AreEqual("pages/stat_page_for_menu_new", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@value='2']")).Selected);

            //check client
            GoToClient();

            

            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("MenuItemStatPage"));

            driver.FindElement(By.LinkText("MenuItemStatPage")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("Страница для нового элемента меню", driver.FindElement(By.TagName("h1")).Text);
        }

        [Test]
        public void CMSMenuMainAddNews()
        {
            GoToAdmin("menus");

            driver.FindElements(By.XPath("//a[contains(text(), 'Добавить пункт меню')]"))[0].Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).SendKeys("MenuItemNews");

            driver.FindElement(By.CssSelector("[data-e2e=\"News\"] span")).Click();

            driver.FindElement(By.XPath("//a[contains(text(), 'Выбрать')]")).Click();
            WaitForElem(By.XPath("//h2[contains(text(), 'Новости')]"));

            Assert.AreEqual("Новости", driver.FindElement(By.TagName("h2")).Text);

            driver.FindElements(By.XPath("//a[contains(text(), 'Выбрать')]"))[0].Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("news/news1", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).GetAttribute("value"));

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Добавить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            Assert.IsTrue(driver.FindElements(By.CssSelector(".ibox-content.category-content.border_none"))[0].Text.Contains("MenuItemNews"));

            driver.FindElement(By.XPath("//span[contains(text(), 'MenuItemNews')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);
            Assert.AreEqual("news/news1", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@value='3']")).Selected);

            //check client
            GoToClient();

            Thread.Sleep(3000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("MenuItemNews"));

            driver.FindElement(By.LinkText("MenuItemNews")).Click();
            Thread.Sleep(4000);
            Assert.AreEqual("Test News 1 title", driver.FindElement(By.TagName("h1")).Text);
        }
        
        [Test]
        public void CMSMenuBottomAddLink()
        {
            GoToAdmin("menus");

            driver.FindElements(By.XPath("//a[contains(text(), 'Добавить пункт меню')]"))[1].Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).SendKeys("MenuItemLink");

            driver.FindElement(By.CssSelector("[data-e2e=\"Link\"] span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).SendKeys("https://www.google.ru/");

            DropFocus("h2");

            Assert.AreEqual("https://www.google.ru/", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).GetAttribute("value"));

            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("4")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Добавить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Добавить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");
            
            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.XPath("//span[contains(text(), 'MenuItemLink')]"));

            Assert.IsTrue(driver.FindElements(By.CssSelector(".ibox-content.category-content.border_none"))[1].Text.Contains("MenuItemLink"));

            driver.FindElement(By.XPath("//span[contains(text(), 'MenuItemLink')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("https://www.google.ru/", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemURL\"]")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@value='5']")).Selected);

            //check client
            GoToClient();
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu"))[0].Text.Contains("MenuItemLink"));

            ScrollTo(By.Name("subscribeEmailField"));
            driver.FindElement(By.LinkText("MenuItemLink")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.Url.Contains("www.google.ru"));
            Assert.IsFalse(Is404Page("https://www.google.ru/"));
        }

    }
}
