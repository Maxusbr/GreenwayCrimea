using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace AdvantShop.SeleniumTest.Admin.CMS.Menu
{
    [TestFixture]
    public class CMSMenuEdit : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\Menu\\CMS.Menu.csv",
            "data\\Admin\\CMS\\Menu\\CMS.StaticBlock.csv",
               "data\\Admin\\CMS\\Menu\\CMS.StaticPage.csv"
           );

            Init();
        }
        

        [Test]
        public void CMSMenuMainEdit()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("1")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("Редактирование элемента меню", driver.FindElement(By.TagName("h2")).Text);

            Assert.AreEqual("Main Menu1", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).GetAttribute("value"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlankSelected\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNoFollowSelected\"]")).Selected);
            Assert.IsFalse(driver.PageSource.Contains("Удалить изображение"));

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).SendKeys("Changed Name1 Новое название");

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlank\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNoFollow\"]")).Click();

            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("icon.jpg"));

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("1")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("Changed Name1 Новое название", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlankSelected\"]")).Selected);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNoFollowSelected\"]")).Selected);
            Assert.IsTrue(driver.PageSource.Contains("Удалить изображение"));

            //check client
            GoToClient();
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Changed Name1 Новое название"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".row.rel.big-z.menu-block")).Text.Contains("Main Menu1"));
            Assert.IsFalse(Is404Page("pages/Main_Menu1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-general-root-link.cs-l-4"))[0].FindElement(By.TagName("img")).Displayed);

            driver.FindElement(By.LinkText("Changed Name1 Новое название")).Click();
            Thread.Sleep(2000);

            Functions.OpenNewTab(driver, baseURL);
            Assert.IsTrue(driver.WindowHandles.Count.Equals(2));

            Functions.CloseTab(driver, baseURL);
        }


        [Test]
        public void CMSMenuMainEditDeleteImg()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("1")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("icon.jpg"));

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);
            
            //delete img
            GoToAdmin("menus");

            driver.FindElement(By.Id("1")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.PageSource.Contains("Удалить изображение"));

            ScrollTo(By.CssSelector("[data-e2e=\"MenuItemShowMode\"]"));
            driver.FindElement(By.LinkText("Удалить изображение")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("1")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsFalse(driver.PageSource.Contains("Удалить изображение"));

            //check client
            GoToClient();
            Assert.IsFalse(driver.FindElements(By.CssSelector(".menu-general-root-link.cs-l-4"))[0].FindElements(By.TagName("img")).Count > 0);
        }

        [Test]
        public void CMSMenuBottomEditDeleteImg()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            driver.FindElement(By.Id("6")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("icon.jpg"));
            Thread.Sleep(9000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //delete img
            GoToAdmin("menus");

            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            driver.FindElement(By.Id("6")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.PageSource.Contains("Удалить изображение"));

            ScrollTo(By.CssSelector("[data-e2e=\"MenuItemShowMode\"]"));
            driver.FindElement(By.LinkText("Удалить изображение")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            driver.FindElement(By.Id("6")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsFalse(driver.PageSource.Contains("Удалить изображение"));

            //check client
            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu"))[1].FindElements(By.CssSelector(".footer-menu-icon-block")).Count > 0);
        }

        [Test]
        public void CMSMenuMainEditParent()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("14")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemParentName\"]")).Text.Contains("Корневой элемент"));

            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Родительский пункт меню')]"));
            Assert.AreEqual("Родительский пункт меню", driver.FindElement(By.CssSelector("h2")).Text);
            Assert.IsFalse(driver.FindElement(By.CssSelector(".jstree-node.jstree-last.jstree-open")).Text.Contains("Main Menu6"));

            //WaitForElemEnabled(By.XPath("//span[contains(text(), 'Main Menu1')]"));
            if (!driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.Id("1")).Enabled)
            {
                Thread.Sleep(3000);
            }
            driver.FindElement(By.Id("1")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//button[contains(text(), 'Изменить')]"));
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("1")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("14"));
            driver.FindElement(By.Id("14")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemParentName\"]")).Text.Contains("Корневой элемент"));

            //check client
            GoToClient();
            
            Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-general-sub-column")).Count == 2);
        }


        [Test]
        public void CMSMenuBottomEditParent()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("15")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemParentName\"]")).Text.Contains("Корневой элемент"));

            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Родительский пункт меню')]"));
            WaitForElem(By.XPath("//span[contains(text(), 'Bottom Menu1')]"));
            WaitForElemEnabled(By.XPath("//span[contains(text(), 'Bottom Menu1')]"));
            driver.FindElement(By.Id("4")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.XPath("//button[contains(text(), 'Изменить')]"));
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);
         
            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("15"));
            driver.FindElement(By.Id("15")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemParentName\"]")).Text.Contains("Корневой элемент"));

            //check client
            GoToClient();
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Bottom Menu5"));

            ScrollTo(By.Name("subscribeEmailField"));
            driver.FindElement(By.LinkText("Bottom Menu5")).Click();
            Thread.Sleep(3000);

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Пункт нижнего меню с родителем"));
        }


        [Test]
        public void CMSMenuBottomEditParentNull()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("16"));
            driver.FindElement(By.Id("16")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemParentName\"]")).Text.Contains("Корневой элемент"));

            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.XPath("//span[contains(text(), 'Корень')]"));
            WaitForElemEnabled(By.XPath("//span[contains(text(), 'Корень')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'Корень')]")).Click();
            Thread.Sleep(2000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(500);

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(3000);

            //check admin
            GoToAdmin("menus");
            
            driver.FindElement(By.Id("16")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemParentName\"]")).Text.Contains("Корневой элемент"));

            //check client
            GoToClient();
            
            var elem = driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu")).Count - 1;
            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu"))[elem].Text.Contains("Bottom Menu6_Parent"));
        }

        [Test]
        public void CMSMenuBottomEdit()
        {
            GoToAdmin("menus");
            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            driver.FindElement(By.Id("6")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("Bottom Menu3_Blank", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlankSelected\"]")).Selected);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNoFollowSelected\"]")).Selected);
            //Assert.IsFalse(driver.PageSource.Contains("Удалить изображение"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".modal-body")).FindElements(By.TagName("img")).Count > 0);

            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(GetPicturePath("icon.jpg"));

           // driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).SendKeys("Changed Bottom 123");

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlank\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNoFollow\"]")).Click();

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("5")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            WaitForElem(By.Id("6"));
            driver.FindElement(By.Id("6")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);
            Assert.AreEqual("Changed Bottom 123", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).GetAttribute("value"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemBlankSelected\"]")).Selected);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemNoFollowSelected\"]")).Selected);
            // Assert.IsTrue(driver.PageSource.Contains("Удалить изображение"));
            string img = driver.FindElement(By.CssSelector(".modal-body")).FindElement(By.TagName("img")).GetAttribute("src");
            Assert.IsTrue(img.Contains("jpg"));


            //check client
            GoToClient();

            

            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Changed Bottom 123"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".site-footer")).Text.Contains("Bottom Menu3_Blank"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".site-footer")).FindElements(By.CssSelector(".col-xs.footer-menu"))[1].FindElement(By.CssSelector(".footer-menu-icon-block")).Displayed);
            Assert.IsFalse(Is404Page("pages/Bottom_Menu3_Blank"));

            ScrollTo(By.Name("subscribeEmailField"));
            driver.FindElement(By.LinkText("Changed Bottom 123")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("Пункт 3 Нижнего меню", driver.FindElement(By.TagName("h1")).Text);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            Assert.IsTrue(driver.WindowHandles.Count.Equals(1));
        }

        [Test]
        public void CMSMenuMobileEdit()
        {
            GoToAdmin("menus");

            driver.FindElement(By.Id("7")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("Mobile Menu1", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).GetAttribute("value"));

            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).SendKeys("Changed Mobile1 Новое название");

            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Сохранить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("menus");

            driver.FindElement(By.Id("7")).Click();
            driver.FindElement(By.CssSelector(".jstree-anchor.jstree-clicked")).FindElement(By.CssSelector(".link-invert.fa.fa-pencil.link-decoration-none.menu-item-action-item")).Click();
            WaitForElem(By.CssSelector(".modal-header-title"));
            Thread.Sleep(2000);

            Assert.AreEqual("Changed Mobile1 Новое название", driver.FindElement(By.CssSelector("[data-e2e=\"MenuItemName\"]")).GetAttribute("value"));
            
            Functions.AdminMobileOn(driver, baseURL);

            //check client
            GoToClient("?forcedmobile=true");

            driver.FindElement(By.CssSelector(".toggle-sidebar-icon.icon-menu-before.icon-margin-drop.cs-t-8")).Click();
            Assert.IsTrue(driver.FindElement(By.CssSelector(".menu")).FindElements(By.CssSelector(".left.cs-br-4.cs-bg-14.inked.ink-light"))[0].Text.Contains("Changed Mobile1 Новое название"));

            driver.FindElement(By.CssSelector(".menu")).FindElements(By.CssSelector(".left.cs-br-4.cs-bg-14.inked.ink-light"))[0].Click();
            Thread.Sleep(3000);

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Пункт 1 Мобильного меню"));

            Functions.AdminMobileOff(driver, baseURL);
        }
    }
}
